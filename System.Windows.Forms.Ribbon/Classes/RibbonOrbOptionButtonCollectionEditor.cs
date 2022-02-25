using System.ComponentModel.Design;

namespace System.Windows.Forms
{
    class RibbonOrbOptionButtonCollectionEditor
        : CollectionEditor
    {
        public RibbonOrbOptionButtonCollectionEditor()
           : base(typeof(RibbonOrbOptionButtonCollection))
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(RibbonOrbOptionButton);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new[] {
            typeof(RibbonOrbOptionButton)
         };
        }
    }
}
