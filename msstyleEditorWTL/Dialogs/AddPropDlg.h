#pragma once

#include <libmsstyle\VisualStyleDefinitions.h>

class CAddPropDlg : public CDialogImpl<CAddPropDlg>
{
public:
	enum { IDD = IDD_ADDPROPDIALOG };

	BEGIN_MSG_MAP(CAddPropDlg)
		MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
		COMMAND_ID_HANDLER(IDOK, OnCloseCmd)
		COMMAND_ID_HANDLER(IDCANCEL, OnCloseCmd)

		COMMAND_HANDLER(IDC_TYPE, CBN_SELCHANGE, OnSelectedTypeChanged)
		COMMAND_HANDLER(IDC_PROP, LBN_SELCHANGE, OnSelectedPropChanged)
	END_MSG_MAP()

	libmsstyle::IDENTIFIER m_selectedType;
	libmsstyle::IDENTIFIER m_selectedProp;

	LRESULT OnInitDialog(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
	{
		CComboBox typeBox(GetDlgItem(IDC_TYPE));
		typeBox.InsertString(0, L"ENUM");
		typeBox.InsertString(1, L"INT");
		typeBox.InsertString(2, L"BOOL");
		typeBox.InsertString(3, L"COLOR");
		typeBox.InsertString(4, L"MARGIN");
		typeBox.InsertString(5, L"POSITION");
		typeBox.SetCurSel(0);

		BOOL dummy;
		OnSelectedTypeChanged(NULL, NULL, NULL, dummy);

		CenterWindow(GetParent());
		return TRUE;
	}

	LRESULT OnCloseCmd(WORD /*wNotifyCode*/, WORD wID, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
	{
		EndDialog(wID);
		return 0;
	}

	LRESULT OnSelectedTypeChanged(WORD /*wNotifyCode*/, WORD wID, HWND hWndCtl, BOOL& /*bHandled*/)
	{
		USES_CONVERSION;

		CComboBox typeBox(GetDlgItem(IDC_TYPE));
		CListBox propBox(GetDlgItem(IDC_PROP));

		const int typeIdArray[] =
		{
			200,
			202,
			203,
			204,
			205,
			208
		};

		int typeId = 0;
		switch (typeBox.GetCurSel())
		{
			case 0: typeId = 200; break;
			case 1: typeId = 202; break;
			case 2: typeId = 203; break;
			case 3: typeId = 204; break;
			case 4: typeId = 205; break;
			case 5: typeId = 208; break;
		}

		m_selectedType = static_cast<libmsstyle::IDENTIFIER>(typeId);

		// clear
		int max = propBox.GetCount();
		for (int i = max - 1; i >= 0; --i)
		{
			propBox.DeleteString(i);
		}

		// add
		for (auto& it = libmsstyle::PROPERTY_INFO_MAP.begin(); it != libmsstyle::PROPERTY_INFO_MAP.end(); ++it)
		{
			// select all properties matching our type, but not the entry of the type itself
			if (typeId == it->second.type &&
				typeId != it->first)
			{
				int index = propBox.AddString(A2W(it->second.name));
				propBox.SetItemData(index, (DWORD_PTR)it->first);
			}
		}

		// update prop selection & description
		propBox.SetCurSel(0);

		BOOL dummy;
		OnSelectedPropChanged(NULL, NULL, NULL, dummy);

		return 0;
	}

	LRESULT OnSelectedPropChanged(WORD /*wNotifyCode*/, WORD wID, HWND hWndCtl, BOOL& /*bHandled*/)
	{
		USES_CONVERSION;

		CListBox propBox(GetDlgItem(IDC_PROP));
		CStatic propDesc(GetDlgItem(IDC_DESCRIPTION));

		int index = propBox.GetCurSel();
		if (index >= 0)
		{
			int nameId = (int)propBox.GetItemData(index);
			m_selectedProp = static_cast<libmsstyle::IDENTIFIER>(nameId);

			auto it = libmsstyle::PROPERTY_INFO_MAP.find(nameId);
			propDesc.SetWindowTextW(A2W(it->second.description));
		}

		return 0;
	}

	INT_PTR DoModal(libmsstyle::StyleProperty* prop)
	{
		INT_PTR result = CDialogImpl<CAddPropDlg>::DoModal();
		if (result == IDOK)
		{
			prop->Initialize(m_selectedType, m_selectedProp);
		}

		return result;
	}
};
