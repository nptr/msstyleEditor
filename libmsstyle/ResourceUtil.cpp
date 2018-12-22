#include "ResourceUtil.h"
#include "StringUtil.h"

#include <string>

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

namespace libmsstyle
{
    namespace detail
    {

        ModuleHandle OpenModule(const std::string& module)
        {
            std::wstring wModule = UTF8ToWide(module);
            return (ModuleHandle)LoadLibraryExW(wModule.c_str(), NULL, LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE);
        }

        void CloseModule(ModuleHandle moduleHandle)
        {
            FreeLibrary(static_cast<HMODULE>(moduleHandle));
        }

        Resource GetResource(ModuleHandle moduleHandle, const char* name, const char* type)
        {
            Resource res;
            HMODULE module = static_cast<HMODULE>(moduleHandle);
            HRSRC resHandle = FindResourceA(module, name, type);
            HGLOBAL dataHandle = LoadResource(module, resHandle);
            res.data = LockResource(dataHandle);
            res.size = SizeofResource(module, resHandle);
            return res;
        }

        Resource GetResource(ModuleHandle moduleHandle, int nameId, const char* type)
        {
            return GetResource(moduleHandle, MAKEINTRESOURCEA(nameId), type);
        }

        Resource FindStringResource(ModuleHandle moduleHandle, int id, int langId)
        {
            LPCWSTR pwsz = NULL;
            HMODULE module = static_cast<HMODULE>(moduleHandle);

            // Convert the string ID into a bundle number
            HRSRC resHandle = FindResourceEx(module, RT_STRING, MAKEINTRESOURCE(id / 16 + 1), langId);
            if (resHandle)
            {
                HGLOBAL dataHandle = LoadResource(module, resHandle);
                if (dataHandle)
                {
                    pwsz = reinterpret_cast<LPCWSTR>(LockResource(dataHandle));
                    if (pwsz)
                    {
                        // okay now walk the string table
                        for (int i = 0; i < (id & 15); i++)
                        {
                            pwsz += 1 + (UINT)*pwsz;
                        }
                        UnlockResource(pwsz);
                    }
                    FreeResource(dataHandle);
                }
            }

            Resource res;
            if (pwsz && *pwsz)
            {
                res.data = pwsz + 1;
                res.size = LOBYTE(*pwsz);
            }
            else
            {
                res.data = 0;
                res.size = 0;
            }
            return res;
        }

        BOOL LanguageCallback(
            _In_opt_ HMODULE hModule,
            _In_ LPCSTR lpType,
            _In_ LPCSTR lpName,
            _In_ WORD wLanguage,
            _In_ LONG_PTR lParam)
        {
            LanguageId* langId = (LanguageId*)lParam;
            *langId = wLanguage;
            return FALSE; // stop, i just need the first result
        }

        LanguageId GetFirstLanguageId(ModuleHandle moduleHandle, const char* name, const char* type)
        {
            LanguageId langId = 0;
            EnumResourceLanguagesA(static_cast<HMODULE>(moduleHandle), type, name, (ENUMRESLANGPROCA)&LanguageCallback, (LONG_PTR)&langId);
            return langId;
        }

        UpdateHandle BeginUpdate(const std::string& modulePath)
        {
            return BeginUpdateResourceA(modulePath.c_str(), FALSE);
        }

        bool UpdateStyleResource(UpdateHandle updateHandle, const char* type, const char* name, LanguageId lid, const char* data, unsigned int length)
        {
            return UpdateResourceA(updateHandle, type, name, (WORD)lid, (LPVOID)data, length) != 0;
        }

        bool UpdateStyleResource(UpdateHandle updateHandle, const char* type, const char* name, const char* data, unsigned int length)
        {
            return UpdateResourceA(updateHandle, type, name, MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), (LPVOID)data, length) != 0;
        }

        bool UpdateStyleResource(UpdateHandle updateHandle, const char* type, int nameId, const char* data, unsigned int length)
        {
            return UpdateResourceA(updateHandle, type, MAKEINTRESOURCEA(nameId), MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), (LPVOID)data, length) != 0;
        }

        int EndUpdate(UpdateHandle updateHandle)
        {
            if (EndUpdateResourceA(updateHandle, FALSE) == TRUE)
                return ERROR_SUCCESS;
            else return GetLastError();
        }
    }
}