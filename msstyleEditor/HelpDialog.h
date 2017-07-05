#pragma once

#include <wx/textctrl.h>
#include <wx/dialog.h>

class HelpDialog : public wxDialog
{
private:

protected:
	wxTextCtrl* helpBox;

	HGLOBAL hResLicData;
	HGLOBAL hResCapsData;

public:

	HelpDialog(wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxEmptyString, const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(600, 350), long style = wxDEFAULT_DIALOG_STYLE);
	~HelpDialog();

};