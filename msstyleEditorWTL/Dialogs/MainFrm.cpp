#include "MainFrm.h"
#include "AboutDlg.h"
#include "SearchDlg.h"

#include "..\SearchLogic.h"
#include "..\TreeItemData.h"
#include "..\Helper.h"

#include <gdiplus.h>
#include <gdiplusgraphics.h>

#define COMMON_WND_STYLE	(WS_CHILD | WS_VISIBLE | WS_CLIPSIBLINGS | WS_CLIPCHILDREN)
#define NOT_A_MENU			1

#define CHANGE_OK			0
#define CHANGE_VETO			1

#define TEXT_PLAY L"&Start Test"
#define TEXT_STOP L"&Stop Test"

CMainFrame::CMainFrame()
	: m_currentStyle(NULL)
	, m_selectedProperty(NULL)
	, m_imageViewMenu(NULL)
	, m_searchDialog(NULL)
{
}

void CMainFrame::OpenStyle(const CString& file)
{
	m_currentStyle = new libmsstyle::VisualStyle();

	try
	{
		std::wstring tmp(file.GetString());
		m_currentStyle->Load(StdWideToUTF8(tmp));
	}
	catch (std::exception& ex)
	{
		MessageBoxA(NULL, ex.what(), "Error loading style!", MB_OK | MB_ICONERROR);
		delete m_currentStyle;
		m_currentStyle = nullptr;
		return;
	}

	FillTreeView();
}

void CMainFrame::CloseStyle()
{
	if (m_currentStyle != nullptr)
	{
		try {
			m_themeManager.Rollback();
			//themeMenu->SetLabel(ID_THEME_APPLY, wxT(TEXT_PLAY));
		}
		catch (...)
		{
		}

		// remove everything that could still point to the style data
		ClearPropView();
		ClearTreeView();

		delete m_currentStyle;
		m_currentStyle = nullptr;
	}
}

HBITMAP MakeBitMapTransparent(HBITMAP hbmSrc)
{
	HDC hdcSrc, hdcDst;
	HBITMAP hbmOld, hbmNew;
	BITMAP bm;
	COLORREF clrTP, clrBK;

	if ((hdcSrc = CreateCompatibleDC(NULL)) != NULL) {
		if ((hdcDst = CreateCompatibleDC(NULL)) != NULL) {
			int nRow, nCol;
			GetObject(hbmSrc, sizeof(bm), &bm);
			hbmOld = (HBITMAP)SelectObject(hdcSrc, hbmSrc);
			hbmNew = CreateBitmap(bm.bmWidth, bm.bmHeight, bm.bmPlanes, bm.bmBitsPixel, NULL);
			SelectObject(hdcDst, hbmNew);

			BitBlt(hdcDst, 0, 0, bm.bmWidth, bm.bmHeight, hdcSrc, 0, 0, SRCCOPY);

			clrTP = GetPixel(hdcDst, 0, 0);// Get color of first pixel at 0,0
			clrBK = GetSysColor(COLOR_MENU);// Get the current background color of the menu

			for (nRow = 0; nRow < bm.bmHeight; nRow++)// work our way through all the pixels changing their color
				for (nCol = 0; nCol < bm.bmWidth; nCol++)// when we hit our set transparency color.
					if (GetPixel(hdcDst, nCol, nRow) == clrTP)
						SetPixel(hdcDst, nCol, nRow, clrBK);

			DeleteDC(hdcDst);
		}
		DeleteDC(hdcSrc);

	}
	return hbmNew;// return our transformed bitmap.
}

