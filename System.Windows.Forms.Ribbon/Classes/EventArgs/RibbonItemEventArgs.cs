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


namespace System.Windows.Forms
{
    public class RibbonItemEventArgs : EventArgs
    {
        public RibbonItemEventArgs(RibbonItem item)
        {
            Item = item;
        }

        public RibbonItem Item { get; set; }
    }
}
