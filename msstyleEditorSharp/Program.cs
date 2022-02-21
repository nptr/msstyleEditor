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
        [STAThread]
        static void Main()
        {
            TypeDescriptor.AddAttributes(typeof(Color), new EditorAttribute(typeof(ColorEditor), typeof(UITypeEditor)));
            TypeDescriptor.AddAttributes(typeof(Color), new TypeConverterAttribute(typeof(NoStandardColorConverter)));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
