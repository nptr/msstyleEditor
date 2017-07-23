#include "SearchDialog.h"

SearchDialog::SearchDialog(wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style) : wxDialog(parent, id, title, pos, size, style)
{
	this->SetSizeHints(wxDefaultSize, wxDefaultSize);

	wxBoxSizer* bSizer1;
	bSizer1 = new wxBoxSizer(wxVERTICAL);

	searchBar = new wxSearchCtrl(this, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0);
#ifndef __WXMAC__
	searchBar->ShowSearchButton(true);
#endif
	searchBar->ShowCancelButton(false);
	bSizer1->Add(searchBar, 0, wxALL | wxEXPAND, 5);

	btFindNext = new wxButton(this, wxID_ANY, wxT("Next"), wxDefaultPosition, wxDefaultSize, 0);
	bSizer1->Add(btFindNext, 0, wxALL | wxEXPAND, 5);
	
	btFindNext->Connect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(SearchDialog::OnNextButtonClicked), NULL, this);
	searchBar->Connect(wxEVT_SEARCHCTRL_SEARCH_BTN, wxCommandEventHandler(SearchDialog::OnNextButtonClicked), NULL, this);
	searchBar->Connect(wxEVT_TEXT_ENTER, wxCommandEventHandler(SearchDialog::OnNextButtonClicked), NULL, this);
	
	this->SetSizer(bSizer1);
	this->Layout();
	this->Centre(wxBOTH);

	// hack: only after a focus switch,
	// the wxEVT_TEXT_ENTER works..
	btFindNext->SetFocus();
	searchBar->SetFocus();
}

SearchDialog::~SearchDialog()
{
}

void SearchDialog::SetSearchHandler(ISearchDialogListener* handler)
{
	this->handler = handler;
}

void SearchDialog::OnNextButtonClicked(wxCommandEvent& evt)
{
	if (handler != nullptr)
		handler->OnFindNext(searchBar->GetValue().ToStdString());
}