/*
 
 //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
 * Copied from original class of RibbonTabRenderEventArgs just changed what i needed for a ribbon Button.
 * 
 * Please give me credit if you use this code. It's all I ask.
 * 
 * Contact me for more info: mspradlin2010@gmail.com
 * 
 */

using System.Drawing;

namespace System.Windows.Forms
{
    public sealed class RibbonButtonRenderEventArgs : RibbonRenderEventArgs
    {
        public RibbonButtonRenderEventArgs(Ribbon owner, Graphics g, Rectangle clip, RibbonButton button)
            : base(owner, g, clip)
        {
            Button = button;
        }

        /// <summary>
        /// Gets or sets the RibbonButton related to the evennt
        /// </summary>
        public RibbonButton Button { get; set; }
    }
}
