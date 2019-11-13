#include "AddPropertyDialog.h"
#include "libmsstyle\VisualStyleDefinitions.h"

using namespace libmsstyle;

const char* typeNameArray[] =
{
	"Enum",
	"Int",
	"Bool",
	"Color",
	"Margin",
	"Filename",
	"Size",
	"Position",
	"HighContrastColor"
};

const int typeIdArray[] =
{
	200,
	202,
	203,
	204,
	205,
	206,
	207,
	208,
	241
};


class wxPropertyClientData : public wxClientData
{
public:
	wxPropertyClientData() : m_data() { }
	wxPropertyClientData(int nameId, const PropertyInfo &data)
		: m_nameId(nameId)
		, m_data(data)
	{}

	int GetNameID() const { return m_nameId; }
	const PropertyInfo& GetPropInfo() const { return m_data; }

private:
	int m_nameId;
	PropertyInfo m_data;
};

AddPropertyDialog::AddPropertyDialog(wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style) : wxDialog(parent, id, title, pos, size, style)
{
	this->SetSizeHints(wxDefaultSize, wxDefaultSize);

	wxBoxSizer* sizerMain;
	sizerMain = new wxBoxSizer(wxVERTICAL);

	wxBoxSizer* sizerHalfHorz;
	sizerHalfHorz = new wxBoxSizer(wxHORIZONTAL);

	wxBoxSizer* bSizer21;
	bSizer21 = new wxBoxSizer(wxVERTICAL);

	wxBoxSizer* bSizer20;
	bSizer20 = new wxBoxSizer(wxHORIZONTAL);

	m_staticText5 = new wxStaticText(this, wxID_ANY, wxT("Type:"), wxDefaultPosition, wxDefaultSize, 0);
	m_staticText5->Wrap(-1);
	m_staticText5->SetMaxSize(wxSize(120, -1));

	bSizer20->Add(m_staticText5, 0, wxTOP | wxRIGHT | wxLEFT | wxALIGN_CENTER_VERTICAL, 5);

	wxArrayString typeBoxChoices;
	typeBoxChoices.Add(typeNameArray[0]);
	typeBoxChoices.Add(typeNameArray[1]);
	typeBoxChoices.Add(typeNameArray[2]);
	typeBoxChoices.Add(typeNameArray[3]);
	typeBoxChoices.Add(typeNameArray[4]);
	typeBoxChoices.Add(typeNameArray[5]);
	typeBoxChoices.Add(typeNameArray[6]);
	typeBoxChoices.Add(typeNameArray[7]);
	typeBoxChoices.Add(typeNameArray[8]);
	typeBox = new wxChoice(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, typeBoxChoices, 0);
	typeBox->SetSelection(0);
	typeBox->SetMaxSize(wxSize(120, -1));

	bSizer20->Add(typeBox, 1, wxALIGN_CENTER_VERTICAL | wxTOP | wxRIGHT | wxLEFT, 5);


	bSizer21->Add(bSizer20, 0, wxEXPAND, 5);

	propBox = new wxListBox(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, 0, NULL, wxLB_SINGLE | wxLB_SORT);
	bSizer21->Add(propBox, 1, wxEXPAND | wxALL, 5);


	sizerHalfHorz->Add(bSizer21, 1, wxEXPAND, 5);

	wxStaticBoxSizer* sbSizer2;
	sbSizer2 = new wxStaticBoxSizer(new wxStaticBox(this, wxID_ANY, wxT("Description")), wxVERTICAL);

	descriptionLabel = new wxStaticText(sbSizer2->GetStaticBox(), wxID_ANY, wxT("-"), wxDefaultPosition, wxDefaultSize, wxST_NO_AUTORESIZE);
	descriptionLabel->Wrap(300);
	sbSizer2->Add(descriptionLabel, 1, wxALL | wxEXPAND, 5);


	sizerHalfHorz->Add(sbSizer2, 1, wxEXPAND | wxBOTTOM | wxRIGHT, 5);


	sizerMain->Add(sizerHalfHorz, 1, wxEXPAND, 5);

	wxBoxSizer* bSizer14;
	bSizer14 = new wxBoxSizer(wxHORIZONTAL);


	sizerMain->Add(bSizer14, 0, wxEXPAND, 5);

	m_staticLine = new wxStaticLine(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxLI_HORIZONTAL);
	sizerMain->Add(m_staticLine, 0, wxTOP | wxRIGHT | wxLEFT | wxEXPAND, 5);

	wxBoxSizer* bSizer22;
	bSizer22 = new wxBoxSizer(wxHORIZONTAL);

	okButton = new wxButton(this, wxID_ANY, wxT("Ok"), wxDefaultPosition, wxDefaultSize, 0);
	bSizer22->Add(okButton, 0, wxALL | wxALIGN_RIGHT, 5);

	cancelButton = new wxButton(this, wxID_ANY, wxT("Cancel"), wxDefaultPosition, wxDefaultSize, 0);
	bSizer22->Add(cancelButton, 0, wxALL, 5);


	sizerMain->Add(bSizer22, 0, wxALIGN_RIGHT, 5);


	this->SetSizer(sizerMain);
	this->Layout();

	this->Centre(wxBOTH);

	// Connect Events
	typeBox->Connect(wxEVT_COMMAND_CHOICE_SELECTED, wxCommandEventHandler(AddPropertyDialog::OnTypeSelectionChanged), NULL, this);
	propBox->Connect(wxEVT_COMMAND_LISTBOX_SELECTED, wxCommandEventHandler(AddPropertyDialog::OnPropertySelectionChanged), NULL, this);
	okButton->Connect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(AddPropertyDialog::OnOkClicked), NULL, this);
	cancelButton->Connect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(AddPropertyDialog::OnCancelClicked), NULL, this);

	OnTypeSelectionChanged(wxCommandEvent());
}

