//////////////////////////////////////////////////////////////////////////////
///
/// @file propertyGrid.h
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

//DWM 1.8: Added Message PG_FLATCHECKS and associated macroes

#ifndef PROPERTYGRID_H 
#define PROPERTYGRID_H

/****************************************************************************/
// Public Messages

// List box message subset handled by the property grid.
 
// LB_ADDSTRING - PropGrid_AddItem()
// LB_DELETESTRING - PropGrid_DeleteItem()
// LB_GETCOUNT - PropGrid_GetCount()
// LB_GETCURSEL - PropGrid_GetCurSel()
// LB_GETHORIZONTALEXTENT - PropGrid_GetHorizontalExtent()
// LB_GETITEMDATA - PropGrid_GetItemData()
// LB_GETITEMHEIGHT - PropGrid_GetItemHeight()
// LB_GETITEMRECT - PropGrid_GetItemRect()
// LB_GETSEL - PropGrid_GetSel()
// LB_RESETCONTENT - PropGrid_ResetContent()
// LB_SETCURSEL - PropGrid_SetCurSel()
// LB_SETHORIZONTALEXTENT - PropGrid_SetHorizontalExtent()
// LB_SETITEMDATA - PropGrid_SetItemData()
// LB_SETITEMHEIGHT - PropGrid_SetItemHeight()

/// @name Property grid specific messages.
/// @{
#define PG_EXPANDCATALOGS WM_USER + 0x01   ///<PropGrid_ExpandCatalogs()
#define PG_COLLAPSECATALOGS WM_USER + 0x02 ///<PropGrid_CollapseCatalogs()
#define PG_SHOWTOOLTIPS WM_USER + 0x03     ///<PropGrid_ShowToolTips()
#define PG_SHOWPROPERTYDESC WM_USER + 0x04 ///<PropGrid_ShowPropertyDescriptions()
#define PG_FLATCHECKS  WM_USER + 0x05      ///<PropGrid_SetFlatStyleChecks()
/// @}

/****************************************************************************/
// Property item types

#define PIT_EDIT        0   ///< Property item type: Edit
#define PIT_COMBO       1   ///< Property item type: Dropdownlist
#define PIT_EDITCOMBO   2   ///< Property item type: Dropdown(editable)
#define PIT_STATIC      3   ///< Property item type: Not editable text
#define PIT_COLOR       4   ///< Property item type: Color
#define PIT_FONT        5   ///< Property item type: Font
#define PIT_FILE        6   ///< Property item type: File select dialog
#define PIT_FOLDER      7   ///< Property item type: Folder select dialog
#define PIT_CHECK       8   ///< Property item type: BOOL
#define PIT_IP          9   ///< Property item type: IP Address
#define PIT_DATE        10  ///< Property item type: Date
#define PIT_TIME        11  ///< Property item type: Time
#define PIT_DATETIME    12  ///< Property item type: Date & Time
#define PIT_CATALOG     99  ///< Property item type: Catalog

/****************************************************************************/
// Public structures and notifications

/// @var PROPGRIDFDITEM
/// @brief A property grid file dialog item object

/// @var LPPROPGRIDFDITEM
/// @brief Pointer to a property grid file dialog item

/// @struct tagPROPGRIDFDITEM
/// @brief This is additional data associated with a property grid file dialog item
typedef struct tagPROPGRIDFDITEM {
    LPTSTR lpszDlgTitle; ///< Dialog title
    LPTSTR lpszFilePath; ///< Initial path
    LPTSTR lpszFilter;   ///< Double null terminated filter string
    LPTSTR lpszDefExt;   ///< Default extension
} PROPGRIDFDITEM, *LPPROPGRIDFDITEM;

/// @var PROPGRIDFONTITEM
/// @brief A property grid font item object

/// @var LPPROPGRIDFONTITEM
/// @brief Pointer to a property grid font item

/// @struct tagPROPGRIDFONTITEM
/// @brief This is additional data associated with a property grid font item
typedef struct tagPROPGRIDFONTITEM {
    LOGFONT logFont; ///< Logical font struct
    COLORREF crFont; ///< Text color
} PROPGRIDFONTITEM, *LPPROPGRIDFONTITEM;

/// @var PROPGRIDITEM
/// @brief A property grid item object

/// @var LPPROPGRIDITEM
/// @brief Pointer to a property grid item

