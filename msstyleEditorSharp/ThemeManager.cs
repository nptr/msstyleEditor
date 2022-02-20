using libmsstyle;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace msstyleEditor
{
    class ThemeManager
    {
        // This ends up calling
        // rundll32.exe uxtheme.dll,#64 C:\Windows\resources\Themes\Aero\Aero.msstyles?NormalColor?NormalSize
        // Can this be? Manually calling fails with access denied except on aero.

        [DllImport("uxtheme.dll", EntryPoint = "#65", CharSet = CharSet.Unicode)]
        private static extern uint SetSystemVisualStyle(string themeFile, string colorName, string sizeName, uint unknownFlags);

        [DllImport("uxtheme.dll", EntryPoint = "GetCurrentThemeName", CharSet = CharSet.Unicode)]
        private static extern uint GetCurrentThemeName(
            StringBuilder themeFile, int maxNameChars,
            StringBuilder colorName, int maxColorChars,
            StringBuilder sizeName, int maxSizeChars);


        private Random m_rng = new Random();
        private string m_prevTheme;
        private string m_prevColor;
        private string m_prevSize;

        private bool m_themeInUse;
        public bool IsThemeInUse { get { return m_themeInUse; } }

        private string m_customTheme;
        public string Theme { get { return m_customTheme; } }

        public ThemeManager()
        {
            StringBuilder t = new StringBuilder(260);
            StringBuilder c = new StringBuilder(260);
            StringBuilder s = new StringBuilder(260);
            if(GetCurrentThemeName(t, 260, c, 260, s, 260) != 0)
            {
                throw new SystemException("GetCurrentThemeName() failed!");
            }

            m_prevTheme = t.ToString();
            m_prevColor = c.ToString();
            m_prevSize = s.ToString();
        }

        ~ThemeManager()
        {
            try
            {
                Rollback();
            }
            catch(Exception) { }
        }

        public void ApplyTheme(VisualStyle style)
        {
            // Applying a style from any folder in /Users/<currentuser>/ breaks Win8.1 badly. (Issue #9)
            // This means /Desktop/, /AppData/, /Temp/, etc. cannot be used; weird...
            // A writeable alternative that doesn't crash Win8.1 is anything inside /Users/Public/.
            var pubDoc = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            var path = String.Format("{0}\\tmp{1:D5}.msstyles", pubDoc, m_rng.Next(0, 10000));

            style.Save(path, true);
            uint hr = SetSystemVisualStyle(path,"NormalColor", "NormalSize", 0);
            if (hr != 0)
            {
                // We typically get codes like this: 0x90004004. To big for a plain Win32 error, so it must 
                // be a HRESULT. But 0x9 would mean, the N (= NTSTATUS) bit is set and that its just a warning?
                // Furthermore, 0x4004 doesn't translate to a meaningful NTSTATUS.
                // 
                // HRESULT:     S R C N X fffffffffff cccccccccccccccc
                // NTSTATUS:    S S C N f fffffffffff cccccccccccccccc
                //
                // If we clear the N bit, we'd get 0x80004004 which is COM error E_ABORT. Much better.
                // So we assume we get regular HRESULTs and ignore the bits R C N X.
                uint hresult = hr & 0x87FFFFFF;
                string msg = Marshal.GetExceptionForHR(unchecked((int)hresult)).Message;

                // 0x90004004 => COM error E_ABORT => Malformed style, OS can't read it
                // 0x90070490 => WIN32 error ERROR_NOT_FOUND ("Element not found.") => Resource missing

                File.Delete(path);

                throw new SystemException($"Failed to apply the theme as the OS rejected it! Message:\r\n\r\n{msg}");
            }
            else
            {
                if (!String.IsNullOrEmpty(m_customTheme))
                {
                    try
                    {
                        File.Delete(m_customTheme);
                    }
                    catch (Exception) { }
                }

                m_customTheme = path;
                m_themeInUse = true;
            }
        }

        public void Rollback()
        {
            if (String.IsNullOrEmpty(m_customTheme))
            {
                return;
            }

            if (SetSystemVisualStyle(m_prevTheme, m_prevColor, m_prevSize, 0) != 0)
            {
                throw new SystemException("Failed to switch back to the previous theme!");
            }

            try
            {
                Thread.Sleep(250); // the OS takes a while to switch visual style
                File.Delete(m_customTheme);
            }
            catch (Exception) { }

            m_themeInUse = false;
        }

        public bool GetActiveTheme(out string theme, out string color, out string size)
        {
            StringBuilder t = new StringBuilder(260);
            StringBuilder c = new StringBuilder(260);
            StringBuilder s = new StringBuilder(260);

            bool res = GetCurrentThemeName(t, 260, c, 260, s, 260) == 0;

            theme = t.ToString();
            color = c.ToString();
            size = s.ToString();
            return res;
        }
    }
}
