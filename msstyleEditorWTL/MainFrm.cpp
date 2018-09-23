#include "stdafx.h"
#include "MainFrm.h"
#include "AboutDlg.h"

#include "TreeItemData.h"

#include "Helper.h"

#include <gdiplus.h>
#include <gdiplusgraphics.h>

#define COMMON_WND_STYLE WS_CHILD | WS_VISIBLE | WS_CLIPSIBLINGS | WS_CLIPCHILDREN
#define NOT_A_MENU 1

#define ITEM_CLASS 1
#define ITEM_PART 2
#define ITEM_STATE 3
#define ITEM_PROPERTY 4

#define CHANGE_OK 0
#define CHANGE_VETO 1

CMainFrame::CMainFrame()
	: m_currentStyle(nullptr)
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
			//themeManager.Rollback();
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

LRESULT CMainFrame::OnCreate(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
{
	Gdiplus::GdiplusStartupInput gdiplusStartupInput;
	ULONG_PTR gdiplusToken;
	GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, NULL);

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
	m_propList.SetExtendedListStyle(PLS_EX_CATEGORIZED);

	m_splitLeft.SetSplitterPanes(m_treeView, m_splitRight);
	m_splitRight.SetSplitterPanes(m_imageView, m_propList);
	m_splitLeft.SetSplitterPosPct(33);
	m_splitRight.SetSplitterPosPct(50);

	UISetCheck(ID_VIEW_TOOLBAR, 1);
	UISetCheck(ID_VIEW_STATUS_BAR, 1);

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

	RECT rectImgView;
	m_imageView.GetWindowRect(&rectImgView);

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

	return TRUE;
}

LRESULT CMainFrame::OnTreeViewSelectionChanged(int idCtrl, LPNMHDR pnmh, BOOL& bHandled)
{
	USES_CONVERSION;

	LPNM_TREEVIEW evt = (LPNM_TREEVIEW)pnmh;

	WCHAR statusText[255];

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
		libmsstyle::StyleProperty* selectedImageProp = static_cast<libmsstyle::StyleProperty*>(data->object);

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
		if (selectedImageProp->GetTypeID() == libmsstyle::IDENTIFIER::FILENAME ||
			selectedImageProp->GetTypeID() == libmsstyle::IDENTIFIER::FILENAME_LITE)
			type = libmsstyle::StyleResourceType::IMAGE;
		else if (selectedImageProp->GetTypeID() == libmsstyle::IDENTIFIER::DISKSTREAM)
			type = libmsstyle::StyleResourceType::ATLAS;
		else type = libmsstyle::StyleResourceType::NONE;

		std::string file = m_currentStyle->GetQueuedResourceUpdate(selectedImageProp->GetResourceID(), type);
		if (!file.empty())
		{
			ShowImageFromFile(A2W(file.c_str()));

			wsprintf(statusText, L"C: %d, P: %d, Img: %d*", selection.ClassId, selection.PartId, selectedImageProp->GetResourceID());
			SetStatusText(statusText);
		}
		else
		{
			ShowImageFromResource(*selectedImageProp);

			wsprintf(statusText, L"C: %d, P: %d, Img: %d", selection.ClassId, selection.PartId, selectedImageProp->GetResourceID());
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
	return 0;
}


LRESULT CMainFrame::OnImageExport(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
	return 0;
}

LRESULT CMainFrame::OnImageReplace(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
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