using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    //[Designer(typeof(RibbonUpDown))]
    public class RibbonUpDown : RibbonTextBox
    {
        #region Fields
        private const int spacing = 3;
        //private Ribbon _ownerRibbon;
        private readonly int _UpDownSize = 16;

        #endregion

        #region Events

        public event MouseEventHandler UpButtonClicked;
        public event MouseEventHandler DownButtonClicked;

        #endregion

        #region Ctor

        public RibbonUpDown()
        {
            _textboxWidth = 50;
            _UpDownSize = 16;
        }

        #endregion

        #region Props


        #endregion

        #region Methods

        /// <summary>
        /// Measures the suposed height of the textbox
        /// </summary>
        /// <returns></returns>
        public override int MeasureHeight()
        {
            return 16 + Owner.ItemMargin.Vertical;
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (Owner == null) return;

            Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(Owner, e.Graphics, Bounds, this));

            if (ImageVisible)
                Owner.Renderer.OnRenderRibbonItemImage(new RibbonItemBoundsEventArgs(Owner, e.Graphics, e.Clip, this, _imageBounds));

            using (StringFormat f = StringFormatFactory.NearCenterNoWrap(StringTrimming.None))
            {
                f.Alignment = StringAlignment.Near;
                f.LineAlignment = StringAlignment.Center;
                f.Trimming = StringTrimming.None;
                f.FormatFlags |= StringFormatFlags.NoWrap;

                Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(Owner, e.Graphics, Bounds, this, TextBoxTextBounds, TextBoxText, f));

                if (LabelVisible)
                {
                    f.Alignment = (StringAlignment)TextAlignment;
                    Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(Owner, e.Graphics, Bounds, this, LabelBounds, Text, f));
                }
            }
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            _textBoxBounds = Rectangle.FromLTRB(
                bounds.Right - TextBoxWidth - _UpDownSize,
                bounds.Top,
                bounds.Right - _UpDownSize,
                bounds.Bottom);

            if (Image != null)
                _imageBounds = new Rectangle(
                    bounds.Left + Owner.ItemMargin.Left,
                    bounds.Top + Owner.ItemMargin.Top, Image.Width, Image.Height);
            else
                _imageBounds = new Rectangle(bounds.Location, Size.Empty);

            _labelBounds = Rectangle.FromLTRB(
                _imageBounds.Right + (_imageBounds.Width > 0 ? spacing : 0),
                bounds.Top,
                _textBoxBounds.Left - spacing,
                bounds.Bottom - Owner.ItemMargin.Bottom);

            UpButtonBounds = new Rectangle(bounds.Right - _UpDownSize, bounds.Top, _UpDownSize, bounds.Height / 2);
            DownButtonBounds = new Rectangle(UpButtonBounds.X, UpButtonBounds.Bottom + 1, UpButtonBounds.Width, bounds.Height - UpButtonBounds.Height);

            if (SizeMode == RibbonElementSizeMode.Large)
            {
                _imageVisible = true;
                _labelVisible = true;
            }
            else if (SizeMode == RibbonElementSizeMode.Medium)
            {
                _imageVisible = true;
                _labelVisible = false;
                _labelBounds = Rectangle.Empty;
            }
            else if (SizeMode == RibbonElementSizeMode.Compact)
            {
                _imageBounds = Rectangle.Empty;
                _imageVisible = false;
                _labelBounds = Rectangle.Empty;
                _labelVisible = false;
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!Visible && !Owner.IsDesignMode())
            {
                SetLastMeasuredSize(new Size(0, 0));
                return LastMeasuredSize;
            }

            Size size = Size.Empty;

            int w = 0;
            int iwidth = Image != null ? Image.Width + spacing : 0;
            int lwidth = string.IsNullOrEmpty(Text) ? 0 : _labelWidth > 0 ? _labelWidth : e.Graphics.MeasureString(Text, Owner.Font).ToSize().Width + spacing;
            int twidth = TextBoxWidth;

            w += TextBoxWidth + _UpDownSize;

            switch (e.SizeMode)
            {
                case RibbonElementSizeMode.Large:
                    w += iwidth + lwidth;
                    break;
                case RibbonElementSizeMode.Medium:
                    w += iwidth;
                    break;
            }

            SetLastMeasuredSize(new Size(w, MeasureHeight()));

            return LastMeasuredSize;
        }

        public override void OnMouseEnter(MouseEventArgs e)
        {
            if (!Enabled) return;

            base.OnMouseEnter(e);
            if (TextBoxBounds.Contains(e.Location))
                Canvas.Cursor = Cursors.IBeam;
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            if (!Enabled) return;

            base.OnMouseLeave(e);

            UpButtonPressed = false;
            DownButtonPressed = false;
            UpButtonSelected = false;
            DownButtonSelected = false;

            Canvas.Cursor = Cursors.Default;
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            if (!Enabled) return;

            base.OnMouseUp(e);
            bool mustRedraw = false;

            if (UpButtonPressed || DownButtonPressed)
                mustRedraw = true;

            UpButtonPressed = false;
            DownButtonPressed = false;

            if (mustRedraw)
                RedrawItem();
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!Enabled) return;

            if (UpButtonBounds.Contains(e.Location))
            {
                UpButtonPressed = true;
                DownButtonPressed = false;
                DownButtonSelected = false;
                if (UpButtonClicked != null)
                    UpButtonClicked(this, e);
            }
            else if (DownButtonBounds.Contains(e.Location))
            {
                DownButtonPressed = true;
                UpButtonPressed = false;
                UpButtonSelected = false;
                if (DownButtonClicked != null)
                    DownButtonClicked(this, e);
            }
            else if (TextBoxBounds.Contains(e.X, e.Y) && AllowTextEdit)
            {
                StartEdit();
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!Enabled) return;

            base.OnMouseMove(e);

            bool mustRedraw = false;

            if (UpButtonBounds.Contains(e.Location))
            {
                Canvas.Cursor = Cursors.Default;
                mustRedraw = !UpButtonSelected || DownButtonSelected || DownButtonPressed;
                UpButtonSelected = true;
                DownButtonSelected = false;
                DownButtonPressed = false;
            }
            else if (DownButtonBounds.Contains(e.Location))
            {
                Canvas.Cursor = Cursors.Default;
                mustRedraw = !DownButtonSelected || UpButtonSelected || UpButtonPressed;
                DownButtonSelected = true;
                UpButtonSelected = false;
                UpButtonPressed = false;
            }
            else if (TextBoxBounds.Contains(e.X, e.Y))
            {
                Canvas.Cursor = Cursors.IBeam;
                mustRedraw = DownButtonSelected || DownButtonPressed || UpButtonSelected || UpButtonPressed;
                UpButtonSelected = false;
                UpButtonPressed = false;
                DownButtonSelected = false;
                DownButtonPressed = false;
            }
            else
            {
                Canvas.Cursor = Cursors.Default;
            }

            if (mustRedraw)
            {
                RedrawItem();
            }
        }

        #endregion

        #region Properties

        ///// <summary>
        ///// Gets the Ribbon this DropDown belongs to
        ///// </summary>
        //public Ribbon OwnerRibbon
        //{
        //   get { return _ownerRibbon; }
        //}

        /// <summary>
        /// Gets a value indicating if the Up button is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UpButtonPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating if the Down button is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DownButtonPressed { get; private set; }

        /// <summary>
        /// Gets a value indicating if the Up button is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UpButtonSelected { get; private set; }

        /// <summary>
        /// Gets a value indicating if the Down button is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DownButtonSelected { get; private set; }

        /// <summary>
        /// Gets or sets the bounds of the DropDown button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle UpButtonBounds { get; private set; }

        /// <summary>
        /// Gets or sets the bounds of the DropDown button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle DownButtonBounds { get; private set; }

        ///// <summary>
        ///// Overriden.
        ///// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public override Rectangle TextBoxTextBounds
        //{
        //   get
        //   {
        //      Rectangle r = base.TextBoxTextBounds;

        //      r.Width -= _UpButtonBounds.Width;

        //      return r;
        //   }
        //}
        #endregion
    }
}
