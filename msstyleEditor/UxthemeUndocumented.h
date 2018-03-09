#pragma once

#include <Windows.h>
#include <Uxtheme.h>

namespace uxtheme
{
	// https://doxygen.reactos.org/de/d08/dll_2win32_2uxtheme_2system_8c_source.html#l01101

	// Ordinal 65
	typedef HRESULT(WINAPI *SetSystemThemePtr)(LPCWSTR themeFile, LPCWSTR colorName, LPCWSTR sizeName, int unknownflags);

	// Ordinal 2
	// colorName: eg. "NormalColor"
	// sizeName: eg. "NormalSize"
	// unknownflags: 0 works fine
	// Can open the aero.msstyles just fine, but fails on custom themes?
	typedef HRESULT(WINAPI *OpenThemeFilePtr)(LPCWSTR themeFile, LPCWSTR colorName, LPCWSTR sizeName, HANDLE* hThemeFile, int unknownflags);

	// Ordinal 3
	typedef HRESULT(WINAPI *CloseThemeFilePtr)(HANDLE hThemeFile);

	// Ordinal 4
	typedef HRESULT(WINAPI *ApplyThemePtr)(HANDLE hThemeFile, char *unknown, HWND hWnd);

	// Ordinal 16
	typedef HTHEME(WINAPI *OpenThemeDataFromFilePtr)(HANDLE hThemeFile, HWND hwnd, LPCWSTR pszClassList, DWORD flags);

	HMODULE hModule = LoadLibrary(_T("uxtheme.dll"));
	SetSystemThemePtr SetSystemTheme = (SetSystemThemePtr)GetProcAddress(hModule, (PCSTR)(65));

	//
	// Initial plan was to use OpenThemeFilePtr() to retrieve a intermediate handle, then OpenThemeDataFromFile() to
	// get a full data handle (HTHEME) and use this in SetWindowTheme() to apply the style for just a single application/control.
	// Unfortunately OpenThemeFile() seems to fail on custom themes :/
	//
	OpenThemeFilePtr OpenThemeFile = (OpenThemeFilePtr)GetProcAddress(hModule, (PCSTR)(2));
	CloseThemeFilePtr CloseThemeFile = (CloseThemeFilePtr)GetProcAddress(hModule, (PCSTR)(3));
	ApplyThemePtr ApplyTheme = (ApplyThemePtr)GetProcAddress(hModule, (PCSTR)(4));
	OpenThemeDataFromFilePtr OpenThemeDataFromFile = (OpenThemeDataFromFilePtr)GetProcAddress(hModule, (PCSTR)(16));
}