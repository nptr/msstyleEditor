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
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms
{
    [ToolboxItem(false)]
    public class RibbonDropDown
        : RibbonPopup, IScrollableRibbonItem
    {
        #region Static

        //private static List<RibbonDropDown> registeredDds = new List<RibbonDropDown>();

        //private static void RegisterDropDown(RibbonDropDown dropDown)
        //{
        //    registeredDds.Add(dropDown);
        //}

        //private static void UnregisterDropDown(RibbonDropDown dropDown)
        //{
        //    registeredDds.Remove(dropDown);
        //}

        //internal static void DismissAll()
        //{
        //    for (int i = 0; i < registeredDds.Count; i++)
        //    {

        //        registeredDds[i].Close();
        //    }

        //    registeredDds.Clear();
        //}

        ///// <summary>
        ///// Closes all the dropdowns before the specified dropDown
        ///// </summary>
        ///// <param name="dropDown"></param>
        //internal static void DismissTo(RibbonDropDown dropDown)
        //{
        //    if (dropDown == null) throw new ArgumentNullException("dropDown");

        //    for (int i = registeredDds.Count - 1; i >= 0; i--)
        //    {
        //        if (i >= registeredDds.Count)
        //        {
        //            break;
        //        }

        //        if (registeredDds[i].Equals(dropDown))
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            registeredDds[i].Close();
        //        }
        //    }
        //}

        #endregion

        #region Fields

        private bool _showSizingGrip;
        private bool _ignoreNext;
        private bool _resizing;
        private Point _resizeOrigin;
        private Size _resizeSize;

        //scroll properties
        private Rectangle _thumbBounds;
        private Rectangle _fullContentBounds;
        private int _scrollValue;
        private bool _avoidNextThumbMeasure;
        private int _jumpDownSize;
        private int _jumpUpSize;
        private int _offset;
        private int _thumbOffset;

        #endregion

        #region Ctor

        private RibbonDropDown()
        {
            //RegisterDropDown(this);
            DoubleBuffered = true;
            DrawIconsBar = true;
        }

        internal RibbonDropDown(RibbonItem parentItem, IEnumerable<RibbonItem> items, Ribbon ownerRibbon)
           : this(parentItem, items, ownerRibbon, RibbonElementSizeMode.DropDown)
        {
        }

        internal RibbonDropDown(RibbonItem parentItem, IEnumerable<RibbonItem> items, Ribbon ownerRibbon, RibbonElementSizeMode measuringSize)
           : this()
        {
            Items = items;
            OwnerRibbon = ownerRibbon;
            SizingGripHeight = 12;
            ParentItem = parentItem;
            Sensor = new RibbonMouseSensor(this, OwnerRibbon, items);
            MeasuringSize = measuringSize;
            ScrollBarSize = 16;

            if (Items != null)
                foreach (RibbonItem item in Items)
                {
                    item.SetSizeMode(RibbonElementSizeMode.DropDown);
                    item.SetCanvas(this);

                    //If item is a RibbonHost, the MouseSensor will not detect the mouse move event, so manually hook into the event.
                    if (item is RibbonHost)
                    {
                        ((RibbonHost)item).ClientMouseMove += OnRibbonHostMouseMove;
                    }
                }

            UpdateSize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets the maximum height in pixels for the dropdown window. Enter 0 for autosize. If the contents is larger than the window scrollbars will be shown.
        /// </summary>
        public int DropDownMaxHeight { get; set; } = 0;

        /// <summary>
        /// Gets or sets the width of the scrollbar
        /// </summary>
        public int ScrollBarSize { get; set; }

        /// <summary>
        /// Gets the control where the item is currently being drawn
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control Canvas => this;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ScrollBarBounds => Rectangle.FromLTRB(ButtonUpBounds.Left, ButtonUpBounds.Top, ButtonDownBounds.Right, ButtonDownBounds.Bottom);

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ScrollBarEnabled { get; private set; }

        /// <summary>
        /// Gets the percent of scrolled content
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ScrolledPercent
        {
            get
            {
                if (_fullContentBounds.Height > (double)ContentBounds.Height)
                    return (ContentBounds.Top - (double)_fullContentBounds.Top) /
                        (_fullContentBounds.Height - (double)ContentBounds.Height);
                return 0.0;
            }
            set
            {
                _avoidNextThumbMeasure = true;
                ScrollTo(-Convert.ToInt32((_fullContentBounds.Height - ContentBounds.Height) * value));
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollMinimum => ButtonUpBounds.Bottom;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollMaximum => ButtonDownBounds.Top - ThumbBounds.Height;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ScrollValue
        {
            get => _scrollValue;
            set
            {
                if (value > ScrollMaximum || value < ScrollMinimum)
                {
                    throw new ArgumentOutOfRangeException(nameof(ScrollValue), "Scroll value must exist between ScrollMinimum and Scroll Maximum");
                }

                _thumbBounds.Y = value;

                double scrolledPixels = value - ScrollMinimum;
                double pixelsAvailable = ScrollMaximum - ScrollMinimum;

                ScrolledPercent = scrolledPixels / pixelsAvailable;

                _scrollValue = value;
            }
        }
        /// <summary>
        /// Redraws the scroll part of the list
        /// </summary>
        private void RedrawScroll()
        {
            if (Canvas != null)
                Canvas.Invalidate(Rectangle.FromLTRB(ButtonDownBounds.X, ButtonUpBounds.Y, ButtonDownBounds.Right, ButtonDownBounds.Bottom));
        }
        /// <summary>
        /// Gets if the scrollbar thumb is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ThumbSelected { get; private set; }

        /// <summary>
        /// Gets if the scrollbar thumb is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ThumbPressed { get; private set; }

        /// <summary>
        /// Gets the bounds of the scrollbar thumb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ThumbBounds => _thumbBounds;

        /// <summary>
        /// Gets a value indicating if the button that scrolls up the content is currently enabled
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonUpEnabled { get; private set; }

        /// <summary>
        /// Gets a value indicating if the button that scrolls down the content is currently enabled
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonDownEnabled { get; private set; }

        /// <summary>
        /// Gets a vaule indicating if the button that scrolls down the content is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonDownSelected { get; private set; }

        /// <summary>
        /// Gets a vaule indicating if the button that scrolls down the content is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonDownPressed { get; private set; }

        /// <summary>
        /// Gets a vaule indicating if the button that scrolls up the content is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonUpSelected { get; private set; }

        /// <summary>
        /// Gets the bounds of the content where items are shown
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ContentBounds { get; private set; }

        /// <summary>
        /// Gets a vaule indicating if the button that scrolls up the content is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ButtonUpPressed { get; private set; }

        /// <summary>
        /// Gets the bounds of the button that scrolls the items up
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ButtonUpBounds { get; private set; }

        /// <summary>
        /// Gets the bounds of the button that scrolls the items down
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ButtonDownBounds { get; private set; }

        /// <summary>
        /// Gets or sets if the icons bar should be drawn
        /// </summary>
        public bool DrawIconsBar { get; set; }

        /// <summary>
        /// Gets or sets the selection service for the dropdown
        /// </summary>
        internal ISelectionService SelectionService { get; set; }

        /// <summary>
        /// Gets the bounds of the sizing grip
        /// </summary>
        public Rectangle SizingGripBounds { get; private set; }

        /// <summary>
        /// Gets or sets the size for measuring items (by default is DropDown)
        /// </summary>
        public RibbonElementSizeMode MeasuringSize { get; set; }

        /// <summary>
        /// Gets the parent item of this dropdown
        /// </summary>
        public RibbonItem ParentItem { get; }

        /// <summary>
        /// Gets the sennsor of this dropdown
        /// </summary>
        public RibbonMouseSensor Sensor { get; }

        /// <summary>
        /// Gets the Ribbon this DropDown belongs to
        /// </summary>
        public Ribbon OwnerRibbon { get; }

        /// <summary>
        /// Gets the RibbonItem this dropdown belongs to
        /// </summary>
        public IEnumerable<RibbonItem> Items { get; }

        /// <summary>
        /// Gets or sets a value indicating if the sizing grip should be visible
        /// </summary>
        public bool ShowSizingGrip
        {
            get => _showSizingGrip;
            set
            {
                _showSizingGrip = value;
                UpdateSize();
            }
        }
        /// <summary>
        /// Gets or sets the height of the sizing grip area
        /// </summary>
        [DefaultValue(12)]
        public int SizingGripHeight { get; set; }

        #endregion

        #region Methods
        public void SetBounds()
        {
            #region Assign grip regions
            if (ShowSizingGrip)
            {
                SizingGripBounds = Rectangle.FromLTRB(
                    ClientSize.Width - SizingGripHeight, ClientSize.Height - SizingGripHeight,
                    ClientSize.Width, ClientSize.Height);
            }
            else
            {
                SizingGripBounds = Rectangle.Empty;
            }
            #endregion

            #region Assign buttons regions
            if (ScrollBarEnabled)
            {
                int bwidth = ScrollBarSize;
                int bheight = ScrollBarSize;
                _thumbBounds.Width = ScrollBarSize;

                ButtonUpBounds = new Rectangle(Bounds.Right - bwidth - 1,
                    Bounds.Top + OwnerRibbon.DropDownMargin.Top, bwidth, bheight);

                ButtonDownBounds = new Rectangle(ButtonUpBounds.Left, Bounds.Height - bheight - SizingGripBounds.Height - OwnerRibbon.DropDownMargin.Bottom - 1,
                    bwidth, bheight);

                _thumbBounds.X = ButtonUpBounds.Left;

                ButtonUpEnabled = _offset < 0;
                if (!ButtonUpEnabled) _offset = 0;
                ButtonDownEnabled = false;
            }
            #endregion

            int scrollWidth = ScrollBarEnabled ? ScrollBarSize : 0;
            int itemsWidth = Math.Max(0, ClientSize.Width - OwnerRibbon.DropDownMargin.Horizontal - scrollWidth);

            ContentBounds = Rectangle.FromLTRB(OwnerRibbon.DropDownMargin.Left,
                OwnerRibbon.DropDownMargin.Top,
                Bounds.Right - scrollWidth - OwnerRibbon.DropDownMargin.Right,
                Bounds.Bottom - OwnerRibbon.DropDownMargin.Bottom - SizingGripBounds.Height);

            int curTop = OwnerRibbon.DropDownMargin.Top + _offset;
            int curLeft = OwnerRibbon.DropDownMargin.Left;
            int maxBottom = curTop; // int.MinValue;
            int iniTop = curTop;

            foreach (RibbonItem item in Items)
            {
                item.SetBounds(Rectangle.Empty);
            }

            foreach (RibbonItem item in Items)
            {
                curTop = maxBottom;

                item.SetBounds(new Rectangle(curLeft, curTop, itemsWidth, item.LastMeasuredSize.Height));

                //maxBottom = Math.Max(maxBottom, item.Bounds.Bottom);
                maxBottom = curTop + item.LastMeasuredSize.Height;

                //if (item.Bounds.Bottom > ContentBounds.Bottom) _buttonDownEnabled = true;

                _jumpDownSize = item.Bounds.Height;
                _jumpUpSize = item.Bounds.Height;
            }

            _fullContentBounds = Rectangle.FromLTRB(ContentBounds.Left, iniTop, ContentBounds.Right, maxBottom);

            #region Adjust thumb size

            double contentHeight = maxBottom - iniTop - 1;
            double viewHeight = Bounds.Height;

            //scrollbars?
            if (ContentBounds.Height < _fullContentBounds.Height)
            {
                double viewPercent = _fullContentBounds.Height > ContentBounds.Height ? (double)ContentBounds.Height / _fullContentBounds.Height : 0.0;
                double availHeight = ButtonDownBounds.Top - ButtonUpBounds.Bottom;
                double thumbHeight = Math.Ceiling(viewPercent * availHeight);

                if (thumbHeight < 30)
                {
                    if (availHeight >= 30)
                    {
                        thumbHeight = 30;
                    }
                    else
                    {
                        thumbHeight = availHeight;
                    }
                }
                ButtonUpEnabled = _offset < 0;
                ButtonDownEnabled = ScrollMaximum > -_offset;

                _thumbBounds.Height = Convert.ToInt32(thumbHeight);

                ScrollBarEnabled = true;

                UpdateThumbPos();
            }
            else
            {
                ScrollBarEnabled = false;
            }

            #endregion
        }

        /// <summary>
        /// Updates the position of the scroll thumb depending on the current offset
        /// </summary>
        private void UpdateThumbPos()
        {
            if (_avoidNextThumbMeasure)
            {
                _avoidNextThumbMeasure = false;
                return;
            }

            double scrolledp = ScrolledPercent;

            if (!double.IsInfinity(scrolledp))
            {
                double availSpace = ScrollMaximum - ScrollMinimum;
                double scrolledSpace = Math.Ceiling(availSpace * ScrolledPercent);

                _thumbBounds.Y = ScrollMinimum + Convert.ToInt32(scrolledSpace);
            }
            else
            {
                _thumbBounds.Y = ScrollMinimum;
            }

            if (_thumbBounds.Y > ScrollMaximum)
            {
                _thumbBounds.Y = ScrollMaximum;
            }

        }

        /// <summary>
        /// Scrolls the list down
        /// </summary>
        public void ScrollDown()
        {
            if (ScrollBarEnabled)
                ScrollOffset(-(_jumpDownSize + 1));
        }

        /// <summary>
        /// Scrolls the list up
        /// </summary>
        public void ScrollUp()
        {
            if (ScrollBarEnabled)
                ScrollOffset(_jumpUpSize + 1);
        }

        /// <summary>
        /// Pushes the amount of _offset of the top of items
        /// </summary>
        /// <param name="amount"></param>
        private void ScrollOffset(int amount)
        {
            ScrollTo(_offset + amount);
        }

        /// <summary>
        /// Scrolls the content to the specified offset
        /// </summary>
        /// <param name="offset"></param>
        private void ScrollTo(int offset)
        {
            if (ScrollBarEnabled)
            {
                int minOffset = ContentBounds.Height - _fullContentBounds.Height;

                if (offset < minOffset)
                {
                    offset = minOffset;
                }

                _offset = offset;
                SetBounds();
                Invalidate();
            }
        }

        /// <summary>
        /// Prevents the form from being hidden the next time the mouse clicks on the form.
        /// It is useful for reacting to clicks of items inside items.
        /// </summary>
        public void IgnoreNextClickDeactivation()
        {
            _ignoreNext = true;
        }

        /// <summary>
        /// Updates the size of the dropdown
        /// </summary>
        private void UpdateSize()
        {
            int heightSum = OwnerRibbon.DropDownMargin.Vertical;
            int maxWidth = 0;
            int scrollableHeight = 0;
            using (Graphics g = CreateGraphics())
            {
                foreach (RibbonItem item in Items)
                {
                    Size s = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, MeasuringSize));

                    heightSum += s.Height;
                    maxWidth = Math.Max(maxWidth, s.Width + OwnerRibbon.DropDownMargin.Horizontal);

                    if (item is IScrollableRibbonItem)
                    {
                        scrollableHeight += s.Height;
                    }
                }
            }

            //This is the initial sizing of the popup window so
            //we need to add the width of the scrollbar if its needed.
            if ((DropDownMaxHeight > 0 && DropDownMaxHeight < heightSum && !_resizing) || (heightSum + (ShowSizingGrip ? SizingGripHeight + 2 : 0) + 1) > Screen.PrimaryScreen.WorkingArea.Height)
            {
                if (DropDownMaxHeight > 0)
                    heightSum = DropDownMaxHeight;
                else
                    heightSum = Screen.PrimaryScreen.WorkingArea.Height - ((ShowSizingGrip ? SizingGripHeight + 2 : 0) + 1);

                maxWidth += ScrollBarSize;
                _thumbBounds.Width = ScrollBarSize;
                ScrollBarEnabled = true;
            }

            if (!_resizing)
            {
                Size sz = new Size(maxWidth, heightSum + (ShowSizingGrip ? SizingGripHeight + 2 : 0));
                Size = sz;
            }

            if (WrappedDropDown != null)
            {
                WrappedDropDown.Size = Size;
            }

            SetBounds();
        }

        ///// <summary>
        ///// Updates the bounds of the items
        ///// </summary>
        //private void UpdateItemsBounds()
        //{
        //   SetBounds();
        //   return;
        //   int curTop = OwnerRibbon.DropDownMargin.Top;
        //   int curLeft = OwnerRibbon.DropDownMargin.Left;
        //   //Got off the patch site from logicalerror
        //   //int itemsWidth = ClientSize.Width - OwnerRibbon.DropDownMargin.Horizontal;
        //   int itemsWidth = Math.Max(0, ClientSize.Width - OwnerRibbon.DropDownMargin.Horizontal);

        //   if (ScrollBarEnabled) itemsWidth -= ScrollBarSize;

        //   int scrollableItemsHeight = 0;
        //   int nonScrollableItemsHeight = 0;
        //   int scrollableItems = 0;
        //   int scrollableItemHeight = 0;

        //   #region Measure scrollable content
        //   foreach (RibbonItem item in Items)
        //   {
        //      if (item is IScrollableRibbonItem)
        //      {
        //         scrollableItemsHeight += item.LastMeasuredSize.Height;
        //         scrollableItems++;
        //      }
        //      else
        //      {
        //         nonScrollableItemsHeight += item.LastMeasuredSize.Height;
        //      }
        //   }

        //   if (scrollableItems > 0)
        //   {
        //      //Got off the patch site from logicalerror
        //      //scrollableItemHeight = (Height - nonScrollableItemsHeight - (ShowSizingGrip ? SizingGripHeight : 0)) / scrollableItems;
        //      scrollableItemHeight = Math.Max(0, (Height - nonScrollableItemsHeight - (ShowSizingGrip ? SizingGripHeight : 0)) / scrollableItems);
        //   }

        //   #endregion

        //   foreach (RibbonItem item in Items)
        //   {
        //      if (item is IScrollableRibbonItem)
        //      {
        //         item.SetBounds(new Rectangle(curLeft, curTop, itemsWidth, scrollableItemHeight - 1));
        //      }
        //      else
        //      {
        //         item.SetBounds(new Rectangle(curLeft, curTop, itemsWidth, item.LastMeasuredSize.Height));
        //      }

        //      curTop += item.Bounds.Height;
        //   }

        //   if (ShowSizingGrip)
        //   {
        //      _sizingGripBounds = Rectangle.FromLTRB(
        //          ClientSize.Width - SizingGripHeight, ClientSize.Height - SizingGripHeight,
        //          ClientSize.Width, ClientSize.Height);
        //   }
        //   else
        //   {
        //      _sizingGripBounds = Rectangle.Empty;
        //   }
        //}

        /// <summary>
        /// Ignores deactivation of canvas if it is a volatile window
        /// </summary>
        private void IgnoreDeactivation()
        {
            if (Canvas is RibbonPanelPopup)
            {
                (Canvas as RibbonPanelPopup).IgnoreNextClickDeactivation();
            }

            if (Canvas is RibbonDropDown)
            {
                (Canvas as RibbonDropDown).IgnoreNextClickDeactivation();
            }
        }

        #region Overrides

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);

            SetBounds();
        }

        protected override void OnShowed(EventArgs e)
        {
            base.OnShowed(e);

            //Allow the Combobox and other items to contain a selected item on the dropdown.
            if (ParentItem is RibbonButton)
            {
                foreach (RibbonItem item in Items)
                {
                    item.SetSelected(false);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (Cursor == Cursors.SizeNWSE)
            {
                _resizeOrigin = new Point(e.X, e.Y);
                _resizeSize = Size;
                _resizing = true;
            }
            if (ButtonDownSelected || ButtonUpSelected)
            {
                IgnoreDeactivation();
            }

            if (ButtonDownSelected && ButtonDownEnabled)
            {
                ButtonDownPressed = true;
                ScrollDown();
            }

            if (ButtonUpSelected && ButtonUpEnabled)
            {
                ButtonUpPressed = true;
                ScrollUp();
            }

            if (ThumbSelected)
            {
                ThumbPressed = true;
                _thumbOffset = e.Y - _thumbBounds.Y;
            }

            if (
                ScrollBarBounds.Contains(e.Location) &&
                e.Y >= ButtonUpBounds.Bottom && e.Y <= ButtonDownBounds.Y &&
                !ThumbBounds.Contains(e.Location) &&
                !ButtonDownBounds.Contains(e.Location) &&
                !ButtonUpBounds.Contains(e.Location))
            {
                if (e.Y < ThumbBounds.Y)
                {
                    ScrollOffset(Bounds.Height);
                }
                else
                {
                    ScrollOffset(-Bounds.Height);
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            //Close();

            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (ShowSizingGrip && SizingGripBounds.Contains(e.X, e.Y))
            {
                Cursor = Cursors.SizeNWSE;
            }
            else if (Cursor == Cursors.SizeNWSE)
            {
                Cursor = Cursors.Default;
            }

            if (_resizing)
            {
                int dx = e.X - _resizeOrigin.X;
                int dy = e.Y - _resizeOrigin.Y;

                int w = _resizeSize.Width + dx;
                int h = _resizeSize.Height + dy;

                if (w != Width || h != Height)
                {
                    Size = new Size(w, h);
                    if (WrappedDropDown != null)
                    {
                        WrappedDropDown.Size = Size;
                    }
                    int contentHeight = Bounds.Height - OwnerRibbon.DropDownMargin.Vertical - SizingGripBounds.Height;
                    if (contentHeight < _fullContentBounds.Height)
                    {
                        ScrollBarEnabled = true;
                        if (-_offset + contentHeight > _fullContentBounds.Height)
                        {
                            _offset = contentHeight - _fullContentBounds.Height;
                        }
                    }
                    else
                    {
                        ScrollBarEnabled = false;
                    }

                    SetBounds();
                    Invalidate();
                }
            }

            if (ButtonDownPressed && ButtonDownSelected && ButtonDownEnabled)
            {
                ScrollOffset(-1);
            }

            if (ButtonUpPressed && ButtonUpSelected && ButtonUpEnabled)
            {
                ScrollOffset(1);
            }

            bool upCache = ButtonUpSelected;
            bool downCache = ButtonDownSelected;
            bool thumbCache = ThumbSelected;

            ButtonUpSelected = ButtonUpBounds.Contains(e.Location);
            ButtonDownSelected = ButtonDownBounds.Contains(e.Location);
            ThumbSelected = _thumbBounds.Contains(e.Location) && ScrollBarEnabled;

            if ((upCache != ButtonUpSelected)
                || (downCache != ButtonDownSelected)
                || (thumbCache != ThumbSelected))
            {
                Invalidate();
            }

            if (ThumbPressed)
            {
                int newval = e.Y - _thumbOffset;

                if (newval < ScrollMinimum)
                {
                    newval = ScrollMinimum;
                }
                else if (newval > ScrollMaximum)
                {
                    newval = ScrollMaximum;
                }

                ScrollValue = newval;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            ButtonDownPressed = false;
            ButtonUpPressed = false;
            ThumbPressed = false;

            if (_resizing)
            {
                _resizing = false;
                return;
            }

            if (_ignoreNext)
            {
                _ignoreNext = false;
                return;
            }

            if (RibbonDesigner.Current != null)
                Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            OwnerRibbon.Renderer.OnRenderDropDownBackground(
                new RibbonCanvasEventArgs(OwnerRibbon, e.Graphics, new Rectangle(Point.Empty, ClientSize), this, ParentItem));

            RectangleF lastClip = e.Graphics.ClipBounds;
            //if (e.ClipRectangle.Top < OwnerRibbon.DropDownMargin.Top)
            //{
            RectangleF newClip = lastClip;
            newClip.Y = OwnerRibbon.DropDownMargin.Top;
            newClip.Height = Bounds.Bottom - SizingGripBounds.Height - OwnerRibbon.DropDownMargin.Vertical;
            e.Graphics.SetClip(newClip);
            //}

            foreach (RibbonItem item in Items)
            {
                //Draw image separator for normal menu buttons and their separators
                if ((item is RibbonButton && !(item is RibbonDescriptionMenuItem)) ||
                       (item is RibbonSeparator && ((RibbonSeparator)item).DropDownWidth == RibbonSeparatorDropDownWidth.Partial))
                {
                    OwnerRibbon.Renderer.OnRenderDropDownDropDownImageSeparator(item,
                        new RibbonCanvasEventArgs(OwnerRibbon, e.Graphics, new Rectangle(Point.Empty, ClientSize), this, ParentItem));
                }

                if (item.Bounds.IntersectsWith(ContentBounds))
                    item.OnPaint(this, new RibbonElementPaintEventArgs(item.Bounds, e.Graphics, RibbonElementSizeMode.DropDown));
            }

            if (ScrollBarEnabled)
                OwnerRibbon.Renderer.OnRenderScrollbar(e.Graphics, this, OwnerRibbon);

            e.Graphics.SetClip(lastClip);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            foreach (RibbonItem item in Items)
            {
                item.SetSelected(false);
            }
        }

        private void OnRibbonHostMouseMove(object sender, MouseEventArgs e)
        {
            //Raise mouse move event from the RibbonHost control as if it were generated by the MouseSensor.
            OnMouseMove(e);
        }

        #endregion
        #endregion
    }
}