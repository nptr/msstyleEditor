#include "ResourceUtil.h"
#include <string>
#include "StringConvert.h"

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

ModuleHandle OpenModule(const std::string& module)
{
	std::wstring wModule = UTF8ToWide(module);
	return (ModuleHandle)LoadLibraryExW(wModule.c_str(), NULL, LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE);
}

void CloseModule(ModuleHandle moduleHandle)
{

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

Resource GetResource(ModuleHandle moduleHandle, const wchar_t* name, const wchar_t* type)
{
	Resource res;
	HMODULE module = static_cast<HMODULE>(moduleHandle);
	HRSRC resHandle = FindResourceW(module, name, type);
	HGLOBAL dataHandle = LoadResource(module, resHandle);
	res.data = LockResource(dataHandle);
	res.size = SizeofResource(module, resHandle);
	return res;
}