LRESULT CMainFrame::OnCreate(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
{
	ULONG_PTR gdiplusToken;
	Gdiplus::GdiplusStartupInput gdiplusStartupInput;
	GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, NULL);

	// create command bar window
	HWND hWndCmdBar = m_commandBar.Create(m_hWnd, rcDefault, NULL, ATL_SIMPLE_CMDBAR_PANE_STYLE);
	m_commandBar.AttachMenu(GetMenu());
	m_commandBar.LoadImages(IDR_MAINFRAME);
	SetMenu(NULL);


	m_themeSubMenu = GetSubMenu(m_commandBar.GetMenu(), 1);
	m_bmpStart = (HBITMAP)LoadImage(GetModuleHandle(NULL), MAKEINTRESOURCE(IDB_THEME_START), IMAGE_BITMAP, 16, 16, LR_SHARED);
	m_bmpStop = (HBITMAP)LoadImage(GetModuleHandle(NULL), MAKEINTRESOURCE(IDB_THEME_STOP), IMAGE_BITMAP, 16, 16, LR_SHARED);

	m_bmpStart = MakeBitMapTransparent(m_bmpStart);
	m_bmpStop = MakeBitMapTransparent(m_bmpStop);

	SetMenuItemBitmaps(m_themeSubMenu, 0, MF_BYPOSITION, m_bmpStart, m_bmpStop);


	//HWND hWndToolBar = CreateSimpleToolBarCtrl(m_hWnd, IDR_MAINFRAME, FALSE, ATL_SIMPLE_TOOLBAR_PANE_STYLE);

	CreateSimpleReBar(ATL_SIMPLE_REBAR_NOBORDER_STYLE);
	AddSimpleReBarBand(hWndCmdBar);
	//AddSimpleReBarBand(hWndToolBar, NULL, TRUE);

	CreateSimpleStatusBar();

	m_splitLeft.Create(m_hWnd, rcDefault, NULL, COMMON_WND_STYLE);
	m_splitRight.Create(m_splitLeft, rcDefault, NULL, COMMON_WND_STYLE);
	m_hWndClient = m_splitLeft;
	

	m_treeView.Create(m_splitLeft, rcDefault, NULL, COMMON_WND_STYLE
		| TVS_HASLINES 
		| TVS_LINESATROOT 
		| TVS_HASBUTTONS 
		| TVS_SHOWSELALWAYS);


	m_imageView.Create(m_splitRight, rcDefault, NULL, COMMON_WND_STYLE);
	m_imageView.SetBitmap(NULL);
	m_imageViewMenu = GetSubMenu(LoadMenu(NULL, MAKEINTRESOURCE(IDR_IMAGEVIEW)), 0);

	m_propListBase.Create(m_splitRight, rcDefault, NULL, COMMON_WND_STYLE
		| WS_VSCROLL
		| LBS_NOTIFY
		| LBS_OWNERDRAWVARIABLE 
		| LBS_NOINTEGRALHEIGHT
		, WS_EX_RIGHTSCROLLBAR
		, NOT_A_MENU // Important! Required for REFLECT_NOTIFICATION()
		, NULL);
	m_propList.SubclassWindow(m_propListBase);
	m_propList.SetExtendedListStyle(PLS_EX_CATEGORIZED | LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);
	m_propListMenu = GetSubMenu(LoadMenu(NULL, MAKEINTRESOURCE(IDR_PROPVIEW)), 0);

	HBITMAP hbmpPropAdd = (HBITMAP)LoadImage(GetModuleHandle(NULL), MAKEINTRESOURCE(IDB_PROP_ADD), IMAGE_BITMAP, 16, 16, LR_SHARED);
	HBITMAP hbmpPropRem = (HBITMAP)LoadImage(GetModuleHandle(NULL), MAKEINTRESOURCE(IDB_PROP_REMOVE), IMAGE_BITMAP, 16, 16, LR_SHARED);
	
	hbmpPropAdd = MakeBitMapTransparent(hbmpPropAdd);
	hbmpPropRem = MakeBitMapTransparent(hbmpPropRem);

	SetMenuItemBitmaps(m_propListMenu, 0, MF_BYPOSITION, hbmpPropAdd, hbmpPropAdd);
	SetMenuItemBitmaps(m_propListMenu, 1, MF_BYPOSITION, hbmpPropRem, hbmpPropRem);

	m_splitLeft.SetSplitterPanes(m_treeView, m_splitRight);
	m_splitRight.SetSplitterPanes(m_imageView, m_propList);
	m_splitLeft.SetSplitterPosPct(33);
	m_splitRight.SetSplitterPosPct(50);

	UISetCheck(ID_VIEW_TOOLBAR, 1);
	UISetCheck(ID_VIEW_STATUS_BAR, 1);

	// Resource editor has some unicode quirks?
	SetThemeTestMenuItemText(TEXT_PLAY, false);

	// register object for message filtering and idle updates
	CMessageLoop* pLoop = _Module.GetMessageLoop();
	ATLASSERT(pLoop != NULL);
	pLoop->AddMessageFilter(this);
	pLoop->AddIdleHandler(this);

	return 0;
}

LRESULT CMainFrame::OnDestroy(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& bHandled)
{
	ClearTreeView();

	if (m_currentStyle != nullptr)
	{
		delete m_currentStyle;
		m_currentStyle = nullptr;
	}

	// unregister message filtering and idle updates
	CMessageLoop* pLoop = _Module.GetMessageLoop();
	ATLASSERT(pLoop != NULL);
	pLoop->RemoveMessageFilter(this);
	pLoop->RemoveIdleHandler(this);

	bHandled = FALSE;
	return 1;
}

#pragma region Control Handling

