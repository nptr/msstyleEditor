using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
    [Editor(typeof(RibbonQuickAccessToolbarItemCollectionEditor), typeof(UITypeEditor))]
    public class RibbonQuickAccessToolbarItemCollection : RibbonItemCollection
    {
        #region Fields

        #endregion

        /// <summary>
        /// Creates a new collection
        /// </summary>
        internal RibbonQuickAccessToolbarItemCollection(RibbonQuickAccessToolbar toolbar)
        {
            OwnerToolbar = toolbar;
            SetOwner(toolbar.Owner);
        }

        internal sealed override void SetOwner(Ribbon owner)
        {
            base.SetOwner(owner);
        }

        /// <summary>
        /// Gets the group that owns this item collection
        /// </summary>
        public RibbonQuickAccessToolbar OwnerToolbar { get; }

        /// <inheritdoc />
        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public override void Add(RibbonItem item)
        {
            item.MaxSizeMode = RibbonElementSizeMode.Compact;
            base.Add(item);
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the specified range of items
        /// </summary>
        /// <param name="items">Items to add</param>
        public override void AddRange(IEnumerable<RibbonItem> items)
        {
            foreach (RibbonItem item in items)
            {
                item.MaxSizeMode = RibbonElementSizeMode.Compact;
            }
            base.AddRange(items);
        }

        /// <summary>
        /// Inserts the specified item at the desired index
        /// </summary>
        /// <param name="index">Desired index of the item</param>
        /// <param name="item">Item to insert</param>
        public override void Insert(int index, RibbonItem item)
        {
            item.MaxSizeMode = RibbonElementSizeMode.Compact;
            base.Insert(index, item);
        }
    }
}