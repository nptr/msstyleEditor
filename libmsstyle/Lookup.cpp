#include "Lookup.h"

#include "VisualStyleEnums.h"
#include "VisualStyleParts.h"
#include "VisualStyleStates.h"

#define MSSTYLE_ARRAY_LENGTH(name) (sizeof(name) / sizeof(name[0]))

namespace libmsstyle
{
	namespace lookup
	{
		EnumList FindEnums(int32_t nameID)
		{
			EnumList m;
			if (nameID == IDENTIFIER::BGTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_BGTYPE;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_BGTYPE);
			}
			else if (nameID == IDENTIFIER::BORDERTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_BORDERTYPE;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_BORDERTYPE);
			}
			else if (nameID == IDENTIFIER::FILLTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_FILLTYPE;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_FILLTYPE);
			}
			else if (nameID == IDENTIFIER::SIZINGTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_SIZINGTYPE;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_SIZINGTYPE);
			}
			else if (nameID == IDENTIFIER::HALIGN)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_ALIGNMENT_H;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_ALIGNMENT_H);
			}
			else if (nameID == IDENTIFIER::CONTENTALIGNMENT)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_ALIGNMENT_H; // same as the real contentalignment..
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_ALIGNMENT_H);
			}
			else if (nameID == IDENTIFIER::VALIGN)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_ALIGNMENT_V;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_ALIGNMENT_V);
			}
			else if (nameID == IDENTIFIER::OFFSETTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_OFFSET;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_OFFSET);
			}
			else if (nameID == IDENTIFIER::IMAGELAYOUT)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_IMAGELAYOUT;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_IMAGELAYOUT);
			}
			else if (nameID == IDENTIFIER::ICONEFFECT)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_ICONEFFECT;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_ICONEFFECT);
			}
			else if (nameID == IDENTIFIER::GLYPHTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_GLYPHTYPE;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_GLYPHTYPE);
			}
			else if (nameID == IDENTIFIER::IMAGESELECTTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_IMAGESELECT;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_IMAGESELECT);
			}
			else if (nameID == IDENTIFIER::GLYPHFONTSIZINGTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_GLYPHFONTSCALING;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_GLYPHFONTSCALING);
			}
			else if (nameID == IDENTIFIER::TRUESIZESCALINGTYPE)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_TRUESIZESCALING;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_TRUESIZESCALING);
			}
			else
			{
				m.numEnums = 0;
				m.numEnums = NULL;
			}

			return m;
		}


		PartList FindParts(const char* className, Platform platform)
		{
			//
			// TODO: Use base class map (BCMAP) instead of relying on the naming
			//

			PartList m;
			if (strstr(className, "Toolbar"))	// Toolbar is often inherited, so find it first. It also has to be matched before "Button", 
			{									// because otherwise the SearchButton::Toolbar class would use the Button parts instead of the toolbar ones.
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TOOLBAR);
				m.parts = PARTS_TOOLBAR;
			}
			else if (strstr(className, "Button"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_BUTTON);
				m.parts = PARTS_BUTTON;
			}
			else if (strstr(className, "Edit"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_EDIT);
				m.parts = PARTS_EDIT;
			}
			else if (strstr(className, "Rebar"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_REBAR);
				m.parts = PARTS_REBAR;
			}
			else if (strstr(className, "Combobox"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_COMBOBOX);
				m.parts = PARTS_COMBOBOX;
			}
			else if (strstr(className, "ControlPanel"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_CONTROLPANEL);
				m.parts = PARTS_CONTROLPANEL;
			}
			else if (strstr(className, "ExplorerBar"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_EXPLORERBAR);
				m.parts = PARTS_EXPLORERBAR;
			}
			else if (strstr(className, "Listbox"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_LISTBOX);
				m.parts = PARTS_LISTBOX;
			}
			else if (strstr(className, "ListView"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_LISTVIEW);
				m.parts = PARTS_LISTVIEW;
			}
			else if (strstr(className, "Menu"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_MENU);
				m.parts = PARTS_MENU;
			}
			else if (strstr(className, "TreeView"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TREEVIEW);
				m.parts = PARTS_TREEVIEW;
			}
			else if (strstr(className, "DWMWindow"))
			{
				switch (platform)
				{
				case Platform::WIN7:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN7);
					m.parts = PARTS_DWMWINDOW_WIN7;
				} break;
				// TODO: discover and use dedicated partlists
				// for the respective platforms..
				case Platform::WIN8:
				case Platform::WIN81:
				case Platform::WIN10:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN81);
					m.parts = PARTS_DWMWINDOW_WIN81;
				} break;
				default:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN81);
					m.parts = PARTS_DWMWINDOW_WIN81;
				} break;
				}
			}
			else if (strstr(className, "Window"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_WINDOW);
				m.parts = PARTS_WINDOW;
			}
			else if (strstr(className, "TaskDialog"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKDIALOG);
				m.parts = PARTS_TASKDIALOG;
			}
			else if (strstr(className, "Header"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_HEADER);
				m.parts = PARTS_HEADER;
			}
			else if (strstr(className, "AeroWizard"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_AEROWIZARD);
				m.parts = PARTS_AEROWIZARD;
			}
			else if (strstr(className, "Progress"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_PROGRESS);
				m.parts = PARTS_PROGRESS;
			}
			else if (strstr(className, "TrackBar"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TRACKBAR);
				m.parts = PARTS_TRACKBAR;
			}
			else if (strstr(className, "Tab"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TAB);
				m.parts = PARTS_TAB;
			}
			else if (strstr(className, "ToolTip") || strstr(className, "Tooltip")) // overcome inconsitencies in the naming
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TOOLTIP);
				m.parts = PARTS_TOOLTIP;
			}
			else if (strstr(className, "TaskBar"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBAR);
				m.parts = PARTS_TASKBAR;
			}
			else if (strstr(className, "TextStyle"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TEXTSTYLE);
				m.parts = PARTS_TEXTSTYLE;
			}
			else if (strstr(className, "Spin"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_SPIN);
				m.parts = PARTS_SPIN;
			}
			else if (strstr(className, "ScrollBar") || strstr(className, "Scrollbar")) // overcome inconsitencies in the naming
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_SCROLLBAR);
				m.parts = PARTS_SCROLLBAR;
			}
			else if (strstr(className, "TaskBand2"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBAND2);
				m.parts = PARTS_TASKBAND2;
			}
			else if (strstr(className, "TaskBand"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBAND);
				m.parts = PARTS_TASKBAND;
			}
			else if (strstr(className, "Flyout"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_FLYOUT);
				m.parts = PARTS_FLYOUT;
			}
			else if (strstr(className, "DragDrop"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DRAGDROP);
				m.parts = PARTS_DRAGDROP;
			}
			else if (strstr(className, "DatePicker"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DATEPICKER);
				m.parts = PARTS_DATEPICKER;
			}
			else if (strstr(className, "StartPanelPriv"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_STARTPANELPRIV);
				m.parts = PARTS_STARTPANELPRIV;
			}
			else if (strstr(className, "StartPanel"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_STARTPANEL);
				m.parts = PARTS_STARTPANEL;
			}
			else if (strstr(className, "MonthCal"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_MONTHCAL);
				m.parts = PARTS_MONTHCAL;
			}
			else
			{
				m.numParts = 0;
				m.parts = NULL;
			}

			return m;
		}


		const char* GetEnumAsString(int32_t nameID, int32_t enumValue)
		{
			EnumList enums = FindEnums(nameID);

			if (enums.enums != nullptr && enums.numEnums > enumValue)
			{
				return enums.enums[enumValue].value;
			}
			else return nullptr;
		}

		const char* FindPropertyName(int nameID)
		{
			auto ret = libmsstyle::PROPERTY_MAP.find(nameID);
			if (ret != libmsstyle::PROPERTY_MAP.end())
				return ret->second;
			else return "UNKNOWN";
		}

		const char* FindTypeName(int typeID)
		{
			auto ret = libmsstyle::DATATYPE_MAP.find(typeID);
			if (ret != libmsstyle::DATATYPE_MAP.end())
				return ret->second;
			else return "UNKNOWN";
		}
	}
}