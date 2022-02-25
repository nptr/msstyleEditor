// *********************************
// Message from Original Author:
//
// 2008 Jose Menendez Poo
// Please give me credit if you use this code. It's all I ask.
// Contact me for more info: menendezpoo@gmail.com
// *********************************
//
// Original project from http://ribbon.codeplex.com/
// Continue to support and maintain by http://officeribbon.codeplex.com/


using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public sealed class RibbonSeparator : RibbonItem
    {
        public RibbonSeparator()
        {
            DropDownWidth = RibbonSeparatorDropDownWidth.Partial;
        }

        public RibbonSeparator(string text)
            : this()
        {
            Text = text;
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if ((Owner == null || !DrawBackground) && !Owner.IsDesignMode())
                return;

            Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(
            Owner, e.Graphics, e.Clip, this));

            if (!string.IsNullOrEmpty(Text))
            {
                Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(
                        Owner, e.Graphics, e.Clip, this,
                        Rectangle.FromLTRB(
                            Bounds.Left + Owner.ItemMargin.Left,
                            Bounds.Top + Owner.ItemMargin.Top,
                            Bounds.Right - Owner.ItemMargin.Right,
                            Bounds.Bottom - Owner.ItemMargin.Bottom), Text, FontStyle.Bold));
            }
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (e.SizeMode == RibbonElementSizeMode.DropDown)
            {
                // A horizontal separator on a menu
                if (string.IsNullOrEmpty(Text))
                {
                    SetLastMeasuredSize(new Size(1, 2));
                }
                else
                {
                    Size sz = e.Graphics.MeasureString(Text, new Font(Owner.Font, FontStyle.Bold)).ToSize();
                    SetLastMeasuredSize(new Size(sz.Width + Owner.ItemMargin.Horizontal + 1, sz.Height + Owner.ItemMargin.Vertical));
                }
            }
            else
            {
                // A vertical separator on a Panel or the QAT
                if (OwnerPanel == null)
                {
                    // A vertical separator on the QAT
                    SetLastMeasuredSize(new Size(7, Owner.QuickAccessToolbar.ContentBounds.Height - Owner.QuickAccessToolbar.Padding.Vertical));
                }
                else
                {
                    // A vertical separator on a Panel
                    SetLastMeasuredSize(new Size(4, OwnerPanel.ContentBounds.Height - Owner.ItemPadding.Vertical - Owner.ItemMargin.Vertical));
                }
            }

            return LastMeasuredSize;
        }

        /// <summary>
        /// Gets or sets a value indicating if the separator should draw the divider lines
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        [Description("Background drawing should be avoided when group contains only TextBoxes and ComboBoxes")]
        public bool DrawBackground { get; set; } = true;

        /// <summary>
        /// The width of the Separator bar when displayed on a drop down
        /// </summary>
        [DefaultValue(0)]
        [Category("Appearance")]
        [Description("The width of the Separator bar when displayed on a drop down")]
        public RibbonSeparatorDropDownWidth DropDownWidth { get; set; }

    }
}
