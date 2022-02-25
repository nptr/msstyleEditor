using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class RibbonLabel : RibbonItem
    {
        #region Fields

        private int _labelWidth;
        private const int spacing = 3;

        #endregion

        #region Methods
        protected virtual int MeasureHeight()
        {
            if (Owner != null)
                return 16 + Owner.ItemMargin.Vertical;
            return 16 + 4;
        }

        /// <summary>
        /// Measures the size of the panel on the mode specified by the event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!Visible && ((Site == null) || !Site.DesignMode))
            {
                return new Size(0, 0);
            }
            Font f = new Font("Microsoft Sans Serif", 8);
            if (Owner != null)
                f = Owner.Font;

            int w = string.IsNullOrEmpty(Text) ? 0 : ((_labelWidth > 0) ? _labelWidth : (e.Graphics.MeasureString(Text, f).ToSize().Width + 6));
            SetLastMeasuredSize(new Size(w, MeasureHeight()));
            return LastMeasuredSize;
        }

        /// <summary>
        /// Raises the paint event and draws the
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(Owner, e.Graphics, Bounds, this));
                StringFormat f = StringFormatFactory.CenterNoWrap(StringTrimming.None);
                f.Alignment = (StringAlignment)TextAlignment;
                Rectangle clipBounds = Rectangle.FromLTRB(Bounds.Left + 3, Bounds.Top + Owner.ItemMargin.Top, Bounds.Right - 3, Bounds.Bottom - Owner.ItemMargin.Bottom);
                Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(Owner, e.Graphics, Bounds, this, clipBounds, Text, f));
            }
        }

        /// <summary>
        /// Sets the bounds of the panel
        /// </summary>
        /// <param name="bounds"></param>
        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);
        }
        #endregion

        #region Properties
        [Description("Sets the width of the label portion of the control")]
        [Category("Appearance")]
        [DefaultValue(0)]
        public int LabelWidth
        {
            get => _labelWidth;
            set
            {
                if (_labelWidth != value)
                {
                    _labelWidth = value;
                    NotifyOwnerRegionsChanged();
                }
            }
        }
        #endregion
    }
}