/// @struct tagPROPGRIDITEM
/// @brief This is the data associated with a property grid item
typedef struct tagPROPGRIDITEM {
    LPTSTR lpszCatalog;   ///< Catalog (group) name
    LPTSTR lpszPropName;  ///< Property (item) name
    LPTSTR lpszzCmbItems; ///< Double null terminated list of strings
    LPTSTR lpszPropDesc;  ///< Property (item) description
    LPARAM lpCurValue;    ///< Property (item) value
    INT    iItemType;     ///< Property (item) type identifier
} PROPGRIDITEM, *LPPROPGRIDITEM;

/// @var NMPROPGRID
/// @brief A property grid notification message data object

/// @var LPNMPROPGRID
/// @brief Pointer to property grid notification message data

/// @struct tagNMPROPGRID
/// @brief This is the data associated with a property grid notification
typedef struct tagNMPROPGRID {
    NMHDR hdr;  ///< Notification message header
    INT iIndex; ///< Index of a property grid item
} NMPROPGRID, *LPNMPROPGRID;

#define PGN_PROPERTYCHANGE WM_USER + 0x2A  ///<property grid Property changed notification message

/****************************************************************************/
/// @name Macroes
/// @{

/// @def PropGrid_AddItem(hwndCtl,lpItem)
///
/// @brief Add an item to a property grid.  Items are appended to their respective
///         catalogs.
///
/// @param hwndCtl The handle of a property grid.
/// @param lpItem Pointer to a property grid item.
///
/// @returns The zero-based index of the item in the list box. If an error occurs,
///           the return value is LB_ERR. If there is insufficient space to store
///           the new string, the return value is LB_ERRSPACE. 
#define PropGrid_AddItem(hwndCtl,lpItem) ((int)(DWORD)SendMessage((hwndCtl),LB_ADDSTRING,0,(LPARAM)(lpItem)))

/// @def PropGrid_DeleteItem(hwndCtl,index)
///
/// @brief Deletes the item at the specified location in a property grid.
///
/// @param hwndCtl The handle of a property grid.
/// @param index The zero-based index of the item to delete.
///
/// @returns A count of the items remaining in the grid. The return value is
///           LB_ERR if the index parameter specifies an index greater than the
///           number of items in the list. 
#define PropGrid_DeleteItem(hwndCtl,index) ((int)(DWORD)SendMessage((hwndCtl),LB_DELETESTRING,(WPARAM)(int)(index),0))

/// @def PropGrid_Enable(hwndCtl,fEnable)
///
/// @brief Enables or disables a property grid control.
///
/// @param hwndCtl The handle of a property grid.
/// @param fEnable TRUE to enable the control, or FALSE to disable it.
///
/// @returns No return value.
#define PropGrid_Enable(hwndCtl,fEnable) EnableWindow((hwndCtl),(fEnable))

/// @def PropGrid_GetCount(hwndCtl)
///
/// @brief Gets the number of items in a property grid.
///
/// @param hwndCtl The handle of a property grid.
///
/// @returns The number of items.
#define PropGrid_GetCount(hwndCtl) ((int)(DWORD)SendMessage((hwndCtl),LB_GETCOUNT,0,0))

/// @def PropGrid_GetCurSel(hwndCtl)
///
/// @brief Gets the index of the currently selected item in a property grid.
///
/// @param hwndCtl The handle of a property grid.
///
/// @returns The zero-based index of the selected item. If there is no selection,
///           the return value is LB_ERR.
#define PropGrid_GetCurSel(hwndCtl) ((int)(DWORD)SendMessage((hwndCtl),LB_GETCURSEL,0,0))

/// @def PropGrid_GetHorizontalExtent(hwndCtl)
///
/// @brief Gets the width that a property grid can be scrolled horizontally
///         (the scrollable width).
///
/// @param hwndCtl The handle of a property grid.
///
/// @returns The scrollable width, in pixels, of the property grid.
#define PropGrid_GetHorizontalExtent(hwndCtl) ((int)(DWORD)SendMessage((hwndCtl),LB_GETHORIZONTALEXTENT,0,0))

/// @def PropGrid_GetItemData(hwndCtl,index)
///
/// @brief Gets the PROPGRIDITEM associated with the specified property grid item.
///
/// @param hwndCtl The handle of a property grid.
/// @param index The zero-based index of the item.
///
/// @returns A pointer to a PROPGRIDITEM object.
#define PropGrid_GetItemData(hwndCtl,index) ((LPPROPGRIDITEM)(DWORD_PTR)SendMessage((hwndCtl),LB_GETITEMDATA,(WPARAM)(int)(index),0))

