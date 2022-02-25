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
    public interface IRibbonToolTip
    {
        /// <summary>
        /// Gets or Sets the ToolTip Text
        /// </summary>
        string ToolTip { get; set; }
        /// <summary>
        /// Gets or Sets the ToolTip Title
        /// </summary>
        string ToolTipTitle { get; set; }
        /// <summary>
        /// Gets or Sets the ToolTip Image
        /// </summary>
        Image ToolTipImage { get; set; }
        /// <summary>
        /// Gets or Sets the stock ToolTip Icon
        /// </summary>
        ToolTipIcon ToolTipIcon { get; set; }

        /// <summary>
        /// Occurs before a ToolTip is initially displayed.
        /// <remarks>Use this event to change the ToolTip or Cancel it at all.</remarks>
        /// </summary>
        event RibbonElementPopupEventHandler ToolTipPopUp;
    }

}
