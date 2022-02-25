using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
   internal class NativeMethods
   {
      // Fields
      public const int AC_SRC_ALPHA = 1;
      public const int AC_SRC_OVER = 0;
      public const int AW_ACTIVATE = 0x20000;
      public const int AW_BLEND = 0x80000;
      public const int AW_CENTER = 0x10;
      public const int AW_HIDE = 0x10000;
      public const int AW_HOR_NEGATIVE = 2;
      public const int AW_HOR_POSITIVE = 1;
      public const int AW_SLIDE = 0x40000;
      public const int AW_VER_NEGATIVE = 8;
      public const int AW_VER_POSITIVE = 4;
      public const int BDR_RAISEDINNER = 4;
      public const int BDR_RAISEDOUTER = 1;
      public const int BDR_SUNKENINNER = 8;
      public const int BDR_SUNKENOUTER = 2;
      public const int BF_BOTTOM = 8;
      public const int BF_LEFT = 1;
      public const int BF_RECT = 15;
      public const int BF_RIGHT = 4;
      public const int BF_TOP = 2;
      public const int BLACKNESS = 0x42;
      public const int BP_CHECKBOX = 3;
      public const int BP_GROEPBOX = 4;
      public const int BP_PUSHBUTTON = 1;
      public const int BP_RADIOBUTTON = 2;
      public const int BP_USERBUTTON = 5;
      public const int BS_DIBPATTERN = 5;
      public const int BS_DIBPATTERN8X8 = 8;
      public const int BS_DIBPATTERNPT = 6;
      public const int BS_HATCHED = 2;
      public const int BS_HOLLOW = 1;
      public const int BS_INDEXED = 4;
      public const int BS_MONOPATTERN = 9;
      public const int BS_NULL = 1;
      public const int BS_PATTERN = 3;
      public const int BS_PATTERN8X8 = 7;
      public const int BS_SOLID = 0;
      public const int CBS_DISABLED = 4;
      public const int CBS_HOT = 2;
      public const int CBS_INACTIVE = 5;
      public const int CBS_NORMAL = 1;
      public const int CBS_PUSHED = 3;
      public const int CBXS_DISABLED = 4;
      public const int CBXS_FOCUSED = 5;
      public const int CBXS_HOT = 2;
      public const int CBXS_NORMAL = 1;
      public const int CBXS_PRESSED = 3;
      public const int COLOR_ACTIVECAPTION = 2;
      public const int COLOR_GRADIENTACTIVECAPTION = 0x1b;
      public const int COLOR_GRADIENTINACTIVECAPTION = 0x1c;
      public const int COLOR_INACTIVECAPTION = 3;
      public const int CP_DROPDOWNBUTTON = 1;
      public const int CS_ACTIVE = 1;
      public const int CS_INACTIVE = 2;
      public const int DC_ACTIVE = 1;
      public const int DC_BUTTONS = 0x1000;
      public const int DC_GRADIENT = 0x20;
      public const int DC_ICON = 4;
      public const int DC_INBUTTON = 0x10;
      public const int DC_SMALLCAP = 2;
      public const int DC_TEXT = 8;
      public const int DCX_EXCLUDERGN = 0x40;
      public const int DCX_INTERSECTRGN = 0x80;
      public const int DCX_WINDOW = 1;
      public const int DFC_BUTTON = 4;
      public const int DFC_CAPTION = 1;
      public const int DFC_MENU = 2;
      public const int DFC_SCROLL = 3;
      public const int DFCS_CAPTIONCLOSE = 0;
      public const int DFCS_CAPTIONHELP = 4;
      public const int DFCS_CAPTIONMAX = 2;
      public const int DFCS_CAPTIONMIN = 1;
      public const int DFCS_CAPTIONRESTORE = 3;
      public const int DFCS_CHECKED = 0x400;
      public const int DFCS_INACTIVE = 0x100;
      public const int DFCS_PUSHED = 0x200;
      public const int DNS_DISABLED = 4;
      public const int DNS_HOT = 2;
      public const int DNS_NORMAL = 1;
      public const int DNS_PRESSED = 3;
      public const int DSTINVERT = 0x550009;
      public const int DT_BOTTOM = 8;
      public const int DT_CALCRECT = 0x400;
      public const int DT_CENTER = 1;
      public const int DT_END_ELLIPSIS = 0x8000;
      public const int DT_EXPANDTABS = 0x40;
      public const int DT_HIDEPREFIX = 0x100000;
      public const int DT_LEFT = 0;
      public const int DT_NOCLIP = 0x100;
      public const int DT_NOPREFIX = 0x800;
      public const int DT_PATH_ELLIPSIS = 0x4000;
      public const int DT_RIGHT = 2;
      public const int DT_SINGLELINE = 0x20;
      public const int DT_TOP = 0;
      public const int DT_VCENTER = 4;
      public const int DT_WORD_ELLIPSIS = 0x40000;
      public const int DT_WORDBREAK = 0x10;
      public const int DTBG_CLIPRECT = 1;
      public const int DTBG_DRAWSOLID = 2;
      public const int DTBG_MIRRORDC = 0x20;
      public const int DTBG_OMITBORDER = 4;
      public const int DTBG_OMITCONTENT = 8;
      public const int EM_CANUNDO = 0xc6;
      public const int EM_CHARFROMPOS = 0xd7;
      public const int EM_EMPTYUNDOBUFFER = 0xcd;
      public const int EM_FMTLINES = 200;
      public const int EM_GETFIRSTVISIBLELINE = 0xce;
      public const int EM_GETHANDLE = 0xbd;
      public const int EM_GETLIMITTEXT = 0xd5;
      public const int EM_GETLINE = 0xc4;
      public const int EM_GETLINECOUNT = 0xba;
      public const int EM_GETMARGINS = 0xd4;
      public const int EM_GETMODIFY = 0xb8;
      public const int EM_GETPASSWORDCHAR = 210;
      public const int EM_GETRECT = 0xb2;
      public const int EM_GETSEL = 0xb0;
      public const int EM_GETTHUMB = 190;
      public const int EM_GETWORDBREAKPROC = 0xd1;
      public const int EM_LIMITTEXT = 0xc5;
      public const int EM_LINEFROMCHAR = 0xc9;
      public const int EM_LINEINDEX = 0xbb;
      public const int EM_LINELENGTH = 0xc1;
      public const int EM_LINESCROLL = 0xb6;
      public const int EM_POSFROMCHAR = 0xd6;
      public const int EM_REPLACESEL = 0xc2;
      public const int EM_SCROLL = 0xb5;
      public const int EM_SCROLLCARET = 0xb7;
      public const int EM_SETHANDLE = 0xbc;
      public const int EM_SETLIMITTEXT = 0xc5;
      public const int EM_SETMARGINS = 0xd3;
      public const int EM_SETMODIFY = 0xb9;
      public const int EM_SETPASSWORDCHAR = 0xcc;
      public const int EM_SETREADONLY = 0xcf;
      public const int EM_SETRECT = 0xb3;
      public const int EM_SETRECTNP = 180;
      public const int EM_SETSEL = 0xb1;
      public const int EM_SETTABSTOPS = 0xcb;
      public const int EM_SETWORDBREAKPROC = 0xd0;
      public const int EM_UNDO = 0xc7;
      public const int ES_AUTOHSCROLL = 0x80;
      public const int ES_AUTOVSCROLL = 0x40;
      public const int ES_CENTER = 1;
      public const int ES_LEFT = 0;
      public const int ES_LOWERCASE = 0x10;
      public const int ES_MULTILINE = 4;
      public const int ES_NOHIDESEL = 0x100;
      public const int ES_NUMBER = 0x2000;
      public const int ES_OEMCONVERT = 0x400;
      public const int ES_PASSWORD = 0x20;
      public const int ES_READONLY = 0x800;
      public const int ES_RIGHT = 2;
      public const int ES_UPPERCASE = 8;
      public const int ES_WANTRETURN = 0x1000;
      public const int FS_ACTIVE = 1;
      public const int FS_INACTIVE = 2;
      public const int GA_PARENT = 1;
      public const int GA_ROOT = 2;
      public const int GA_ROOTOWNER = 3;
      public const int GDC_LOGPIXELSX = 0x58;
      public const int GDC_LOGPIXELSY = 90;
      public const int GDI_PATINVERT = 0x5a0049;
      public const int GDI_SRCCOPY = 0xcc0020;
      public const int GW_CHILD = 5;
      public const int GW_OWNER = 4;
      public const int GWL_EXSTYLE = -20;
      public const int GWL_HWNDPARENT = -8;
      public const int GWL_STYLE = -16;
      public const int HC_ACTION = 0;
      public const int HC_NOREMOVE = 3;
      public const uint HOVER_DEFAULT = uint.MaxValue;
      public const int HTBORDER = 0x12;
      public const int HTBOTTOM = 15;
      public const int HTBOTTOMLEFT = 0x10;
      public const int HTBOTTOMRIGHT = 0x11;
      public const int HTCAPTION = 2;
      public const int HTCLIENT = 1;
      public const int HTCLOSE = 20;
      public const int HTLEFT = 10;
      public const int HTMAXBUTTON = 9;
      public const int HTMINBUTTON = 8;
      public const int HTNOWHERE = 0;
      public const int HTRIGHT = 11;
      public const int HTSYSMENU = 3;
      public const int HTTOP = 12;
      public const int HTTOPLEFT = 13;
      public const int HTTOPRIGHT = 14;
      public const int HTTRANSPARENT = -1;
      public const int HWND_BOTTOM = 1;
      public const int HWND_MESSAGE = -3;
      public const int HWND_NOTOPMOST = -2;
      public const int HWND_TOP = 0;
      public const int HWND_TOPMOST = -1;
      public const int KF_UP = 0x8000;
      public const int LAYOUT_BITMAPORIENTATIONPRESERVED = 8;
      public const int LAYOUT_RTL = 1;
      public const int LWA_ALPHA = 2;
      public const int LWA_COLORKEY = 1;
      public const int MA_ACTIVATE = 1;
      public const int MA_ACTIVATEANDEAT = 2;
      public const int MA_NOACTIVATE = 3;
      public const int MA_NOACTIVATEANDEAT = 4;
      public const int MEM_COMMIT = 0x1000;
      public const int MEM_RELEASE = 0x8000;
      public const int MERGECOPY = 0xc000ca;
      public const int MERGEPAINT = 0xbb0226;
      public const int MK_MBUTTON = 0x10;
      public const int MP_CHEVRON = 5;
      public const int MP_MENUBARDROPDOWN = 4;
      public const int MP_MENUBARITEM = 3;
      public const int MP_MENUDROPDOWN = 2;
      public const int MP_MENUITEM = 1;
      public const int MP_SEPARATOR = 6;
      public const int NOTSRCCOPY = 0x330008;
      public const int NOTSRCERASE = 0x1100a6;
      public const uint OBJID_CLIENT = 0xfffffffc;
      public const uint OBJID_HSCROLL = 0xfffffffa;
      public const uint OBJID_VSCROLL = 0xfffffffb;
      public const int OPAQUE = 2;
      public const int PAGE_READWRITE = 4;
      public const int PATCOPY = 0xf00021;
      public const int PATINVERT = 0x5a0049;
      public const int PATPAINT = 0xfb0a09;
      public const int PBS_DEFAULTED = 5;
      public const int PBS_DISABLED = 4;
      public const int PBS_HOT = 2;
      public const int PBS_NORMAL = 1;
      public const int PBS_PUSHED = 3;
      public const int PM_NOREMOVE = 0;
      public const int PM_NOYIELD = 2;
      public const int PM_REMOVE = 1;
      public const int PRF_CHECKVISIBLE = 1;
      public const int PRF_CHILDREN = 0x10;
      public const int PRF_CLIENT = 4;
      public const int PRF_ERASEBKGND = 8;
      public const int PRF_NONCLIENT = 2;
      public const int PRF_OWNED = 0x20;
      public const int PROCESS_ALL_ACCESS = 0x1f0fff;
      public const int RDW_ALLCHILDREN = 0x80;
      public const int RDW_ERASE = 4;
      public const int RDW_ERASENOW = 0x200;
      public const int RDW_FRAME = 0x400;
      public const int RDW_INTERNALPAINT = 2;
      public const int RDW_INVALIDATE = 1;
      public const int RDW_NOCHILDREN = 0x40;
      public const int RDW_NOERASE = 0x20;
      public const int RDW_NOFRAME = 0x800;
      public const int RDW_NOINTERNALPAINT = 0x10;
      public const int RDW_UPDATENOW = 0x100;
      public const int RDW_VALIDATE = 8;
      public const int RGN_AND = 1;
      public const int RGN_COPY = 5;
      public const int RGN_DIFF = 4;
      public const int RGN_MAX = 5;
      public const int RGN_MIN = 1;
      public const int RGN_OR = 2;
      public const int RGN_XOR = 3;
      public const int SB_BOTH = 3;
      public const int SB_BOTTOM = 7;
      public const int SB_CTL = 2;
      public const int SB_ENDSCROLL = 8;
      public const int SB_HORZ = 0;
      public const int SB_LEFT = 6;
      public const int SB_LINEDOWN = 1;
      public const int SB_LINELEFT = 0;
      public const int SB_LINERIGHT = 1;
      public const int SB_LINEUP = 0;
      public const int SB_PAGEDOWN = 3;
      public const int SB_PAGELEFT = 2;
      public const int SB_PAGERIGHT = 3;
      public const int SB_PAGEUP = 2;
      public const int SB_RIGHT = 7;
      public const int SB_THUMBPOSITION = 4;
      public const int SB_THUMBTRACK = 5;
      public const int SB_TOP = 6;
      public const int SB_VERT = 1;
      public const int SBM_GETSCROLLINFO = 0xea;
      public const int SBM_SETSCROLLINFO = 0xe9;
      public const int SC_HOTKEY = 0xf150;
      public const int SC_KEYMENU = 0xf100;
      public const int SC_MAXIMIZE = 0xf030;
      public const int SC_MINIMIZE = 0xf020;
      public const int SC_MOUSEMENU = 0xf090;
      public const int SC_MOVE = 0xf010;
      public const int SC_RESTORE = 0xf120;
      public const uint SHACF_AUTOAPPEND_FORCE_OFF = 0x80000000;
      public const uint SHACF_AUTOAPPEND_FORCE_ON = 0x40000000;
      public const uint SHACF_AUTOSUGGEST_FORCE_OFF = 0x20000000;
      public const uint SHACF_AUTOSUGGEST_FORCE_ON = 0x10000000;
      public const uint SHACF_DEFAULT = 0;
      public const uint SHACF_FILESYS_ONLY = 0x10;
      public const uint SHACF_FILESYSTEM = 1;
      public const uint SHACF_URLALL = 6;
      public const uint SHACF_URLHISTORY = 2;
      public const uint SHACF_URLMRU = 4;
      public const uint SHACF_USETAB = 8;
      public const int SIF_ALL = 0x17;
      public const int SIF_DISABLENOSCROLL = 8;
      public const int SIF_PAGE = 2;
      public const int SIF_POS = 4;
      public const int SIF_RANGE = 1;
      public const int SIF_TRACKPOS = 0x10;
      public const int SIZE_MAXHIDE = 4;
      public const int SIZE_MAXIMIZED = 2;
      public const int SIZE_MAXSHOW = 3;
      public const int SIZE_MINIMIZED = 1;
      public const int SIZE_RESTORED = 0;
      public const int SM_CXSMSIZE = 0x34;
      public const int SM_CYSMSIZE = 0x35;
      public const int SPI_GETDROPSHADOW = 0x1024;
      public const int SPI_GETKEYBOARDCUES = 0x100a;
      public const int SPI_GETMENUANIMATION = 0x1002;
      public const int SPI_GETMENUFADE = 0x1012;
      public const int SPI_GETMENUSHOWDELAY = 0x6a;
      public const int SPI_GETNONCLIENTMETRICS = 0x29;
      public const int SPI_GETTOOLTIPANIMATION = 0x1016;
      public const int SPNP_DOWN = 2;
      public const int SPNP_UP = 1;
      public const int SRCAND = 0x8800c6;
      public const int SRCCOPY = 0xcc0020;
      public const int SRCERASE = 0x440328;
      public const int SRCINVERT = 0x660046;
      public const int SRCPAINT = 0xee0086;
      public const int STATE_SYSTEM_INVISIBLE = 0x8000;
      public const int SW_ERASE = 4;
      public const int SW_FORCEMINIMIZE = 11;
      public const int SW_HIDE = 0;
      public const int SW_INVALIDATE = 2;
      public const int SW_MAX = 11;
      public const int SW_MAXIMIZE = 3;
      public const int SW_MINIMIZE = 6;
      public const int SW_NORMAL = 1;
      public const int SW_RESTORE = 9;
      public const int SW_SCROLLCHILDREN = 1;
      public const int SW_SHOW = 5;
      public const int SW_SHOWDEFAULT = 10;
      public const int SW_SHOWMAXIMIZED = 3;
      public const int SW_SHOWMINIMIZED = 2;
      public const int SW_SHOWMINNOACTIVE = 7;
      public const int SW_SHOWNA = 8;
      public const int SW_SHOWNOACTIVATE = 4;
      public const int SW_SHOWNORMAL = 1;
      public const int SWP_FRAMECHANGED = 0x20;
      public const int SWP_HIDEWINDOW = 0x80;
      public const int SWP_NOACTIVATE = 0x10;
      public const int SWP_NOCOPYBITS = 0x100;
      public const int SWP_NOMOVE = 2;
      public const int SWP_NOOWNERZORDER = 0x200;
      public const int SWP_NOREDRAW = 8;
      public const int SWP_NOSENDCHANGING = 0x400;
      public const int SWP_NOSIZE = 1;
      public const int SWP_NOZORDER = 4;
      public const int SWP_SHOWWINDOW = 0x40;
      public const int TABP_TABITEM = 1;
      public const int TABP_TOPTABITEM = 5;
      public const int TB_BUTTONCOUNT = 0x418;
      public const int TB_DELETEBUTTON = 0x416;
      public const int TB_GETBUTTON = 0x417;
      public const int TB_GETBUTTONINFOW = 0x43f;
      public const int TB_GETITEMRECT = 0x41d;
      public const int TBIF_BYINDEX = -2147483648;
      public const int TBIF_COMMAND = 0x20;
      public const int TBIF_IMAGE = 1;
      public const int TBIF_LPARAM = 0x10;
      public const int TBIF_SIZE = 0x40;
      public const int TBIF_STATE = 4;
      public const int TBIF_STYLE = 8;
      public const int TBIF_TEXT = 2;
      public const int TIS_DISABLED = 4;
      public const int TIS_HOT = 2;
      public const int TIS_NORMAL = 1;
      public const int TIS_SELECTED = 3;
      public const uint TME_HOVER = 1;
      public const uint TME_LEAVE = 2;
      public const uint TME_NONCLIENT = 0x10;
      public const int TMT_ACTIVECAPTION = 0x643;
      public const int TMT_BOOL = 0xcb;
      public const int TMT_BORDERSIZE = 0x963;
      public const int TMT_BTNFACE = 0x650;
      public const int TMT_BTNHIGHLIGHT = 0x655;
      public const int TMT_BTNSHADOW = 0x651;
      public const int TMT_CAPTIONFONT = 0x321;
      public const int TMT_CAPTIONMARGINS = 0xe13;
      public const int TMT_COLOR = 0xcc;
      public const int TMT_CONTENTMARGINS = 0xe12;
      public const int TMT_FILENAME = 0xce;
      public const int TMT_FILLCOLOR = 0xeda;
      public const int TMT_FONT = 210;
      public const int TMT_GRADIENTCOLOR1 = 0xee2;
      public const int TMT_GRADIENTCOLOR2 = 0xee3;
      public const int TMT_GRADIENTCOLOR3 = 0xee4;
      public const int TMT_GRADIENTCOLOR4 = 0xee5;
      public const int TMT_GRADIENTCOLOR5 = 0xee6;
      public const int TMT_HEIGHT = 0x971;
      public const int TMT_HOTTRACKING = 0x65b;
      public const int TMT_ICONTITLEFONT = 0x326;
      public const int TMT_INT = 0xca;
      public const int TMT_INTLIST = 0xd3;
      public const int TMT_MARGINS = 0xcd;
      public const int TMT_MENUFONT = 0x323;
      public const int TMT_MSGBOXFONT = 0x325;
      public const int TMT_POSITION = 0xd0;
      public const int TMT_RECT = 0xd1;
      public const int TMT_SIZE = 0xcf;
      public const int TMT_SIZINGMARGINS = 0xe11;
      public const int TMT_SMALLCAPTIONFONT = 0x322;
      public const int TMT_STATUSFONT = 0x324;
      public const int TMT_STRING = 0xc9;
      public const int TMT_WIDTH = 0x970;
      public const int TRANSPARENT = 1;
      public const int TTCS_HOT = 2;
      public const int TTCS_NORMAL = 1;
      public const int TTCS_PRESSED = 3;
      public const int TTP_CLOSE = 5;
      public const int ULW_ALPHA = 2;
      public const int ULW_COLORKEY = 1;
      public const int ULW_OPAQUE = 4;
      public const int UNS_DISABLED = 4;
      public const int UNS_HOT = 2;
      public const int UNS_NORMAL = 1;
      public const int UNS_PRESSED = 3;
      public const int WA_ACTIVE = 1;
      public const int WA_CLICKACTIVE = 2;
      public const int WA_INACTIVE = 0;
      public const int WH_CALLWNDPROC = 4;
      public const int WH_CBT = 5;
      public const int WH_GETMESSAGE = 3;
      public const int WH_JOURNALPLAYBACK = 1;
      public const int WH_JOURNALRECORD = 0;
      public const int WH_KEYBOARD = 2;
      public const int WH_KEYBOARD_LL = 13;
      public const int WH_MIN = -1;
      public const int WH_MOUSE = 7;
      public const int WH_MOUSE_LL = 14;
      public const int WH_MSGFILTER = -1;
      public const int WH_SYSMSGFILTER = 6;
      public const int WHITENESS = 0xff0062;
      public const int WM_ACTIVATE = 6;
      public const int WM_ACTIVATEAPP = 0x1c;
      public const int WM_CAPTURECHANGED = 0x215;
      public const int WM_CHAR = 0x102;
      public const int WM_CHILDACTIVATE = 0x22;
      public const int WM_CLOSE = 0x10;
      public const int WM_COMMAND = 0x111;
      public const int WM_CONTEXTMENU = 0x7b;
      public const int WM_CREATE = 1;
      public const int WM_CTLCOLOREDIT = 0x133;
      public const int WM_CTLCOLORSTATIC = 0x138;
      public const int WM_DEADCHAR = 0x103;
      public const int WM_DESTROY = 2;
      public const int WM_ENDSESSION = 0x16;
      public const int WM_ENTERSIZEMOVE = 0x231;
      public const int WM_ERASEBKGND = 20;
      public const int WM_EXITSIZEMOVE = 0x232;
      public const int WM_GETMINMAXINFO = 0x24;
      public const int WM_HSCROLL = 0x114;
      public const int WM_KEYDOWN = 0x100;
      public const int WM_KEYFIRST = 0x100;
      public const int WM_KEYUP = 0x101;
      public const int WM_KILLFOCUS = 8;
      public const int WM_LBUTTONDBLCLK = 0x203;
      public const int WM_LBUTTONDOWN = 0x201;
      public const int WM_LBUTTONUP = 0x202;
      public const int WM_MBUTTONDBLCLK = 0x209;
      public const int WM_MBUTTONDOWN = 0x207;
      public const int WM_MBUTTONUP = 520;
      public const int WM_MDIGETACTIVE = 0x229;
      public const int WM_MDIMAXIMIZE = 0x225;
      public const int WM_MDIREFRESHMENU = 0x234;
      public const int WM_MDIRESTORE = 0x223;
      public const int WM_MDISETMENU = 560;
      public const int WM_MOUSEACTIVATE = 0x21;
      public const int WM_MOUSEMOVE = 0x200;
      public const int WM_MOUSEWHEEL = 0x20a;
      public const int WM_NCACTIVATE = 0x86;
      public const int WM_NCALLMOUSEBUTTONACTIONS = 160;
      public const int WM_NCCALCSIZE = 0x83;
      public const int WM_NCHITTEST = 0x84;
      public const int WM_NCLBUTTONDBLCLK = 0xa3;
      public const int WM_NCLBUTTONDOWN = 0xa1;
      public const int WM_NCLBUTTONUP = 0xa2;
      public const int WM_NCMBUTTONDBLCLK = 0xa9;
      public const int WM_NCMBUTTONDOWN = 0xa7;
      public const int WM_NCMBUTTONUP = 0xa8;
      public const int WM_NCMOUSEHOVER = 0x2a0;
      public const int WM_NCMOUSELEAVE = 0x2a2;
      public const int WM_NCMOUSEMOVE = 160;
      public const int WM_NCPAINT = 0x85;
      public const int WM_NCRBUTTONDBLCLK = 0xa6;
      public const int WM_NCRBUTTONDOWN = 0xa4;
      public const int WM_NCRBUTTONUP = 0xa5;
      public const int WM_NCXBUTTONDBLCLK = 0xad;
      public const int WM_NCXBUTTONDOWN = 0xab;
      public const int WM_NCXBUTTONUP = 0xac;
      public const int WM_PAINT = 15;
      public const int WM_PRINT = 0x317;
      public const int WM_PRINTCLIENT = 0x318;
      public const int WM_QUERYENDSESSION = 0x11;
      public const int WM_RBUTTONDBLCLK = 0x206;
      public const int WM_RBUTTONDOWN = 0x204;
      public const int WM_RBUTTONUP = 0x205;
      public const int WM_SETCURSOR = 0x20;
      public const int WM_SETFOCUS = 7;
      public const int WM_SETICON = 0x80;
      public const int WM_SETTEXT = 12;
      public const int WM_SHOWWINDOW = 0x18;
      public const int WM_SIZE = 5;
      public const int WM_SIZING = 0x214;
      public const int WM_SYSCHAR = 0x106;
      public const int WM_SYSCOLORCHANGE = 0x15;
      public const int WM_SYSCOMMAND = 0x112;
      public const int WM_SYSKEYDOWN = 260;
      public const int WM_SYSKEYUP = 0x105;
      public const int WM_THEMECHANGED = 0x31a;
      public const int WM_TIMER = 0x113;
      public const int WM_USER = 0x400;
      public const int WM_VSCROLL = 0x115;
      public const int WM_WINDOWPOSCHANGED = 0x47;
      public const int WM_WINDOWPOSCHANGING = 70;
      public const int WM_XBUTTONDBLCLK = 0x20d;
      public const int WM_XBUTTONDOWN = 0x20b;
      public const int WM_XBUTTONUP = 0x20c;
      public const int WMSZ_BOTTOMRIGHT = 8;
      public const int WP_CAPTION = 1;
      public const int WP_CLOSEBUTTON = 0x12;
      public const int WP_FRAMEBOTTOM = 9;
      public const int WP_FRAMELEFT = 7;
      public const int WP_FRAMERIGHT = 8;
      public const int WP_MAXCAPTION = 5;
      public const int WP_MDICLOSEBUTTON = 20;
      public const int WP_MDIMINBUTTON = 0x10;
      public const int WP_MDIRESTOREBUTTON = 0x16;
      public const int WP_MINBUTTON = 15;
      public const int WP_SMALLCAPTION = 2;
      public const int WP_SMALLCLOSEBUTTON = 0x13;
      public const int WP_SMALLFRAMEBOTTOM = 9;
      public const int WP_SMALLFRAMELEFT = 7;
      public const int WP_SMALLFRAMERIGHT = 8;
      public const int WP_SYSBUTTON = 13;
      public const int WS_BORDER = 0x800000;
      public const int WS_CAPTION = 0xc00000;
      public const int WS_CHILD = 0x40000000;
      public const int WS_CLIPCHILDREN = 0x2000000;
      public const int WS_CLIPSIBLINGS = 0x4000000;
      public const int WS_EX_APPWINDOW = 0x40000;
      public const int WS_EX_CLIENTEDGE = 0x200;
      public const int WS_EX_CONTROLPARENT = 0x10000;
      public const int WS_EX_LAYERED = 0x80000;
      public const int WS_EX_LAYOUTRTL = 0x400000;
      public const int WS_EX_LEFT = 0;
      public const int WS_EX_LEFTSCROLLBAR = 0x4000;
      public const int WS_EX_LTRREADING = 0;
      public const int WS_EX_NOACTIVATE = 0x8000000;
      public const int WS_EX_NOINHERITLAYOUT = 0x100000;
      public const int WS_EX_RIGHT = 0x1000;
      public const int WS_EX_RIGHTSCROLLBAR = 0;
      public const int WS_EX_RTLREADING = 0x2000;
      public const int WS_EX_TOOLWINDOW = 0x80;
      public const int WS_EX_TOPMOST = 8;
      public const int WS_EX_TRANSPARENT = 0x20;
      public const int WS_EX_WINDOWEDGE = 0x100;
      public const int WS_GROUP = 0x20000;
      public const int WS_HSCROLL = 0x100000;
      public const int WS_MAXIMIZEBOX = 0x10000;
      public const int WS_MINIMIZE = 0x20000000;
      public const int WS_MINIMIZEBOX = 0x20000;
      public const int WS_POPUP = -2147483648;
      public const int WS_POPUPWINDOW = -2138570752;
      public const int WS_SYSMENU = 0x80000;
      public const int WS_TABSTOP = 0x10000;
      public const int WS_THICKFRAME = 0x40000;
      public const int WS_VISIBLE = 0x10000000;
      public const int WS_VSCROLL = 0x200000;
      public const int WVR_ALIGNBOTTOM = 0x40;
      public const int WVR_ALIGNLEFT = 0x20;
      public const int WVR_ALIGNRIGHT = 0x80;
      public const int WVR_ALIGNTOP = 0x10;
      public const int WVR_HREDRAW = 0x100;
      public const int WVR_REDRAW = 0x300;
      public const int WVR_VALIDRECTS = 0x400;
      public const int WVR_VREDRAW = 0x200;

      // Methods
      private NativeMethods();
      [DllImport("user32.dll")]
      public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("Msimg32.dll", CharSet = CharSet.Auto)]
      public static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, BLENDFUNCTION blendFunction);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", SetLastError = true)]
      public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("gdi32.dll")]
      public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll")]
      public static extern bool BringWindowToTop(IntPtr hWnd);
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
      [DllImport("gdi32.dll", SetLastError = true)]
      public static extern IntPtr CancelDC(IntPtr hdc);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, POINT Point);
      [DllImport("User32.dll")]
      public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);
      [DllImport("kernel32.dll")]
      public static extern bool CloseHandle(IntPtr hObject);
      [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
      public static extern int CloseThemeData(IntPtr hTheme);
      [return: MarshalAs(UnmanagedType.Interface)]
      [DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
      public static extern object CoCreateInstance([In] ref Guid clsid, [MarshalAs(UnmanagedType.Interface)] object punkOuter, int context, [In] ref Guid iid);
      [DllImport("gdi32.dll")]
      public static extern int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, int fnCombineMode);
      [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
      public static extern IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, [MarshalAs(UnmanagedType.LPArray)] short[] lpvBits);
      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateBrushIndirect(ref LOGBRUSH lplb);
      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
      [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr CreateFontIndirect(ref LOGFONT lplf);
      [DllImport("gdi32", CharSet = CharSet.Auto)]
      public static extern IntPtr CreatePatternBrush(IntPtr hBitmap);
      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateRectRgnIndirect(ref RECT lprc);
      [DllImport("gdi32", CharSet = CharSet.Auto)]
      public static extern IntPtr CreateSolidBrush(int crColor);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("gdi32.dll")]
      public static extern bool DeleteDC(IntPtr hDC);
      [DllImport("gdi32.dll")]
      public static extern int DeleteObject(IntPtr hObject);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool DispatchMessage(ref MSG msg);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll")]
      public static extern bool DrawEdge(IntPtr hdc, ref RECT qrc, int edge, int grfFlags);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll")]
      public static extern bool DrawFrameControl(IntPtr hdc, ref RECT lprc, int uType, int uState);
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern int DrawText(IntPtr hdc, string lpString, int cbString, ref RECT lpRect, int uFormat);
      [DllImport("UxTheme.dll")]
      public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref RECT pClipRect);
      [DllImport("UxTheme.dll")]
      public static extern int DrawThemeBackgroundEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref DTBGOPTS pOptions);
      [DllImport("UxTheme.dll")]
      public static extern int DrawThemeEdge(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pDestRect, int uEdge, int uFlags, ref RECT pContentRect);
      [DllImport("user32.dll")]
      public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);
      [DllImport("User32.dll")]
      public static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadWindowsCallBack lpfn, IntPtr lParam);
      [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
      public static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
      [DllImport("user32.dll")]
      public static extern IntPtr FindWindow(StringBuilder lpszClass, StringBuilder lpszWindow);
      [DllImport("user32.dll")]
      public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, StringBuilder lpszClass, StringBuilder lpszWindow);
      [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
      internal static extern int GdipCreateBitmapFromScan0(int width, int height, int stride, int format, HandleRef scan0, out IntPtr bitmap);
      [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
      internal static extern int GdipCreateHBITMAPFromBitmap(HandleRef nativeBitmap, out IntPtr hbitmap, int argbBackground);
      [DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
      internal static extern int GdipGetDC(HandleRef graphics, out IntPtr hdc);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr GetActiveWindow();
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr GetAncestor(IntPtr hWnd, uint gaFlags);
      [DllImport("User32.dll")]
      public static extern IntPtr GetCapture();
      [DllImport("user32.dll")]
      public static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);
      [DllImport("gdi32.dll")]
      public static extern int GetClipBox(IntPtr hdc, ref RECT lprc);
      [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
      public static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int cchMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);
      [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
      public static extern int GetCurrentThreadId();
      [DllImport("user32.dll", SetLastError = true)]
      public static extern IntPtr GetDC(IntPtr hWnd);
      [DllImport("gdi32.dll", SetLastError = true)]
      public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
      [DllImport("user32.dll")]
      public static extern IntPtr GetFocus();
      [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      public static extern IntPtr GetForegroundWindow();
      [DllImport("gdi32.dll")]
      public static extern int GetGraphicsMode(IntPtr hdc);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool GetIconInfo(IntPtr hIcon, ref ICONINFO iconInfo);
      [DllImport("user32.dll")]
      private static extern long GetKeyboardLayoutName(StringBuilder pwszKLID);
      [DllImport("kernel32.dll")]
      public static extern int GetLastError();
      [DllImport("gdi32.dll")]
      public static extern uint GetLayout(IntPtr hdc);
      [DllImport("gdi32.dll")]
      public static extern int GetMapMode(IntPtr hdc);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool GetMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax);
      [DllImport("User32.dll")]
      public static extern IntPtr GetParent(IntPtr hWnd);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll")]
      public static extern bool GetScrollBarInfo(IntPtr hWnd, uint idObject, ref SCROLLBARINFO psbi);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll")]
      public static extern bool GetScrollInfo(IntPtr hWnd, int fnBar, [MarshalAs(UnmanagedType.Struct)] ref SCROLLINFO lpsi);
      [DllImport("user32.dll")]
      public static extern int GetSysColor(int nIndex);
      [DllImport("gdi32.dll")]
      public static extern uint GetTextAlign(IntPtr hdc);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
      public static extern bool GetTextExtentExPoint(IntPtr hdc, string lpszStr, int cchString, int nMaxExtent, ref short lpnFit, IntPtr alpDx, ref SIZE lpSize);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("gdi32.dll")]
      public static extern bool GetTextExtentPoint32(IntPtr hdc, string lpString, int cbString, ref SIZE lpSize);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("gdi32.dll")]
      public static extern bool GetTextMetrics(IntPtr hdc, IntPtr lptm);
      [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
      public static extern int GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, ref RECT prc, ref MARGINS pMargins);
      [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
      public static extern int GetThemeMetric(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, ref int piVal);
      [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
      public static extern int GetThemePartSize(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT prc, THEMESIZE eSize, ref SIZE psz);
      [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
      public static extern int GetThemeSysFont(IntPtr hTheme, int iIntID, out LOGFONT pFont);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool GetUpdateRect(IntPtr hWnd, ref RECT lpRect, bool erase);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowDC(IntPtr hWnd);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll")]
      public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll")]
      public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool HideCaret(IntPtr hWnd);
      [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
      public static extern int IntersectClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool InvalidateRect(IntPtr hWnd, IntPtr rectangle, bool bErase);
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr KillTimer(IntPtr hWnd, IntPtr nIDEvent);
      [DllImport("user32.dll")]
      private static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
      [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      public static extern int MapVirtualKey(int uCode, int uMapType);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("gdi32.dll")]
      public static extern bool MaskBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, IntPtr hbmMask, int xMask, int yMask, uint dwRop);
      [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
      public static extern bool MessageBeep(int type);
      [DllImport("gdi32.dll")]
      public static extern int ModifyWorldTransform(IntPtr tmp_hDC, ref XFORM lpXform, uint iMode);
      [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
      public static extern int MsgWaitForMultipleObjects(int nCount, int pHandles, bool fWaitAll, int dwMilliseconds, int dwWakeMask);
      [DllImport("gdi32.dll")]
      public static extern uint OffsetViewportOrgEx(IntPtr hdc, int nXOffset, int nYOffset, ref POINT lpPoint);
      [DllImport("gdi32.dll")]
      public static extern int OffsetWindowOrgEx(IntPtr hdc, int nXOffset, int nYOffset, ref POINT lpPoint);
      [DllImport("kernel32.dll", SetLastError = true)]
      public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
      [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
      public static extern IntPtr OpenThemeData(IntPtr hWnd, string ClassList);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("gdi32.dll")]
      public static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, int dwRop);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool PeekMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdc, int nFlags);
      [DllImport("kernel32.dll", SetLastError = true)]
      public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
      [DllImport("gdi32", CharSet = CharSet.Auto)]
      public static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, int flags);
      [DllImport("User32.dll")]
      public static extern bool ReleaseCapture();
      [DllImport("user32.dll")]
      public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
      [DllImport("gdi32.dll")]
      public static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);
      [DllImport("gdi32.dll")]
      public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SendNotifyMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SetActiveWindow(IntPtr hWnd);
      [DllImport("gdi32.dll")]
      public static extern int SetBkColor(IntPtr hdc, int crColor);
      [DllImport("gdi32.dll")]
      public static extern int SetBkMode(IntPtr hdc, int iBkMode);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool SetCaretPos(int X, int Y);
      [DllImport("user32.dll")]
      public static extern IntPtr SetFocus(IntPtr hWnd);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool SetForegroundWindow(IntPtr hWnd);
      [DllImport("gdi32.dll")]
      public static extern int SetGraphicsMode(IntPtr hdc, int iMode);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);
      [DllImport("gdi32.dll")]
      public static extern uint SetLayout(IntPtr hdc, uint dwLayout);
      [DllImport("gdi32.dll")]
      public static extern int SetMapMode(IntPtr hdc, int fnMapMode);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll")]
      public static extern bool SetScrollInfo(IntPtr hWnd, int fnBar, [MarshalAs(UnmanagedType.Struct)] ref SCROLLINFO lpsi, [MarshalAs(UnmanagedType.Bool)] bool fRedraw);
      [DllImport("gdi32.dll")]
      public static extern uint SetTextAlign(IntPtr hdc, uint fMode);
      [DllImport("gdi32.dll")]
      public static extern int SetTextColor(IntPtr hdc, int crColor);
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SetTimer(IntPtr hWnd, IntPtr nIDEvent, uint uElapse, QTimerCallbackDelegate lpTimerFunc);
      [DllImport("gdi32.dll")]
      public static extern int SetViewportExtEx(IntPtr hdc, int nXOffset, int nYOffset, ref POINT lpPoint);
      [DllImport("gdi32.dll")]
      public static extern int SetViewportOrgEx(IntPtr hdc, int nXOffset, int nYOffset, ref POINT lpPoint);
      [DllImport("gdi32.dll")]
      public static extern bool SetWindowExtEx(IntPtr hdc, int X, int Y, ref POINT lpPoint);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
      [DllImport("gdi32.dll")]
      public static extern bool SetWindowOrgEx(IntPtr hdc, int X, int Y, ref POINT lpPoint);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll")]
      public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int iX, int iY, int cX, int cY, uint uFlags);
      [DllImport("user32.dll")]
      public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, int bRedraw);
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);
      [DllImport("gdi32.dll")]
      public static extern int SetWorldTransform(IntPtr tmp_hDC, ref XFORM lpXform);
      [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SHAutoComplete(IntPtr hwndEdit, uint dwFlags);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool ShowCaret(IntPtr hWnd);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll")]
      public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool SystemParametersInfo(int uiAction, int uiParam, IntPtr pvParam, int fWinIni);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("gdi32.dll")]
      public static extern bool TextOut(IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool TranslateMessage(ref MSG msg);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool UnhookWindowsHookEx(IntPtr hhook);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool UpdateWindow(IntPtr hwnd);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool ValidateRect(IntPtr hWnd, IntPtr rectangle);
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);
      [DllImport("kernel32.dll", SetLastError = true)]
      public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int flAllocationType, int flProtect);
      [DllImport("kernel32.dll", SetLastError = true)]
      public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int dwFreeType);
      [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      public static extern short VkKeyScan(char ch);
      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("User32.dll", CharSet = CharSet.Auto)]
      public static extern bool WaitMessage();
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr WindowFromPoint(POINT Point);

      // Nested Types
      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      public struct BLENDFUNCTION
      {
         public byte BlendOp;
         public byte BlendFlags;
         public byte SourceConstantAlpha;
         public byte AlphaFormat;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct DTBGOPTS
      {
         public int dwSize;
         public int dwFlags;
         public NativeMethods.RECT rcClip;
      }

      public delegate bool EnumThreadWindowsCallBack(IntPtr hWnd, IntPtr lParam);

      internal delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

      [StructLayout(LayoutKind.Sequential)]
      public struct ICONINFO : IDisposable
      {
         public bool fIcon;
         public int xHotspot;
         public int yHotspot;
         public IntPtr hbmMask;
         public IntPtr hbmColor;
         public void Dispose();
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct KBDLLHOOKSTRUCT
      {
         public int vkCode;
         public int scanCode;
         public int flags;
         public int time;
         public IntPtr dwExtraInfo;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct LOGBRUSH
      {
         public uint lbStyle;
         public uint lbColor;
         public uint lbHatch;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct LOGFONT
      {
         public int lfHeight;
         public int lfWidth;
         public int lfEscapement;
         public int lfOrientation;
         public int lfWeight;
         public byte lfItalic;
         public byte lfUnderline;
         public byte lfStrikeOut;
         public byte lfCharSet;
         public byte lfOutPrecision;
         public byte lfClipPrecision;
         public byte lfQuality;
         public byte lfPitchAndFamily;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
         public string lfFaceName;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct MARGINS
      {
         public int cxLeftWidth;
         public int cxRightWidth;
         public int cyTopHeight;
         public int cyBottomHeight;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct MINMAXINFO
      {
         public NativeMethods.POINT ptReserved;
         public NativeMethods.POINT ptMaxSize;
         public NativeMethods.POINT ptMaxPosition;
         public NativeMethods.POINT ptMinTrackSize;
         public NativeMethods.POINT ptMaxTrackSize;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct MOUSEHOOKSTRUCT
      {
         public int pt_x;
         public int pt_y;
         public IntPtr hWnd;
         public int wHitTestCode;
         public int dwExtraInfo;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct MOUSEHOOKSTRUCTEX
      {
         public NativeMethods.MOUSEHOOKSTRUCT MOUSEHOOKSTRUCT;
         public uint mouseData;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct MSG : IDisposable
      {
         public IntPtr hwnd;
         public int message;
         public IntPtr wParam;
         public IntPtr lParam;
         public int time;
         public int pt_x;
         public int pt_y;
         public void Dispose();
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct NCCALCSIZE_PARAMS : IDisposable
      {
         public NativeMethods.RECT rgrc0;
         public NativeMethods.RECT rgrc1;
         public NativeMethods.RECT rgrc2;
         public IntPtr lppos;
         public void Dispose();
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct NONCLIENTMETRICS
      {
         public int cbSize;
         public int iBorderWidth;
         public int iScrollWidth;
         public int iScrollHeight;
         public int iCaptionWidth;
         public int iCaptionHeight;
         public NativeMethods.LOGFONT lfCaptionFont;
         public int iSmCaptionWidth;
         public int iSmCaptionHeight;
         public NativeMethods.LOGFONT lfSmCaptionFont;
         public int iMenuWidth;
         public int iMenuHeight;
         public NativeMethods.LOGFONT lfMenuFont;
         public NativeMethods.LOGFONT lfStatusFont;
         public NativeMethods.LOGFONT lfMessageFont;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct PAINTSTRUCT
      {
         public IntPtr hdc;
         public bool fErase;
         public int rcPaint_left;
         public int rcPaint_top;
         public int rcPaint_right;
         public int rcPaint_bottom;
         public bool fRestore;
         public bool fIncUpdate;
         public int reserved1;
         public int reserved2;
         public int reserved3;
         public int reserved4;
         public int reserved5;
         public int reserved6;
         public int reserved7;
         public int reserved8;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct POINT
      {
         public int x;
         public int y;
         public POINT(int ix, int iy);
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct RECT
      {
         public int left;
         public int top;
         public int right;
         public int bottom;
         public RECT(int iLeft, int iTop, int iWidth, int iHeight);
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct SCROLLBARINFO
      {
         public int cbSize;
         public NativeMethods.RECT rcScrollBar;
         public int dxyLineButton;
         public int xyThumbTop;
         public int xyThumbBottom;
         public int reserved;
         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
         public int[] rgstate;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct SCROLLINFO
      {
         public int cbSize;
         public int fMask;
         public int nMin;
         public int nMax;
         public int nPage;
         public int nPos;
         public int nTrackPos;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct SIZE
      {
         public int cx;
         public int cy;
         public SIZE(int icx, int icy);
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct TBBUTTON
      {
         public int iBitmap;
         public int idCommand;
         public byte fsState;
         public byte fsStyle;
         public short bReserved;
         public IntPtr dwData;
         public IntPtr iString;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct TEXTMETRIC
      {
         public int tmHeight;
         public int tmAscent;
         public int tmDescent;
         public int tmInternalLeading;
         public int tmExternalLeading;
         public int tmAveCharWidth;
         public int tmMaxCharWidth;
         public int tmWeight;
         public int tmOverhang;
         public int tmDigitizedAspectX;
         public int tmDigitizedAspectY;
         public char tmFirstChar;
         public char tmLastChar;
         public char tmDefaultChar;
         public char tmBreakChar;
         public byte tmItalic;
         public byte tmUnderlined;
         public byte tmStruckOut;
         public byte tmPitchAndFamily;
         public byte tmCharSet;
      }

      public enum THEMESIZE
      {
         TS_MIN,
         TS_TRUE,
         TS_DRAW
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct TRACKMOUSEEVENT : IDisposable
      {
         public uint cbSize;
         public uint dwFlags;
         public IntPtr hwndTrack;
         public uint dwHoverTime;
         public void Dispose();
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct WINDOWPLACEMENT
      {
         public int length;
         public int flags;
         public int showCmd;
         public NativeMethods.POINT ptMinPosition;
         public NativeMethods.POINT ptMaxPosition;
         public NativeMethods.RECT rcNormalPosition;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct WINDOWPOS : IDisposable
      {
         public IntPtr hwnd;
         public IntPtr hwndInsertAfter;
         public int x;
         public int y;
         public int cx;
         public int cy;
         public uint flags;
         public void Dispose();
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct XFORM
      {
         public float eM11;
         public float eM12;
         public float eM21;
         public float eM22;
         public float eDx;
         public float eDy;
      }
   }
}