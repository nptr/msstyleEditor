#pragma once

#include <stdint.h>

namespace libmsstyle
{
	typedef struct
	{
		int32_t stateID;
		const char* stateName;
	} StateMap;

	extern const StateMap STATES_AEROWIZARD_HEADERAREA[2];

	extern const StateMap STATES_PUSHBUTTON[7];

	extern const StateMap STATES_RADIOBUTTON[9];

	extern const StateMap STATES_CHARTVIEW_LINE[4];

	extern const StateMap STATES_CHECKBOX[21];

	extern const StateMap STATES_GROUPBOX[3];

	extern const StateMap STATES_COMMANDLINK[7];

	extern const StateMap STATES_COMMANDLINKGLYPHS[6];

	extern const StateMap STATES_CB_STYLE[5];

	extern const StateMap STATES_CB_DROPDOWNLR[5];

	extern const StateMap STATES_CB_TRANSPARENTBG[5];

	extern const StateMap STATES_CB_BORDER[5];

	extern const StateMap STATES_CB_READONLY[5];

	extern const StateMap STATES_CB_CUEBANNER[5];

	extern const StateMap STATES_TAB[4];

	extern const StateMap STATES_LINK_HELP[5];

	extern const StateMap STATES_LINK_TASK[6];

	extern const StateMap STATES_LINK_CONTENT[5];

	extern const StateMap STATES_LINK_SECTIONTITLE[3];

	extern const StateMap STATES_DATE_TEXT[4];

	extern const StateMap STATES_DATE_BORDER[5];

	extern const StateMap STATES_DATE_CALENDERBUTTONRIGHT[5];

	extern const StateMap STATES_DND_GENERAL[3];

	extern const StateMap STATES_EDITTEXT[9];

	extern const StateMap STATES_EDITTEXT_BG[7];

	extern const StateMap STATES_EDITTEXT_BGWITHBORDER[5];

	extern const StateMap STATES_EDITTEXT_GENERAL[5];

	extern const StateMap STATES_EXPLORERBAR_HDRPIN[7];

	extern const StateMap STATES_EXPLORERBAR_GENERAL[4];

	extern const StateMap STATES_FLYOUT_LABEL[5];

	extern const StateMap STATES_FLYOUT_LINK[3];

	extern const StateMap STATES_FLYOUT_BODY[3];

	extern const StateMap STATES_FLYOUT_HEADER[3];

	extern const StateMap STATES_HEADER_ITEMSTATES[13];

	extern const StateMap STATES_HEADER_LEFT_AND_RIGHT[4];

	extern const StateMap STATES_HEADERSORTARROWSTATES[3];

	extern const StateMap STATES_HEADERDROPDOWNSTATES[4];

	extern const StateMap STATES_HEADERDROPDOWNFILTERSTATES[4];

	extern const StateMap STATES_HEADEROVERFLOWSTATES[3];

	extern const StateMap STATES_ITEMSVIEW_SEARCHHIT[5];

	extern const StateMap STATES_ITEMSVIEW_FOCUSRECT[3];

	extern const StateMap STATES_ITEMSVIEW_PROPERTY[14];

	extern const StateMap STATES_LISTBOX_SCROLL[5];

	extern const StateMap STATES_LISTBOX_ITEMS[5];

	extern const StateMap STATES_LISTVIEW_ITEMS[7];

	extern const StateMap STATES_LISTVIEW_GROUPHEADER_GENERAL[17];

	extern const StateMap STATES_LISTVIEW_EXPCOLLAPSE[4];

	extern const StateMap STATES_MENU_BARBG[3];

	extern const StateMap STATES_MENU_BARITEM[7];

	extern const StateMap STATES_MENU_POPCHECK[5];

	extern const StateMap STATES_MENU_POPCHECKBG[4];

	extern const StateMap STATES_MENU_POPITEMS[5];

	extern const StateMap STATES_MENU_SYSTEM_GENERAL[3];

	extern const StateMap STATES_MONTHCAL_CELL[8];

	extern const StateMap STATES_GENERAL[5];

	extern const StateMap STATES_PROGRESS_TRANSPARENT_GENERAL[3];

	extern const StateMap STATES_PROGRESS_FILL_GENERAL[5];

	extern const StateMap STATES_REBAR_GENERAL[4];

	extern const StateMap STATES_SCROLLBAR_ARROWBTN[21];

	extern const StateMap STATES_SCROLLBAR_STYLE[6];

	extern const StateMap STATES_SCROLLBAR_SIZEBOX[9];

	extern const StateMap STATES_SPIN_GENERAL[5];

	extern const StateMap STATES_TABITEM_GENERAL[6];

	extern const StateMap STATES_TASKDLG_EXPANDOBUTTON[9];

	extern const StateMap STATES_TEXTSTYLE_HLINK[5];

	extern const StateMap STATES_TEXTSTYLE_CTRLLABEL[3];

	extern const StateMap STATES_TOOLBARSTYLE[9];

	extern const StateMap STATES_TOOLTIP_CLOSE[4];

	extern const StateMap STATES_TOOLTIP_BALLOON_AND_STANDARD[3];

	extern const StateMap STATES_TOOLTIP_BALLOONSTEM[7];

	extern const StateMap STATES_TOOLTIP_WRENCH[4];

	extern const StateMap STATES_TRACKBAR_GENERAL[6];

	extern const StateMap STATES_TRACKBAR_THUMB_GEN[6];

	extern const StateMap STATES_TREEVIEW_ITEM[7];

	extern const StateMap STATES_TREEVIEW_GLYPH[3];

	extern const StateMap STATES_USERTILE_HOVERBACKGROUND[4];

	extern const StateMap STATES_GRIPPER[3];

	extern const StateMap STATES_WINDOW_FRAME_GEN[3];

	extern const StateMap STATES_WINDOW_CAPTION_GEN[4];

	extern const StateMap STATES_WINDOW_BTN_AND_THUMB[5];

	extern const StateMap STATES_WINDOW_CAPTION_SMALL[4];
}