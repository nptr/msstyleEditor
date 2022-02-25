/*
 
 2011 Thomas Koglbauer
 * 
 */

using System.Drawing;

namespace System.Windows.Forms
{
    public class RibbonDropDownRenderEventArgs
        : EventArgs
    {
        #region ctor

        public RibbonDropDownRenderEventArgs(
            Graphics g, RibbonDropDown dropDown
            )
        {
            Graphics = g;
            DropDown = dropDown;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the graphics to paint
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets or sets the Ribbon DropDown
        /// </summary>
        public RibbonDropDown DropDown { get; set; }

        #endregion
    }
}
