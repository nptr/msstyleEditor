//////////////////////////////////////////////////////////////////////////////
///
/// @file propertyGrid.c
///
/// @brief A property grid control in Win32 SDK C.
///
/// @author David MacDermot
///
/// @par Comments:
///         This source is distributed in the hope that it will be useful,
///         but WITHOUT ANY WARRANTY; without even the implied warranty of
///         MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
/// 
/// @date 2-27-16
/// 
/// @todo 
///
/// @bug 
///
//////////////////////////////////////////////////////////////////////////////

//DWM 1.1: Version 1.1 changes labelled thus.
//DWM 1.2: Version 1.2 changes labelled thus.
//DWM 1.3: Version 1.3 changes labelled thus.
//DWM 1.4: Version 1.4 changes labelled thus.
//DWM 1.5: Version 1.5 changes labelled thus.
//DWM 1.6: Version 1.6 changes labelled thus.
//DWM 1.7: Version 1.7 changes labelled thus.
//DWM 1.8: Version 1.8 changes labelled thus.
//DWM 1.9: Version 1.9 changes labelled thus.

//DWM 1.9: Suppress POCC Warning "Argument x to 'sscanf' does not match the format string; 
//          expected 'unsigned char *' but found 'unsigned long'"
#ifdef __POCC__
#pragma warn(disable:2234)
#endif

#ifndef _WIN32_WINNT // Necessary for WM_MOUSEWHEEL support
#define _WIN32_WINNT 0x0500
#endif

#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <windowsx.h>
#include <commctrl.h>
#include <commdlg.h>
#include <shlobj.h>
#include <stdlib.h>
#include <stdio.h>
#include <wchar.h>
#include <tchar.h>
#include <uxtheme.h>
#include "propertyGrid.h"

#define ID_LISTBOX 2000   ///<An Id for the Listbox
#define ID_LISTMAP 2001   ///<An Id for the Listmap
#define ID_PROPDESC 2002  ///<An Id for the Property description
#define ID_EDIT 2003      ///<An Id for an editor
#define ID_BUTTON 2004    ///<An Id for the button
#define ID_IPEDIT 2005    ///<An Id for an editor
#define ID_DATE 2006      ///<An Id for an editor
#define ID_TIME 2007      ///<An Id for an editor
#define ID_COMBO 2008     ///<An Id for an editor
#define ID_EDITCOMBO 2009 ///<An Id for an editor

#define WIDTH_PART0 16         ///< Constant
#define WIDTH_PART2 20         ///< Constant
#define HEIGHT_DESC 80         ///< Constant
#define MINIMUM_ITEM_HEIGHT 20 ///< Constant

//DWM 1.2: Converted the following 4 items to constants
#define SELECT _T("T")         ///< PIT_CHECK select
#define UNSELECT _T("F")       ///< PIT_CHECK unselect
#define CHECKED SELECT         ///< PIT_CHECK checked 
#define UNCHECKED UNSELECT     ///< PIT_CHECK unchecked

//DWM 1.3: Added
#define WPRC _T("Wprc")

//DWM 1.8: Added
#define FLATCHECKS 0x01        ///< Draw flat checks flag

/// @name Macroes
/// @{

/// @def NELEMS(a)
///
/// @brief Computes number of elements of an array.
///
/// @param a An array.
#define NELEMS(a) (sizeof(a) / sizeof((a)[0]))

/// @def HEIGHT(rect)
///
/// @brief Given a RECT, Computes height.
///
/// @param rect A RECT struct.
#define HEIGHT(rect) ((LONG)(rect.bottom - rect.top))

/// @def WIDTH(rect)
///
/// @brief Given a RECT, Computes width.
///
/// @param rect A RECT struct.
#define WIDTH(rect) ((LONG)(rect.right - rect.left))

/// @def MAKE_PRECT(left, top, right, bottom)
///
/// @brief Declare an inline rect.
///
/// @param left The left coordinate.
/// @param top The top coordinate.
/// @param right The right coordinate.
/// @param bottom The bottom coordinate.
#define MAKE_PRECT(left, top, right, bottom) \
    (&(RECT) { (left), (top), (right), (bottom) })

/// @def ListBox_ItemFromPoint(hwndCtl, xPos, yPos)
///
/// @brief Gets the zero-based index of the item nearest the specified point
///         in a list box.
///
/// @param hwndCtl The handle of a listbox.
/// @param xPos The x coordinate of a point. 
/// @param yPos The y coordinate of a point.
///
/// @returns The return value contains the index of the nearest item
///           in the low-order word.  The high-order word is zero if
///           the specified point is in the client area of the list box,
///           or one if it is outside the client area.
#define ListBox_ItemFromPoint(hwndCtl, xPos, yPos) \
    (DWORD)SendMessage((hwndCtl),LB_ITEMFROMPOINT, \
        (WPARAM)0,MAKELPARAM((UINT)(xPos),(UINT)(yPos)))

/// @def Refresh(hwnd)
///
/// @brief Refresh a window.
///
/// @param hwnd The handle to the window.
#define Refresh(hwnd) RedrawWindow((hwnd), NULL, NULL, \
    RDW_ERASE|RDW_INVALIDATE|RDW_UPDATENOW)

/// @def AllocatedString_Replace(lpszTarget, lpszReplace)
///
/// @brief Replace an allocated string.
///
/// @param lpszTarget The existing allocated string.
/// @param lpszReplace The new string.
#define AllocatedString_Replace(lpszTarget, lpszReplace) \
    (free((lpszTarget)), (lpszTarget) = NewString(lpszReplace))

/// @}

#ifndef _tmemmove
	#ifdef _UNICODE
		#define _tmemmove wmemmove
	#else
		#define _tmemmove memmove
	#endif
#endif

#ifndef _tmemset
	#ifdef _UNICODE
		#define _tmemset wmemset
	#else
		#define _tmemset memset
	#endif
#endif

// not sure if "snprintf" is the pendant to "swprintf"
#ifdef _UNICODE
	#define _stprintf swprintf
#else
	#define _stprintf snprintf
#endif

#define ToolTip_AddTool(hwnd, pti) SendMessage(hwnd, TTM_ADDTOOL, 0, (LPARAM)pti)
#define ToolTip_EnumTools(hwnd, index, pti) SendMessage(hwnd, TTM_ENUMTOOLS, index, (LPARAM)pti)
#define ToolTip_UpdateTipText(hwnd, pti) SendMessage(hwnd, TTM_UPDATETIPTEXT, 0, (LPARAM)pti)
#define ToolTip_NewToolRect(hwnd, pti) SendMessage(hwnd, TTM_NEWTOOLRECT, 0, (LPARAM)pti)

LPCTSTR g_szClassName = _T("PropGridCtl"); ///< The classname.

/// @brief A nice assortment of custom colors.
static COLORREF g_CustomColors[] = {
    0xDCDCDC, 0xCDCDDB, 0xBDBED2, 0xE3C4C3,
    0xFFE0C0, 0xE0C8AB, 0xD7AFB0, 0xF0BFEB,
    0xE2A7DA, 0x9697FF, 0xBFBFFF, 0xBBDBFF,
    0xCBFFFF, 0xBFEFCB, 0xA6DEB3, 0x82D28B
};

/****************************************************************************/
//Functions
static LRESULT CALLBACK Grid_Proc(HWND, UINT, WPARAM, LPARAM);
static LRESULT CALLBACK ListBox_Proc(HWND, UINT, WPARAM, LPARAM);
static VOID Grid_NotifyParent(VOID);

/// @brief Default window procedure for the grid and child windows.
///
/// @param hwnd Handle of grid or child.
/// @param msg Which message?
/// @param wParam Message parameter.
/// @param lParam Message parameter.
///
/// @returns LRESULT depends on message.
static LRESULT DefProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    //DWM 1.3: Added
    return CallWindowProc((WNDPROC)GetProp(hwnd, WPRC), hwnd, msg, wParam, lParam);
}

/// @brief Determine if this is running on XP or lower.
///
/// @returns TRUE if OS is XP or earlier.
static BOOL IsWindowsXPorLower(VOID) {
    //DWM 1.4: Added
    OSVERSIONINFO osvi;
    ZeroMemory(&osvi, sizeof(OSVERSIONINFO));
    osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
    GetVersionEx(&osvi);
    return osvi.dwMajorVersion < 6;
}

#pragma region Instance Data

/// @var LISTBOXITEM
/// @brief An item object

/// @var LPLISTBOXITEM
/// @brief Pointer to an item

/// @struct tagLISTBOXITEM
/// @brief This is the data associated with a grid item
typedef struct tagLISTBOXITEM {
    LPTSTR lpszCatalog; ///< Catalog (group) name
    LPTSTR lpszPropName;///< Property (item) name
    LPTSTR lpszMisc;    ///< Item specific data string
    LPTSTR lpszPropDesc;///< Property (item) description
    LPTSTR lpszCurValue;///< Property (item) value
    INT iItemType;      ///< Property (item) type identifier
    BOOL fCollapsed;    ///< Catalog (group) collapsed flag
} LISTBOXITEM , *LPLISTBOXITEM;

/// @var INSTANCEDATA
/// @brief Data for this instance of the control

/// @var LPINSTANCEDATA
/// @brief Pointer instance data

/// @struct tagINSTANCEDATA
/// @brief This is the data associated with an instance of the grid
typedef struct tagINSTANCEDATA {
    HINSTANCE hInstance;    ///< Handle to this instance
    LPLISTBOXITEM lpCurrent;///< Currently selected grid item
    HWND hwndParent;        ///< Handle of grid's parent
    HWND hwndToolTip;       ///< ToolTip handle
    HWND hwndPropDesc;      ///< Description pane handle
    HWND hwndListBox;       ///< Handle of the visible list box
    HWND hwndListMap;       ///< Handle of the hidden list box
    HWND hwndCtl1;          ///< Handle of the primary edit control
    HWND hwndCtl2;          ///< Handle of the secondary edit control
    INT iHDivider;          ///< Position of horizontal divider
    INT iVDivider;          ///< Position of verticle divider
    INT iDescHeight;        ///< Height of description pane
    INT iPrevSel;           ///< Index of previously selected item
    BOOL fXpOrLower;        ///< TRUE is running on Xp or earlier version of OS
    BOOL fGotFocus;         ///< TRUE while focus resides within property grid
    BOOL fScrolling;        ///< TRUE while scrolling
    BOOL fTracking;         ///< TRUE while moving dividers
    LONG nOldDivX;          ///< Previous divider x position
    LONG nOldDivY;          ///< Previous divider y position
    LONG nDivTop;           ///< Horizontal Divider positon limit
    LONG nDivBtm;           ///< Horizontal Divider positon limit
    LONG nDivLft;           ///< Vertical Divider positon limit
    LONG nDivRht;           ///< Vertical Divider positon limit
} INSTANCEDATA , *LPINSTANCEDATA;

static LPINSTANCEDATA g_lpInst; ///< instance data (this) pointer

/// @brief Get the Instance data associated with this instance.
///
/// @param hControl Handle to Current instance.
/// @param ppInstanceData - Pointer to the address of an INSTANCEDATA struct. 
///
/// @returns TRUE if successful
static BOOL Control_GetInstanceData(HWND hControl, LPINSTANCEDATA * ppInstanceData)
{
    *ppInstanceData = (LPINSTANCEDATA)GetProp(hControl, (LPCTSTR)_T("lpInsData"));
    if (NULL != *ppInstanceData)
        return TRUE;
    return FALSE;
}

/// @brief Allocate the Instance data associated with this instance.
///
/// @param hControl Handle to current instance.
/// @param pInstanceData Pointer to an INSTANCEDATA struct
///
/// @returns TRUE if successful
static BOOL Control_CreateInstanceData(HWND hControl, LPINSTANCEDATA pInstanceData)
{
    LPINSTANCEDATA pInst = (LPINSTANCEDATA)malloc(sizeof(INSTANCEDATA));
    memmove(pInst, pInstanceData, sizeof(INSTANCEDATA));
    return SetProp(hControl, (LPCTSTR)_T("lpInsData"), pInst);
}

/// @brief Free the instance data allocation of an instance of the Grid Control.
///
/// @param hControl Handle to current instance.
///
/// @returns TRUE if successful
static BOOL Control_FreeInstanceData(HWND hControl)
{
    LPINSTANCEDATA pInst;
    if (Control_GetInstanceData(hControl, &pInst))
    {
        free((LPINSTANCEDATA)pInst);
        RemoveProp(hControl, (LPCTSTR)_T("lpInsData"));
        return TRUE;
    }
    return FALSE;
}

/// @brief Get Item data associated with a property grid item.
///
/// @param hwnd Handle of a listbox.
/// @param idx The item index.
///
/// @returns a listbox item pointer if successful, otherwise NULL
static LPLISTBOXITEM ListBox_GetItemDataSafe(HWND hwnd, INT idx)
{
    LRESULT lres = ListBox_GetItemData(hwnd, idx);
    // Don't return LB_ERR cast to an item!
    if (LB_ERR == lres)
        return NULL;
    return (LPLISTBOXITEM)lres;
}

#pragma endregion Instance Data

#pragma region memory management

/// @brief Allocate and store a string.
///
/// @param str The string to store.
///
/// @returns a Pointer to the allocated string.
static LPTSTR NewString(LPTSTR str)
{
    if(NULL == str || _T('\0') == *str) str = _T("");
    LPTSTR tmp = (LPTSTR)calloc(_tcslen(str) + 1, sizeof(TCHAR));

    if(NULL == tmp)
    {
        return (LPTSTR)calloc(1, sizeof(TCHAR)); 
    }

    return _tmemmove(tmp, str, _tcslen(str));
}

/// @brief Allocate and store a string array (double-null-terminated string).
///
/// @param szzStr The double-null-terminated string to store.
///
/// @returns a Pointer to the allocated string array.
static LPTSTR NewStringArray(LPTSTR szzStr)
{
    if(NULL == szzStr || _T('\0') == *szzStr) szzStr = _T("");

    //Determine total buffer length
    INT iLen = 0;
    //Walk the buffer to find the terminating empty string.
    for (LPTSTR p = szzStr; *p; (p += _tcslen(p) + 1, iLen = p - szzStr)) ;

    //Allocate for array
    LPTSTR tmp = (LPTSTR)calloc(iLen + 1, sizeof(TCHAR));

    if(NULL == tmp)
    {
        return (LPTSTR)calloc(1, sizeof(TCHAR)); 
    }
    return _tmemmove(tmp, szzStr, iLen);
}

/// @brief Allocate and populate a list box item data structure.
///
/// @param szCatalog The catalog this item belongs to.
/// @param szPropName The item's name.
/// @param szCurValue The item's current value.
/// @param szMisc This data varies with item type.
/// @param szPropDesc The item's description string.
/// @param iItemType The item type designation.
///
/// @returns a Pointer to the allocated list box item.
static LPLISTBOXITEM NewItem(LPTSTR szCatalog, LPTSTR szPropName, LPTSTR szCurValue, LPTSTR szMisc, LPTSTR szPropDesc, INT iItemType)
{
    LPLISTBOXITEM lpItem = (LPLISTBOXITEM)calloc(1, sizeof(LISTBOXITEM));

    lpItem->iItemType = iItemType;
    lpItem->lpszCatalog = NewString(szCatalog);
    lpItem->lpszPropName = NewString(szPropName);
    lpItem->lpszCurValue = NewString(szCurValue);

    if (PIT_COMBO == iItemType || PIT_EDITCOMBO == iItemType)
        lpItem->lpszMisc = NewStringArray(szMisc);
    else
        lpItem->lpszMisc = NewString(szMisc);

    lpItem->lpszPropDesc = NewString(szPropDesc);

    return lpItem;
}

/// @brief Free a list box item's data structure.
///
/// @param lpItem A pointer to a LISTBOXITEM object.
///
/// @returns VOID.
static VOID DeleteItem(LPLISTBOXITEM lpItem)
{
    free(lpItem->lpszCatalog);
    free(lpItem->lpszPropName);
    if(PIT_CHECK != lpItem->iItemType)  //DWM 1.2: Don't attempt to free a constant
    {
        free(lpItem->lpszCurValue);
        free(lpItem->lpszMisc);
    }
    free(lpItem->lpszPropDesc);
    free(lpItem);
}

/// @brief Handle the WM_DELETEIETM message.
///
/// @param hwnd The handle of a list box (in this case the list map).
/// @param lpDeleteItem A pointer to the delete item struct.
///
/// @returns VOID.
static VOID Grid_OnDeleteItem(HWND hwnd, const DELETEITEMSTRUCT * lpDeleteItem)
{
    if (g_lpInst->hwndListMap == lpDeleteItem->hwndItem)
        DeleteItem((LPLISTBOXITEM)lpDeleteItem->itemData);

    // Catalogs only reside in visible list box
    if (g_lpInst->hwndListBox == lpDeleteItem->hwndItem)
    {
        if(PIT_CATALOG == ((LPLISTBOXITEM)lpDeleteItem->itemData)->iItemType)
            DeleteItem((LPLISTBOXITEM)lpDeleteItem->itemData);
    }
}

#pragma endregion memory management

#pragma region Drawing

/// @brief Pass keyboard focus to parent and refresh grid.
///
/// @returns VOID.
static VOID SetFocusToParent(VOID)
{
    SetFocus(g_lpInst->hwndParent);
}

/// @brief Handle WM_KILLFOCUS in editors.
///
/// @param hwnd Handle of the editor.
/// @param hwndNewFocus Handle of the window that recieved focus.
///
/// @returns VOID.
static VOID Editor_OnKillFocus(HWND hwnd, HWND hwndNewFocus)
{
    //DWM 1.3: Added so that grid selection is drawn inactive
    // when grid doesn't have focus.
    g_lpInst->fGotFocus =
        (NULL != hwndNewFocus &&
        (g_lpInst->hwndListBox  == hwndNewFocus ||
         g_lpInst->hwndCtl1     == hwndNewFocus || 
         g_lpInst->hwndCtl2     == hwndNewFocus ||
         g_lpInst->hwndPropDesc == hwndNewFocus ||
         g_lpInst->hwndToolTip  == hwndNewFocus));

    if(!g_lpInst->fGotFocus)
        Refresh(g_lpInst->hwndListBox);
}

/// @brief Handle WM_KILLFOCUS in listbox.
///
/// @param hwnd Handle of the editor.
/// @param hwndNewFocus Handle of the window that recieved focus.
///
/// @returns VOID.
static VOID ListBox_OnKillFocus(HWND hwnd, HWND hwndNewFocus)
{
    if (NULL != g_lpInst->lpCurrent)
    {
        if(PIT_CHECK == g_lpInst->lpCurrent->iItemType)//DWM 1.3: Added
            g_lpInst->lpCurrent->lpszMisc = UNSELECT;
    }
    Editor_OnKillFocus(hwnd, hwndNewFocus);
}

/// @brief Set a control's font to bold or back to normal.
///
/// @param hwndCtl The handle of a control.
/// @param fBold TRUE if bold desired.
///
/// @returns HFONT A new font with the desired font weight.
static HFONT Font_SetBold(HWND hwndCtl, BOOL fBold)
{
    HFONT hFont;
    LOGFONT lf;

    // Get a handle to the control's font object
    hFont = (HFONT)SendMessage(hwndCtl, WM_GETFONT, 0, 0);

    // Pull the handle into a Logical Font UDT type
    GetObject(hFont, sizeof(LOGFONT), &lf);

    // Determine if that font should be bold or not
    if (fBold)
        lf.lfWeight = FW_BOLD;
    else
        lf.lfWeight = FW_NORMAL;

    // Create a new font based off the logical font UDT
    return CreateFontIndirect(&lf);
}

/// @brief Draw a filled rectangle of a desired color.
///
/// @param hdc A handle to a device context.
/// @param lprc The address of a RECT structure with drawing coordinates.
/// @param clr The desired fill color value.
///
/// @returns VOID.
static VOID FillSolidRect(HDC hdc, LPRECT lprc, COLORREF clr)
{
    HBRUSH hbrush = CreateSolidBrush(clr);
    FillRect(hdc, lprc, hbrush);
    DeleteObject(hbrush);
}

/// @brief Draw a line.
///
/// @param hdc A handle to a device context.
/// @param x1 From point x-coordinate.
/// @param y1 From point y-coordinate.
/// @param x2 To point x-coordinate.
/// @param y2 To point y-coordinate.
///
/// @returns VOID.
static VOID DrawLine(HDC hdc, LONG x1, LONG y1, LONG x2, LONG y2)
{
    MoveToEx(hdc, x1, y1, NULL);
    LineTo(hdc, x2, y2);
}

/// @brief Draw an inverted line.
///
/// @param hdc A handle to a device context.
/// @param x1 From point x-coordinate.
/// @param y1 From point y-coordinate.
/// @param x2 To point x-coordinate.
/// @param y2 To point y-coordinate.
///
/// @returns VOID.
static VOID InvertLine(HDC hdc, LONG x1, LONG y1, LONG x2, LONG y2)
{
    INT nOldMode = SetROP2(hdc, R2_NOT);
    DrawLine(hdc, x1, y1, x2, y2);
    SetROP2(hdc, nOldMode);
}

