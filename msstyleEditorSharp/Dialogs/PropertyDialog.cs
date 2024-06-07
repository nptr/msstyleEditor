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

namespace msstyleEditor
{
    public partial class PropertyDialog : Form
    {
        private StyleProperty m_property = null;

        private class PropertyWrapper
        {
            public int Id;
            public PropertyInfo Info;
            public PropertyWrapper(int id, PropertyInfo info)
            {
                Id = id;
                Info = info;
            }

            public override string ToString()
            {
                return Info.ToString();
            }
        }

        private string[] TYPE_NAMES =
        {
            "Enum",
            "Int",
            "Bool",
            "Color",
            "Margin",
            "Filename",
            "Size",
            "Position",
            "Rectangle",
            "Font",
            "IntList",
            "ColorList",
            "HighContrastColor"
        };

        private int[] TYPE_IDS =
        {
            200,
            202,
            203,
            204,
            205,
            206,
            207,
            208,
            209,
            210,
            211,
            240,
            241
        };

        public PropertyDialog()
        {
            InitializeComponent();

            cbType.Items.AddRange(TYPE_NAMES);
            cbType.SelectedIndex = 0;
        }

        private void OnTypeChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex < 0)
                return;

            int typeId = TYPE_IDS[cbType.SelectedIndex];

            cbProp.Items.Clear();
            foreach (var pi in VisualStyleProperties.PROPERTY_INFO_MAP)
            {
                // Select all properties matching our type, but not the entry of the type itself
                if (typeId == pi.Value.TypeId &&
                   (typeId != pi.Key || pi.Key == (int)IDENTIFIER.FONT))
                {
                    cbProp.Items.Add(new PropertyWrapper(pi.Key, pi.Value));
                }
            }
            cbProp.SelectedIndex = 0;
        }

        private void OnPropertySelectionChanged(object sender, EventArgs e)
        {
            if (cbProp.SelectedIndex < 0)
            {
                lbPropDescription.Text = String.Empty;
                return;
            }

            var prop = (PropertyWrapper)cbProp.SelectedItem;
            lbPropDescription.Text = prop.Info.Description;
        }

        public new StyleProperty ShowDialog()
        {
            base.ShowDialog();
            return m_property;
        }

        private void OnOk(object sender, EventArgs e)
        {
            var prop = (PropertyWrapper)cbProp.SelectedItem;
            m_property = new StyleProperty((IDENTIFIER)prop.Id, (IDENTIFIER)prop.Info.TypeId);
            this.Close();
        }
    }
}
