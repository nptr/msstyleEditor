#pragma once

#include <Windows.h>
#include <Uxtheme.h>

namespace uxtheme
{
	// https://doxygen.reactos.org/de/d08/dll_2win32_2uxtheme_2system_8c_source.html#l01101


	// Ordinal 2
	// colorName: eg. "NormalColor"
	// sizeName: eg. "NormalSize"
	// unknownflags: 0 works fine
	// Can open the aero.msstyles just fine, but fails on custom themes
	typedef HRESULT(WINAPI *OpenThemeFilePtr)(LPCWSTR themeFile, LPCWSTR colorName, LPCWSTR sizeName, HANDLE* hThemeFile, DWORD unknownFlags);

	// Ordinal 3
	typedef HRESULT(WINAPI *CloseThemeFilePtr)(HANDLE hThemeFile);
	// Ordinal 4
	typedef HRESULT(WINAPI *ApplyThemePtr)(HANDLE hThemeFile, char *unknown, HWND hWnd);
	// Ordinal 16
	typedef HTHEME(WINAPI *OpenThemeDataFromFilePtr)(HANDLE hThemeFile, HWND hwnd, LPCWSTR pszClassList, DWORD flags);
	// Ordinal 65
	typedef HRESULT(WINAPI *SetSystemThemePtr)(LPCWSTR themeFile, LPCWSTR colorName, LPCWSTR sizeName, DWORD unknownFlags);
	// Ordinal 94 - exported by name as well
	typedef HRESULT(WINAPI *GetCurrentThemeNamePtr)(LPWSTR themeFile, int maxNameChars,
		LPWSTR colorName, int maxColorChars,
		LPWSTR sizeName, int maxSizeChars);

	HMODULE hModule = LoadLibraryW(L"uxtheme.dll");


	OpenThemeFilePtr OpenThemeFile = (OpenThemeFilePtr)GetProcAddress(hModule, (PCSTR)(2));
	CloseThemeFilePtr CloseThemeFile = (CloseThemeFilePtr)GetProcAddress(hModule, (PCSTR)(3));
	ApplyThemePtr ApplyTheme = (ApplyThemePtr)GetProcAddress(hModule, (PCSTR)(4));
	OpenThemeDataFromFilePtr OpenThemeDataFromFile = (OpenThemeDataFromFilePtr)GetProcAddress(hModule, (PCSTR)(16));
	SetSystemThemePtr SetSystemTheme = (SetSystemThemePtr)GetProcAddress(hModule, (PCSTR)(65));
	GetCurrentThemeNamePtr GetCurrentThemeName = (GetCurrentThemeNamePtr)GetProcAddress(hModule, "GetCurrentThemeName");
}