#include "StringResourceDialog.h"


StringResourceDialog::StringResourceDialog(wxWindow* parent, libmsstyle::StringTable& table, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style) 
    : wxDialog(parent, id, title, pos, size, style)
    , m_table(table)
{
    this->SetSizeHints(wxDefaultSize, wxDefaultSize);

    wxBoxSizer* bSizer15;
    bSizer15 = new wxBoxSizer(wxVERTICAL);

    resourceTable = new wxListCtrl(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxLC_REPORT | wxLC_SINGLE_SEL);
    bSizer15->Add(resourceTable, 1, wxALL | wxEXPAND, 5);

    wxBoxSizer* bSizer19;
    bSizer19 = new wxBoxSizer(wxHORIZONTAL);

    idField = new wxTextCtrl(this, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0);
    idField->SetMaxSize(wxSize(70, -1));
    idField->SetHint(wxT("ID"));

    bSizer19->Add(idField, 0, wxALL, 5);

    textField = new wxTextCtrl(this, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0);
    textField->SetMinSize(wxSize(200, -1));
    textField->SetHint(wxT("Text"));

    bSizer19->Add(textField, 0, wxALL, 5);

    wxBoxSizer* bSizer22;
    bSizer22 = new wxBoxSizer(wxHORIZONTAL);

    addButton = new wxButton(this, wxID_ANY, wxT("Set"), wxDefaultPosition, wxDefaultSize, 0);
    bSizer22->Add(addButton, 0, wxALL, 5);

    deleteButton = new wxButton(this, wxID_ANY, wxT("Remove"), wxDefaultPosition, wxDefaultSize, 0);
    bSizer22->Add(deleteButton, 0, wxALL, 5);
    bSizer19->Add(bSizer22, 0, wxEXPAND, 5);
    bSizer15->Add(bSizer19, 0, wxEXPAND, 5);


    this->SetSizer(bSizer15);
    this->Layout();

    this->Centre(wxBOTH);

    FillTableControl(table);

    resourceTable->Connect(wxEVT_COMMAND_LIST_ITEM_SELECTED, wxListEventHandler(StringResourceDialog::OnListItemSelected), NULL, this);
    resourceTable->Connect(wxEVT_COMMAND_LIST_KEY_DOWN, wxListEventHandler(StringResourceDialog::OnListKeyDown), NULL, this);
    addButton->Connect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(StringResourceDialog::OnSetStringEntry), NULL, this);
    deleteButton->Connect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(StringResourceDialog::OnRemoveResourceEntry), NULL, this);
}

StringResourceDialog::~StringResourceDialog()
{
    resourceTable->Disconnect(wxEVT_COMMAND_LIST_ITEM_SELECTED, wxListEventHandler(StringResourceDialog::OnListItemSelected), NULL, this);
    resourceTable->Disconnect(wxEVT_COMMAND_LIST_KEY_DOWN, wxListEventHandler(StringResourceDialog::OnListKeyDown), NULL, this);
    addButton->Disconnect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(StringResourceDialog::OnSetStringEntry), NULL, this);
    deleteButton->Disconnect(wxEVT_COMMAND_BUTTON_CLICKED, wxCommandEventHandler(StringResourceDialog::OnRemoveResourceEntry), NULL, this);
}

void StringResourceDialog::FillTableControl(libmsstyle::StringTable& table)
{
    char buffer[32];
    resourceTable->Freeze();
    resourceTable->ClearAll();
    resourceTable->InsertColumn(0, wxT("ID"), 0, 100);
    resourceTable->InsertColumn(1, wxT("Text"), 0, 300);
    for (auto it = table.begin(); it != table.end(); ++it)
    {
        sprintf(buffer, "%d", it->first);
        long index = resourceTable->InsertItem(resourceTable->GetItemCount(), buffer);
        resourceTable->SetItem(index, 1, it->second);
    }
    resourceTable->Thaw();
}

void StringResourceDialog::OnListItemSelected(wxListEvent& args)
{
    int selectedIndex = args.GetIndex();
    if (selectedIndex != -1)
    {
        idField->SetValue(resourceTable->GetItemText(selectedIndex, 0));
        textField->SetValue(resourceTable->GetItemText(selectedIndex, 1));
    }
}

void StringResourceDialog::OnListKeyDown(wxListEvent& args)
{
    if (args.GetKeyCode() == wxKeyCode::WXK_DELETE)
    {
        int selectedIndex = args.GetIndex();
        if (selectedIndex != -1)
        {
            resourceTable->DeleteItem(selectedIndex);
        }
    }
}

void StringResourceDialog::OnSetStringEntry(wxCommandEvent& event)
{
    wxString idFieldText = idField->GetValue();

    long id = 0;
    if (idFieldText.ToLong(&id, 10))
    {
        m_table.Set(id, textField->GetValue().ToStdString());
        FillTableControl(m_table); // lazy ui update

        for (int i = 0; i < resourceTable->GetItemCount(); ++i)
        {
            wxString uiEntry = resourceTable->GetItemText(i, 0);
            if (uiEntry == idFieldText)
            {
                resourceTable->SetItemState(i, wxLIST_STATE_SELECTED, wxLIST_STATE_SELECTED);
                resourceTable->ScrollLines(i); // FillTableControl() set the scroll pos to top so we can just scroll down here
                break;
            }
        }
    }
}

void StringResourceDialog::OnRemoveResourceEntry(wxCommandEvent& event)
{
    wxString idFieldText = idField->GetValue();

    long id = 0;
    if (idFieldText.ToLong(&id, 10))
    {
        m_table.Remove(id);

        int selectedIndex = resourceTable->GetNextItem(-1, 1, wxLIST_STATE_SELECTED);
        if (selectedIndex != -1)
        {
            resourceTable->DeleteItem(selectedIndex);
        }
    }
}


