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
    /// Represents a quick access toolbar hosted on the Ribbon
    /// </summary>
    public class RibbonQuickAccessToolbar : RibbonItem,
        IContainsSelectableRibbonItems, IContainsRibbonComponents
    {
        #region Fields
        private readonly RibbonQuickAccessToolbarItemCollection _items;
        private bool _dropDownButtonVisible;
        private RibbonMouseSensor _sensor;
        private RibbonButton _dropDownButton;
        #endregion

        #region Ctor

        internal RibbonQuickAccessToolbar(Ribbon ownerRibbon)
        {
            if (ownerRibbon == null) throw new ArgumentNullException(nameof(ownerRibbon));

            SetOwner(ownerRibbon);

            _dropDownButton = new RibbonButton();
            _dropDownButton.SetOwner(ownerRibbon);
            _dropDownButton.SmallImage = CreateDropDownButtonImage();
            _dropDownButton.Style = RibbonButtonStyle.DropDown;

            Margin = new Padding(9);
            Padding = new Padding(3, 0, 0, 0);
            _items = new RibbonQuickAccessToolbarItemCollection(this);
            _sensor = new RibbonMouseSensor(ownerRibbon, ownerRibbon, Items);
            _dropDownButtonVisible = true;
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
                    foreach (RibbonItem item in _items)
                        item.Dispose();
                }
                catch (InvalidOperationException)
                {
                    if (!IsOpenInVisualStudioDesigner())
                    {
                        throw;
                    }
                }
                _dropDownButton.Dispose();
                _sensor.Dispose();
            }
            base.Dispose(disposing);
        }

        private Image CreateDropDownButtonImage()
        {
            Bitmap bmp = new Bitmap(7, 7);
            RibbonProfessionalRenderer renderer = Owner.Renderer as RibbonProfessionalRenderer;

            Color dk = Color.Navy;
            Color lt = Color.White;

            if (renderer != null)
            {
                dk = renderer.ColorTable.Arrow;
                lt = renderer.ColorTable.ArrowLight;
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                DrawDropDownButtonArrow(g, lt, 0, 1);
                DrawDropDownButtonArrow(g, dk, 0, 0);
            }


            return bmp;
        }

        private void DrawDropDownButtonArrow(Graphics g, Color c, int x, int y)
        {
            using (Pen p = new Pen(c))
            {
                using (SolidBrush b = new SolidBrush(c))
                {

                    g.DrawLine(p, x, y, x + 4, y);
                    g.FillPolygon(b, new[] {
                            new Point(x, y + 3),
                            new Point(x + 5, y + 3),
                            new Point(x + 2, y + 6)
                        });
                }
            }
        }

        #endregion

        #region Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        [Description("Shows or hides the dropdown button of the toolbar")]
        [Category("Drop Down")]
        [DefaultValue(true)]
        public bool DropDownButtonVisible
        {
            get => _dropDownButtonVisible;
            set { _dropDownButtonVisible = value; Owner.OnRegionsChanged(); }
        }


        /// <summary>
        /// Gets the bounds of the toolbar including the graphic adornments
        /// </summary>
        [Browsable(false)]
        internal Rectangle SuperBounds => Rectangle.FromLTRB(Bounds.Left - Padding.Horizontal, Bounds.Top, DropDownButton.Bounds.Right, Bounds.Bottom);

        /// <summary>
        /// Gets the dropdown button
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonButton DropDownButton { get { return _dropDownButton; } }

        [Description("The drop down items of the dropdown button of the toolbar")]
        [Category("Drop Down")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonItemCollection DropDownButtonItems => DropDownButton.DropDownItems;

        /// <summary>
        /// Gets or sets the padding (internal) of the toolbar
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding Padding { get; }

        /// <summary>
        /// Gets or sets the margin (external) of the toolbar
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding Margin { get; }

        /// <summary>
        /// Gets or sets a value indicating if the button that shows the menu of the 
        /// QuickAccess toolbar should be visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MenuButtonVisible { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonMouseSensor Sensor { get { return _sensor; } }

        /// <summary>
        /// Gets the Items of the QuickAccess toolbar.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonQuickAccessToolbarItemCollection Items
        {
            get
            {
                if (DropDownButtonVisible)
                {
                    if (!_items.Contains(DropDownButton))
                    {
                        _items.Add(DropDownButton);
                    }
                }
                else
                {
                    if (_items.Contains(DropDownButton))
                    {
                        _items.Remove(DropDownButton);
                    }
                }
                return _items;
            }
        }

        #endregion

        #region Methods

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (Visible && Owner.CaptionBarVisible)
            {
                if (Owner.OrbStyle == RibbonOrbStyle.Office_2007)
                    Owner.Renderer.OnRenderRibbonQuickAccessToolbarBackground(new RibbonRenderEventArgs(Owner, e.Graphics, e.Clip));

                foreach (RibbonItem item in Items)
                {
                    if (item.Visible)
                        item.OnPaint(this, new RibbonElementPaintEventArgs(item.Bounds, e.Graphics, RibbonElementSizeMode.Compact));
                }
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            //For RibbonItemGroup, size is always compact, and it's designed to be on an horizontal flow
            //tab panel.
            //
            if (!Visible || !Owner.CaptionBarVisible)
            {
                SetLastMeasuredSize(new Size(0, 0));
                return LastMeasuredSize;
            }

            int widthSum = Padding.Horizontal;
            int maxHeight = 16;

            foreach (RibbonItem item in Items)
            {
                if (item.Equals(DropDownButton)) continue;
                item.SetSizeMode(RibbonElementSizeMode.Compact);
                Size s = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(e.Graphics, RibbonElementSizeMode.Compact));
                widthSum += s.Width + 1;
                maxHeight = Math.Max(maxHeight, s.Height);
            }

            widthSum -= 1;

            if (Site != null && Site.DesignMode) widthSum += 16;

            Size result = new Size(widthSum, maxHeight);
            SetLastMeasuredSize(result);
            return result;
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            if (Owner.RightToLeft == RightToLeft.No)
            {
                int curLeft = bounds.Left + Padding.Left;

                foreach (RibbonItem item in Items)
                {
                    item.SetBounds(new Rectangle(new Point(curLeft, bounds.Top), item.LastMeasuredSize));

                    curLeft = item.Bounds.Right + 1;
                }
                if (DropDownButtonVisible)
                    DropDownButton.SetBounds(new Rectangle(bounds.Right + bounds.Height / 2 + 2, bounds.Top, 12, bounds.Height));
            }
            else
            {
                int curLeft = bounds.Left + Padding.Left;

                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    Items[i].SetBounds(new Rectangle(new Point(curLeft, bounds.Top), Items[i].LastMeasuredSize));

                    curLeft = Items[i].Bounds.Right + 1;
                }
                if (DropDownButtonVisible)
                    DropDownButton.SetBounds(new Rectangle(bounds.Left - bounds.Height / 2 - 14, bounds.Top, 12, bounds.Height));
            }
        }

        #endregion

        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return Items.ToArray();
        }

        #endregion

        #region IContainsSelectableRibbonItems Members

        public IEnumerable<RibbonItem> GetItems()
        {
            return Items;
        }

        public Rectangle GetContentBounds()
        {
            return Bounds;
        }

        #endregion
    }
}
