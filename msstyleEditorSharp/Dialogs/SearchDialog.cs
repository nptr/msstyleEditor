using libmsstyle;
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
    public partial class SearchDialog : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private readonly libmsstyle.IDENTIFIER[] TYPE_IDS = new libmsstyle.IDENTIFIER[]
        {
            libmsstyle.IDENTIFIER.COLOR,
            libmsstyle.IDENTIFIER.MARGINS,
            libmsstyle.IDENTIFIER.SIZE,
            libmsstyle.IDENTIFIER.POSITION,
            libmsstyle.IDENTIFIER.RECTTYPE,
            libmsstyle.IDENTIFIER.FILENAME,
            libmsstyle.IDENTIFIER.FONT,
        };

        private readonly string[] SEARCH_HINTS = new string[]
        {
            "r; g; b",
            "l; t; r; b",
            "size",
            "x; y",
            "l; t; r; b",
            "id",
            "id"
        };

        public enum SearchMode
        {
            Name = 0,
            Property = 1
        }

        public enum ReplaceMode
        {
            Next = 0,
            All = 1
        }

        public delegate void SearchDelegate(SearchMode searchType, libmsstyle.IDENTIFIER dataType, string search);
        public event SearchDelegate OnSearch;

        public delegate void ReplaceDelegate(ReplaceMode replaceMode, libmsstyle.IDENTIFIER dataType, string search, string replace);
        public event ReplaceDelegate OnReplace;

        public SearchDialog()
        {
            InitializeComponent();

            cbItemType.SelectedIndex = 0;
            cbDataType.SelectedIndex = 0;

            SendMessage(tbReplaceText.Handle, EM_SETCUEBANNER, 0, "Replace...");

            tbSearchText.Select();
        }

        private void OnItemTypeChanged(object sender, EventArgs e)
        {
            bool searchForProperty = cbItemType.SelectedIndex == 1;
            cbDataType.Enabled = searchForProperty;
            tbReplaceText.Enabled = searchForProperty;
            btReplaceNext.Enabled = searchForProperty;
            btReplaceAll.Enabled = searchForProperty;
        }

        private void OnDataTypeChanged(object sender, EventArgs e)
        {
            string hint = SEARCH_HINTS[cbDataType.SelectedIndex];
            SendMessage(tbSearchText.Handle, EM_SETCUEBANNER, 0, hint);
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            if(OnSearch != null)
            {
                if (!String.IsNullOrEmpty(tbSearchText.Text))
                {
                    OnSearch((SearchMode)cbItemType.SelectedIndex,
                        TYPE_IDS[cbDataType.SelectedIndex], tbSearchText.Text);
                }
            }
        }

        private void OnReplaceNextClicked(object sender, EventArgs e)
        {
            if (OnReplace != null)
            {
                if (!String.IsNullOrEmpty(tbSearchText.Text) &&
                    !String.IsNullOrEmpty(tbReplaceText.Text))
                {
                    OnReplace(ReplaceMode.Next, TYPE_IDS[cbDataType.SelectedIndex], tbSearchText.Text, tbReplaceText.Text);
                }
            }
        }

        private void OnReplaceAllClicked(object sender, EventArgs e)
        {
            if (OnReplace != null)
            {
                if (!String.IsNullOrEmpty(tbSearchText.Text) &&
                    !String.IsNullOrEmpty(tbReplaceText.Text))
                {
                    OnReplace(ReplaceMode.All, TYPE_IDS[cbDataType.SelectedIndex], tbSearchText.Text, tbReplaceText.Text);
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if(keyData == Keys.Escape && Form.ModifierKeys == Keys.None)
            {
                this.Hide();
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
