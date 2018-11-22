#pragma once

#include "libmsstyle\StyleProperty.h"
#include "Controls\propertyGrid.h"

#include <string>

void SetInitPropgridItem(HWND grid, PROPGRIDITEM& item, libmsstyle::StyleProperty& prop, int index = -1);

std::string StdWideToUTF8(const std::wstring& str);
std::wstring StdUTF8ToWide(const std::string& str);
char* WideToUTF8(const wchar_t* str);
wchar_t* UTF8ToWide(const char* str);