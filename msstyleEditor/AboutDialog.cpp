#include "AboutDialog.h"
#include <wx/statbox.h>

AboutDialog::AboutDialog(wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style) : wxDialog(parent, id, title, pos, size, style)
{
    this->SetSizeHints(wxSize(-1, -1), wxSize(-1, -1));

    wxBoxSizer* bSizer24;
    bSizer24 = new wxBoxSizer(wxVERTICAL);

    wxStaticBoxSizer* staticBoxSizer;
    staticBoxSizer = new wxStaticBoxSizer(new wxStaticBox(this, wxID_ANY, wxEmptyString), wxVERTICAL);

    wxBoxSizer* bSizer71;
    bSizer71 = new wxBoxSizer(wxHORIZONTAL);

    wxBoxSizer* bSizer18;
    bSizer18 = new wxBoxSizer(wxHORIZONTAL);

    m_bitmap2 = new wxStaticBitmap(staticBoxSizer->GetStaticBox(), wxID_ANY, wxBitmap(wxT("ABOUTICON"), wxBITMAP_TYPE_RESOURCE), wxDefaultPosition, wxDefaultSize, 0);
    bSizer18->Add(m_bitmap2, 0, wxALL, 5);

    wxBoxSizer* bSizer21;
    bSizer21 = new wxBoxSizer(wxVERTICAL);

    m_staticText2 = new wxStaticText(staticBoxSizer->GetStaticBox(), wxID_ANY, wxT("msstyleEditor v1.4.0.2"), wxDefaultPosition, wxDefaultSize, 0);
    m_staticText2->Wrap(-1);
    bSizer21->Add(m_staticText2, 0, wxBOTTOM | wxLEFT, 5);

    m_staticText3 = new wxStaticText(staticBoxSizer->GetStaticBox(), wxID_ANY, wxT("© 2015-2019, Jakob K."), wxDefaultPosition, wxDefaultSize, 0);
    m_staticText3->Wrap(-1);
    bSizer21->Add(m_staticText3, 0, wxBOTTOM | wxLEFT, 5);


    bSizer18->Add(bSizer21, 1, wxEXPAND, 5);


    bSizer71->Add(bSizer18, 1, wxALIGN_BOTTOM | wxTOP, 5);


    staticBoxSizer->Add(bSizer71, 1, wxEXPAND | wxALL, 5);

    wxBoxSizer* bSizer5;
    bSizer5 = new wxBoxSizer(wxHORIZONTAL);

    m_staticText4 = new wxStaticText(staticBoxSizer->GetStaticBox(), wxID_ANY, wxT("Home:"), wxDefaultPosition, wxDefaultSize, 0);
    m_staticText4->Wrap(-1);
    bSizer5->Add(m_staticText4, 0, wxLEFT, 5);

    m_hyperlink1 = new wxHyperlinkCtrl(staticBoxSizer->GetStaticBox(), wxID_ANY, wxT("github.com/nptr/msstyleEditor"), wxT("https://github.com/nptr/msstyleEditor"), wxDefaultPosition, wxDefaultSize, wxHL_DEFAULT_STYLE);
    bSizer5->Add(m_hyperlink1, 1, wxLEFT, 2);


    staticBoxSizer->Add(bSizer5, 0, wxEXPAND | wxALL, 5);


    bSizer24->Add(staticBoxSizer, 1, wxALL | wxEXPAND, 5);


    this->SetSizer(bSizer24);
    this->Layout();
    bSizer24->Fit(this);

    this->Centre(wxBOTH);
    this->SetForegroundColour(wxSystemSettings::GetColour(wxSYS_COLOUR_WINDOW));
    this->SetBackgroundColour(wxSystemSettings::GetColour(wxSYS_COLOUR_WINDOW));
}


AboutDialog::~AboutDialog()
{
}
