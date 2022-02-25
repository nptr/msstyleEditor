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


using System.Collections.Generic;
using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    /// Exposes GetItems, to indicate that the type contains a collection of RibbonItems
    /// </summary>
    public interface IContainsSelectableRibbonItems
    {
        /// <summary>
        /// When implemented, must return an  enumerator to acces the items inside the type
        /// </summary>
        IEnumerable<RibbonItem> GetItems();

        /// <summary>
        /// When implemented, must return the bounds of the content where items are displayed
        /// </summary>
        /// <returns></returns>
        Rectangle GetContentBounds();
    }
}
