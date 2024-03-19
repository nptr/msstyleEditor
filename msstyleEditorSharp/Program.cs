using msstyleEditor.PropView;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

using TypeDescriptor = System.ComponentModel.TypeDescriptor;

namespace msstyleEditor
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main(String[] args)
        {
            TypeDescriptor.AddAttributes(typeof(Color), new EditorAttribute(typeof(ColorEditor), typeof(UITypeEditor)));
            TypeDescriptor.AddAttributes(typeof(Color), new TypeConverterAttribute(typeof(NoStandardColorConverter)));

            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow(args));
        }
    }
}