LRESULT CMainFrame::OnContextMenu(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	POINT curPoint;
	GetCursorPos(&curPoint);

	RECT rectImgView, rectPropList;
	m_imageView.GetWindowRect(&rectImgView);
	m_propList.GetWindowRect(&rectPropList);

	if (PtInRect(&rectImgView, curPoint))
	{
		HWND source = reinterpret_cast<HWND>(wParam);
		BOOL res = TrackPopupMenu(m_imageViewMenu, TPM_RETURNCMD
			, curPoint.x
			, curPoint.y
			, 0
			, source
			, NULL);

		switch (res)
		{
		case ID_IMGBG_WHITE:
			m_imageView.SetBackgroundStyle(CImageCtrl::White);
			m_imageView.Invalidate();
			break;
		case ID_IMGBG_LGREY:
			m_imageView.SetBackgroundStyle(CImageCtrl::LightGrey);
			m_imageView.Invalidate();
			break;
		case ID_IMGBG_BLACK:
			m_imageView.SetBackgroundStyle(CImageCtrl::Black);
			m_imageView.Invalidate();
			break;
		case ID_IMGBG_CHESS:
			m_imageView.SetBackgroundStyle(CImageCtrl::Chessboard);
			m_imageView.Invalidate();
			break;
		}
	}
	else if (PtInRect(&rectPropList, curPoint))
	{
		POINT pt = { 0 };
		pt.x = GET_X_LPARAM(lParam);
		pt.y = GET_Y_LPARAM(lParam);
		m_propListBase.ScreenToClient(&pt);

		LRESULT res = ::SendMessage(m_propList, LB_ITEMFROMPOINT, 0, MAKELPARAM(pt.x, pt.y));

		int index = LOWORD(res);
		if (HIWORD(res) == 0)
		{
			m_propList.SetFocus();
			m_propList.SetCurSel(index);

			HWND source = reinterpret_cast<HWND>(wParam);
			BOOL res = TrackPopupMenu(m_propListMenu, TPM_RETURNCMD
				, curPoint.x
				, curPoint.y
				, 0
				, source
				, NULL);

			switch (res)
			{
			case ID_PROP_ADD:
				break;
			case ID_PROP_REMOVE:
				break;
			}
		}
	}

	return TRUE;
}

LRESULT CMainFrame::OnFindOpen(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	if (!m_searchDialog)
	{
		m_searchDialog = new CSearchDlg();
		m_searchDialog->Create(m_hWnd);
	}

	if (!m_searchDialog->IsWindowVisible())
	{
		m_searchDialog->ShowWindow(SW_SHOWNORMAL);
	}

	return 0;
}

HTREEITEM CMainFrame::DoFindNext(const SearchProperties* search, HTREEITEM node)
{
	HTREEITEM originalNode = node;
	while (node != NULL)
	{
		//CTreeItem aaa(node, &m_treeView);
		//CString strrr;
		//aaa.GetText(strrr);
		//MessageBox(strrr.GetString());

		// see whether the node contains "search.value" somewhere.
		// skip the first node to not get stuck.
		if (node != originalNode)
		{
			TreeItemData* data = reinterpret_cast<TreeItemData*>(m_treeView.GetItemData(node));
			switch (search->mode)
			{
			case SearchProperties::MODE_NAME:
			{
				if (ContainsName(search->text, data))
					return node;
			} break;
			case SearchProperties::MODE_PROPERTY:
			{
				if (ContainsProperty(*search, data))
					return node;
			} break;
			default:
				break;
			}
		}

		// find nodes: depth
		HTREEITEM nextNode = m_treeView.GetChildItem(node);
		if (!nextNode)
		{
			// find nodes: breadth
			nextNode = m_treeView.GetNextSiblingItem(node);
			if (!nextNode)
			{
				// TODO: the root node is one of the sibling and not a single parent

				// back out and try finding a node in the breadth
				HTREEITEM previous = node;
				nextNode = m_treeView.GetParentItem(node);
				while (nextNode && nextNode != m_treeView.GetRootItem() && !m_treeView.GetNextSiblingItem(nextNode))
				{
					nextNode = m_treeView.GetParentItem(nextNode);
				}

				if (nextNode) // parent ok, so we have a sibling
					nextNode = m_treeView.GetNextSiblingItem(nextNode);

				if (!nextNode)
					return NULL;
			}
		}

		node = nextNode;
	}

	return NULL;
}

