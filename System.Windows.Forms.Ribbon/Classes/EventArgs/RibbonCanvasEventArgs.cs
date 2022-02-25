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
    public class RibbonCanvasEventArgs
        : EventArgs
    {
        #region ctor

        public RibbonCanvasEventArgs(
            Ribbon owner, Graphics g, Rectangle bounds, Control canvas, object relatedObject
            )
        {
            Owner = owner;
            Graphics = g;
            Bounds = bounds;
            Canvas = canvas;
            RelatedObject = relatedObject;
        }

        #endregion

        #region Props

        public object RelatedObject { get; set; }


        /// <summary>
        /// Gets or sets the Ribbon that raised the event
        /// </summary>
        public Ribbon Owner { get; set; }

        /// <summary>
        /// Gets or sets the graphics to paint
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets or sets the bounds that should be painted
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the control where to be painted
        /// </summary>
        public Control Canvas { get; set; }

        #endregion
    }
}
