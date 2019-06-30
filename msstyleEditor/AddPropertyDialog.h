#pragma once

#include "libmsstyle/StyleProperty.h"

#include <wx/string.h>
#include <wx/stattext.h>
#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/choice.h>
#include <wx/sizer.h>
#include <wx/listbox.h>
#include <wx/statbox.h>
#include <wx/statline.h>
#include <wx/button.h>
#include <wx/dialog.h>

class AddPropertyDialog : public wxDialog
{
private:

protected:
	wxStaticText* m_staticText5;
	wxChoice* typeBox;
	wxListBox* propBox;
	wxStaticText* descriptionLabel;
	wxStaticLine* m_staticLine;
	wxButton* okButton;
	wxButton* cancelButton;

	// Virtual event handlers, overide them in your derived class
	virtual void OnTypeSelectionChanged(wxCommandEvent& event);
	virtual void OnPropertySelectionChanged(wxCommandEvent& event);
	virtual void OnOkClicked(wxCommandEvent& event);
	virtual void OnCancelClicked(wxCommandEvent& event);

public:

	AddPropertyDialog(wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxT("Choose a property"), const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(380, 440), long style = wxDEFAULT_DIALOG_STYLE);
	~AddPropertyDialog();

	int ShowModal(libmsstyle::StyleProperty& prop);
};