/// @def PropGrid_GetItemHeight(hwndCtl)
///
/// @brief Retrieves the height of all items in a property grid.
///
/// @param hwndCtl The handle of a property grid.
///
/// @returns The height, in pixels, of the items, or LB_ERR if an error occurs.
#define PropGrid_GetItemHeight(hwndCtl) ((int)(DWORD)SendMessage((hwndCtl),LB_GETITEMHEIGHT,(WPARAM)0,0))

/// @def PropGrid_GetItemRect(hwndCtl,index,lprc)
///
/// @brief Gets the dimensions of the rectangle that bounds a property grid item
///         as it is currently displayed in the property grid.
///
/// @param hwndCtl The handle of a property grid.
/// @param index The zero-based index of the item in the property grid.
/// @param lprc A pointer to a RECT structure that receives the client
///              coordinates for the item in the property grid.
///
/// @returns If an error occurs, the return value is LB_ERR.
#define PropGrid_GetItemRect(hwndCtl,index,lprc) ((int)(DWORD)SendMessage((hwndCtl),LB_GETITEMRECT,(WPARAM)(int)(index),(LPARAM)(RECT*)(lprc)))

/// @def PropGrid_GetSel(hwndCtl,index)
///
/// @brief Gets the selection state of an item.
///
/// @param hwndCtl The handle of a property grid.
/// @param index The zero-based index of the item.
///
/// @returns If the item is selected, the return value is greater than zero;
///           otherwise, it is zero. If an error occurs, the return value is LB_ERR.
#define PropGrid_GetSel(hwndCtl,index) ((int)(DWORD)SendMessage((hwndCtl),LB_GETSEL,(WPARAM)(int)(index),0))

/// @def PropGrid_ResetContent(hwndCtl)
///
/// @brief Removes all items from a property grid.
///
/// @param hwndCtl The handle of a property grid.
///
/// @returns The return value is not meaningful.
#define PropGrid_ResetContent(hwndCtl) ((BOOL)(DWORD)SendMessage((hwndCtl),LB_RESETCONTENT,0,0))

/// @def PropGrid_SetCurSel(hwndCtl,index)
///
/// @brief Sets the currently selected item in a property grid.
///
/// @param hwndCtl The handle of a property grid.
/// @param index The zero-based index of the item to select, or –1 to clear the selection.
///
/// @returns If an error occurs, the return value is LB_ERR. If the index
///           parameter is –1, the return value is LB_ERR even though no error occurred. 
#define PropGrid_SetCurSel(hwndCtl,index) ((int)(DWORD)SendMessage((hwndCtl),LB_SETCURSEL,(WPARAM)(int)(index),0))

/// @def PropGrid_SetHorizontalExtent(hwndCtl,cxExtent)
///
/// @brief Set the width by which a property grid can be scrolled horizontally
///         (the scrollable width). If the width of the property grid is smaller
///         than this value, the horizontal scroll bar horizontally scrolls items
///         in the property grid. If the width of the property grid is equal to or
///         greater than this value, the horizontal scroll bar is hidden.
///
/// @param hwndCtl The handle of a property grid.
/// @param cxExtent The number of pixels by which the list box can be scrolled.
///
/// @returns No return value.
#define PropGrid_SetHorizontalExtent(hwndCtl,cxExtent) ((void)SendMessage((hwndCtl),LB_SETHORIZONTALEXTENT,(WPARAM)(int)(cxExtent),0))

/// @def PropGrid_SetItemData(hwndCtl,index,data)
///
/// @brief Sets the PROPGRIDITEM associated with the specified property grid item.
///
/// @param hwndCtl The handle of a property grid.
/// @param index The zero-based index of the item.
/// @param data The item data to set.
///
/// @returns If an error occurs, the return value is LB_ERR.
#define PropGrid_SetItemData(hwndCtl,index,data) ((int)(DWORD)SendMessage((hwndCtl),LB_SETITEMDATA,(WPARAM)(int)(index),(LPARAM)(data)))

/// @def PropGrid_SetItemHeight(hwndCtl,cy)
///
/// @brief Sets the height of all items in a property grid.
///
/// @param hwndCtl The handle of a property grid.
/// @param cy The height of the items, in pixels.
///
/// @returns If the height is invalid, the return value is LB_ERR.
#define PropGrid_SetItemHeight(hwndCtl,cy) ((int)(DWORD)SendMessage((hwndCtl),LB_SETITEMHEIGHT,(WPARAM)0,MAKELPARAM((cy),0)))

