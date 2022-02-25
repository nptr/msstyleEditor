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


using System.Collections.Generic;

namespace System.Windows.Forms
{
    /// <summary>
    /// Represents a collection of items that is hosted by the RibbonItemGroup
    /// </summary>
    public class RibbonItemGroupItemCollection : RibbonItemCollection
    {
        /// <param name="ownerGroup">Group that this collection belongs to</param>
        internal RibbonItemGroupItemCollection(RibbonItemGroup ownerGroup)
        {
            OwnerGroup = ownerGroup;
        }
        /// <summary>
        /// Gets the group that owns this item collection
        /// </summary>
        public RibbonItemGroup OwnerGroup { get; }

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public override void Add(RibbonItem item)
        {
            item.MaxSizeMode = RibbonElementSizeMode.Compact;
            item.SetOwnerItem(OwnerGroup);
            base.Add(item);
        }

        /// <summary>
        /// Adds the specified range of items
        /// </summary>
        /// <param name="items">Items to add</param>
        public override void AddRange(IEnumerable<RibbonItem> items)
        {
            foreach (RibbonItem item in items)
            {
                item.MaxSizeMode = RibbonElementSizeMode.Compact;
                item.SetOwnerItem(OwnerGroup);
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
            item.SetOwnerItem(OwnerGroup);
            base.Insert(index, item);
        }
    }
}