/// @brief Draw specified borders of a rectangle.
///
/// @param hdc A handle to a device context.
/// @param lprc The address of a RECT structure with drawing coordinates.
/// @param dwBorder specifies which borders to draw.
/// @param clr The desired line color value.
///
/// @returns VOID.
static VOID DrawBorder(HDC hdc, LPRECT lprc, DWORD dwBorder, COLORREF clr)
{
    LOGPEN oLogPen;

    HPEN hOld;
    GetObject(hOld = SelectObject(hdc, GetStockObject(BLACK_PEN)), sizeof(oLogPen), &oLogPen);
    oLogPen.lopnColor = clr;

    SelectObject(hdc, CreatePenIndirect(&oLogPen));

    if (dwBorder & BF_LEFT)
        DrawLine(hdc, lprc->left, lprc->top, lprc->left, lprc->bottom);
    if (dwBorder & BF_TOP)
        DrawLine(hdc, lprc->left, lprc->top, lprc->right, lprc->top);
    if (dwBorder & BF_RIGHT)
        DrawLine(hdc, lprc->right, lprc->top, lprc->right, lprc->bottom);
    if (dwBorder & BF_BOTTOM)
        DrawLine(hdc, lprc->left, lprc->bottom, lprc->right, lprc->bottom);

    DeleteObject(SelectObject(hdc, hOld));
}

/// @brief Convert string value, Ex: "255 255 255" to RGB COLORREF.
///
/// @param src The string to convert.
///
/// @returns COLORREF The converted color.
static COLORREF GetColor(LPTSTR src)
{
    INT RVal, GVal, BVal;
    RVal = GVal = BVal = 0;
    if (_tcslen(src) > 0)
        _stscanf(src, _T("%d,%d,%d"), &RVal, &GVal, &BVal);

    return RGB(RVal, GVal, BVal);
}

/// @brief Handle the WM_PAINT message for most of the edit controls.
///         Obliterate the control's border to get that integrated flat look.
///
/// @param hwnd The control's handle.
/// @param msg The window message (in this case WM_PAINT).
/// @param wParam The message WPARAM.
/// @param lParam The message LPARAM.
///
/// @returns BOOL Always TRUE.
static BOOL Editor_OnPaint(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    HDC hdc = GetWindowDC(hwnd);
    RECT rect;

    // First let the system do its thing
    DefProc(hwnd, msg, wParam, lParam);

    // Next obliterate the border
    GetWindowRect(hwnd, &rect);
    MapWindowPoints(HWND_DESKTOP, hwnd, (LPPOINT) & rect.left, 2);

    rect.top += 2;
    rect.left += 2;
    DrawBorder(hdc, &rect, BF_RECT, GetSysColor(COLOR_WINDOW));

    rect.top += 1;
    rect.left += 1;
    rect.bottom += 1;
    rect.right += 1;
    
    DrawBorder(hdc, &rect, BF_RECT, GetSysColor(COLOR_WINDOW));

    ReleaseDC(hwnd, hdc);
    return TRUE;
}

/// @brief Set the colors used to paint controls in WM_CTLCOLORLISTBOX handler.
///
/// @param hdc Handle of a device context.
/// @param TxtColr Desired text color.
/// @param BkColr Desired back color.
///
/// @returns HBRUSH A reusable brush object.
static HBRUSH SetColor(HDC hdc, COLORREF TxtColr, COLORREF BkColr)
{
    static HBRUSH ReUsableBrush;
    DeleteObject(ReUsableBrush);
    ReUsableBrush = CreateSolidBrush(BkColr);
    SetTextColor(hdc, TxtColr);
    SetBkColor(hdc, BkColr);
    return ReUsableBrush;
}

#pragma endregion Drawing

#pragma region ToolTip

/// @brief Create the tool tip object that will display tips for grid items.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the tooltip's parent.
///
/// @returns HWND A handle to a tooltip object.
static HWND CreateToolTip(HINSTANCE hInstance, HWND hwndParent)
{
    RECT rect;
    TOOLINFO ti;
    DWORD dwStyle, dwExStyle;
    HWND hwnd;

    dwStyle = WS_POPUP | TTS_NOPREFIX | TTS_ALWAYSTIP;
    dwExStyle = WS_EX_TOPMOST;

    hwnd = CreateWindowEx(dwExStyle, // ex style
        TOOLTIPS_CLASS, // class name - defined in commctrl.h
        NULL,           // dummy text
        dwStyle,        // style
        CW_USEDEFAULT,  // x position
        CW_USEDEFAULT,  // y position
        CW_USEDEFAULT,  // width
        CW_USEDEFAULT,  // height
        hwndParent,     // parent
        NULL,           // ID
        hInstance,      // instance
        NULL);          // no extra data

    if (!hwnd)
        return NULL;

    SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);

    SendMessage(hwnd, WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT), 0);

    GetClientRect(hwndParent, &rect);

    // Initialize members of toolinfo structure
    ti.cbSize = sizeof(TOOLINFO);
    ti.uFlags = TTF_SUBCLASS;
    ti.hwnd = hwndParent;
    ti.hinst = hInstance;
    ti.uId = 0;
    ti.lpszText = _T("");
    // ToolTip control will cover the entire window
    ti.rect.left = rect.left;
    ti.rect.top = rect.top;
    ti.rect.right = rect.right;
    ti.rect.bottom = rect.bottom;
    ToolTip_AddTool(hwnd, &ti);

    return hwnd;
}

#pragma endregion ToolTip

#pragma region Static

/// @brief Create the static control for the property description pane.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the parent (the Property Grid window).
/// @param id An id tag for this control
///
/// @returns HWND A handle to a static control.
static HWND CreateStatic(HINSTANCE hInstance, HWND hwndParent, INT id)
{
    DWORD dwStyle, dwExStyle;
    HWND hwnd;

    dwStyle = WS_CHILD | SS_LEFT;

    dwExStyle = WS_EX_LEFT | WS_EX_CLIENTEDGE;

    hwnd = CreateWindowEx(dwExStyle,
        WC_STATIC,
        NULL,
        dwStyle,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        hwndParent,
        (HMENU)id,
        hInstance,
        NULL);
    if (!hwnd)
        return NULL;

    SendMessage(hwnd, WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT), 0);

    return hwnd;
}

#pragma endregion Static

#pragma region ListBox and ListMap

/// @brief Create the owner draw listbox for item display.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the parent (the Property Grid window).
/// @param id An id tag for this control
///
/// @par Comments:
///       There is some kind of bug where a listbox created as an
///       LBS_OWNERDRAWVARIABLE will scroll erratically when the mouse
///       wheel is used.
///
/// @returns HWND A handle to the owner draw listbox control.
static HWND CreateListBox(HINSTANCE hInstance, HWND hwndParent, INT id)
{
    DWORD dwStyle, dwExStyle;
    HWND hwnd;

    dwStyle = WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_VSCROLL | WS_HSCROLL |
                LBS_NOTIFY | LBS_OWNERDRAWFIXED |
                LBS_NOINTEGRALHEIGHT | LBS_WANTKEYBOARDINPUT;

    dwExStyle = WS_EX_LEFT | WS_EX_RTLREADING | WS_EX_RIGHTSCROLLBAR |
                WS_EX_CLIENTEDGE;
    hwnd = CreateWindowEx(dwExStyle,
        WC_LISTBOX,
        NULL,
        dwStyle,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        hwndParent,
        (HMENU)id,
        hInstance,
        NULL);

    if (!hwnd)
        return NULL;

    SendMessage(hwnd, WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT), 0);

    // Subclass listbox and save the old proc
    SetProp(hwnd, WPRC, (HANDLE)GetWindowLongPtr(hwnd, GWLP_WNDPROC));
    SubclassWindow(hwnd, ListBox_Proc);

    return hwnd;
}

/// @brief Create a minimal hidden listbox (the listmap) to store pointers
///          to all items, including those not displayed.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the parent (the Property Grid window).
/// @param id An id tag for this control
///
/// @returns HWND A handle to the hidden listmap control.
static HWND CreateListMap(HINSTANCE hInstance, HWND hwndParent, INT id)
{
    DWORD dwStyle, dwExStyle;
    HWND hwnd;

    dwStyle = WS_CHILD | LBS_OWNERDRAWFIXED;    // Want WM_DELETEITEM messages

    dwExStyle = 0;
    hwnd = CreateWindowEx(dwExStyle,
        WC_LISTBOX,
        NULL,
        dwStyle,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        hwndParent,
        (HMENU)id,
        hInstance,
        NULL);

    if (!hwnd)
        return NULL;

    return hwnd;
}

#pragma endregion ListBox and ListMap

#pragma region Edit Control

/// @brief Window procedure for the edit control.
///
/// @param hEdit Handle of editor.
/// @param msg Which message?
/// @param wParam Message parameter.
/// @param lParam Message parameter.
///
/// @returns LRESULT depends on message.
static LRESULT CALLBACK Edit_Proc(HWND hEdit, UINT msg, WPARAM wParam, LPARAM lParam)
{
    HWND hGParent = GetParent(GetParent(hEdit));

    // Note: Instance data is attached to Edit's grandparent
    Control_GetInstanceData(hGParent, &g_lpInst);

    if (WM_DESTROY == msg)  // Unsubclass the Edit Control
    {
        SetWindowLongPtr(hEdit, GWLP_WNDPROC, (DWORD_PTR)GetProp(hEdit, WPRC)); //DWM 1.5: fixed cast
        RemoveProp(hEdit, WPRC);
        return 0;
    }
    else if (WM_KILLFOCUS == msg)
    {
        FORWARD_WM_CHAR(hEdit, VK_RETURN, 0, SNDMSG);//DWM 1.6: force update of grid data
        Editor_OnKillFocus(hEdit, (HWND)wParam);
    }
    else if (WM_PAINT == msg)   // Obliterate border
    {
        return Editor_OnPaint(hEdit, msg, wParam, lParam);
    }
    else if (WM_MOUSEWHEEL == msg)
    {
        FORWARD_WM_CHAR(hEdit, VK_RETURN, 0, SNDMSG);
    }
    else if (WM_CHAR == msg && VK_RETURN == wParam)
    {
        if (NULL != g_lpInst->lpCurrent)
        {
            TCHAR buf[MAX_PATH];
            Edit_GetText(hEdit, buf, sizeof buf);
            AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, buf);
        }
        ShowWindow(hEdit, SW_HIDE);
        SetFocus(g_lpInst->hwndListBox);
        Grid_NotifyParent();
        return TRUE;    // handle Enter (NO BELL)
    }
    else if (WM_KEYDOWN == msg)
    {
        switch (wParam)
        {
            case VK_TAB:
                if (GetKeyState(VK_SHIFT) & 0x8000)
                {
                    FORWARD_WM_CHAR(hEdit, VK_RETURN, 0, SNDMSG);
                }
                else //DWM 1.3: Added Focus to grid parent
                {
                    SetFocusToParent();
                }
                return TRUE;
            case VK_ESCAPE:
                ShowWindow(hEdit, SW_HIDE);
                SetFocus(g_lpInst->hwndListBox);
                return FALSE;
        }
    }
    return DefProc(hEdit, msg, wParam, lParam);
}

/// @brief Create an Edit control to edit PIT_EDIT fields.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the parent (the visible listbox).
/// @param id An id tag for this control.
///
/// @returns HWND A handle to the edit control.
static HWND CreateEdit(HINSTANCE hInstance, HWND hwndParent, INT id)
{
    DWORD dwStyle, dwExStyle;
    HWND hwnd;

    dwStyle = WS_CHILD | ES_LEFT | ES_AUTOHSCROLL | ES_MULTILINE | ES_WANTRETURN;

    dwExStyle = WS_EX_LEFT | WS_EX_CLIENTEDGE;

    hwnd = CreateWindowEx(dwExStyle, WC_EDIT, TEXT(""), dwStyle, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, hwndParent, (HMENU)id, hInstance, NULL);

    if (!hwnd)
        return NULL;

    //DWM 1.4: Added Disable visual styles for this control.
    SetWindowTheme(hwnd, L" ", L" ");

    SendMessage(hwnd, WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT), 0L);

    // Subclass Editor and save the OldProc
    SetProp(hwnd, WPRC, (HANDLE)GetWindowLongPtr(hwnd, GWLP_WNDPROC));
    SubclassWindow(hwnd, Edit_Proc);

    return hwnd;
}

#pragma endregion Edit Control

#pragma region IP Control

/// @brief Window procedure for the ipedit control.
///
/// @param hwnd Handle of the ipedit or a child edit control.
/// @param msg Which message?
/// @param wParam Message parameter.
/// @param lParam Message parameter.
///
/// @par Comments
///       The ipedit control wraps four edit control children.
///       Each child needs to be subclassed in order to handle
///       keyboard events.  Each edit control posts some notification
///       to the parent ipedit control and I use this behavior to capture
///       and subclass these children.  These children are subclassed to
///       this very procedure and so 'hwnd' could be parent ipedit or one
///       of the child Edits.  Care then, must be taken to differentiate between
///       child and parent.  I do this by getting the class name and restricting
///       the handling of certain messages to one or the other control type.
///
/// @returns LRESULT depends on message.
static LRESULT CALLBACK IpEdit_Proc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    static TCHAR buf[MAX_PATH];
    HWND hGParent = GetParent(GetParent(hwnd));

    // Note: Instance data is attached to ipedit's grandparent
    //  or the edit field's greatgrandparent
    GetClassName(hwnd, buf, NELEMS(buf));
    BOOL fEdit = (0 == _tcsicmp(buf, WC_EDIT));

    if (fEdit)
        Control_GetInstanceData(GetParent(hGParent), &g_lpInst);
    else
        Control_GetInstanceData(hGParent, &g_lpInst);

    HWND hIpEdit = fEdit ? GetParent(hwnd) : hwnd;

    if (WM_DESTROY == msg)  //Unsubclass the ipedit or child edit control
    {
        SetWindowLongPtr(hwnd, GWLP_WNDPROC, (DWORD_PTR)GetProp(hwnd, WPRC)); //DWM 1.5: fixed cast
        RemoveProp(hwnd, WPRC);
        return 0;
    }
    else if (WM_KILLFOCUS == msg)//DWM 1.3: Added
    {
        if(fEdit)
        {
            // Determine if hwndNewfocus is a child of the IpEdit
            if(hIpEdit != GetParent((HWND) wParam)) //No, result of mouse click
            {
                FORWARD_WM_CHAR(hIpEdit, VK_RETURN, 0, SNDMSG);//DWM 1.6: force update of grid data
                Editor_OnKillFocus(hIpEdit, (HWND)wParam);
            }
        }
    }
    else if (WM_CHAR == msg && VK_RETURN == wParam)
    {
        if (NULL != g_lpInst->lpCurrent)
        {
			//DWM 1.9: Updated this code to use DWORD and IPADDRESS macroes instead of 4 byte array and cast.
            DWORD ip;
            TCHAR buf[MAX_PATH];
            if (4 == SNDMSG(hIpEdit, IPM_GETADDRESS, 0, (LPARAM) &ip))
            {
                _stprintf(buf, MAX_PATH, _T("%d.%d.%d.%d"), FIRST_IPADDRESS(ip), SECOND_IPADDRESS(ip), THIRD_IPADDRESS(ip), FOURTH_IPADDRESS(ip));
                AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, buf);
            }
        }
        ShowWindow(hIpEdit, SW_HIDE);
        SetFocus(g_lpInst->hwndListBox);
        Grid_NotifyParent();
        return TRUE;
    }
    if (fEdit)  //Handle keyboard events in the child edit controls
    {
        if (WM_GETDLGCODE == msg)
        {
            return DLGC_WANTALLKEYS;
        }
        else if (WM_MOUSEWHEEL == msg)
        {
            FORWARD_WM_CHAR(hwnd, VK_RETURN, 0, SNDMSG);
        }
        else if (WM_CHAR == msg && VK_TAB == wParam)
        {
            HWND hNext;
            if (GetKeyState(VK_SHIFT) & 0x8000) //Shift tab
            {
                hNext = GetWindow(hwnd, GW_HWNDNEXT);
                if (NULL == hNext) //DWM 1.3: Added Focus to Listbox
                {
                    SetFocus(g_lpInst->hwndListBox);
                    return TRUE;
                }
            }
            else
            {
                hNext = GetWindow(hwnd, GW_HWNDPREV);
                if (NULL == hNext) //DWM 1.3: Added Focus to grid parent
                {
                    SetFocusToParent();
                    Editor_OnKillFocus(hIpEdit, NULL);
                    return TRUE;
                }
            }
            Edit_SetSel(hNext, 0, -1);
            SetFocus(hNext);
            return TRUE;
        }
        else if (WM_CHAR == msg && VK_ESCAPE == wParam)
        {
            ShowWindow(hIpEdit, SW_HIDE);
            SetFocus(g_lpInst->hwndListBox);
            return TRUE;
        }
        else if (WM_PASTE == msg)
            return TRUE;    //Do not allow paste
    }
    else    //Handle messages (events) in the parent ipedit control
    {
        if (WM_PAINT == msg)    // Obliterate border
        {
            return Editor_OnPaint(hwnd, msg, wParam, lParam);
        }
        else if (WM_COMMAND == msg)
        {
            // Each of the control's edit fields posts notifications on showing,
            //  the first time they do so we'll grab and subclass them.
            HWND hwndCtl = GET_WM_COMMAND_HWND(wParam, lParam);
            {
                WNDPROC lpfn = (WNDPROC)GetProp(hwndCtl, WPRC);
                if (NULL == lpfn)
                {
                    //Subclass child and save the OldProc
                    SetProp(hwndCtl, WPRC, (HANDLE)GetWindowLongPtr(hwndCtl, GWLP_WNDPROC));
                    SubclassWindow(hwndCtl, IpEdit_Proc);
                }
            }
        }
    }
    return DefProc(hwnd, msg, wParam, lParam);
}

/// @brief Create an ipedit control to edit PIT_IP fields.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the parent (the visible listbox).
/// @param id An id tag for this control.
/// @param lprc A pointer to RECT struct that contains initial size data.
///
/// @par Comments
///      It is not possible to create this control with CW_USEDEFAULT for width
///      and height.  The control cannot be properly resized once created and
///      so it is necessary to specify width and height at the time of creation.
///
/// @returns HWND A handle to the ipedit control.
static HWND CreateIpEdit(HINSTANCE hInstance, HWND hwndParent, INT id, LPRECT lprc)
{
    INITCOMMONCONTROLSEX cc;
    cc.dwSize = sizeof(INITCOMMONCONTROLSEX);
    cc.dwICC = ICC_INTERNET_CLASSES;
    if (!InitCommonControlsEx(&cc))
        return NULL;

    DWORD dwStyle, dwExStyle;
    HWND hwnd;

    dwStyle = WS_CHILD;

    dwExStyle = WS_EX_LEFT;

    hwnd = CreateWindowEx(dwExStyle,
        WC_IPADDRESS,
        NULL,
        dwStyle,
        CW_USEDEFAULT, // x position can be changed after creation
        CW_USEDEFAULT, // y position can be changed after creation 
        lprc->right - lprc->left, // width can only be set here
        lprc->bottom - lprc->top, // height can only be set here
        hwndParent, (HMENU)id, hInstance, NULL);

    if (!hwnd)
        return NULL;

    SendMessage(hwnd, WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT), 0L);

    SetProp(hwnd, WPRC, (HANDLE)GetWindowLongPtr(hwnd, GWLP_WNDPROC));
    SubclassWindow(hwnd, IpEdit_Proc);

    return hwnd;
}

#pragma endregion IP Control

#pragma region Button Control

/// @brief Window procedure for the button control.
///
/// @param hButton Handle of button control.
/// @param msg Which message?
/// @param wParam Message parameter.
/// @param lParam Message parameter.
///
/// @returns LRESULT depends on message.
static LRESULT CALLBACK Button_Proc(HWND hButton, UINT msg, WPARAM wParam, LPARAM lParam)
{
    HWND hGParent = GetParent(GetParent(hButton));

    // Note: Instance data is attached to buttons's grandparent
    Control_GetInstanceData(hGParent, &g_lpInst);

    if (WM_DESTROY == msg)  // Unsubclass the button control
    {
        SetWindowLongPtr(hButton, GWLP_WNDPROC, (DWORD_PTR)GetProp(hButton, WPRC)); //DWM 1.5: fixed cast
        RemoveProp(hButton, WPRC);
        return 0;
    }
    else if (WM_KILLFOCUS == msg)
    {
        ShowWindow(hButton, SW_HIDE);
        Editor_OnKillFocus(hButton, (HWND) wParam);
    }
    else if (WM_MOUSEWHEEL == msg)
    {
        FORWARD_WM_KEYDOWN(hButton, VK_ESCAPE, 0, 0, SNDMSG);
    }
    else if (WM_GETDLGCODE == msg)
    {
        return DLGC_WANTALLKEYS;
    }
    else if (WM_KEYUP == msg && VK_RETURN == wParam)
    {
        FORWARD_WM_KEYUP(hButton, VK_SPACE, 0, 0, SNDMSG);
        return TRUE;
    }
    else if (WM_KEYDOWN == msg)
    {
        switch (wParam)
        {
            case VK_RETURN:
                FORWARD_WM_KEYDOWN(hButton, VK_SPACE, 0, 0, SNDMSG);
                return TRUE;
            case VK_TAB:
                if (GetKeyState(VK_SHIFT) & 0x8000)
                {
                    FORWARD_WM_KEYDOWN(hButton, VK_ESCAPE, 0, 0, SNDMSG);
                }
                else //DWM 1.3: Added Focus to grid parent
                {
                    ShowWindow(hButton, SW_HIDE);
                    SetFocusToParent();
                }
                return FALSE;
            case VK_ESCAPE:
            {
                ShowWindow(hButton, SW_HIDE);
                SetFocus(g_lpInst->hwndListBox);
                return FALSE;
            }
        }
    }
    return DefProc(hButton, msg, wParam, lParam);
}

