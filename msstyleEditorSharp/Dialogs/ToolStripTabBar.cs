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
    public partial class ToolStripTabBar : UserControl
    {
        public event EventHandler SelectedIndexChanged;

        private int m_selectedIndex = 0;
        public int SelectedIndex => m_selectedIndex;


        public ToolStripTabBar()
        {
            InitializeComponent();
        }

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
                item.Visible = i++ < numActive;
                item.Checked = i == activeIndex;
            }

            m_selectedIndex = activeIndex;
        }

        public void SetActiveTabIndex(int activeIndex)
        {
            if (toolStrip.Items.Count < activeIndex)
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
