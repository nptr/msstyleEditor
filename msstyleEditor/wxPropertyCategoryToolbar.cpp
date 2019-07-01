#include "wxPropertyCategoryToolbar.h"

wxCategoryToolbarRenderer::~wxCategoryToolbarRenderer()
{

}

bool wxCategoryToolbarRenderer::Render(wxDC& dc, const wxRect& rect,
	const wxPropertyGrid* propertyGrid, wxPGProperty* property,
	int column, int item, int flags) const
{
	wxString text;
	const wxPGCell* cell = NULL;

	int preDrawFlags = flags;
	property->GetDisplayInfo(column, item, flags, &text, &cell);
	PreDrawCell(dc, rect, *cell, preDrawFlags);

	if (!text.IsEmpty())
	{
		const wxColour& hCol = propertyGrid->GetCellDisabledTextColour();
		dc.SetTextForeground(hCol);
		DrawText(dc, rect, 0, text);
	}

	int yMargin = 0;// rect.GetHeight() / 10;
	int featureHeight = rect.GetHeight() - (2 * yMargin);
	int featureWidth = featureHeight;

	wxPropertyCategoryToolbar* cat = dynamic_cast<wxPropertyCategoryToolbar*>(property);
	if (column == 1)
	{
		wxRect featureRect(rect.GetRight() - featureWidth - 4,
			rect.GetTop() + yMargin,
			featureWidth,
			featureHeight);

		if (cat->IsExpanded() || cat->GetChildCount() == 0)
		{
			if (cat->GetChildCount() > 0)
			{
				cat->m_removeButton->SetBackgroundColour(cell->GetBgCol());
				cat->m_removeButton->SetSize(featureRect);
				cat->m_removeButton->Show();

				featureRect.SetX(featureRect.GetX() - featureWidth - 4);
			}

			cat->m_addButton->SetBackgroundColour(cell->GetBgCol());
			cat->m_addButton->SetSize(featureRect);
			cat->m_addButton->Show();
		}
		else
		{
			cat->m_addButton->Hide();
			cat->m_removeButton->Hide();
		}
	}

	PostDrawCell(dc, propertyGrid, *cell, preDrawFlags);

	return true;
}


wxPropertyCategoryToolbar::wxPropertyCategoryToolbar(wxWindow* parent, const wxString& label, const wxString& name)
	: wxPropertyCategory(label, name)
	, m_addButton(new wxButton(parent, wxID_ANY, wxT("+")))
	, m_removeButton(new wxButton(parent, wxID_ANY, wxT("-")))
	, m_myParent(parent)
{
	m_addButton->Hide();
	m_addButton->SetForegroundColour(*wxBLACK);
	m_addButton->Bind(wxEVT_BUTTON, &wxPropertyCategoryToolbar::AddHandlerWrapper, this);

	m_removeButton->Hide();
	m_removeButton->SetForegroundColour(*wxRED);
	m_removeButton->Bind(wxEVT_BUTTON, &wxPropertyCategoryToolbar::RemoveHandlerWrapper, this);
}

wxPropertyCategoryToolbar::~wxPropertyCategoryToolbar()
{
	m_addButton->Destroy();
	m_removeButton->Destroy();
}

wxPGCellRenderer* wxPropertyCategoryToolbar::GetCellRenderer(int column) const
{
	return &m_renderer;
}

void wxPropertyCategoryToolbar::AddHandlerWrapper(wxCommandEvent& event)
{
	wxCommandEvent evt(wxEVT_COMMAND_BUTTON_CLICKED, ID_ADD_PROP);
	evt.SetEventObject(this);
	wxPostEvent(m_myParent, evt);
}

void wxPropertyCategoryToolbar::RemoveHandlerWrapper(wxCommandEvent& event)
{
	wxCommandEvent evt(wxEVT_COMMAND_BUTTON_CLICKED, ID_REMOVE_PROP);
	evt.SetEventObject(this);
	wxPostEvent(m_myParent, evt);
}
