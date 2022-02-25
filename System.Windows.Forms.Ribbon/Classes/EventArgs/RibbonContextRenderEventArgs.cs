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
    public sealed class RibbonContextRenderEventArgs : RibbonRenderEventArgs
    {
        public RibbonContextRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonContext context)
            : base(owner, g, clip)
        {
            Context = context;
        }

        /// <summary>
        /// Gets or sets the RibbonTab related to the evennt
        /// </summary>
        public RibbonContext Context { get; set; }
    }
}
