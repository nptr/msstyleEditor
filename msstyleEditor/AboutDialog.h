#pragma once

#include <wx/bitmap.h>
#include <wx/image.h>
#include <wx/icon.h>
#include <wx/statbmp.h>
#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/string.h>
#include <wx/stattext.h>
#include <wx/sizer.h>
#include <wx/hyperlink.h>
#include <wx/dialog.h>


class AboutDialog : public wxDialog
{
private:

protected:
	wxStaticBitmap* m_bitmap2;
	wxStaticText* m_staticText1;
	wxStaticText* m_staticText2;
	wxStaticText* m_staticText3;
	wxStaticText* m_staticText4;
	wxHyperlinkCtrl* m_hyperlink1;

public:

	AboutDialog(wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxEmptyString, const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(250, 170), long style = wxDEFAULT_DIALOG_STYLE);
	~AboutDialog();

};