/// @brief Create button control to launch dialogs.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the parent (the visible listbox).
/// @param id An id tag for this control.
///
/// @returns HWND A handle to the button control.
static HWND CreateButton(HINSTANCE hInstance, HWND hwndParent, INT id)
{
    DWORD dwStyle;
    HWND hwnd;

    dwStyle = WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON;

    hwnd = CreateWindowEx(0,
        WC_BUTTON,
        TEXT("..."),
        dwStyle,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        hwndParent,
        (HMENU)id,
        hInstance,
        NULL);

    if (!hwnd)
        return NULL;

    //DWM 1.4: Added Disable visual styles for this control.
    SetWindowTheme(hwnd, L" ", L" ");

    SendMessage(hwnd, WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT), 0L);

    SetProp(hwnd, WPRC, (HANDLE)GetWindowLongPtr(hwnd, GWLP_WNDPROC));
    SubclassWindow(hwnd, Button_Proc);

    return hwnd;
}

#pragma endregion Button Control

#pragma region Date Picker

/// @brief Window Procedure for the datepicker control.
///
/// @param hDate Handle of the datepicker.
/// @param msg Which message?
/// @param wParam Message parameter.
/// @param lParam Message parameter.
///
/// @par Comments
///       This procedure handles datepicker controls configured either as
///       date pickers or time pickers for three grid field types PIT_DATE,
///       PIT_TIME, and PIT_DATETIME.
///
/// @returns LRESULT depends on message.
static LRESULT CALLBACK DatePicker_Proc(HWND hDate, UINT msg, WPARAM wParam, LPARAM lParam)
{
    HWND hGParent = GetParent(GetParent(hDate));

    // Note: Instance data is attached to datepicker's grandparent
    Control_GetInstanceData(hGParent, &g_lpInst);

    if (WM_DESTROY == msg)  // Unsubclass the control
    {
        SetWindowLongPtr(hDate, GWLP_WNDPROC, (DWORD_PTR)GetProp(hDate, WPRC)); //DWM 1.5: fixed cast
        RemoveProp(hDate, WPRC);
        return 0;
    }
    else if (WM_KILLFOCUS == msg) //DWM 1.3: Added this
    {
        if (NULL != g_lpInst->lpCurrent)
        {
            if(PIT_DATETIME == g_lpInst->lpCurrent->iItemType)
            {
                HWND hFocus = (HWND)wParam;
                if(g_lpInst->hwndCtl1 == hFocus ||
                   g_lpInst->hwndCtl2 == hFocus) // ignore thise two windows
                    return 0;

                ShowWindow(g_lpInst->hwndCtl2, SW_HIDE);
            }
        }
        FORWARD_WM_CHAR(hDate, VK_RETURN, 0, SNDMSG);//DWM 1.6: force update of grid data
        Editor_OnKillFocus(hDate, (HWND)wParam);
    }
    else if (WM_PAINT == msg) // Obliterate border
    {
        //DWM 1.4: Changed this handler
        if(g_lpInst->fXpOrLower)
        {
            return Editor_OnPaint(hDate, msg, wParam, lParam);
        }
        else
        {
            // First let the system do its thing
            DefProc(hDate, msg, wParam, lParam);

            // Next obliterate the border
            HDC hdc = GetWindowDC(hDate);
            RECT rect;

            GetClientRect(hDate, &rect);

            DrawBorder(hdc, &rect, BF_TOPLEFT, GetSysColor(COLOR_WINDOW));
            rect.top += 1;
            rect.left += 1;

			//DWM 1.9: Added
#ifdef __POCC__
            rect.bottom -= 2;
#else
			rect.bottom += 2;
#endif
            DrawBorder(hdc, &rect, BF_RECT, GetSysColor(COLOR_WINDOW));

            ReleaseDC(hDate, hdc);

            return TRUE;
        }
    }
    else if (WM_MOUSEWHEEL == msg)
    {
        FORWARD_WM_CHAR(hDate, VK_RETURN, 0, SNDMSG);
    }
    else if (WM_GETDLGCODE == msg)
    {
        return DLGC_WANTALLKEYS;
    }
    else if (WM_CHAR == msg && VK_RETURN == wParam)
    {
        if (NULL != g_lpInst->lpCurrent)
        {
            switch (g_lpInst->lpCurrent->iItemType)
            {
                case PIT_DATE:
                {
                    SYSTEMTIME st = {0};
                    TCHAR buf[MAX_PATH] = {0};
                    DateTime_GetSystemtime(g_lpInst->hwndCtl1, &st);
                    GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE, &st, NULL, buf, MAX_PATH);
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, buf);
                    ShowWindow(g_lpInst->hwndCtl1, SW_HIDE);
                }
                    break;
                case PIT_TIME:
                {
                    SYSTEMTIME st = {0};
                    TCHAR buf[MAX_PATH] = {0};
                    DateTime_GetSystemtime(g_lpInst->hwndCtl1, &st);
                    GetTimeFormat(LOCALE_USER_DEFAULT, 0, &st, _T("hh':'mm':'ss tt"), buf, MAX_PATH);
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, buf);
                    ShowWindow(g_lpInst->hwndCtl1, SW_HIDE);
                }
                    break;
                case PIT_DATETIME:
                {
                    SYSTEMTIME st = {0};
                    TCHAR buf[MAX_PATH] = {0};
                    DateTime_GetSystemtime(g_lpInst->hwndCtl1, &st);
                    GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE, &st, NULL, buf, MAX_PATH);
                    _tcscat(buf, _T(" "));
                    DateTime_GetSystemtime(g_lpInst->hwndCtl2, &st);
                    GetTimeFormat(LOCALE_USER_DEFAULT, 0, &st, _T("hh':'mm':'ss tt"), (LPTSTR) (buf + _tcslen(buf)), MAX_PATH - _tcslen(buf));
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, buf);
                    ShowWindow(g_lpInst->hwndCtl1, SW_HIDE);
                    ShowWindow(g_lpInst->hwndCtl2, SW_HIDE);
                }
                    break;
            }
        }
        SetFocus(g_lpInst->hwndListBox);
        Grid_NotifyParent();
        return TRUE;    // handle Enter (NO BELL)
    }
    else if (WM_CHAR == msg && VK_TAB == wParam)
    {
        if (NULL != g_lpInst->lpCurrent)
        {
            if (PIT_DATETIME == g_lpInst->lpCurrent->iItemType)
            {
                //DWM 1.3: Fixed focus selection
                HWND hFocus = GetFocus();
                if(g_lpInst->hwndCtl1 == hFocus)
                {
                    if (GetKeyState(VK_SHIFT) & 0x8000)
                    {
                        FORWARD_WM_CHAR(hDate, VK_RETURN, 0, SNDMSG);
                    }
                    else
                    {
                        SetFocus(g_lpInst->hwndCtl2);
                    }
                }
                else if (g_lpInst->hwndCtl2 == hFocus)
                {
                    if (GetKeyState(VK_SHIFT) & 0x8000)
                    {
                        SetFocus(g_lpInst->hwndCtl1);
                    }
                    else
                    {
                        FORWARD_WM_CHAR(hDate, VK_RETURN, 0, SNDMSG);
                        SetFocusToParent();
                    }
                }
            }
            else
            {
                if (GetKeyState(VK_SHIFT) & 0x8000)
                {
                    FORWARD_WM_CHAR(hDate, VK_RETURN, 0, SNDMSG);
                }
                else
                {
                    FORWARD_WM_CHAR(hDate, VK_RETURN, 0, SNDMSG);
                    SetFocusToParent();
                }
            }
            return TRUE;
        }
    }
    else if (WM_KEYDOWN == msg && VK_ESCAPE == wParam)
    {
        if (NULL != g_lpInst->lpCurrent)
        {
            switch (g_lpInst->lpCurrent->iItemType)
            {
                case PIT_DATE:
                case PIT_TIME:
                    ShowWindow(g_lpInst->hwndCtl1, SW_HIDE);
                    break;
                case PIT_DATETIME:
                {
                    ShowWindow(g_lpInst->hwndCtl1, SW_HIDE);
                    ShowWindow(g_lpInst->hwndCtl2, SW_HIDE);
                }
                    break;
            }
        }
    }
    return DefProc(hDate, msg, wParam, lParam);
}

/// @brief Create datepicker control configured either as a date or time picker.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the parent (the visible listbox).
/// @param id An id tag for this control.
/// @param fDate TRUE to create a date picker, FALSE for time picker.
///
/// @returns HWND A handle to the control.
static HWND CreateDatePicker(HINSTANCE hInstance, HWND hwndParent, INT id, BOOL fDate)
{
    DWORD dwStyle, dwExStyle;
    HWND hwnd;

    dwStyle = WS_CHILD | (fDate ? DTS_SHORTDATEFORMAT : DTS_TIMEFORMAT);

    dwExStyle = WS_EX_LEFT;

    hwnd = CreateWindowEx(dwExStyle, DATETIMEPICK_CLASS, TEXT(""), dwStyle, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, hwndParent, (HMENU)id, hInstance, NULL);

    if (!hwnd)
        return NULL;

    //DWM 1.4: Added Disable visual styles for this control.
    SetWindowTheme(hwnd, L" ", L" ");

    SendMessage(hwnd, WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT), 0L);

    // Subclass date or timepicker and save the old proc
    SetProp(hwnd, WPRC, (HANDLE)GetWindowLongPtr(hwnd, GWLP_WNDPROC));
    SubclassWindow(hwnd, DatePicker_Proc);

    return hwnd;
}

#pragma endregion Date Picker

#pragma region Combo Box

/// @brief Window procedure for the combobox control.
///
/// @param hwnd Handle of the combobox or a child edit control.
/// @param msg Which message?
/// @param wParam Message parameter.
/// @param lParam Message parameter.
///
/// @par Comments
///       The combobox control wraps a listbox (drop down) and an edit control
///       child (if it is an editable list box).  The edit control child needs
///       to be subclassed in order to handle keyboard events.  Each child control
///       posts some notification to the parent combobox control and I use this
///       behavior to capture and subclass the child edit control.  This edit control is
///       subclassed to this very procedure and so 'hwnd' could be parent combobox or
///       child edit control.  Care then, must be taken to differentiate between
///       child and parent.  I do this by getting the class name and restricting
///       the handling of certain messages to one or the other control type.
/// @par Warning
///       The drop down list control of a combobox is not a child of the combobox
///       it is the child of the desktop so that the list is not clipped by the
///       combobox's client area.  Do not sub class it to this procedure since
///       doing so will cause the instance data pointer to be reset to NULL!  The
///       instance data pointer is attached to the PropertyGrid's window.
///
/// @returns LRESULT depends on message.
static LRESULT CALLBACK ComboBox_Proc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    static TCHAR classname[MAX_PATH];
    HWND hGParent = GetParent(GetParent(hwnd));

    // Note: Instance data is attached to combo's grandparent
    //  or the edit field's greatgrandparent
    GetClassName(hwnd, classname, NELEMS(classname));
    BOOL fEdit = (0 == _tcsicmp(classname, WC_EDIT));

    if (fEdit)
        Control_GetInstanceData(GetParent(hGParent), &g_lpInst);
    else
        Control_GetInstanceData(hGParent, &g_lpInst);

    if (WM_DESTROY == msg)  //Unsubclass the combobox or child edit control
    {
        SetWindowLongPtr(hwnd, GWLP_WNDPROC, (DWORD_PTR)GetProp(hwnd, WPRC)); //DWM 1.5: fixed cast
        RemoveProp(hwnd, WPRC);
        return 0;
    }
    else if (WM_KILLFOCUS == msg)//DWM 1.3: Added
    {
        HWND hFocus = (HWND) wParam;
        if(hwnd != GetParent(hFocus))// not a combobox editor or a combobox
        {
            FORWARD_WM_CHAR(hwnd, VK_RETURN, 0, SNDMSG);//DWM 1.6: force update of grid data
            Editor_OnKillFocus(hwnd, (HWND) wParam);
        }
    }
    else if (WM_PAINT == msg && !fEdit) // Obliterate border (differs from standard method)
    {
        // First let the system do its thing
        DefProc(hwnd, msg, wParam, lParam);

        // Next obliterate the border
        HDC hdc = GetWindowDC(hwnd);
        RECT rect;

        GetClientRect(hwnd, &rect);

        DrawBorder(hdc, &rect, BF_TOPLEFT, GetSysColor(COLOR_WINDOW));

        rect.top += 1;
        rect.left += 1;
        DrawBorder(hdc, &rect, BF_TOPLEFT, GetSysColor(COLOR_WINDOW));

        ReleaseDC(hwnd, hdc);
        return TRUE;
    }
    else if (WM_GETDLGCODE == msg)
    {
        return DLGC_WANTALLKEYS;
    }
    else if (WM_COMMAND == msg)
    {
        // The editable combo's edit box posts a notification on loading
        //  the first time it does so we'll grab and subclass it.
        HWND hwndCtl = GET_WM_COMMAND_HWND(wParam, lParam);
        {
            WNDPROC lpfn = (WNDPROC)GetProp(hwndCtl, WPRC);
            if (NULL == lpfn)
            {
                // Do not subclass the drop down list
                GetClassName(hwndCtl, classname, NELEMS(classname));
                if (0 == _tcsicmp(classname, WC_EDIT))
                {
                    //Subclass edit and save the old proc
                    SetProp(hwndCtl, WPRC, (HANDLE)GetWindowLongPtr(hwndCtl, GWLP_WNDPROC));
                    SubclassWindow(hwndCtl, ComboBox_Proc);
                }
            }
        }
    }
    else if (WM_CHAR == msg && VK_RETURN == wParam)
    {
        if (NULL != g_lpInst->lpCurrent)
        {
            GetWindowText(hwnd, classname, sizeof classname);   //Combo or child edit text is the same
            AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, classname);
        }
        if (fEdit)
            ShowWindow(GetParent(hwnd), SW_HIDE);
        else
            ShowWindow(hwnd, SW_HIDE);

        SetFocus(g_lpInst->hwndListBox);
        Grid_NotifyParent();
        return TRUE;    // handle Enter (NO BELL)
    }
    else if (WM_CHAR == msg && VK_TAB == wParam)
    {
        if (GetKeyState(VK_SHIFT) & 0x8000)
        {
            FORWARD_WM_CHAR(hwnd, VK_RETURN, 0, SNDMSG);
        }
        else //DWM 1.3: Added Focus to grid parent
        {
            ShowWindow(fEdit ? GetParent(hwnd) : hwnd, SW_HIDE);
            SetFocusToParent();
            Editor_OnKillFocus(hwnd, NULL);
        }
        return TRUE;
    }
    else if (WM_KEYDOWN == msg && VK_ESCAPE == wParam)
    {
        if (fEdit)
            ShowWindow(GetParent(hwnd), SW_HIDE);
        else
            ShowWindow(hwnd, SW_HIDE);

        SetFocus(g_lpInst->hwndListBox);
        return FALSE;
    }
    return DefProc(hwnd, msg, wParam, lParam);
}

/// @brief Create combobox control configured either as editable or static.
///
/// @param hInstance The handle of an instance.
/// @param hwndParent The handle of the parent (the visible listbox).
/// @param id An id tag for this control.
/// @param fEditable TRUE to create an editable combo, FALSE for static.
///
/// @returns HWND A handle to the control.
static HWND CreateCombo(HINSTANCE hInstance, HWND hwndParent, INT id, BOOL fEditable)
{
    DWORD dwStyle, dwExStyle;
    HWND hwnd;

    dwStyle = WS_CHILD | CBS_NOINTEGRALHEIGHT | (fEditable ? CBS_DROPDOWN : CBS_DROPDOWNLIST);

    dwExStyle = WS_EX_LEFT;

    hwnd = CreateWindowEx(dwExStyle,
        WC_COMBOBOX,
        NULL,
        dwStyle,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        hwndParent,
        (HMENU)id,
        hInstance,
        NULL);

    if (!hwnd)
        return NULL;

    //DWM 1.4: Disable visual styles for the time being since the combo looks bad
    // in this grid when drawn using the Vista and later styles.
    SetWindowTheme(hwnd, L" ", L" ");

    SendMessage(hwnd, WM_SETFONT, (WPARAM)GetStockObject(DEFAULT_GUI_FONT), 0L);

    SetProp(hwnd, WPRC, (HANDLE)GetWindowLongPtr(hwnd, GWLP_WNDPROC));
    SubclassWindow(hwnd, ComboBox_Proc);

    return hwnd;
}

#pragma endregion Combo Box

#pragma region choose font

/// @brief Create a string from a property grid font dialog item.
///
/// @param lpLogFontItem The address of a PROPGRIDFONTITEM struct.
/// @param lpszFont The string representation of PROPGRIDFONTITEM elements.
///
/// @returns VOID.
static VOID LogFontItem_FromString(LPPROPGRIDFONTITEM lpLogFontItem, LPTSTR lpszFont)
{
    LPLOGFONT lpLf = (LPLOGFONT)lpLogFontItem;
    _stscanf(lpszFont,
#ifdef _UNICODE
		_T( "Height: %d " \
			L"Width: %d " \
			L"Escapement: %d " \
			L"Orientation:  %d " \
			L"Weight: %d " \
			L"Italic: %hhd " \
			L"Underline: %hhd " \
			L"StrikeOut: %hhd " \
			L"CharSet: %hhd " \
			L"OutPrecision: %hhd " \
			L"ClipPrecision: %hhd " \
			L"Quality: %hhd " \
			L"PitchAndFamily: %hhd " \
			L"FaceName: %32l[^\r\n] " \
            L"Color: %d"),
#else
        _T( "Height: %d " \
            "Width: %d " \
            "Escapement: %d " \
            "Orientation:  %d " \
            "Weight: %d " \
            "Italic: %hhd " \
            "Underline: %hhd " \
            "StrikeOut: %hhd " \
            "CharSet: %hhd " \
            "OutPrecision: %hhd " \
            "ClipPrecision: %hhd " \
            "Quality: %hhd " \
            "PitchAndFamily: %hhd " \
            "FaceName: %32[^\r\n] " \
            "Color: %d"),
#endif
        &lpLf->lfHeight,
        &lpLf->lfWidth,
        &lpLf->lfEscapement,
        &lpLf->lfOrientation,
        &lpLf->lfWeight,
        &lpLf->lfItalic,
        &lpLf->lfUnderline,
        &lpLf->lfStrikeOut,
        &lpLf->lfCharSet,
        &lpLf->lfOutPrecision,
        &lpLf->lfClipPrecision,
        &lpLf->lfQuality,
        &lpLf->lfPitchAndFamily,
        (LPTSTR)&lpLf->lfFaceName,
        &lpLogFontItem->crFont);
}

/// @brief Create a string from a property grid font dialog item.
///
/// @param lpLogFontItem A pointer to a PROPGRIDFONTITEM.
///
/// @returns LPTSTR The string representation of PROPGRIDFONTITEM elements.
static LPTSTR LogFontItem_ToString(LPPROPGRIDFONTITEM lpLogFontItem)
{
    static TCHAR buf[MAX_PATH];
    _tmemset(buf, (TCHAR)0, MAX_PATH);

    LPLOGFONT lpLf = (LPLOGFONT)lpLogFontItem;
    _stprintf(buf, MAX_PATH,
#ifdef _UNICODE
		L"Height: %d\r\n" \
		L"Width: %d\r\n" \
		L"Escapement: %d\r\n" \
		L"Orientation:  %d\r\n" \
		L"Weight: %d\r\n" \
		L"Italic: %d\r\n" \
		L"Underline: %d\r\n" \
		L"StrikeOut: %d\r\n" \
		L"CharSet: %d\r\n" \
		L"OutPrecision: %d\r\n" \
		L"ClipPrecision: %d\r\n" \
		L"Quality: %d\r\n" \
		L"PitchAndFamily: %d\r\n" \
		L"FaceName: %ls\r\n" \
		L"Color: %d",
#else
        "Height: %d\r\n" \
		"Width: %d\r\n" \
		"Escapement: %d\r\n" \
		"Orientation:  %d\r\n" \
		"Weight: %d\r\n" \
		"Italic: %d\r\n" \
		"Underline: %d\r\n" \
		"StrikeOut: %d\r\n" \
		"CharSet: %d\r\n" \
		"OutPrecision: %d\r\n" \
		"ClipPrecision: %d\r\n" \
		"Quality: %d\r\n" \
		"PitchAndFamily: %d\r\n" \
		"FaceName: %s\r\n" \
		"Color: %d",
#endif
        lpLf->lfHeight,
        lpLf->lfWidth,
        lpLf->lfEscapement,
        lpLf->lfOrientation,
        lpLf->lfWeight,
        lpLf->lfItalic,
        lpLf->lfUnderline,
        lpLf->lfStrikeOut,
        lpLf->lfCharSet,
        lpLf->lfOutPrecision,
        lpLf->lfClipPrecision,
        lpLf->lfQuality,
        lpLf->lfPitchAndFamily,
        lpLf->lfFaceName,
        lpLogFontItem->crFont);
    return buf;
}

