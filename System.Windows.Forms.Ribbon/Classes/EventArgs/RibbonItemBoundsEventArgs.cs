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
    public class RibbonItemBoundsEventArgs
        : RibbonItemRenderEventArgs
    {
        public RibbonItemBoundsEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonItem item, Rectangle bounds)
            : base(owner, g, clip, item)
        {
            Bounds = bounds;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the suggested bounds
        /// </summary>
        public Rectangle Bounds { get; set; }

        #endregion
    }
}
