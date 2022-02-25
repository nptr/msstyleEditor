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
    public class RibbonTextBox
    : RibbonItem
    {
        #region Fields
        private const int spacing = 3;

        internal TextBox _actualTextBox;

        internal bool _removingTxt;

        internal bool _labelVisible;

        internal bool _imageVisible;

        internal Rectangle _labelBounds;

        internal Rectangle _imageBounds;

        internal int _textboxWidth;

        internal int _labelWidth;

        internal Rectangle _textBoxBounds;

        internal string _textBoxText;

        internal bool _AllowTextEdit = true;
        /// <summary>
        /// Set to true when using a Combobox to inhibit mouse cursor from flickering as this class and Combobox 
        /// fight for control of the cursor.
        /// </summary>
        internal bool _disableTextboxCursor;
        #endregion
        #region Events
        /// <summary>
        /// Raised when the <see cref="TextBoxText"/> property value has changed
        /// </summary>
        public event EventHandler TextBoxTextChanged;

        public event KeyPressEventHandler TextBoxKeyPress;

        public event KeyEventHandler TextBoxKeyDown;

        public event KeyEventHandler TextBoxKeyUp;

        public event EventHandler TextBoxValidating;

        public event EventHandler TextBoxValidated;
        #endregion
        #region Ctor
        public RibbonTextBox()
        {
            _textboxWidth = 100;
            _textBoxText = "";
        }
        #endregion
        #region Props
        /// <summary>
        /// Gets or sets if the textbox allows editing
        /// </summary>
        [Description("Allow Test Edit")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool AllowTextEdit
        {
            get => _AllowTextEdit;
            set
            {
                _AllowTextEdit = value;
                if (Canvas != null) { Canvas.Cursor = AllowTextEdit ? Cursors.IBeam : Cursors.Default; }
            }
        }

        //
        // Summary:
        //     Gets or sets the character used to mask characters of a password in a single-line
        //     System.Windows.Forms.TextBox control.
        //
        // Returns:
        //     The character used to mask characters entered in a single-line System.Windows.Forms.TextBox
        //     control. Set the value of this property to 0 (character value) if you do
        //     not want the control to mask characters as they are typed. Equals 0 (character
        //     value) by default.
        [DefaultValue('\0')]
        [Category("Behavior")]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public char PasswordChar
        {
            get => _passwordChar;
            set
            {
                _passwordChar = value;

                if (_actualTextBox != null)
                {
                    _actualTextBox.PasswordChar = value;
                }
            }
        }

        private char _passwordChar = '\0';

        /// <summary>
        /// Gets or sets the text on the textbox
        /// </summary>
        [Category("Appearance")]
        [Description("Text on the textbox")]
        public string TextBoxText
        {
            get => _textBoxText;
            set
            {
                _textBoxText = value;
                if (_actualTextBox != null)
                {
                    _actualTextBox.Text = _textBoxText;
                }
                OnTextChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the bounds of the text on the textbox
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle TextBoxTextBounds => TextBoxBounds;

        /// <summary>
        /// Gets the bounds of the image
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ImageBounds => _imageBounds;

        /// <summary>
        /// Gets the bounds of the label that is shown next to the textbox
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle LabelBounds => _labelBounds;

        /// <summary>
        /// Gets a value indicating if the image is currenlty visible
        /// </summary>
        [Category("Appearance")]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ImageVisible => _imageVisible;

        /// <summary>
        /// Gets a value indicating if the label is currently visible
        /// </summary>
        [Category("Appearance")]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool LabelVisible => _labelVisible;

        /// <summary>
        /// Gets the bounds of the text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Rectangle TextBoxBounds => _textBoxBounds;

        /// <summary>
        /// Gets a value indicating if user is currently editing the text of the textbox
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Editing => _actualTextBox != null;

        /// <summary>
        /// Gets or sets the width of the textbox
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(100)]
        public int TextBoxWidth
        {
            get => _textboxWidth;
            set
            {
                _textboxWidth = value;
                NotifyOwnerRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the width of the Label. Enter zero to auto size based on contents.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(0)]
        public int LabelWidth
        {
            get => _labelWidth;
            set
            {
                _labelWidth = value;
                NotifyOwnerRegionsChanged();
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Starts editing the text and focuses the TextBox
        /// </summary>
        public void StartEdit()
        {
            //if (!Enabled) return;
            PlaceActualTextBox();

            _actualTextBox.SelectAll();
            _actualTextBox.Focus();

        }

        /// <summary>
        /// Ends the editing of the textbox
        /// </summary>
        public void EndEdit()
        {
            RemoveActualTextBox();
        }

        /// <summary>
        /// Places the Actual TextBox on the owner so user can edit the text
        /// </summary>
        protected void PlaceActualTextBox()
        {
            _actualTextBox = new TextBox();

            InitTextBox(_actualTextBox);

            _actualTextBox.TextChanged += _actualTextbox_TextChanged;
            _actualTextBox.KeyDown += _actualTextbox_KeyDown;
            _actualTextBox.KeyUp += _actualTextbox_KeyUp;
            _actualTextBox.KeyPress += _actualTextbox_KeyPress;
            _actualTextBox.LostFocus += _actualTextbox_LostFocus;
            _actualTextBox.VisibleChanged += _actualTextBox_VisibleChanged;
            _actualTextBox.Validating += _actualTextbox_Validating;
            _actualTextBox.Validated += _actualTextbox_Validated;

            _actualTextBox.PasswordChar = PasswordChar;

            _actualTextBox.Visible = true;
            //_actualTextBox.AcceptsTab = true;
            Canvas.Controls.Add(_actualTextBox);
            Owner.ActiveTextBox = this;
        }

        public void _actualTextBox_VisibleChanged(object sender, EventArgs e)
        {
            if (!(sender as TextBox).Visible && !_removingTxt)
            {
                RemoveActualTextBox();
            }
        }

        /// <summary>
        /// Removes the actual TextBox that edits the text
        /// </summary>
        protected void RemoveActualTextBox()
        {
            if (_actualTextBox == null || _removingTxt)
            {
                return;
            }
            _removingTxt = true;

            TextBoxText = _actualTextBox.Text;
            _actualTextBox.Visible = false;
            if (_actualTextBox.Parent != null)
            {
                _actualTextBox.Parent.Controls.Remove(_actualTextBox);
            }
            _actualTextBox.Dispose();
            _actualTextBox = null;

            RedrawItem();
            _removingTxt = false;
            Owner.ActiveTextBox = null;
        }

        /// <summary>
        /// Initializes the texbox that edits the text
        /// </summary>
        /// <param name="t"></param>
        protected virtual void InitTextBox(TextBox t)
        {
            t.Text = TextBoxText;
            t.BorderStyle = BorderStyle.None;
            t.Width = TextBoxBounds.Width - 2;

            t.Location = new Point(
                TextBoxBounds.Left + 2,
                Bounds.Top + (Bounds.Height - t.Height) / 2);
        }

        /// <summary>
        /// Handles the LostFocus event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_LostFocus(object sender, EventArgs e)
        {
            RemoveActualTextBox();
        }

        /// <summary>
        /// Handles the KeyDown event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (TextBoxKeyDown != null)
            {
                TextBoxKeyDown(this, e);
            }

            if (e.KeyCode == Keys.Return ||
                e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Escape)
            {
                RemoveActualTextBox();
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (TextBoxKeyUp != null)
            {
                TextBoxKeyUp(this, e);
            }

            if (e.KeyCode == Keys.Return ||
                e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Escape)
            {
                RemoveActualTextBox();
            }
        }


        /// <summary>
        /// Handles the KeyPress event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            if (TextBoxKeyPress != null)
            {
                TextBoxKeyPress(this, e);
            }

            //bool next = false;
            //RibbonItem nextItem = this;

            //foreach (var item in this.OwnerPanel.GetItems())
            //{

            //    if (item == this)
            //    {
            //        next = true;
            //    }
            //    else if (next)
            //    {
            //        nextItem = item;
            //        break;
            //    }
            //}

            //if (nextItem == this)
            //{
            //    var nextTab = this.OwnerPanel.OwnerTab;
            //    var nextFoundTab = false;

            //    foreach (var tab in this.OwnerPanel.OwnerTab.Owner.Tabs)
            //    {
            //        if (tab == this.OwnerPanel.OwnerTab)
            //        {
            //            nextFoundTab = true;
            //        }
            //        else if (nextFoundTab)
            //        {
            //            nextTab = tab;
            //            break;
            //        }
            //    }

            //    if (nextTab == this.OwnerPanel.OwnerTab)
            //    {
            //        nextTab.Owner.OrbSelected = true;
            //        nextTab.Owner.ShowOrbDropDown();
            //    }
            //    else
            //    {
            //        nextTab.Owner.ActiveTab = nextTab;
            //    }

            //}
            //else
            //{
            //    nextItem.SetSelected(true);

            //}
        }

        /// <summary>
        /// Handles the Validating event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_Validating(object sender, CancelEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            if (TextBoxValidating != null)
            {
                TextBoxValidating(this, e);
            }
        }

        /// <summary>
        /// Handles the Validated event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_Validated(object sender, EventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            if (TextBoxValidated != null)
            {
                TextBoxValidated(this, e);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the actual TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _actualTextbox_TextChanged(object sender, EventArgs e)
        {
            //Text = (sender as TextBox).Text;
            {
                TextBoxText = (sender as TextBox).Text;
            }
        }

        /// <summary>
        /// Measures the suposed height of the textobx
        /// </summary>
        /// <returns></returns>
        public virtual int MeasureHeight()
        {
            return 16 + Owner.ItemMargin.Vertical;
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (Owner == null)
            {
                return;
            }

            Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(Owner, e.Graphics, Bounds, this));

            if (ImageVisible)
            {
                Owner.Renderer.OnRenderRibbonItemImage(new RibbonItemBoundsEventArgs(Owner, e.Graphics, e.Clip, this, _imageBounds));
            }

            using (StringFormat f = StringFormatFactory.NearCenterNoWrap(StringTrimming.None))
            {
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
                bounds.Right - TextBoxWidth,
                bounds.Top,
                bounds.Right,
                bounds.Bottom);

            if (Image != null)
            {
                _imageBounds = new Rectangle(
                    bounds.Left + Owner.ItemMargin.Left,
                    bounds.Top + Owner.ItemMargin.Top, Image.Width, Image.Height);
            }
            else
            {
                _imageBounds = new Rectangle(bounds.Location, Size.Empty);
            }

            _labelBounds = Rectangle.FromLTRB(
                _imageBounds.Right + (_imageBounds.Width > 0 ? spacing : 0),
                bounds.Top,
                _textBoxBounds.Left - spacing,
                bounds.Bottom - Owner.ItemMargin.Bottom);

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

            w += TextBoxWidth;

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
            if (!Enabled)
            {
                return;
            }

            base.OnMouseEnter(e);
            if (TextBoxBounds.Contains(e.Location))
            {
                Canvas.Cursor = AllowTextEdit ? Cursors.IBeam : Cursors.Default;
            }
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            base.OnMouseLeave(e);

            Canvas.Cursor = Cursors.Default;
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            base.OnMouseDown(e);

            if (TextBoxBounds.Contains(e.X, e.Y) && _AllowTextEdit)
            {
                StartEdit();
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            base.OnMouseMove(e);

            /* Hack code. Combobox SetBound function should redefine all the bounds (refer to RibbonUpDown) of the 
             * Combobox so that the there is no overlap between the textbox and dropdown. However for some reason 
             * the two controls don't work the same so the position and rendering of the Combobox end up wrong.
            */
            if (!_disableTextboxCursor)
            {
                if (TextBoxBounds.Contains(e.X, e.Y) && AllowTextEdit)
                {
                    Canvas.Cursor = Cursors.IBeam;
                }
                else
                {
                    Canvas.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="TextBoxTextChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        public void OnTextChanged(EventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            NotifyOwnerRegionsChanged();

            if (TextBoxTextChanged != null)
            {
                TextBoxTextChanged(this, e);
            }
        }
        #endregion
    }
}