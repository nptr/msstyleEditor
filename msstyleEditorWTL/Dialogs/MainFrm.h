#pragma once

#include <atlapp.h>
#include <atlframe.h>
#include <atlsplit.h>
#include <atlmisc.h>
#include <atlctrls.h>
#include <atlctrlw.h>
#include <atlctrlx.h>
#include <atldlgs.h>

#include "..\ThemeManager.h"
#include "..\Controls\PropertyList.h"
#include "..\Controls\ImageCtrl.h"
#include "..\Controls\DropTarget.h"
#include "..\resource.h"

#include "libmsstyle\VisualStyle.h"

#include <vector>

struct ItemData;
struct SearchProperties;

class CSearchDlg;

class CMainFrame :
	public CFrameWindowImpl<CMainFrame>,
	public CUpdateUI<CMainFrame>,
	public CMessageFilter,
	public CIdleHandler,
	public CDropTarget
{
private:
	CCommandBarCtrl		m_commandBar;
	CSplitterWindow		m_splitLeft;
	CSplitterWindow		m_splitRight;
	CTreeViewCtrlEx		m_treeView;
	CListBox			m_propListBase;
	CPropertyListCtrl	m_propList;
	CImageCtrl			m_imageView;

	CSearchDlg*			m_searchDialog;

	libmsstyle::VisualStyle*	m_currentStyle;
	libmsstyle::StyleProperty*	m_selectedProperty;
	libmsstyle::StyleResource	m_selectedImage;

	ThemeManager m_themeManager;

	HMENU m_imageViewMenu;
	HMENU m_propListMenu;

	HMENU m_fileSubMenu;
	HMENU m_themeSubMenu;
	HBITMAP m_bmpStart;
	HBITMAP m_bmpStop;

	struct SelectionModel
	{
		SelectionModel()
			: ClassId(-1)
			, PartId(-1)
			, StateId(-1)
		{}

		int ClassId;
		int PartId;
		int StateId;
	} selection;

public:
	DECLARE_FRAME_WND_CLASS(_T("mseWndClass"), IDR_MAINFRAME)

	CMainFrame();

	void OpenStyle(const CString& file);
	void CloseStyle();

	virtual BOOL PreTranslateMessage(MSG* pMsg)
	{
		return CFrameWindowImpl<CMainFrame>::PreTranslateMessage(pMsg);
	}

	virtual BOOL OnIdle()
	{
		UIUpdateToolBar();
		return FALSE;
	}

	BEGIN_UPDATE_UI_MAP(CMainFrame)
		UPDATE_ELEMENT(ID_VIEW_STATUS_BAR, UPDUI_MENUPOPUP)
	END_UPDATE_UI_MAP()

	BEGIN_MSG_MAP(CMainFrame)
		MESSAGE_HANDLER(WM_CREATE, OnCreate)
		MESSAGE_HANDLER(WM_DESTROY, OnDestroy)
		MESSAGE_HANDLER(WM_CONTEXTMENU, OnContextMenu)
		MESSAGE_HANDLER(WM_USER + 44, OnFindNext)

		NOTIFY_CODE_HANDLER(TVN_SELCHANGED, OnTreeViewSelectionChanged)
		NOTIFY_CODE_HANDLER(PIN_ITEMCHANGING, OnPropGridItemChanging)

		COMMAND_ID_HANDLER(ID_APP_EXIT, OnFileExit)
		COMMAND_ID_HANDLER(ID_FILE_NEW, OnFileNew)
		COMMAND_ID_HANDLER(ID_FILE_OPEN, OnFileOpen)
		COMMAND_ID_HANDLER(ID_FILE_SAVE_AS, OnFileSave)
		COMMAND_ID_HANDLER(ID_EXPORT_STYLEINFO, OnFileExportStyleInfo)
		COMMAND_ID_HANDLER(ID_THEME_TEST, OnThemeTest)
		COMMAND_ID_HANDLER(ID_IMAGE_EXPORT, OnImageExport)
		COMMAND_ID_HANDLER(ID_IMAGE_REPLACE, OnImageReplace)
		COMMAND_ID_HANDLER(ID_VIEW_EXPANDALL, OnViewExpand)
		COMMAND_ID_HANDLER(ID_VIEW_COLLAPSEALL, OnViewCollapse)
		COMMAND_ID_HANDLER(ID_VIEW_THEMEFOLDER, OnViewThemeFolder)
		COMMAND_ID_HANDLER(ID_VIEW_STATUS_BAR, OnViewStatusBar)
		COMMAND_ID_HANDLER(ID_APP_ABOUT, OnAppAbout)
		COMMAND_ID_HANDLER(ID_FIND, OnFindOpen)

		CHAIN_MSG_MAP(CUpdateUI<CMainFrame>)
		CHAIN_MSG_MAP(CFrameWindowImpl<CMainFrame>)
		REFLECT_NOTIFICATIONS()
	END_MSG_MAP()

	// Handler prototypes (uncomment arguments if needed):
	//	LRESULT MessageHandler(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
	//	LRESULT CommandHandler(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
	//	LRESULT NotifyHandler(int /*idCtrl*/, LPNMHDR /*pnmh*/, BOOL& /*bHandled*/)

	HRESULT STDMETHODCALLTYPE QueryInterface(REFIID refiid, void FAR* FAR* ppvObject);
	ULONG STDMETHODCALLTYPE AddRef(void);
	ULONG STDMETHODCALLTYPE Release(void);

	HRESULT STDMETHODCALLTYPE DragEnter(IDataObject *pDataObj, DWORD grfKeyState, POINTL pt, DWORD *pdwEffect);
	HRESULT STDMETHODCALLTYPE DragOver(DWORD grfKeyState, POINTL pt, DWORD *pdwEffect);
	HRESULT STDMETHODCALLTYPE DragLeave(void);
	HRESULT STDMETHODCALLTYPE Drop(IDataObject *pDataObj, DWORD grfKeyState, POINTL pt, DWORD *pdwEffect);

	//
	// APP
	//
	LRESULT OnCreate(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/);
	LRESULT OnDestroy(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& bHandled);

	LRESULT OnTreeViewSelectionChanged(int /*idCtrl*/, LPNMHDR /*pnmh*/, BOOL& /*bHandled*/);
	LRESULT OnPropGridItemChanging(int /*idCtrl*/, LPNMHDR /*pnmh*/, BOOL& /*bHandled*/);

	LRESULT OnContextMenu(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/);
	LRESULT OnFindOpen(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnFindNext(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/);

	LRESULT OnAddProperty(int32_t stateId);
	LRESULT OnRemoveProperty(libmsstyle::StyleProperty* selectedProp);

	//
	// MENU ITEMS
	//
	LRESULT OnFileNew(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnFileOpen(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnFileSave(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnFileExportStyleInfo(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnFileExit(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

	LRESULT OnThemeTest(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

	LRESULT OnImageExport(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnImageReplace(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

	LRESULT OnViewExpand(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnViewCollapse(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnViewThemeFolder(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
	LRESULT OnViewStatusBar(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

	LRESULT OnAppAbout(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

private:
	void FillTreeView();
	void ClearTreeView();

	void FillPropView(libmsstyle::StylePart& part);
	void ClearPropView();

	void ShowImageFromFile(LPCWSTR path);
	void ShowImageFromResource(libmsstyle::StyleProperty& prop);
	void ClearImageView();

	void SetStatusText(LPCWSTR text);
	void SetThemeTestMenuItemText(LPWSTR text, bool checked);

	HTREEITEM DoFindNext(const SearchProperties* p, HTREEITEM node);
	LPARAM RegUserData(void* data, int type);

	std::vector<ItemData*> m_treeItemData;
};
