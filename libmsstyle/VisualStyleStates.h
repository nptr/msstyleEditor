#pragma once

#include <stdint.h>

namespace libmsstyle
{
	typedef struct
	{
		int32_t stateID;
		const char* stateName;
	} StateMap;


	static const StateMap STATES_PUSHBUTTON[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
		{ 5, "DEFAULTED" },
		{ 6, "DEFAULTED_ANIMATING" }
	};

	static const StateMap STATES_RADIOBUTTON[] =
	{
		{ 0, "Common" },
		{ 1, "UNCHECKEDNORMAL" },
		{ 2, "UNCHECKEDHOT" },
		{ 3, "UNCHECKEDPRESSED" },
		{ 4, "UNCHECKEDDISABLED" },
		{ 5, "CHECKEDNORMAL" },
		{ 6, "CHECKEDHOT" },
		{ 7, "CHECKEDPRESSED" },
		{ 8, "CHECKEDDISABLED" },
	};

	static const StateMap STATES_CHECKBOX[] =
	{
		{ 0, "Common" },
		{ 1, "UNCHECKEDNORMAL" },
		{ 2, "UNCHECKEDHOT" },
		{ 3, "UNCHECKEDPRESSED" },
		{ 4, "UNCHECKEDDISABLED" },
		{ 5, "CHECKEDNORMAL" },
		{ 6, "CHECKEDHOT" },
		{ 7, "CHECKEDPRESSED" },
		{ 8, "CHECKEDDISABLED" },
		{ 9, "MIXEDNORMAL" },
		{ 10, "MIXEDHOT" },
		{ 11, "MIXEDPRESSED" },
		{ 12, "MIXEDDISABLED" },
		{ 13, "IMPLICITNORMAL" },
		{ 14, "IMPLICITHOT" },
		{ 15, "IMPLICITPRESSED" },
		{ 16, "IMPLICITDISABLED" },
		{ 17, "EXCLUDEDNORMAL" },
		{ 18, "EXCLUDEDHOT" },
		{ 19, "EXCLUDEDPRESSED" },
		{ 20, "EXCLUDEDDISABLED" },
	};

