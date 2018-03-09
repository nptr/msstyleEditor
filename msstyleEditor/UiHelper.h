#pragma once

#include "libmsstyle/StyleProperty.h"

#include <wx\wx.h>
#include <wx\propgrid\property.h>
#include <wx\propgrid\advprops.h>

wxPGProperty* GetWXPropertyFromMsStyleProperty(libmsstyle::StyleProperty& prop);
wxPGChoices* GetEnumsFromMsStyleProperty(libmsstyle::StyleProperty& prop);

std::string WideToUTF8(const std::wstring& str);
std::wstring UTF8ToWide(const std::string& str);