#pragma endregion choose font

#pragma region browse folder

/// @brief Create a string from a property grid file dialog item.
///
/// @param lpPgFdItem The address of a PROPGRIDFDITEM struct.
/// @param lpszFdItem The string representation of PROPGRIDFDITEM elements.
///
/// @returns VOID.
static VOID FileDialogItem_FromString(LPPROPGRIDFDITEM lpPgFdItem, LPTSTR lpszFdItem)
{
    //DWM 1.2: Added method
    static TCHAR PgFdItem[4][MAX_PATH];
    memset(PgFdItem, (TCHAR)0, sizeof(PgFdItem));

    _stscanf(lpszFdItem,
#ifdef _UNICODE
		L"Title: %256l[^\r\n] " \
		L"Path: %256l[^\r\n] " \
		L"Filter: %256l[^\r\n] " \
		L"Default Extension: %3ls",
#else
		"Title: %256[^\r\n] " \
		"Path: %256[^\r\n] " \
		"Filter: %256[^\r\n] " \
		"Default Extension: %3s",
#endif
        (LPTSTR)&PgFdItem[0], (LPTSTR)&PgFdItem[1], (LPTSTR)&PgFdItem[2], (LPTSTR)&PgFdItem[3]);

    for(int i = 0; i < NELEMS(PgFdItem); i++)
    {
        if (0 == _tcscmp(_T("?"), PgFdItem[i]))
        {
            //Convert back to empty string
            PgFdItem[i][0] = (TCHAR)0;
        }
        else if(2 == i)
        {
            //Convert back to double null-terminated string
            for (LPTSTR ptr = PgFdItem[2]; *ptr; ptr++)
                if (_T('\t') == *ptr) *ptr = _T('\0');
        }
    }
    lpPgFdItem->lpszDlgTitle = PgFdItem[0];
    lpPgFdItem->lpszFilePath = PgFdItem[1];
    lpPgFdItem->lpszFilter = PgFdItem[2];
    lpPgFdItem->lpszDefExt = PgFdItem[3];
}

/// @brief Create a string from a property grid file dialog item.
///
/// @param lpPgFdItem A pointer to a PROPGRIDFDITEM.
///
/// @returns LPTSTR The string representation of PROPGRIDFDITEM elements.
static LPTSTR FileDialogItem_ToString(LPPROPGRIDFDITEM lpPgFdItem)
{
    static TCHAR szBuf[3 * MAX_PATH];
    _tmemset(szBuf, (TCHAR)0, NELEMS(szBuf));

    TCHAR filter[MAX_PATH] = { 0 };

    //Copy filter string replacing \0 with \t"
    INT iLen = 0;
    for (LPTSTR psz = (LPTSTR)lpPgFdItem->lpszFilter,
          ps = filter, pe = filter + NELEMS(filter) - 1;
           *psz && ps < pe; psz += iLen + 1)
    {
        _tmemmove(ps, psz, (iLen = _tcslen(psz)));
        ps += iLen;
        *ps++ = _T('\t');
    }

    _stprintf(szBuf, NELEMS(szBuf),
#ifdef _UNICODE
		L"Title: %ls\r\n" \
		L"Path: %ls\r\n" \
		L"Filter: %ls\r\n" \
		L"Default Extension: %3ls",
#else
		"Title: %s\r\n" \
		"Path: %s\r\n" \
		"Filter: %s\r\n" \
		"Default Extension: %3s",
#endif
        0 < _tcslen(lpPgFdItem->lpszDlgTitle) ? lpPgFdItem->lpszDlgTitle : _T("?"), //DWM 1.2: Added default "?"
        0 < _tcslen(lpPgFdItem->lpszFilePath) ? lpPgFdItem->lpszFilePath : _T("?"), //DWM 1.2: Added default "?"
        0 < _tcslen(filter) ? filter : _T("?"), //DWM 1.2: Added default "?"
        0 < _tcslen(lpPgFdItem->lpszDefExt) ? lpPgFdItem->lpszDefExt : _T("?")); //DWM 1.2: Added default "?"
    return szBuf;
}

/// @brief Convert a CHAR string to a WCHAR string.
///
/// @param dest  Pointer to a buffer that will contain the converted string.
/// @param ccDest  destination buffer size (number of characters)
/// @param source  Constant pointer to a source string to be converted to WCHAR.
///
/// @returns VOID.
static VOID CharToWide(LPWSTR dest, INT ccDest, const LPSTR source)
{
	//DWM 1.7: Added
    int i = 0;

    while(source[i] != '\0' && i <= ccDest)
    {
        dest[i] = (WCHAR)source[i];
        ++i;
    }
}

/// @brief Convert a path to an item id list object.
///
/// @param pszPath The file path string.
///
/// @returns LPITEMIDLIST A pointer to an item id list object.
static LPITEMIDLIST ConvertPathToLpItemIdList(LPTSTR pszPath)
{
    LPITEMIDLIST pidl;
    LPSHELLFOLDER pDesktopFolder;
    ULONG chEaten;
    ULONG dwAttributes;

	//DWM 1.7: Added
#ifdef _UNICODE
	LPWSTR pwzPath = pszPath;
#else
	int iLen = strlen(pszPath);
	WCHAR pwzPath[iLen + 1];
	wmemset(pwzPath, (WCHAR)0, NELEMS(pwzPath));
	CharToWide(pwzPath, NELEMS(pwzPath), pszPath);
#endif

    if (SUCCEEDED(SHGetDesktopFolder(&pDesktopFolder)))
    {
        pDesktopFolder->lpVtbl->ParseDisplayName(pDesktopFolder,
         NULL, NULL, pwzPath, &chEaten, &pidl, &dwAttributes);
        pDesktopFolder->lpVtbl->Release(pDesktopFolder);
    }
    return pidl;
}

/// @brief An application-defined callback function used with the
///         SHBrowseForFolder function.
///
/// @param hwnd Handle of the dialog.
/// @param uMsg Which message?
/// @param lParam Message parameter.
/// @param lpData Pointer to message data.
///
/// @returns BOOL FALSE.
static BOOL CALLBACK BrowseCallbackProc(HWND hwnd, UINT uMsg, LPARAM lParam, LPARAM lpData)
{
    //Buffer for the folder dialog
    static TCHAR SelectedDir[MAX_PATH];

    switch (uMsg)
    {
        case BFFM_INITIALIZED:
        {
            // change the selected folder.
            SNDMSG(hwnd, BFFM_SETSELECTION, TRUE, lpData);
            break;
        }
        case BFFM_SELCHANGED:
        {
            // Set the status window to the currently selected path.
            if (SHGetPathFromIDList((LPITEMIDLIST)lParam, SelectedDir))
                SNDMSG(hwnd, BFFM_SETSTATUSTEXT, 0, (LPARAM)SelectedDir);
            break;
        }
        default:
            break;
    }
    return FALSE;
}

/// @brief Show the folder browser dialog to get a directory pathname.
///
/// @param hwnd Handle of the dialog's owner.
/// @param curPath The path of the currently selected folder.
/// @param title Browser dialog title.
/// @param rootPath Path location to begin browsing.
///
/// @returns BOOL Depends on message.
static LPTSTR BrowseFolder(HWND hwnd, LPTSTR curPath, LPTSTR title, LPTSTR rootPath)
{
    BROWSEINFO bi;
    static TCHAR szDir[MAX_PATH];
    LPITEMIDLIST pidl;
    LPMALLOC pMalloc;

    if (SUCCEEDED(SHGetMalloc(&pMalloc)))
    {
        memset(&bi, 0, sizeof(bi));
        bi.hwndOwner = hwnd;
        bi.pszDisplayName = 0;
        bi.lpszTitle = title;
        bi.pidlRoot = ConvertPathToLpItemIdList(rootPath);
        bi.ulFlags = BIF_RETURNONLYFSDIRS | BIF_STATUSTEXT;
        bi.lpfn = BrowseCallbackProc;
        bi.lParam = (long)curPath;

        pidl = SHBrowseForFolder(&bi);
        if (pidl)
            SHGetPathFromIDList(pidl, szDir);
        else
            _tmemset(szDir, (TCHAR)0, MAX_PATH);

        // free memory used
        pMalloc->lpVtbl->Free(pMalloc, pidl);
        pMalloc->lpVtbl->Release(pMalloc);
    }
    return szDir;
}

#pragma endregion browse folder

#pragma region list box message handlers

/// @brief Find the catalog item that matches the given catalog name.
///
/// @param hwnd Handle of the listbox.
/// @param indexStart Index to begin search.
/// @param szCatalog The catalog name.
///
/// @returns INT The index of the catalog otherwise LB_ERR.
static INT ListBox_FindCatalog(HWND hwnd, INT indexStart, LPCTSTR szCatalog)
{
    INT ln = _tcslen(szCatalog) + 1;
    if (ln == 1 || indexStart >= ListBox_GetCount(hwnd))
        return LB_ERR;

    for (INT i = indexStart < 0 ? 0 : indexStart; i < ListBox_GetCount(hwnd); i++)
    {
        LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(hwnd, i);
        if (NULL != pItem && 0 == _tcsicmp(pItem->lpszCatalog, szCatalog))
            return i;
    }
    return LB_ERR;
}

/// @brief Given the address of a LISTBOXITEM object of type PIT_CATALOG,
///         collapse it if it is expanded, or expand it if it is collapsed.
///
/// @param pItem Pointer to an LISTBOXITEM object.
///
/// @returns VOID.
static VOID ToggleCatalog(LPLISTBOXITEM pItem)
{
    if (NULL != pItem && PIT_CATALOG != pItem->iItemType)
        return;

    pItem->fCollapsed = !pItem->fCollapsed;

    if (pItem->fCollapsed)
    {
        for (INT i = ListBox_GetCount(g_lpInst->hwndListBox) - 1; i >= 0; i--)
        {
            LPLISTBOXITEM p = (LPLISTBOXITEM)ListBox_GetItemDataSafe(g_lpInst->hwndListBox, i);
            if (NULL != p && _tcsicmp(p->lpszCatalog, pItem->lpszCatalog) == 0)
            {
                if (PIT_CATALOG != p->iItemType)
                {
                    ListBox_DeleteString(g_lpInst->hwndListBox, i);
                }
            }
        }
    }
    else
    {
        INT idx = ListBox_FindCatalog(g_lpInst->hwndListBox, 0, pItem->lpszCatalog);
        if (LB_ERR != idx)
        {
            LPLISTBOXITEM pp;
            for (INT i = ListBox_GetCount(g_lpInst->hwndListMap) - 1; 0 <= i; i--)
            {
                pp = ListBox_GetItemDataSafe(g_lpInst->hwndListMap, i);
                if (NULL != pp->lpszCatalog)
                {
                    if (0 == _tcsicmp(pp->lpszCatalog, pItem->lpszCatalog))
                    {
                        if (PIT_CATALOG != pp->iItemType)
                        {
                            ListBox_InsertItemData(g_lpInst->hwndListBox, idx + 1, pp);
                        }
                    }
                }
            }
        }
    }
}

/// @brief Handle the begin scroll event of a listbox.
///
/// @param hwnd Handle of a listbox.
///
/// @par Comments:
///       This event is not raised by the listbox but is detected
///        by indirect means.
///
/// @see ListBox_Proc() WM_NCLBUTTONDOWN for details.
///
/// @returns VOID.
static VOID ListBox_OnBeginScroll(HWND hwnd)
{
    g_lpInst->fGotFocus = TRUE;//DWM 1:3: Added

    if (NULL == g_lpInst->lpCurrent)
        return;

    switch (g_lpInst->lpCurrent->iItemType)
    {
        case PIT_CHECK:
            g_lpInst->lpCurrent->lpszMisc = UNSELECT;
            break;
        case PIT_STATIC:
            break;
        case PIT_DATETIME:
            ShowWindow(g_lpInst->hwndCtl1, SW_HIDE);
            ShowWindow(g_lpInst->hwndCtl2, SW_HIDE);
            break;
        default:
            ShowWindow(g_lpInst->hwndCtl1, SW_HIDE);
    }
}

/// @brief Handle the end scroll event of a listbox.
///
/// @param hwnd Handle of a listbox.
///
/// @par Comments:
///       This event is not raised by the listbox but is detected
///        by indirect means.
///
/// @see ListBox_Proc() WM_SETCURSOR for details.
///
/// @returns VOID.
static VOID ListBox_OnEndScroll(HWND hwnd)
{
    if (NULL == g_lpInst->lpCurrent)
        return;

    RECT rect;
    memset(&rect, 0, sizeof rect);

    //Caculate the grid width
    ListBox_GetItemRect(hwnd, ListBox_GetCurSel(hwnd), &rect);

    rect.top += 1;
    rect.left = g_lpInst->iHDivider + 1;

    switch (g_lpInst->lpCurrent->iItemType)
    {
        case PIT_CATALOG:
        case PIT_STATIC:
            break; //Don't display anything
        case PIT_CHECK:
            g_lpInst->lpCurrent->lpszMisc = SELECT;
            RedrawWindow(hwnd, &rect, NULL, RDW_ERASE | RDW_INVALIDATE | RDW_UPDATENOW);
            break;
        case PIT_EDIT:
            if (NULL == g_lpInst->hwndCtl1)
            {
                SetFocus(g_lpInst->hwndListBox);
            }
            else //Display edit box
            {
                MoveWindow(g_lpInst->hwndCtl1, rect.left, rect.top, WIDTH(rect), HEIGHT(rect), TRUE);
                ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
                SetFocus(g_lpInst->hwndCtl1);
            }
            break;
        case PIT_COMBO:
        case PIT_EDITCOMBO:
        case PIT_IP:
        case PIT_DATE:
        case PIT_TIME:
            if (NULL == g_lpInst->hwndCtl1)
                SetFocus(g_lpInst->hwndListBox);
            else //Display editable combobox
            {
                MoveWindow(g_lpInst->hwndCtl1, rect.left, rect.top, WIDTH(rect), HEIGHT(rect), TRUE);
                ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
                SetFocus(g_lpInst->hwndCtl1);
            }
            break;
        case PIT_DATETIME:
            if (NULL == g_lpInst->hwndCtl1 || NULL == g_lpInst->hwndCtl2)
                SetFocus(g_lpInst->hwndListBox);
            else //Display date and time
            {
                RECT rect0, rect1;
                rect0 = rect1 = rect;
                rect0.right = rect0.left + (rect0.right - rect0.left) / 2;
                rect1.left = rect1.left + (rect1.right - rect1.left) / 2;
                MoveWindow(g_lpInst->hwndCtl1, rect0.left, rect0.top, WIDTH(rect0), HEIGHT(rect0), TRUE);
                MoveWindow(g_lpInst->hwndCtl2, rect1.left, rect1.top, WIDTH(rect1), HEIGHT(rect1), TRUE);
                ShowWindow(g_lpInst->hwndCtl2, SW_SHOW);
                ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
                SetFocus(g_lpInst->hwndCtl1);
            }
            break;
        default: //Button control
            if (NULL == g_lpInst->hwndCtl1)
            {
                SetFocus(g_lpInst->hwndListBox);
            }
            else //Display Button
            {
                if (WIDTH(rect) > 19)
                {
                    rect.left = rect.right - 19;
                    rect.right -= 2;
                }

                rect.top += 2;
                rect.bottom -= 2;

                MoveWindow(g_lpInst->hwndCtl1, rect.left, rect.top, WIDTH(rect), HEIGHT(rect), TRUE);

                ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
                SetFocus(g_lpInst->hwndCtl1);
            }
            break;
    }
}

/// @brief Handles WM_LBUTTONDOWN message in the listbox.
///
/// @param hwnd  Handle of listbox.
/// @param fDoubleClick TRUE if this is a double click event.
/// @param x The xpos of the mouse.
/// @param y The ypos of the mouse.
/// @param keyFlags Set if certain keys down at time of click.
///
/// @returns VOID.
static VOID ListBox_OnLButtonDown(HWND hwnd, BOOL fDoubleClick, INT x, INT y, UINT keyFlags)
{
    if ((x >= g_lpInst->iHDivider - 5) && (x <= g_lpInst->iHDivider + 5))
    {
        //If mouse clicked on divider line, then start resizing
        SetCursor(LoadCursor(NULL, IDC_SIZEWE));

        RECT rcWindow;
        GetWindowRect(hwnd, &rcWindow);
        rcWindow.left += WIDTH_PART0;
        rcWindow.right -= 10;
        //Do not let mouse leave the list box boundary
        ClipCursor(&rcWindow);

        RECT rcClient;
        GetClientRect(hwnd, &rcClient);

        g_lpInst->fTracking = TRUE;
        g_lpInst->nDivTop = rcClient.top;
        g_lpInst->nDivBtm = rcClient.bottom;
        g_lpInst->nOldDivX = x;

        HDC hdc = GetDC(hwnd);
        InvertLine(hdc, x, rcClient.top, x, rcClient.bottom);
        ReleaseDC(hwnd, hdc);

        if (NULL != g_lpInst->lpCurrent)
        {
            TCHAR buf[MAX_PATH] = {0};

            switch (g_lpInst->lpCurrent->iItemType)
            {
                case PIT_CATALOG:
                case PIT_STATIC:
                    break; //Ignore
                case PIT_CHECK:
                    g_lpInst->lpCurrent->lpszMisc = UNSELECT; //Prevent toggle
                    break;
                case PIT_DATETIME:
                    //DWM 1.6: force update of grid data
                    GetWindowText(g_lpInst->hwndCtl2,buf, sizeof buf);
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, buf);
                    ShowWindow(g_lpInst->hwndCtl2, SW_HIDE);
                // Fall through
                default:
                    //DWM 1.6: force update of grid data
                    GetWindowText(g_lpInst->hwndCtl1,buf, sizeof buf);
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, buf);
                    ShowWindow(g_lpInst->hwndCtl1, SW_HIDE);
                    break;
            }
        }

        //Capture the mouse
        SetCapture(hwnd);
    }
    else
    {
        if (fDoubleClick)
        {
            INT i = LOWORD(ListBox_ItemFromPoint(hwnd, x, y));
            LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(hwnd, i);
            if (NULL != pItem)
            {
                if (PIT_CATALOG == pItem->iItemType)
                    ToggleCatalog(pItem);
            }
        }
        g_lpInst->fTracking = FALSE;
    }
}

/// @brief Handles WM_LBUTTONUP message in the listbox.
///
/// @param hwnd  Handle of listbox.
/// @param x The xpos of the mouse.
/// @param y The ypos of the mouse.
/// @param keyFlags Set if certain keys down at time of click.
///
/// @returns VOID.
static VOID ListBox_OnLButtonUp(HWND hwnd, INT x, INT y, UINT keyFlags)
{
    if (g_lpInst->fTracking)
    {
        //If columns were being resized then this indicates
        // that mouse is up so resizing is done.  Need to redraw
        // columns to reflect their new widths.

        g_lpInst->fTracking = FALSE;
        //If mouse was captured then release it
        if (hwnd == GetCapture())
            ReleaseCapture();

        ClipCursor(NULL);

        HDC hdc = GetDC(hwnd);
        InvertLine(hdc, x, g_lpInst->nDivTop, x, g_lpInst->nDivBtm);
        ReleaseDC(hwnd, hdc);

        //Set the divider position to the new value
        g_lpInst->iHDivider = x;
        Refresh(hwnd);

        if (NULL != g_lpInst->lpCurrent)
        {
            switch (g_lpInst->lpCurrent->iItemType)
            {
                case PIT_CATALOG:
                case PIT_STATIC:
                case PIT_CHECK:
                    break; //Ignore
                case PIT_DATETIME:
                    ShowWindow(g_lpInst->hwndCtl2, SW_SHOW);
                //Fall through
                default:
                    ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
                    break;
            }
        }
    }
    else
    {
        INT i = LOWORD(ListBox_ItemFromPoint(hwnd, x, y));

        if (ListBox_GetCurSel(hwnd) != i)
        {
            ListBox_SetCurSel(hwnd, i);
        }
    }
    //Update the fields
    FORWARD_WM_CHAR(g_lpInst->hwndCtl1, VK_RETURN, 0, SNDMSG);
    FORWARD_WM_CHAR(g_lpInst->hwndCtl2, VK_RETURN, 0, SNDMSG);

    FORWARD_WM_COMMAND(GetParent(hwnd), GetDlgCtrlID(hwnd), hwnd, LBN_SELCHANGE, SNDMSG);

    if (NULL != g_lpInst->lpCurrent)
    {
        switch(g_lpInst->lpCurrent->iItemType)//DWM 1.3: Added switch
        {
            case PIT_CATALOG:
            case PIT_STATIC:
                return;
            case PIT_CHECK:
                if(0 == _tcsicmp(g_lpInst->lpCurrent->lpszMisc, SELECT))
                {
                    FORWARD_WM_KEYDOWN(hwnd, VK_SPACE, 0, 0, SNDMSG);
                    break;
                } // else fallthrough
            default:
                    FORWARD_WM_KEYDOWN(hwnd, VK_TAB, 0, 0, SNDMSG); //Focus to editor
        }
    }
}

