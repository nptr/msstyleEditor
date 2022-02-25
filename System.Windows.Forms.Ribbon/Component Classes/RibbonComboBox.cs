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
    [Designer(typeof(RibbonComboBoxDesigner))]
    public class RibbonComboBox
        : RibbonTextBox, IContainsRibbonComponents, IDropDownRibbonItem
    {
        #region Fields
        // Steve
        private RibbonItem _selectedItem;
        private readonly Set<RibbonItem> _assignedHandlers = new Set<RibbonItem>();
        #endregion

        #region Events

        /// <summary>
        /// Raised when the DropDown is about to be displayed
        /// </summary>
        public event EventHandler DropDownShowing;

        public event RibbonItemEventHandler DropDownItemClicked;
        public delegate void RibbonItemEventHandler(object sender, RibbonItemEventArgs e);

        #endregion

        #region Ctor

        public RibbonComboBox()
        {
            DropDownItems = new RibbonItemCollection();
            DropDownItems.SetOwnerItem(this);
            DropDownVisible = false;
            AllowTextEdit = true;
            DrawIconsBar = true;
            DropDownMaxHeight = 0;
            _disableTextboxCursor = true;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveHandlers();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum height for the dropdown window.  0 = autosize.  If the size is smaller than the contents then scrollbars will be shown.
        /// </summary>
        [DefaultValue(0)]
        [Category("Behavior")]
        [Description("Gets or sets the maximum height for the dropdown window.  0 = Autosize.  If the size is smaller than the contents then scrollbars will be shown.")]
        public int DropDownMaxHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the DropDown portion of the combo box is currently shown.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        [Description("Indicates if the dropdown window is currently visible")]
        public bool DropDownVisible { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if the DropDown should be resizable
        /// </summary>
        [DefaultValue(false)]
        [Category("Drop Down")]
        [Description("Makes the DropDown resizable with a grip on the corner")]
        public bool DropDownResizable { get; set; }

        /// <summary>
        /// Overriden.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override Rectangle TextBoxTextBounds
        {
            get
            {
                Rectangle r = base.TextBoxTextBounds;

                r.Width -= DropDownButtonBounds.Width;

                return r;
            }
        }

        /// <summary>
        /// Gets the collection of items to be displayed on the dropdown
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Drop Down")]
        public RibbonItemCollection DropDownItems { get; }

        // Steve
        /// <summary>
        /// Gets the selected of item on the dropdown
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonItem SelectedItem
        {
            get
            {
                if (_selectedItem == null)
                {
                    foreach (RibbonItem item in DropDownItems)
                    {
                        if (item.Text == TextBoxText)
                        {
                            _selectedItem = item;
                            return item;
                        }
                    }
                    return null;
                }

                if (DropDownItems.Contains(_selectedItem))
                {
                    return _selectedItem;
                }

                _selectedItem = null;
                return null;
            }
            //Steve
            set
            {
                if (value == null)
                {
                    _selectedItem = null;
                    TextBoxText = String.Empty;
                }
                else if (value.GetType().BaseType == typeof(RibbonItem))
                {
                    if (DropDownItems.Contains(value))
                    {
                        _selectedItem = value;
                        TextBoxText = _selectedItem.Text;
                    }
                    else
                    {
                        //_dropDownItems.Add(value);
                        _selectedItem = value;
                        TextBoxText = _selectedItem.Text;
                    }
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int SelectedIndex
        {
            get
            {
                if (_selectedItem != null)
                {
                    return DropDownItems.IndexOf(_selectedItem);
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (value == -1)
                {
                    SelectedItem = null;
                }
                else
                {
                    if ((DropDownItems.Count > 0) &&
                        (value >= 0) &&
                        (value < DropDownItems.Count))
                    {
                        SelectedItem = DropDownItems[value];
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(SelectedIndex));
                    }
                }
            }
        }

        // Kevin
        /// <summary>
        /// Gets or sets the value of selected item on the dropdown.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedValue
        {
            get
            {
                if (_selectedItem == null)
                {
                    return null;
                }

                return _selectedItem.Value;
            }
            set
            {
                foreach (RibbonItem item in DropDownItems)
                {
                    if (String.Compare(item.Value, value, false) == 0)
                    {
                        if (_selectedItem != item)
                        {
                            _selectedItem = item;
                            TextBoxText = _selectedItem.Text;
                            RibbonItemEventArgs arg = new RibbonItemEventArgs(item);
                            OnDropDownItemClicked(ref arg);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the DropDown of the button
        /// </summary>
        internal RibbonDropDown DropDown { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="DropDownShowing"/> event;
        /// </summary>
        /// <param name="e"></param>
        public void OnDropDownShowing(EventArgs e)
        {
            if (DropDownShowing != null)
            {
                DropDownShowing(this, e);
            }
        }

        /// <summary>
        /// Creates the DropDown menu
        /// </summary>
        protected virtual void CreateDropDown()
        {
            DropDown = new RibbonDropDown(this, DropDownItems, Owner);
        }

        /// <summary>
        /// Gets or sets if the icons bar should be drawn
        /// </summary>
        [Category("Drop Down")]
        [DisplayName("DrawDropDownIconsBar")]
        [DefaultValue(true)]
        public bool DrawIconsBar { get; set; }

        /// <summary>
        /// Shows the DropDown
        /// </summary>
        public virtual void ShowDropDown()
        {
            if (!DropDownVisible)
            {
                AssignHandlers();
                OnDropDownShowing(EventArgs.Empty);

                //Highlight (select) the item on the drop down that matches the ComboBox text.
                foreach (RibbonItem item in DropDownItems)
                {
                    item.SetSelected(item == SelectedItem);
                }

                CreateDropDown();

                DropDown.DropDownMaxHeight = DropDownMaxHeight;
                DropDown.ShowSizingGrip = DropDownResizable;
                DropDown.DrawIconsBar = DrawIconsBar;
                DropDown.Closed += DropDown_Closed;

                Point location = OnGetDropDownMenuLocation();
                DropDown.Show(location);
                DropDownVisible = true;
            }
        }

        private void DropDown_Closed(object sender, EventArgs e)
        {
            DropDownVisible = false;

            //Steve - when popup closed, un-highlight the dropdown arrow and redraw
            DropDownButtonPressed = false;
            //Kevin - Unselect it as well
            DropDownButtonSelected = false;

            SetSelected(false);

            RedrawItem();
        }

        private void AssignHandlers()
        {
            foreach (RibbonItem item in DropDownItems)
            {
                if (_assignedHandlers.Contains(item) == false)
                {
                    item.Click += DropDownItem_Click;
                    _assignedHandlers.Add(item);
                }
            }
        }

        private void RemoveHandlers()
        {
            foreach (RibbonItem item in _assignedHandlers)
            {
                item.Click -= DropDownItem_Click;
            }
            _assignedHandlers.Clear();
        }

        private void DropDownItem_Click(object sender, EventArgs e)
        {
            // Steve
            _selectedItem = (sender as RibbonItem);

            TextBoxText = (sender as RibbonItem).Text;
            //Kevin Carbis
            RibbonItemEventArgs ev = new RibbonItemEventArgs(sender as RibbonItem);
            OnDropDownItemClicked(ref ev);
        }


        #endregion

        #region Overrides

        protected override bool ClosesDropDownAt(Point p)
        {
            return false;
        }

        protected override void InitTextBox(TextBox t)
        {
            base.InitTextBox(t);

            t.Width -= DropDownButtonBounds.Width;
        }

        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);

            DropDownButtonBounds = Rectangle.FromLTRB(
                bounds.Right - 15,
                bounds.Top,
                bounds.Right + 1,
                bounds.Bottom + 1);
        }

        public virtual void OnDropDownItemClicked(ref RibbonItemEventArgs e)
        {
            if (DropDownItemClicked != null)
            {
                DropDownItemClicked(this, e);
            }
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (!Enabled) return;

            base.OnMouseMove(e);

            bool mustRedraw = false;

            if (DropDownButtonBounds.Contains(e.X, e.Y))
            {
                Canvas.Cursor = Cursors.Default;

                mustRedraw = !DropDownButtonSelected;

                DropDownButtonSelected = true;
            }
            else if (TextBoxBounds.Contains(e.X, e.Y))
            {
                Canvas.Cursor = AllowTextEdit ? Cursors.IBeam : Cursors.Default;

                mustRedraw = DropDownButtonSelected;

                DropDownButtonSelected = false;
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

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (!Enabled) return;
            // Steve - if allowtextedit is false, allow the textbox to bring up the popup
            if (DropDownButtonBounds.Contains(e.X, e.Y) || (TextBoxBounds.Contains(e.X, e.Y) != AllowTextEdit))
            {
                DropDownButtonPressed = true;

                ShowDropDown();
            }
            else if (TextBoxBounds.Contains(e.X, e.Y) && AllowTextEdit)
            {
                StartEdit();
            }
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            if (!Enabled) return;

            base.OnMouseUp(e);

            //Steve - pressed if set false when popup is closed
            //_dropDownPressed = false;
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            if (!Enabled) return;

            base.OnMouseLeave(e);

            DropDownButtonSelected = false;

        }

        /// <summary>
        /// Gets the location where the dropdown menu will be shown
        /// </summary>
        /// <returns></returns>
        internal virtual Point OnGetDropDownMenuLocation()
        {
            Point location = Point.Empty;

            if (Canvas is RibbonDropDown)
            {
                location = Canvas.PointToScreen(new Point(TextBoxBounds.Left, Bounds.Bottom));
            }
            else
            {
                location = Owner.PointToScreen(new Point(TextBoxBounds.Left, Bounds.Bottom));
            }

            return location;
        }

        #endregion

        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return DropDownItems.ToArray();
        }

        #endregion

        #region IDropDownRibbonItem Members

        /// <summary>
        /// Gets or sets the bounds of the DropDown button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle DropDownButtonBounds { get; private set; }

        /// <summary>
        /// Gets a value indicating if the DropDown is currently visible
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownButtonVisible => DropDownVisible;

        /// <summary>
        /// Gets a value indicating if the DropDown button is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownButtonSelected { get; private set; }

        /// <summary>
        /// Gets a value indicating if the DropDown button is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DropDownButtonPressed { get; private set; }

        internal override void SetOwner(Ribbon owner)
        {
            base.SetOwner(owner);

            DropDownItems.SetOwner(owner);
        }

        internal override void SetOwnerPanel(RibbonPanel ownerPanel)
        {
            base.SetOwnerPanel(ownerPanel);

            DropDownItems.SetOwnerPanel(ownerPanel);
        }

        internal override void SetOwnerTab(RibbonTab ownerTab)
        {
            base.SetOwnerTab(ownerTab);

            DropDownItems.SetOwnerTab(OwnerTab);
        }

        internal override void SetOwnerItem(RibbonItem ownerItem)
        {
            base.SetOwnerItem(ownerItem);
        }

        internal override void ClearOwner()
        {
            List<RibbonItem> oldItems = new List<RibbonItem>(DropDownItems);

            base.ClearOwner();

            foreach (RibbonItem item in oldItems)
            {
                item.ClearOwner();
            }
        }

        #endregion
    }
}
