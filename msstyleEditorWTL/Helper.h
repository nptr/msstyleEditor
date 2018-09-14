#pragma once

#include "libmsstyle\StyleProperty.h"
#include "Controls\PropertyItem.h"

#include <string>

HPROPERTY GetPropertyItemFromStyleProperty(libmsstyle::StyleProperty& prop);

std::string StdWideToUTF8(const std::wstring& str);
std::wstring StdUTF8ToWide(const std::string& str);
char* WideToUTF8(const wchar_t* str);
wchar_t* UTF8ToWide(const char* str);