/// @brief Handles WM_MOUSEMOVE message in the listbox.
///
/// @param hwnd  Handle of listbox.
/// @param x The xpos of the mouse.
/// @param y The ypos of the mouse.
/// @param keyFlags Set if certain keys down at time of move.
///
/// @returns VOID.
static VOID ListBox_OnMouseMove(HWND hwnd, INT x, INT y, UINT keyFlags)
{
    if (g_lpInst->fTracking)
    {
        //Move divider line to the mouse pos. if columns are
        // currently being resized
        HDC hdc = GetDC(hwnd);
        //Remove old divider line
        InvertLine(hdc, g_lpInst->nOldDivX, g_lpInst->nDivTop, g_lpInst->nOldDivX, g_lpInst->nDivBtm);
        //Draw new divider line
        InvertLine(hdc, x, g_lpInst->nDivTop, x, g_lpInst->nDivBtm);
        ReleaseDC(hwnd, hdc);

        g_lpInst->nOldDivX = x;
    }
    else if ((x >= g_lpInst->iHDivider - 5) && (x <= g_lpInst->iHDivider + 5))
    {
        //Set the cursor to a sizing cursor if the cursor is over the row divider
        SetCursor(LoadCursor(NULL, IDC_SIZEWE));
    }
    //
    //Set the tool tip text
    //
    if (NULL != g_lpInst->hwndToolTip)
    {
        TOOLINFO tiToolInfo;
        //Allocate a buf for lpszText
        TCHAR buf[80];
        TCHAR newText[80] = {0};
        tiToolInfo.lpszText = buf;
        tiToolInfo.cbSize = sizeof(TOOLINFO);

        //Populate TOOLINFO with the info for the tool (including old text)
        ToolTip_EnumTools(g_lpInst->hwndToolTip, 0, &tiToolInfo);

        if ((x >= g_lpInst->iHDivider + 5) && !g_lpInst->fTracking)
        {
            INT i = LOWORD(ListBox_ItemFromPoint(hwnd, x, y));
            LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(hwnd, i);
            if (NULL != pItem)
            {
                switch (pItem->iItemType)
                {
                    case PIT_FONT:
                    {
                        LPTSTR szFmt;
                        PROPGRIDFONTITEM pgfi;
                        LONG PointSize = 0;
                        LogFontItem_FromString(&pgfi, pItem->lpszCurValue);
                        HDC hDC = GetDC(hwnd);
                        PointSize = -MulDiv(pgfi.logFont.lfHeight, 72, GetDeviceCaps(hDC, LOGPIXELSY));
                        ReleaseDC(hwnd, hDC);
#ifdef _UNICODE
                        szFmt = _T("%ls %d");
#else
                        szFmt = _T("%s %d");
#endif
                        //Replace the text in the buf and update the tool
                        _stprintf(newText, NELEMS(buf), szFmt, pgfi.logFont.lfFaceName, PointSize);
                    }
                        break;
                    case PIT_CHECK: //Skip this item
                        break;
                    default:
                        //Replace the text in the buf and update the tool
                        _tcsncpy(newText, pItem->lpszCurValue, NELEMS(newText) - 1);
                        break;
                }
            }
        }
        if(0 != _tcsncmp(buf, newText, _tcslen(newText))) //DWM 1.4: Do not update unless text changed to reduce flicker
        {
            //Clear out old text
            _tmemset(buf, (TCHAR)0, NELEMS(buf)-1);
            //Replace the text in the buf and update the tool
            _tcsncpy(buf, newText, NELEMS(buf) - 1);
            ToolTip_UpdateTipText(g_lpInst->hwndToolTip, &tiToolInfo);
        }
    }
}

/// @brief Handle the selection of a listbox item of type PIT_EDIT.
///
/// @param hwnd The handle of the listbox.
/// @param rc RECT containing desired coordinates for the edit control.
/// @param pItem Pointer to a LISTBOXITEM object.
///
/// @returns VOID.
static VOID ListBox_OnSelectEdit(HWND hwnd, RECT rc, LPLISTBOXITEM pItem)
{
    rc.top += 1;

    //display edit box
    if (NULL == g_lpInst->hwndCtl1)
        g_lpInst->hwndCtl1 = CreateEdit(g_lpInst->hInstance, hwnd, ID_EDIT);

    MoveWindow(g_lpInst->hwndCtl1, rc.left, rc.top, WIDTH(rc), HEIGHT(rc), TRUE);

    //Set the text in the edit box to the property's current value
    Edit_SetText(g_lpInst->hwndCtl1, pItem->lpszCurValue);
    ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
    SetFocus(g_lpInst->hwndCtl1);
    Edit_SetSel(g_lpInst->hwndCtl1, 0, -1);
}

/// @brief Handle the selection of a listbox item of type PIT_IP.
///
/// @param hwnd The handle of the listbox.
/// @param rc RECT containing desired coordinates for the ipedit control.
/// @param pItem Pointer to a LISTBOXITEM object.
///
/// @returns VOID.
static VOID ListBox_OnSelectIP(HWND hwnd, RECT rc, LPLISTBOXITEM pItem)
{
    rc.top += 1;

    if (NULL == g_lpInst->hwndCtl1)
        g_lpInst->hwndCtl1 = CreateIpEdit(g_lpInst->hInstance, hwnd, ID_IPEDIT, &rc);

    MoveWindow(g_lpInst->hwndCtl1, rc.left, rc.top, WIDTH(rc), HEIGHT(rc), TRUE);

	//DWM 1.9: Updated this code to use DWORD array and MAKEIPADDRESS macroe instead of 4 byte array and cast.
	//DWM 1.9: This was necessary to support MSVC _stscanf (sscanf - swscanf) beahavior which expects DWORD arguments.
    DWORD ip[4] = { 0, 0, 0, 0 };
    _stscanf(pItem->lpszCurValue, _T("%hhu.%hhu.%hhu.%hhu"), &ip[0], &ip[1], &ip[2], &ip[3]);

    SNDMSG(g_lpInst->hwndCtl1, IPM_SETADDRESS, 0, MAKEIPADDRESS(ip[0],ip[1],ip[2],ip[3]));

    ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);

    SetFocus(g_lpInst->hwndCtl1);
}

/// @brief Handle the selection of a listbox item of the following types:
///         PIT_COLOR, PIT_FONT, PIT_FILE, and PIT_FOLDER.
///
/// @param hwnd The handle of the listbox.
/// @param rc RECT containing desired coordinates for the button control.
///
/// @returns VOID.
static VOID ListBox_OnDisplayButton(HWND hwnd, RECT rc)
{
    if (WIDTH(rc) > 19)
    {
        rc.left = rc.right - 19;
        rc.right -= 2;
    }

    rc.top += 2;
    rc.bottom -= 2;

    if (NULL == g_lpInst->hwndCtl1)
        g_lpInst->hwndCtl1 = CreateButton(g_lpInst->hInstance, hwnd, ID_BUTTON);

    MoveWindow(g_lpInst->hwndCtl1, rc.left, rc.top, WIDTH(rc), HEIGHT(rc), TRUE);

    ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
    SetFocus(g_lpInst->hwndCtl1);
}

/// @brief Handle the selection of a listbox item of the following types:
///         PIT_DATE, PIT_TIME, and PIT_DATETIME.
///
/// @param hwnd The handle of the listbox.
/// @param rc RECT containing desired coordinates for the date or timepicker control(s).
/// @param pItem Pointer to a LISTBOXITEM object.
///
/// @returns VOID.
static VOID ListBox_OnSelectDateTime(HWND hwnd, RECT rc, LPLISTBOXITEM pItem)
{
    rc.top += 1;

    LPTSTR szFormat;
    TCHAR buf[3];
    _tmemset(buf, (TCHAR)0, 3);

    SYSTEMTIME st;
    memset(&st, 0, sizeof(SYSTEMTIME));

    if (PIT_DATE == pItem->iItemType)
    {
        if (NULL == g_lpInst->hwndCtl1)
            g_lpInst->hwndCtl1 = CreateDatePicker(g_lpInst->hInstance, hwnd, ID_DATE, TRUE);

        MoveWindow(g_lpInst->hwndCtl1, rc.left, rc.top, WIDTH(rc), HEIGHT(rc), TRUE);

        if (NULL != pItem)
            _stscanf(pItem->lpszCurValue, _T("%hd/%hd/%hd"), &st.wMonth, &st.wDay, &st.wYear);

        DateTime_SetSystemtime(g_lpInst->hwndCtl1, GDT_VALID, &st);

        ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
        SetFocus(g_lpInst->hwndCtl1);
    }
    else if (PIT_TIME == pItem->iItemType)
    {
        if (NULL == g_lpInst->hwndCtl1)
            g_lpInst->hwndCtl1 = CreateDatePicker(g_lpInst->hInstance, hwnd, ID_TIME, FALSE);

        MoveWindow(g_lpInst->hwndCtl1, rc.left, rc.top, WIDTH(rc), HEIGHT(rc), TRUE);

#ifdef _UNICODE
        szFormat = _T("%hd:%hd:%hd %2ls");
#else
        szFormat = _T("%hd:%hd:%hd %2s");
#endif

        if (NULL != pItem)
        {
            //DWM 1.6: Initialize unused date portion so DTP doesn't default to current date time
            GetLocalTime(&st);
            _stscanf(pItem->lpszCurValue, szFormat, &st.wHour, &st.wMinute, &st.wSecond, &buf);
        }
        if ((0 == _tcsicmp(_T("PM"), buf)) && st.wHour != 12)//DWM 1.6:Added st.wHour != 12
            st.wHour += 12;

        DateTime_SetSystemtime(g_lpInst->hwndCtl1, GDT_VALID, &st);

        ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
        SetFocus(g_lpInst->hwndCtl1);
    }
    else if (PIT_DATETIME == pItem->iItemType)
    {
        RECT rect0, rect1;
        rect0 = rect1 = rc;
        rect0.right = rect0.left + (rect0.right - rect0.left) / 2;
        rect1.left = rect1.left + (rect1.right - rect1.left) / 2;

        if (NULL == g_lpInst->hwndCtl1)
            g_lpInst->hwndCtl1 = CreateDatePicker(g_lpInst->hInstance, hwnd, ID_DATE, TRUE);

        MoveWindow(g_lpInst->hwndCtl1, rect0.left, rect0.top, WIDTH(rect0), HEIGHT(rect0), TRUE);

        if (NULL == g_lpInst->hwndCtl2)
            g_lpInst->hwndCtl2 = CreateDatePicker(g_lpInst->hInstance, hwnd, ID_TIME, FALSE);

        MoveWindow(g_lpInst->hwndCtl2, rect1.left, rect1.top, WIDTH(rect1), HEIGHT(rect1), TRUE);

#ifdef _UNICODE
        szFormat = _T("%hd/%hd/%hd %hd:%hd:%hd %2ls");
#else
        szFormat = _T("%hd/%hd/%hd %hd:%hd:%hd %2s");
#endif

        if (NULL != pItem)
            _stscanf(pItem->lpszCurValue, szFormat, &st.wMonth, &st.wDay, &st.wYear, &st.wHour, &st.wMinute, &st.wSecond, &buf);

        if ((0 == _tcsicmp(_T("PM"), buf)) && st.wHour != 12)//DWM 1.6:Added st.wHour != 12
            st.wHour += 12;

        DateTime_SetSystemtime(g_lpInst->hwndCtl1, GDT_VALID, &st);
        DateTime_SetSystemtime(g_lpInst->hwndCtl2, GDT_VALID, &st);

        ShowWindow(g_lpInst->hwndCtl1, SW_SHOW);
        ShowWindow(g_lpInst->hwndCtl2, SW_SHOW);
        SetFocus(g_lpInst->hwndCtl1);
    }
}

/// @brief Handle the selection of a listbox item of the following types:
///         PIT_COMBO, and PIT_EDITCOMBO.
///
/// @param hwnd The handle of the listbox.
/// @param rc RECT containing desired coordinates for the combobox control.
/// @param pItem Pointer to a LISTBOXITEM object.
///
/// @returns VOID.
static VOID ListBox_OnSelectComboBox(HWND hwnd, RECT rc, LPLISTBOXITEM pItem)
{
    HWND hCombo;

    rc.top += 1;
    rc.bottom += 100;

    if (PIT_COMBO == pItem->iItemType)
    {
        if (NULL == g_lpInst->hwndCtl1)
            g_lpInst->hwndCtl1 = CreateCombo(g_lpInst->hInstance, hwnd, ID_COMBO, FALSE);

        hCombo = g_lpInst->hwndCtl1;
    }
    else //PIT_EDITCOMBO
    {
        if (NULL == g_lpInst->hwndCtl1)
            g_lpInst->hwndCtl1 = CreateCombo(g_lpInst->hInstance, hwnd, ID_EDITCOMBO, TRUE);

        hCombo = g_lpInst->hwndCtl1;
    }
    ComboBox_ResetContent(hCombo);
    MoveWindow(hCombo, rc.left, rc.top, WIDTH(rc), HEIGHT(rc), TRUE);

    //Walk the item list and add each string until the empty string
    for (LPTSTR p = pItem->lpszMisc; *p; p += _tcslen(p) + 1)
    {
        if (CB_ERR == ComboBox_FindStringExact(hCombo, 0, p))
            ComboBox_AddString(hCombo, p);
    }

    ShowWindow(hCombo, SW_SHOW);
    SetFocus(hCombo);

    //Jump to the property's current value in the combo box
    INT itm = ComboBox_FindStringExact(hCombo, 0, pItem->lpszCurValue);
    if (itm != CB_ERR)
        ComboBox_SetCurSel(hCombo, itm);
    else
    {
        ComboBox_SetCurSel(hCombo, 0);
        ComboBox_SetText(hCombo, pItem->lpszCurValue);
        ComboBox_SetEditSel(hCombo, 0, -1);
    }
}

/// @brief Handles WM_KEYDOWN messages sent to the listbox.
///
/// @param hwnd  Handle of the listbox.
/// @param vk The virtual key code.
/// @param fDown TRUE for keydown (always TRUE).
/// @param cRepeat The number of times the keystroke is repeated
///         as a result of the user holding down the key.
/// @param flags Indicate OEM scan codes etc.
///
/// @returns VOID.
static VOID ListBox_OnKeyDown(HWND hwnd, UINT vk, BOOL fDown, INT cRepeat, UINT flags)
{
    BOOL fHandled = FALSE;

    if (NULL != g_lpInst->lpCurrent)
    {
        RECT rc;
        memset(&rc, 0, sizeof rc);
        ListBox_GetItemRect(hwnd, ListBox_GetCurSel(hwnd), &rc);

        if (PIT_CATALOG == g_lpInst->lpCurrent->iItemType)
        {
            if (VK_RIGHT == vk && g_lpInst->lpCurrent->fCollapsed || VK_LEFT == vk && !g_lpInst->lpCurrent->fCollapsed)
            {
                ToggleCatalog(g_lpInst->lpCurrent);
                fHandled = TRUE;
            }
        }
        if (PIT_CHECK == g_lpInst->lpCurrent->iItemType && (VK_SPACE == vk || VK_RETURN == vk))
        {
            if (0 == _tcsicmp(g_lpInst->lpCurrent->lpszMisc, SELECT))
            {
                g_lpInst->lpCurrent->lpszCurValue = (0 == _tcsicmp(g_lpInst->lpCurrent->lpszCurValue, CHECKED) ? UNCHECKED : CHECKED);
                RedrawWindow(hwnd, &rc, NULL, RDW_ERASE | RDW_INVALIDATE | RDW_UPDATENOW);
                Grid_NotifyParent(); //DWM 1.2: Notify of check change
            }
        }
        else if (PIT_CHECK == g_lpInst->lpCurrent->iItemType && VK_ESCAPE == vk)
        {
            g_lpInst->lpCurrent->lpszMisc = UNSELECT;
            RedrawWindow(hwnd, &rc, NULL, RDW_ERASE | RDW_INVALIDATE | RDW_UPDATENOW);
        }
        else if (PIT_CHECK == g_lpInst->lpCurrent->iItemType &&
                VK_LEFT <= vk && vk <= VK_DOWN &&
                0 == _tcsicmp(g_lpInst->lpCurrent->lpszMisc, SELECT))
        {
            fHandled = TRUE;
        }
        else
        {
            if (VK_TAB == vk)
            {
                if (PIT_COLOR == g_lpInst->lpCurrent->iItemType)
                {
                    rc.left = g_lpInst->iHDivider + 1 + WIDTH_PART2;
                }
                else
                {
                    rc.left = g_lpInst->iHDivider + 1;
                }
                switch (g_lpInst->lpCurrent->iItemType)
                {
                    case PIT_CATALOG: //Ignore this
                    case PIT_STATIC:
                        SetFocusToParent(); //DWM 1.3: Added
                        break;
                    case PIT_EDIT:
                        ListBox_OnSelectEdit(hwnd, rc, g_lpInst->lpCurrent);
                        break;
                    case PIT_COMBO:
                    case PIT_EDITCOMBO:
                        ListBox_OnSelectComboBox(hwnd, rc, g_lpInst->lpCurrent);
                        break;
                    case PIT_IP:
                        ListBox_OnSelectIP(hwnd, rc, g_lpInst->lpCurrent);
                        break;
                    case PIT_CHECK:
                        if(GetKeyState(VK_SHIFT) & 0x8000)
                        {
                            FORWARD_WM_KEYDOWN(hwnd, VK_ESCAPE, cRepeat, flags, SNDMSG);
                        }
                        else if(0 == _tcsicmp(g_lpInst->lpCurrent->lpszMisc, SELECT))
                        {
                            g_lpInst->lpCurrent->lpszMisc = UNSELECT;
                            SetFocusToParent();
                        }
                        else
                        {
                            g_lpInst->lpCurrent->lpszMisc = SELECT;
                        }
                        RedrawWindow(hwnd, &rc, NULL, RDW_ERASE | RDW_INVALIDATE | RDW_UPDATENOW);
                        break;
                    case PIT_DATE:
                    case PIT_TIME:
                    case PIT_DATETIME:
                        ListBox_OnSelectDateTime(hwnd, rc, g_lpInst->lpCurrent);
                        break;
                    default:
                        ListBox_OnDisplayButton(hwnd, rc);
                        break;
                }
            }
            else if (VK_PRIOR == vk) //Mimic behavior of VS propGrid
            {
                FORWARD_WM_KEYDOWN(hwnd, VK_HOME, cRepeat, flags, SNDMSG);
                fHandled = TRUE;
            }
            else if (VK_NEXT == vk) //Mimic behavior of VS propGrid
            {
                FORWARD_WM_KEYDOWN(hwnd, VK_END, cRepeat, flags, SNDMSG);
                fHandled = TRUE;
            }
        }
    }
    if (!fHandled) //Not fully handled so follow up with default handler
        FORWARD_WM_KEYDOWN(hwnd, vk, cRepeat, flags, DefProc);
}

