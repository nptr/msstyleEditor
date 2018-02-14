#pragma once

#include <string>

typedef struct tagResource
{
	const void* data;
	unsigned long size;
} Resource;

typedef void* ModuleHandle;


ModuleHandle OpenModule(const std::string& module);
void CloseModule(ModuleHandle moduleHandle);

Resource GetResource(ModuleHandle moduleHandle, const char* name, const char* type);
Resource GetResource(ModuleHandle moduleHandle, const wchar_t* name, const wchar_t* type);