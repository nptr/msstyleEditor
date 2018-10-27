#pragma once

#include "..\resource.h"

#include <atlctrls.h>
#include <atlctrlx.h>

struct SearchProperties
{
	static const int MODE_NAME = 0;
	static const int MODE_PROPERTY = 1;

	int mode;
	int type;
	WCHAR text[32];
};

class CSearchDlg : public CDialogImpl<CSearchDlg>
				 , public CMessageFilter
{
private:
	SearchProperties m_searchProperties;
public:
	enum { IDD = IDD_SEARCHDIALOG };

	virtual BOOL PreTranslateMessage(MSG* pMsg)
	{
		// Using TranslateAccelerator() would call the window procedure and
		// not post the translated messages to the queue as we want it to do.
		if (pMsg->message == WM_KEYDOWN && pMsg->wParam == VK_RETURN)
		{
			SendMessage(WM_COMMAND, (WPARAM)IDC_SEARCH_GO, LPARAM(0));
		}

		return CWindow::IsDialogMessage(pMsg);
	}

	BEGIN_MSG_MAP(CSearchDlg)
		MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
		COMMAND_ID_HANDLER(IDOK, OnCloseCmd)
		COMMAND_ID_HANDLER(IDCANCEL, OnCloseCmd)

		COMMAND_HANDLER(IDC_SEARCH_GO, BN_CLICKED, OnSearchInitiate)
		COMMAND_HANDLER(IDC_SEARCH_LOCATION, CBN_SELCHANGE, OnSearchLocationChanged)
		COMMAND_HANDLER(IDC_SEARCH_TYPE, CBN_SELCHANGE, OnSearchTypeChanged)
	END_MSG_MAP()

	LRESULT OnInitDialog(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
	{
		CenterWindow(GetParent());

		CMessageLoop* msgLoop = _Module.GetMessageLoop();
		msgLoop->AddMessageFilter(this);

		CComboBox locationBox(GetDlgItem(IDC_SEARCH_LOCATION));
		locationBox.InsertString(0, L"Class or Part");
		locationBox.InsertString(1, L"Property");
		locationBox.SetCurSel(0);

		CComboBox typeBox(GetDlgItem(IDC_SEARCH_TYPE));
		typeBox.InsertString(0, L"COLOR");
		typeBox.InsertString(1, L"MARGINS");
		typeBox.InsertString(2, L"SIZE");
		typeBox.InsertString(3, L"POSITION");
		typeBox.InsertString(4, L"RECT");
		typeBox.SetCurSel(0);
		typeBox.EnableWindow(FALSE);

		CButton searchButton(GetDlgItem(IDC_SEARCH_GO));
		searchButton.SetIcon((HICON)LoadImage(GetModuleHandle(NULL)
			, MAKEINTRESOURCE(IDI_FIND)
			, IMAGE_ICON
			, 16, 16
			, LR_SHARED));

		CEdit searchBox(GetDlgItem(IDC_SEARCH_TEXT));
		searchBox.SetCueBannerText(L"Search", TRUE);

		m_searchProperties.mode = SearchProperties::MODE_NAME;
		m_searchProperties.type = libmsstyle::IDENTIFIER::COLOR;

		return TRUE;
	}

	LRESULT OnCloseCmd(WORD /*wNotifyCode*/, WORD wID, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
	{
		ShowWindow(SW_HIDE);
		return 0;
	}

	LRESULT OnSearchInitiate(WORD /*wNotifyCode*/, WORD wID, HWND hWndCtl, BOOL& /*bHandled*/)
	{
		CEdit searchBox(GetDlgItem(IDC_SEARCH_TEXT));
		searchBox.GetWindowText(m_searchProperties.text, 31);

		SendMessage(GetParent(), WM_USER + 44, (WPARAM)&m_searchProperties, (LPARAM)hWndCtl);
		return 0;
	}

	LRESULT OnSearchLocationChanged(WORD /*wNotifyCode*/, WORD wID, HWND hWndCtl, BOOL& /*bHandled*/)
	{
		CEdit searchBox(GetDlgItem(IDC_SEARCH_TEXT));
		CComboBox typeBox(GetDlgItem(IDC_SEARCH_TYPE));
		CComboBox locationBox(hWndCtl);

		switch (locationBox.GetCurSel())
		{
		case 0:
		{
			m_searchProperties.mode = SearchProperties::MODE_NAME;
			searchBox.SetCueBannerText(L"Search", TRUE);
			typeBox.EnableWindow(FALSE);
		} break;
		case 1:
		{
			m_searchProperties.mode = SearchProperties::MODE_PROPERTY;
			BOOL dummy = 0;
			OnSearchTypeChanged(CBN_SELCHANGE, NULL, GetDlgItem(IDC_SEARCH_TYPE), dummy);
			typeBox.EnableWindow(TRUE);
		} break;
		default: break;
		}

		return 0;
	}

	LRESULT OnSearchTypeChanged(WORD /*wNotifyCode*/, WORD wID, HWND hWndCtl, BOOL& /*bHandled*/)
	{
		CEdit searchBox(GetDlgItem(IDC_SEARCH_TEXT));
		CComboBox typeBox(hWndCtl);

		switch (typeBox.GetCurSel())
		{
		case 0: searchBox.SetCueBannerText(L"r, g, b", TRUE);		m_searchProperties.type = libmsstyle::IDENTIFIER::COLOR; break;
		case 1: searchBox.SetCueBannerText(L"l, t, r, b", TRUE);	m_searchProperties.type = libmsstyle::IDENTIFIER::MARGINS; break;
		case 2: searchBox.SetCueBannerText(L"size", TRUE);			m_searchProperties.type = libmsstyle::IDENTIFIER::SIZE; break;
		case 3: searchBox.SetCueBannerText(L"x, y", TRUE);			m_searchProperties.type = libmsstyle::IDENTIFIER::POSITION; break;
		case 4: searchBox.SetCueBannerText(L"l, t, r, b", TRUE);	m_searchProperties.type = libmsstyle::IDENTIFIER::RECTTYPE; break;
		default: searchBox.SetCueBannerText(L"", TRUE); break;
		}

		return 0;
	}
};