/// @brief Handles WM_COMMAND messages sent to the listbox.
///
/// @param hwnd  Handle of listbox.
/// @param id The id of the sender.
/// @param hwndCtl The hwnd of the sender.
/// @param codeNotify The notification code sent.
///
/// @par Comments:
///       We are only concerned with handling button click notifications here.
///       These notifications originate when the button is clicked for items of
///        type PIT_COLOR, PIT_FONT, PIT_FILE, and PIT_FOLDER.
///
/// @returns VOID.
static VOID ListBox_OnCommand(HWND hwnd, INT id, HWND hwndCtl, UINT codeNotify)
{
    if (ID_BUTTON == id)
    {
        if (NULL == g_lpInst->lpCurrent)
            return;

        //Display the appropriate common dialog depending on what type
        // of chooser is associated with the property
        switch (g_lpInst->lpCurrent->iItemType)
        {
            case PIT_COLOR:
            {
                CHOOSECOLOR cc;
                memset(&cc, 0, sizeof(cc));
                cc.lStructSize = sizeof(cc);
                cc.hwndOwner = hwnd;
                cc.hInstance = (HWND)g_lpInst->hInstance;
                cc.rgbResult = GetColor(g_lpInst->lpCurrent->lpszCurValue);
                cc.lpCustColors = (LPDWORD)g_CustomColors;
                cc.Flags = CC_FULLOPEN | CC_RGBINIT;
                if (ChooseColor(&cc))
                {
                    TCHAR buf[MAX_PATH];
                    _stprintf(buf, MAX_PATH, _T("%d,%d,%d"), GetRValue(cc.rgbResult), GetGValue(cc.rgbResult), GetBValue(cc.rgbResult));

                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, buf);
                }
            }
                break;

            case PIT_FILE:
            {
                TCHAR title[MAX_PATH] = { 0 };
                TCHAR filename[MAX_PATH] = { 0 };
                TCHAR path[MAX_PATH] = { 0 };
                TCHAR filter[MAX_PATH] = { 0 };
                TCHAR ext[4] = { 0 };

                OPENFILENAME ofn;
                memset(&ofn, 0, sizeof(OPENFILENAME));

                ofn.lStructSize = sizeof(OPENFILENAME);
                ofn.hwndOwner = hwnd;
                ofn.hInstance = g_lpInst->hInstance;

                _stscanf(g_lpInst->lpCurrent->lpszMisc,
#ifdef _UNICODE
					L"Title: %32l[^\r\n] " \
					L"Path: %256l[^\r\n] " \
					L"Filter: %256l[^\r\n] " \
					L"Default Extension: %3l[^\r\n] ",
#else
					"Title: %32[^\r\n] " \
					"Path: %256[^\r\n] " \
					"Filter: %256[^\r\n] " \
					"Default Extension: %3[^\r\n]",
#endif
                    (LPTSTR)&title, (LPTSTR)&path, (LPTSTR)&filter, (LPTSTR)&ext);

                if (0 != _tcscmp(_T("?"), title)) //DWM 1.2: Added test
                    ofn.lpstrTitle = title;
                else
                    ofn.lpstrTitle = _T("Select file");

                if (0 != _tcscmp(_T("?"), path)) //DWM 1.2: Added test
                {
                    //Exclude the filename
                    for (LPTSTR ptr = path + _tcslen(path) - 1; *ptr; ptr--)
                        if (*ptr == _T('\\'))
                        {
                            *ptr = _T('\0');
                            break;
                        }
                    ofn.lpstrInitialDir = path;
                }

                if (0 != _tcscmp(_T("?"), filter)) //DWM 1.2: Added test
                {
                    //Convert back to double null-terminated string
                    for (LPTSTR ptr = filter; *ptr; ptr++)
                        if (_T('\t') == *ptr)
                            *ptr = _T('\0');

                    ofn.lpstrFilter = filter;
                }
                else
                    ofn.lpstrFilter = _T("All Files (*.*)\0*.*\0");

                if (0 != _tcscmp(_T("?"), ext)) //DWM 1.2: Added test
                {
                    ofn.lpstrDefExt = ext;
                }
                else
                {
                    ofn.lpstrDefExt = _T("txt");
                }
                ofn.lpstrFile = filename;
                ofn.nMaxFile = MAX_PATH;
                ofn.Flags = OFN_EXPLORER | OFN_FILEMUSTEXIST | OFN_PATHMUSTEXIST | OFN_HIDEREADONLY;

                if (GetOpenFileName(&ofn))
                {
                    PROPGRIDFDITEM pgi;
                    pgi.lpszDlgTitle = (LPTSTR)ofn.lpstrTitle;
                    pgi.lpszFilePath = (LPTSTR)ofn.lpstrFile;
                    pgi.lpszFilter = (LPTSTR)ofn.lpstrFilter;
                    pgi.lpszDefExt = (LPTSTR)ofn.lpstrDefExt;
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, pgi.lpszFilePath);
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszMisc, FileDialogItem_ToString(&pgi));
                }
                else //DWM 1.2: Reset to unselected file
                {
                    PROPGRIDFDITEM pgi;
                    pgi.lpszDlgTitle = (LPTSTR)ofn.lpstrTitle;
                    pgi.lpszFilePath = _T("");
                    pgi.lpszFilter = (LPTSTR)ofn.lpstrFilter;
                    pgi.lpszDefExt = (LPTSTR)ofn.lpstrDefExt;
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, pgi.lpszFilePath);
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszMisc, FileDialogItem_ToString(&pgi));
                }

            }
                break;

            case PIT_FONT:
            {
                CHOOSEFONT ocf;
                memset(&ocf, 0, sizeof(ocf));

                PROPGRIDFONTITEM pgfi;
                LogFontItem_FromString(&pgfi, g_lpInst->lpCurrent->lpszCurValue);

                ocf.lStructSize = sizeof(ocf);
                ocf.hwndOwner = hwnd;
                ocf.hInstance = g_lpInst->hInstance;
                ocf.Flags = CF_INITTOLOGFONTSTRUCT | CF_EFFECTS | CF_SCREENFONTS;
                ocf.lpLogFont = &pgfi.logFont;
                ocf.rgbColors = pgfi.crFont;

                if (ChooseFont(&ocf))
                {
                    pgfi.crFont = ocf.rgbColors;
                    AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, LogFontItem_ToString(&pgfi));
                }
            }
                break;

            case PIT_FOLDER:
            {
                LPTSTR temp = BrowseFolder(hwnd, g_lpInst->lpCurrent->lpszCurValue, _T(""), _T(""));
                //DWM 1.2: Reset to unselected folder //if (0 < _tcslen(temp))
                AllocatedString_Replace(g_lpInst->lpCurrent->lpszCurValue, temp);
            }
                break;
        }
        ShowWindow(hwndCtl, SW_HIDE);
        SetFocus(hwnd);
        Refresh(hwnd); //Trigger WM_DRAWITEM
        Grid_NotifyParent();
    }
}

#pragma endregion list box message handlers

#pragma region grid message handlers

/// @brief Handles WM_SIZE message.
///
/// @param hwnd  Handle of grid.
/// @param state Specifies the type of resizing requested.
/// @param cx The width of client area.
/// @param cy The height of client area.
///
/// @returns VOID.
static VOID Grid_OnSize(HWND hwnd, UINT state, INT cx, INT cy)
{
    g_lpInst->iVDivider = cy - g_lpInst->iDescHeight;

    if (NULL != g_lpInst->hwndPropDesc)
    {
        //Size listbox component
        SetWindowPos(g_lpInst->hwndListBox, NULL, 0, 0, cx,
            g_lpInst->iVDivider - 2, SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE);

        SetWindowPos(g_lpInst->hwndPropDesc, NULL, 0, g_lpInst->iVDivider,
            cx, g_lpInst->iDescHeight, SWP_NOZORDER | SWP_NOACTIVATE);
    }
    else
    {
        //Size listbox component to fill parent
        SetWindowPos(g_lpInst->hwndListBox, NULL, 0, 0, cx, cy,
            SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE);
    }
    if(NULL != g_lpInst->hwndToolTip)//Resize ToolTips
    {
        TOOLINFO ti = { sizeof(ti) };
        ti.hwnd = g_lpInst->hwndListBox;
        ti.uId = 0;
        GetClientRect(hwnd, &ti.rect);
        ToolTip_NewToolRect(g_lpInst->hwndToolTip, &ti);
    }
    if (NULL != g_lpInst->lpCurrent)
    {
        if(PIT_CHECK == g_lpInst->lpCurrent->iItemType)
            g_lpInst->lpCurrent->lpszMisc = UNSELECT; //Prevent toggle
    }

    FORWARD_WM_COMMAND(hwnd, GetDlgCtrlID(g_lpInst->hwndListBox),
        g_lpInst->hwndListBox, LBN_SELCHANGE, SNDMSG);
}

/// @brief Handles WM_SETCURSOR message.
///
/// @param hwnd  Handle of grid.
/// @param hwndCursor The handle of the cursor.
/// @param codeHitTest The hit test code.
/// @param msg The windows mouse message related to hit test.
///
/// @par Comments:
///       We are only concerned with detecting the completion of a sizing operation
///        consequently return false and let the default behavior stand.
///
/// @returns BOOL TRUE if handled.
static BOOL Grid_OnSetCursor(HWND hwnd, HWND hwndCursor, UINT codeHitTest, UINT msg)
{
    if (NULL != g_lpInst->lpCurrent)
    {
        if(g_lpInst->fTracking) //Sizing operation concluded
        {
            if(PIT_CHECK == g_lpInst->lpCurrent->iItemType)
                g_lpInst->lpCurrent->lpszMisc = SELECT;

            g_lpInst->fTracking = FALSE;
            FORWARD_WM_KEYDOWN(g_lpInst->hwndListBox, VK_TAB, 0, 0, SNDMSG); //Focus to editor
        }
    }
    return FALSE;
}

/// @brief Handles WM_LBUTTONDOWN message.
///
/// @param hwnd  Handle of grid.
/// @param fDoubleClick TRUE if this is a double click event.
/// @param x The xpos of the mouse.
/// @param y The ypos of the mouse.
/// @param keyFlags Set if certain keys down at time of click.
///
/// @returns VOID.
static VOID Grid_OnLButtonDown(HWND hwnd, BOOL fDoubleClick, INT x, INT y, UINT keyFlags)
{
    if ((y >= g_lpInst->iVDivider - 5) && (y <= g_lpInst->iVDivider + 5))
    {
        //If mouse clicked on divider line, then start resizing
        SetCursor(LoadCursor(NULL, IDC_SIZENS));

        RECT rc;
        GetWindowRect(hwnd, &rc);
        rc.top += 10;
        rc.bottom -= 10;
        //Do not let mouse leave the list box boundary
        ClipCursor(&rc);

        g_lpInst->fTracking = TRUE;
        g_lpInst->nDivLft = 0;
        g_lpInst->nDivRht = WIDTH(rc); //DWM 1.7: Changed from = rc.right
        g_lpInst->nOldDivY = y;

        HDC hdc = GetDC(hwnd);
        HPEN hOld = SelectObject(hdc, CreatePen(PS_SOLID, 3, 0));

        InvertLine(hdc, g_lpInst->nDivLft, g_lpInst->nOldDivY,
            g_lpInst->nDivRht, g_lpInst->nOldDivY);

        DeleteObject(SelectObject(hdc, hOld));
        ReleaseDC(hwnd, hdc);

        //Capture the mouse
        SetCapture(hwnd);
    }
}

/// @brief Handles WM_LBUTTONUP message.
///
/// @param hwnd  Handle of grid.
/// @param x The xpos of the mouse.
/// @param y The ypos of the mouse.
/// @param keyFlags Set if certain keys down at time of click.
///
/// @returns VOID.
static VOID Grid_OnLButtonUp(HWND hwnd, INT x, INT y, UINT keyFlags)
{
    if (g_lpInst->fTracking)
    {
        g_lpInst->fTracking = FALSE;
        //If mouse was captured then release it
        if (hwnd == GetCapture())
            ReleaseCapture();

        ClipCursor(NULL);

        //Set the divider position to the new value
        g_lpInst->nOldDivY = y;

        HDC hdc = GetDC(hwnd);
        HPEN hOld = SelectObject(hdc, CreatePen(PS_SOLID, 3, 0));

        InvertLine(hdc, g_lpInst->nDivLft, g_lpInst->nOldDivY,
            g_lpInst->nDivRht, g_lpInst->nOldDivY);

        DeleteObject(SelectObject(hdc, hOld));
        ReleaseDC(hwnd,hdc);

        //Trigger a resize
        RECT rc;
        GetClientRect(hwnd, &rc);
        g_lpInst->iDescHeight = HEIGHT(rc) - y;
        Grid_OnSize(hwnd, 0, WIDTH(rc), HEIGHT(rc));
    }
}

/// @brief Handles WM_MOUSEMOVE message.
///
/// @param hwnd  Handle of grid.
/// @param x The xpos of the mouse.
/// @param y The ypos of the mouse.
/// @param keyFlags Set if certain keys down at time of move.
///
/// @returns VOID.
static VOID Grid_OnMouseMove(HWND hwnd, INT x, INT y, UINT keyFlags)
{
    if ((y >= g_lpInst->iVDivider - 5) && (y <= g_lpInst->iVDivider + 5))
    {
        SetCursor(LoadCursor(NULL, IDC_SIZENS));
    }
    else if (g_lpInst->fTracking)
    {
        //Move divider line to the mouse pos. if columns are
        // currently being resized
        HDC hdc = GetDC(hwnd);
        HPEN hOld = SelectObject(hdc, CreatePen(PS_SOLID, 3, 0));

        //Remove old divider line
        InvertLine(hdc, g_lpInst->nDivLft, g_lpInst->nOldDivY,
            g_lpInst->nDivRht, g_lpInst->nOldDivY);
        //Draw new divider line
        g_lpInst->nOldDivY = y;

        InvertLine(hdc, g_lpInst->nDivLft, g_lpInst->nOldDivY,
            g_lpInst->nDivRht, g_lpInst->nOldDivY);

        DeleteObject(SelectObject(hdc, hOld));
        ReleaseDC(hwnd, hdc);
    }
    else
    {
        SetCursor(LoadCursor(NULL, IDC_ARROW));
    }
}

/// @brief Handles WM_CTLCOLORLISTBOX message sent to the grid.
///
/// @param hwnd  Handle of grid.
/// @param hdc The handle of the device context.
/// @param hwndChild The handle of the listbox.
/// @param type CTLCOLOR_LISTBOX.
///
/// @returns HBRUSH The handle of the brush used to paint the
///                  listbox's background.
static HBRUSH Grid_OnCtlColorListbox(HWND hwnd, HDC hdc, HWND hwndChild, INT type)
{
    return SetColor(hdc, GetSysColor(COLOR_MENUTEXT), GetSysColor(COLOR_3DFACE));
}

/// @brief Handles WM_CTLCOLORSTATIC message sent to the grid.
///
/// @param hwnd  Handle of grid.
/// @param hdc The handle of the device context.
/// @param hwndChild The handle of the static.
/// @param type CTLCOLOR_STATIC.
///
/// @returns HBRUSH The handle of the brush used to paint the
///                  static's background.
static HBRUSH Grid_OnCtlColorStatic(HWND hwnd, HDC hdc, HWND hwndChild, INT type)
{
    //DWM 1.3: Keep the area between the description and the list refreshed
    if (NULL != g_lpInst->hwndPropDesc)
    {
        RECT rc;
        GetClientRect(hwnd, &rc);

        HDC hdc = GetDC(hwnd);
        FillSolidRect(hdc,MAKE_PRECT(0, g_lpInst->iVDivider - 2,
            WIDTH(rc), g_lpInst->iVDivider),GetSysColor(COLOR_BTNFACE));
        ReleaseDC(hwnd,hdc);
    } 
    return FORWARD_WM_CTLCOLORSTATIC(hwnd, hdc, hwndChild, DefWindowProc);
}

/// @brief Handles WM_MEASUREITEM message sent to the grid when the owner-drawn
///         listbox is created.
///
/// @param hwnd  Handle of grid.
/// @param lpMeasureItem The structure that contains the dimensions of the
///                       owner-drawn listbox.
///
/// @returns VOID.
static VOID Grid_OnMeasureItem(HWND hwnd, LPMEASUREITEMSTRUCT lpMeasureItem)
{
    lpMeasureItem->itemHeight = MINIMUM_ITEM_HEIGHT; //pixels
}

/// @brief Ensure that a catalog item matching the given catalog name is expanded.
///
/// @param szCatalog The catalog name.
///
/// @returns VOID.
static VOID Grid_ExpandCatalog(LPCTSTR szCatalog)
{
    if (NULL != szCatalog)
    {
        INT ln = _tcslen(szCatalog) + 1; //Check for empty string
        if (ln == 1)
            return;
    }

    for (INT i = 0; i < ListBox_GetCount(g_lpInst->hwndListBox); i++)
    {
        LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(g_lpInst->hwndListBox, i);
        if (NULL != pItem && PIT_CATALOG == pItem->iItemType &&
            (NULL == szCatalog || 0 == _tcsicmp(pItem->lpszCatalog, szCatalog)))
        {
            if (pItem->fCollapsed)
                ToggleCatalog(pItem);
        }
    }
}

/// @brief Ensure that a catalog item matching the given catalog name is collapsed.
///
/// @param szCatalog The catalog name.
///
/// @returns VOID.
static VOID Grid_CollapseCatalog(LPCTSTR szCatalog)
{
    if (NULL != szCatalog)
    {
        INT ln = _tcslen(szCatalog) + 1; //Check for empty string
        if (ln == 1)
            return;
    }
    for (INT i = 0; i < ListBox_GetCount(g_lpInst->hwndListBox); i++)
    {
        LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(g_lpInst->hwndListBox, i);
        if (NULL != pItem && PIT_CATALOG == pItem->iItemType &&
            (NULL == szCatalog || 0 == _tcsicmp(pItem->lpszCatalog, szCatalog)))
        {
            if (!pItem->fCollapsed)
                ToggleCatalog(pItem);
        }
    }
}

/// @brief Handles WM_COMMAND message.
///
/// @param hwnd  Handle of grid.
/// @param id The id of the sender.
/// @param hwndCtl The hwnd of the sender.
/// @param codeNotify The notification code sent.
///
/// @returns VOID.
static VOID Grid_OnCommand(HWND hwnd, INT id, HWND hwndCtl, UINT codeNotify)
{
    if (g_lpInst->hwndListBox == hwndCtl)
    {
        switch (codeNotify)
        {
            case LBN_SELCHANGE:
            {
                SetFocus(hwndCtl);
                INT iCurSel = ListBox_GetCurSel(hwndCtl);

                //If this is a checkbox, notify of the previous item's change
                LPLISTBOXITEM pItem = (LPLISTBOXITEM)ListBox_GetItemDataSafe(hwndCtl,
                    g_lpInst->iPrevSel);
                if (NULL != pItem)
                {
                    if (PIT_CHECK == pItem->iItemType)
                    {
                        if (iCurSel != g_lpInst->iPrevSel &&
                            0 == _tcsicmp(pItem->lpszMisc, SELECT))
                            pItem->lpszMisc = UNSELECT;
                    }
                }
                g_lpInst->lpCurrent = (LPLISTBOXITEM)ListBox_GetItemDataSafe(hwndCtl,
                    iCurSel);
                if (NULL == g_lpInst->lpCurrent)
                    return;

                //Display the property description if desired
                if (NULL != g_lpInst->hwndPropDesc)
                    Static_SetText(g_lpInst->hwndPropDesc,
                        g_lpInst->lpCurrent->lpszPropDesc);

                //Destroy previous editor
                if (NULL != g_lpInst->hwndCtl1)
                {
                    DestroyWindow(g_lpInst->hwndCtl1);
                    g_lpInst->hwndCtl1 = NULL;
                }
                if (NULL != g_lpInst->hwndCtl2)
                {
                    DestroyWindow(g_lpInst->hwndCtl2);
                    g_lpInst->hwndCtl2 = NULL;
                }

                //Redraw the selection
                if(iCurSel != g_lpInst->iPrevSel)
                {
                    RECT rc;
                    memset(&rc, 0, sizeof rc);

                    ListBox_GetItemRect(hwndCtl, g_lpInst->iPrevSel, &rc);
                    RedrawWindow(hwndCtl, &rc,
                     NULL, RDW_ERASE | RDW_INVALIDATE | RDW_UPDATENOW);

                    ListBox_GetItemRect(hwndCtl, iCurSel, &rc);
                    RedrawWindow(hwndCtl, &rc,
                        NULL, RDW_ERASE | RDW_INVALIDATE | RDW_UPDATENOW);

                    g_lpInst->iPrevSel = iCurSel;
                }
            }
        }
    }
}

