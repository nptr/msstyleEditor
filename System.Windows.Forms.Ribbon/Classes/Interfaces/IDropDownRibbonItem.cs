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
    public interface IDropDownRibbonItem
    {
        RibbonItemCollection DropDownItems { get; }

        Rectangle DropDownButtonBounds { get; }

        bool DropDownButtonVisible { get; }

        bool DropDownButtonSelected { get; }

        bool DropDownButtonPressed { get; }
    }
}
