using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace msstyleEditor
{

    public partial class ControlDemoDialog : Form
    {
        private const int PBM_SETSTATE = 0x410;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public ControlDemoDialog()
        {
            InitializeComponent();

            // Native main menu
            MenuItem miClose = new MenuItem("Close", 
                (s, e) => this.Close());
            MenuItem miToolWindow = new MenuItem("Toggle Tool Window", 
                (s, e) => this.FormBorderStyle = this.FormBorderStyle == FormBorderStyle.Sizable
                    ? FormBorderStyle.SizableToolWindow 
                    : FormBorderStyle.Sizable);
            MenuItem miFileDlg = new MenuItem("File Dialog", 
                (s, e) => new OpenFileDialog().ShowDialog());
            MenuItem miColorDlg = new MenuItem("Color Dialog", 
                (s, e) => new ColorDialog() { FullOpen = true }.ShowDialog());

            var mainMenu = new MainMenu();
            mainMenu.MenuItems.Add(new MenuItem("Window", new MenuItem[] { miClose, miToolWindow }));
            mainMenu.MenuItems.Add(new MenuItem("View", new MenuItem[] { miFileDlg, miColorDlg }));
            this.Menu = mainMenu;

            // Native status bar
            StatusBarPanel panel1 = new StatusBarPanel();
            panel1.Text = "Status Bar";
            StatusBarPanel panel2 = new StatusBarPanel();
            panel2.Text = "Panel 1";
            StatusBar statusBar = new StatusBar();
            statusBar.ShowPanels = true;
            statusBar.Panels.Add(panel1);
            statusBar.Panels.Add(panel2);
            this.Controls.Add(statusBar);

            // List view
            string[] text = new string[] { "Item 1", "Item 2" };
            listView1.Items.Add(new ListViewItem(text, 0, listView1.Groups[0]));
            listView1.Items.Add(new ListViewItem(text, 0, listView1.Groups[1]));

            // Progress bar with states
            SendMessage(progressBarError.Handle, PBM_SETSTATE, 2, 0);
            SendMessage(progressBarPaused.Handle, PBM_SETSTATE, 3, 0);

            // Combo box setup
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

    }
}
