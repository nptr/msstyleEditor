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
    /// Represents a tab that can contain RibbonPanel objects
    /// </summary>
    [DesignTimeVisible(false)]
    [Designer(typeof(RibbonTabDesigner))]
    public class RibbonTab : Component, IRibbonElement, IRibbonToolTip, IContainsRibbonComponents
    {
        #region Fields
        private bool? _isopeninvisualstudiodesigner;
        private bool _enabled;
        private bool _pressed;
        private bool _selected;
        private bool _active;
        private string _text;
        private RibbonContext _context;
        private int _offset;
        private bool _visible = true;

        private readonly RibbonToolTip _TT;

        /// <summary>
		/// Occurs when the mouse pointer enters the panel
		/// </summary>
		public event MouseEventHandler MouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves the panel
        /// </summary>
        public event MouseEventHandler MouseLeave;

        /// <summary>
        /// Occurs when the mouse pointer is moved inside the panel
        /// </summary>
        public event MouseEventHandler MouseMove;

        #endregion

        #region Ctor

        public RibbonTab()
        {
            Panels = new RibbonPanelCollection(this);
            _enabled = true;

            //Initialize the ToolTip for this Item
            _TT = new RibbonToolTip(this)
            {
                InitialDelay = 100,
                AutomaticDelay = 800,
                AutoPopDelay = 8000,
                UseAnimation = true,
                Active = false
            };
            _TT.Popup += _TT_Popup;
        }

        public RibbonTab(string text)
           : this()
        {
            _text = text;
        }

        /// <summary>
        /// Creates a new RibbonTab
        /// </summary>
        [Obsolete("Use 'public RibbonTab(string text)' instead!")]
        public RibbonTab(Ribbon owner, string text)
           : this(text)
        {
        }

        /// <summary>
        /// RibbonTab is open in Visual Studio Designer
        /// </summary>
        protected bool IsOpenInVisualStudioDesigner()
        {
            if (!_isopeninvisualstudiodesigner.HasValue)
            {
                _isopeninvisualstudiodesigner = LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                                                this.DesignMode;
                if (!_isopeninvisualstudiodesigner.Value)
                {
                    try
                    {
                        using (var process = System.Diagnostics.Process.GetCurrentProcess())
                        {
                            _isopeninvisualstudiodesigner = process.ProcessName.ToLowerInvariant().Contains("devenv");
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return _isopeninvisualstudiodesigner.Value;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && RibbonDesigner.Current == null)
            {
                // ADDED
                _TT.Popup -= _TT_Popup;

                _TT.Dispose();
                try
                {
                    foreach (RibbonPanel p in Panels)
                        p.Dispose();
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

        #region Events


        public event EventHandler ScrollRightVisibleChanged;
        public event EventHandler ScrollRightPressedChanged;
        public event EventHandler ScrollRightBoundsChanged;
        public event EventHandler ScrollRightSelectedChanged;
        public event EventHandler ScrollLeftVisibleChanged;
        public event EventHandler ScrollLeftPressedChanged;
        public event EventHandler ScrollLeftSelectedChanged;
        public event EventHandler ScrollLeftBoundsChanged;
        public event EventHandler TabBoundsChanged;
        public event EventHandler TabContentBoundsChanged;
        public event EventHandler OwnerChanged;
        public event EventHandler PressedChanged;
        public event EventHandler ActiveChanged;
        public event EventHandler TextChanged;
        public event EventHandler ContextChanged;
        public virtual event RibbonElementPopupEventHandler ToolTipPopUp;

        #endregion

        #region Props

        private string _Name = string.Empty;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual string Name
        {
            get
            {
                if (Site != null)
                {
                    _Name = Site.Name;
                }
                return _Name;
            }
            set => _Name = value;
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Sets if the tab should be enabled")]
        public bool Enabled
        {
            get
            {
                if (Owner != null)
                {
                    return _enabled && Owner.Enabled;
                }

                return _enabled;
            }
            set
            {
                _enabled = value;
                Owner.Invalidate();

                foreach (RibbonPanel item in Panels)
                {
                    item.Enabled = value;
                }
            }
        }

        /// <summary>
        /// Gets if the right-side scroll button is currently visible
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollRightVisible { get; private set; }

        /// <summary>
		/// Gets if the right-side scroll button is currently selected
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollRightSelected { get; private set; }

        /// <summary>
		/// Gets if the right-side scroll button is currently pressed
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollRightPressed { get; private set; }

        /// <summary>
		/// Gets if the right-side scroll button bounds
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Rectangle ScrollRightBounds { get; private set; }

        /// <summary>
		/// Gets if the left scroll button is currently visible
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        public bool ScrollLeftVisible { get; private set; }

        /// <summary>
		/// Gets if the left scroll button bounds
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Rectangle ScrollLeftBounds { get; private set; }

        /// <summary>
		/// Gets if the left scroll button is currently selected
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollLeftSelected { get; private set; }

        /// <summary>
		/// Gets if the left scroll button is currently pressed
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool ScrollLeftPressed { get; private set; }

        /// <summary>
		/// Gets the <see cref="TabBounds"/> property value
		/// </summary>
		[Browsable(false)]
        public Rectangle Bounds => TabBounds;

        /// <summary>
		/// Gets the collection of panels that belong to this tab
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonPanelCollection Panels { get; }

        /// <summary>
		/// Gets the bounds of the little tab showing the text
		/// </summary>
		[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle TabBounds { get; private set; }

        /// <summary>
		/// Gets the bounds of the tab content on the Ribbon
		/// </summary>
		[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle TabContentBounds { get; private set; }

        /// <summary>
		/// Gets the Ribbon that contains this tab
		/// </summary>
		[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        /// <summary>
		/// Gets a value indicating whether the state of the tab is being pressed by the mouse or a key
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Pressed => _pressed;

        /// <summary>
		/// Gets a value indicating whether the tab is selected
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Selected => _selected;

        /// <summary>
		/// Gets a value indicating if the tab is currently the active tab
		/// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Active => _active;

        /// <summary>
        /// Gets or sets the object that contains data about the control
        /// </summary>
        [Description("An Object field for associating custom data for this control")]
        [DefaultValue(null)]
        [Category("Data")]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        /// <summary>
		/// Gets or sets the custom string data associated with this control
		/// </summary>
		[DefaultValue(null)]
        [Category("Data")]
        [Description("A string field for associating custom data for this control")]
        public string Value { get; set; }

        /// <summary>
		/// Gets or sets the key combination that activates this element when the Alt key was pressed
		/// </summary>
        [Category("Behavior")]
        [DefaultValue(null)]
        public string AltKey { get; set; }


        /// <summary>
		/// Gets or sets the text that is to be displayed on the tab
		/// </summary>
		[Localizable(true)]
        [Category("Appearance")]
        public string Text
        {
            get => _text;
            set
            {

                _text = value;

                OnTextChanged(EventArgs.Empty);

                if (Owner != null) Owner.OnRegionsChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tab is attached to a  Context
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Contextual => _context != null;

        /// <summary>
		/// Gets or sets the context this tab belongs to
		/// </summary>
		/// <remarks>Tabs on a context are highlighted with a special glow color</remarks>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public RibbonContext Context
        {
            get => _context;
            set
            {
                _context = value;

                OnContextChanged(EventArgs.Empty);

                if (Owner != null)
                {
                    Owner.OnRegionsChanged();
                    Owner.UpdateRegions();
                }
            }
        }

        /// <summary>
        /// Gets or sets the visibility of this tab
        /// </summary>
        /// <remarks>Tabs on a context are highlighted with a special glow color</remarks>
        [Category("Behavior")]
        [Localizable(true)]
        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                if (Owner != null && !Owner.IsDesignMode() && !Owner.Visible)
                    return false;

                //If this tab has a context, follow the visibility of the context.
                return (Contextual) ? Context.Visible : _visible;
            }
            set
            {
                _visible = value;
                if (Owner != null)
                {
                    Owner.UpdateRegions();
                    if (Active)
                    {
                        EnsureAnyTabVisible();
                    }
                    else
                    {
                        Owner.Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the tool tip title
        /// </summary>
        [DefaultValue("")]
        public string ToolTipTitle
        {
            get => _TT.ToolTipTitle;
            set => _TT.ToolTipTitle = value;
        }

        /// <summary>
        /// Gets or sets the image of the tool tip
        /// </summary>
        [DefaultValue(ToolTipIcon.None)]
        public ToolTipIcon ToolTipIcon
        {
            get => _TT.ToolTipIcon;
            set => _TT.ToolTipIcon = value;
        }

        /// <summary>
        /// Gets or sets the tool tip text
        /// </summary>
        [DefaultValue(null)]
        [Localizable(true)]
        public string ToolTip { get; set; }

        /// <summary>
		/// Gets or sets the tool tip image
		/// </summary>
		[DefaultValue(null)]
        [Localizable(true)]
        public Image ToolTipImage
        {
            get => _TT.ToolTipImage;
            set => _TT.ToolTipImage = value;
        }

        /// <summary>
        /// Gets whether the tab should be drawn invisible.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool Invisible => Owner != null && Owner.HideSingleTabIfTextEmpty && Owner.Tabs.Count == 1 && string.IsNullOrEmpty(Text);

        #endregion

        #region IRibbonElement Members

        public void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (Owner == null) return;

            Owner.Renderer.OnRenderRibbonTab(new RibbonTabRenderEventArgs(Owner, e.Graphics, e.Clip, this));
            Owner.Renderer.OnRenderRibbonTabText(new RibbonTabRenderEventArgs(Owner, e.Graphics, e.Clip, this));

            if (Active && (!Owner.Minimized || (Owner.Minimized && Owner.Expanded)))
            {
                int displayIndex = 0;
                foreach (RibbonPanel panel in Panels)
                {
                    if (panel.Visible)
                    {
                        panel.Index = displayIndex;
                        panel.IsFirstPanel = (displayIndex == 0);

                        panel.OnPaint(this, new RibbonElementPaintEventArgs(e.Clip, e.Graphics, panel.SizeMode, e.Control));

                        displayIndex++;
                    }
                }

                foreach (RibbonPanel panel in Panels)
                {
                    if (panel.Visible)
                    {
                        panel.IsLastPanel = (panel.Index == displayIndex - 1);
                        break;
                    }
                }
            }

            Owner.Renderer.OnRenderTabScrollButtons(new RibbonTabRenderEventArgs(Owner, e.Graphics, e.Clip, this));
        }

        /// <summary>
        /// This method is not relevant for this class
        /// </summary>
        /// <exception cref="NotSupportedException">Always</exception>
        public void SetBounds(Rectangle bounds)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets or sets the context this tab belongs to
        /// </summary>
        /// <remarks>Tabs on a context are highlighted with a special glow color</remarks>
        /// <param name="context"></param>
		public void SetContext(RibbonContext context)
        {
            bool trigger = !context.Equals(context);

            if (trigger)
                OnContextChanged(EventArgs.Empty);

            _context = context;
        }

        /// <summary>
        /// Measures the size of the tab. The tab content bounds is measured by the Ribbon control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!Visible && !Owner.IsDesignMode()) return new Size(0, 0);

            return Size.Ceiling(e.Graphics.MeasureString(string.IsNullOrEmpty(Text) ? " " : Text, Owner.Font));
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            Owner = owner;

            Panels.SetOwner(owner);

            OnOwnerChanged(EventArgs.Empty);
        }

        /// <summary>
        /// When an item is removed from the RibbonItemCollection remove all its references.
        /// </summary>
        internal virtual void ClearOwner()
        {
            Owner = null;
            OnOwnerChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the Pressed property
        /// </summary>
        /// <param name="pressed">Value that indicates if the element is pressed</param>
        internal void SetPressed(bool pressed)
        {
            _pressed = pressed;

            OnPressedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the Selected property
        /// </summary>
        /// <param name="selected">Value that indicates if the element is selected</param>
        internal void SetSelected(bool selected)
        {
            _selected = selected;

            if (selected)
            {
                OnMouseEnter(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
            }
            else
            {
                OnMouseLeave(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
            }
        }

        #endregion

        #region Method Triggers

        public void OnContextChanged(EventArgs e)
        {
            if (ContextChanged != null)
            {
                ContextChanged(this, e);
            }
        }

        public void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        public void OnActiveChanged(EventArgs e)
        {
            if (ActiveChanged != null)
            {
                ActiveChanged(this, e);
            }
        }

        public void OnPressedChanged(EventArgs e)
        {
            if (PressedChanged != null)
            {
                PressedChanged(this, e);
            }
        }

        public void OnOwnerChanged(EventArgs e)
        {
            if (OwnerChanged != null)
            {
                OwnerChanged(this, e);
            }
        }

        public void OnTabContentBoundsChanged(EventArgs e)
        {
            if (TabContentBoundsChanged != null)
            {
                TabContentBoundsChanged(this, e);
            }
        }

        public void OnTabBoundsChanged(EventArgs e)
        {
            if (TabBoundsChanged != null)
            {
                TabBoundsChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollRightVisibleChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollRightVisibleChanged(EventArgs e)
        {
            if (ScrollRightVisibleChanged != null)
            {
                ScrollRightVisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollRightPressedChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollRightPressedChanged(EventArgs e)
        {
            if (ScrollRightPressedChanged != null)
            {
                ScrollRightPressedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollRightBoundsChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollRightBoundsChanged(EventArgs e)
        {
            if (ScrollRightBoundsChanged != null)
            {
                ScrollRightBoundsChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollRightSelectedChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollRightSelectedChanged(EventArgs e)
        {
            if (ScrollRightSelectedChanged != null)
            {
                ScrollRightSelectedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollLeftVisibleChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollLeftVisibleChanged(EventArgs e)
        {
            if (ScrollLeftVisibleChanged != null)
            {
                ScrollLeftVisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollLeftPressedChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollLeftPressedChanged(EventArgs e)
        {
            if (ScrollLeftPressedChanged != null)
            {
                ScrollLeftPressedChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollLeftBoundsChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollLeftBoundsChanged(EventArgs e)
        {
            if (ScrollLeftBoundsChanged != null)
            {
                ScrollLeftBoundsChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ScrollLeftSelectedChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        public void OnScrollLeftSelectedChanged(EventArgs e)
        {
            if (ScrollLeftSelectedChanged != null)
            {
                ScrollLeftSelectedChanged(this, e);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Sets the tab as active without sending the message to the Ribbon
        /// </summary>
        internal void SetActive(bool active)
        {
            bool trigger = _active != active;

            _active = active;

            if (trigger)
                OnActiveChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the TabBounds property
        /// </summary>
        /// <param name="tabBounds">Rectangle representing the bounds of the tab</param>
        internal void SetTabBounds(Rectangle tabBounds)
        {
            bool tigger = TabBounds != tabBounds;

            TabBounds = tabBounds;

            OnTabBoundsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the TabContentBounds
        /// </summary>
        /// <param name="tabContentBounds">Rectangle representing the bounds of the tab's content</param>
        internal void SetTabContentBounds(Rectangle tabContentBounds)
        {
            bool trigger = TabContentBounds != tabContentBounds;

            TabContentBounds = tabContentBounds;

            OnTabContentBoundsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Gets the panel with the larger width and the specified size mode
        /// </summary>
        /// <param name="size">Size mode of panel to search</param>
        /// <returns>Larger panel. Null if none of the specified size mode</returns>
        private RibbonPanel GetLargerPanel(RibbonElementSizeMode size)
        {
            RibbonPanel result = null;

            foreach (RibbonPanel panel in Panels)
            {
                if (panel.SizeMode != size) continue;

                if (result == null) result = panel;

                if (panel.Bounds.Width > result.Bounds.Width)
                {
                    result = panel;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the panel with a larger size
        /// </summary>
        /// <returns></returns>
        private RibbonPanel GetLargerPanel()
        {
            RibbonPanel largeLarger = GetLargerPanel(RibbonElementSizeMode.Large);

            if (largeLarger != null) return largeLarger;

            RibbonPanel mediumLarger = GetLargerPanel(RibbonElementSizeMode.Medium);

            if (mediumLarger != null) return mediumLarger;

            RibbonPanel compactLarger = GetLargerPanel(RibbonElementSizeMode.Compact);

            if (compactLarger != null) return compactLarger;

            RibbonPanel overflowLarger = GetLargerPanel(RibbonElementSizeMode.Overflow);

            if (overflowLarger != null) return overflowLarger;

            return null;
        }

        private bool AllPanelsOverflow()
        {

            foreach (RibbonPanel panel in Panels)
            {
                if (panel.SizeMode != RibbonElementSizeMode.Overflow)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Updates the regions of the panels and its contents
        /// </summary>
        internal void UpdatePanelsRegions()
        {
            if (Panels.Count == 0) return;
            if (Owner == null || Owner.IsDisposed) return;

            if (!Owner.IsDesignMode())
                _offset = 0;

            int curRight = TabContentBounds.Left + Owner.PanelPadding.Left + _offset;
            int curLeft = TabContentBounds.Right - Owner.PanelPadding.Right;
            int panelsTop = TabContentBounds.Top + Owner.PanelPadding.Top;
            int panelsVisibles = 0;

            using (Graphics g = Owner.CreateGraphics())
            {
                //Check all at full size
                foreach (RibbonPanel panel in Panels)
                {
                    if (panel.Visible && Owner.RightToLeft == RightToLeft.No)
                    {
                        RibbonElementSizeMode sMode = panel.FlowsTo == RibbonPanelFlowDirection.Right ? RibbonElementSizeMode.Medium : RibbonElementSizeMode.Large;
                        //Set the bounds of the panel to let it know it's height
                        panel.SetBounds(new Rectangle(0, 0, 1, TabContentBounds.Height - Owner.PanelPadding.Vertical));

                        //Size of the panel
                        Size size = panel.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, sMode));

                        //Creates the bounds of the panel
                        Rectangle bounds = new Rectangle(
                             curRight, panelsTop,
                             size.Width, size.Height);

                        //Set the bounds of the panel
                        panel.SetBounds(bounds);

                        //Let the panel know what size we have decided for it
                        panel.SetSizeMode(sMode);

                        //Update curRight
                        curRight = bounds.Right + Owner.PanelSpacing;

                        //Update panelsVisibles
                        panelsVisibles += 1;
                    }
                    else if (panel.Visible && Owner.RightToLeft == RightToLeft.Yes)
                    {
                        RibbonElementSizeMode sMode = panel.FlowsTo == RibbonPanelFlowDirection.Right ? RibbonElementSizeMode.Medium : RibbonElementSizeMode.Large;

                        //Set the bounds of the panel to let it know it's height
                        panel.SetBounds(new Rectangle(0, 0, 1, TabContentBounds.Height - Owner.PanelPadding.Vertical));

                        //Size of the panel
                        Size size = panel.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, sMode));

                        curLeft -= size.Width + Owner.PanelSpacing;

                        //Creates the bounds of the panel
                        Rectangle bounds = new Rectangle(
                             curLeft, panelsTop,
                             size.Width, size.Height);

                        //Set the bounds of the panel
                        panel.SetBounds(bounds);

                        //Let the panel know what size we have decided for it
                        panel.SetSizeMode(sMode);

                        //Update curLeft
                        curLeft = bounds.Left - 1 - Owner.PanelSpacing;

                        //Update panelsVisibles
                        panelsVisibles += 1;
                    }
                    else
                    {
                        panel.SetBounds(Rectangle.Empty);
                    }
                }

                if (!Owner.IsDesignMode())
                {
                    //check if there are visible panels
                    if (panelsVisibles > 0)
                    {
                        while (curRight > TabContentBounds.Right && !AllPanelsOverflow())
                        {
                            #region Down grade the larger panel one position

                            RibbonPanel larger = GetLargerPanel();

                            if (larger.SizeMode == RibbonElementSizeMode.Large)
                                larger.SetSizeMode(RibbonElementSizeMode.Medium);
                            else if (larger.SizeMode == RibbonElementSizeMode.Medium)
                                larger.SetSizeMode(RibbonElementSizeMode.Compact);
                            else if (larger.SizeMode == RibbonElementSizeMode.Compact)
                                larger.SetSizeMode(RibbonElementSizeMode.Overflow);

                            Size size = larger.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, larger.SizeMode));

                            larger.SetBounds(new Rectangle(larger.Bounds.Location, new Size(size.Width + Owner.PanelMargin.Horizontal, size.Height)));

                            #endregion

                            //Reset x-axis reminder
                            curRight = TabContentBounds.Left + Owner.PanelPadding.Left;

                            //Re-arrange location because of the new bounds
                            foreach (RibbonPanel panel in Panels)
                            {
                                Size s = panel.Bounds.Size;
                                panel.SetBounds(new Rectangle(new Point(curRight, panelsTop), s));
                                curRight += panel.Bounds.Width + Owner.PanelSpacing;
                            }

                        }
                    }
                }

                //Update regions of all panels
                foreach (RibbonPanel panel in Panels)
                {
                    panel.UpdateItemsRegions(g, panel.SizeMode);
                }
            }

            UpdateScrollBounds();
        }

        /// <summary>
        /// Updates the bounds of the scroll bounds
        /// </summary>
        private void UpdateScrollBounds()
        {
            int w = 13;
            bool scrBuffer = ScrollRightVisible;
            bool sclBuffer = ScrollLeftVisible;
            Rectangle rrBuffer = ScrollRightBounds;
            Rectangle rlBuffer = ScrollLeftBounds;

            if (Panels.Count == 0) return;



            if (Panels[Panels.Count - 1].Bounds.Right > TabContentBounds.Right)
            {
                ScrollRightVisible = true;
            }
            else
            {
                ScrollRightVisible = false;
            }

            if (ScrollRightVisible != scrBuffer)
            {
                OnScrollRightVisibleChanged(EventArgs.Empty);
            }



            if (_offset < 0)
            {
                ScrollLeftVisible = true;
            }
            else
            {
                ScrollLeftVisible = false;
            }

            if (ScrollRightVisible != scrBuffer)
            {
                OnScrollLeftVisibleChanged(EventArgs.Empty);
            }

            if (ScrollLeftVisible || ScrollRightVisible)
            {
                ScrollRightBounds = Rectangle.FromLTRB(
                     Owner.ClientRectangle.Right - w,
                     TabContentBounds.Top,
                     Owner.ClientRectangle.Right,
                     TabContentBounds.Bottom);

                ScrollLeftBounds = Rectangle.FromLTRB(
                     0,
                     TabContentBounds.Top,
                     w,
                     TabContentBounds.Bottom);

                if (ScrollRightBounds != rrBuffer)
                {
                    OnScrollRightBoundsChanged(EventArgs.Empty);
                }

                if (ScrollLeftBounds != rlBuffer)
                {
                    OnScrollLeftBoundsChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Overriden. Returns a string representation of the tab
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Tab: {0}", Text);
        }

        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseEnter(MouseEventArgs e)
        {
            if (MouseEnter != null)
            {
                MouseEnter(this, e);
            }
        }

        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseLeave(MouseEventArgs e)
        {
            _TT.Active = false;

            if (MouseLeave != null)
            {
                MouseLeave(this, e);
            }
        }

        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">Event data</param>
        public virtual void OnMouseMove(MouseEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, e);
            }
            if (!_TT.Active && !string.IsNullOrEmpty(ToolTip))  // ToolTip should be working without title as well - to get Office 2007 Look & Feel
            {
                if (ToolTip != _TT.GetToolTip(Owner))
                {
                    _TT.SetToolTip(Owner, ToolTip);
                }
                _TT.Active = true;
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollLeftPressed"/>
        /// </summary>
        /// <param name="pressed"></param>
        internal void SetScrollLeftPressed(bool pressed)
        {
            ScrollLeftPressed = pressed;

            if (pressed)
                ScrollLeft();

            OnScrollLeftPressedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollLeftSelected"/>
        /// </summary>
        /// <param name="selected"></param>
        internal void SetScrollLeftSelected(bool selected)
        {
            ScrollLeftSelected = selected;

            OnScrollLeftSelectedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollRightPressed"/>
        /// </summary>
        /// <param name="pressed"></param>
        internal void SetScrollRightPressed(bool pressed)
        {
            ScrollRightPressed = pressed;

            if (pressed) ScrollRight();

            OnScrollRightPressedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the <see cref="ScrollRightSelected"/>
        /// </summary>
        /// <param name="selected"></param>
        internal void SetScrollRightSelected(bool selected)
        {
            ScrollRightSelected = selected;

            OnScrollRightSelectedChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Presses the lef-scroll button
        /// </summary>
        public void ScrollLeft()
        {
            ScrollOffset(50);
        }

        /// <summary>
        /// Presses the left-scroll button
        /// </summary>
        public void ScrollRight()
        {
            ScrollOffset(-50);
        }

        public void ScrollOffset(int amount)
        {
            _offset += amount;

            foreach (RibbonPanel p in Panels)
            {
                p.SetBounds(new Rectangle(p.Bounds.Left + amount,
                     p.Bounds.Top, p.Bounds.Width, p.Bounds.Height));
            }

            if (Site != null && Site.DesignMode)
                UpdatePanelsRegions();

            UpdateScrollBounds();

            Owner.Invalidate();
        }

        private void _TT_Popup(object sender, PopupEventArgs e)
        {
            if (ToolTipPopUp != null)
            {
                ToolTipPopUp(sender, new RibbonElementPopupEventArgs(this, e));
                if (ToolTip != _TT.GetToolTip(Owner))
                    _TT.SetToolTip(Owner, ToolTip);
            }
        }

        private void EnsureAnyTabVisible()
        {
            int myIndex = Owner.Tabs.IndexOf(this);
            // check if any of the next tabs is visible
            for (int i = myIndex; i < Owner.Tabs.Count; i++)
            {
                if (this == Owner.Tabs[i])
                    continue;
                // check member, Property is false if control is hidden
                if (Owner.Tabs[i]._visible)
                {
                    Owner.ActiveTab = Owner.Tabs[i];
                    return;  // return because Invalidate is done during setting the active tab
                }
            }
            // check if any of the previous tabs is visible
            for (int i = 0; i < myIndex; i++)
            {
                if (this == Owner.Tabs[i])
                    continue;
                // check member, Property is false if control is hidden
                if (Owner.Tabs[i]._visible)
                {
                    Owner.ActiveTab = Owner.Tabs[i];
                    return;  // return because Invalidate is done during setting the active tab
                }
            }
            Owner.Invalidate();
        }

        #endregion

        #region IContainsRibbonComponents Members

        public IEnumerable<Component> GetAllChildComponents()
        {
            return Panels.ToArray();
        }

        #endregion
    }
}
