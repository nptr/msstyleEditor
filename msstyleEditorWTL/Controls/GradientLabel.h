#ifndef __GRADIENTLABEL_H__
#define __GRADIENTLABEL_H__

#pragma once

/////////////////////////////////////////////////////////////////////////////
// GradientLabel - Gradient label control implementation
//
// Written by Bjarke Viksoe (bjarke@viksoe.dk)
// Copyright (c) 2000 Bjarke Viksoe.
//
// This code may be used in compiled form in any way you desire. This
// file may be redistributed by any means PROVIDING it is 
// not sold for profit without the authors written consent, and 
// providing that this notice and the authors name is included. 
//
// This file is provided "as is" with no expressed or implied warranty.
// The author accepts no liability if it causes any damage to you or your
// computer whatsoever. It's free, so don't hassle me about it.
//
// Beware of bugs.
//

#ifndef __cplusplus
  #error ATL requires C++ compilation (use a .cpp suffix)
#endif

#ifndef __ATLAPP_H__
  #error GradientLabel.h requires atlapp.h to be included first
#endif

#ifndef __ATLCTRLS_H__
  #error GradientLabel.h requires atlctrls.h to be included first
#endif

#if (_WIN32_IE < 0x0400)
  #error GradientLabel.h requires _WIN32_IE >= 0x0400
#endif


template< class T, class TBase = CStatic, class TWinTraits = CControlWinTraits >
class ATL_NO_VTABLE CGradientLabelImpl : public CWindowImpl< T, TBase, TWinTraits >
{
public:
   typedef CGradientLabelImpl< T, TBase, TWinTraits > thisClass;

   CToolTipCtrl m_tip;
   CFont m_font;
   COLORREF m_clrText;
   bool m_bActive;
   bool m_bHorizontal;

   CGradientLabelImpl() : 
      m_clrText(CLR_INVALID), 
      m_bActive(true), 
      m_bHorizontal(false)
   { 
   }

   // Operations

   BOOL SubclassWindow(HWND hWnd)
   {
      ATLASSERT(m_hWnd==NULL);
      ATLASSERT(::IsWindow(hWnd));
      BOOL bRet = CWindowImpl< T, TBase, TWinTraits >::SubclassWindow(hWnd);
      if( bRet ) _Init();
      return bRet;
   }

   void SetTextColor(COLORREF clrText)
   {
      m_clrText = clrText;
      if( IsWindow() ) Invalidate();
   }
   void SetHorizontalFill(BOOL bHoriz)
   {
      m_bHorizontal = bHoriz == TRUE;
      if( IsWindow() ) Invalidate();
   }
   void SetActive(BOOL bActive = TRUE)
   {
      m_bActive = bActive == TRUE;
      if( IsWindow() ) Invalidate();
   }

   // Implementation

   void _Init()
   {
      ATLASSERT(::IsWindow(m_hWnd));

      // Check if we should paint a label
      TCHAR lpszBuffer[10] = { 0 };
      if( ::GetClassName(m_hWnd, lpszBuffer, 9) ) {
         if( ::lstrcmpi(lpszBuffer, TBase::GetWndClassName()) == 0 ) {
            ModifyStyle(0, SS_NOTIFY);  // We need this
            DWORD dwStyle = GetStyle() & 0x000000FF;
            if( dwStyle == SS_ICON || dwStyle == SS_BLACKRECT || dwStyle == SS_GRAYRECT ||
                dwStyle == SS_WHITERECT || dwStyle == SS_BLACKFRAME || dwStyle == SS_GRAYFRAME ||
                dwStyle == SS_WHITEFRAME || dwStyle == SS_OWNERDRAW ||
                dwStyle == SS_BITMAP || dwStyle == SS_ENHMETAFILE )
            ATLASSERT("Invalid static style for gradient label"==NULL);
         }
      }

      // Set font
      CWindow wnd = GetParent();
      CFontHandle font = wnd.GetFont();
      if( !font.IsNull() ) {
         LOGFONT lf;
         font.GetLogFont(&lf);
         lf.lfWeight = FW_BOLD;
         m_font.CreateFontIndirect(&lf);
         SetFont(m_font);
      }

      // Set label (defaults to window text)
      int nLen = GetWindowTextLength();
      if( nLen > 0 ) {
         LPTSTR lpszText = (LPTSTR) _alloca((nLen + 1) * sizeof(TCHAR));
         if( GetWindowText(lpszText, nLen + 1) ) {
           // create a tool tip
           m_tip.Create(m_hWnd);
           ATLASSERT(m_tip.IsWindow());
           m_tip.Activate(TRUE);
           m_tip.AddTool(m_hWnd, lpszText);
         }
      }
   }

   // Message map and handlers

   BEGIN_MSG_MAP( thisClass )
      MESSAGE_HANDLER(WM_CREATE, OnCreate)
      MESSAGE_RANGE_HANDLER(WM_MOUSEFIRST, WM_MOUSELAST, OnMouseMessage)
      MESSAGE_HANDLER(WM_ERASEBKGND, OnEraseBackground)
      MESSAGE_HANDLER(WM_PAINT, OnPaint)
      MESSAGE_HANDLER(WM_PRINTCLIENT, OnPaint)
   END_MSG_MAP()

