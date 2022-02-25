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
    /// <remarks>Ribbon rendering event data</remarks>
    public class RibbonRenderEventArgs
        : EventArgs
    {
        public RibbonRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip)
        {
            Ribbon = owner;
            Graphics = g;
            ClipRectangle = clip;
        }

        /// <summary>
        /// Gets the Ribbon related to the render
        /// </summary>
        public Ribbon Ribbon { get; set; }

        /// <summary>
        /// Gets the Device to draw into
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets the Rectangle area where to draw into
        /// </summary>
        public Rectangle ClipRectangle { get; set; }
    }
}
