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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Security.Permissions;
using System.Windows.Forms.RibbonHelpers;

namespace System.Windows.Forms
{
    /// <summary>
    /// Provides a Ribbon toolbar
    /// </summary>
    [Designer(typeof(RibbonDesigner))]
    public class Ribbon
         : Control, IMessageFilter
    {
        private delegate void HandlerCallbackMethode();

        #region Const

        public const string Version = "5.0";

        private const int DefaultTabSpacing = 6;
        private const int DefaultPanelSpacing = 3;

        #endregion

        #region Static

        public static int CaptionBarHeight = 24;

        #endregion

        #region Fields
        private int _contextspace = 0;
        private bool? _isopeninvisualstudiodesigner;
        internal bool ForceOrbMenu;
        private Size _lastSizeMeasured;
        private Padding _tabsMargin;
        internal bool _minimized = true;//is the ribbon minimized?
        internal bool _expanded; //is the ribbon currently expanded when in minimize mode
        internal bool _expanding; //is the ribbon expanding. Flag used to prevent calling methods multiple time while the size changes
                                  //private int _minimizedHeight;//height when minimized
        private int _expandedHeight; //height when expanded
        private RibbonRenderer _renderer;
        private bool _useAlwaysStandardTheme;
        private Theme _theme;
        private Padding _panelMargin;
        private RibbonTab _activeTab;
        private RibbonTab _lastSelectedTab;

        //private float _tabSum;
        private bool _updatingSuspended;
        private bool _orbSelected;
        private bool _orbPressed;
        private bool _orbVisible;
        private Image _orbImage;
        private string _orbText;
        private Size _orbTextSize = Size.Empty;

        //private bool _quickAcessVisible;
        private RibbonWindowMode _borderMode;
        private GlobalHook _mouseHook;
        private GlobalHook _keyboardHook;
        private Font _RibbonItemFont = new Font("Trebuchet MS", 9);
        private Font _RibbonTabFont = new Font("Trebuchet MS", 9);
        private bool _CaptionBarVisible;
        private bool _enabled;

        internal RibbonItem ActiveTextBox; //tracks the current active textbox so we can hide it when you click off it
        #endregion

        #region Events

        /// <summary>
        /// Occours when the Orb is clicked
        /// </summary>
        public event EventHandler OrbClicked;

        /// <summary>
        /// Occours when the Orb is double-clicked
        /// </summary>
        public event EventHandler OrbDoubleClick;

        /// <summary>
        /// Occours when the <see cref="ActiveTab"/> property value has changed
        /// </summary>
        public event EventHandler ActiveTabChanged;

        /// <summary>
        /// Occours when the <see cref="ActualBorderMode"/> property has changed
        /// </summary>
        public event EventHandler ActualBorderModeChanged;

        /// <summary>
        /// Occours when the <see cref="CaptionButtonsVisible"/> property value has changed
        /// </summary>
        public event EventHandler CaptionButtonsVisibleChanged;

        ///// <summary>
        ///// Occours when the Ribbon changes its miminized state
        ///// </summary>
        public event EventHandler ExpandedChanged;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new Ribbon control
        /// </summary>
        public Ribbon()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            Dock = DockStyle.Top;

            Tabs = new RibbonTabCollection(this);
            Contexts = new RibbonContextCollection(this);

            OrbsPadding = new Padding(8, 5, 8, 3);
            TabsPadding = new Padding(8, 5, 8, 3);
            _tabsMargin = new Padding(12, 24 + 2, 20, 0);
            TabTextMargin = new Padding(4, 2, 4, 2);

            TabContentMargin = new Padding(1, 0, 1, 2);
            PanelPadding = new Padding(3);
            _panelMargin = new Padding(3, 2, 3, 15);
            PanelMoreMargin = new Padding(0, 0, 1, 1);
            PanelSpacing = DefaultPanelSpacing;
            ItemPadding = new Padding(1, 0, 1, 0);
            ItemMargin = new Padding(4, 2, 4, 2);
            ItemImageToTextSpacing = 3;
            TabSpacing = DefaultTabSpacing;
            DropDownMargin = new Padding(2);
            _renderer = new RibbonProfessionalRenderer(this);
            _orbVisible = true;
            OrbDropDown = new RibbonOrbDropDown(this);
            QuickAccessToolbar = new RibbonQuickAccessToolbar(this);
            //_quickAcessVisible = true;
            MinimizeButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Minimize);
            MaximizeRestoreButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Maximize);
            CloseButton = new RibbonCaptionButton(RibbonCaptionButton.CaptionButton.Close);
            LayoutHelper = new LayoutHelper(this);

            MinimizeButton.SetOwner(this);
            MaximizeRestoreButton.SetOwner(this);
            CloseButton.SetOwner(this);
            _CaptionBarVisible = true;

            Font = SystemFonts.CaptionFont;

            BorderMode = RibbonWindowMode.NonClientAreaGlass;

            _minimized = false;
            _expanded = true;
            _enabled = true;

            RibbonPopupManager.PopupRegistered += OnPopupRegistered;
            RibbonPopupManager.PopupUnRegistered += OnPopupUnregistered;
            Control parent = null;
            ParentChanged += (o1, e1) =>
            {
                if (parent != null)
                {
                    parent.KeyUp -= Ribbon_KeyUp;
                    parent.KeyDown -= parent_KeyDown;
                }

                parent = Parent;

                Application.AddMessageFilter(this);

                if (parent is Form)
                {
                    var form = parent as Form;

                    form.KeyPreview = true;

                    form.FormClosing += (o, e) =>
                    {
                        Application.RemoveMessageFilter(this);
                    };
                }
                if (parent != null)
                {
                    parent.KeyDown += parent_KeyDown;
                    parent.KeyUp += Ribbon_KeyUp;
                }
            };
        }

        public bool PreFilterMessage(ref Message m)
        {

            // Handle Alt - KeyUp
            const int WM_SYSKEYUP = 261;

            if (m.Msg == WM_SYSKEYUP)
            {
                AltPressed = false;
                Invalidate();
            }

            return false;
        }

        internal bool AltPressed;

        private void parent_KeyDown(object sender, KeyEventArgs e)
        {
            //var reDraw = false;

            //if (AltPressed != e.Alt)
            //{

            //    reDraw = true;
            //}

            AltPressed = e.Alt;

            //if (reDraw)
            Invalidate();
        }

        // Function to check if item was targeted by AltKey            
        private bool IsTargetedAltKey(string key, string altKey)
        {
            if (!String.IsNullOrEmpty(key) && String.Equals(key, altKey, StringComparison.InvariantCultureIgnoreCase))
                return true;
            return false;

        }

        // Choose an action for the selected item
        private void ParseItem(RibbonItem item)
        {
            //if (item is RibbonDropDown)
            //    (item as RibbonDropDown).Focus();
            //else 
            if (item is RibbonButton)
                (item as RibbonButton).PerformClick();
            else if (item is RibbonCheckBox)
                (item as RibbonCheckBox).Checked = !(item as RibbonCheckBox).Checked;
            else if (item is RibbonTextBox)
                (item as RibbonTextBox).SetSelected(true);
        }

        public void Ribbon_KeyUp(object sender, KeyEventArgs e)
        {

            // character on key up
            var char1 = (new KeysConverter().ConvertToString(e.KeyValue));

            if (e.Alt && e.KeyValue > 0)
            {

                var ribbon1 = this;

                if (ribbon1.OrbPressed)
                {
                    #region Check in Orb Menus
                    foreach (var item in ribbon1.OrbDropDown.MenuItems)
                    {

                        if (IsTargetedAltKey(item.AltKey, char1))
                        {

                            ParseItem(item);
                            goto done;
                        }
                    }
                    #endregion
                }

                if (ribbon1.ActiveTab != null)
                {
                    #region Check in Selected Tab
                    foreach (var panel in ribbon1.ActiveTab.Panels)
                    {
                        foreach (var item in panel.GetItems())
                        {

                            if (IsTargetedAltKey(item.AltKey, char1))
                            {

                                ParseItem(item);
                                goto done;
                            }
                        }
                    }
                    #endregion
                }

                if (IsTargetedAltKey(ribbon1.AltKey, char1))
                {
                    ribbon1.ShowOrbDropDown();
                    goto done;
                }

                #region Check Tabs
                foreach (var tab in ribbon1.Tabs)
                {
                    if (IsTargetedAltKey(tab.AltKey, char1))
                    {
                        ribbon1.ActiveTab = tab;
                        goto done;
                    }
                }
                #endregion

                done:;

            }

            //AltPressed = false;
            //Invalidate();
        }

        /// <summary>
        /// Ribbon is open in Visual Studio Designer
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
                try
                {
                    foreach (RibbonTab tab in Tabs)
                        tab.Dispose();
                }
                catch (InvalidOperationException)
                {
                    if (!IsOpenInVisualStudioDesigner())
                    {
                        throw;
                    }
                }
                OrbDropDown.Dispose();
                QuickAccessToolbar.Dispose();
                MinimizeButton.Dispose();
                MaximizeRestoreButton.Dispose();
                if (_RibbonItemFont != null)
                {
                    _RibbonItemFont.Dispose();
                }
                if (_RibbonTabFont != null)
                {
                    _RibbonTabFont.Dispose();
                }
                CloseButton.Dispose();

                RibbonPopupManager.PopupRegistered -= OnPopupRegistered;
                RibbonPopupManager.PopupUnRegistered -= OnPopupUnregistered;

                //GC.SuppressFinalize(this); //not necessary, it is called in base class
            }

            DisposeHooks();

            base.Dispose(disposing);
        }

        //Finalize is called by base class "System.ComponentModel.Component"
        //~Ribbon()
        //{
        //    Dispose(false);
        //}

        private void DisposeHooks()
        {
            if (_mouseHook != null)
            {
                _mouseHook.MouseWheel -= _mouseHook_MouseWheel;
                _mouseHook.MouseDown -= _mouseHook_MouseDown;
                _mouseHook.Dispose();
                _mouseHook = null;
            }
            if (_keyboardHook != null)
            {
                _keyboardHook.KeyDown -= _keyboardHook_KeyDown;
                _keyboardHook.Dispose();
                _keyboardHook = null;
            }
        }

        #endregion

        #region Props

        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(0)]
        [Category("Behavior")]
        public int ContextSpace
        {
            get => _contextspace;
            set
            {
                _contextspace = value;
                OrbStyle = OrbStyle;
            }
        }

        /// <summary>
        /// Gets or sets the key combination that activates this element when the Alt key was pressed
        /// </summary>
        [DefaultValue(null)]
        [Category("Behavior")]
        public string AltKey { get; set; }

        /// <summary>
        /// Gets or sets the tabs expanded state when in minimize mode
        /// </summary>
        [DefaultValue(true)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Expanded
        {
            get => _expanded;
            set
            {
                _expanded = value;
                if (!IsDesignMode() && Minimized)
                {
                    _expanding = true;
                    if (_expanded)
                        Height = _expandedHeight;
                    else
                        Height = MinimizedHeight;

                    OnExpandedChanged(EventArgs.Empty);
                    if (_expanded)
                        SetUpHooks();
                    else if (!_expanded && RibbonPopupManager.PopupCount == 0)
                        DisposeHooks();
                    //UpdateRegions();
                    Invalidate();
                    _expanding = false;
                }
            }
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Sets if the Ribbon should be enabled")]
        public new bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                Invalidate();
                UpdateRegions();
            }
        }

        /// <summary>
        /// Gets the height of the ribbon when collapsed <see cref="MinimizedHeight"/>
        /// </summary>
        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        [Description("Gets the height of the ribbon when collapsed")]
        public int MinimizedHeight
        {
            get
            {
                int tabBottom = Tabs.Count > 0 ? Tabs[0].Bounds.Bottom : 0;
                return Math.Max(OrbBounds.Bottom, tabBottom) + 1;
            }
        }

        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size Size
        {
            get => base.Size;
            set
            {
                base.Size = value;
                Height = value.Height;
                if (!Minimized || (!_expanding && Expanded))
                    _expandedHeight = Height;
            }
        }

        internal Rectangle CaptionTextBounds
        {
            get
            {
                if (RightToLeft == RightToLeft.No)
                {
                    int left = 0;
                    int right = Width - 100;
                    int contextLeft = right;
                    int contextRight = left;
                    if (OrbVisible) left = OrbBounds.Right;
                    if (QuickAccessToolbar.Visible) left = QuickAccessToolbar.Bounds.Right + 20;
                    if (QuickAccessToolbar.Visible && QuickAccessToolbar.DropDownButtonVisible) left = QuickAccessToolbar.DropDownButton.Bounds.Right;

                    // Determine the limits of the visible contexts
                    foreach (RibbonContext context in Contexts)
                    {
                        if (context.Visible)
                        {
                            contextLeft = Math.Min(contextLeft, context.Bounds.Left);
                            contextRight = Math.Max(contextRight, context.Bounds.Right);
                        }
                    }

                    /* Determine which side of the context is bigger to display the caption bar.
					 * In Word/Excel, there are always enough tabs that the left side is always big enough, but this
					 * may not be the case in the user's app. If no context is displayed, both will be the same size. */
                    Rectangle r;
                    if (contextLeft - left > right - contextRight)
                    {
                        r = Rectangle.FromLTRB(left, 0, contextLeft, CaptionBarSize);
                    }
                    else
                    {
                        r = Rectangle.FromLTRB(contextRight, 0, right, CaptionBarSize);
                    }

                    return r;
                }
                else
                {
                    int right = ClientRectangle.Right;
                    int left = 100;
                    int contextLeft = right;
                    int contextRight = left;
                    if (OrbVisible) right = OrbBounds.Left;
                    if (QuickAccessToolbar.Visible) right = QuickAccessToolbar.Bounds.Left - 20;
                    if (QuickAccessToolbar.Visible && QuickAccessToolbar.DropDownButtonVisible) right = QuickAccessToolbar.DropDownButton.Bounds.Left;

                    // Determine the limits of the visible contexts
                    foreach (RibbonContext context in Contexts)
                    {
                        if (context.Visible)
                        {
                            contextLeft = Math.Min(contextLeft, context.Bounds.Left);
                            contextRight = Math.Max(contextRight, context.Bounds.Right);
                        }
                    }

                    /* Determine which side of the context is bigger to display the caption bar.
					 * In Word/Excel, there are always enough tabs that the left side is always big enough, but this
					 * may not be the case in the user's app. If no context is displayed, both will be the same size. */
                    Rectangle r;
                    if (contextLeft - left > right - contextRight)
                    {
                        r = Rectangle.FromLTRB(left, 0, contextLeft, CaptionBarSize);
                    }
                    else
                    {
                        r = Rectangle.FromLTRB(contextRight, 0, right, CaptionBarSize);
                    }

                    return r;
                }
            }
        }

        /// <summary>
        /// Gets if the caption buttons are currently visible, according to the value specified in <see cref="BorderMode"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CaptionButtonsVisible { get; private set; }

        /// <summary>
        /// Gets the Ribbon's close button
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton CloseButton { get; }

        /// <summary>
        /// Gets the Ribbon's maximize-restore button
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton MaximizeRestoreButton { get; }

        /// <summary>
        /// Gets the Ribbon's minimize button
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonCaptionButton MinimizeButton { get; }

        /// <summary>
        /// Gets or sets the RibbonFormHelper object if the parent form is IRibbonForm
        /// </summary>
        [Browsable(false)]
        public RibbonFormHelper FormHelper
        {
            get
            {
                IRibbonForm irf = Parent as IRibbonForm;

                if (irf != null)
                {
                    return irf.Helper;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the Layout Helper
        /// </summary>
        [Browsable(false)]
        public LayoutHelper LayoutHelper { get; }

        /// <summary>
        /// Gets the actual <see cref="RibbonWindowMode"/> that the ribbon has. 
        /// It's value may vary from <see cref="BorderMode"/>
        /// because of computer and operative system capabilities.
        /// </summary>
        [Browsable(false)]
        public RibbonWindowMode ActualBorderMode { get; private set; }

        /// <summary>
        /// Gets or sets the border mode of the ribbon relative to the window where it is contained
        /// </summary>
        [DefaultValue(RibbonWindowMode.NonClientAreaGlass)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Specifies how the Ribbon is placed on the window border and the non-client area")]
        public RibbonWindowMode BorderMode
        {
            get => _borderMode;
            set
            {
                _borderMode = value;

                RibbonWindowMode actual = value;

                if (value == RibbonWindowMode.NonClientAreaGlass && !WinApi.IsGlassEnabled)
                {
                    actual = RibbonWindowMode.NonClientAreaCustomDrawn;
                }

                if (FormHelper == null || (value == RibbonWindowMode.NonClientAreaCustomDrawn && Environment.OSVersion.Platform != PlatformID.Win32NT))
                {
                    actual = RibbonWindowMode.InsideWindow;
                }

                SetActualBorderMode(actual);
            }
        }

        /// <summary>
        /// Gets the Orb's DropDown
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Orb")]
        [Browsable(true)]
        public RibbonOrbDropDown OrbDropDown { get; }

        /// <summary>
        /// Gets or sets the height of the Panel Caption area.
        /// </summary>
        [DefaultValue(15)]
        [Category("Appearance")]
        [Description("Gets or sets the height of the Panel Caption area")]
        public int PanelCaptionHeight
        {
            get => _panelMargin.Bottom;
            set
            {
                _panelMargin.Bottom = value;
                UpdateRegions();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets  the QuickAcessToolbar
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonQuickAccessToolbar QuickAccessToolbar { get; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Theme Theme => _theme == null ? Theme.Standard : _theme;

        /// <summary>
        /// Gets or sets the Style of the orb.
        /// 
        /// This is where the default values are loaded when changing the OrbStyle.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Orb")]
        [DefaultValue(RibbonOrbStyle.Office_2007)]
        public RibbonOrbStyle OrbStyle
        {
            get => Theme.Style;
            set
            {
                var changed = value != Theme.Style;
                EnsureCustomThemeCreated(value, ThemeColor);
                Theme.Style = value;
                if (value == RibbonOrbStyle.Office_2007)
                {
                    TabsPadding = new Padding(8, 4, 8, 4);
                    OrbsPadding = new Padding(8, 4, 8, 4);
                    if (CaptionBarVisible)
                    {
                        _tabsMargin = new Padding(12, CaptionBarHeight + 2 + ContextSpace, 20, 0);
                    }
                    else
                    {
                        _tabsMargin = new Padding(12, 2 + ContextSpace, 20, 0);
                    }
                    TabContentMargin = new Padding(1, 0, 2, 2);
                    TabContentPadding = new Padding(0);
                    TabSpacing = 6;
                    PanelSpacing = 4;
                    PanelPadding = new Padding(3);
                    _panelMargin = new Padding(3, 2, 3, 15);
                    PanelMoreSize = new Size(6, 6);
                    PanelMoreMargin = new Padding(0, 0, 1, 1);
                    ItemMargin = new Padding(4, 2, 4, 2);
                    ItemPadding = new Padding(1, 0, 1, 0);
                    ItemImageToTextSpacing = 4;
                    this.OrbDropDown.BorderRoundness = 8;
                }
                else if ((value == RibbonOrbStyle.Office_2010) ||
                         (value == RibbonOrbStyle.Office_2010_Extended))
                {
                    TabsPadding = new Padding(10, 3, 7, 2);
                    OrbsPadding = new Padding(17, 4, 15, 4);
                    if (CaptionBarVisible)
                    {
                        TabsMargin = new Padding(6, CaptionBarHeight + 2 + ContextSpace, 20, 0);
                    }
                    else
                    {
                        TabsMargin = new Padding(6, 2 + ContextSpace, 20, 0);
                    }
                    TabContentMargin = new Padding(0, 0, 0, 2);
                    TabContentPadding = new Padding(0);
                    TabSpacing = 3;
                    PanelSpacing = 0;
                    PanelPadding = new Padding(0, 1, 1, 1);
                    _panelMargin = new Padding(2, 2, 2, 15);
                    PanelMoreSize = new Size(6, 6);
                    PanelMoreMargin = new Padding(0, 0, 2, 0);
                    ItemMargin = new Padding(3, 2, 0, 2);
                    ItemPadding = new Padding(1, 0, 1, 0);
                    ItemImageToTextSpacing = 11;
                    this.OrbDropDown.BorderRoundness = 2;
                }
                else if (value == RibbonOrbStyle.Office_2013)
                {
                    TabsPadding = new Padding(8, 4, 8, 1);
                    OrbsPadding = new Padding(15, 3, 15, 3);
                    if (CaptionBarVisible)
                    {
                        _tabsMargin = new Padding(5, CaptionBarHeight + 2 + ContextSpace, 20, 0);
                    }
                    else
                    {
                        _tabsMargin = new Padding(5, 2 + ContextSpace, 20, 0);
                    }
                    TabContentMargin = new Padding(0, 0, 0, 2);
                    TabContentPadding = new Padding(0);
                    TabSpacing = 4;
                    PanelSpacing = 0;
                    PanelPadding = new Padding(3);
                    _panelMargin = new Padding(3, 2, 3, 15);
                    PanelMoreSize = new Size(6, 6);
                    PanelMoreMargin = new Padding(0, 0, 1, 0);
                    ItemMargin = new Padding(2, 2, 0, 2);
                    ItemPadding = new Padding(1, 0, 1, 0);
                    ItemImageToTextSpacing = 11;
                    this.OrbDropDown.BorderRoundness = 2;
                }
                UpdateRegions();
                Invalidate();
                if (changed)
                {
                    OrbStyleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler OrbStyleChanged;

        /// <summary>
        /// Gets or sets the theme of the ribbon control
        /// </summary>
        //Michael Spradlin 07/05/2013
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [DefaultValue(RibbonTheme.Normal)]
        public RibbonTheme ThemeColor
        {
            get => Theme.RibbonTheme;
            set
            {
                EnsureCustomThemeCreated(OrbStyle, value);
                Theme.RibbonTheme = value;
                OnRegionsChanged();
                Invalidate();
            }
        }

        /// <summary>
        /// If this value is set, you can still link a Ribbon fixed to the Standard Theme.
        /// E.g. used to statically link the theme builder form to the Standard theme which is
        /// used in the <see cref="ProToolstripRenderer"/>.
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("If this value is set, you can still link a Ribbon fixed to the Standard Theme.")]
        public bool UseAlwaysStandardTheme
        {
            get => _useAlwaysStandardTheme;
            set
            {
                _useAlwaysStandardTheme = value;
                if (value)
                {
                    _theme = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Text in the orb. Only available when the OrbStyle is set to Office2010
        /// </summary>
        [DefaultValue(null)]
        [Category("Orb")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string OrbText
        {
            get => _orbText;
            set
            {
                _orbText = value;
                RecalculateOrbTextSize();
                OnRegionsChanged();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the Image of the orb
        /// </summary>
        [DefaultValue(null)]
        [Category("Orb")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image OrbImage
        {
            get => _orbImage;
            set { _orbImage = value; OnRegionsChanged(); Invalidate(OrbBounds); }
        }

        /// <summary>
        /// Gets or sets if the Ribbon should show an orb on the corner
        /// </summary>
        [DefaultValue(true)]
        [Category("Orb")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OrbVisible
        {
            get => _orbVisible;
            set { _orbVisible = value; OnRegionsChanged(); }
        }

        /// <summary>
        /// Gets or sets if the Orb is currently selected
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OrbSelected
        {
            get => _orbSelected;
            set { _orbSelected = value; Invalidate(OrbBounds); }
        }

        /// <summary>
        /// Gets or sets if the Orb is currently pressed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OrbPressed
        {
            get => _orbPressed;
            set { _orbPressed = value; Invalidate(OrbBounds); }
        }

        /// <summary>
        /// Gets the Height of the caption bar
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CaptionBarSize => CaptionBarHeight;

        /// <summary>
        /// Gets the bounds of the orb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle OrbBounds
        {
            get
            {
                if (OrbStyle == RibbonOrbStyle.Office_2007)
                {
                    if (OrbVisible && RightToLeft == RightToLeft.No && CaptionBarVisible)
                    {
                        return new Rectangle(4, 4, 36, 36);
                    }

                    if (OrbVisible && RightToLeft == RightToLeft.Yes && CaptionBarVisible)
                    {
                        return new Rectangle(Width - 36 - 4, 4, 36, 36);
                    }

                    if (RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }

                    return new Rectangle(Width - 4, 4, 0, 0);
                }

                if ((OrbStyle == RibbonOrbStyle.Office_2010) || (OrbStyle == RibbonOrbStyle.Office_2010_Extended)) //Kevin Carbis - office 2010 style orb
                {
                    //Measure the string size of the button text so we know how big to make the button
                    Size contentSize = _orbTextSize;
                    //If we are using an image adjust the size
                    if (OrbImage != null)
                    {
                        contentSize.Width = Math.Max(contentSize.Width, OrbImage.Size.Width);
                        contentSize.Height = Math.Max(contentSize.Height, OrbImage.Size.Height);
                    }

                    if (OrbVisible && RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(1, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom);
                    }

                    if (OrbVisible && RightToLeft == RightToLeft.Yes && CaptionBarVisible)
                    {
                        return new Rectangle(Width - contentSize.Width - OrbsPadding.Left - OrbsPadding.Right - 1, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom);
                    }

                    if (RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }

                    return new Rectangle(Width - 4, 4, 0, 0);
                }
                else  //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                {
                    //Measure the string size of the button text so we know how big to make the button
                    Size contentSize = _orbTextSize;
                    //If we are using an image adjust the size
                    if (OrbImage != null)
                    {
                        contentSize.Width = Math.Max(contentSize.Width, OrbImage.Size.Width);
                        contentSize.Height = Math.Max(contentSize.Height, OrbImage.Size.Height);
                    }

                    if (OrbVisible && RightToLeft == RightToLeft.No)
                    {
                        //Steve
                        //return new Rectangle(0, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom);
                        return new Rectangle(0, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom + 1);
                    }

                    if (OrbVisible && RightToLeft == RightToLeft.Yes && CaptionBarVisible)
                    {
                        return new Rectangle(Width - contentSize.Width - OrbsPadding.Left - OrbsPadding.Right - 4, TabsMargin.Top, contentSize.Width + OrbsPadding.Left + OrbsPadding.Right, OrbsPadding.Top + contentSize.Height + OrbsPadding.Bottom);
                    }

                    if (RightToLeft == RightToLeft.No)
                    {
                        return new Rectangle(4, 4, 0, 0);
                    }

                    return new Rectangle(Width - 4, 4, 0, 0);
                }
            }
        }

        /// <summary>
        /// Gets the next tab to be activated
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab NextTab
        {
            get
            {

                if (ActiveTab == null || Tabs.Count == 0)
                {
                    if (Tabs.Count == 0)
                        return null;

                    return Tabs[0];
                }

                int index = Tabs.IndexOf(ActiveTab);

                if (index == Tabs.Count - 1)
                {
                    return ActiveTab;
                }

                return Tabs[index + 1];
            }
        }

        /// <summary>
        /// Gets the next tab to be activated
        /// </summary>
        /// <returns></returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab PreviousTab
        {
            get
            {

                if (ActiveTab == null || Tabs.Count == 0)
                {
                    if (Tabs.Count == 0)
                        return null;

                    return Tabs[0];
                }

                int index = Tabs.IndexOf(ActiveTab);

                if (index == 0)
                {
                    return ActiveTab;
                }

                return Tabs[index - 1];
            }
        }

        /// <summary>
        /// Gets or sets the internal spacing between the tab and its text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabTextMargin { get; set; }

        /// <summary> 
        /// Gets or sets the margis of the DropDowns shown by the Ribbon
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding DropDownMargin { get; set; }

        /// <summary>
        /// Gets or sets the external spacing of items on panels
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding ItemPadding { get; set; }

        /// <summary>
        /// Gets or sets the padding between the image and text on a drop down RibbonItem.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ItemImageToTextSpacing { get; set; }

        /// <summary>
        /// Gets or sets the internal spacing of items
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding ItemMargin { get; set; }

        /// <summary>
        /// Gets or sets the tab that is currently active
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab ActiveTab
        {
            get => _activeTab;
            set
            {
                RibbonTab NewTab = _activeTab;
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab != value)
                    {
                        tab.SetActive(false);
                    }
                    else
                    {
                        NewTab = tab;
                    }
                }
                NewTab.SetActive(true);

                _activeTab = value;

                RemoveHelperControls();

                value.UpdatePanelsRegions();

                Invalidate();

                RenewSensor();

                OnActiveTabChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the spacing leaded between panels
        /// </summary>
        [DefaultValue(DefaultPanelSpacing)]
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PanelSpacing { get; set; }

        /// <summary>
        /// Gets or sets the size of the panel More glyph
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size PanelMoreSize { get; set; } = new Size(7, 7);

        /// <summary>
        /// Gets or sets the external spacing of panels inside of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelPadding { get; set; }

        /// <summary>
        /// Gets or sets the internal spacing of panels inside of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelMargin
        {
            get => _panelMargin;
            set => _panelMargin = value;
        }

        /// <summary>
        /// Gets or sets the external spacing of the More glyph
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding PanelMoreMargin { get; set; }

        /// <summary>
        /// Gets or sets the spacing between tabs
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DefaultTabSpacing)]
        public int TabSpacing { get; set; }

        /// <summary>
        /// Gets the collection of RibbonTab tabs
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonTabCollection Tabs { get; }

        /// <summary>
        /// Gets or sets a value indicating if the Ribbon supports being minimized
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Minimized
        {
            get => _minimized;
            set
            {
                _minimized = value;
                if (!IsDesignMode())
                {
                    if (_minimized)
                    {
                        Height = MinimizedHeight;
                    }
                    else
                    {
                        Height = _expandedHeight;
                    }
                    Expanded = !Minimized;
                    UpdateRegions();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the collection of Contexts of this Ribbon
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RibbonContextCollection Contexts { get; }

        /// <summary>
        /// Gets or sets the Renderer for this Ribbon control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonRenderer Renderer
        {
            get => _renderer;
            set
            {
                _renderer = value ?? throw new ArgumentNullException(nameof(Renderer), "Null renderer!");
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the internal spacing of the tab content pane
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabContentMargin { get; set; }

        /// <summary>
        /// Gets or sets the external spacing of the tabs content pane
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabContentPadding { get; set; }

        /// <summary>
        /// Gets a value indicating the external spacing of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabsMargin
        {
            get => _tabsMargin;
            set
            {
                _tabsMargin = value;
                UpdateRegions();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets a value indicating the internal spacing of tabs
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding TabsPadding { get; set; }

        /// <summary>
        /// Gets a value indicating the internal spacing of the orb
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Padding OrbsPadding { get; set; }

        /// <summary>
        /// Overriden. The maximum size is fixed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MaximumSize
        {
            get => new Size(0, 200);
            set
            {
                //not supported
            }
        }

        /// <summary>
        /// Overriden. The minimum size is fixed
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Size MinimumSize
        {
            get => new Size(0, 27);
            set
            {
                //not supported
            }
        }

        /// <summary>
        /// Overriden. The default dock of the ribbon is top
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(DockStyle.Top)]
        public override DockStyle Dock
        {
            get => base.Dock;
            set => base.Dock = value;
        }

        /// <summary>
        /// Gets or sets the current panel sensor for this ribbon
        /// </summary>
        [Browsable(false)]
        public RibbonMouseSensor Sensor { get; private set; }

        [DefaultValue(RightToLeft.No)]
        public override RightToLeft RightToLeft
        {
            get => base.RightToLeft;
            set
            {
                base.RightToLeft = value;
                OnRegionsChanged();
            }
        }

        /// <summary>
        /// sets or gets the visibility of the caption bar
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool CaptionBarVisible
        {
            get => _CaptionBarVisible;
            set
            {
                _CaptionBarVisible = value;
                OrbStyle = OrbStyle;
            }
        }

        public override void Refresh()
        {
            try
            {
                if (IsDisposed == false)
                {
                    if (InvokeRequired)
                    {
                        HandlerCallbackMethode del = Refresh;
                        Invoke(del);
                    }
                    else
                    {
                        base.Refresh();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #region cr
        private string cr => "Professional Ribbon\n\n2009 Jos?Manuel Menéndez Poo\nwww.menendezpoo.com";

        #endregion

        ///// <summary>
        ///// Gets or sets the Font associated with Ribbon Items.
        ///// </summary>
        //[DefaultValue(null)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //public Font RibbonItemFont
        //{
        //    get { return _RibbonItemFont; }
        //    set { _RibbonItemFont = value;}
        //}


        /// <summary>
        /// Gets or sets the Font associated with Ribbon tabs and the ORB.
        /// </summary>
        [DefaultValue(null)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Font RibbonTabFont
        {
            get => _RibbonTabFont;
            set { _RibbonTabFont = value; RecalculateOrbTextSize(); }
        }

        /// <summary>
        /// Specifies if a Tab is invisible in case it is the only one and the text is set to string.Empty.
        /// </summary>
        [Category("Appearance")]
        [Description("Specifies if a Tab is invisible in case it is the only one and the text is set to string.Empty.")]
        [DefaultValue(true)]
        public bool HideSingleTabIfTextEmpty { get; set; } = true;

        #endregion

        #region Handler Methods

        /// <summary>
        /// Resends the mousedown to PopupManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            bool handled = false;
            if (!RectangleToScreen(OrbBounds).Contains(e.Location))
            {
                handled = RibbonPopupManager.FeedHookClick(e);
            }
            if (RectangleToScreen(Bounds).Contains(e.Location))
            {
                //they clicked inside the ribbon
                handled = true;
            }
            if (Minimized && !handled)
                Expanded = false;
        }

        /// <summary>
        /// Checks if MouseWheel should be raised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!RibbonPopupManager.FeedMouseWheel(e))
            {
                if (RectangleToScreen(
                     new Rectangle(Point.Empty, Size)
                     ).Contains(e.Location))
                {
                    OnMouseWheel(e);
                }
            }
        }


        /// <summary>
        /// Raises the OrbClicked event
        /// </summary>
        /// <param name="e">event data</param>
        internal virtual void OnOrbClicked(EventArgs e)
        {
            if (OrbPressed)
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.ItemClicked);
            }
            else
            {
                ShowOrbDropDown();
            }

            if (OrbClicked != null)
            {
                OrbClicked(this, e);
            }
        }

        /// <summary>
        /// Raises the OrbDoubleClicked
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnOrbDoubleClicked(EventArgs e)
        {
            if (OrbDoubleClick != null)
            {
                OrbDoubleClick(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes hooks
        /// </summary>
        private void SetUpHooks()
        {
            if (RibbonDesigner.Current == null)
            {
                if (_mouseHook == null)
                {
                    _mouseHook = new GlobalHook(GlobalHook.HookTypes.Mouse);
                    _mouseHook.MouseWheel += _mouseHook_MouseWheel;
                    _mouseHook.MouseDown += _mouseHook_MouseDown;
                }

                if (_keyboardHook == null)
                {
                    _keyboardHook = new GlobalHook(GlobalHook.HookTypes.Keyboard);
                    _keyboardHook.KeyDown += _keyboardHook_KeyDown;
                }
            }
        }

        private void _keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.EscapePressed);
            }
        }

        /// <summary>
        /// Shows the Orb's dropdown
        /// </summary>
        public void ShowOrbDropDown()
        {
            OrbPressed = true;
            if (RightToLeft == RightToLeft.No)
                if (OrbStyle == RibbonOrbStyle.Office_2007)
                    OrbDropDown.Show(PointToScreen(new Point(OrbBounds.X - 4, OrbBounds.Bottom - OrbDropDown.ContentMargin.Top + 2)));
                else if (OrbStyle == RibbonOrbStyle.Office_2010 || OrbStyle == RibbonOrbStyle.Office_2010_Extended || OrbStyle == RibbonOrbStyle.Office_2013)//Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                    OrbDropDown.Show(PointToScreen(new Point(OrbBounds.X - 4, OrbBounds.Bottom)));
                else
                    if (OrbStyle == RibbonOrbStyle.Office_2007)
                    OrbDropDown.Show(PointToScreen(new Point(OrbBounds.Right + 4 - OrbDropDown.Width, OrbBounds.Bottom - OrbDropDown.ContentMargin.Top + 2)));
                else if (OrbStyle == RibbonOrbStyle.Office_2010 || OrbStyle == RibbonOrbStyle.Office_2010_Extended || OrbStyle == RibbonOrbStyle.Office_2013) //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
                    OrbDropDown.Show(PointToScreen(new Point(OrbBounds.Right + 4 - OrbDropDown.Width, OrbBounds.Bottom)));
        }

        /// <summary>
        /// Shows the Orb's dropdown at the specified point.
        /// </summary>
        public void ShowOrbDropDown(Point pt)
        {
            OrbPressed = true;
            OrbDropDown.Show(PointToScreen(pt));
        }

        /// <summary>
        /// Drops out the old sensor and creates a new one
        /// </summary>
        private void RenewSensor()
        {
            if (ActiveTab == null)
            {
                return;
            }

            if (Sensor != null) Sensor.Dispose();

            Sensor = new RibbonMouseSensor(this, this, ActiveTab);

            if (CaptionButtonsVisible)
            {
                Sensor.Items.AddRange(new RibbonItem[] { CloseButton, MaximizeRestoreButton, MinimizeButton });
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="BorderMode"/> property
        /// </summary>
        /// <param name="borderMode">Actual border mode accquired</param>
        private void SetActualBorderMode(RibbonWindowMode borderMode)
        {
            bool trigger = ActualBorderMode != borderMode;

            ActualBorderMode = borderMode;

            if (trigger)
                OnActualBorderModeChanged(EventArgs.Empty);

            SetCaptionButtonsVisible(borderMode == RibbonWindowMode.NonClientAreaCustomDrawn);


        }

        /// <summary>
        /// Sets the value of the <see cref="CaptionButtonsVisible"/> property
        /// </summary>
        /// <param name="visible">Value to set to the caption buttons</param>
        private void SetCaptionButtonsVisible(bool visible)
        {
            bool trigger = CaptionButtonsVisible != visible;

            CaptionButtonsVisible = visible;

            if (trigger)
                OnCaptionButtonsVisibleChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Suspends any drawing/regions update operation
        /// </summary>
        public void SuspendUpdating()
        {
            _updatingSuspended = true;
        }

        /// <summary>
        /// Resumes any drawing/regions update operation
        /// </summary>
        ///// <param name="update"></param>
        public void ResumeUpdating()
        {
            ResumeUpdating(true);
        }

        /// <summary>
        /// Resumes any drawing/regions update operation
        /// </summary>
        /// <param name="update"></param>
        public void ResumeUpdating(bool update)
        {
            _updatingSuspended = false;

            if (update)
            {
                OnRegionsChanged();
            }
        }

        /// <summary>
        /// Removes all helper controls placed by any reason.
        /// Contol's visibility is set to false before removed.
        /// </summary>
        private void RemoveHelperControls()
        {
            RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.AppClicked);

            while (Controls.Count > 0)
            {
                Control ctl = Controls[0];

                ctl.Visible = false;

                Controls.Remove(ctl);
            }
        }

        /// <summary>
        /// Hittest on tab
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if a tab has been clicked</returns>
        internal bool TabHitTest(int x, int y)
        {
            //if (Rectangle.FromLTRB(Right - 10, Bottom - 10, Right, Bottom).Contains(x, y))
            //{
            //   MessageBox.Show(cr);
            //}

            //look for mouse on tabs
            foreach (RibbonTab tab in Tabs)
            {
                if (tab.TabBounds.Contains(x, y))
                {
                    ActiveTab = tab;
                    Expanded = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Hittest on context
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if a tab has been clicked</returns>
        internal bool ContextHitTest(int x, int y)
        {
            //look for mouse on context
            foreach (RibbonContext context in Contexts)
            {
                if (context.Bounds.Contains(x, y))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the regions of the tabs and the tab content bounds of the active tab
        /// </summary>
        internal void UpdateRegions()
        {
            UpdateRegions(null);
        }

        /// <summary>
        /// Updates the regions of the tabs and the tab content bounds of the active tab
        /// </summary>
        internal void UpdateRegions(Graphics g)
        {
            bool graphicsCreated = false;

            if (IsDisposed || _updatingSuspended) return;

            //Graphics for measurement
            if (g == null)
            {
                g = CreateGraphics();
                graphicsCreated = true;
            }

            UpdateRegionsTabsConsiderRTL(g);

            if (RightToLeft == RightToLeft.No)
            {
                #region Update QuickAccess bounds

                QuickAccessToolbar.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.Compact));
                if (OrbStyle == RibbonOrbStyle.Office_2007)
                    QuickAccessToolbar.SetBounds(new Rectangle(new Point(OrbBounds.Right + QuickAccessToolbar.Margin.Left, OrbBounds.Top - 2), QuickAccessToolbar.LastMeasuredSize));
                else if ((OrbStyle == RibbonOrbStyle.Office_2010) || (OrbStyle == RibbonOrbStyle.Office_2010_Extended)) //2010 - no need to offset for the orb
                    QuickAccessToolbar.SetBounds(new Rectangle(new Point(QuickAccessToolbar.Margin.Left, 0), QuickAccessToolbar.LastMeasuredSize));
                else if (OrbStyle == RibbonOrbStyle.Office_2013) //Michael Spradlin - 05/03/2013 Office 2013 Style Changes : no need to offset for the orb
                    QuickAccessToolbar.SetBounds(new Rectangle(new Point(QuickAccessToolbar.Margin.Left, 0), QuickAccessToolbar.LastMeasuredSize));

                #endregion

                #region Update Caption Buttons bounds

                if (CaptionButtonsVisible)
                {
                    Size cbs = new Size(20, 20);
                    int cbg = 2;
                    CloseButton.SetBounds(new Rectangle(new Point(ClientRectangle.Right - cbs.Width - cbg, cbg), cbs));
                    MaximizeRestoreButton.SetBounds(new Rectangle(new Point(CloseButton.Bounds.Left - cbs.Width, cbg), cbs));
                    MinimizeButton.SetBounds(new Rectangle(new Point(MaximizeRestoreButton.Bounds.Left - cbs.Width, cbg), cbs));
                }

                #endregion
            }
            else  // RightToLeft.Yes
            {
                #region Update QuickAccess bounds

                QuickAccessToolbar.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.Compact));
                if (OrbStyle == RibbonOrbStyle.Office_2007)
                    QuickAccessToolbar.SetBounds(new Rectangle(new Point(OrbBounds.Left - QuickAccessToolbar.Margin.Right - QuickAccessToolbar.LastMeasuredSize.Width, OrbBounds.Top - 2), QuickAccessToolbar.LastMeasuredSize));
                else if ((OrbStyle == RibbonOrbStyle.Office_2010) || (OrbStyle == RibbonOrbStyle.Office_2010_Extended)) //2010 - no need to offset for the orb
                    QuickAccessToolbar.SetBounds(new Rectangle(new Point(ClientRectangle.Right - QuickAccessToolbar.Margin.Right - QuickAccessToolbar.LastMeasuredSize.Width, 0), QuickAccessToolbar.LastMeasuredSize));
                else if (OrbStyle == RibbonOrbStyle.Office_2013)  //Michael Spradlin - 05/03/2013 Office 2013 Style Changes: no need to offset for the orb
                    QuickAccessToolbar.SetBounds(new Rectangle(new Point(ClientRectangle.Right - QuickAccessToolbar.Margin.Right - QuickAccessToolbar.LastMeasuredSize.Width, 0), QuickAccessToolbar.LastMeasuredSize));

                #endregion

                #region Update Caption Buttons bounds

                if (CaptionButtonsVisible)
                {
                    Size cbs = new Size(20, 20);
                    int cbg = 2;
                    CloseButton.SetBounds(new Rectangle(new Point(ClientRectangle.Left, cbg), cbs));
                    MaximizeRestoreButton.SetBounds(new Rectangle(new Point(CloseButton.Bounds.Right, cbg), cbs));
                    MinimizeButton.SetBounds(new Rectangle(new Point(MaximizeRestoreButton.Bounds.Right, cbg), cbs));
                }

                #endregion
            }

            //Update the minimize settings
            //_minimizedHeight = tabsBottom;

            if (graphicsCreated)
                g.Dispose();

            _lastSizeMeasured = Size;

            RenewSensor();
        }

        private void UpdateRegionsTabsConsiderRTL(Graphics g)
        {
            // Saves the bottom of the tabs
            int tabsBottom = 0;
            // X coordinate reminder
            Point curXPos = new Point((RightToLeft == RightToLeft.No) ? OrbBounds.Width + TabsMargin.Left : OrbBounds.Left - TabsMargin.Left + 4, 0);

            // Saves the width of the larger tab
            int maxWidth = 0;
            int tabTop = 0;

            #region Assign default tab and unused context bounds (best case)

            foreach (RibbonTab tab in Tabs)
            {
                if (tab.Visible || IsDesignMode())
                {
                    Size tabSize = tab.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));

                    if (tab.Contextual && tab.Context.ContextualTabsCount == 1)
                    {
                        //Only if the context covers one tab do we need to be concerned that the context text maybe longer than the tab text.
                        Size contextSize = tab.Context.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));
                        tabSize.Width = Math.Max(tabSize.Width, contextSize.Width);
                    }

                    tabTop = (tab.Invisible == false || OrbVisible)
                       ? TabsMargin.Top
                       : TabsMargin.Top - tabSize.Height - 8 + (OrbStyle == RibbonOrbStyle.Office_2013 ? 2 : 0);
                    Rectangle bounds = new Rectangle(0, tabTop,
                           TabsPadding.Left + tabSize.Width + TabsPadding.Right,
                           TabsPadding.Top + tabSize.Height + TabsPadding.Bottom);

                    bounds = LayoutHelper.CalcNewPosition(curXPos, bounds, LayoutHelper.RTLLayoutPosition.Far, TabSpacing);

                    tab.SetTabBounds(bounds);

                    curXPos = LayoutHelper.CalcNewPosition(bounds, curXPos, LayoutHelper.RTLLayoutPosition.Far, 0);

                    maxWidth = Math.Max(bounds.Width, maxWidth);
                    tabsBottom = Math.Max(bounds.Bottom, tabsBottom);

                    tab.SetTabContentBounds(Rectangle.FromLTRB(
                         TabContentMargin.Left, tabsBottom + TabContentMargin.Top,
                         ClientSize.Width - TabContentMargin.Right, ClientSize.Height - TabContentMargin.Bottom));

                    if (tab.Active)
                    {
                        tab.UpdatePanelsRegions();
                    }
                }
                else
                {
                    tab.SetTabBounds(Rectangle.Empty);
                    tab.SetTabContentBounds(Rectangle.Empty);
                    if (tab.Contextual)
                    {
                        tab.Context.SetBounds(Rectangle.Empty);
                        tab.Context.SetHeaderBounds(Rectangle.Empty);
                    }
                }
            }

            foreach (RibbonContext context in Contexts)
            {
                //Include unused RibbonContexts that need to be displayed
                if ((context.ContextualTabsCount == 0) && IsDesignMode())
                {
                    Size contextSize = context.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));

                    tabTop = (context.Visible || OrbVisible)
                       ? TabsMargin.Top
                       : TabsMargin.Top - contextSize.Height - 8 + (OrbStyle == RibbonOrbStyle.Office_2013 ? 2 : 0);

                    Rectangle contextBounds = new Rectangle(0, tabTop - CaptionBarHeight,
                        TabsPadding.Left + contextSize.Width + TabsPadding.Right,
                        TabsPadding.Top + contextSize.Height + TabsPadding.Bottom + CaptionBarHeight);
                    Rectangle contextHeaderBounds = new Rectangle(contextBounds.Left, contextBounds.Top,
                        contextBounds.Width, CaptionBarHeight);

                    contextBounds = LayoutHelper.CalcNewPosition(curXPos, contextBounds, LayoutHelper.RTLLayoutPosition.Far, TabSpacing);
                    contextHeaderBounds = LayoutHelper.CalcNewPosition(curXPos, contextHeaderBounds, LayoutHelper.RTLLayoutPosition.Far, TabSpacing);

                    context.SetBounds(contextBounds);
                    context.SetHeaderBounds(contextHeaderBounds);

                    curXPos = LayoutHelper.CalcNewPosition(contextBounds, curXPos, LayoutHelper.RTLLayoutPosition.Far, 0);

                    maxWidth = Math.Max(contextBounds.Width, maxWidth);
                    tabsBottom = Math.Max(contextBounds.Bottom, tabsBottom);
                }
            }


            #endregion

            #region Reduce bounds of tabs if needed

            while ((RightToLeft == RightToLeft.No ? curXPos.X > ClientRectangle.Right : curXPos.X < ClientRectangle.Left)
               && maxWidth > 0)
            {

                curXPos = new Point((RightToLeft == RightToLeft.No) ? OrbBounds.Width + TabsMargin.Left : OrbBounds.Left - TabsMargin.Left + 4, 0);
                maxWidth--;

                // Shrink each tab
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.Visible)
                    {

                        Size tabSize = tab.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));

                        if (tabSize.Width >= maxWidth)
                        {
                            tabSize.Width = maxWidth;
                        }

                        if (tab.Contextual && tab.Context.ContextualTabsCount == 1)
                        {
                            //Only if the context covers one tab do we need to be concerned that the context text maybe longer than the tab text.
                            Size contextSize = tab.Context.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));
                            tabSize.Width = Math.Max(tabSize.Width, contextSize.Width);
                        }

                        tabTop = (tab.Invisible == false || OrbVisible)
                           ? TabsMargin.Top
                           : TabsMargin.Top - tabSize.Height - 8 + (OrbStyle == RibbonOrbStyle.Office_2013 ? 2 : 0);
                        Rectangle bounds = new Rectangle(0, tabTop,
                               TabsPadding.Left + tabSize.Width + TabsPadding.Right,
                               TabsPadding.Top + tabSize.Height + TabsPadding.Bottom);

                        bounds = LayoutHelper.CalcNewPosition(curXPos, bounds, LayoutHelper.RTLLayoutPosition.Far, TabSpacing);

                        tab.SetTabBounds(bounds);

                        curXPos = LayoutHelper.CalcNewPosition(bounds, curXPos, LayoutHelper.RTLLayoutPosition.Far, 0);
                    }
                }

                // Shrink each context
                foreach (RibbonContext context in Contexts)
                {
                    //Include unused RibbonContexts that need to be displayed
                    if ((context.ContextualTabsCount == 0) && IsDesignMode())
                    {
                        Size contextSize = context.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g, RibbonElementSizeMode.None));

                        if (contextSize.Width >= maxWidth)
                        {
                            contextSize.Width = maxWidth;
                        }

                        tabTop = (context.Visible || OrbVisible)
                           ? TabsMargin.Top
                           : TabsMargin.Top - contextSize.Height - 8 + (OrbStyle == RibbonOrbStyle.Office_2013 ? 2 : 0);

                        Rectangle contextBounds = new Rectangle(0, tabTop - CaptionBarHeight,
                            TabsPadding.Left + contextSize.Width + TabsPadding.Right,
                            TabsPadding.Top + contextSize.Height + TabsPadding.Bottom + CaptionBarHeight);
                        Rectangle contextHeaderBounds = new Rectangle(contextBounds.Left, contextBounds.Top,
                            contextBounds.Width, CaptionBarHeight);

                        contextBounds = LayoutHelper.CalcNewPosition(curXPos, contextBounds, LayoutHelper.RTLLayoutPosition.Far, TabSpacing);
                        contextHeaderBounds = LayoutHelper.CalcNewPosition(curXPos, contextHeaderBounds, LayoutHelper.RTLLayoutPosition.Far, TabSpacing);

                        context.SetBounds(contextBounds);
                        context.SetHeaderBounds(contextHeaderBounds);

                        curXPos = LayoutHelper.CalcNewPosition(contextBounds, curXPos, LayoutHelper.RTLLayoutPosition.Far, 0);
                    }
                }
            }

            #endregion

            #region Fix the width of contexts that contain tabs

            foreach (RibbonContext context in Contexts)
            {
                //Define bounds for contexts
                if (context.ContextualTabsCount > 0)
                {
                    foreach (RibbonTab tab in context.ContextualTabs)
                    {
                        Rectangle firstTabBounds = context.ContextualTabs[context.ContextualTabs.Count - 1].Bounds;
                        Rectangle lastTabBounds = context.ContextualTabs[0].Bounds;

                        int maxRight = Math.Max(firstTabBounds.Right, lastTabBounds.Right);
                        int maxLeft = Math.Min(firstTabBounds.Left, lastTabBounds.Left);

                        Rectangle contextBounds = new Rectangle(maxLeft, tabTop - CaptionBarHeight,
                             (maxRight - maxLeft),
                             TabsPadding.Top + firstTabBounds.Height + TabsPadding.Bottom + CaptionBarHeight);
                        Rectangle contextHeaderBounds = new Rectangle(contextBounds.Left, contextBounds.Top,
                             contextBounds.Width, CaptionBarHeight);

                        tab.Context.SetBounds(contextBounds);
                        tab.Context.SetHeaderBounds(contextHeaderBounds);
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Forces a size recalculation on the entire control
        /// </summary>
        internal void OnRegionsChanged()
        {
            if (_updatingSuspended) return;

            //Kevin - Fix when only one tab present and there is a textbox on it. It will loose focus after each char is entered
            //if (Tabs.Count == 1)
            if (Tabs.Count == 1 && ActiveTab != Tabs[0])
            {
                ActiveTab = Tabs[0];
            }

            _lastSizeMeasured = Size.Empty;

            Refresh();
        }

        /// <summary>
        /// Redraws the specified tab
        /// </summary>
        /// <param name="tab"></param>
        internal void RedrawTab(RibbonTab tab)
        {
            using (Graphics g = CreateGraphics())
            {
                Rectangle clip = Rectangle.FromLTRB(
                     tab.TabBounds.Left,
                     tab.TabBounds.Top,
                     tab.TabBounds.Right,
                     tab.TabBounds.Bottom);

                g.SetClip(clip);
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                tab.OnPaint(this, new RibbonElementPaintEventArgs(tab.TabBounds, g, RibbonElementSizeMode.None));
                g.SmoothingMode = sm;
                g.TextRenderingHint = TextRenderingHint.SystemDefault;
            }
        }

        /// <summary>
        /// Sets the currently selected tab
        /// </summary>
        /// <param name="tab"></param>
        private void SetSelectedTab(RibbonTab tab)
        {
            if (tab == _lastSelectedTab) return;

            if (_lastSelectedTab != null)
            {
                _lastSelectedTab.SetSelected(false);
                RedrawTab(_lastSelectedTab);
            }

            if (tab != null)
            {
                tab.SetSelected(true);
                RedrawTab(tab);
            }

            _lastSelectedTab = tab;

        }

        /// <summary>
        /// Suspends the sensor activity
        /// </summary>
        internal void SuspendSensor()
        {
            if (Sensor != null)
                Sensor.Suspend();
        }

        /// <summary>
        /// Resumes the sensor activity
        /// </summary>
        internal void ResumeSensor()
        {
            Sensor.Resume();
        }

        /// <summary>
        /// Redraws the specified area on the sensor's control
        /// </summary>
        /// <param name="area"></param>
        public void RedrawArea(Rectangle area)
        {
            Sensor.Control.Invalidate(area);
        }

        /// <summary>
        /// Activates the next tab available
        /// </summary>
        public void ActivateNextTab()
        {
            RibbonTab tab = NextTab;

            if (tab != null)
            {
                ActiveTab = tab;
            }
        }

        /// <summary>
        /// Activates the previous tab available
        /// </summary>
        public void ActivatePreviousTab()
        {
            RibbonTab tab = PreviousTab;

            if (tab != null)
            {
                ActiveTab = tab;
            }
        }

        /// <summary>
        /// Handles the mouse down on the orb area
        /// </summary>
        internal void OrbMouseDown()
        {
            OnOrbClicked(EventArgs.Empty);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {

            bool bypassed = false;

            if (WinApi.IsWindows && (ActualBorderMode == RibbonWindowMode.NonClientAreaGlass || ActualBorderMode == RibbonWindowMode.NonClientAreaCustomDrawn))
            {
                if (m.Msg == WinApi.WM_NCHITTEST) //0x84
                {
                    Form f = FindForm();
                    Rectangle caption;

                    if (RightToLeft == RightToLeft.No)
                    {
                        int captionLeft = QuickAccessToolbar.Visible ? QuickAccessToolbar.Bounds.Right : OrbBounds.Right;
                        if (QuickAccessToolbar.Visible && QuickAccessToolbar.DropDownButtonVisible) captionLeft = QuickAccessToolbar.DropDownButton.Bounds.Right;
                        caption = Rectangle.FromLTRB(captionLeft, 0, Width, CaptionBarSize);
                    }
                    else
                    {
                        int captionRight = QuickAccessToolbar.Visible ? QuickAccessToolbar.Bounds.Left : OrbBounds.Left;
                        if (QuickAccessToolbar.Visible && QuickAccessToolbar.DropDownButtonVisible) captionRight = QuickAccessToolbar.DropDownButton.Bounds.Left;
                        caption = Rectangle.FromLTRB(0, 0, captionRight, CaptionBarSize);
                    }

                    Point screenPoint = new Point(WinApi.LoWord((int)m.LParam), WinApi.HiWord((int)m.LParam));
                    Point ribbonPoint = PointToClient(screenPoint);
                    bool onCaptionButtons = false;

                    if (CaptionButtonsVisible)
                    {
                        onCaptionButtons = CloseButton.Bounds.Contains(ribbonPoint) ||
                        MinimizeButton.Bounds.Contains(ribbonPoint) ||
                        MaximizeRestoreButton.Bounds.Contains(ribbonPoint);
                    }

                    if (RectangleToScreen(caption).Contains(screenPoint) && !onCaptionButtons)
                    {
                        //on the caption bar area
                        Point p = PointToScreen(screenPoint);
                        WinApi.SendMessage(f.Handle, WinApi.WM_NCHITTEST, m.WParam, WinApi.MakeLParam(p.X, p.Y));
                        m.Result = new IntPtr(-1);
                        bypassed = true;
                        //Kevin - fix so when you mouse off the caption buttons onto the caption area
                        //the buttons will clear the selection. same with the QAT buttons
                        CloseButton.SetSelected(false);
                        MinimizeButton.SetSelected(false);
                        MaximizeRestoreButton.SetSelected(false);
                        OrbSelected = false;
                        QuickAccessToolbar.DropDownButton.SetSelected(false);
                    }
                }
            }

            if (!bypassed)
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Paints the Ribbon on the specified device
        /// </summary>
        /// <param name="g">Device where to paint on</param>
        /// <param name="clip">Clip rectangle</param>
        private void PaintOn(Graphics g, Rectangle clip)
        {
            try
            {
                if (WinApi.IsWindows && Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                }

                //Caption Background
                Renderer.OnRenderRibbonBackground(new RibbonRenderEventArgs(this, g, clip));

                //Caption Bar
                Renderer.OnRenderRibbonCaptionBar(new RibbonRenderEventArgs(this, g, clip));

                //Caption Buttons
                if (CaptionButtonsVisible)
                {
                    MinimizeButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                    MaximizeRestoreButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                    CloseButton.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Medium));
                }

                //Orb
                Renderer.OnRenderRibbonOrb(new RibbonRenderEventArgs(this, g, clip));

                //QuickAccess toolbar
                QuickAccessToolbar.OnPaint(this, new RibbonElementPaintEventArgs(clip, g, RibbonElementSizeMode.Compact));

                //Render Contexts
                foreach (RibbonContext context in Contexts)
                {
                    if (context.Visible || IsDesignMode())
                    {
                        context.OnPaint(this, new RibbonElementPaintEventArgs(context.Bounds, g, RibbonElementSizeMode.None, this));
                    }
                }

                //Render Tabs
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.Visible || IsDesignMode())
                    {
                        tab.OnPaint(this, new RibbonElementPaintEventArgs(tab.TabBounds, g, RibbonElementSizeMode.None, this));
                    }
                }

                if (OrbVisible && !_expanded && !string.IsNullOrEmpty(OrbText))
                {
                    if ((OrbStyle == RibbonOrbStyle.Office_2010) ||
                        (OrbStyle == RibbonOrbStyle.Office_2010_Extended))
                    {
                        //draw the divider line at the bottom of the ribbon
                        Pen p = new Pen(Theme.RendererColorTable.TabBorder);
                        g.DrawLine(p, OrbBounds.Left, OrbBounds.Bottom, Bounds.Right, OrbBounds.Bottom);
                    }
                    else if (OrbStyle == RibbonOrbStyle.Office_2013)
                    {
                        //There is no line at the bottom of the Ribbon in Office 2013
                    }
                }
            }
            catch
            {
            }
        }

        private void PaintDoubleBuffered(Graphics wndGraphics, Rectangle clip)
        {
            using (Bitmap bmp = new Bitmap(Width, Height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Black);
                    PaintOn(g, clip);
                    g.Flush();

                    WinApi.BitBlt(wndGraphics.GetHdc(), clip.X, clip.Y, clip.Width, clip.Height, g.GetHdc(), clip.X, clip.Y, WinApi.SRCCOPY);
                    //WinApi.BitBlt(wndGraphics.GetHdc(), 0, 0, Width, Height, g.GetHdc(), 0, 0, WinApi.SRCCOPY);
                }

                //wndGraphics.DrawImage(bmp, Point.Empty);
            }
        }

        internal bool IsDesignMode()
        {
            return Site != null && Site.DesignMode;
        }

        private void EnsureCustomThemeCreated(RibbonOrbStyle orbStyle, RibbonTheme theme)
        {
            if (_theme == null && Theme.StandardThemeIsGlobal == false && UseAlwaysStandardTheme == false)
            {
                _theme = new Theme(orbStyle, theme);
            }
        }

        private void RecalculateOrbTextSize()
        {
            if (string.IsNullOrEmpty(OrbText))
            {
                _orbTextSize = Size.Empty;
            }
            else
            {
                try
                {
                    using (Graphics g = CreateGraphics())
                    {
                        _orbTextSize = Size.Ceiling(g.MeasureString(OrbText, RibbonTabFont));
                    }
                }
                catch { }
            }
        }

        #endregion

        #region Event Overrides

        /// <summary>
        /// Raises the <see cref="ActiveTabChanged"/> event
        /// </summary>
        /// <param name="e">Event data</param>
        protected virtual void OnActiveTabChanged(EventArgs e)
        {
            if (ActiveTabChanged != null)
            {
                ActiveTabChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ActualBorderMode"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnActualBorderModeChanged(EventArgs e)
        {
            if (ActualBorderModeChanged != null)
            {
                ActualBorderModeChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="CaptionButtonsVisibleChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCaptionButtonsVisibleChanged(EventArgs e)
        {
            if (CaptionButtonsVisibleChanged != null)
            {
                CaptionButtonsVisibleChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ExpandedChanged"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExpandedChanged(EventArgs e)
        {
            if (ExpandedChanged != null)
            {
                ExpandedChanged(this, e);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (OrbBounds.Contains(e.Location))
            {
                OnOrbDoubleClicked(EventArgs.Empty);
            }

            if (Tabs.Count != 1 || Tabs[0].Invisible == false)
            {
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.Bounds.Contains(e.Location))
                    {
                        Minimized = !Minimized;
                        break;
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Overriden. Raises the Paint event and draws all the Ribbon content
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_updatingSuspended) return;

            if (Size != _lastSizeMeasured)
                UpdateRegions(e.Graphics);

            PaintOn(e.Graphics, e.ClipRectangle);
        }

        /// <summary>
        /// Overriden. Raises the Click event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        /// <summary>
        /// Overriden. Riases the MouseEnter event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Overriden. Raises the MouseLeave  event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            //Console.WriteLine("Ribbon Mouse Leave");
            SetSelectedTab(null);
            if (!Expanded)
                foreach (RibbonTab tab in Tabs)
                {
                    tab.SetSelected(false);
                }
            Invalidate();
        }

        /// <summary>
        /// Overriden. Raises the MouseMove event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //Console.WriteLine("Ribbon: " + e.Location.ToString());

            // Kevin Carbis's new code, edited by adriancs, on behave of Carbis
            // The below fix some minor bug. The cursor is not displayed properly
            // when cursor is entering CheckBound of CheckBox and TextBox.
            // The cursor keep changing from Cursors.Default to Cursors.Hand a few times
            // within a second.
            // The below code is obtain from Kcarbis's website
            #region Kevin Carbis's new code, edited by adriancs
            base.OnMouseMove(e);

            if (ActiveTab == null) return;

            bool someTabHitted = false;

            //Check if mouse on tab
            if (ActiveTab.TabContentBounds.Contains(e.X, e.Y))
            {
                //Do nothing, everything is on the sensor
            }
            //Check if mouse on orb
            else if (OrbVisible && OrbBounds.Contains(e.Location) && !OrbSelected)
            {
                OrbSelected = true;
                Invalidate(OrbBounds);
            }
            //Check if mouse on QuickAccess toolbar
            else if (QuickAccessToolbar.Visible && QuickAccessToolbar.Bounds.Contains(e.Location))
            {

            }
            else
            {
                //look for mouse on tabs
                foreach (RibbonTab tab in Tabs)
                {
                    if (tab.TabBounds.Contains(e.X, e.Y))
                    {
                        SetSelectedTab(tab);
                        someTabHitted = true;
                        tab.OnMouseMove(e);
                    }
                }
            }

            if (!someTabHitted)
                SetSelectedTab(null);

            if (OrbSelected && !OrbBounds.Contains(e.Location))
            {
                OrbSelected = false;
                Invalidate(OrbBounds);
            }
            #endregion

            #region Kevin Carbis's old code, commented out by adriancs
            //base.OnMouseMove(e);

            ////Kevin Carbis - Need to reset the curor here so we can pickup the cursor when it moves off of an active textbox.  If we don't we will
            ////have the IBeam cursor over the entire ribbon.
            //Cursor.Current = Cursors.Default;

            //if (ActiveTab == null) return;

            //bool someTabHitted = false;

            ////Check if mouse on tab
            //if (ActiveTab.TabContentBounds.Contains(e.X, e.Y))
            //{
            //    //Do nothing, everything is on the sensor
            //}
            ////Check if mouse on orb
            //else if (OrbVisible && OrbBounds.Contains(e.Location) && !OrbSelected)
            //{
            //    OrbSelected = true;
            //    Invalidate(OrbBounds);
            //}
            ////Check if mouse on QuickAccess toolbar
            //else if (QuickAcessToolbar.Visible && QuickAcessToolbar.Bounds.Contains(e.Location))
            //{

            //}
            //else
            //{
            //    //look for mouse on tabs
            //    foreach (RibbonTab tab in Tabs)
            //    {
            //        if (tab.TabBounds.Contains(e.X, e.Y))
            //        {
            //            SetSelectedTab(tab);
            //            someTabHitted = true;
            //            tab.OnMouseMove(e);
            //        }
            //    }
            //}

            //if (!someTabHitted)
            //    SetSelectedTab(null);

            ////Clear the orb highlight
            //if (OrbSelected && !OrbBounds.Contains(e.Location))
            //{
            //    OrbSelected = false;
            //    Invalidate(OrbBounds);
            //}
            #endregion
        }

        /// <summary>
        /// Overriden. Raises the MouseUp event and tunnels the message to child elements
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Overriden. Raises the MouseDown event and tunnels the message to child elements
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Kevin Carbis - this fixes the focus problem with textboxes when a different item is clicked
            //and the edit box is still visible. This will now close the edit box for the textbox that previously
            //had the focus. Otherwise the first click on a button would close the textbox and you would have to click twice.
            //Control.Focus();
            if (ActiveTextBox != null)
                (ActiveTextBox as RibbonTextBox).EndEdit();

            base.OnMouseDown(e);

            if (OrbBounds.Contains(e.Location))
            {
                OrbMouseDown();
            }
            else
            {
                TabHitTest(e.X, e.Y);
            }

        }

        /// <summary>
        /// Handles the mouse wheel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            //if (Tabs.Count == 0 || ActiveTab == null) return;

            //int index = Tabs.IndexOf(ActiveTab);

            //if (e.Delta < 0)
            //{
            //   _tabSum += 0.4f;
            //}
            //else
            //{
            //   _tabSum -= 0.4f;
            //}

            //int tabRounded = Convert.ToInt16(Math.Round(_tabSum));

            //if (tabRounded != 0)
            //{
            //   index += tabRounded;

            //   if (index < 0)
            //   {
            //      index = 0;
            //   }
            //   else if (index >= Tabs.Count - 1)
            //   {
            //      index = Tabs.Count - 1;
            //   }

            //   ActiveTab = Tabs[index];
            //   _tabSum = 0f;
            //}
        }

        /// <summary>
        /// Handles the MouseMove on a RibbonHost control. Not the best solution, but there
        /// is no simple method to hook into the host controls.
        /// </summary>
        /// <param name="e"></param>
        internal void OnRibbonHostMouseMove(MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        /// <summary>
        /// Overriden. Raises the OnSizeChanged event and performs layout calculations
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateRegions();

            RemoveHelperControls();

            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Handles when its parent has changed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (!(Site != null && Site.DesignMode))
            {
                BorderMode = BorderMode;

                if (Parent is IRibbonForm)
                {
                    FormHelper.Ribbon = this;
                }
            }

            if (Parent != null)
            {
                Control p = Parent;
                while (p.Parent != null)
                    p = p.Parent;
                Form parentForm = p as Form;
                if (parentForm != null)
                    parentForm.Deactivate += parentForm_Deactivate;
            }
        }

        private void parentForm_Deactivate(object sender, EventArgs e)
        {
            if (Form.ActiveForm == null)  // check for ActiveForm, because Click in Orb Menu causes the Form as well to fire the Deactivate Event
            {
                RibbonPopupManager.Dismiss(RibbonPopupManager.DismissReason.AppFocusChanged);
            }
        }

        private void OnPopupRegistered(object sender, EventArgs args)
        {
            if (RibbonPopupManager.PopupCount == 1)
                SetUpHooks();
        }

        private void OnPopupUnregistered(object sender, EventArgs args)
        {
            if (RibbonPopupManager.PopupCount == 0 && (Minimized == false || (Minimized && Expanded == false)))
                DisposeHooks();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            // Because the RibbonItem, RibbonPanel and RibbonTab return "Visible == false" if any of
            // their parents is not visible, a redraw is required when the Ribbon becomes visible finally!
            if (Visible)
            {
                UpdateRegions();
                Invalidate();
            }
        }

        #endregion

    }
}