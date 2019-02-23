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
	"Position"
};

const int typeIdArray[] =
{
	200,
	202,
	203,
	204,
	205,
	208
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
	this->SetSizeHints(size, size);

	wxBoxSizer* bSizer13;
	bSizer13 = new wxBoxSizer(wxVERTICAL);

	wxBoxSizer* bSizer16;
	bSizer16 = new wxBoxSizer(wxHORIZONTAL);

	m_staticText5 = new wxStaticText(this, wxID_ANY, wxT("Type:"), wxDefaultPosition, wxDefaultSize, 0);
	m_staticText5->Wrap(-1);
	m_staticText5->SetMaxSize(wxSize(120, -1));
	bSizer16->Add(m_staticText5, 1, wxTOP | wxRIGHT | wxLEFT, 5);

	m_staticText6 = new wxStaticText(this, wxID_ANY, wxT("Property:"), wxDefaultPosition, wxDefaultSize, 0);
	m_staticText6->Wrap(-1);
	bSizer16->Add(m_staticText6, 1, wxTOP | wxRIGHT | wxLEFT, 5);


	bSizer13->Add(bSizer16, 0, wxEXPAND, 5);

	wxBoxSizer* bSizer14;
	bSizer14 = new wxBoxSizer(wxHORIZONTAL);

	wxArrayString typeBoxChoices;
	typeBoxChoices.Add(typeNameArray[0]);
	typeBoxChoices.Add(typeNameArray[1]);
	typeBoxChoices.Add(typeNameArray[2]);
	typeBoxChoices.Add(typeNameArray[3]);
	typeBoxChoices.Add(typeNameArray[4]);
	typeBoxChoices.Add(typeNameArray[5]);
	typeBox = new wxChoice(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, typeBoxChoices, 0);
	typeBox->SetSelection(0);
	typeBox->SetMaxSize(wxSize(120, -1));

	bSizer14->Add(typeBox, 1, wxALL, 5);

	wxArrayString propBoxChoices;
	propBox = new wxChoice(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, propBoxChoices, wxCB_SORT);
	propBox->SetSelection(0);
	bSizer14->Add(propBox, 1, wxALL, 5);
	bSizer13->Add(bSizer14, 0, wxEXPAND, 5);

	descriptionLabel = new wxStaticText(this, wxID_ANY, wxT("Description:"), wxDefaultPosition, wxDefaultSize, wxST_NO_AUTORESIZE);
    descriptionLabel->SetSizeHints(wxSize(300, 75));
    descriptionLabel->Wrap(300);
	bSizer13->Add(descriptionLabel, 1, wxALL | wxEXPAND, 5);

	m_staticLine = new wxStaticLine(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxLI_HORIZONTAL);
	bSizer13->Add(m_staticLine, 0, wxEXPAND | wxTOP | wxRIGHT | wxLEFT, 5);

	okButton = new wxButton(this, wxID_ANY, wxT("Ok"), wxDefaultPosition, wxDefaultSize, 0);
	bSizer13->Add(okButton, 0, wxALL | wxALIGN_RIGHT, 5);


	this->SetSizer(bSizer13);
	this->Layout();
    bSizer13->Fit(this);

	this->Centre(wxBOTH);

	typeBox->Connect(wxEVT_COMMAND_CHOICE_SELECTED, wxCommandEventHandler(AddPropertyDialog::OnTypeSelectionChanged), NULL, this);
	propBox->Connect(wxEVT_COMMAND_CHOICE_SELECTED, wxCommandEventHandler(AddPropertyDialog::OnPropertySelectionChanged), NULL, this);
	okButton->Connect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(AddPropertyDialog::OnOkClicked), NULL, this);

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
	descriptionLabel->SetLabel(wxString("Description: ") + cd->GetPropInfo().description);
}

void AddPropertyDialog::OnOkClicked(wxCommandEvent& event)
{
	EndModal(wxID_OK);
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
	propBox->Disconnect(wxEVT_COMMAND_CHOICE_SELECTED, wxCommandEventHandler(AddPropertyDialog::OnPropertySelectionChanged), NULL, this);
	okButton->Disconnect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(AddPropertyDialog::OnOkClicked), NULL, this);
}