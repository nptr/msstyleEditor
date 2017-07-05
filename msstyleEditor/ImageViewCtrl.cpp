#include "ImageViewCtrl.h"

ImageViewCtrl::ImageViewCtrl()
	: wxScrolledWindow()
	, bitmap(0,0)
{
	
}

ImageViewCtrl::ImageViewCtrl(wxWindow *parent, wxWindowID id, const wxPoint& pos, const wxSize& size, long style)
{
	wxScrolledWindow::Create(parent, id, pos, size, style | wxFULL_REPAINT_ON_RESIZE);
}

ImageViewCtrl::~ImageViewCtrl()
{

}

void ImageViewCtrl::SetImage(wxImage& image)
{
	bitmap = wxBitmap(image);
	SetVirtualSize(bitmap.GetWidth(), bitmap.GetHeight());
	this->Refresh();
	this->Update();
}

void ImageViewCtrl::RemoveImage()
{
	bitmap = wxBitmap();
	this->Refresh();
	this->Update();
}

void ImageViewCtrl::OnMouse(wxMouseEvent &event)
{
	event.ResumePropagation(1);
	event.Skip();
}

void ImageViewCtrl::OnPaint(wxPaintEvent &event)
{
	if (!bitmap.IsOk())
		return;
	
	wxPaintDC dc(this);
	PrepareDC(dc);

	wxSize dcSize = dc.GetSize();
	wxSize bmpSize = bitmap.GetSize();

	int drawX = (dcSize.GetWidth() / 2) - (bmpSize.GetWidth() / 2);
	int drawY = (dcSize.GetHeight() / 2) - (bmpSize.GetHeight() / 2);

	dc.DrawBitmap(bitmap, drawX, drawY, false);
	
	wxString txt;
	txt << bitmap.GetWidth() << " x " << bitmap.GetHeight() << "px";
	dc.DrawText(txt, 5, 5);
}

BEGIN_EVENT_TABLE(ImageViewCtrl, wxScrolledWindow)
EVT_PAINT(ImageViewCtrl::OnPaint)
EVT_MOUSE_EVENTS(ImageViewCtrl::OnMouse)
END_EVENT_TABLE()