bool endReached = false;
LRESULT CMainFrame::OnFindNext(UINT /*uMsg*/, WPARAM wParam, LPARAM /*lParam*/, BOOL& /*bHandled*/)
{
	if (m_currentStyle == nullptr)
		return 0;

	SearchProperties* props = (SearchProperties*)wParam;
	if (lstrlenW(props->text) == 0)
		return 0;

	HTREEITEM startItem = m_treeView.GetSelectedItem();
	if (startItem == NULL || endReached)
	{
		endReached = false;
		startItem = m_treeView.GetRootItem();
		if (startItem == NULL)
			return 0;
	}

	HTREEITEM item = DoFindNext(props, startItem);
	if (item != NULL)
	{
		m_treeView.SelectItem(item);
	}
	else
	{
		WCHAR textbuffer[128];
		wsprintf(textbuffer, L"No further match for \"%s\" !\nSearch will begin from top again.", props->text);
		MessageBox(textbuffer, L"Search - End reached", MB_OK | MB_ICONINFORMATION);
		endReached = true;
	}

	return 0;
}

LRESULT CMainFrame::OnTreeViewSelectionChanged(int idCtrl, LPNMHDR pnmh, BOOL& bHandled)
{
	USES_CONVERSION;

	LPNM_TREEVIEW evt = (LPNM_TREEVIEW)pnmh;

	WCHAR statusText[255];

	m_selectedProperty = nullptr;
	m_imageView.SetBitmap(NULL);
	SetStatusText(L"");

	TreeItemData* data = reinterpret_cast<TreeItemData*>(evt->itemNew.lParam);
	if (data == nullptr)
		return 0;

	HTREEITEM parentItem = m_treeView.GetParentItem(evt->itemNew.hItem);
	TreeItemData* parentData = reinterpret_cast<TreeItemData*>(m_treeView.GetItemData(parentItem));

	HTREEITEM grandparentItem = m_treeView.GetParentItem(parentItem);
	TreeItemData* grandparentData = reinterpret_cast<TreeItemData*>(m_treeView.GetItemData(grandparentItem));

	// Class Node
	if (data->type == ITEM_CLASS)
	{
		libmsstyle::StyleClass* classData = static_cast<libmsstyle::StyleClass*>(data->object);

		selection.ClassId = classData->classID;
		selection.PartId = -1;
		selection.StateId = -1;

		wsprintf(statusText, L"C: %d", selection.ClassId);
		SetStatusText(statusText);

		// Update UI
		ClearPropView();
	}

	// Part Node
	if (data->type == ITEM_PART)
	{
		libmsstyle::StylePart* part = static_cast<libmsstyle::StylePart*>(data->object);

		if (parentData->type == ITEM_CLASS)
		{
			selection.ClassId = static_cast<libmsstyle::StyleClass*>(parentData->object)->classID;
		}

		selection.PartId = part->partID;
		selection.StateId = -1;

		wsprintf(statusText, L"C: %d, P: %d", selection.ClassId, part->partID);
		SetStatusText(statusText);

		ClearPropView();
		FillPropView(*part);
	}

	// Image Node
	if (data->type == ITEM_PROPERTY)
	{
		m_selectedProperty = static_cast<libmsstyle::StyleProperty*>(data->object);

		if (parentData->type == ITEM_CLASS)
		{
			selection.ClassId = static_cast<libmsstyle::StyleClass*>(grandparentData->object)->classID;
		}

		if (parentData->type == ITEM_PART)
		{
			selection.PartId = static_cast<libmsstyle::StylePart*>(parentData->object)->partID;
		}

		selection.StateId = -1;

		// Update UI
		libmsstyle::StyleResourceType type;
		if (m_selectedProperty->GetTypeID() == libmsstyle::IDENTIFIER::FILENAME ||
			m_selectedProperty->GetTypeID() == libmsstyle::IDENTIFIER::FILENAME_LITE)
		{
			type = libmsstyle::StyleResourceType::IMAGE;
		}
		else if (m_selectedProperty->GetTypeID() == libmsstyle::IDENTIFIER::DISKSTREAM)
		{
			type = libmsstyle::StyleResourceType::ATLAS;
		}
		else
		{
			type = libmsstyle::StyleResourceType::NONE;
		}

		std::string file = m_currentStyle->GetQueuedResourceUpdate(m_selectedProperty->GetResourceID(), type);
		if (!file.empty())
		{
			ShowImageFromFile(A2W(file.c_str()));

			wsprintf(statusText, L"C: %d, P: %d, Img: %d*", selection.ClassId, selection.PartId, m_selectedProperty->GetResourceID());
			SetStatusText(statusText);
		}
		else
		{
			ShowImageFromResource(*m_selectedProperty);

			wsprintf(statusText, L"C: %d, P: %d, Img: %d", selection.ClassId, selection.PartId, m_selectedProperty->GetResourceID());
			SetStatusText(statusText);
		}
	}

	return 0;
}


