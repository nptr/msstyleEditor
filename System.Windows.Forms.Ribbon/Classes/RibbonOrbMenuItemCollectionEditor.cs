using System.ComponentModel.Design;

namespace System.Windows.Forms
{
    class RibbonOrbMenuItemCollectionEditor
        : CollectionEditor
    {
        public RibbonOrbMenuItemCollectionEditor()
           : base(typeof(RibbonOrbMenuItemCollection))
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(RibbonOrbMenuItem);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new[] {
            typeof(RibbonOrbMenuItem),
            typeof(RibbonSeparator)
         };
        }
    }
}
