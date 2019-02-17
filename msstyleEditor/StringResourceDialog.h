#pragma once

#include <wx/artprov.h>
#include <wx/xrc/xmlres.h>
#include <wx/listctrl.h>
#include <wx/gdicmn.h>
#include <wx/font.h>
#include <wx/colour.h>
#include <wx/settings.h>
#include <wx/string.h>
#include <wx/textctrl.h>
#include <wx/button.h>
#include <wx/sizer.h>
#include <wx/dialog.h>

#include <libmsstyle\StringTable.h>

class StringResourceDialog : public wxDialog
{
private:
    libmsstyle::StringTable& m_table;

protected:
    wxListCtrl* resourceTable;
    wxTextCtrl* idField;
    wxTextCtrl* textField;
    wxButton* addButton;
    wxButton* deleteButton;

public:

    StringResourceDialog(wxWindow* parent, libmsstyle::StringTable& table, wxWindowID id = wxID_ANY, const wxString& title = wxT("String Resources"), const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(495, 440), long style = wxDEFAULT_DIALOG_STYLE);
    ~StringResourceDialog();

    void OnListItemSelected(wxListEvent&);
    void OnListKeyDown(wxListEvent& args);
    void OnSetStringEntry(wxCommandEvent& event);
    void OnRemoveResourceEntry(wxCommandEvent& event);

    void FillTableControl(libmsstyle::StringTable& table);
};