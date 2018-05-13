#include "AboutDialog.h"

AboutDialog::AboutDialog(wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style) : wxDialog(parent, id, title, pos, size, style)
{
	this->SetSizeHints(wxSize(200, 138), wxSize(-1, -1));

	wxBoxSizer* bSizer4;
	bSizer4 = new wxBoxSizer(wxVERTICAL);

	wxBoxSizer* bSizer7;
	bSizer7 = new wxBoxSizer(wxHORIZONTAL);

	m_bitmap2 = new wxStaticBitmap(this, wxID_ANY, wxBitmap(wxT("ABOUTICON"), wxBITMAP_TYPE_RESOURCE), wxDefaultPosition, wxDefaultSize, 0);
	bSizer7->Add(m_bitmap2, 0, wxALL, 5);

	m_staticText1 = new wxStaticText(this, wxID_ANY, wxT("msstyleEditor"), wxDefaultPosition, wxDefaultSize, 0);
	m_staticText1->Wrap(-1);
	m_staticText1->SetFont(wxFont(18, wxFONTFAMILY_SWISS, wxFONTSTYLE_NORMAL, wxFONTWEIGHT_NORMAL, false, wxT("Arial")));

	bSizer7->Add(m_staticText1, 0, wxALL | wxALIGN_CENTER_HORIZONTAL, 5);

	bSizer4->Add(bSizer7, 0, wxEXPAND, 5);

	m_staticText2 = new wxStaticText(this, wxID_ANY, wxT("Version 1.3.0.0"), wxDefaultPosition, wxDefaultSize, 0);
	m_staticText2->Wrap(-1);
	bSizer4->Add(m_staticText2, 0, wxBOTTOM | wxLEFT, 5);

	m_staticText3 = new wxStaticText(this, wxID_ANY, wxT("Copyright (C) 2015-2018 Jakob K."), wxDefaultPosition, wxDefaultSize, 0);
	m_staticText3->Wrap(-1);
	bSizer4->Add(m_staticText3, 0, wxBOTTOM | wxLEFT, 5);

	wxBoxSizer* bSizer5;
	bSizer5 = new wxBoxSizer(wxHORIZONTAL);

	m_staticText4 = new wxStaticText(this, wxID_ANY, wxT("Home:"), wxDefaultPosition, wxDefaultSize, 0);
	m_staticText4->Wrap(-1);
	bSizer5->Add(m_staticText4, 0, wxLEFT, 5);

	m_hyperlink1 = new wxHyperlinkCtrl(this, wxID_ANY, wxT("github.com/nptr/msstyleEditor"), wxT("https://github.com/nptr/msstyleEditor"), wxDefaultPosition, wxDefaultSize, wxHL_DEFAULT_STYLE);
	bSizer5->Add(m_hyperlink1, 1, wxLEFT, 2);


	bSizer4->Add(bSizer5, 1, wxEXPAND, 5);


	this->SetSizer(bSizer4);
	this->Layout();

	this->Centre(wxBOTH);
	this->SetForegroundColour(wxSystemSettings::GetColour(wxSYS_COLOUR_WINDOW));
	this->SetBackgroundColour(wxSystemSettings::GetColour(wxSYS_COLOUR_WINDOW));
}


AboutDialog::~AboutDialog()
{
}
