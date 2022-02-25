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
    public sealed class RibbonTabRenderEventArgs : RibbonRenderEventArgs
    {
        public RibbonTabRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonTab tab)
            : base(owner, g, clip)
        {
            Tab = tab;
        }

        /// <summary>
        /// Gets or sets the RibbonTab related to the evennt
        /// </summary>
        public RibbonTab Tab { get; set; }
    }
}
