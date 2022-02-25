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


using System.Windows.Forms.Classes.Collections;

namespace System.Windows.Forms
{
    /// <summary>
    /// Represents a collection of RibbonTabContext
    /// </summary>
    public sealed class RibbonContextCollection
        : RibbonCollectionBase<RibbonContext>
    {

        /// <summary>
        /// Creates a new RibbonTabContext Collection
        /// </summary>
        /// <param name="owner">Ribbon that owns this collection</param>
        /// <exception cref="ArgumentNullException">owner is null</exception>
        internal RibbonContextCollection(Ribbon owner)
            : base(owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
        }

        public new void Remove(RibbonContext context)
        {

            foreach (RibbonTab tab in context.ContextualTabs)
            {
                tab.Context = null;
            }

            base.Remove(context);
        }

        public new int RemoveAll(Predicate<RibbonContext> predicate)
        {
            throw new NotSupportedException("RibbonContextCollectin.RemoveAll function is not supported");
        }

        public new void RemoveAt(int index)
        {
            RibbonContext context = this[index];

            foreach (RibbonTab tab in context.ContextualTabs)
            {
                tab.Context = null;
            }

            base.RemoveAt(index);
        }

        public new void RemoveRange(int index, int count)
        {
            throw new NotSupportedException("RibbonContextCollection.RemoveRange function is not supported");
        }

        internal override void SetOwner(RibbonContext item)
        {
            item.SetOwner(Owner);
        }

        internal override void ClearOwner(RibbonContext item)
        {
            item.ClearOwner();
        }

        internal override void UpdateRegions()
        {
            try
            {
                Owner.OnRegionsChanged();
            }
            catch
            {
                // ignored
            }
        }
    }
}
