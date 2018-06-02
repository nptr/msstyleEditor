#include "HelpDialog.h"
#include "resource.h"

#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/string.h>
#include <wx/sizer.h>

HelpDialog::HelpDialog(wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style) : wxDialog(parent, id, title, pos, size, style)
{
	this->SetSizeHints(wxDefaultSize, wxDefaultSize);
	this->SetForegroundColour(wxSystemSettings::GetColour(wxSYS_COLOUR_WINDOW));
	this->SetBackgroundColour(wxSystemSettings::GetColour(wxSYS_COLOUR_WINDOW));

	wxBoxSizer* bSizer6;
	bSizer6 = new wxBoxSizer(wxVERTICAL);

	helpBox = new wxTextCtrl(this, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, wxTE_READONLY | wxTE_MULTILINE | wxNO_BORDER );
	bSizer6->Add(helpBox, 1, wxALL | wxEXPAND, 5);


	this->SetSizer(bSizer6);
	this->Layout();

	this->Centre(wxBOTH);

	
	HRSRC hResLic = FindResource(NULL, MAKEINTRESOURCE(IDR_LICENSE), RT_HTML);
	hResLicData = LoadResource(NULL, hResLic);
	char* license = (char*)LockResource(hResLicData);

	HRSRC hResCaps = FindResource(NULL, MAKEINTRESOURCE(IDR_CAPS), RT_HTML);
	hResCapsData = LoadResource(NULL, hResCaps);
	char* caps = (char*)LockResource(hResCapsData);

	helpBox->AppendText(license);
	helpBox->AppendText(caps);
}

HelpDialog::~HelpDialog()
{
	UnlockResource(hResLicData);
	UnlockResource(hResCapsData);
}