LRESULT CMainFrame::OnPropGridItemChanging(int idCtrl, LPNMHDR pnmh, BOOL& bHandled)
{
	LPNMPROPERTYITEM evt = (LPNMPROPERTYITEM)pnmh;
	
	IProperty* gridProp = evt->prop;
	libmsstyle::StyleProperty* styleProp = (libmsstyle::StyleProperty*)gridProp->GetItemData();

	if (!styleProp)
		return CHANGE_VETO;

	CComVariant propValue;;
	gridProp->GetValue(&propValue);

	// COLORIZATIONCOLOR is internally an integer...
	if (styleProp->GetNameID() == libmsstyle::IDENTIFIER::COLORIZATIONCOLOR)
	{
		int colorARGB = 
			GetRValue(propValue.uintVal) << 16 |
			GetGValue(propValue.uintVal) << 8 |
			GetBValue(propValue.uintVal) << 0;
		styleProp->UpdateIntegerUnchecked(colorARGB);
		return CHANGE_OK;
	}

	switch (styleProp->header.typeID)
	{
	case libmsstyle::IDENTIFIER::FILENAME:
		styleProp->UpdateImageLink(propValue.uintVal); break;
	case libmsstyle::IDENTIFIER::INT:
		styleProp->UpdateInteger(propValue.intVal); break;
	case libmsstyle::IDENTIFIER::SIZE:
		styleProp->UpdateSize(propValue.intVal); break;
	case libmsstyle::IDENTIFIER::HIGHCONTRASTCOLORTYPE:
	case libmsstyle::IDENTIFIER::ENUM:
		styleProp->UpdateEnum(propValue.uintVal); break;
	case libmsstyle::IDENTIFIER::BOOLTYPE:
		styleProp->UpdateBoolean((propValue.uintVal > 0)); break;
	case libmsstyle::IDENTIFIER::COLOR:
	{
		styleProp->UpdateColor(GetRValue(propValue.uintVal), GetGValue(propValue.uintVal), GetBValue(propValue.uintVal));
	} break;
	case libmsstyle::IDENTIFIER::RECTTYPE:
	case libmsstyle::IDENTIFIER::MARGINS:
	{
		int l, t, r, b;
		if (swscanf(propValue.bstrVal, L"%d, %d, %d, %d", &l, &t, &r, &b) != 4)
		{
			MessageBoxW(L"Invalid format! expected: a, b, c, d", L"format error", MB_OK | MB_ICONERROR);
			return CHANGE_VETO;
		}
		else
		{
			if (styleProp->header.typeID == libmsstyle::IDENTIFIER::RECTTYPE)
				styleProp->UpdateRectangle(l, t, r, b);
			if (styleProp->header.typeID == libmsstyle::IDENTIFIER::MARGINS)
				styleProp->UpdateMargin(l, t, r, b);
		}
	} break;
	case libmsstyle::IDENTIFIER::POSITION:
	{
		int x, y;
		if (swscanf(propValue.bstrVal, L"%d, %d", &x, &y) != 2)
		{
			MessageBoxW(L"Invalid format! expected: a, b", L"format error", MB_OK | MB_ICONERROR);
			return CHANGE_VETO;
		}
		else
		{
			styleProp->UpdatePosition(x, y);
		}
	} break;
	case libmsstyle::IDENTIFIER::FONT:
		styleProp->UpdateFont(propValue.uintVal); break;
	default:
	{
		WCHAR msg[100];
		wsprintf(msg, L"Changing properties of type '%s' is not supported yet!", libmsstyle::lookup::FindTypeName(styleProp->GetTypeID()));
		MessageBoxW(msg, L"Unsupported", MB_OK | MB_ICONINFORMATION);
		
		return CHANGE_VETO;
	} break;
	}

	return CHANGE_OK;
}

LPARAM CMainFrame::RegUserData(void* data, int type)
{
	TreeItemData* userData = new TreeItemData(data, type);
	m_treeItemData.push_back(userData);

	return reinterpret_cast<LPARAM>(userData);
}

