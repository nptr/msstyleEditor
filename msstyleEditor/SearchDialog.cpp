#include "SearchDialog.h"
#include "libmsstyle/VisualStyleDefinitions.h"

SearchDialog::SearchDialog(wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style) : wxDialog(parent, id, title, pos, size, style)
{
	this->SetSizeHints(wxDefaultSize, wxDefaultSize);

	wxBoxSizer* bSizer9;
	bSizer9 = new wxBoxSizer(wxVERTICAL);

	wxBoxSizer* bSizer10;
	bSizer10 = new wxBoxSizer(wxHORIZONTAL);

	wxString searchTypeChoices[] = { wxT("Class or Part"), wxT("Property") };
	int searchTypeNChoices = sizeof(searchTypeChoices) / sizeof(wxString);
	searchType = new wxChoice(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, searchTypeNChoices, searchTypeChoices, 0);
	searchType->SetSelection(0);
	bSizer10->Add(searchType, 1, wxALL | wxALIGN_CENTER_VERTICAL, 5);

	wxArrayString typeBoxChoices;
	typeBox = new wxChoice(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, typeBoxChoices, 0);
	typeBox->SetSelection(0);
	bSizer10->Add(typeBox, 1, wxALL | wxALIGN_CENTER_VERTICAL, 5);
	bSizer9->Add(bSizer10, 1, wxEXPAND, 5);

	wxBoxSizer* bSizer11;
	bSizer11 = new wxBoxSizer(wxHORIZONTAL);

	searchBar = new wxSearchCtrl(this, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, wxTE_PROCESS_ENTER);
#ifndef __WXMAC__
	searchBar->ShowSearchButton(true);
#endif
	searchBar->ShowCancelButton(false);

	bSizer11->Add(searchBar, 1, wxALL | wxALIGN_CENTER_VERTICAL, 5);
	bSizer9->Add(bSizer11, 1, wxEXPAND, 5);


	searchType->Connect(wxEVT_CHOICE, wxCommandEventHandler(SearchDialog::OnSearchModeChanged), NULL, this);
	typeBox->Connect(wxEVT_CHOICE, wxCommandEventHandler(SearchDialog::OnSearchTypeChanged), NULL, this);
	searchBar->Connect(wxEVT_SEARCHCTRL_SEARCH_BTN, wxCommandEventHandler(SearchDialog::OnNextButtonClicked), NULL, this);
	searchBar->Connect(wxEVT_TEXT_ENTER, wxCommandEventHandler(SearchDialog::OnNextButtonClicked), NULL, this);
	typeBox->Enable(false);
	typeBox->Append("COLOR",	(void*)libmsstyle::IDENTIFIER::COLOR);
	typeBox->Append("MARGINS",	(void*)libmsstyle::IDENTIFIER::MARGINS);
	typeBox->Append("SIZE",		(void*)libmsstyle::IDENTIFIER::SIZE);
	typeBox->Append("POSITION", (void*)libmsstyle::IDENTIFIER::POSITION);
	typeBox->Append("RECT",		(void*)libmsstyle::IDENTIFIER::RECT);
	typeBox->Select(0);

	this->SetSizer(bSizer9);
	this->Layout();

	this->Centre(wxBOTH);

	searchBar->SetFocus();
}

SearchDialog::~SearchDialog()
{
}

void SearchDialog::SetSearchHandler(ISearchDialogListener* handler)
{
	this->handler = handler;
}

void SearchDialog::OnSearchModeChanged(wxCommandEvent& evt)
{
	if (evt.GetSelection() == 0)
	{
		search.mode = SearchProperties::MODE_NAME;
		searchBar->SetDescriptiveText("Search");
		typeBox->Enable(false);
	}
	else
	{
		search.mode = SearchProperties::MODE_PROPERTY;

		wxCommandEvent evt;
		evt.SetClientData(typeBox->GetClientData(typeBox->GetSelection()));
		OnSearchTypeChanged(evt);
		typeBox->Enable(true);
	}
}

void SearchDialog::OnSearchTypeChanged(wxCommandEvent& evt)
{
	search.type = (int)evt.GetClientData();
	switch (search.type)
	{
		case libmsstyle::IDENTIFIER::COLOR:
		{
			searchBar->SetDescriptiveText("r, g, b");
		} break;
		case libmsstyle::IDENTIFIER::SIZE:
		{
			searchBar->SetDescriptiveText("size");
		} break;
		case libmsstyle::IDENTIFIER::MARGINS:
		case libmsstyle::IDENTIFIER::RECT:
		{
			searchBar->SetDescriptiveText("l, t, r, b");
		} break;
		case libmsstyle::IDENTIFIER::POSITION:
		{
			searchBar->SetDescriptiveText("x, y");
		} break;
	}
}

void SearchDialog::OnNextButtonClicked(wxCommandEvent& evt)
{
	if (handler != nullptr)
	{
		search.value = searchBar->GetValue().ToStdString();
		handler->OnFindNext(search);
	}
}