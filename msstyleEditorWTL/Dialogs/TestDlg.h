#pragma once

class CTestDlg : public CDialogImpl<CTestDlg>
{
public:
	enum { IDD = IDD_TESTDIALOG };

	BEGIN_MSG_MAP(CTestDlg)
		MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
		COMMAND_ID_HANDLER(IDCANCEL, OnCloseCmd)
	END_MSG_MAP()

	LRESULT OnInitDialog(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
	{
		CenterWindow(GetParent());

		CListBox lb(GetDlgItem(IDC_LISTBOX));
		lb.AddString(_T("Item 1"));
		lb.AddString(_T("Item 2"));
		lb.AddString(_T("Item 3"));

		CListViewCtrl lv(GetDlgItem(IDC_LISTVIEW));
		lv.AddColumn(_T("Col 1"), 0);
		lv.AddColumn(_T("Col 2"), 1);
		lv.AddItem(0, 0, _T("Item 1-1"));
		lv.AddItem(0, 1, _T("Item 1-2"));
		lv.AddItem(1, 0, _T("Item 2-1"));
		lv.AddItem(1, 1, _T("Item 2-2"));

		CComboBox cb1(GetDlgItem(IDC_COMBO1));
		cb1.AddString(_T("Item 1"));
		cb1.AddString(_T("Item 2"));
		cb1.AddString(_T("Item 3"));
		cb1.SetCurSel(0);

		CComboBox cb2(GetDlgItem(IDC_COMBO2));
		cb2.AddString(_T("Item 1"));
		cb2.AddString(_T("Item 2"));
		cb2.AddString(_T("Item 3"));
		cb2.SetCurSel(0);

		CTreeViewCtrlEx tv(GetDlgItem(IDC_TREE1));
		HTREEITEM ch1 = tv.InsertItem(TVIF_TEXT, _T("Item 1"), 0, 0, 0, 0, NULL, TVI_ROOT, TVI_LAST);
		HTREEITEM ch2 = tv.InsertItem(TVIF_TEXT, _T("Item 2"), 0, 0, 0, 0, NULL, TVI_ROOT, TVI_LAST);
		HTREEITEM ch3 = tv.InsertItem(TVIF_TEXT, _T("Item 3"), 0, 0, 0, 0, NULL, TVI_ROOT, TVI_LAST);
		HTREEITEM ch1_1 = tv.InsertItem(TVIF_TEXT, _T("SubItem 1"), 0, 0, 0, 0, NULL, ch1, TVI_LAST);
		HTREEITEM ch2_1 = tv.InsertItem(TVIF_TEXT, _T("SubItem 1"), 0, 0, 0, 0, NULL, ch2, TVI_LAST);
		HTREEITEM ch2_2 = tv.InsertItem(TVIF_TEXT, _T("SubItem 2"), 0, 0, 0, 0, NULL, ch2, TVI_LAST);

		CProgressBarCtrl pb1(GetDlgItem(IDC_PROGRESS1));
		CProgressBarCtrl pb2(GetDlgItem(IDC_PROGRESS2));
		CProgressBarCtrl pb3(GetDlgItem(IDC_PROGRESS3));
		CProgressBarCtrl pb4(GetDlgItem(IDC_PROGRESS4));
		pb1.SetPos(66);
		pb1.SetState(PBST_NORMAL);
		pb2.SetPos(50);
		pb2.SetState(PBST_ERROR);
		pb3.SetPos(35);
		pb3.SetState(PBST_PAUSED);
		pb4.SetMarquee(TRUE, 50);

		CEdit ed1(GetDlgItem(IDC_EDIT1));
		ed1.SetWindowTextW(_T("normal"));
		CEdit ed2(GetDlgItem(IDC_EDIT2));
		ed2.SetWindowTextW(_T("password"));
		CEdit ed3(GetDlgItem(IDC_EDIT3));
		ed3.SetWindowTextW(_T("disabled"));

		CButton cb3(GetDlgItem(IDC_CHECK3));
		cb3.SetCheck(BST_CHECKED);
		CButton cb4(GetDlgItem(IDC_CHECK4));
		cb4.SetCheck(BST_CHECKED);

		CButton cb5(GetDlgItem(IDC_CHECK5));
		cb5.SetCheck(BST_INDETERMINATE);
		CButton cb6(GetDlgItem(IDC_CHECK6));
		cb6.SetCheck(BST_INDETERMINATE);

		CButton rb3(GetDlgItem(IDC_RADIO3));
		rb3.SetCheck(BST_CHECKED);

		CTabCtrl tab(GetDlgItem(IDC_TAB2));
		tab.AddItem(_T("Tab 1"));
		tab.AddItem(_T("Tab 2"));
		tab.AddItem(_T("Tab 3"));
		tab.AddItem(_T("Tab 4"));


		return FALSE;
	}

	LRESULT OnCloseCmd(WORD /*wNotifyCode*/, WORD wID, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
	{
		//EndDialog(wID);
		ShowWindow(SW_HIDE);
		return 0;
	}
};