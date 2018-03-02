#pragma once

#include "libmsstyle/StyleProperty.h"

#include <wx\wx.h>
#include <wx\propgrid\property.h>
#include <wx\propgrid\advprops.h>

wxPGProperty* GetWXPropertyFromMsStyleProperty(libmsstyle::StyleProperty& prop);
wxPGChoices* GetEnumsFromMsStyleProperty(libmsstyle::StyleProperty& prop);