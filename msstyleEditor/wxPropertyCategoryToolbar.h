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

	/**
	Returns @true if rendered something in the foreground (text or
	bitmap.
	*/
	virtual bool Render(wxDC& dc,
		const wxRect& rect,
		const wxPropertyGrid* propertyGrid,
		wxPGProperty* property,
		int column,
		int item,
		int flags) const;
};

class wxPropertyCategoryToolbar : public wxPropertyCategory
{
	friend wxCategoryToolbarRenderer;

public:
	wxPropertyCategoryToolbar(wxWindow* parent, const wxString& label, const wxString& name = wxPG_LABEL);
	virtual ~wxPropertyCategoryToolbar();

	virtual wxPGCellRenderer* GetCellRenderer(int column) const;

	inline void SetAddPropertyHandler(wxObjectEventFunction func) { m_addButton->Connect(wxEVT_COMMAND_BUTTON_CLICKED, func); }
	inline void SetRemovePropertyHandler(wxObjectEventFunction func) { m_removeButton->Connect(wxEVT_COMMAND_BUTTON_CLICKED, func); }

private:
	wxButton* m_addButton;
	wxButton* m_removeButton;
	mutable wxCategoryToolbarRenderer m_renderer;
};