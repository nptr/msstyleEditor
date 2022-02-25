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
    /// Represents a collection of RibbonTab objects
    /// </summary>
    public sealed class RibbonTabCollection
           : RibbonCollectionBase<RibbonTab>
    {

        /// <summary>
        /// Creates a new RibbonTabCollection
        /// </summary>
        /// <param name="owner">|</param>
        /// <exception cref="ArgumentNullException">owner is null</exception>
        internal RibbonTabCollection(Ribbon owner)
           : base(owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
        }

        internal override void SetOwner(RibbonTab item)
        {
            item.SetOwner(Owner);
        }

        internal override void ClearOwner(RibbonTab item)
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

        /// <summary>
        /// Removes the tab from the collection.
        /// </summary>
        /// <param name="tab">tab to remove</param>
        public new bool Remove(RibbonTab tab)
        {
            /* If the tab being deleted is the active tab, then another tab needs to be selected
             * to prevent errors. */
            if (tab == Owner.ActiveTab)
            {
                if (Owner.Tabs.IndexOf(tab) > 0)
                {
                    // This is not the first tab, make the tab preceeding this one active.
                    Owner.ActiveTab = Owner.Tabs[Owner.Tabs.IndexOf(tab) - 1];
                    Owner.Tabs.Remove(tab);
                }
                else if (Owner.Tabs.IndexOf(tab) < Owner.Tabs.Count - 1)
                {
                    // This is not the last tab , make the tab following this tab active.
                    Owner.ActiveTab = Owner.Tabs[Owner.Tabs.IndexOf(tab) + 1];
                    Owner.Tabs.Remove(tab);
                }
            }

            return base.Remove(tab);
        }

    }
}