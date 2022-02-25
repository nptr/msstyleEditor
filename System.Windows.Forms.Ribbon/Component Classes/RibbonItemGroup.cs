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


using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    /// A container for grouping buttons on the Ribbon. Prevents the buttons from being separated the owner panel is resized.
    /// An example group could be the Bold, Italic and Underline buttons in th eFont panel of Word. The buttons cannot be separated
    /// and disappear together when the panel size is reduced.
    /// </summary>
    //[Designer("System.Windows.Forms.RibbonQuickAccessToolbarDesigner")]
    [Designer(typeof(RibbonItemGroupDesigner))]
    public class RibbonItemGroup : RibbonItem,
        IContainsSelectableRibbonItems, IContainsRibbonComponents
    {
        #region Fields

        #endregion

        #region Ctor
        public RibbonItemGroup()
        {
            Items = new RibbonItemGroupItemCollection(this);
            Items.SetOwnerItem(this);
            DrawBackground = true;
        }

        public RibbonItemGroup(IEnumerable<RibbonItem> items)
            : this()
        {
            Items.AddRange(items);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                try
                {
                    foreach (RibbonItem ri in Items)
                        ri.Dispose();
                }
                catch (InvalidOperationException)
                {
                    if (!IsOpenInVisualStudioDesigner())
                    {
                        throw;
                    }
                }
            }

            base.Dispose(disposing);
        }
        #endregion

        #region Props

        /// <summary>
        /// This property is not relevant for this class
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Checked
        {
            get => base.Checked;
            set => base.Checked = value;
        }

        /// <summary>
        /// Gets or sets a value indicating if the group should
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        [Description("Background drawing should be avoided when group contains only TextBoxes and ComboBoxes")]
        public bool DrawBackground { get; set; }

        /// <summary>
        /// Gets the first item of the group
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonItem FirstItem
        {
            get
            {
                if (Items.Count > 0)
                {
                    return Items[0];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the last item of the group
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonItem LastItem
        {
            get
            {
                if (Items.Count > 0)
                {
                    return Items[Items.Count - 1];
                }

                return null;
            }
        }


        /// <summary>
        /// Gets the collection of items of this group
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemGroupItemCollection Items { get; }

        #endregion

        #region Methods

        protected override bool ClosesDropDownAt(Point p)
        {
            return false;
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            int curLeft = bounds.Left;

            foreach (RibbonItem item in Items)
            {
                item.SetBounds(new Rectangle(new Point(curLeft, bounds.Top), item.LastMeasuredSize));

                curLeft = item.Bounds.Right + 1;
            }

        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (DrawBackground)
            {
                Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(Owner, e.Graphics, e.Clip, this));
            }

            foreach (RibbonItem item in Items)
            {
                if (item.Visible || Owner.IsDesignMode())
                    item.OnPaint(this, new RibbonElementPaintEventArgs(item.Bounds, e.Graphics, RibbonElementSizeMode.Compact));
            }

            if (DrawBackground)
            {
                Owner.Renderer.OnRenderRibbonItemBorder(new RibbonItemRenderEventArgs(Owner, e.Graphics, e.Clip, this));
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!Visible && !Owner.IsDesignMode())
            {
                SetLastMeasuredSize(new Size(0, 0));
                return LastMeasuredSize;
            }

            //For RibbonItemGroup, size is always compact, and it's designed to be on an horizontal flow
            //tab panel.
            //
            int minWidth = 16;
            int widthSum = 0;
            int maxHeight = 16;

            foreach (RibbonItem item in Items)
            {
                Size s = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(e.Graphics, RibbonElementSizeMode.Compact));
                widthSum += s.Width + 1;
                maxHeight = Math.Max(maxHeight, s.Height);
            }

            widthSum -= 1;

            widthSum = Math.Max(widthSum, minWidth);

            if (Site != null && Site.DesignMode)
            {
                widthSum += 10;
            }

            Size result = new Size(widthSum, maxHeight);
            SetLastMeasuredSize(result);
            return result;
        }

        /// <param name="ownerPanel">RibbonPanel where this item is located</param>
        internal override void SetOwnerPanel(RibbonPanel ownerPanel)
        {
            base.SetOwnerPanel(ownerPanel);

            Items.SetOwnerPanel(ownerPanel);
        }

        /// <param name="owner">Ribbon that owns this item</param>
        internal override void SetOwner(Ribbon owner)
        {
            base.SetOwner(owner);

            Items.SetOwner(owner);
        }

        /// <param name="ownerTab">RibbonTab where this item is located</param>
        internal override void SetOwnerTab(RibbonTab ownerTab)
        {
            base.SetOwnerTab(ownerTab);

            Items.SetOwnerTab(ownerTab);
        }

        /// <param name="ownerItem">RibbonItem where this item is located</param>
        internal override void SetOwnerItem(RibbonItem ownerItem)
        {
            base.SetOwnerItem(ownerItem);
        }

        internal override void ClearOwner()
        {
            List<RibbonItem> oldItems = new List<RibbonItem>(Items);

            base.ClearOwner();

            foreach (RibbonItem item in oldItems)
            {
                item.ClearOwner();
            }
        }

        internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            base.SetSizeMode(sizeMode);

            foreach (RibbonItem item in Items)
            {
                item.SetSizeMode(RibbonElementSizeMode.Compact);
            }
        }

        public override string ToString()
        {
            return "Group: " + Items.Count + " item(s)";
        }
        #endregion

        #region IContainsRibbonItems Members

        public IEnumerable<RibbonItem> GetItems()
        {
            return Items;
        }

        public Rectangle GetContentBounds()
        {
            return Rectangle.FromLTRB(Bounds.Left + 1, Bounds.Top + 1, Bounds.Right - 1, Bounds.Bottom);
        }

        #endregion

        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return Items.ToArray();
        }

        #endregion
    }
}
