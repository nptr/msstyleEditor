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
    /// <summary>
    /// Holds data and tools to measure the size
    /// </summary>
    public class RibbonElementMeasureSizeEventArgs
        : EventArgs
    {
        /// <summary>
        /// Creates a new RibbonElementMeasureSizeEventArgs object
        /// </summary>
        /// <param name="graphics">Device info to draw and measure</param>
        /// <param name="sizeMode">Size mode to measure</param>
        internal RibbonElementMeasureSizeEventArgs(Graphics graphics, RibbonElementSizeMode sizeMode)
        {
            Graphics = graphics;
            SizeMode = sizeMode;
        }

        /// <summary>
        /// Gets the size mode to measure
        /// </summary>
        public RibbonElementSizeMode SizeMode { get; }

        /// <summary>
        /// Gets the device to measure objects
        /// </summary>
        public Graphics Graphics { get; }
    }
}
