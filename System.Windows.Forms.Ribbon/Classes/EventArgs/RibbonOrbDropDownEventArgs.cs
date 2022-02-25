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
    public class RibbonOrbDropDownEventArgs
        : RibbonRenderEventArgs
    {
        #region Fields

        #endregion

        #region Ctor
        public RibbonOrbDropDownEventArgs(Ribbon ribbon, RibbonOrbDropDown dropDown, Graphics g, Rectangle clip)
            : base(ribbon, g, clip)
        {
            RibbonOrbDropDown = dropDown;
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets the RibbonOrbDropDown related to the event
        /// </summary>
        public RibbonOrbDropDown RibbonOrbDropDown { get; }

        #endregion
    }
}
