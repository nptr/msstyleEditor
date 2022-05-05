using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace msstyleEditor
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();

            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                lbVersion.Text = String.Format("msstyleEditor v{0}.{1}.{2}.{3}",
                    version.Major, 
                    version.Minor, 
                    version.Build, 
                    version.Revision
                );
            }
            catch (Exception) { }
        }

        private void OnVisitWebsite(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nptr/msstyleEditor");
        }
    }
}
