using System.ComponentModel.Design;

namespace System.Windows.Forms
{
    class RibbonOrbRecentItemCollectionEditor
        : CollectionEditor
    {
        public RibbonOrbRecentItemCollectionEditor()
           : base(typeof(RibbonOrbRecentItemCollection))
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(RibbonOrbRecentItem);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new[] {
            typeof(RibbonOrbRecentItem),
            typeof(RibbonSeparator)
         };
        }
    }
}
