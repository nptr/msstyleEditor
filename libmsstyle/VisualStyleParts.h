#pragma once

#include <stdint.h>
#include "VisualStyleStates.h"

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

	extern const PartMap PARTS_BUTTON[8];

	extern const PartMap PARTS_COMBOBOX[9];

	extern const PartMap PARTS_CONTROLPANEL[20];

	extern const PartMap PARTS_EXPLORERBAR[13];

	extern const PartMap PARTS_LISTBOX[6];

	extern const PartMap PARTS_LISTVIEW[11];

	extern const PartMap PARTS_LINK[2];

	extern const PartMap PARTS_MENU[21];

	extern const PartMap PARTS_TREEVIEW[5];

	extern const PartMap PARTS_WINDOW[39];

	extern const PartMap PARTS_DWMWINDOW_WIN7[72];

	// Incomplete and maybe incorrect list of parts for Win 8.1
	extern const PartMap PARTS_DWMWINDOW_WIN81[51];

	extern const PartMap PARTS_EDIT[10];

	extern const PartMap PARTS_TASKDIALOG[22];

	extern const PartMap PARTS_HEADER[8];

	extern const PartMap PARTS_REBAR[9];

	extern const PartMap PARTS_AEROWIZARD[6];

	extern const PartMap PARTS_PROGRESS[13];

	extern const PartMap PARTS_TRACKBAR[11];

	extern const PartMap PARTS_TAB[12];

	extern const PartMap PARTS_TOOLTIP[8];

	extern const PartMap PARTS_TOOLBAR[8];

	extern const PartMap PARTS_TASKBAND[4];

	extern const PartMap PARTS_TASKBAND2[15];

	extern const PartMap PARTS_TEXTSTYLE[10];

	extern const PartMap PARTS_SPIN[5];

	extern const PartMap PARTS_SCROLLBAR[11];

	extern const PartMap PARTS_STATUS[4];

	extern const PartMap PARTS_FLYOUT[9];

	extern const PartMap PARTS_DRAGDROP[9];

	extern const PartMap PARTS_DATEPICKER[4];

	extern const PartMap PARTS_TASKBAR[9];

	extern const PartMap PARTS_STARTPANEL[20];

	// incomplete list, derived from PARTS_STARTPANEL
	// with a bit of guessing. Works best for Win7.
	extern const PartMap PARTS_STARTPANELPRIV[39];

	extern const PartMap PARTS_MONTHCAL[12];
}