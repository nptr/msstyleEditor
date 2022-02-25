// *********************************
// Message from Original Author:
//
// 2008 Jose Menendez Poo
// Please give me credit if you use this code. It's all I ask.
// Contact me for more info: menendezpoo@gmail.com
// *********************************
//
// Original project from http://ribbon.codeplex.com/
// Continue to support and maintain by http://officeribbon.codeplex.com/


using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms.RibbonHelpers
{
    internal class GlobalHook
        : IDisposable
    {
        #region Subclasses

        /// <summary>
        /// Types of available hooks
        /// </summary>
        public enum HookTypes
        {
            /// <summary>
            /// Installs a mouse hook
            /// </summary>
            Mouse,

            /// <summary>
            /// Installs a keyboard hook
            /// </summary>
            Keyboard
        }

        #endregion

        #region Fields
        private HookProcCallBack _HookProc;
        private IntPtr _handle;
        private HookTypes _hookType;

        #endregion

        #region Events

        /// <summary>
        /// Occours when the hook captures a mouse click
        /// </summary>
        public event MouseEventHandler MouseClick;

        /// <summary>
        /// Occours when the hook captures a mouse double click
        /// </summary>
        public event MouseEventHandler MouseDoubleClick;

        /// <summary>
        /// Occours when the hook captures the mouse wheel
        /// </summary>
        public event MouseEventHandler MouseWheel;

        /// <summary>
        /// Occours when the hook captures the press of a mouse button
        /// </summary>
        public event MouseEventHandler MouseDown;

        /// <summary>
        /// Occours when the hook captures the release of a mouse button
        /// </summary>
        public event MouseEventHandler MouseUp;

        /// <summary>
        /// Occours when the hook captures the mouse moving over the screen
        /// </summary>
        public event MouseEventHandler MouseMove;

        /// <summary>
        /// Occours when a key is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// Occours when a key is released
        /// </summary>
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// Occours when a key is pressed
        /// </summary>
        public event KeyPressEventHandler KeyPress;

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate used to recieve HookProc
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        internal delegate IntPtr HookProcCallBack(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new Hook of the specified type
        /// </summary>
        /// <param name="hookType"></param>
        public GlobalHook(HookTypes hookType)
        {
            _hookType = hookType;
            InstallHook();
        }

        ~GlobalHook()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        ///// <summary>
        ///// Gets the type of this hook
        ///// </summary>
        //public HookTypes HookType { get { return _hookType; } }

        ///// <summary>
        ///// Gets the handle of the hook
        ///// </summary>
        //public IntPtr Handle { get { return _handle; } }

        #endregion

        #region Event Triggers

        /// <summary>
        /// Raises the <see cref="MouseClick"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            if (MouseClick != null)
            {
                MouseClick(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDoubleClick"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (MouseDoubleClick != null)
            {
                MouseDoubleClick(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseWheel"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
            if (MouseWheel != null)
            {
                MouseWheel(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyDown"/> event
        /// </summary>
        /// <param name="e">Event Data</param>
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            if (KeyDown != null)
            {
                KeyDown(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyUp"/> event
        /// </summary>
        /// <param name="e">Event Data</param>
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            if (KeyUp != null)
            {
                KeyUp(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyPress"/> event
        /// </summary>
        /// <param name="e">Event Data</param>
        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {
            if (KeyPress != null)
            {
                KeyPress(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recieves the actual unsafe mouse hook procedure
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
            {
                return WinApi.CallNextHookEx(_handle, code, wParam, lParam);
            }

            switch (_hookType)
            {
                case HookTypes.Mouse:
                    return MouseProc(code, wParam, lParam);
                case HookTypes.Keyboard:
                    return KeyboardProc(code, wParam, lParam);
                default:
                    throw new ArgumentException("HookType not supported");
            }
        }

        /// <summary>
        /// Recieves the actual unsafe keyboard hook procedure
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private IntPtr KeyboardProc(int code, IntPtr wParam, IntPtr lParam)
        {
            WinApi.KeyboardLLHookStruct hookStruct = (WinApi.KeyboardLLHookStruct)Marshal.PtrToStructure(lParam, typeof(WinApi.KeyboardLLHookStruct));

            int msg = wParam.ToInt32();
            bool handled = false;

            if (msg == WinApi.WM_KEYDOWN || msg == WinApi.WM_SYSKEYDOWN)
            {
                KeyEventArgs e = new KeyEventArgs((Keys)hookStruct.vkCode);
                OnKeyDown(e);
                handled = e.Handled;
            }
            else if (msg == WinApi.WM_KEYUP || msg == WinApi.WM_SYSKEYUP)
            {
                KeyEventArgs e = new KeyEventArgs((Keys)hookStruct.vkCode);
                OnKeyUp(e);
                handled = e.Handled;
            }

            if (msg == WinApi.WM_KEYDOWN && KeyPress != null)
            {
                byte[] keyState = new byte[256];
                byte[] buffer = new byte[2];
                WinApi.GetKeyboardState(keyState);
                int conversion = WinApi.ToAscii(hookStruct.vkCode, hookStruct.scanCode, keyState, buffer, hookStruct.flags);

                if (conversion == 1 || conversion == 2)
                {
                    bool shift = (WinApi.GetKeyState(WinApi.VK_SHIFT) & 0x80) == 0x80;
                    bool capital = WinApi.GetKeyState(WinApi.VK_CAPITAL) != 0;
                    char c = (char)buffer[0];
                    if ((shift ^ capital) && Char.IsLetter(c))
                    {
                        c = Char.ToUpper(c);
                    }
                    KeyPressEventArgs e = new KeyPressEventArgs(c);
                    OnKeyPress(e);
                    handled |= e.Handled;
                }
            }


            return handled ? (IntPtr)1 : WinApi.CallNextHookEx(_handle, code, wParam, lParam);
        }

        /// <summary>
        /// Processes Mouse Procedures
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private IntPtr MouseProc(int code, IntPtr wParam, IntPtr lParam)
        {
            WinApi.MouseLLHookStruct hookStruct = (WinApi.MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(WinApi.MouseLLHookStruct));

            int msg = wParam.ToInt32();
            int x = hookStruct.pt.x;
            int y = hookStruct.pt.y;
            int delta = (short)((hookStruct.mouseData >> 16) & 0xffff);

            if (msg == WinApi.WM_MOUSEWHEEL)
            {
                OnMouseWheel(new MouseEventArgs(MouseButtons.None, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_MOUSEMOVE)
            {
                OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_LBUTTONDBLCLK)
            {
                OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_LBUTTONDOWN)
            {
                OnMouseDown(new MouseEventArgs(MouseButtons.Left, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_LBUTTONUP)
            {
                OnMouseUp(new MouseEventArgs(MouseButtons.Left, 0, x, y, delta));
                OnMouseClick(new MouseEventArgs(MouseButtons.Left, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_MBUTTONDBLCLK)
            {
                OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Middle, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_MBUTTONDOWN)
            {
                OnMouseDown(new MouseEventArgs(MouseButtons.Middle, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_MBUTTONUP)
            {
                OnMouseUp(new MouseEventArgs(MouseButtons.Middle, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_RBUTTONDBLCLK)
            {
                OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Right, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_RBUTTONDOWN)
            {
                OnMouseDown(new MouseEventArgs(MouseButtons.Right, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_RBUTTONUP)
            {
                OnMouseUp(new MouseEventArgs(MouseButtons.Right, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_XBUTTONDBLCLK)
            {
                OnMouseDoubleClick(new MouseEventArgs(MouseButtons.XButton1, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_XBUTTONDOWN)
            {
                OnMouseDown(new MouseEventArgs(MouseButtons.XButton1, 0, x, y, delta));
            }
            else if (msg == WinApi.WM_XBUTTONUP)
            {
                OnMouseUp(new MouseEventArgs(MouseButtons.XButton1, 0, x, y, delta));
            }

            return WinApi.CallNextHookEx(_handle, code, wParam, lParam);
        }

        /// <summary>
        /// Installs the actual unsafe hook
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private void InstallHook()
        {
            // Error check
            if (_handle != IntPtr.Zero)
                throw new InvalidOperationException("Hook is already installed");

            #region htype
            int htype = 0;

            switch (_hookType)
            {
                case HookTypes.Mouse:
                    htype = WinApi.WH_MOUSE_LL;
                    break;
                case HookTypes.Keyboard:
                    htype = WinApi.WH_KEYBOARD_LL;
                    break;
                default:
                    throw new ArgumentException("HookType is not supported");
            }
            #endregion

            // Delegate to recieve message
            _HookProc = HookProc;

            // Hook
            // Ed Obeda suggestion for .net 4.0
            //_hHook = WinApi.SetWindowsHookEx(htype, _HookProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
            _handle = WinApi.SetWindowsHookEx(htype, _HookProc, Process.GetCurrentProcess().MainModule.BaseAddress, 0);
            int lastWin32Error = Marshal.GetLastWin32Error();
            // Error check
            if (_handle == IntPtr.Zero) throw new Win32Exception(lastWin32Error);
        }

        /// <summary>
        /// Unhooks the hook
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private void Unhook()
        {
            if (_handle != IntPtr.Zero)
            {
                //bool ret = WinApi.UnhookWindowsHookEx(Handle);

                //if (ret == false)
                //    throw new Win32Exception(Marshal.GetLastWin32Error());

                //_hHook = 0; 
                try
                {
                    //Fix submitted by Simon Dallmair to handle win32 error when closing the form in vista
                    if (!WinApi.UnhookWindowsHookEx(_handle))
                    {
                        int lastWin32Error = Marshal.GetLastWin32Error();
                        Win32Exception ex = new Win32Exception(lastWin32Error);
                        if (ex.NativeErrorCode != 0)
                            throw ex;
                    }

                    _handle = IntPtr.Zero;
                }
                catch (Exception)
                {
                }

            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_handle != IntPtr.Zero)
            {
                Unhook();
            }
        }

        #endregion
    }
}
