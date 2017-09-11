#include "ImageViewCtrl.h"

#define CELL_SIZE 12

ImageViewCtrl::ImageViewCtrl()
	: wxScrolledWindow()
	, bitmap(0,0)
	, backgroundStyle(BackgroundStyle::LightGrey)
	, cellSize(CELL_SIZE, CELL_SIZE)
{
	wxWindowBase::SetBackgroundStyle(wxBackgroundStyle::wxBG_STYLE_PAINT);
}

ImageViewCtrl::ImageViewCtrl(wxWindow *parent, wxWindowID id, const wxPoint& pos, const wxSize& size, long style)
	: bitmap(0, 0)
	, backgroundStyle(BackgroundStyle::LightGrey)
	, cellSize(CELL_SIZE, CELL_SIZE)
{
	wxWindowBase::SetBackgroundStyle(wxBackgroundStyle::wxBG_STYLE_PAINT);
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

void ImageViewCtrl::SetBackgroundStyle(BackgroundStyle style)
{
	backgroundStyle = style;
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
	wxPaintDC dc(this);
	PrepareDC(dc);

	wxSize dcSize = dc.GetSize();
	dc.SetPen(*wxTRANSPARENT_PEN); // no border

	if (backgroundStyle == BackgroundStyle::Chessboard)
	{
		int numXCells = dcSize.GetWidth() / cellSize.GetWidth() + 1;
		int numYCells = dcSize.GetHeight() / cellSize.GetHeight() + 1;

		for (int x = 0; x < numXCells; ++x)
		{
			for (int y = 0; y < numYCells; ++y)
			{
				if ((x + y) % 2 > 0)
					dc.SetBrush(*wxWHITE_BRUSH);
				else dc.SetBrush(*wxLIGHT_GREY_BRUSH);

				dc.DrawRectangle(x * cellSize.GetWidth(),
					y * cellSize.GetHeight(),
					cellSize.GetWidth(),
					cellSize.GetHeight());
			}
		}
	}
	else
	{
		if (backgroundStyle == BackgroundStyle::Black)
			dc.SetBrush(*wxBLACK_BRUSH);
		else if (backgroundStyle == BackgroundStyle::LightGrey)
			dc.SetBrush(wxBrush(wxColour(0xEFEFEF)));
		else dc.SetBrush(*wxWHITE_BRUSH);

		dc.DrawRectangle(this->GetClientRect());
	}

	if (!bitmap.IsOk())
		return;

	wxSize bmpSize = bitmap.GetSize();
	if (bmpSize.GetWidth() + bmpSize.GetHeight() == 0)
		return;

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