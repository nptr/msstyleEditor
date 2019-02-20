#pragma once

#include "StringTable.h"
#include <string>

namespace libmsstyle
{
    namespace priv
    {
        typedef struct
        {
            const void* data;
            unsigned long size;
        } Resource;

        typedef void* ModuleHandle;
        typedef void* UpdateHandle;
        typedef unsigned short LanguageId;

        ModuleHandle OpenModule(const std::string& modulePath);
        void CloseModule(ModuleHandle moduleHandle);

        Resource GetResource(ModuleHandle moduleHandle, const char* name, const char* type);
        Resource GetResource(ModuleHandle moduleHandle, int nameId, const char* type);

        LanguageId GetFirstLanguageId(ModuleHandle moduleHandle, const char* name, const char* type);

        void LoadStringTable(ModuleHandle moduleHandle, StringTable& table);
        bool UpdateStringTable(ModuleHandle moduleHandle, UpdateHandle updateHandle, StringTable& table, std::runtime_error& error);

        // TODO: return error code instead of bool
        UpdateHandle BeginUpdate(const std::string& modulePath);
        bool UpdateStyleResource(UpdateHandle, const char* type, const char* name, LanguageId lid, const char* data, unsigned int length);
        bool UpdateStyleResource(UpdateHandle, const char* type, const char* name, const char* data, unsigned int length);
        bool UpdateStyleResource(UpdateHandle, const char* type, int nameId, const char* data, unsigned int length);
        int EndUpdate(UpdateHandle updateHandle, bool discard);

        // TODO: this file is not exactly the right place
        void RemoveFile(const std::string& path);
        bool DuplicateFile(const std::string& src, const std::string& dst);
        bool FileReadAllBytes(const std::string& src, char** buffer, unsigned int* length);
        void FileFreeBytes(char* bytes);
    }
}

