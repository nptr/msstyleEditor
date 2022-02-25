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
    internal class RibbonOrbMenuItemDesigner
        : RibbonElementWithItemCollectionDesigner
    {
        public override Ribbon Ribbon
        {
            get
            {
                if (Component is RibbonButton button)
                {
                    return button.Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (Component is RibbonButton button)
                {
                    return button.DropDownItems;
                }
                return null;
            }
        }

        protected override DesignerVerbCollection OnGetVerbs()
        {
            return new DesignerVerbCollection(new[] {
                new DesignerVerb("Add DescriptionMenuItem", AddDescriptionMenuItem),
                new DesignerVerb("Add Separator", AddSeparator)
            });
        }
    }
}
