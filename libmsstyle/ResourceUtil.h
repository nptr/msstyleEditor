#pragma once

#include <string>

namespace libmsstyle
{
    namespace detail
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

        Resource FindStringResource(ModuleHandle moduleHandle, int id, int langId);

        LanguageId GetFirstLanguageId(ModuleHandle moduleHandle, const char* name, const char* type);

        UpdateHandle BeginUpdate(const std::string& modulePath);
        bool UpdateStyleResource(UpdateHandle, const char* type, const char* name, LanguageId lid, const char* data, unsigned int length);
        bool UpdateStyleResource(UpdateHandle, const char* type, const char* name, const char* data, unsigned int length);
        bool UpdateStyleResource(UpdateHandle, const char* type, int nameId, const char* data, unsigned int length);
        int EndUpdate(UpdateHandle updateHandle);
    }
}