   LRESULT OnCreate(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
   {
      _Init();
      return 0;
   }
   LRESULT OnEraseBackground(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
   {
      return 1; // We're painting it all
   }
   LRESULT OnPaint(UINT /*uMsg*/, WPARAM wParam, LPARAM /*lParam*/, BOOL& /*bHandled*/)
   {
      T* pT = static_cast<T*>(this);
      if( wParam != NULL ) {
         pT->DoPaint( (HDC) wParam );
      }
      else {
         CPaintDC dc(m_hWnd);
         pT->DoPaint( (HDC) dc );
      }
      return 0;
   }
   LRESULT OnMouseMessage(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
   {
      MSG msg = { m_hWnd, uMsg, wParam, lParam };
      if( m_tip.IsWindow() ) m_tip.RelayEvent(&msg);
      bHandled = FALSE;
      return 0;
   }

   // Paint methods

   void DoGradientFill(CDCHandle& dc, const RECT& rc)
   {
#ifndef SPI_GETGRADIENTCAPTIONS
      // If you're missing the Win2K headers
      const UINT SPI_GETGRADIENTCAPTIONS = 0x1008;
      const UINT COLOR_GRADIENTACTIVECAPTION = 27;
      const UINT COLOR_GRADIENTINACTIVECAPTION = 28;
#endif

      // Draw the caption background
      COLORREF clrCptn = m_bActive ?
        ::GetSysColor(COLOR_ACTIVECAPTION) :
        ::GetSysColor(COLOR_INACTIVECAPTION);
      HBRUSH brCptn = m_bActive ?
        ::GetSysColorBrush(COLOR_ACTIVECAPTION) :
        ::GetSysColorBrush(COLOR_INACTIVECAPTION);

      // Gradient from left to right or from bottom to top
      COLORREF clrCptnRight = ::GetSysColor(COLOR_BTNFACE);
      BOOL bSysGradient = FALSE;
      ::SystemParametersInfo(SPI_GETGRADIENTCAPTIONS, 0, &bSysGradient, 0);
      if( bSysGradient ) {
         // Get second gradient color (the right end)
         clrCptnRight = m_bActive ?
           ::GetSysColor(COLOR_GRADIENTACTIVECAPTION) :
           ::GetSysColor(COLOR_GRADIENTINACTIVECAPTION);
      }

      // This will make 2^6 = 64 fountain steps
      int nShift = 6;
      int nSteps = 1 << nShift;
      for( int i = 0; i < nSteps; i++ ) {
         // Do a little alpha blending
         BYTE bR = (BYTE) ((GetRValue(clrCptn) * (nSteps - i) +
                    GetRValue(clrCptnRight) * i) >> nShift);
         BYTE bG = (BYTE) ((GetGValue(clrCptn) * (nSteps - i) +
                    GetGValue(clrCptnRight) * i) >> nShift);
         BYTE bB = (BYTE) ((GetBValue(clrCptn) * (nSteps - i) +
                    GetBValue(clrCptnRight) * i) >> nShift);

         CBrush br;
         br.CreateSolidBrush( RGB(bR,bG,bB) );
         // then paint with the resulting color
         RECT r2 = rc;
         if( m_bHorizontal ) {
            r2.bottom = rc.bottom - ((i * (rc.bottom - rc.top)) >> nShift);
            r2.top = rc.bottom - (((i + 1) * (rc.bottom - rc.top)) >> nShift);
            if( (r2.bottom - r2.top) > 0 ) dc.FillRect(&r2, br);
         }
         else {
            r2.left = rc.left + ((i * (rc.right - rc.left)) >> nShift);
            r2.right = rc.left + (((i + 1) * (rc.right - rc.left)) >> nShift);
            if( (r2.right - r2.left) > 0 ) dc.FillRect(&r2, br);
         }
      }
   }

   void DoPaint(CDCHandle dc)
   {
      RECT rcClient;
      GetClientRect(&rcClient);

      DoGradientFill(dc, rcClient);

      dc.SetBkMode(TRANSPARENT);
      dc.SetTextColor(m_clrText == CLR_INVALID ? ::GetSysColor(COLOR_CAPTIONTEXT) : m_clrText);
      HFONT hOldFont = dc.SelectFont(GetFont());

      int nLen = GetWindowTextLength();
      if(nLen > 0) {
         LPTSTR lpszText = (LPTSTR) _alloca((nLen + 1) * sizeof(TCHAR));
         if( GetWindowText(lpszText, nLen + 1) ) {
            DWORD dwStyle = GetStyle();
            int nDrawStyle = DT_LEFT;
            if( dwStyle & SS_CENTER ) nDrawStyle = DT_CENTER;
            else if( dwStyle & SS_RIGHT ) nDrawStyle = DT_RIGHT;
            if( dwStyle & SS_SIMPLE ) nDrawStyle = DT_VCENTER | DT_SINGLELINE;
            dc.DrawText(lpszText, -1, &rcClient, nDrawStyle | DT_WORDBREAK);
         }
      }

      dc.SelectFont(hOldFont);
   }
};


class CGradientLabelCtrl : public CGradientLabelImpl<CGradientLabelCtrl>
{
public:
  DECLARE_WND_CLASS(_T("WTL_GradientLabel"))
};


#endif //__GRADIENTLABEL_H__