void AddPropertyDialog::OnTypeSelectionChanged(wxCommandEvent& event)
{
	propBox->Clear();

	int selectedIndex = typeBox->GetSelection();
	if (selectedIndex < 0)
		return;

	int typeId = typeIdArray[selectedIndex];
	for (auto& it = PROPERTY_INFO_MAP.begin(); it != PROPERTY_INFO_MAP.end(); ++it)
	{
		// Select all properties matching our type, but not the entry of the type itself
		if (typeId == it->second.type &&
			typeId != it->first)
		{
			propBox->Append(it->second.name, new wxPropertyClientData(it->first, it->second));
		}
	}

	propBox->Select(0);
	wxCommandEvent dummy;
	OnPropertySelectionChanged(dummy);
}

void AddPropertyDialog::OnPropertySelectionChanged(wxCommandEvent& event)
{
	int selectedIndex = propBox->GetSelection();
	if (selectedIndex < 0)
		return;

	wxPropertyClientData* cd = static_cast<wxPropertyClientData*>(propBox->GetClientObject(selectedIndex));
	descriptionLabel->SetLabel(cd->GetPropInfo().description);
}

void AddPropertyDialog::OnOkClicked(wxCommandEvent& event)
{
	EndModal(wxID_OK);
}

void AddPropertyDialog::OnCancelClicked(wxCommandEvent& event)
{
	EndModal(wxID_CANCEL);
}

int AddPropertyDialog::ShowModal(StyleProperty& prop)
{
	int ret = wxDialog::ShowModal();
	if (ret == wxID_OK)
	{
		int selectedIndex = propBox->GetSelection();
		if (selectedIndex < 0)
			return wxID_CANCEL;

		wxPropertyClientData* cd = static_cast<wxPropertyClientData*>(propBox->GetClientObject(selectedIndex));

		prop.Initialize((libmsstyle::IDENTIFIER)cd->GetPropInfo().type,
			(libmsstyle::IDENTIFIER)cd->GetNameID());
	}

	return ret;
}

AddPropertyDialog::~AddPropertyDialog()
{
	typeBox->Disconnect(wxEVT_COMMAND_CHOICE_SELECTED, wxCommandEventHandler(AddPropertyDialog::OnTypeSelectionChanged), NULL, this);
	propBox->Disconnect(wxEVT_COMMAND_LISTBOX_SELECTED, wxCommandEventHandler(AddPropertyDialog::OnPropertySelectionChanged), NULL, this);
	okButton->Disconnect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(AddPropertyDialog::OnOkClicked), NULL, this);
	cancelButton->Disconnect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(AddPropertyDialog::OnCancelClicked), NULL, this);
}