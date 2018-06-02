#pragma once

#include "libmsstyle/StyleProperty.h"

#include <wx/artprov.h>
#include <wx/xrc/xmlres.h>
#include <wx/string.h>
#include <wx/stattext.h>
#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/sizer.h>
#include <wx/choice.h>
#include <wx/statline.h>
#include <wx/button.h>
#include <wx/dialog.h>

class AddPropertyDialog : public wxDialog
{
private:

protected:
	wxStaticText* m_staticText5;
	wxStaticText* m_staticText6;
	wxChoice* typeBox;
	wxChoice* propBox;
	wxStaticText* descriptionLabel;
	wxStaticLine* m_staticLine;
	wxButton* okButton;

	virtual void OnTypeSelectionChanged(wxCommandEvent& event);
	virtual void OnPropertySelectionChanged(wxCommandEvent& event);
	virtual void OnOkClicked(wxCommandEvent& event);

public:

	AddPropertyDialog(wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxT("Choose a property"), const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(320, 200), long style = wxDEFAULT_DIALOG_STYLE);
	~AddPropertyDialog();

	int ShowModal(libmsstyle::StyleProperty& prop);
};