	static const StateMap STATES_GROUPBOX[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "DISABLED" },
	};

	static const StateMap STATES_COMMANDLINK[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
		{ 5, "DEFAULTED" },
		{ 6, "DEFAULTED_ANIMATING" }
	};

	static const StateMap STATES_COMMANDLINKGLYPHS[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
		{ 5, "DEFAULTED" },
	};

	static const StateMap STATES_CB_STYLE[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_CB_DROPDOWNLR[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_CB_TRANSPARENTBG[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "DISABLED" },
		{ 4, "FOCUSED" },
	};

	static const StateMap STATES_CB_BORDER[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "FOCUSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_CB_READONLY[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_CB_CUEBANNER[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_TAB[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "SELECTED" },
	};

	static const StateMap STATES_LINK_HELP[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },

	};

	static const StateMap STATES_LINK_TASK[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
		{ 5, "PAGE" },
	};

	static const StateMap STATES_LINK_CONTENT[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_LINK_SECTIONTITLE[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
	};

	static const StateMap STATES_DATE_TEXT[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "DISABLED" },
		{ 3, "SELECTED" },
	};

	static const StateMap STATES_DATE_BORDER[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "FOCUSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_DATE_CALENDERBUTTONRIGHT[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_DND_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "HIGHLIGHT" },
		{ 2, "NOHIGHLIGHT" },
	};

	static const StateMap STATES_EDITTEXT[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "SELECTED" },
		{ 4, "DISABLED" },
		{ 5, "FOCUSED" },
		{ 6, "READONLY" },
		{ 7, "ASSIST" },
		{ 8, "CUEBANNER" },
	};

	static const StateMap STATES_EDITTEXT_BG[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "DISABLED" },
		{ 4, "FOCUSED" },
		{ 5, "READONLY" },
		{ 6, "ASSIST" },

	};

	static const StateMap STATES_EDITTEXT_BGWITHBORDER[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "DISABLED" },
		{ 4, "FOCUSED" },
	};

	static const StateMap STATES_EDITTEXT_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "FOCUSED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_EXPLORERBAR_HDRPIN[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "SELECTEDNORMAL" },
		{ 5, "SELECTEDHOT" },
		{ 6, "SELECTEDPRESSED" },
	};

	static const StateMap STATES_EXPLORERBAR_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
	};

	static const StateMap STATES_FLYOUT_LABEL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "SELECTED" },
		{ 3, "EMPHASIZED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_FLYOUT_LINK[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOVER" },
	};

	static const StateMap STATES_FLYOUT_BODY[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "EMPHASIZED" },
	};

	static const StateMap STATES_FLYOUT_HEADER[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOVER" },
	};

	static const StateMap STATES_HEADER_ITEMSTATES[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "SORTEDNORMAL" },
		{ 5, "SORTEDHOT" },
		{ 6, "SORTEDPRESSED" },
		{ 7, "ICONNORMAL" },
		{ 8, "ICONHOT" },
		{ 9, "ICONPRESSED" },
		{ 10, "ICONSORTEDNORMAL" },
		{ 11, "ICONSORTEDHOT" },
		{ 12, "ICONSORTEDPRESSED" },
	};

	static const StateMap STATES_HEADER_LEFT_AND_RIGHT[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
	};

	static const StateMap STATES_HEADERSORTARROWSTATES[] =
	{
		{ 0, "Common" },
		{ 1, "SORTEDUP" },
		{ 2, "SORTEDDOWN" },
	};

	static const StateMap STATES_HEADERDROPDOWNSTATES[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "SOFTHOT" },
		{ 3, "HOT" },

	};

	static const StateMap STATES_HEADERDROPDOWNFILTERSTATES[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "SOFTHOT" },
		{ 3, "HOT" },
	};

	static const StateMap STATES_HEADEROVERFLOWSTATES[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
	};

	static const StateMap STATES_LISTBOX_SCROLL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "FOCUSED" },
		{ 3, "HOT" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_LISTBOX_ITEMS[] =
	{
		{ 0, "Common" },
		{ 1, "HOT" },
		{ 2, "HOTSELECTED" },
		{ 3, "SELECTED" },
		{ 4, "SELECTEDNOTFOCUS" },
	};

	static const StateMap STATES_LISTVIEW_ITEMS[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "SELECTED" },
		{ 4, "DISABLED" },
		{ 5, "SELECTEDNOTFOCUS" },
		{ 6, "HOTSELECTED" },
	};

	static const StateMap STATES_LISTVIEW_GROUPHEADER_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "OPEN" },
		{ 2, "OPENHOT" },
		{ 3, "OPENSELECTED" },
		{ 4, "OPENSELECTEDHOT" },
		{ 5, "OPENSELECTEDNOTFOCUSED" },
		{ 6, "OPENSELECTEDNOTFOCUSEDHOT" },
		{ 7, "OPENMIXEDSELECTION" },
		{ 8, "OPENMIXEDSELECTIONHOT" },
		{ 9, "CLOSE" },
		{ 10, "CLOSEHOT" },
		{ 11, "CLOSESELECTED" },
		{ 12, "CLOSESELECTEDHOT" },
		{ 13, "CLOSESELECTEDNOTFOCUSED" },
		{ 14, "CLOSESELECTEDNOTFOCUSEDHOT" },
		{ 15, "CLOSEMIXEDSELECTION" },
		{ 16, "CLOSEMIXEDSELECTIONHOT" },
	};

	static const StateMap STATES_LISTVIEW_EXPCOLLAPSE[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOVER" },
		{ 3, "PUSHED" },
	};

	static const StateMap STATES_MENU_BARBG[] =
	{
		{ 0, "Common" },
		{ 1, "ACTIVE" },
		{ 2, "INACTIVE" },
	};

	static const StateMap STATES_MENU_BARITEM[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PUSHED" },
		{ 4, "DISABLED" },
		{ 5, "DISABLEDHOT" },
		{ 6, "DISABLEDPUSHED" },
	};

	static const StateMap STATES_MENU_POPCHECK[] =
	{
		{ 0, "Common" },
		{ 1, "CHECKMARKNORMAL" },
		{ 2, "CHECKMARKDISABLED" },
		{ 3, "BULLETNORMAL" },
		{ 4, "BULLETDISABLED" },
	};

	static const StateMap STATES_MENU_POPCHECKBG[] =
	{
		{ 0, "Common" },
		{ 1, "DISABLED" },
		{ 2, "NORMAL" },
		{ 3, "BITMAP" },
	};

	static const StateMap STATES_MENU_POPITEMS[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "DISABLED" },
		{ 4, "DISABLEDHOT" },
	};

	static const StateMap STATES_MENU_SYSTEM_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "DISABLED" },
	};

	static const StateMap STATES_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "BB_NORMAL" },
		{ 2, "BB_HOT" },
		{ 3, "BB_PRESSED" },
		{ 4, "BB_DISABLED" },
	};

	static const StateMap STATES_PROGRESS_TRANSPARENT_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "PARTIAL" },
	};

	static const StateMap STATES_PROGRESS_FILL_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "ERROR" },
		{ 3, "PAUSED" },
		{ 4, "PARTIAL" },
	};

	static const StateMap STATES_REBAR_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
	};

	static const StateMap STATES_SCROLLBAR_ARROWBTN[] =
	{
		{ 0, "Common" },
		{ 1, "UPNORMAL" },
		{ 2, "UPHOT" },
		{ 3, "UPPRESSED" },
		{ 4, "UPDISABLED" },
		{ 5, "DOWNNORMAL" },
		{ 6, "DOWNHOT" },
		{ 7, "DOWNPRESSED" },
		{ 8, "DOWNDISABLED" },
		{ 9, "LEFTNORMAL" },
		{ 10, "LEFTHOT" },
		{ 11, "LEFTPRESSED" },
		{ 12, "LEFTDISABLED" },
		{ 13, "RIGHTNORMAL" },
		{ 14, "RIGHTHOT" },
		{ 15, "RIGHTPRESSED" },
		{ 16, "RIGHTDISABLED" },
		{ 17, "UPHOVER" },
		{ 18, "DOWNHOVER" },
		{ 19, "LEFTHOVER" },
		{ 20, "RIGHTHOVER" },

	};

	static const StateMap STATES_SCROLLBAR_STYLE[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
		{ 5, "HOVER" },
	};

	static const StateMap STATES_SCROLLBAR_SIZEBOX[] =
	{
		{ 0, "Common" },
		{ 1, "RIGHTALIGN" },
		{ 2, "LEFTALIGN" },
		{ 3, "TOPRIGHTALIGN" },
		{ 4, "TOPLEFTALIGN" },
		{ 5, "HALFBOTTOMRIGHTALIGN" },
		{ 6, "HALFBOTTOMLEFTALIGN" },
		{ 7, "HALFTOPRIGHTALIGN" },
		{ 8, "HALFTOPLEFTALIGN" },
	};


	static const StateMap STATES_SPIN_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "Normal" },
		{ 2, "Hot" },
		{ 3, "Pressed" },
		{ 4, "Disabled" },
	};

	static const StateMap STATES_TABITEM_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "SELECTED" },
		{ 4, "DISABLED" },
		{ 5, "FOCUSED" },
	};

	static const StateMap STATES_TASKDLG_EXPANDOBUTTON[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOVER" },
		{ 3, "PRESSED" },
		{ 4, "EXPANDEDNORMAL" },
		{ 5, "EXPANDEDHOVER" },
		{ 6, "EXPANDEDPRESSED" },
		{ 7, "NORMALDISABLED" },
		{ 8, "EXPANDEDDISABLED" },
	};

	static const StateMap STATES_TEXTSTYLE_HLINK[] =
	{
		{ 0, "Common" },
		{ 1, "HYPERLINK_NORMAL" },
		{ 2, "HYPERLINK_HOT" },
		{ 3, "HYPERLINK_PRESSED" },
		{ 4, "HYPERLINK_DISABLED" },
	};

	static const StateMap STATES_TEXTSTYLE_CTRLLABEL[] =
	{
		{ 0, "Common" },
		{ 1, "CONTROLLABEL_NORMAL" },
		{ 2, "CONTROLLABEL_DISABLED" },
	};

	static const StateMap STATES_TOOLBARSTYLE[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "DISABLED" },
		{ 5, "CHECKED" },
		{ 6, "HOTCHECKED" },
		{ 7, "NEARHOT" },
		{ 8, "OTHERSIDEHOT" },

	};

	static const StateMap STATES_TOOLTIP_CLOSE[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
	};

	static const StateMap STATES_TOOLTIP_BALLOON_AND_STANDARD[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "LINK" },
	};

	static const StateMap STATES_TOOLTIP_BALLOONSTEM[] =
	{
		{ 0, "Common" },
		{ 1, "POINTINGUPLEFTWALL" },
		{ 2, "POINTINGUPCENTERED" },
		{ 3, "POINTINGUPRIGHTWALL" },
		{ 4, "POINTINGDOWNRIGHTWALL" },
		{ 5, "POINTINGDOWNCENTERED" },
		{ 6, "POINTINGDOWNLEFTWALL" },
	};

	static const StateMap STATES_TOOLTIP_WRENCH[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
	};

	static const StateMap STATES_TRACKBAR_GENERAL[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },

	};

	static const StateMap STATES_TRACKBAR_THUMB_GEN[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
		{ 4, "FOCUSED" },
		{ 5, "DISABLED" },
	};

	static const StateMap STATES_TREEVIEW_ITEM[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "SELECTED" },
		{ 4, "DISABLED" },
		{ 5, "SELECTEDNOTFOCUS" },
		{ 6, "HOTSELECTED" },
	};

	static const StateMap STATES_TREEVIEW_GLYPH[] =
	{
		{ 0, "Common" },
		{ 1, "CLOSED" },
		{ 2, "OPENED" },
	};


	static const StateMap HOVERBACKGROUNDSTATES[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PRESSED" },
	};

	static const StateMap STATES_GRIPPER[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "CENTERED" },
	};

	static const StateMap STATES_WINDOW_FRAME_GEN[] =
	{
		{ 0, "Common" },
		{ 1, "ACTIVE" },
		{ 2, "INACTIVE" },
	};

	static const StateMap STATES_WINDOW_CAPTION_GEN[] =
	{
		{ 0, "Common" },
		{ 1, "ACTIVE" },
		{ 2, "INACTIVE" },
		{ 3, "DISABLED" },
	};

	static const StateMap STATES_WINDOW_BTN_AND_THUMB[] =
	{
		{ 0, "Common" },
		{ 1, "NORMAL" },
		{ 2, "HOT" },
		{ 3, "PUSHED" },
		{ 4, "DISABLED" },
	};

	static const StateMap STATES_WINDOW_CAPTION_SMALL[] =
	{
		{ 0, "Common" },
		{ 1, "ACTIVE" },
		{ 2, "INACTIVE" },
		{ 3, "DISABLED" },
	};
}