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
    /// <summary>
    /// Large menu item with a description below the text
    /// </summary>
    public class RibbonDescriptionMenuItem
         : RibbonButton
    {
        #region Fields

        #endregion

        #region Ctor

        public RibbonDescriptionMenuItem()
        {
            DropDownArrowDirection = RibbonArrowDirection.Left;
            SetDropDownMargin(new Padding(10));
        }

        /// <summary>
        /// Creates a new menu item with description
        /// </summary>
        /// <param name="text">Text of the menuitem</param>
        public RibbonDescriptionMenuItem(string text)
            : this(null, text, null)
        { }

        /// <summary>
        /// Creates a new menu item with description
        /// </summary>
        /// <param name="text">Text of the menuitem</param>
        /// <param name="description">Descripion of the menuitem</param>
        public RibbonDescriptionMenuItem(string text, string description)
            : this(null, text, description)
        { }

        /// <summary>
        /// Creates a new menu item with description
        /// </summary>
        /// <param name="image">Image for the menuitem</param>
        /// <param name="text">Text for the menuitem</param>
        /// <param name="description">Description for the menuitem</param>
        public RibbonDescriptionMenuItem(Image image, string text, string description)
        {
            Image = image;
            Text = text;
            Description = description;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the bounds of the description
        /// </summary>
        public Rectangle DescriptionBounds { get; set; }

        /// <summary>
        /// This property is not relevant for this class
        /// </summary>
        [Browsable(false)]
        public override Image LargeImage
        {
            get => base.Image;
            set => base.Image = value;
        }

        [DefaultValue(null)]
        [Browsable(true)]
        [Category("Appearance")]
        public override Image Image
        {
            get => base.Image;
            set
            {
                base.Image = value;

                SmallImage = value;
            }
        }

        /// <summary>
        /// This property is not relevant for this class
        /// </summary>
        [Browsable(false)]
        public override Image SmallImage
        {
            get => base.SmallImage;
            set => base.SmallImage = value;
        }

        /// <summary>
        /// Gets or sets the description of the button
        /// </summary>
        [DefaultValue(null)]
        public string Description { get; set; }

        #endregion

        #region Methods

        protected override void OnPaintText(RibbonElementPaintEventArgs e)
        {
            if (e.Mode == RibbonElementSizeMode.DropDown)
            {
                StringFormat sf = StringFormatFactory.NearCenter();

                Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(
                     Owner, e.Graphics, e.Clip, this, TextBounds, Text, Color.Empty, FontStyle.Bold, sf));

                sf.Alignment = StringAlignment.Near;

                Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(
                     Owner, e.Graphics, e.Clip, this, DescriptionBounds, Description, sf));
            }
            else
            {
                base.OnPaintText(e);
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (!Visible && !Owner.IsDesignMode())
            {
                SetLastMeasuredSize(new Size(0, 0));
                return LastMeasuredSize;
            }

            Size s = base.MeasureSize(sender, e);

            s.Height = 52;

            SetLastMeasuredSize(s);

            return s;
        }

        internal override Rectangle OnGetTextBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            Rectangle r = base.OnGetTextBounds(sMode, bounds);
            DescriptionBounds = r;

            r.Height = 20;

            DescriptionBounds = Rectangle.FromLTRB(DescriptionBounds.Left, r.Bottom, DescriptionBounds.Right, DescriptionBounds.Bottom);

            return r;
        }

        #endregion
    }
}