/// @brief Handles WM_DRAWITEM message sent to the grid when a visual aspect of
///         the owner-drawn listbox has changed.
///
/// @param hwnd  Handle of grid.
/// @param lpDIS The structure that contains information about the item
///               to be drawn and the type of drawing required.
///
/// @returns VOID.
static VOID Grid_OnDrawItem(HWND hwnd, const DRAWITEMSTRUCT *lpDIS)
{
    UINT nIndex = lpDIS->itemID;

    if (lpDIS->hwndItem != g_lpInst->hwndListBox)
        return;

    if (nIndex == (UINT) - 1 || !(lpDIS->itemAction & ODA_DRAWENTIRE))
        return;

    //Get the item for the current row
    LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(lpDIS->hwndItem, nIndex);
    if (NULL == pItem)
        return;

    RECT rectFullItem, rectPart0, rectCatPart1, rectPart1, rectPart2, rectPart3;

    rectFullItem = lpDIS->rcItem;

    if (g_lpInst->iHDivider == 0)
        g_lpInst->iHDivider = WIDTH(rectFullItem) / 2;

    rectPart0 = rectFullItem;
    rectPart0.right = rectPart0.left + WIDTH_PART0;

    rectCatPart1 = rectFullItem;
    rectCatPart1.left = rectPart0.right;

    rectPart1 = rectCatPart1;
    rectPart1.right = g_lpInst->iHDivider;

    rectPart2 = rectFullItem;
    rectPart2.left = g_lpInst->iHDivider + (PIT_CATALOG == pItem->iItemType ? 0 : 1);
    if (PIT_COLOR == pItem->iItemType || PIT_CHECK == pItem->iItemType)
    {
        rectPart2.right = rectPart2.left + WIDTH_PART2;
    }
    else
        rectPart2.right = rectPart2.left;

    rectPart3 = rectFullItem;
    rectPart3.left = rectPart2.right;

    //
    //First part of item
    //
    FillRect(lpDIS->hDC, &rectPart0, GetStockObject(HOLLOW_BRUSH));

    if (PIT_CATALOG == pItem->iItemType) //Draw expand / collapse box
    {
        RECT rect2;
        rect2.left = rectPart0.left + 4;
        rect2.top = rectPart0.top + 4;
        rect2.right = rect2.left + 9;
        rect2.bottom = rect2.top + 9;

        FillRect(lpDIS->hDC, &rect2, GetSysColorBrush(COLOR_WINDOW));
        FrameRect(lpDIS->hDC, &rect2, GetStockObject(BLACK_BRUSH));

        POINT ptCtr;
        ptCtr.x = (LONG) (rect2.left + (WIDTH(rect2) * 0.5));
        ptCtr.y = (LONG) (rect2.top + (HEIGHT(rect2) * 0.5));
        InflateRect(&rect2, -2, -2);

        DrawLine(lpDIS->hDC, rect2.left, ptCtr.y, rect2.right, ptCtr.y); //Make a -
        if (pItem->fCollapsed) //Convert to +
            DrawLine(lpDIS->hDC, ptCtr.x, rect2.top, ptCtr.x, rect2.bottom);
    }

    //
    //Second part of item
    //

    //If it is the selected item, set its background-color
    HFONT oldFont;
    if (PIT_CATALOG == pItem->iItemType)
    {
        FillRect(lpDIS->hDC, &rectCatPart1, GetStockObject(HOLLOW_BRUSH));

        if (nIndex == (UINT)ListBox_GetCurSel(lpDIS->hwndItem) && g_lpInst->fGotFocus)
        {
            InflateRect(&rectCatPart1, -2, -2);
            FrameRect(lpDIS->hDC, &rectCatPart1, GetSysColorBrush(COLOR_BTNSHADOW));
            InflateRect(&rectCatPart1, 2, 2);
        }

        //Write the property name (bold font, Visual Studio style)
        oldFont = SelectObject(lpDIS->hDC, Font_SetBold(lpDIS->hwndItem, TRUE));
        SetTextColor(lpDIS->hDC, GetSysColor(COLOR_BTNTEXT));
        DrawText(lpDIS->hDC, pItem->lpszCatalog, _tcslen(pItem->lpszCatalog),
            MAKE_PRECT(rectCatPart1.left + 6, rectCatPart1.top + 3,
            rectCatPart1.right - 6, rectCatPart1.bottom + 3),
            DT_NOCLIP | DT_LEFT | DT_SINGLELINE);
    }
    else
    {
        SetBkMode(lpDIS->hDC, TRANSPARENT);

        FillRect(lpDIS->hDC, &rectPart1,
            GetSysColorBrush(nIndex == (UINT)ListBox_GetCurSel(lpDIS->hwndItem) ?
            (g_lpInst->fGotFocus ? COLOR_HIGHLIGHT : COLOR_BTNFACE) : COLOR_WINDOW));

        //Write the property name
        oldFont = SelectObject(lpDIS->hDC, Font_SetBold(lpDIS->hwndItem, FALSE));
        SetTextColor(lpDIS->hDC,
            GetSysColor(nIndex == (UINT)ListBox_GetCurSel(lpDIS->hwndItem) ?
            (g_lpInst->fGotFocus ? COLOR_HIGHLIGHTTEXT : COLOR_BTNTEXT) : COLOR_WINDOWTEXT));
        DrawText(lpDIS->hDC, pItem->lpszPropName, _tcslen(pItem->lpszPropName),
            MAKE_PRECT(rectPart1.left + 3, rectPart1.top + 3, rectPart1.right - 3,
            rectPart1.bottom + 3), DT_NOCLIP | DT_LEFT | DT_SINGLELINE);

        DrawBorder(lpDIS->hDC, &rectPart1, BF_TOPRIGHT,
            GetSysColor(COLOR_BTNFACE));
    }
    DeleteObject(oldFont);

    //
    //Third part - Draw color box or check box
    //
    if (PIT_CATALOG != pItem->iItemType)
    {
        FillRect(lpDIS->hDC, &rectPart2, GetSysColorBrush(COLOR_WINDOW));

        RECT rect3 = rectPart2;
        rect3.left += 2;
        rect3.top += 3;
        rect3.bottom = rect3.top + 15;
        rect3.right = rect3.left + 15;

        if (PIT_COLOR == pItem->iItemType)
        {
            FillSolidRect(lpDIS->hDC, &rect3, GetColor(pItem->lpszCurValue));
            FrameRect(lpDIS->hDC, &rect3, GetStockObject(BLACK_BRUSH));
        }
        else if (PIT_CHECK == pItem->iItemType)
        {
            if (0 == _tcsicmp(pItem->lpszMisc, SELECT))
            {
                FillRect(lpDIS->hDC, &rectPart2,
                    GetSysColorBrush(COLOR_HIGHLIGHT));
            }
            DrawFrameControl(lpDIS->hDC, &rect3, DFC_BUTTON, DFCS_BUTTONCHECK |
                (_tcsicmp(pItem->lpszCurValue, CHECKED) == 0 ? DFCS_CHECKED : 0));

			//DWM 1.8: Added
		    if (FLATCHECKS & (DWORD)GetWindowLongPtr(lpDIS->hwndItem, GWLP_USERDATA))
		    {
		        //Make border thin
	            FrameRect(lpDIS->hDC, &rect3, GetSysColorBrush(COLOR_BTNFACE));
	            InflateRect(&rect3, -1, -1);
	            FrameRect(lpDIS->hDC, &rect3, GetSysColorBrush(COLOR_WINDOW));
		    }
        }
    }
    DrawBorder(lpDIS->hDC, &rectPart2, BF_TOP, GetSysColor(COLOR_BTNFACE));

    //
    //Fourth part - Write the initial property value in the second rectangle
    //
    if (PIT_CATALOG != pItem->iItemType)
    {
        FillRect(lpDIS->hDC, &rectPart3, GetSysColorBrush(COLOR_WINDOW));
    }
    if (PIT_CHECK != pItem->iItemType)
    {
        SetBkMode(lpDIS->hDC, TRANSPARENT);

        if (PIT_FONT == pItem->iItemType)
        {
            TCHAR buf[MAX_PATH] = { 0 };
            LPTSTR szFmt;
            PROPGRIDFONTITEM pgfi;
            TEXTMETRIC tm;
            HFONT hf, hfOld;
            LONG PointSize = 0;

            LogFontItem_FromString(&pgfi, pItem->lpszCurValue);
            PointSize = -MulDiv(pgfi.logFont.lfHeight, 72,
                GetDeviceCaps(lpDIS->hDC, LOGPIXELSY));

            if (0 < PointSize)
            {
                if (GetTextMetrics(lpDIS->hDC, &tm))
                    pgfi.logFont.lfHeight = tm.tmHeight + 2; //So displayed text is not oversized

                hf = CreateFontIndirect(&pgfi.logFont);
                hfOld = SelectObject(lpDIS->hDC, hf);

                SetTextColor(lpDIS->hDC, pgfi.crFont);
#ifdef _UNICODE
                szFmt = _T("%ls %d");
#else
                szFmt = _T("%s %d");
#endif
                _stprintf(buf, MAX_PATH, szFmt, pgfi.logFont.lfFaceName, PointSize);
                DrawText(lpDIS->hDC, buf, _tcslen(buf),
                    MAKE_PRECT(rectPart3.left + 3, rectPart3.top + 3,
                    rectPart3.right + 3, rectPart3.bottom + 3),
                    DT_NOCLIP | DT_LEFT | DT_SINGLELINE);

                DeleteObject(SelectObject(lpDIS->hDC, hfOld));
            }
        }
        else
        {
            SetTextColor(lpDIS->hDC, GetSysColor(COLOR_WINDOWTEXT));
            DrawText(lpDIS->hDC, pItem->lpszCurValue, _tcslen(pItem->lpszCurValue),
                MAKE_PRECT(rectPart3.left + 3, rectPart3.top + 3,
                rectPart3.right + 3, rectPart3.bottom + 3),
                DT_NOCLIP | DT_LEFT | DT_SINGLELINE);
        }
    }
    DrawBorder(lpDIS->hDC, &rectPart3, BF_TOP, GetSysColor(COLOR_BTNFACE));
}

/// @brief Handles WM_SHOWWINDOW message.
///
/// @param hwnd Handle of grid.
/// @param fShow Show/hide flag TRUE for show FALSE for hide.
/// @param status Status flag.
///
/// @returns VOID.
static VOID Grid_OnShowWindow(HWND hwnd, BOOL fShow, UINT status)
{
    //DWM 1.3: Added this handler
    if(!fShow) //Hiding so make sure we update the fields
    {
        FORWARD_WM_CHAR(g_lpInst->hwndCtl1, VK_RETURN, 0, SNDMSG);
        FORWARD_WM_CHAR(g_lpInst->hwndCtl2, VK_RETURN, 0, SNDMSG);
    }
    FORWARD_WM_SHOWWINDOW(hwnd, fShow, status, DefWindowProc);
}

#pragma endregion grid message handlers

#pragma region public interface handlers

/// @brief Handles LB_ADDSTRING message sent to the grid.
///
/// @param pgi A pointer to a PROPGRIDITEM object.
///
/// @returns INT The index of the item added to the grid.
static INT Grid_OnAddString(LPPROPGRIDITEM pgi)
{
    LPLISTBOXITEM lpi;
    TCHAR buf[MAX_PATH];
    _tmemset(buf, (TCHAR)0, MAX_PATH);

    LPTSTR lpszCurValue = _T("");

    //Make sure the catalog for this item has been inserted
    INT idx = ListBox_FindCatalog(g_lpInst->hwndListBox, 0, pgi->lpszCatalog);

    if (LB_ERR == idx)  // Must add catalog to the listBox
    {
        lpi = NewItem(pgi->lpszCatalog,_T(""), _T(""),
            _T(""), _T(""), PIT_CATALOG);
        lpi->fCollapsed = TRUE;
        ListBox_AddItemData(g_lpInst->hwndListBox, lpi);
    }
    switch (pgi->iItemType)
    {
        case PIT_CATALOG: //We explicitly added a catalog item to the listbox
            return -2;    // catalogs are not added to the list map so skip the rest
        case PIT_EDIT:
        case PIT_STATIC:
        case PIT_COMBO:
        case PIT_EDITCOMBO:
        case PIT_FOLDER:
            lpszCurValue = (LPTSTR)pgi->lpCurValue;
            break;
        case PIT_COLOR:
            _stprintf(buf, MAX_PATH, _T("%d,%d,%d"),
                GetRValue((COLORREF)pgi->lpCurValue),
                GetGValue((COLORREF)pgi->lpCurValue),
                GetBValue((COLORREF)pgi->lpCurValue));
            lpszCurValue = buf;
            break;
        case PIT_FONT:
            lpszCurValue = LogFontItem_ToString((LPPROPGRIDFONTITEM)pgi->lpCurValue);
            break;
        case PIT_CHECK:
            lpszCurValue = (BOOL)pgi->lpCurValue ? CHECKED : UNCHECKED;
            break;
        case PIT_FILE:
            pgi->lpszzCmbItems = FileDialogItem_ToString((LPPROPGRIDFDITEM)pgi->lpCurValue);
            lpszCurValue = ((LPPROPGRIDFDITEM)pgi->lpCurValue)->lpszFilePath;
            break;
        case PIT_IP:
            _stprintf(buf, MAX_PATH,
                _T("%d.%d.%d.%d"),
                FIRST_IPADDRESS((DWORD)pgi->lpCurValue),
                SECOND_IPADDRESS((DWORD)pgi->lpCurValue),
                THIRD_IPADDRESS((DWORD)pgi->lpCurValue),
                FOURTH_IPADDRESS((DWORD)pgi->lpCurValue));
            lpszCurValue = buf;
            break;
        case PIT_DATE:
            GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE,
                (LPSYSTEMTIME)pgi->lpCurValue, NULL, buf, MAX_PATH);
            lpszCurValue = buf;
            break;
        case PIT_TIME:
            GetTimeFormat(LOCALE_USER_DEFAULT, 0, (LPSYSTEMTIME)pgi->lpCurValue,
                _T("hh':'mm':'ss tt"), buf, MAX_PATH);
            lpszCurValue = buf;
            break;
        case PIT_DATETIME:
            GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE,
                (LPSYSTEMTIME)pgi->lpCurValue, NULL, buf, MAX_PATH);
            _tcscat(buf, _T(" "));
            GetTimeFormat(LOCALE_USER_DEFAULT, 0, (LPSYSTEMTIME)pgi->lpCurValue,
                _T("hh':'mm':'ss tt"), (LPTSTR) (buf + _tcslen(buf)),
                MAX_PATH - _tcslen(buf));
            lpszCurValue = buf;
            break;
    }
    lpi = NewItem(pgi->lpszCatalog, pgi->lpszPropName,
            lpszCurValue, pgi->lpszzCmbItems, pgi->lpszPropDesc, pgi->iItemType);

    return ListBox_AddItemData(g_lpInst->hwndListMap, lpi);
}

/// @brief Handles LB_DELETESTRING message sent to the grid.
///
/// @param nIndex The index of the item to delete.
///
/// @returns INT A count of the items remaining in the grid. The return value is
///               LB_ERR if the index parameter specifies an index greater than
///               the number of items in the grid.
static INT Grid_OnDeleteString(INT nIndex)
{
    LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(g_lpInst->hwndListMap, nIndex);
    if (NULL == pItem)
        return LB_ERR;

    INT iItem = ListBox_FindItemData(g_lpInst->hwndListBox, 0, pItem);
    if (LB_ERR != iItem)
        ListBox_DeleteString(g_lpInst->hwndListBox, iItem);

    return ListBox_DeleteString(g_lpInst->hwndListMap, nIndex);
}

/// @brief Handles LB_GETCURSEL message sent to the grid.
///
/// @returns INT The zero-based index of the selected item. If there is no
///               selection, the return value is LB_ERR.
static INT Grid_OnGetCurSel(VOID)
{
    INT iItem = ListBox_GetCurSel(g_lpInst->hwndListBox);
    if (LB_ERR != iItem)
    {
        LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(g_lpInst->hwndListBox, iItem);
        if (NULL != pItem)
        {
            if (PIT_CATALOG != pItem->iItemType)
                return ListBox_FindItemData(g_lpInst->hwndListMap, 0, pItem);
        }
    }
    //If we get here we didn't succeed
    return LB_ERR;
}

/// @brief Handles LB_GETITEMDATA message sent to the grid.
///
/// @param iItem The zero-based index of the item.
///
/// @returns LRESULT In this case a pointer to a PROPGRIDITEM object.
static LRESULT Grid_OnGetItemData(INT iItem)
{
    static PROPGRIDITEM pgi;
    static PROPGRIDFONTITEM pgfi;
    static PROPGRIDFDITEM pgfdi;
    static SYSTEMTIME st = {0};
    static TCHAR outbuf[MAX_PATH];

    LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(g_lpInst->hwndListMap, iItem);
    if (NULL != pItem)
    {
        if (PIT_CATALOG != pItem->iItemType)
        {
            pgi.lpszCatalog = pItem->lpszCatalog;
            pgi.lpszPropName = pItem->lpszPropName;
            pgi.lpszzCmbItems = pItem->lpszMisc;
            pgi.lpszPropDesc = pItem->lpszPropDesc;
            pgi.iItemType = pItem->iItemType;

            switch (pgi.iItemType)
            {
                case PIT_EDIT:
                case PIT_COMBO:
                case PIT_EDITCOMBO:
                case PIT_STATIC:
                case PIT_FOLDER:
                    pgi.lpCurValue = (LPARAM)pItem->lpszCurValue;
                    break;
                case PIT_COLOR:
                    pgi.lpCurValue = (LPARAM)GetColor(pItem->lpszCurValue);
                    break;
                case PIT_FONT:
                    LogFontItem_FromString(&pgfi, pItem->lpszCurValue);
                    pgi.lpCurValue = (LPARAM) & pgfi;
                    break;
                case PIT_CHECK:
                    pgi.lpCurValue = (LPARAM) (0 == _tcsicmp(CHECKED,
                        pItem->lpszCurValue));
                    break;
                case PIT_FILE:
                    FileDialogItem_FromString(&pgfdi, pgi.lpszzCmbItems);
                    pgi.lpCurValue = (LPARAM) & pgfdi;
                    break;
                case PIT_IP:
                {
					//DWM 1.9: Updated this code to use DWORD array and MAKEIPADDRESS macroe instead of 4 byte array and cast.
					//DWM 1.9: This was necessary to support MSVC _stscanf (sscanf - swscanf) beahavior which expects DWORD arguments.
                    DWORD ip[4] = { 0, 0, 0, 0 };
                    _stscanf(pItem->lpszCurValue, _T("%hhd.%hhd.%hhd.%hhd"),
                        &ip[0], &ip[1], &ip[2], &ip[3]);
                    pgi.lpCurValue = (LPARAM)  MAKEIPADDRESS(ip[0],ip[1],ip[2],ip[3]);
                }
                    break;
                case PIT_DATE:
                    memset(&st, 0, sizeof(SYSTEMTIME));
                    _stscanf(pItem->lpszCurValue, _T("%hd/%hd/%hd"),
                        &st.wMonth, &st.wDay, &st.wYear);
                    pgi.lpCurValue = (LPARAM) & st;
                    break;
                case PIT_TIME:
                    memset(&st, 0, sizeof(SYSTEMTIME));
                    _tmemset(outbuf, (TCHAR)0, MAX_PATH);
                    _stscanf(pItem->lpszCurValue,
#ifdef _UNICODE
                        _T("%hd:%hd:%hd %2ls"),
#else
                        _T("%hd:%hd:%hd %2s"),
#endif
                        &st.wHour, &st.wMinute, &st.wSecond, (LPTSTR)&outbuf);
                    if ((0 == _tcsicmp(_T("PM"), outbuf)) && st.wHour != 12)//DWM 1.6:Added st.wHour != 12
                        st.wHour += 12;

                    pgi.lpCurValue = (LPARAM)&st;
                    break;
                case PIT_DATETIME:
                    memset(&st, 0, sizeof(SYSTEMTIME));
                    _tmemset(outbuf, (TCHAR)0, MAX_PATH);
                    _stscanf(pItem->lpszCurValue,
#ifdef _UNICODE
                        _T("%hd/%hd/%hd %hd:%hd:%hd %2ls"),
#else
                        _T("%hd/%hd/%hd %hd:%hd:%hd %2s"),
#endif
                        &st.wMonth, &st.wDay, &st.wYear,
                        &st.wHour, &st.wMinute, &st.wSecond, (LPTSTR)&outbuf);
                    if ((0 == _tcsicmp(_T("PM"), outbuf)) && st.wHour != 12)//DWM 1.6:Added st.wHour != 12
                        st.wHour += 12;

                    pgi.lpCurValue = (LPARAM) & st;
                    break;
            }
        }
        return (LRESULT) & pgi;
    }
    else
        return LB_ERR;
}

/// @brief Handles LB_GETSEL message sent to the grid.
///
/// @param iItem The zero-based index of the item.
///
/// @returns BOOL TRUE if the item is selected, FALSE if not, or LB_ERR
///           if an error occurs.
static BOOL Grid_OnGetSel(INT iItem)
{
    LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(g_lpInst->hwndListMap, iItem);
    if (NULL != pItem)
    {
        if (PIT_CATALOG != pItem->iItemType)
        {
            INT i = ListBox_FindItemData(g_lpInst->hwndListBox, 0, pItem);
            if (LB_ERR != i)
                return ListBox_GetSel(g_lpInst->hwndListBox, i);
            else
                return FALSE;   //Collapsed Not selected
        }
    }
    return LB_ERR;
}

/// @brief Handles LB_RESETCONTENT message sent to the grid.
///
/// @returns VOID.
static VOID Grid_OnResetContent(VOID)
{
    ListBox_ResetContent(g_lpInst->hwndListMap);

    if (NULL != g_lpInst->hwndCtl1)
    {
        DestroyWindow(g_lpInst->hwndCtl1);
        g_lpInst->hwndCtl1 = NULL;
    }
    if (NULL != g_lpInst->hwndCtl2)
    {
        DestroyWindow(g_lpInst->hwndCtl2);
        g_lpInst->hwndCtl2 = NULL;
    }
    ListBox_ResetContent(g_lpInst->hwndListBox);
    Static_SetText(g_lpInst->hwndPropDesc,_T("")); //DWM 1.2: Clear the property pane
}

