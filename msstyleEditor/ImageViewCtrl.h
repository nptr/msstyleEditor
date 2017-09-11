#pragma once
#include <wx\wx.h>

class ImageViewCtrl : public wxScrolledWindow
{
public:
	enum BackgroundStyle
	{
		White,
		LightGrey,
		Chessboard,
		Black
	};


	ImageViewCtrl();
	ImageViewCtrl(wxWindow *parent, wxWindowID id, const wxPoint& pos, const wxSize& size, long style);
	virtual ~ImageViewCtrl();

	void Create(wxWindow *parent, wxWindowID id = -1);
	void SetImage(wxImage &image);
	void SetBackgroundStyle(BackgroundStyle style);
	void RemoveImage();

protected:
	wxBitmap bitmap;
	BackgroundStyle backgroundStyle;
	wxSize cellSize;

	void OnMouse(wxMouseEvent &event);
	void OnPaint(wxPaintEvent &event);

private:
	DECLARE_EVENT_TABLE()
};