void CMainFrame::FillTreeView()
{
	m_treeView.SetRedraw(0);

	ClearTreeView();

	HTREEITEM rootNode = TVI_ROOT;
	

	// Add classes
	for (auto& cls : *m_currentStyle)
	{
		std::wstring tmp = StdUTF8ToWide(cls.second.className);
		CTreeItem classNode = m_treeView.InsertItem(TVIF_TEXT | TVIF_PARAM, tmp.c_str(), 0, 0, 0, 0, RegUserData(&cls.second, ITEM_CLASS), rootNode, TVI_LAST);

		// Add parts
		for (auto& part : cls.second)
		{
			tmp = StdUTF8ToWide(part.second.partName);
			CTreeItem partNode = m_treeView.InsertItem(TVIF_TEXT | TVIF_PARAM, tmp.c_str(), 0, 0, 0, 0, RegUserData(&part.second, ITEM_PART), classNode, TVI_LAST);

			// Add images
			for (auto& state : part.second)
			{
				// Add properties
				for (auto& prop : state.second)
				{
					// Add images
					if (prop->header.typeID == libmsstyle::IDENTIFIER::FILENAME ||
						prop->header.typeID == libmsstyle::IDENTIFIER::FILENAME_LITE ||
						prop->header.typeID == libmsstyle::IDENTIFIER::DISKSTREAM)
					{
						tmp = StdUTF8ToWide(prop->LookupName()); // propnames have to be looked up, but thats fast
						m_treeView.InsertItem(TVIF_TEXT | TVIF_PARAM, tmp.c_str(), 0, 0, 0, 0, RegUserData(prop, ITEM_PROPERTY), partNode, TVI_LAST);
					}
				}
			}
		}

	}

	m_treeView.SortChildren(TVI_ROOT, true);
	m_treeView.SetRedraw(1);
}

void CMainFrame::ClearTreeView()
{
	m_treeView.DeleteAllItems();

	auto it = m_treeItemData.begin();
	for (; it != m_treeItemData.end(); ++it)
	{
		delete *it;
	}
}

void CMainFrame::FillPropView(libmsstyle::StylePart& part)
{
	for (auto& state : part)
	{
		std::wstring tmp = StdUTF8ToWide(state.second.stateName);
		CCategoryProperty* category = new CCategoryProperty(tmp.c_str(), (LPARAM)&state);
		m_propList.AddItem(category);
		for (auto& prop : state.second)
		{
			HPROPERTY p = GetPropertyItemFromStyleProperty(*prop);
			if (p != nullptr)
			{
				m_propList.AddItem(p);
			}
			else
			{
				throw new std::runtime_error("unknown prop");
			}
		}
	}
}

void CMainFrame::ClearPropView()
{
	// use baseclass since the subclass doesn't
	// allow calling DeleteString() for some reason
	int max = m_propListBase.GetCount();
	for (int i = max-1; i >= 0; --i)
	{
		m_propListBase.DeleteString(i);
	}
}

void CMainFrame::ShowImageFromFile(LPCWSTR path)
{
	Gdiplus::Image* img = Gdiplus::Image::FromFile(path);
	m_imageView.SetBitmap(img);
}

void CMainFrame::ShowImageFromResource(libmsstyle::StyleProperty& prop)
{
	libmsstyle::StyleResource res = m_currentStyle->GetResourceFromProperty(prop);
	if (res.GetData() != nullptr && res.GetSize() != 0)
	{
		m_selectedImage = res;
		
		IStream* stream = SHCreateMemStream(reinterpret_cast<const BYTE*>(res.GetData()), res.GetSize());
		Gdiplus::Image* img = new Gdiplus::Image(stream);
		m_imageView.SetBitmap(img);
		stream->Release();
	}
}

void CMainFrame::ClearImageView()
{
	m_imageView.SetBitmap(NULL);
}

void CMainFrame::SetStatusText(LPCWSTR text)
{
	CStatusBarCtrl bar(m_hWndStatusBar);
	bar.SetText(0, text);
}

void CMainFrame::SetThemeTestMenuItemText(LPWSTR text, bool checked)
{
	WCHAR menuItemText[48];

	MENUITEMINFO themeTestItem = { 0 };
	themeTestItem.cbSize = sizeof(MENUITEMINFO);
	themeTestItem.dwTypeData = menuItemText;
	themeTestItem.cch = 48;
	themeTestItem.fMask = MIIM_TYPE | MIIM_DATA | MIIM_STATE;

	HMENU mainMenu = m_commandBar.GetMenu();
	HMENU themeMenu = GetSubMenu(mainMenu, 1);
	GetMenuItemInfo(themeMenu, ID_THEME_TEST, FALSE, &themeTestItem);

	themeTestItem.dwTypeData = text;
	themeTestItem.fState = checked ? MFS_CHECKED : MFS_UNCHECKED;
	SetMenuItemInfo(themeMenu, ID_THEME_TEST, FALSE, &themeTestItem);
}

#pragma endregion

#pragma region Menu Handling

LRESULT CMainFrame::OnFileNew(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	return 0;
}

LRESULT CMainFrame::OnFileOpen(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	COMDLG_FILTERSPEC filter[] =
	{
		{ _T("Visual Style(*.msstyles)"), _T("*.msstyles") },
		{ _T("All Files(*.*)"), _T("*.*") }
	};

	CShellFileOpenDialog openDialog(NULL
		, FOS_FORCEFILESYSTEM | FOS_FILEMUSTEXIST | FOS_PATHMUSTEXIST
		, _T("txt")
		, filter
		, _countof(filter));

	if (openDialog.DoModal() == IDOK)
	{
		CloseStyle();
		
		CString filePath;
		openDialog.GetFilePath(filePath);

		OpenStyle(filePath);
	}

	return 0;
}