/// @def PropGrid_ExpandCatalogs(hwndCtl, lpszzCatalogs)
///
/// @brief Expand certain specified catalogs in a property grid.
///
/// @param hwndCtl The handle of a property grid.
/// @param lpszzCatalogs The list of catalog names each terminated by a null (\\0).
///
/// @returns No return value.
#define PropGrid_ExpandCatalogs(hwndCtl, lpszzCatalogs) ((void)SendMessage((hwndCtl),PG_EXPANDCATALOGS,(WPARAM)0,(LPARAM)(lpszzCatalogs)))

/// @def PropGrid_ExpandAllCatalogs(hwndCtl)
///
/// @brief Expand all catalogs in a property grid.
///
/// @param hwndCtl The handle of a property grid.
///
/// @returns No return value.
#define PropGrid_ExpandAllCatalogs(hwndCtl) ((void)SendMessage((hwndCtl),PG_EXPANDCATALOGS,(WPARAM)0,(LPARAM)NULL))

/// @def PropGrid_CollapseCatalogs(hwndCtl, lpszzCatalogs)
///
/// @brief Collapse certain specified catalogs in a property grid.
///
/// @param hwndCtl The handle of a property grid.
/// @param lpszzCatalogs The catalog names each terminated by a null (\\0).
///
/// @returns No return value.
#define PropGrid_CollapseCatalogs(hwndCtl, lpszzCatalogs) ((void)SendMessage((hwndCtl),PG_COLLAPSECATALOGS,(WPARAM)0,(LPARAM)(lpszzCatalogs)))

/// @def PropGrid_CollapseAllCatalogs(hwndCtl)
///
/// @brief Collapse all catalogs in a property grid.
///
/// @param hwndCtl The handle of a property grid.
///
/// @returns No return value.
#define PropGrid_CollapseAllCatalogs(hwndCtl) ((void)SendMessage((hwndCtl),PG_COLLAPSECATALOGS,(WPARAM)0,(LPARAM)NULL))

/// @def PropGrid_ShowToolTips(hwndCtl,fShow)
///
/// @brief Show or hide tooltips in the property grid.
///
/// @param hwndCtl The handle of a property grid.
/// @param fShow TRUE for tooltips; FALSE do not show tooltips.
///
/// @returns No return value.
#define PropGrid_ShowToolTips(hwndCtl,fShow) ((void)SendMessage((hwndCtl),PG_SHOWTOOLTIPS,(WPARAM)(fShow),(LPARAM)0L))

/// @def PropGrid_ShowPropertyDescriptions(hwndCtl,fShow)
///
/// @brief Show or hide the property discription pane in the property grid.
///
/// @param hwndCtl The handle of a property grid.
/// @param fShow TRUE for descriptions; FALSE do not show discription pane.
///
/// @returns No return value.
#define PropGrid_ShowPropertyDescriptions(hwndCtl,fShow) ((void)SendMessage((hwndCtl),PG_SHOWPROPERTYDESC,(WPARAM)(fShow),(LPARAM)0L))

/// @def PropGrid_SetFlatStyleChecks(hwndCtl, fFlat)
///
/// @brief Sets the appearance of the checkboxes.
///
/// @param hwndCtl The handle of a property grid.
/// @param fFlat TRUE for flat checkboxes, or FALSE for standard checkboxes.
///
/// @returns No return value.
#define PropGrid_SetFlatStyleChecks(hwndCtl, fFlat) \
     ((void)SendMessage((hwndCtl),PG_FLATCHECKS,(WPARAM)(BOOL) (fFlat),(LPARAM)0L))

/// @def PropGrid_ItemInit(pgi)
///
/// @brief Initialize an item struct.
///
/// @param pgi The PROPGRIDITEM struct.
///
/// @returns No return value.
#define PropGrid_ItemInit(pgi) \
    (pgi.lpszCatalog = NULL,  pgi.lpszPropName = NULL, \
    pgi.lpszzCmbItems = NULL,  pgi.lpszPropDesc = NULL, \
    pgi.lpCurValue = 0,  pgi.iItemType = 0)

/// @}

/****************************************************************************/
// Exported function prototypes

#ifdef __cplusplus
	#define PROPGRID_LINKAGE extern "C" 
#else
	#define PROPGRID_LINKAGE
#endif

PROPGRID_LINKAGE ATOM InitPropertyGrid(HINSTANCE hInstance);
PROPGRID_LINKAGE HWND New_PropertyGrid(HWND hParent, DWORD dwID);

#endif //PROPERTYGRID_H
