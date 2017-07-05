#pragma once

#include "VisualStyle.h"

#include <wx\wx.h>
#include <wx\propgrid\property.h>
#include <wx\propgrid\advprops.h>

wxPGProperty* GetWXPropertyFromMsStyleProperty(msstyle::MsStyleProperty& prop);
wxPGChoices* GetEnumsFromMsStyleProperty(msstyle::MsStyleProperty& prop);