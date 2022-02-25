using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
    [Editor(typeof(RibbonOrbRecentItemCollectionEditor), typeof(UITypeEditor))]
    public class RibbonOrbRecentItemCollection : RibbonItemCollection
    {
    }
}
