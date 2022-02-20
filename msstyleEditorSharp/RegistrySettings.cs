using Microsoft.Win32;
using System;
using System.Text;

namespace msstyleEditor
{
    class RegistrySettings
    {
        private readonly string m_EditorKeyPath = @"HKEY_CURRENT_USER\Software\msstyleEditor";

        public RegistrySettings()
        {
        }

        private object RegGetValueSafe(string key, string name, object defaultValue)
        {
            try
            {
                object r = Registry.GetValue(key, name, defaultValue);
                return r == null ? defaultValue : r;
            }
            catch(Exception)
            {
                return defaultValue;
            }
        }

        private void RegSetValueSafe(string key, string name, object value)
        {
            try
            {
                Registry.SetValue(key, name, value);
            }
            catch (Exception)
            { }
        }

        public bool HasConfirmedWarning
        {
            get
            {
                var obj = RegGetValueSafe(m_EditorKeyPath, "HasConfirmedWarning", "false");
                if (obj is string s)
                {
                    return s.ToLower() == "true";
                }
                else return false;
            }
            set
            {
                RegSetValueSafe(m_EditorKeyPath, "HasConfirmedWarning", "true");
            }
        }
    }
}
