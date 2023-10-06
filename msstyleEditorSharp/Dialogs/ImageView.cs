using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace msstyleEditor.Dialogs
{
    public partial class ImageView : ToolWindow
    {
        public event EventHandler OnViewBackColorChanged;
        public ImageView()
        {
            InitializeComponent();
            whiteToolStripMenuItem.Tag = Color.White;
            greyToolStripMenuItem.Tag = Color.LightGray;
            blackToolStripMenuItem.Tag = Color.Black;
            checkerToolStripMenuItem.Tag = Color.MediumVioletRed; // hack
        }

        public Image ViewImage
        {
            get { return imageControl.BackgroundImage; }
            set { imageControl.BackgroundImage = value; }
        }

        public Rectangle? ViewHighlightArea
        {
            get { return imageControl.HighlightArea; }
            set { imageControl.HighlightArea = value; }
        }

        public Color ViewBackColor
        {
            get { return imageControl.BackColor; }
            set
            {
                whiteToolStripMenuItem.Checked
                    = greyToolStripMenuItem.Checked
                    = blackToolStripMenuItem.Checked
                    = checkerToolStripMenuItem.Checked
                    = false;

                if (value == Color.White) whiteToolStripMenuItem.Checked = true;
                if (value == Color.LightGray) greyToolStripMenuItem.Checked = true;
                if (value == Color.Black) blackToolStripMenuItem.Checked = true;
                if (value == Color.MediumVioletRed) // TODO: remove this hack..
                {
                    checkerToolStripMenuItem.Checked = true;
                    imageControl.Background = ImageControl.BackgroundStyle.Chessboard;
                }
                else
                {
                    imageControl.Background = ImageControl.BackgroundStyle.Color;
                    imageControl.BackColor = value;
                }
                imageControl.Refresh();
            }
        }


        private void OnImageControlBackColorChanged(object sender, EventArgs e)
        {
            if (OnViewBackColorChanged != null)
            {
                OnViewBackColorChanged(sender, e);
            }
        }

        private void OnImageViewBackgroundChange(object sender, EventArgs e)
        {
            if(sender is ToolStripMenuItem item)
            {
                if(item.Tag is Color c)
                {
                    ViewBackColor = c;
                }
            }
        }

        // TOOLSTRIP STUFF


        public event EventHandler SelectedIndexChanged;

        private int m_selectedIndex = 0;
        public int SelectedIndex => m_selectedIndex;


        private void OnToolButtonClicked(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            foreach (ToolStripButton item in toolStrip.Items)
            {
                if (item == sender) item.Checked = true;
                if ((item != null) && (item != sender))
                {
                    item.Checked = false;
                }
            }

            m_selectedIndex = (int)button.Tag - 1;
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(sender, e);
            }
        }


        public void SetActiveTabs(int activeIndex, int numActive)
        {
            int i = 0;
            foreach (ToolStripButton item in toolStrip.Items)
            {
                item.Visible = i < numActive;
                item.Checked = i == activeIndex;
                ++i;
            }

            m_selectedIndex = activeIndex;
        }

        public void SetActiveTabIndex(int activeIndex)
        {
            if (activeIndex >= toolStrip.Items.Count)
                return;

            if (activeIndex < 0)
                return;

            if (!toolStrip.Items[activeIndex].Visible)
                return;

            int i = 0;
            foreach (ToolStripButton item in toolStrip.Items)
            {
                item.Checked = i++ == activeIndex;
            }

            m_selectedIndex = activeIndex;
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(toolStrip.Items[activeIndex], null);
            }
        }
    }
}
