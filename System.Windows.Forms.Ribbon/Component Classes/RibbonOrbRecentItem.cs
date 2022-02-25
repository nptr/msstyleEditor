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

using System.Drawing;

namespace System.Windows.Forms
{
    public class RibbonOrbRecentItem
        : RibbonButton
    {
        #region Ctor

        public RibbonOrbRecentItem()
        {

        }

        public RibbonOrbRecentItem(string text)
            : this()
        {
            Text = text;
        }

        #endregion

        #region Methods

        internal override Rectangle OnGetImageBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            return Rectangle.Empty;
        }

        internal override Rectangle OnGetTextBounds(RibbonElementSizeMode sMode, Rectangle bounds)
        {
            Rectangle r = base.OnGetTextBounds(sMode, bounds);

            r.X = Bounds.Left + 3;

            return r;
        }

        #endregion
    }
}