LRESULT CMainFrame::OnFileSave(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	COMDLG_FILTERSPEC filter[] =
	{
		{ _T("Visual Style(*.msstyles)"), _T("*.msstyles") }
	};

	CShellFileSaveDialog saveDialog(NULL
		, FOS_FORCEFILESYSTEM | FOS_FILEMUSTEXIST | FOS_PATHMUSTEXIST
		, _T("txt")
		, filter
		, _countof(filter));

	if (saveDialog.DoModal() == IDOK)
	{
		CString filePath;
		saveDialog.GetFilePath(filePath);

		try
		{
			std::wstring tmp(filePath.GetString());
			m_currentStyle->Save(StdWideToUTF8(tmp));

			SetStatusText(L"Style saved successfully!");
		}
		catch (std::runtime_error err)
		{
			MessageBoxA(m_hWnd, err.what(), "Error saving file!", MB_OK | MB_ICONERROR);
		}
	}

	return 0;
}

LRESULT CMainFrame::OnFileExportStyleInfo(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	return 0;
}

LRESULT CMainFrame::OnFileExit(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	PostMessage(WM_CLOSE);
	return 0;
}


LRESULT CMainFrame::OnThemeTest(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	if (m_currentStyle == nullptr)
		return 0;


	if (m_themeManager.IsThemeInUse())
	{
		try
		{
			m_themeManager.Rollback();
			SetThemeTestMenuItemText(TEXT_PLAY, false);
			Sleep(300); // prevent doubleclicks
			return 0;
		}
		catch (...)
		{
		}
	}

	bool needConfirmation = false;
	OSVERSIONINFO version;
	ZeroMemory(&version, sizeof(OSVERSIONINFO));
	version.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);

#pragma warning( disable : 4996 )
	GetVersionEx(&version);

	libmsstyle::Platform styleplatform = m_currentStyle->GetCompatiblePlatform();

	if (version.dwMajorVersion == 6 &&
		version.dwMinorVersion == 1 &&
		styleplatform != libmsstyle::Platform::WIN7)
	{
		needConfirmation = true;
	}

	if (version.dwMajorVersion == 6 &&
		version.dwMinorVersion >= 2 &&
		styleplatform != libmsstyle::Platform::WIN8 &&
		styleplatform != libmsstyle::Platform::WIN81)
	{
		needConfirmation = true;
	}

	if (version.dwMajorVersion == 10 &&
		version.dwMinorVersion >= 0 &&
		styleplatform != libmsstyle::Platform::WIN10)
	{
		needConfirmation = true;
	}

	if (needConfirmation)
	{
		if (MessageBoxW(L"It looks like the style was not made for this windows version. Try to apply it anyways?", L"msstyleEditor", MB_YESNO | MB_ICONQUESTION) == IDNO)
		{
			return 0;
		}
	}

	try
	{
		m_themeManager.ApplyTheme(*m_currentStyle);
		SetThemeTestMenuItemText(TEXT_STOP, true);
		Sleep(300); // prevent doubleclicks
	}
	catch (std::runtime_error& err)
	{
		MessageBoxA(m_hWnd, err.what(), "Error applying style!", MB_OK | MB_ICONERROR);
	}
	catch (std::exception& ex)
	{
		MessageBoxA(m_hWnd, ex.what(), "Error applying style!", MB_OK | MB_ICONERROR);
	}
	catch (...)
	{
		MessageBoxA(m_hWnd, "Unknown exception!", "Error applying style!", MB_OK | MB_ICONERROR);
	}
	return 0;
}


