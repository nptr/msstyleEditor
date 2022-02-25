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
    /// Represents a context on the Ribbon
    /// </summary>
    /// <remarks>Contexts are useful when some tabs are volatile, depending on some selection. A RibbonTabContext can be added to the ribbon by calling Ribbon.Contexts.Add</remarks>
    [ToolboxItem(false)]
    [DesignTimeVisible(false)]
    public class RibbonContext
        : Component, IRibbonElement
    {
        #region Fields

        private string _text;
        private Color _glowColor;

        private bool _visible;
        //private ContextualTabCollection _tabs;
        public event EventHandler OwnerChanged;

        #endregion

        #region Ctor

        #endregion

        #region Events

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the text of the Context
        /// </summary>
        [Category("Appearance")]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                if (Owner != null) Owner.OnRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the color of the glow that indicates a context
        /// </summary>
        [Category("Appearance")]
        public Color GlowColor
        {
            get => _glowColor;
            set
            {
                _glowColor = value;
                if (Owner != null) Owner.OnRegionsChanged();
            }
        }

        /// <summary>
        /// Gets or sets the visibility of this tab
        /// </summary>
        /// <remarks>Tabs on a context are highlighted with a special glow color</remarks>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool Visible
        {
            get
            {
                if (Owner != null && !Owner.IsDesignMode() && !Owner.Visible)
                    return false;
                return _visible;
            }
            set
            {
                _visible = value;

                if (Owner != null)
                {
                    if (_visible)
                    {
                        if (ContextualTabsCount > 0)
                            Owner.ActiveTab = ContextualTabs[0];
                    }
                    else
                    {
                        Owner.ActiveTab = Owner.Tabs[0];
                    }
                }

                if (Owner != null) Owner.OnRegionsChanged();
            }
        }

        /// <summary>
        /// Gets the <see cref="Bounds"/> property value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Gets the bounds of the context header (the portion above the tab).
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle HeaderBounds { get; private set; }

        /// <summary>
        /// An enumerable list of RibbonTabs assigned to the context.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<RibbonTab> ContextualTabs
        {
            get
            {
                List<RibbonTab> myList = new List<RibbonTab>();
                foreach (RibbonTab tab in Owner.Tabs)
                {
                    if (tab.Context == this)
                        myList.Add(tab);
                }

                return myList;
            }
        }


        /// <summary>
        /// Context tab count.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ContextualTabsCount
        {
            get
            {
                int total = 0;
                foreach (RibbonTab tab in Owner.Tabs)
                {
                    if (tab.Context == this)
                        total++;
                }
                return total;
            }
        }

        /// <summary>
        /// Gets the Ribbon that owns this context
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// This method is not relevant for this class
        /// </summary>
        /// <exception cref="NotSupportedException">Always</exception>
        public void SetBounds(Rectangle bounds)
        {
            Bounds = bounds;

            //OnBoundsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// This method is not relevant for this class
        /// </summary>
        /// <exception cref="NotSupportedException">Always</exception>
        public void SetHeaderBounds(Rectangle bounds)
        {
            HeaderBounds = bounds;

            //OnBoundsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            Owner = owner;
            //_tabs.SetOwner(owner);
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
        /// Measures the size of the context.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!Visible && !Owner.IsDesignMode()) return new Size(0, 0);

            return Size.Ceiling(e.Graphics.MeasureString(string.IsNullOrEmpty(Text) ? "   " : Text, Owner.Font));
        }

        public void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (Owner == null) return;

            if (ContextualTabsCount > 0 || Owner.IsDesignMode())
            {
                Owner.Renderer.OnRenderRibbonContext(new RibbonContextRenderEventArgs(Owner, e.Graphics, e.Clip, this));
                Owner.Renderer.OnRenderRibbonContextText(new RibbonContextRenderEventArgs(Owner, e.Graphics, e.Clip, this));
            }
        }

        public void OnOwnerChanged(EventArgs e)
        {
            if (OwnerChanged != null)
            {
                OwnerChanged(this, e);
            }
        }

        #endregion

    }
}
