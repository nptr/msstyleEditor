#pragma once

#include <wx/artprov.h>
#include <wx/xrc/xmlres.h>
#include <wx/string.h>
#include <wx/choice.h>
#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/sizer.h>
#include <wx/srchctrl.h>
#include <wx/button.h>
#include <wx/dialog.h>

#include "SearchDialogListener.h"

class SearchDialog : public wxDialog
{
private:

protected:
	wxChoice* searchType;
	wxChoice* typeBox;
	wxSearchCtrl* searchBar;
	ISearchDialogListener* handler;

	SearchProperties search;

	void OnNextButtonClicked(wxCommandEvent& evt);
	void OnSearchModeChanged(wxCommandEvent& evt);
	void OnSearchTypeChanged(wxCommandEvent& evt);
public:
	void SetSearchHandler(ISearchDialogListener* handler);

	SearchDialog(wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxT("Find"), const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(199, 97), long style = wxDEFAULT_DIALOG_STYLE);
	~SearchDialog();

};