LRESULT CMainFrame::OnImageExport(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	if (m_selectedImage.GetSize() == 0 || m_selectedImage.GetData() == 0 || m_selectedProperty == nullptr)
	{
		MessageBoxW(L"Select an image first!", L"Export Image", MB_OK | MB_ICONERROR);
		return 0;
	}

	COMDLG_FILTERSPEC filter[] =
	{
		{ _T("PNG Image(*.png)"), _T("*.png") }
	};

	CShellFileSaveDialog saveDialog(NULL
		, FOS_FORCEFILESYSTEM | FOS_FILEMUSTEXIST | FOS_PATHMUSTEXIST
		, _T("txt")
		, filter
		, _countof(filter));

	if (saveDialog.DoModal() == IDOK)
	{
		CString filePath;
		saveDialog.GetFilePath(filePath);

		HANDLE hFile = CreateFileW(filePath.GetString()
			, GENERIC_READ | GENERIC_WRITE
			, FILE_SHARE_READ
			, NULL
			, CREATE_ALWAYS
			, FILE_ATTRIBUTE_NORMAL
			, NULL);

		if (hFile == INVALID_HANDLE_VALUE)
		{
			MessageBoxW(L"Creating imagefile at target location failed!", L"Export Image", MB_OK | MB_ICONERROR);
			return 0;
		}

		DWORD bytesWritten = 0;
		BOOL success = WriteFile(hFile
			, m_selectedImage.GetData()
			, m_selectedImage.GetSize()
			, &bytesWritten
			, NULL);

		if (!success)
		{
			MessageBoxW(L"Error while writing to the file!", L"Export Image", MB_OK | MB_ICONERROR);
		}
		else
		{
			SetStatusText(L"Image exported successfully!");
		}

		CloseHandle(hFile);
	}

	return 0;
}

LRESULT CMainFrame::OnImageReplace(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	if (m_selectedProperty == nullptr)
	{
		MessageBoxW(L"Select an image resource first!", L"Replace Image", MB_OK | MB_ICONERROR);
		return 1;
	}


	COMDLG_FILTERSPEC filter[] =
	{
		{ _T("PNG Image(*.png)"), _T("*.png") },
		{ _T("All Files(*.*)"), _T("*.*") }
	};

	CShellFileOpenDialog openDialog(NULL
		, FOS_FORCEFILESYSTEM | FOS_FILEMUSTEXIST | FOS_PATHMUSTEXIST
		, _T("txt")
		, filter
		, _countof(filter));

	if (openDialog.DoModal() == IDOK)
	{
		libmsstyle::StyleResourceType tp;
		switch (m_selectedProperty->GetTypeID())
		{
		case libmsstyle::IDENTIFIER::FILENAME:
		case libmsstyle::IDENTIFIER::FILENAME_LITE:
			tp = libmsstyle::StyleResourceType::IMAGE; break;
		case libmsstyle::IDENTIFIER::DISKSTREAM:
			tp = libmsstyle::StyleResourceType::ATLAS; break;
		default:
			tp = libmsstyle::StyleResourceType::NONE; break;
		}

		CString filePath;
		openDialog.GetFilePath(filePath);

		std::wstring tmp(filePath.GetString());
		m_currentStyle->QueueResourceUpdate(m_selectedProperty->GetResourceID(), tp, StdWideToUTF8(tmp));
	}

	return 0;
}


LRESULT CMainFrame::OnViewExpand(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	m_treeView.SetRedraw(0);
	HTREEITEM item = m_treeView.GetRootItem();
	while (item != NULL)
	{
		m_treeView.Expand(item, TVE_EXPAND);
		item = m_treeView.GetNextSiblingItem(item);
	}
	m_treeView.SetRedraw(1);
	return 0;
}

LRESULT CMainFrame::OnViewCollapse(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	m_treeView.SetRedraw(0);
	HTREEITEM item = m_treeView.GetRootItem();
	while (item != NULL)
	{
		m_treeView.Expand(item, TVE_COLLAPSE);
		item = m_treeView.GetNextSiblingItem(item);
	}
	m_treeView.SetRedraw(1);
	return 0;
}

LRESULT CMainFrame::OnViewThemeFolder(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	WCHAR* windowsFolder = nullptr;
	WCHAR path[255];

	if (SHGetKnownFolderPath(FOLDERID_Windows, KF_FLAG_DEFAULT, NULL, &windowsFolder) == S_OK)
	{
		wsprintf(path, L"%s\\Resources\\Themes\\", windowsFolder);
		CoTaskMemFree(windowsFolder);
		ShellExecute(NULL, L"explore", path, NULL, NULL, SW_SHOWDEFAULT);
	}
	else
	{
		ShellExecute(NULL, L"explore", L"C:\\Windows\\Resources\\Themes\\", NULL, NULL, SW_SHOWDEFAULT);
	}
	return 0;
}

LRESULT CMainFrame::OnViewStatusBar(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	BOOL bVisible = !::IsWindowVisible(m_hWndStatusBar);
	::ShowWindow(m_hWndStatusBar, bVisible ? SW_SHOWNOACTIVATE : SW_HIDE);
	UISetCheck(ID_VIEW_STATUS_BAR, bVisible);
	UpdateLayout();
	return 0;
}


LRESULT CMainFrame::OnAppLicense(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	return 0;
}

LRESULT CMainFrame::OnAppAbout(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	CAboutDlg dlg;
	dlg.DoModal();
	return 0;
}

#pragma endregion