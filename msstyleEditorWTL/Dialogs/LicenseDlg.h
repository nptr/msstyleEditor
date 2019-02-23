#pragma once

class CLicenseDlg : public CDialogImpl<CLicenseDlg>
{
public:
	enum { IDD = IDD_LICENSEDIALOG };

	BEGIN_MSG_MAP(CLicenseDlg)
		MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
		COMMAND_ID_HANDLER(IDCANCEL, OnCloseCmd)
	END_MSG_MAP()

	LRESULT OnInitDialog(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
	{
		CenterWindow(GetParent());

		HRSRC hResource = FindResource(NULL, MAKEINTRESOURCE(IDR_LICENSE), RT_HTML);
		HGLOBAL hData = LoadResource(NULL, hResource);
		char* license = (char*)LockResource(hData);

		USES_CONVERSION;

		CEdit edit(GetDlgItem(IDC_EDIT_LICENSE));
		edit.SetWindowTextW(A2W(license));
		edit.SetFocus();

		return FALSE;
	}

	LRESULT OnCloseCmd(WORD /*wNotifyCode*/, WORD wID, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
	{
		EndDialog(wID);
		return 0;
	}
};