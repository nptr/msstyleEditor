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


using System.ComponentModel.Design;

namespace System.Windows.Forms
{
    public class RibbonQuickAccessToolbarItemCollectionEditor
        : CollectionEditor
    {
        public RibbonQuickAccessToolbarItemCollectionEditor()
            : base(typeof(RibbonQuickAccessToolbarItemCollection))
        {

        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(RibbonItem);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new[] {
            typeof(RibbonButton),
            typeof(RibbonComboBox),
            typeof(RibbonSeparator),
            typeof(RibbonTextBox),
            typeof(RibbonColorChooser),
            typeof(RibbonCheckBox),
            typeof(RibbonUpDown),
            typeof(RibbonLabel),
            typeof(RibbonHost)
         };
        }
    }
}
