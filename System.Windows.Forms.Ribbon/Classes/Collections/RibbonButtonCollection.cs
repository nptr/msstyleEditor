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
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class RibbonButtonCollection : RibbonItemCollection
    {
        internal RibbonButtonCollection(RibbonButtonList list)
        {
            OwnerList = list;
        }

        /// <summary>
        /// Gets the list that owns the collection (If any)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonButtonList OwnerList { get; }

        /// <summary>
        /// Checks for the restrictions that buttons should have on the RibbonButton List
        /// </summary>
        /// <param name="button"></param>
        private void CheckRestrictions(RibbonButton button)
        {
            if (button == null)
                throw new ArgumentNullException(nameof(button), "The RibbonButtonList only accepts button in the Buttons collection");

            //if (!string.IsNullOrEmpty(button.Text))
            //    throw new ArgumentException("The buttons on the RibbonButtonList should have no text");

            if (button.Style != RibbonButtonStyle.Normal)
                throw new ArgumentException("The only style supported by the RibbonButtonList is Normal");
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public override void Add(RibbonItem item)
        {
            CheckRestrictions(item as RibbonButton);

            item.SetOwner(Owner);
            item.SetOwnerPanel(OwnerPanel);
            item.SetOwnerTab(OwnerTab);
            item.SetOwnerItem(OwnerItem);

            item.Click += OwnerList.item_Click;

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
                CheckRestrictions(item as RibbonButton);

                item.SetOwner(Owner);
                item.SetOwnerPanel(OwnerPanel);
                item.SetOwnerTab(OwnerTab);
                item.SetOwnerItem(OwnerItem);
            }

            base.AddRange(items);
        }

        /// <inheritdoc />
        /// <summary>
        /// Inserts the specified item at the desired index
        /// </summary>
        /// <param name="index">Desired index of the item</param>
        /// <param name="item">Item to insert</param>
        public override void Insert(int index, RibbonItem item)
        {
            CheckRestrictions(item as RibbonButton);

            item.SetOwner(Owner);
            item.SetOwnerPanel(OwnerPanel);
            item.SetOwnerTab(OwnerTab);
            item.SetOwnerItem(OwnerList);

            base.Insert(index, item);
        }
    }
}