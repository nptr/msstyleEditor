#pragma once

#include "VisualStyleStates.h"
#include <stdint.h>

namespace libmsstyle
{
	typedef struct _PartMap
	{
		int32_t partID;
		const char* partName;
		const StateMap* states;
		const int32_t numStates;
	} PartMap;

	extern const PartMap PARTS_ADDRESSBAND[2];

	extern const PartMap PARTS_BARRIERPAGE[2];

	extern const PartMap PARTS_BREADCRUMBBAR[2];

	extern const PartMap PARTS_BUTTON[11];

	extern const PartMap PARTS_CLOCK[2];

	extern const PartMap PARTS_CHARTVIEW[43];

	extern const PartMap PARTS_COMMANDMODULE[13];

	extern const PartMap PARTS_COMMUNICATIONS[2];

	extern const PartMap PARTS_COMBOBOX[9];

	extern const PartMap PARTS_CONTROLPANEL[20];

	extern const PartMap PARTS_COPYCLOSE[2];

	extern const PartMap PARTS_DROPLIST[2];

	extern const PartMap PARTS_EMPTYMARKUP[2];

	extern const PartMap PARTS_EXPLORERBAR[13];

	extern const PartMap PARTS_INFOBAR[3];

	extern const PartMap PARTS_ITEMSVIEW[7];

	extern const PartMap PARTS_LISTBOX[6];

	extern const PartMap PARTS_LISTVIEW[11];

	extern const PartMap PARTS_LINK[2];

	extern const PartMap PARTS_MENU[21];

	extern const PartMap PARTS_NAVIGATION[4];

	extern const PartMap PARTS_TREEVIEW[5];

	extern const PartMap PARTS_WINDOW[40];
	// W8+
	extern const PartMap PARTS_DWMPEN[25];
	// W8+
	extern const PartMap PARTS_DWMTOUCH[9];

	extern const PartMap PARTS_DWMWINDOW_WIN7[72];

	extern const PartMap PARTS_DWMWINDOW_WIN81[51];

	extern const PartMap PARTS_DWMWINDOW_WIN10[92];

	extern const PartMap PARTS_EDIT[10];

	extern const PartMap PARTS_TASKDIALOG[22];

	extern const PartMap PARTS_HEADER[8];

	extern const PartMap PARTS_READINGPANE[3];

	extern const PartMap PARTS_REBAR[9];

	extern const PartMap PARTS_AEROWIZARD[6];

	extern const PartMap PARTS_PAUSE[2];

	extern const PartMap PARTS_PROGRESS[13];

	extern const PartMap PARTS_PROPERTREE[3];

	extern const PartMap PARTS_PREVIEWPANE[10];

	extern const PartMap PARTS_TRACKBAR[11];

	extern const PartMap PARTS_TAB[12];

	extern const PartMap PARTS_TOOLTIP[8];

	extern const PartMap PARTS_TOOLBAR[8];

	extern const PartMap PARTS_TASKBARPEARL[3];

	extern const PartMap PARTS_TASKBARSHOWDESKTOP[3];

	extern const PartMap PARTS_TASKBAND[4];

	extern const PartMap PARTS_TASKBAND2[15];

	extern const PartMap PARTS_TASKBANDEXUI[17];

	extern const PartMap PARTS_TASKMANAGER[47];

	extern const PartMap PARTS_TEXTGLOW[2];

	extern const PartMap PARTS_TEXTSTYLE[10];

	extern const PartMap PARTS_TEXTSELECTIONGRIPPER[2];

	extern const PartMap PARTS_TRAYNOTIFY[3];

	extern const PartMap PARTS_TRYHARDER[3];

	extern const PartMap PARTS_SEARCHBOX[4];

	extern const PartMap PARTS_SEARCHHOME[3];

	extern const PartMap PARTS_SPIN[5];

	extern const PartMap PARTS_SCROLLBAR[11];

	extern const PartMap PARTS_STATIC[2];

	extern const PartMap PARTS_STATUS[4];

	extern const PartMap PARTS_FLYOUT[9];

	extern const PartMap PARTS_DRAGDROP[9];

	extern const PartMap PARTS_DATEPICKER[4];

	extern const PartMap PARTS_TASKBAR[9];

	extern const PartMap PARTS_STARTPANEL[20];

	extern const PartMap PARTS_STARTPANELPRIV[39];

	extern const PartMap PARTS_MONTHCAL[12];

	extern const PartMap PARTS_USERTILE[3];
}