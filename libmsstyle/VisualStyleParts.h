#pragma once

#include <stdint.h>
#include "VisualStyleStates.h"

namespace libmsstyle
{
	typedef struct _NameMap
	{
		int32_t partID;
		const char* partName;
		const StateMap* states;
		const int32_t numStates;
	} NameMap;

	extern const NameMap PARTS_BUTTON[8];

	extern const NameMap PARTS_COMBOBOX[9];

	extern const NameMap PARTS_CONTROLPANEL[20];

	extern const NameMap PARTS_EXPLORERBAR[13];

	extern const NameMap PARTS_LISTBOX[6];

	extern const NameMap PARTS_LISTVIEW[11];

	extern const NameMap PARTS_MENU[21];

	extern const NameMap PARTS_TREEVIEW[5];

	extern const NameMap PARTS_WINDOW[39];

	extern const NameMap PARTS_DWMWINDOW_WIN7[72];

	// Incomplete and maybe incorrect list of parts for Win 8.1
	extern const NameMap PARTS_DWMWINDOW_WIN81[51];

	extern const NameMap PARTS_EDIT[10];

	extern const NameMap PARTS_TASKDIALOG[22];

	extern const NameMap PARTS_HEADER[8];

	extern const NameMap PARTS_REBAR[9];

	extern const NameMap PARTS_AEROWIZARD[6];

	extern const NameMap PARTS_PROGRESS[13];

	extern const NameMap PARTS_TRACKBAR[11];

	extern const NameMap PARTS_TAB[12];

	extern const NameMap PARTS_TOOLTIP[8];

	extern const NameMap PARTS_TOOLBAR[8];

	extern const NameMap PARTS_TASKBAND[4];

	extern const NameMap PARTS_TASKBAND2[15];

	extern const NameMap PARTS_TEXTSTYLE[10];

	extern const NameMap PARTS_SPIN[5];

	extern const NameMap PARTS_SCROLLBAR[11];

	extern const NameMap PARTS_FLYOUT[9];

	extern const NameMap PARTS_DRAGDROP[9];

	extern const NameMap PARTS_DATEPICKER[4];

	extern const NameMap PARTS_TASKBAR[9];

	extern const NameMap PARTS_STARTPANEL[20];

	// incomplete list, derived from PARTS_STARTPANEL
	// with a bit of guessing. Works best for Win7.
	extern const NameMap PARTS_STARTPANELPRIV[39];

	extern const NameMap PARTS_MONTHCAL[12];
}