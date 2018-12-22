#pragma once

#include "libmsstyle\VisualStyle.h"
#include "libmsstyle\StyleProperty.h"

#include <string>

class wxMenu;
class wxPGProperty;
class wxPGChoices;

wxPGProperty* GetWXPropertyFromMsStyleProperty(libmsstyle::VisualStyle& style, libmsstyle::StyleProperty& prop);
wxPGChoices* GetFontsFromMsStyleProperty(libmsstyle::VisualStyle& style, libmsstyle::StyleProperty& prop);
wxPGChoices* GetEnumsFromMsStyleProperty(libmsstyle::StyleProperty& prop);

std::string WideToUTF8(const std::wstring& str);
std::wstring UTF8ToWide(const std::string& str);