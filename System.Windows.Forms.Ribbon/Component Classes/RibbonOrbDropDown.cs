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
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class RibbonOrbDropDown
         : RibbonPopup
    {
        #region const

        private const bool DefaultAutoSizeContentButtons = true;
        private const int DefaultContentButtonsMinWidth = 150;
        private const int DefaultContentRecentItemsMinWidth = 150;

        #endregion

        #region Fields
        internal RibbonOrbMenuItem LastPoppedMenuItem;
        private Rectangle designerSelectedBounds;
        private readonly int glyphGap = 3;
        private Padding _contentMargin;
        private DateTime OpenedTime; //Steve - capture time popup was shown
        private string _recentItemsCaption;

        //private GlobalHook _keyboardHook;
        private int _contentButtonsWidth = DefaultContentButtonsMinWidth;

        #endregion

        #region Ctor

        internal RibbonOrbDropDown(Ribbon ribbon)
        {
            DoubleBuffered = true;
            Ribbon = ribbon;
            MenuItems = new RibbonOrbMenuItemCollection();
            RecentItems = new RibbonOrbRecentItemCollection();
            OptionItems = new RibbonOrbOptionButtonCollection();

            MenuItems.SetOwner(Ribbon);
            RecentItems.SetOwner(Ribbon);
            OptionItems.SetOwner(Ribbon);

            OptionItemsPadding = 6;
            Size = new Size(527, 447);
            BorderRoundness = 8;

            //if (!(Site != null && Site.DesignMode))
            //{
            //   _keyboardHook = new GlobalHook(GlobalHook.HookTypes.Keyboard);
            //   _keyboardHook.KeyUp += new KeyEventHandler(_keyboardHook_KeyUp);
            //}
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (Sensor != null && !Sensor.Disposed)
            {
                Sensor.Dispose();
            }
            //if (_keyboardHook != null)
            //{
            //   _keyboardHook.Dispose();
            //}
        }
        //Finalize is called by base class "System.ComponentModel.Component"

        #endregion

        #region Props

        /// <summary>
        /// Gets all items involved in the dropdown
        /// </summary>
        internal List<RibbonItem> AllItems
        {
            get
            {
                List<RibbonItem> lst = new List<RibbonItem>();
                lst.AddRange(MenuItems); lst.AddRange(RecentItems); lst.AddRange(OptionItems);
                return lst;
            }
        }

        /// <summary>
        /// Gets the margin of the content bounds
        /// </summary>
        [Browsable(false)]
        public Padding ContentMargin
        {
            get
            {
                if (_contentMargin.Size.IsEmpty)
                {
                    _contentMargin = new Padding(6, 17, 6, 29);
                }

                return _contentMargin;
            }
        }

        /// <summary>
        /// Gets the bounds of the content (where menu buttons are)
        /// </summary>
        [Browsable(false)]
        public Rectangle ContentBounds => Rectangle.FromLTRB(ContentMargin.Left, ContentMargin.Top,
            ClientRectangle.Right - ContentMargin.Right,
            ClientRectangle.Bottom - ContentMargin.Bottom);

        /// <summary>
        /// Gets the bounds of the content part that contains the buttons on the left
        /// </summary>
        [Browsable(false)]
        public Rectangle ContentButtonsBounds
        {
            get
            {
                Rectangle r = ContentBounds;
                r.Width = _contentButtonsWidth;
                if (Ribbon.RightToLeft == RightToLeft.Yes)
                    r.X = ContentBounds.Right - _contentButtonsWidth;
                return r;
            }
        }

        /// <summary>
        /// Gets or sets the minimum width for the content buttons.
        /// </summary>
        [DefaultValue(DefaultContentButtonsMinWidth)]
        public int ContentButtonsMinWidth { get; set; } = DefaultContentButtonsMinWidth;

        /// <summary>
        /// Gets the bounds fo the content part that contains the recent-item list
        /// </summary>
        [Browsable(false)]
        public Rectangle ContentRecentItemsBounds
        {
            get
            {
                Rectangle r = ContentBounds;
                r.Width -= _contentButtonsWidth;

                //Steve - Recent Items Caption
                r.Height -= ContentRecentItemsCaptionBounds.Height;
                r.Y += ContentRecentItemsCaptionBounds.Height;

                if (Ribbon.RightToLeft == RightToLeft.No)
                    r.X += _contentButtonsWidth;

                return r;
            }
        }
        /// <summary>
        /// Gets the bounds of the caption area on the content part of the recent-item list
        /// </summary>
        [Browsable(false)]
        public Rectangle ContentRecentItemsCaptionBounds
        {
            get
            {
                if (RecentItemsCaption != null)
                {
                    //Lets measure the height of the text so we take into account the font and its size
                    SizeF cs;
                    using (Graphics g = CreateGraphics())
                    {
                        cs = g.MeasureString(RecentItemsCaption, Ribbon.RibbonTabFont);
                    }
                    Rectangle r = ContentBounds;
                    r.Width -= _contentButtonsWidth;
                    r.Height = Convert.ToInt32(cs.Height) + Ribbon.ItemMargin.Top + Ribbon.ItemMargin.Bottom; //padding
                    r.Height += RecentItemsCaptionLineSpacing; //Spacing for the divider line

                    if (Ribbon.RightToLeft == RightToLeft.No)
                        r.X += _contentButtonsWidth;
                    return r;
                }

                return Rectangle.Empty;
            }
        }

        /// <summary>
        /// Gets the bounds of the caption area on the content part of the recent-item list
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RecentItemsCaptionLineSpacing { get; } = 8;

        /// <summary>
        /// Gets or sets the minimum width for the recent items.
        /// </summary>
        [DefaultValue(DefaultContentRecentItemsMinWidth)]
        public int ContentRecentItemsMinWidth { get; set; } = DefaultContentRecentItemsMinWidth;

        /// <summary>
        /// Gets if currently on design mode
        /// </summary>
        private bool RibbonInDesignMode => RibbonDesigner.Current != null;

        /// <summary>
        /// Gets the collection of items shown in the menu area
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonOrbMenuItemCollection MenuItems { get; }

        /// <summary>
        /// Gets the collection of items shown in the options area (bottom)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonOrbOptionButtonCollection OptionItems { get; }

        [DefaultValue(6)]
        [Description("Spacing between option buttons (those on the bottom)")]
        public int OptionItemsPadding { get; set; }

        /// <summary>
        /// Gets the collection of items shown in the recent items area
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonOrbRecentItemCollection RecentItems { get; }

        /// <summary>
        /// Gets or Sets the caption for the Recent Items area
        /// </summary>
        [DefaultValue(null)]
        public string RecentItemsCaption
        {
            get => _recentItemsCaption;
            set { _recentItemsCaption = value; Invalidate(); }
        }

        /// <summary>
        /// Gets the ribbon that owns this dropdown
        /// </summary>
        [Browsable(false)]
        public Ribbon Ribbon { get; }

        /// <summary>
        /// Gets the sensor of the dropdown
        /// </summary>
        [Browsable(false)]
        public RibbonMouseSensor Sensor { get; private set; }

        /// <summary>
        /// Gets the bounds of the glyph
        /// </summary>
        internal Rectangle ButtonsGlyphBounds
        {
            get
            {
                Size s = new Size(50, 18);
                Rectangle rf = ContentButtonsBounds;
                Rectangle r = new Rectangle(rf.Left + (rf.Width - s.Width * 2) / 2, rf.Top + glyphGap, s.Width, s.Height);

                if (MenuItems.Count > 0)
                {
                    r.Y = MenuItems[MenuItems.Count - 1].Bounds.Bottom + glyphGap;
                }

                return r;
            }
        }

        /// <summary>
        /// Gets the bounds of the glyph
        /// </summary>
        internal Rectangle ButtonsSeparatorGlyphBounds
        {
            get
            {
                Size s = new Size(18, 18);

                Rectangle r = ButtonsGlyphBounds;

                r.X = r.Right + glyphGap;

                return r;
            }
        }

        /// <summary>
        /// Gets the bounds of the recent items add glyph
        /// </summary>
        internal Rectangle RecentGlyphBounds
        {
            get
            {
                Size s = new Size(50, 18);
                Rectangle rf = ContentRecentItemsBounds;
                Rectangle r = new Rectangle(rf.Left + glyphGap, rf.Top + glyphGap, s.Width, s.Height);

                if (RecentItems.Count > 0)
                {
                    r.Y = RecentItems[RecentItems.Count - 1].Bounds.Bottom + glyphGap;
                }

                return r;
            }
        }

        /// <summary>
        /// Gets the bounds of the option items add glyph
        /// </summary>
        internal Rectangle OptionGlyphBounds
        {
            get
            {
                Size s = new Size(50, 18);
                Rectangle rf = ContentBounds;
                Rectangle r = new Rectangle(rf.Right - s.Width, rf.Bottom + glyphGap, s.Width, s.Height);

                if (OptionItems.Count > 0)
                {
                    r.X = OptionItems[OptionItems.Count - 1].Bounds.Left - s.Width - glyphGap;
                }

                return r;
            }
        }

        [DefaultValue(DefaultAutoSizeContentButtons)]
        public bool AutoSizeContentButtons { get; set; } = DefaultAutoSizeContentButtons;

        #endregion

        #region Methods

        internal void HandleDesignerItemRemoved(RibbonItem item)
        {
            if (MenuItems.Contains(item))
            {
                MenuItems.Remove(item);
            }
            else if (RecentItems.Contains(item))
            {
                RecentItems.Remove(item);
            }
            else if (OptionItems.Contains(item))
            {
                OptionItems.Remove(item);
            }

            OnRegionsChanged();
        }

        /// <summary>
        /// Gets the height that a separator should be on the DropDown
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int SeparatorHeight(RibbonSeparator s)
        {
            if (!string.IsNullOrEmpty(s.Text))
            {
                return 20;
            }

            return 3;
        }

        /// <summary>
        /// Updates the regions and bounds of items
        /// </summary>
        private void UpdateRegions()
        {
            int curtop = 0;
            int curright = 0;
            int menuItemHeight = 44;
            int recentHeight = 22;
            int mbuttons = 1; //margin
            int mrecent = 1;  //margin
            int buttonsHeight = 0;
            int recentsHeight = 0;

            if (AutoSizeContentButtons)
            {
                #region important to do the item max width check before the ContentBounds and other stuff is used (internal Property stuff)
                int itemMaxWidth = 0;
                using (Graphics g = CreateGraphics())
                {
                    foreach (RibbonItem item in MenuItems)
                    {
                        int width = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.DropDown)).Width;
                        if (width > itemMaxWidth)
                            itemMaxWidth = width;
                    }
                }
                itemMaxWidth = Math.Min(itemMaxWidth, ContentBounds.Width - ContentRecentItemsMinWidth);
                itemMaxWidth = Math.Max(itemMaxWidth, ContentButtonsMinWidth);
                _contentButtonsWidth = itemMaxWidth;
                #endregion
            }

            Rectangle rcontent = ContentBounds;
            Rectangle rbuttons = ContentButtonsBounds;
            Rectangle rrecent = ContentRecentItemsBounds;

            foreach (RibbonItem item in AllItems)
            {
                item.SetSizeMode(RibbonElementSizeMode.DropDown);
                item.SetCanvas(this);
            }

            #region Menu Items

            curtop = rcontent.Top + 1;

            foreach (RibbonItem item in MenuItems)
            {
                Rectangle ritem = new Rectangle(rbuttons.Left + mbuttons, curtop, rbuttons.Width - mbuttons * 2, menuItemHeight);

                if (item is RibbonSeparator) ritem.Height = SeparatorHeight(item as RibbonSeparator);

                item.SetBounds(ritem);

                curtop += ritem.Height;
            }

            buttonsHeight = curtop - rcontent.Top + 1;

            #endregion

            #region Recent List

            //curtop = rbuttons.Top; //Steve - for recent documents
            curtop = rrecent.Top; //Steve - for recent documents

            foreach (RibbonItem item in RecentItems)
            {
                Rectangle ritem = new Rectangle(rrecent.Left + mrecent, curtop, rrecent.Width - mrecent * 2, recentHeight);

                if (item is RibbonSeparator) ritem.Height = SeparatorHeight(item as RibbonSeparator);

                item.SetBounds(ritem);

                curtop += ritem.Height;
            }

            recentsHeight = curtop - rbuttons.Top;

            #endregion

            #region Set size

            int actualHeight = Math.Max(buttonsHeight, recentsHeight);

            if (RibbonDesigner.Current != null)
            {
                actualHeight += ButtonsGlyphBounds.Height + glyphGap * 2;
            }

            Height = actualHeight + ContentMargin.Vertical;
            rcontent = ContentBounds;

            #endregion

            #region Option buttons

            curright = ClientSize.Width - ContentMargin.Right;

            using (Graphics g = CreateGraphics())
            {
                foreach (RibbonItem item in OptionItems)
                {
                    Size s = item.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.DropDown));
                    curtop = rcontent.Bottom + (ContentMargin.Bottom - s.Height) / 2;
                    item.SetBounds(new Rectangle(new Point(curright - s.Width, curtop), s));
                    curright = item.Bounds.Left - OptionItemsPadding;
                }
            }

            #endregion
        }

        /// <summary>
        /// Refreshes the sensor
        /// </summary>
        private void UpdateSensor()
        {
            if (Sensor != null && !Sensor.Disposed)
            {
                Sensor.Dispose();
            }

            Sensor = new RibbonMouseSensor(this, Ribbon, AllItems);
        }

        /// <summary>
        /// Updates all areas and bounds of items
        /// </summary>
        internal void OnRegionsChanged()
        {
            UpdateRegions();
            UpdateSensor();
            UpdateDesignerSelectedBounds();
            Invalidate();
        }

        /// <summary>
        /// Selects the specified item on the designer
        /// </summary>
        /// <param name="item"></param>
        internal void SelectOnDesigner(RibbonItem item)
        {
            if (RibbonDesigner.Current != null)
            {
                RibbonDesigner.Current.SelectedElement = item;
                UpdateDesignerSelectedBounds();
                Invalidate();
            }
        }

        /// <summary>
        /// Updates the selection bounds on the designer
        /// </summary>
        internal void UpdateDesignerSelectedBounds()
        {
            designerSelectedBounds = Rectangle.Empty;

            if (RibbonInDesignMode)
            {
                RibbonItem item = RibbonDesigner.Current.SelectedElement as RibbonItem;

                if (item != null && AllItems.Contains(item))
                {
                    designerSelectedBounds = item.Bounds;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (RibbonInDesignMode)
            {
                #region DesignMode clicks
                if (ContentBounds.Contains(e.Location))
                {
                    if (ContentButtonsBounds.Contains(e.Location))
                    {
                        foreach (RibbonItem item in MenuItems)
                        {
                            if (item.Bounds.Contains(e.Location))
                            {
                                SelectOnDesigner(item);
                                break;
                            }
                        }
                    }
                    else if (ContentRecentItemsBounds.Contains(e.Location))
                    {
                        foreach (RibbonItem item in RecentItems)
                        {
                            if (item.Bounds.Contains(e.Location))
                            {
                                SelectOnDesigner(item);
                                break;
                            }
                        }
                    }
                }
                if (ButtonsGlyphBounds.Contains(e.Location))
                {
                    RibbonDesigner.Current.CreateOrbMenuItem(typeof(RibbonOrbMenuItem));
                }
                else if (ButtonsSeparatorGlyphBounds.Contains(e.Location))
                {
                    RibbonDesigner.Current.CreateOrbMenuItem(typeof(RibbonSeparator));
                }
                else if (RecentGlyphBounds.Contains(e.Location))
                {
                    RibbonDesigner.Current.CreateOrbRecentItem(typeof(RibbonOrbRecentItem));
                }
                else if (OptionGlyphBounds.Contains(e.Location))
                {
                    RibbonDesigner.Current.CreateOrbOptionItem(typeof(RibbonOrbOptionButton));
                }
                else
                {
                    foreach (RibbonItem item in OptionItems)
                    {
                        if (item.Bounds.Contains(e.Location))
                        {
                            SelectOnDesigner(item);
                            break;
                        }
                    }
                }
                #endregion
            }

        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);

            UpdateRegions();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Ribbon.Renderer.OnRenderOrbDropDownBackground(
                 new RibbonOrbDropDownEventArgs(Ribbon, this, e.Graphics, e.ClipRectangle));

            foreach (RibbonItem item in AllItems)
            {
                item.OnPaint(this, new RibbonElementPaintEventArgs(e.ClipRectangle, e.Graphics, RibbonElementSizeMode.DropDown));
            }

            if (RibbonInDesignMode)
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(50, Color.Blue)))
                {
                    e.Graphics.FillRectangle(b, ButtonsGlyphBounds);
                    e.Graphics.FillRectangle(b, RecentGlyphBounds);
                    e.Graphics.FillRectangle(b, OptionGlyphBounds);
                    e.Graphics.FillRectangle(b, ButtonsSeparatorGlyphBounds);
                }

                using (StringFormat sf = StringFormatFactory.Center(StringTrimming.None))
                {
                    e.Graphics.DrawString("+", Font, Brushes.White, ButtonsGlyphBounds, sf);
                    e.Graphics.DrawString("+", Font, Brushes.White, RecentGlyphBounds, sf);
                    e.Graphics.DrawString("+", Font, Brushes.White, OptionGlyphBounds, sf);
                    e.Graphics.DrawString("---", Font, Brushes.White, ButtonsSeparatorGlyphBounds, sf);
                }

                using (Pen p = new Pen(Color.Black))
                {
                    p.DashStyle = DashStyle.Dot;
                    e.Graphics.DrawRectangle(p, designerSelectedBounds);
                }

                //e.Graphics.DrawString("Press ESC to Hide", Font, Brushes.Black, Width - 100f, 2f);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Ribbon.OrbPressed = false;
            Ribbon.OrbSelected = false;
            LastPoppedMenuItem = null;
            foreach (RibbonItem item in AllItems)
            {
                item.SetSelected(false);
                item.SetPressed(false);
            }
            base.OnClosed(e);
        }

        protected override void OnShowed(EventArgs e)
        {
            base.OnShowed(e);
            OpenedTime = DateTime.Now;
            UpdateSensor();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (Ribbon.RectangleToScreen(Ribbon.OrbBounds).Contains(PointToScreen(e.Location)))
            {
                Ribbon.OnOrbClicked(EventArgs.Empty);
                //Steve - if click time is within the double click time after the drop down was shown, then this is a double click
                if (DateTime.Compare(DateTime.Now, OpenedTime.AddMilliseconds(SystemInformation.DoubleClickTime)) < 0)
                    Ribbon.OnOrbDoubleClicked(EventArgs.Empty);
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (Ribbon.RectangleToScreen(Ribbon.OrbBounds).Contains(PointToScreen(e.Location)))
            {
                Ribbon.OnOrbDoubleClicked(EventArgs.Empty);
            }
        }

        private void _keyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            //base.OnKeyUp(e);
            if (e.KeyCode == Keys.Down)
            {
                RibbonItem NextItem = null;
                RibbonItem SelectedItem = null;
                foreach (RibbonItem itm in MenuItems)
                {
                    if (itm.Selected)
                    {
                        SelectedItem = itm;
                        break;
                    }
                }
                if (SelectedItem != null)
                {
                    //get the next item in the chain
                    int Index = MenuItems.IndexOf(SelectedItem);
                    NextItem = GetNextSelectableMenuItem(Index + 1);
                }
                else
                {
                    //nothing found so lets search through the recent buttons
                    foreach (RibbonItem itm in RecentItems)
                    {
                        if (itm.Selected)
                        {
                            SelectedItem = itm;
                            itm.SetSelected(false);
                            itm.RedrawItem();
                            break;
                        }
                    }
                    if (SelectedItem != null)
                    {
                        //get the next item in the chain
                        int Index = RecentItems.IndexOf(SelectedItem);
                        NextItem = GetNextSelectableRecentItem(Index + 1);
                    }
                    else
                    {
                        //nothing found so lets search through the option buttons
                        foreach (RibbonItem itm in OptionItems)
                        {
                            if (itm.Selected)
                            {
                                SelectedItem = itm;
                                itm.SetSelected(false);
                                itm.RedrawItem();
                                break;
                            }
                        }
                        if (SelectedItem != null)
                        {
                            //get the next item in the chain
                            int Index = OptionItems.IndexOf(SelectedItem);
                            NextItem = GetNextSelectableOptionItem(Index + 1);
                        }
                    }
                }
                //last check to make sure we found a selected item
                if (SelectedItem == null)
                {
                    //we should have the right item by now so lets select it
                    NextItem = GetNextSelectableMenuItem(0);
                    if (NextItem != null)
                    {
                        NextItem.SetSelected(true);
                        NextItem.RedrawItem();
                    }
                }
                else
                {
                    SelectedItem.SetSelected(false);
                    SelectedItem.RedrawItem();

                    NextItem.SetSelected(true);
                    NextItem.RedrawItem();
                }
                //_sensor.SelectedItem = NextItem;
                //_sensor.HittedItem = NextItem;
            }
            else if (e.KeyCode == Keys.Up)
            {
            }
        }
        private RibbonItem GetNextSelectableMenuItem(int StartIndex)
        {
            for (int idx = StartIndex; idx < MenuItems.Count; idx++)
            {
                RibbonButton btn = MenuItems[idx] as RibbonButton;
                if (btn != null)
                    return btn;
            }
            //nothing found so lets move on to the recent items
            RibbonItem NextItem = GetNextSelectableRecentItem(0);
            if (NextItem == null)
            {
                //nothing found so lets try the option items
                NextItem = GetNextSelectableOptionItem(0);
                if (NextItem == null)
                {
                    //nothing again so go back to the top of the menu items
                    NextItem = GetNextSelectableMenuItem(0);
                }
            }
            return NextItem;
        }
        private RibbonItem GetNextSelectableRecentItem(int StartIndex)
        {
            for (int idx = StartIndex; idx < RecentItems.Count; idx++)
            {
                RibbonButton btn = RecentItems[idx] as RibbonButton;
                if (btn != null)
                    return btn;
            }
            //nothing found so lets move on to the option items
            RibbonItem NextItem = GetNextSelectableOptionItem(0);
            if (NextItem == null)
            {
                //nothing found so lets try the menu items
                NextItem = GetNextSelectableMenuItem(0);
                if (NextItem == null)
                {
                    //nothing again so go back to the top of the recent items
                    NextItem = GetNextSelectableRecentItem(0);
                }
            }
            return NextItem;
        }
        private RibbonItem GetNextSelectableOptionItem(int StartIndex)
        {
            for (int idx = StartIndex; idx < OptionItems.Count; idx++)
            {
                RibbonButton btn = OptionItems[idx] as RibbonButton;
                if (btn != null)
                    return btn;
            }
            //nothing found so lets move on to the menu items
            RibbonItem NextItem = GetNextSelectableMenuItem(0);
            if (NextItem == null)
            {
                //nothing found so lets try the recent items
                NextItem = GetNextSelectableRecentItem(0);
                if (NextItem == null)
                {
                    //nothing again so go back to the top of the option items
                    NextItem = GetNextSelectableOptionItem(0);
                }
            }
            return NextItem;
        }
        #endregion

    }
}