/// @brief Handles LB_SETCURSEL message sent to the grid.
///
/// @param iItem The zero-based index of the item to select, or –1 to clear the selection.
///
/// @returns LRESULT If an error occurs, the return value is LB_ERR.
///                   If the index parameter is –1, the return value is LB_ERR
///                   even though no error occurred.
static LRESULT Grid_OnSetCurSel(INT iItem)
{
    LPLISTBOXITEM pItem = ListBox_GetItemDataSafe(g_lpInst->hwndListMap, iItem);
    if (NULL != pItem)
    {
        if (PIT_CATALOG != pItem->iItemType)
        {
            INT i = ListBox_FindItemData(g_lpInst->hwndListBox, 0, pItem);
            if (LB_ERR != i)
            {
                INT ret = ListBox_SetCurSel(g_lpInst->hwndListBox, i);
                FORWARD_WM_COMMAND(GetParent(g_lpInst->hwndListBox),
                    GetDlgCtrlID(g_lpInst->hwndListBox), g_lpInst->hwndListBox,
                    LBN_SELCHANGE, SNDMSG);

                if(PIT_CHECK == pItem->iItemType)
                    pItem->lpszMisc = SELECT; //DWM 1.2: Ensure Checkbox selected

                Refresh(g_lpInst->hwndListBox);
                return ret;
            }
        }
    }
    return LB_ERR;
}

/// @brief Handles LB_ADDSTRING message sent to the grid.
///
/// @param iItem The zero-based index of the item.
/// @param pgi A pointer to a PROPGRIDITEM object.
///
/// @returns LRESULT The return value is the zero-based index of the item in
///                   the list. If an error occurs, the return value is LB_ERR.
///                   If there is insufficient space to store the data,
///                   the return value is LB_ERRSPACE.
static LRESULT Grid_OnSetItemData(INT iItem, LPPROPGRIDITEM pgi)
{
    if (PIT_CATALOG == pgi->iItemType)
        return LB_ERR;  //Can't change an item to a catalog

    LRESULT lrtn = LB_ERR;
    LPLISTBOXITEM lpiCurrent, lpiNew;
    TCHAR buf[MAX_PATH];
    _tmemset(buf, (TCHAR)0, MAX_PATH);

    LPTSTR lpszCurValue = _T("");

    //Get the current item
    lpiCurrent = ListBox_GetItemDataSafe(g_lpInst->hwndListMap, iItem);
    if (NULL != lpiCurrent)
    {
        switch (pgi->iItemType)
        {
            case PIT_EDIT:
            case PIT_STATIC:
            case PIT_COMBO:
            case PIT_EDITCOMBO:
            case PIT_FOLDER:
                lpszCurValue = (LPTSTR)pgi->lpCurValue;
                break;
            case PIT_COLOR:
                _stprintf(buf, MAX_PATH, _T("%d,%d,%d"),
                    GetRValue((COLORREF)pgi->lpCurValue),
                    GetGValue((COLORREF)pgi->lpCurValue),
                    GetBValue((COLORREF)pgi->lpCurValue));
                lpszCurValue = buf;
                break;
            case PIT_FONT:
                lpszCurValue = LogFontItem_ToString((LPPROPGRIDFONTITEM)pgi->lpCurValue);
                break;
            case PIT_CHECK:
                lpszCurValue = (BOOL)pgi->lpCurValue ? CHECKED : UNCHECKED;
                break;
            case PIT_FILE:
                pgi->lpszzCmbItems = FileDialogItem_ToString((LPPROPGRIDFDITEM)pgi->lpCurValue);
                lpszCurValue = ((LPPROPGRIDFDITEM)pgi->lpCurValue)->lpszFilePath;
                break;
            case PIT_IP:
                _stprintf(buf, MAX_PATH, _T("%d.%d.%d.%d"),
                    FIRST_IPADDRESS((DWORD)pgi->lpCurValue),
                    SECOND_IPADDRESS((DWORD)pgi->lpCurValue),
                    THIRD_IPADDRESS((DWORD)pgi->lpCurValue),
                    FOURTH_IPADDRESS((DWORD)pgi->lpCurValue));
                lpszCurValue = buf;
                break;
            case PIT_DATE:
                GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE,
                    (LPSYSTEMTIME)pgi->lpCurValue, NULL, buf, MAX_PATH);
                lpszCurValue = buf;
                break;
            case PIT_TIME:
                GetTimeFormat(LOCALE_USER_DEFAULT, 0,
                    (LPSYSTEMTIME)pgi->lpCurValue, _T("hh':'mm':'ss tt"),
                    buf, MAX_PATH);
                lpszCurValue = buf;
                break;
            case PIT_DATETIME:
                GetDateFormat(LOCALE_USER_DEFAULT, DATE_SHORTDATE,
                    (LPSYSTEMTIME)pgi->lpCurValue, NULL, buf, MAX_PATH);
                _tcscat(buf, _T(" "));
                GetTimeFormat(LOCALE_USER_DEFAULT, 0, (LPSYSTEMTIME)pgi->lpCurValue,
                    _T("hh':'mm':'ss tt"), (LPTSTR) (buf + _tcslen(buf)),
                    MAX_PATH - _tcslen(buf));
                lpszCurValue = buf;
                break;
        }
        //Do not allow the catalog to change; to do so would necessitate moving
        // the item to a different index throwing off the indexing for items.
        lpiNew = NewItem(lpiCurrent->lpszCatalog, pgi->lpszPropName,
            lpszCurValue, pgi->lpszzCmbItems, pgi->lpszPropDesc, pgi->iItemType);

        //Set new data
        lrtn = ListBox_SetItemData(g_lpInst->hwndListMap, iItem, lpiNew);
        if(LB_ERR != lrtn)
        {
            lrtn = ListBox_FindItemData(g_lpInst->hwndListBox,0,lpiCurrent);
            if(LB_ERR != lrtn)
            {
                lrtn = ListBox_SetItemData(g_lpInst->hwndListBox, lrtn, lpiNew);

                //DWM 1.3: Refresh Display of item
                Refresh(g_lpInst->hwndListBox);
            }
        }
        if(LB_ERR == lrtn)
            return LB_ERR;

        //Reset lpCurrent if necessary
        if(lpiCurrent == g_lpInst->lpCurrent)
            g_lpInst->lpCurrent = lpiNew;

        //Delete old data
        DeleteItem(lpiCurrent);
    }
    return lrtn;
}

#pragma endregion public interface handlers

#pragma region create destroy

/// @brief Handles WM_CREATE message.
///
/// @param hwnd Handle of grid.
/// @param lpCreateStruct Pointer to a structure with creation data.
///
/// @returns BOOL If an application processes this message,
///                it should return TRUE to continue creation of the window.
static BOOL Grid_OnCreate(HWND hwnd, LPCREATESTRUCT lpCreateStruct)
{
    INSTANCEDATA inst;
    memset(&inst, 0, sizeof(INSTANCEDATA));

    inst.hInstance = lpCreateStruct->hInstance;
    inst.hwndParent = lpCreateStruct->hwndParent;
    inst.fXpOrLower = IsWindowsXPorLower(); // DWM 1.4: Added
    inst.fGotFocus = FALSE;
    inst.fScrolling = FALSE;
    inst.fTracking = FALSE;
    inst.iDescHeight = HEIGHT_DESC;
    inst.iPrevSel = LB_ERR; //Nothing selected

    //Create the listbox control
    inst.hwndListBox = CreateListBox(lpCreateStruct->hInstance, hwnd, ID_LISTBOX);
    if (NULL == inst.hwndListBox)
        return FALSE;

    //And the hidden list map
    inst.hwndListMap = CreateListMap(lpCreateStruct->hInstance, hwnd, ID_LISTMAP);
    if (NULL == inst.hwndListMap)
        return FALSE;

    return Control_CreateInstanceData(hwnd, &inst);
}

/// @brief Handles WM_DESTROY message.
///
/// @param hwnd Handle of Grid.
///
/// @returns VOID.
static VOID Grid_OnDestroy(HWND hwnd)
{
    if (NULL != g_lpInst)
    {
        ListBox_ResetContent(g_lpInst->hwndListMap); //Delete all Items
        DestroyWindow(g_lpInst->hwndListMap);
        DestroyWindow(g_lpInst->hwndListBox);

        if (NULL != g_lpInst->hwndToolTip)
            DestroyWindow(g_lpInst->hwndToolTip);
        if (NULL != g_lpInst->hwndPropDesc)
            DestroyWindow(g_lpInst->hwndPropDesc);
        if (NULL != g_lpInst->hwndCtl1)
            DestroyWindow(g_lpInst->hwndCtl1);
        if (NULL != g_lpInst->hwndCtl2)
            DestroyWindow(g_lpInst->hwndCtl2);

        Control_FreeInstanceData(hwnd);

        //DWM 1.3: Removed PostQuitMessage() call.
    }
}

/// @brief Initialize and register the property grid class.
///
/// @param hInstance Handle of application instance.
///
/// @returns ATOM If the function succeeds, the atom that uniquely identifies
///                the class being registered, otherwise 0.
PROPGRID_LINKAGE ATOM InitPropertyGrid(HINSTANCE hInstance)
{
    WNDCLASSEX wcex;

    // Get standard listbox information
    wcex.cbSize = sizeof(wcex);
    if (!GetClassInfoEx(NULL, WC_LISTBOX, &wcex))
        return 0;

    // Add our own stuff
    wcex.lpfnWndProc = (WNDPROC)Grid_Proc;;
    wcex.hInstance = hInstance;
    wcex.lpszClassName = g_szClassName;

    // Register our new class
    return RegisterClassEx(&wcex);
}

/// @brief Create an new instance of the property grid.
///
/// @param hParent Handle of the grid's parent.
/// @param dwID The ID for this control.
///
/// @returns HWND If the function succeeds, the grid handle, otherwise NULL.
PROPGRID_LINKAGE HWND New_PropertyGrid(HWND hParent, DWORD dwID)
{
    static ATOM aPropertyGrid = 0;
    static HWND hPropertyGrid;

    HINSTANCE hinst = (HINSTANCE)GetWindowLongPtr(hParent, GWLP_HINSTANCE);

    //Only need to register the property grid once
    if (!aPropertyGrid)
        aPropertyGrid = InitPropertyGrid(hinst);

    hPropertyGrid = CreateWindowEx(0, g_szClassName, _T(""),
         WS_CHILD | WS_TABSTOP, 0, 0, 0, 0, hParent, (HMENU)dwID, hinst, NULL);

    return hPropertyGrid;
}

#pragma endregion create destroy

/// @brief Send a PGN_PROPERTYCHANGE notification to grid's parent.
///
/// @returns VOID.
static VOID Grid_NotifyParent(VOID)
{
    static NMPROPGRID nmPropGrid;

    // Notify of the change
    if (NULL != g_lpInst->lpCurrent &&
        PIT_CATALOG != g_lpInst->lpCurrent->iItemType &&
        PIT_STATIC != g_lpInst->lpCurrent->iItemType)
    {
        nmPropGrid.hdr.hwndFrom = GetParent(g_lpInst->hwndListBox);
        nmPropGrid.hdr.idFrom = GetDlgCtrlID(nmPropGrid.hdr.hwndFrom);
        nmPropGrid.hdr.code = PGN_PROPERTYCHANGE;

        LRESULT lres = ListBox_FindItemData(g_lpInst->hwndListMap, 0, g_lpInst->lpCurrent);
        nmPropGrid.iIndex = LB_ERR == lres ? -1 : lres;

        FORWARD_WM_NOTIFY(g_lpInst->hwndParent, nmPropGrid.hdr.idFrom, &nmPropGrid, SNDMSG);
    }
}

/// @brief Window procedure for the visible owner-drawn listbox control.
///
/// @param hList Handle of listbox.
/// @param msg Which message?
/// @param wParam Message parameter.
/// @param lParam Message parameter.
///
/// @returns LRESULT depends on message.
static LRESULT CALLBACK ListBox_Proc(HWND hList, UINT msg,
        WPARAM wParam, LPARAM lParam)
{
    HWND hParent = GetParent(hList);

    // Note: Instance data is attached to listbox's parent
    Control_GetInstanceData(hParent, &g_lpInst);

    switch (msg)
    {
        HANDLE_MSG(hList, WM_COMMAND, ListBox_OnCommand);
        HANDLE_MSG(hList, WM_LBUTTONDOWN, ListBox_OnLButtonDown);
        HANDLE_MSG(hList, WM_LBUTTONDBLCLK, ListBox_OnLButtonDown);
        HANDLE_MSG(hList, WM_LBUTTONUP, ListBox_OnLButtonUp);
        HANDLE_MSG(hList, WM_MOUSEMOVE, ListBox_OnMouseMove);
        HANDLE_MSG(hList, WM_KEYDOWN, ListBox_OnKeyDown);
        HANDLE_MSG(hList, WM_KILLFOCUS, ListBox_OnKillFocus);

        case WM_SETFOCUS: //DWM 1.3: Focus to ListBox
            g_lpInst->fGotFocus = TRUE;
            Refresh(g_lpInst->hwndListBox);
            return 0;

        case WM_MBUTTONDOWN:
        case WM_NCLBUTTONDOWN:
            //The listbox doesn't have a scrollbar component, it draws a scroll
            // bar in the non-client area of the control.  A mouse click in the
            // non-client area then, equals clicking on a scroll bar.  A click
            // on the middle mouse button equals pan, we'll handle that as if
            // it were a scroll event.
            ListBox_OnBeginScroll(hList);
            g_lpInst->fScrolling = TRUE;
            break;

        case WM_SETCURSOR:
            //Whenever the mouse leaves the non-client area of a listbox, it
            // fires a WM_SETCURSOR message.  The same happens when the middle
            // mouse button is released.  We can use this behavior to detect the
            // completion of a scrolling operation.
            if (g_lpInst->fScrolling)
            {
                ListBox_OnEndScroll(hList);
                g_lpInst->fScrolling = FALSE;
            }
            break;

        case WM_GETDLGCODE:
            return DLGC_WANTALLKEYS;

        case WM_DESTROY: //Unsubclass the listbox Control
        {
            SetWindowLongPtr(hList, GWLP_WNDPROC, (DWORD_PTR)GetProp(hList, WPRC)); //DWM 1.5: fixed cast
            RemoveProp(hList, WPRC);
            return 0;
        }
    }
    return DefProc(hList, msg, wParam, lParam);
}

/// @brief Window procedure and public interface for the property grid.
///
/// @param hwnd Handle of grid.
/// @param msg Which message?
/// @param wParam Message parameter.
/// @param lParam Message parameter.
///
/// @returns LRESULT depends on message.
static LRESULT CALLBACK Grid_Proc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    Control_GetInstanceData(hwnd, &g_lpInst);//Update the instance pointer

    switch (msg)
    {
        HANDLE_MSG(hwnd, WM_COMMAND, Grid_OnCommand);
        HANDLE_MSG(hwnd, WM_CREATE, Grid_OnCreate);
        HANDLE_MSG(hwnd, WM_DESTROY, Grid_OnDestroy);
        HANDLE_MSG(hwnd, WM_SIZE, Grid_OnSize);
        HANDLE_MSG(hwnd, WM_CTLCOLORLISTBOX, Grid_OnCtlColorListbox);
        HANDLE_MSG(hwnd, WM_CTLCOLORSTATIC, Grid_OnCtlColorStatic);
        HANDLE_MSG(hwnd, WM_DRAWITEM, Grid_OnDrawItem);
        HANDLE_MSG(hwnd, WM_MEASUREITEM, Grid_OnMeasureItem);
        HANDLE_MSG(hwnd, WM_DELETEITEM, Grid_OnDeleteItem);
        HANDLE_MSG(hwnd, WM_LBUTTONDOWN, Grid_OnLButtonDown);
        HANDLE_MSG(hwnd, WM_LBUTTONUP, Grid_OnLButtonUp);
        HANDLE_MSG(hwnd, WM_MOUSEMOVE, Grid_OnMouseMove);
        HANDLE_MSG(hwnd, WM_SETCURSOR, Grid_OnSetCursor);
        HANDLE_MSG(hwnd, WM_SHOWWINDOW, Grid_OnShowWindow);

        case WM_SETFOCUS: //DWM 1.3: Focus to ListBox
            SetFocus(g_lpInst->hwndListBox);
            return 0;
        case LB_ADDSTRING: //PropGrid_AddItem
            return Grid_OnAddString((LPPROPGRIDITEM)lParam);
        case LB_DELETESTRING: //PropGrid_DeleteItem
            return Grid_OnDeleteString((INT)wParam);
        case LB_GETCOUNT: //PropGrid_GetCount
            return ListBox_GetCount(g_lpInst->hwndListMap);
        case LB_GETCURSEL: //PropGrid_GetCurSel
            return Grid_OnGetCurSel();
        case LB_GETHORIZONTALEXTENT: //PropGrid_GetHorizontalExtent
            return ListBox_GetHorizontalExtent(g_lpInst->hwndListBox);
        case LB_GETITEMDATA: //PropGrid_GetItemData
            return Grid_OnGetItemData((INT)wParam);
        case LB_GETITEMHEIGHT: //PropGrid_GetItemHeight
            return ListBox_GetItemHeight(g_lpInst->hwndListBox, wParam);
        case LB_GETITEMRECT: //PropGrid_GetItemRect
            return ListBox_GetItemRect(g_lpInst->hwndListBox, wParam, lParam);
        case LB_GETSEL: //PropGrid_GetSel
            return Grid_OnGetSel((INT)wParam);
        case LB_RESETCONTENT: //PropGrid_ResetContent
            Grid_OnResetContent();
            break;
        case LB_SETCURSEL: //PropGrid_SetCurSel
            return Grid_OnSetCurSel((INT)wParam);
        case LB_SETHORIZONTALEXTENT: //PropGrid_SetHorizontalExtent
            ListBox_SetHorizontalExtent(g_lpInst->hwndListBox, wParam);
            break;
        case LB_SETITEMDATA: //PropGrid_SetItemData
            return Grid_OnSetItemData((INT)wParam, (LPPROPGRIDITEM)lParam);
        case LB_SETITEMHEIGHT: //PropGrid_SetItemHeight
        {
            if (MINIMUM_ITEM_HEIGHT > LOWORD(lParam))
                return LB_ERR;
            LRESULT lres = SNDMSG(g_lpInst->hwndListBox, msg, wParam, lParam);
            Refresh(g_lpInst->hwndListBox);
            return lres;
        }
        case PG_FLATCHECKS: //DWM 1.8: Added
        {
            DWORD dwUserData = (DWORD)GetWindowLongPtr(g_lpInst->hwndListBox, GWLP_USERDATA);
            if (FALSE != (BOOL)wParam)
                dwUserData |= FLATCHECKS;
            else
                dwUserData &= ~FLATCHECKS;

            return SetWindowLongPtr(g_lpInst->hwndListBox, GWLP_USERDATA,
                        (LONG_PTR)dwUserData);
        }
        case PG_EXPANDCATALOGS:
        {
            if (NULL == (LPTSTR)lParam) //Expand all
            {
                Grid_ExpandCatalog(NULL);
            }
            else
            {
                //Walk the catalog list and handle each string until the empty string
                for (LPTSTR p = (LPTSTR)lParam; *p; p += _tcslen(p) + 1)
                    Grid_ExpandCatalog((LPCTSTR)p);
            }
        }
            break;
        case PG_COLLAPSECATALOGS:
        {
            if (NULL == (LPTSTR)lParam) //Collapse all
            {
                Grid_CollapseCatalog(NULL);
            }
            else
            {
                for (LPTSTR p = (LPTSTR)lParam; *p; p += _tcslen(p) + 1)
                    Grid_CollapseCatalog((LPCTSTR)p);
            }
        }
            break;
        case PG_SHOWTOOLTIPS:
        {
            if ((BOOL)wParam)
            {
                if (NULL == g_lpInst->hwndToolTip)
                {
                    g_lpInst->hwndToolTip = CreateToolTip(g_lpInst->hInstance, g_lpInst->hwndListBox);

                    if (NULL != g_lpInst->hwndToolTip)
                    {
                        TOOLINFO ti = { sizeof(ti) };
                        ti.hwnd = g_lpInst->hwndListBox;
                        ti.uId = 0;
                        GetClientRect(hwnd, &ti.rect);
                        ToolTip_NewToolRect(g_lpInst->hwndToolTip, &ti);

                        ShowWindow(g_lpInst->hwndToolTip, SW_SHOW);
                    }
                }
            }
            else
            {
                DestroyWindow(g_lpInst->hwndToolTip);
                g_lpInst->hwndToolTip = NULL;
            }
        }
            break;
        case PG_SHOWPROPERTYDESC:
        {
            if ((BOOL)wParam)
            {
                if (NULL == g_lpInst->hwndPropDesc)
                    g_lpInst->hwndPropDesc = CreateStatic(g_lpInst->hInstance, hwnd, ID_PROPDESC);

                ShowWindow(g_lpInst->hwndPropDesc, SW_SHOW);
            }
            else
            {
                DestroyWindow(g_lpInst->hwndPropDesc);
                g_lpInst->hwndPropDesc = NULL;
            }
            //Call Grid_OnSize() to position and display the control
            RECT rc;
            GetClientRect(hwnd, &rc);
            Grid_OnSize(hwnd, 0, WIDTH(rc), HEIGHT(rc));
        }
            break;
    }
    return DefWindowProc(hwnd, msg, wParam, lParam);
}
