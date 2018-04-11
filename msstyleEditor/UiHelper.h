#pragma once

#include "libmsstyle\StyleProperty.h"

class wxMenu;
class wxPGProperty;
class wxPGChoices;

wxPGProperty* GetWXPropertyFromMsStyleProperty(libmsstyle::StyleProperty& prop);
wxPGChoices* GetEnumsFromMsStyleProperty(libmsstyle::StyleProperty& prop);

std::string WideToUTF8(const std::wstring& str);
std::wstring UTF8ToWide(const std::string& str);