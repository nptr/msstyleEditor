#pragma once

#include <atlapp.h>
#include <atlscrl.h>
#include <atlwin.h>
#include <gdiplus.h>
#include <gdiplusgraphics.h>

class CImageCtrl : public CScrollWindowImpl<CImageCtrl>
{
public:

	enum BackgroundStyle
	{
		White,
		LightGrey,
		Chessboard,
		Black
	};

	DECLARE_WND_CLASS_EX(_T("MsstyleImageView"), 0, -1)

	Gdiplus::Image* m_image;
	SIZE m_drawAt;
	SIZE m_size;
	SIZE m_cellSize;
	BackgroundStyle m_bgStyle;

	CImageCtrl()
		: m_image(NULL)
		, m_drawAt()
		, m_bgStyle(LightGrey)
	{
		m_size.cx = m_size.cy = 1;
		m_cellSize.cx = m_cellSize.cy = 12;
	}

	~CImageCtrl()
	{
		if (m_image != NULL)
		{
			delete m_image;
		}
	}

	BOOL PreTranslateMessage(MSG* pMsg)
	{
		pMsg;
		return FALSE;
	}

	void SetBackgroundStyle(BackgroundStyle style)
	{
		m_bgStyle = style;
	}

	void SetBitmap(Gdiplus::Image* image)
	{
		if (m_image != NULL)
		{
			delete m_image;
		}

		m_image = image;

		if (m_image != NULL)
		{
			m_size.cx = image->GetWidth();
			m_size.cy = image->GetHeight();

			RECT rect;
			GetClientRect(&rect);

			SIZE areaSize;
			areaSize.cx = rect.right - rect.left;
			areaSize.cy = rect.bottom - rect.top;

			AdjustImageOrigin(areaSize, m_size, m_drawAt);
		}
		else
		{
			m_size.cx = m_size.cy = 100;
		}

		this->Invalidate();
	}

	BEGIN_MSG_MAP(CImageCtrl)
		MESSAGE_HANDLER(WM_ERASEBKGND, OnEraseBackground)
		CHAIN_MSG_MAP(CScrollWindowImpl<CImageCtrl>);
	END_MSG_MAP()

	LRESULT OnEraseBackground(UINT /*uMsg*/, WPARAM wParam, LPARAM /*lParam*/, BOOL& /*bHandled*/)
	{
		return TRUE;
	}

	void DoSize(int x, int y)
	{
		if (m_image != NULL)
		{
			SIZE newArea;
			newArea.cx = x;
			newArea.cy = y;

			SIZE imageSize;
			imageSize.cx = m_image->GetWidth();
			imageSize.cy = m_image->GetHeight();

			AdjustImageOrigin(newArea, imageSize, m_drawAt);

			this->Invalidate();
		}
	}

	void DoPaint(CDCHandle dc)
	{
		RECT rc;
		GetClientRect(&rc);
		Gdiplus::Bitmap memBmp(rc.right, rc.bottom);
		Gdiplus::Graphics gMem(&memBmp);

		switch (m_bgStyle)
		{
		case BackgroundStyle::White:
		{
			Gdiplus::SolidBrush bgBrush(Gdiplus::Color(255, 255, 255));
			gMem.FillRectangle(&bgBrush, (INT)rc.left, (INT)rc.top, (INT)rc.right, (INT)rc.bottom);
		} break;
		case BackgroundStyle::LightGrey:
		{
			Gdiplus::SolidBrush bgBrush(Gdiplus::Color(230, 230, 230));
			gMem.FillRectangle(&bgBrush, (INT)rc.left, (INT)rc.top, (INT)rc.right, (INT)rc.bottom);
		} break;
		case BackgroundStyle::Black:
		{
			Gdiplus::SolidBrush bgBrush(Gdiplus::Color(0, 0, 0));
			gMem.FillRectangle(&bgBrush, (INT)rc.left, (INT)rc.top, (INT)rc.right, (INT)rc.bottom);
		} break;
		case BackgroundStyle::Chessboard:
		{
			Gdiplus::SolidBrush bgWhite(Gdiplus::Color(255, 255, 255));
			Gdiplus::SolidBrush bgGrey(Gdiplus::Color(192, 192, 192));
			Gdiplus::SolidBrush* currentBg;

			int numXCells = (rc.right - rc.left) / m_cellSize.cx + 1;
			int numYCells = (rc.bottom - rc.top) / m_cellSize.cy + 1;

			for (int x = 0; x < numXCells; ++x)
			{
				for (int y = 0; y < numYCells; ++y)
				{
					if ((x + y) % 2 > 0)
						currentBg = &bgWhite;
					else currentBg = &bgGrey;

					gMem.FillRectangle(currentBg
						, x * m_cellSize.cx
						, y * m_cellSize.cy
						, m_cellSize.cx
						, m_cellSize.cy);
				}
			}
		} break;
		}

		if (m_image != NULL)
		{
			gMem.DrawImage(m_image, m_drawAt.cx, m_drawAt.cy);

			WCHAR txt[32];
			wsprintf(txt, L"%d x %dpx", m_image->GetWidth(), m_image->GetHeight());

			Gdiplus::Font font(L"Arial", 9);
			Gdiplus::PointF position(5.0f, 5.0f);
			Gdiplus::SolidBrush brush(Gdiplus::Color(30, 30, 30));
			gMem.DrawString(txt, -1, &font, position, &brush);
		}

		Gdiplus::Graphics g(dc);
		g.DrawImage(&memBmp, 0, 0);
	}

private:
	void AdjustImageOrigin(SIZE areaSize, SIZE imageSize, SIZE& result)
	{
		// try center X
		if (imageSize.cx < areaSize.cx)
		{
			result.cx = (areaSize.cx / 2) - (imageSize.cx / 2);
		}

		// try center y
		if (imageSize.cy < areaSize.cy)
		{
			result.cy = (areaSize.cy / 2) - (imageSize.cy / 2);
		}
	}
};