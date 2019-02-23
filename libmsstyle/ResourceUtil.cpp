#include "ResourceUtil.h"
#include "StringUtil.h"
#include "StringTable.h"

#include <string>
#include <vector>

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

namespace libmsstyle
{
    namespace priv
    {
        /**************************************************************************
        * FILE STUFF I SHOULD PUT SOMEWHERE ELSE
        **************************************************************************/

        void RemoveFile(const std::string& path)
        {
            std::wstring wpath = UTF8ToWide(path);
            DeleteFileW(wpath.c_str());
        }

        bool DuplicateFile(const std::string& src, const std::string& dst)
        {
            std::wstring wsrc = UTF8ToWide(src);
            std::wstring wdst = UTF8ToWide(dst);

            // FALSE: don't fail, force overwrite
            return CopyFileW(wsrc.c_str(), wdst.c_str(), FALSE) == TRUE;
        }

        bool FileReadAllBytes(const std::string& src, char** buffer, unsigned int* length)
        {
            std::wstring wsrc = UTF8ToWide(src);
            HANDLE hFile = CreateFileW(wsrc.c_str(), GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
            if (hFile == INVALID_HANDLE_VALUE)
                goto exit;

            LARGE_INTEGER fileSize;
            if (GetFileSizeEx(hFile, &fileSize) == FALSE)
                goto exitCloseFile;

            *length = (unsigned int)fileSize.QuadPart;
            *buffer = (char*)malloc(*length);
            if (*buffer == NULL)
                goto exitCloseFile;

            DWORD bytesRead;
            if (ReadFile(hFile, *buffer, *length, &bytesRead, NULL) == FALSE)
                goto exitFreeBuffer;

            *length = bytesRead;

            CloseHandle(hFile);
            return true;

        exitFreeBuffer:
            free(*buffer);
        exitCloseFile:
            CloseHandle(hFile);
        exit:
            return false;
        }

        void FileFreeBytes(char* bytes)
        {
            free(bytes);
        }


        /**************************************************************************
        * STRING TABLE LOAD AND STORE 
        **************************************************************************/

        const int BUCKET_SIZE = 16;

        int BUCKETED_TO_FLAT(int bucket, int index)
        {
            return (bucket - 1) * BUCKET_SIZE + index;
        }

        void FLAT_TO_BUCKETED(int flatid, int& bucket, int& index)
        {
            bucket = (flatid / BUCKET_SIZE) + 1;
            index = flatid % BUCKET_SIZE;
        }


        char* BucketAppendString(char* dest, const wchar_t* str, int length)
        {
            // append char count (not byte count!) of the string
            WORD cch = (WORD)length;
            memcpy(dest, &cch, sizeof(cch));
            dest += sizeof(cch);

            // append the string data
            if (cch > 0)
            {
                memcpy(dest, str, cch * sizeof(wchar_t));
                dest += cch * sizeof(wchar_t);
            }

            return dest;
        }

        char* BucketAppendEmpty(char* dest, int numEntries)
        {
            memset(dest, 0, numEntries * sizeof(WORD));
            return dest + numEntries * sizeof(WORD);
        }


        BOOL CALLBACK ResourceNameCallbackReadStrings(HMODULE hModule, LPCWSTR lpType, LPWSTR lpName, LONG_PTR lParam)
        {
            StringTable* table = (StringTable*)lParam;

            HRSRC resHandle = FindResourceExW(hModule, RT_STRING, lpName, MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL));
            if (resHandle)
            {
                HGLOBAL dataHandle = LoadResource(hModule, resHandle);
                if (dataHandle)
                {
                    LPCWSTR dataPtr = reinterpret_cast<LPCWSTR>(LockResource(dataHandle));
                    if (dataPtr)
                    {
                        for (int i = 0; i < BUCKET_SIZE; ++i)
                        {
                            int length = (UINT)*dataPtr++;
                            int id = BUCKETED_TO_FLAT((UINT)lpName, i);

                            if (length)
                            {
                                std::wstring wstr(dataPtr, length);
                                table->Set(id, WideToUTF8(wstr));
                            }
                            dataPtr += length;
                        }
                    }
                    FreeResource(dataHandle);
                }
            }

            return TRUE;
        }


