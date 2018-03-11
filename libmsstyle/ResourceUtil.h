#pragma once

#include <string>

namespace libmsstyle
{

	typedef struct tagResource
	{
		const void* data;
		unsigned long size;
	} Resource;

	typedef void* ModuleHandle;


	ModuleHandle OpenModule(const std::string& module);
	void CloseModule(ModuleHandle moduleHandle);

	Resource GetResource(ModuleHandle moduleHandle, const char* name, const char* type);
	Resource GetResource(ModuleHandle moduleHandle, int nameId, const char* type);

}

