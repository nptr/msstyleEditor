#pragma once

#include <atlapp.h>
#include <atlscrl.h>
#include <atlwin.h>
#include <gdiplus.h>
#include <gdiplusgraphics.h>

class CImageCtrl : public CScrollWindowImpl<CImageCtrl>
{
public:
	DECLARE_WND_CLASS_EX(NULL, 0, -1)

	Gdiplus::Image* m_image;
	SIZE m_area;
	SIZE m_drawAt;

	SIZE m_size;

	CImageCtrl()
	{
		m_image = NULL;
		m_size.cx = m_size.cy = 1;
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
		}
		else
		{
			m_size.cx = m_size.cy = 100;
		}

		this->Invalidate();
		//SetScrollOffset(0, 0, FALSE);
		//SetScrollSize(m_size);
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
		m_area.cx = x;
		m_area.cy = y;

		if (m_image == NULL)
		{
			return;
		}

		bool couldFit = false;

		// try center X
		if (m_image->GetWidth() < m_area.cx)
		{
			m_drawAt.cx = (m_area.cx / 2) - (m_image->GetWidth() / 2);
			couldFit = true;
		}

		// try center y
		if (m_image->GetWidth() < m_area.cx)
		{
			m_drawAt.cy = (m_area.cy / 2) - (m_image->GetHeight() / 2);
			couldFit = true;
		}

		this->Invalidate();
	}

	void DoPaint(CDCHandle dc)
	{
		RECT rc;
		GetClientRect(&rc);
		Gdiplus::Bitmap memBmp(rc.right, rc.bottom);
		Gdiplus::Graphics gMem(&memBmp);

		COLORREF bgColor = GetSysColor(COLOR_WINDOW);
		Gdiplus::SolidBrush bgBrush(Gdiplus::Color(
			GetRValue(bgColor)
			, GetGValue(bgColor)
			, GetBValue(bgColor)));
		gMem.FillRectangle(&bgBrush, (INT)rc.left, (INT)rc.top, (INT)rc.right, (INT)rc.bottom);

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
};