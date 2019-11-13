#include "Lookup.h"

#include "VisualStyleEnums.h"
#include "VisualStyleParts.h"
#include "VisualStyleStates.h"

#include "StringUtil.h"

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
			else if (nameID >= IDENTIFIER::UNKNOWN_5110_HC &&
				nameID <= IDENTIFIER::UNKNOWN_5122_HC)
			{
				m.enums = (EnumMap*)&libmsstyle::ENUM_HIGHCONTRASTTYPE;
				m.numEnums = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_HIGHCONTRASTTYPE);
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
			if (strstr(className, "Toolbar"))	// Toolbar is often inherited, so find it first. It also has to be caught before "Button", 
			{									// because otherwise the SearchButton::Toolbar class would use the Button parts instead of the toolbar ones.
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TOOLBAR);
				m.parts = PARTS_TOOLBAR;
			}
			else if (strstr(className, "::Header")) // match inherited..
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_HEADER);
				m.parts = PARTS_HEADER;
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
			else if (strstr(className, "AddressBand"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_ADDRESSBAND);
				m.parts = PARTS_ADDRESSBAND;
			}
			else if (strstr(className, "BarrierPage"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_BARRIERPAGE);
				m.parts = PARTS_BARRIERPAGE;
			}
			else if (strstr(className, "BreadcrumbBar"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_BREADCRUMBBAR);
				m.parts = PARTS_BREADCRUMBBAR;
			}
			else if (strstr(className, "ReadingPane"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_READINGPANE);
				m.parts = PARTS_READINGPANE;
			}
			else if (strstr(className, "Rebar"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_REBAR);
				m.parts = PARTS_REBAR;
			}
			else if (strstr(className, "::Clock"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_CLOCK);
				m.parts = PARTS_CLOCK;
			}
			else if (strstr(className, "ChartView"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_CHARTVIEW);
				m.parts = PARTS_CHARTVIEW;
			}
			else if (strstr(className, "CommandModule"))
			{
				switch (platform)
				{
				case Platform::WIN7:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_COMMANDMODULE_WIN7);
					m.parts = PARTS_COMMANDMODULE_WIN7;
				} break;
				case Platform::WIN8:
				case Platform::WIN81:
				case Platform::WIN10:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_COMMANDMODULE_WIN8);
					m.parts = PARTS_COMMANDMODULE_WIN8;
				} break;
				default:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_COMMANDMODULE_WIN8);
					m.parts = PARTS_COMMANDMODULE_WIN8;
				} break;
				}
			}
			else if (strstr(className, "CommunicationsStyle"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_COMMUNICATIONS);
				m.parts = PARTS_COMMUNICATIONS;
			}
			else if (strstr(className, "Combobox") || strstr(className, "ComboBox"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_COMBOBOX);
				m.parts = PARTS_COMBOBOX;
			}
			else if (strstr(className, "ControlPanel"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_CONTROLPANEL);
				m.parts = PARTS_CONTROLPANEL;
			}
			else if (strstr(className, "CopyClose"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_COPYCLOSE);
				m.parts = PARTS_COPYCLOSE;
			}
			else if (strstr(className, "DropListControl"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DROPLIST);
				m.parts = PARTS_DROPLIST;
			}
			else if (strstr(className, "EmptyMarkup"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_EMPTYMARKUP);
				m.parts = PARTS_EMPTYMARKUP;
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
			else if (strstr(className, "InfoBar"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_INFOBAR);
				m.parts = PARTS_INFOBAR;
			}
			else if (strstr(className, "ItemsView")) // after listview since it inherits..
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_ITEMSVIEW);
				m.parts = PARTS_ITEMSVIEW;
			}
			else if (strstr(className, "Link"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_LINK);
				m.parts = PARTS_LINK;
			}
			else if (strstr(className, "Menu"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_MENU);
				m.parts = PARTS_MENU;
			}
			else if (strstr(className, "Navigation"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_NAVIGATION);
				m.parts = PARTS_NAVIGATION;
			}
			else if (strstr(className, "TreeView"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TREEVIEW);
				m.parts = PARTS_TREEVIEW;
			}
			else if (strstr(className, "DWMPen"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMPEN);
				m.parts = PARTS_DWMPEN;
			}
			else if (strstr(className, "DWMTouch"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMTOUCH);
				m.parts = PARTS_DWMTOUCH;
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
				case Platform::WIN8:
				case Platform::WIN81:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN81);
					m.parts = PARTS_DWMWINDOW_WIN81;
				} break;
				case Platform::WIN10:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN10);
					m.parts = PARTS_DWMWINDOW_WIN10;
				} break;
				default:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN10);
					m.parts = PARTS_DWMWINDOW_WIN10;
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
			else if (strstr(className, "Pause"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_PAUSE);
				m.parts = PARTS_PAUSE;
			}
			else if (strstr(className, "Progress"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_PROGRESS);
				m.parts = PARTS_PROGRESS;
			}
			else if (strstr(className, "ProperTree"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_PROPERTREE);
				m.parts = PARTS_PROPERTREE;
			}
			else if (strstr(className, "PreviewPane"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_PREVIEWPANE);
				m.parts = PARTS_PREVIEWPANE;
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
			else if (strstr(className, "ToolTip") || strstr(className, "Tooltip")) // overcome inconsistencies in the naming
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TOOLTIP);
				m.parts = PARTS_TOOLTIP;
			}
			else if (strstr(className, "TaskBar"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBAR);
				m.parts = PARTS_TASKBAR;
			}
			else if (strstr(className, "TextGlow"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TEXTGLOW);
				m.parts = PARTS_TEXTGLOW;
			}
			else if (strstr(className, "TextStyle"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TEXTSTYLE);
				m.parts = PARTS_TEXTSTYLE;
			}
			else if (strstr(className, "TextSelectionGripper"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TEXTSELECTIONGRIPPER);
				m.parts = PARTS_TEXTSELECTIONGRIPPER;
			}
			else if (strstr(className, "::TrayNotify"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TRAYNOTIFY);
				m.parts = PARTS_TRAYNOTIFY;
			}
			else if (strstr(className, "TryHarder"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TRYHARDER);
				m.parts = PARTS_TRYHARDER;
			}
			else if (strstr(className, "SearchBox") || strstr(className, "Searchbox")) // matches HelpSearchBox as well, thats ok
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_SEARCHBOX);
				m.parts = PARTS_SEARCHBOX;
			}
			else if (strstr(className, "SearchHome"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_SEARCHHOME);
				m.parts = PARTS_SEARCHHOME;
			}
			else if (strstr(className, "Spin"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_SPIN);
				m.parts = PARTS_SPIN;
			}
			else if (strstr(className, "ScrollBar") || strstr(className, "Scrollbar")) // overcome inconsistencies in the naming
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_SCROLLBAR);
				m.parts = PARTS_SCROLLBAR;
			}
			else if (strstr(className, "Static"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_STATIC);
				m.parts = PARTS_STATIC;
			}
			else if (strstr(className, "Status"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_STATUS);
				m.parts = PARTS_STATUS;
			}
			else if (strstr(className, "TaskbarPearl"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBARPEARL);
				m.parts = PARTS_TASKBARPEARL;
			}
			else if (strstr(className, "TaskbarShowDesktop"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBARSHOWDESKTOP);
				m.parts = PARTS_TASKBARSHOWDESKTOP;
			}
			else if (strstr(className, "TaskbandExtendedUI"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBANDEXUI);
				m.parts = PARTS_TASKBANDEXUI;
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
			else if (strstr(className, "TaskManager"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKMANAGER);
				m.parts = PARTS_TASKMANAGER;
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
			else if (strstr(className, "UserTile"))
			{
				m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_USERTILE);
				m.parts = PARTS_USERTILE;
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
			auto ret = libmsstyle::PROPERTY_INFO_MAP.find(nameID);
			if (ret != libmsstyle::PROPERTY_INFO_MAP.end())
				return ret->second.name;
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