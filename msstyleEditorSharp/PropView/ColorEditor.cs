using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace msstyleEditor.PropView
{
    class ColorEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value.GetType() != typeof(Color))
            {
                return value;
            }

            using (ColorDialog dlg = new ColorDialog())
            {
                dlg.AnyColor = true;
                dlg.Color = (Color)value;
                dlg.FullOpen = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.Color;
                }
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush((Color)e.Value))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
        }
    }
}
