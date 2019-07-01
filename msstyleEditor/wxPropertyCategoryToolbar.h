#pragma once

#include <wx/font.h>
#include <wx/dc.h>
#include <wx/event.h>
#include <wx/validate.h>
#include <wx/propgrid/propgrid.h>
#include <wx/propgrid/property.h>
#include <wx/string.h>
#include <wx/gdicmn.h>


class wxCategoryToolbarRenderer : public wxPGCellRenderer
{
public:
	virtual ~wxCategoryToolbarRenderer();

	virtual bool Render(wxDC& dc, const wxRect& rect, const wxPropertyGrid* propertyGrid,
		wxPGProperty* property, int column, int item, int flags) const;
};


class wxPropertyCategoryToolbar : public wxPropertyCategory
{
	friend wxCategoryToolbarRenderer;

public:

	wxPropertyCategoryToolbar(wxWindow* parent, const wxString& label, const wxString& name = wxPG_LABEL);
	virtual ~wxPropertyCategoryToolbar();

	virtual wxPGCellRenderer* GetCellRenderer(int column) const;

	static const int ID_ADD_PROP = 1;
	static const int ID_REMOVE_PROP = 2;

protected:
	void AddHandlerWrapper(wxCommandEvent& event);
	void RemoveHandlerWrapper(wxCommandEvent& event);

	wxButton* m_addButton;
	wxButton* m_removeButton;
	wxWindow* m_myParent;
	mutable wxCategoryToolbarRenderer m_renderer;
};