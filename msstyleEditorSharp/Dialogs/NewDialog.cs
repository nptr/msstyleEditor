using libmsstyle;
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
    public partial class NewDialog : Form
    {
        public int PartID { get; private set; }
        public int StateID { get; private set; }
        private bool HasState { get; set; }
        public NewDialog()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPartId.Text, out int PartID))
            {
                MessageBox.Show("Part ID must be a number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.PartID = PartID;

            if (!int.TryParse(txtStateId.Text, out int StateID) && HasState)
            {
                MessageBox.Show("State ID must be a number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.StateID = StateID;

            DialogResult = DialogResult.OK;
        }

        public Animation ShowDialogAnimation(PropertyHeader header)
        {
            Text = "New animation";
            HasState = true;
            txtStateId.Enabled = true;
            if (base.ShowDialog() == DialogResult.OK)
            {
                return new Animation(header, PartID, StateID);
            }
            else
            {
                return null;
            }
        }

        public TimingFunction ShowDialogTimingFunction(PropertyHeader header)
        {
            Text = "New timing function";
            HasState = false;
            txtStateId.Enabled = false;
            label2.Enabled = false;
            if (base.ShowDialog() == DialogResult.OK)
            {
                return new TimingFunction(header, PartID);
            }
            else
            {
                return null;
            }
        }
    }
}
