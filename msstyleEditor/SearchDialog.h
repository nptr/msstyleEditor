#pragma once

#include <wx/artprov.h>
#include <wx/xrc/xmlres.h>
#include <wx/string.h>
#include <wx/srchctrl.h>
#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/button.h>
#include <wx/sizer.h>
#include <wx/dialog.h>

#include "SearchDialogListener.h"

class SearchDialog : public wxDialog
{
private:

protected:
	wxSearchCtrl* searchBar;
	wxButton* btFindNext;
	ISearchDialogListener* handler;

	void OnNextButtonClicked(wxCommandEvent& evt);

public:

	SearchDialog(wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxT("Find"), const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(220, 100), long style = wxDEFAULT_DIALOG_STYLE | wxSTAY_ON_TOP);
	~SearchDialog();

	void SetSearchHandler(ISearchDialogListener* handler);
};