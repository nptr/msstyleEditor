using libmsstyle;
using msstyleEditor.PropView;
using System;
using System.Windows.Forms;

namespace msstyleEditor.Dialogs
{
    public partial class PropertyViewWindow : ToolWindow
    {
        private VisualStyle m_style;
        private StyleClass m_class;
        private StylePart m_part;

        private StyleState m_state;
        private StyleProperty m_prop;

        public PropertyViewWindow()
        {
            InitializeComponent();
        }

        public void SetStylePart(VisualStyle style, StyleClass cls, StylePart part)
        {
            m_style = style;
            m_class = cls;
            m_part = part;

            propertyView.SelectedObject = (part != null)
                ? new TypeDescriptor(part, style)
                : null;
        }

        private void OnPropertyAdd(object sender, EventArgs e)
        {
            if (m_state == null)
            {
                MessageBox.Show("Select a state or property within this state first!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dlg = new PropertyDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.Text = "Add Property to " + m_state.StateName;
            var newProp = dlg.ShowDialog();
            if (newProp != null)
            {
                // Add the prop where requested. TODO: test this
                newProp.Header.classID = m_class.ClassId;
                newProp.Header.partID = m_part.PartId;
                newProp.Header.stateID = m_state.StateId;

                m_state.Properties.Add(newProp);
                propertyView.Refresh();
            }
        }

        private void OnPropertyRemove(object sender, EventArgs e)
        {
            if (m_state == null ||
                m_prop == null)
            {
                return;
            }

            m_state.Properties.Remove(m_prop);
            propertyView.Refresh();
        }

        private void OnPropertySelected(object sender, SelectedGridItemChangedEventArgs e)
        {
            const int CTX_ADD = 0;
            const int CTX_REM = 1;

            bool haveSelection = e.NewSelection != null;
            propViewContextMenu.Items[CTX_ADD].Enabled = haveSelection;
            propViewContextMenu.Items[CTX_REM].Enabled = haveSelection;
            if (!haveSelection)
                return;

            var propDesc = e.NewSelection.PropertyDescriptor as StylePropertyDescriptor;
            if (propDesc != null)
            {
                propViewContextMenu.Items[CTX_ADD].Text = "Add Property to [" + propDesc.Category + "]";
                propViewContextMenu.Items[CTX_REM].Text = "Remove " + propDesc.Name;

                m_state = propDesc.StyleState;
                m_prop = propDesc.StyleProperty;
                return;
            }

            var dummyDesc = e.NewSelection.PropertyDescriptor as PlaceHolderPropertyDescriptor;
            if (dummyDesc != null)
            {
                propViewContextMenu.Items[CTX_ADD].Enabled = true;
                propViewContextMenu.Items[CTX_ADD].Text = "Add Property to [" + dummyDesc.Category + "]";
                propViewContextMenu.Items[CTX_REM].Enabled = false;
                propViewContextMenu.Items[CTX_REM].Text = "Remove";
                m_state = dummyDesc.StyleState;
                return;
            }

            if (e.NewSelection.GridItemType == GridItemType.Category)
            {
                OnPropertySelected(sender, new SelectedGridItemChangedEventArgs(null, e.NewSelection.GridItems[0])); // select child
                return;
            }

            if (e.NewSelection.GridItemType == GridItemType.Property)
            {
                OnPropertySelected(sender, new SelectedGridItemChangedEventArgs(null, e.NewSelection.Parent)); // select parent
                return;
            }
        }
    }
}