        void LoadStringTable(ModuleHandle moduleHandle, StringTable& table)
        {
            HMODULE module = static_cast<HMODULE>(moduleHandle);
            EnumResourceNamesExW(module, RT_STRING, ResourceNameCallbackReadStrings, (LONG_PTR)&table, RESOURCE_ENUM_LN | RESOURCE_ENUM_MUI, MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL));
        }

        struct DeleteStringsCallbackArgs
        {
            StringTable* table;
            HANDLE updateHandle;
            std::vector<int>* bucketsUsed;
        };

        BOOL CALLBACK ResourceNameCallbackDeleteStrings(HMODULE hModule, LPCWSTR lpType, LPWSTR lpName, LONG_PTR lParam)
        {
            DeleteStringsCallbackArgs* args = (DeleteStringsCallbackArgs*)lParam;
            UINT bucket = (UINT)lpName;

            if (std::find(args->bucketsUsed->begin(), args->bucketsUsed->end(), bucket) == args->bucketsUsed->end())
            {
                if (UpdateResourceW(args->updateHandle
                    , RT_STRING
                    , MAKEINTRESOURCEW(bucket)
                    , MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL)
                    , NULL
                    , 0) == FALSE)
                {
                    // should not happend
                }
            }

            return TRUE;
        }

        bool UpdateStringTable(ModuleHandle moduleHandle, UpdateHandle updateHandle, StringTable& table, std::runtime_error& error)
        {
            char bucketBuffer[4096];
            char* bucketBufferPtr = bucketBuffer;

            int previousBucketIndex = -1;
            int currentBucketIndex = 0;

            int currentBucket = 0;
            int previousBucket = (table.begin() != table.end()) ? (table.begin()->first / 16) + 1 : 0;

            std::vector<int> bucketsTouched;

            for (auto& item : table)
            {
                // get current bucket
                int currentBucket;
                int currentBucketIndex;
                FLAT_TO_BUCKETED(item.first, currentBucket, currentBucketIndex);

                // write changes once a bucket is complete
                if (currentBucket != previousBucket)
                {
                    // fill in empty entries at the end if required
                    int toFill = (BUCKET_SIZE - 1) - previousBucketIndex;
                    bucketBufferPtr = BucketAppendEmpty(bucketBufferPtr, toFill);

                    // commit bucket
                    if (UpdateResourceW((HANDLE)updateHandle
                        , RT_STRING
                        , MAKEINTRESOURCEW(previousBucket)
                        , MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL)
                        , bucketBuffer
                        , bucketBufferPtr - bucketBuffer) == FALSE)
                    {
                        char errorMsg[512];
                        sprintf(errorMsg, "Updating string resource with id '%d' failed! GetLastError() returned: %d", previousBucket, GetLastError());
                        error = std::runtime_error(errorMsg);
                        return false;
                    }

                    // remember bucket
                    bucketsTouched.push_back(previousBucket);

                    // reset bucket buffer
                    previousBucket = currentBucket;
                    bucketBufferPtr = bucketBuffer;

                    previousBucketIndex = -1;
                }

                std::wstring wstr = UTF8ToWide(item.second);

                // fill in empty entries as required
                int toFill = currentBucketIndex - previousBucketIndex - 1;
                bucketBufferPtr = BucketAppendEmpty(bucketBufferPtr, toFill);
                bucketBufferPtr = BucketAppendString(bucketBufferPtr, wstr.c_str(), wstr.length());

                previousBucketIndex = currentBucketIndex;
            }

            // fill in empty entries at the end if required
            int toFill = (BUCKET_SIZE - 1) - previousBucketIndex;
            bucketBufferPtr = BucketAppendEmpty(bucketBufferPtr, toFill);

            // commit bucket
            if (UpdateResourceW((HANDLE)updateHandle
                , RT_STRING
                , MAKEINTRESOURCEW(previousBucket)
                , MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL)
                , bucketBuffer
                , bucketBufferPtr - bucketBuffer) == FALSE)
            {
                char errorMsg[512];
                sprintf(errorMsg, "Updating string resource with id '%d' failed! GetLastError() returned: %d", previousBucket, GetLastError());
                error = std::runtime_error(errorMsg);
                return false;
            }

            // remember bucket
            bucketsTouched.push_back(previousBucket);

            DeleteStringsCallbackArgs args;
            args.table = &table;
            args.bucketsUsed = &bucketsTouched;
            args.updateHandle = (HANDLE)updateHandle;

            // remove all buckets we haven't touched
            EnumResourceNamesExW((HMODULE)moduleHandle, RT_STRING, ResourceNameCallbackDeleteStrings, (LONG_PTR)&args, RESOURCE_ENUM_LN | RESOURCE_ENUM_MUI, MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL));

            return true;
        }


        /**************************************************************************
        * OPEN AND CLOSE MODULE
        **************************************************************************/

        ModuleHandle OpenModule(const std::string& module)
        {
            std::wstring wModule = UTF8ToWide(module);
            return (ModuleHandle)LoadLibraryExW(wModule.c_str(), NULL, LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE);
        }

        void CloseModule(ModuleHandle moduleHandle)
        {
            FreeLibrary(static_cast<HMODULE>(moduleHandle));
        }


        Resource GetResourceInternal(ModuleHandle moduleHandle, LPCWSTR name, LPCWSTR type)
        {
            Resource res;
            HMODULE module = static_cast<HMODULE>(moduleHandle);

            HRSRC resHandle = FindResourceW(module, name, type);
            HGLOBAL dataHandle = LoadResource(module, resHandle);
            res.data = LockResource(dataHandle);
            res.size = SizeofResource(module, resHandle);
            return res;
        }

        Resource GetResource(ModuleHandle moduleHandle, const char* name, const char* type)
        {
            std::wstring wtype = UTF8ToWide(type);
            std::wstring wname = UTF8ToWide(name);
            return GetResourceInternal(moduleHandle, wname.c_str(), wtype.c_str());
        }

        Resource GetResource(ModuleHandle moduleHandle, int nameId, const char* type)
        {
            std::wstring wtype = UTF8ToWide(type);
            return GetResourceInternal(moduleHandle, MAKEINTRESOURCEW(nameId), wtype.c_str());
        }


        BOOL CALLBACK LanguageCallback(HMODULE hModule, LPCWSTR lpType, LPCWSTR lpName, WORD wLanguage, LONG_PTR lParam)
        {
            LanguageId* langId = (LanguageId*)lParam;
            *langId = wLanguage;
            return FALSE; // stop, i just need the first result
        }

        LanguageId GetFirstLanguageId(ModuleHandle moduleHandle, const char* name, const char* type)
        {
            LanguageId langId = 0;

            std::wstring wtype = UTF8ToWide(type);
            std::wstring wname = UTF8ToWide(name);
            EnumResourceLanguagesW(static_cast<HMODULE>(moduleHandle), wtype.c_str(), wname.c_str(), (ENUMRESLANGPROCW)&LanguageCallback, (LONG_PTR)&langId);
            return langId;
        }

        
        /**************************************************************************
        * UPDATE RESOURCES
        **************************************************************************/

        UpdateHandle BeginUpdate(const std::string& modulePath)
        {
            std::wstring wmodulePath = UTF8ToWide(modulePath);
            return BeginUpdateResourceW(wmodulePath.c_str(), FALSE);
        }

        bool UpdateStyleResource(UpdateHandle updateHandle, const char* type, const char* name, LanguageId lid, const char* data, unsigned int length)
        {
            std::wstring wtype = UTF8ToWide(type);
            std::wstring wname = UTF8ToWide(name);
            return UpdateResourceW(updateHandle, wtype.c_str(), wname.c_str(), (WORD)lid, (LPVOID)data, length) != 0;
        }

        bool UpdateStyleResource(UpdateHandle updateHandle, const char* type, const char* name, const char* data, unsigned int length)
        {
            std::wstring wtype = UTF8ToWide(type);
            std::wstring wname = UTF8ToWide(name);
            return UpdateResourceW(updateHandle, wtype.c_str(), wname.c_str(), MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), (LPVOID)data, length) != 0;
        }

        bool UpdateStyleResource(UpdateHandle updateHandle, const char* type, int nameId, const char* data, unsigned int length)
        {
            std::wstring wtype = UTF8ToWide(type);
            return UpdateResourceW(updateHandle, wtype.c_str(), MAKEINTRESOURCEW(nameId), MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), (LPVOID)data, length) != 0;
        }

        int EndUpdate(UpdateHandle updateHandle, bool discard)
        {
            if (EndUpdateResourceW(updateHandle, discard ? TRUE : FALSE) == TRUE)
                return ERROR_SUCCESS;
            else return GetLastError();
        }
    }
}