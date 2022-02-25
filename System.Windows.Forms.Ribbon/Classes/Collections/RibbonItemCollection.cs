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
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Classes.Collections;

namespace System.Windows.Forms
{
    [Editor(typeof(RibbonItemCollectionEditor), typeof(UITypeEditor))]
    public class RibbonItemCollection
        : RibbonCollectionBase<RibbonItem>
    {
        #region Fields

        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new ribbon item collection
        /// </summary>
        internal RibbonItemCollection()
           : base(null)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the RibbonPanel where this item is located
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonPanel OwnerPanel { get; private set; }

        /// <summary>
        /// Gets the RibbonTab that contains this item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab OwnerTab { get; private set; }

        /// <summary>
        /// Gets the RibbonItem that contains this item
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonItem OwnerItem { get; private set; }

        #endregion

        /// <summary>
        /// Sets the owner Tab of the collection
        /// </summary>
        /// <param name="tab"></param>
        internal void SetOwnerTab(RibbonTab tab)
        {
            OwnerTab = tab;
            foreach (RibbonItem item in this)
            {
                item.SetOwnerTab(tab);
            }
        }

        /// <summary>
        /// Sets the owner panel of the collection
        /// </summary>
        /// <param name="panel"></param>
        internal void SetOwnerPanel(RibbonPanel panel)
        {
            OwnerPanel = panel;
            foreach (RibbonItem item in this)
            {
                item.SetOwnerPanel(panel);
            }
        }

        /// <summary>
        /// Sets the owner item of the collection
        /// </summary>
        /// <param name="item"></param>
        internal void SetOwnerItem(RibbonItem item)
        {
            OwnerItem = item;
            foreach (RibbonItem subitem in this)
            {
                subitem.SetOwnerItem(item);
            }
        }

        #region Overrides

        /// <inheritdoc />
        /// <summary>
        /// Sets the owner Ribbon of the collection
        /// </summary>
        /// <param name="owner"></param>
        internal override void SetOwner(Ribbon owner)
        {
            base.SetOwner(owner);
            foreach (RibbonItem item in this)
            {
                item.SetOwner(owner);
            }
        }

        internal override void SetOwner(RibbonItem item)
        {
            item.SetOwner(Owner);
            item.SetOwnerPanel(OwnerPanel);
            item.SetOwnerTab(OwnerTab);
            item.SetOwnerItem(OwnerItem);
        }

        /// <summary>
        /// Clears the owner of the given <see cref="RibbonItem"/>.
        /// </summary>
        /// <param name="item">the RibbonItem to clear the owner</param>
        internal override void ClearOwner(RibbonItem item)
        {
            item.ClearOwner();
        }

        /// <summary>
        /// Notifies the <see cref="OwnerTab"/> and <see cref="OwnerPanel"/> about changes in the <see cref="RibbonItemCollection"/>.
        /// </summary>
        internal override void UpdateRegions()
        {
            try
            {
                OwnerTab?.UpdatePanelsRegions();
                if (Owner != null && Owner.IsDisposed == false)
                {
                    Owner.UpdateRegions();
                    Owner.Invalidate();
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the left of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsLeft(IEnumerable<RibbonItem> items)
        {
            if (Count == 0) return 0;

            int min = int.MaxValue;

            foreach (RibbonItem item in items)
            {
                if (item.Bounds.X < min)
                {
                    min = item.Bounds.X;
                }
            }

            return min;
        }

        /// <summary>
        /// Gets the right of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsRight(IEnumerable<RibbonItem> items)
        {
            if (Count == 0) return 0;

            int max = int.MinValue; ;

            foreach (RibbonItem item in items)
            {
                if (item.Bounds.Right > max)
                {
                    max = item.Bounds.Right;
                }
            }

            return max;
        }

        /// <summary>
        /// Gets the top of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsTop(IEnumerable<RibbonItem> items)
        {
            if (Count == 0) return 0;

            int min = int.MaxValue;

            foreach (RibbonItem item in items)
            {
                if (item.Bounds.Y < min)
                {
                    min = item.Bounds.Y;
                }
            }

            return min;
        }

        /// <summary>
        /// Gets the bottom of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsBottom(IEnumerable<RibbonItem> items)
        {
            if (Count == 0) return 0;

            int max = int.MinValue;

            foreach (RibbonItem item in items)
            {
                if (item.Bounds.Bottom > max)
                {
                    max = item.Bounds.Bottom;
                }
            }

            return max;
        }

        /// <summary>
        /// Gets the width of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsWidth(IEnumerable<RibbonItem> items) => GetItemsRight(items) - GetItemsLeft(items);

        /// <summary>
        /// Gets the height of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsHeight(IEnumerable<RibbonItem> items) => GetItemsBottom(items) - GetItemsTop(items);

        /// <summary>
        /// Gets the bounds of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal Rectangle GetItemsBounds(IEnumerable<RibbonItem> items) => Rectangle.FromLTRB(GetItemsLeft(items), GetItemsTop(items), GetItemsRight(items), GetItemsBottom(items));

        /// <summary>
        /// Gets the left of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsLeft()
        {
            if (Count == 0) return 0;

            int min = int.MaxValue;

            foreach (RibbonItem item in this)
            {
                if (item.Bounds.X < min)
                {
                    min = item.Bounds.X;
                }
            }

            return min;
        }

        /// <summary>
        /// Gets the right of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsRight()
        {
            if (Count == 0) return 0;

            int max = int.MinValue; ;

            foreach (RibbonItem item in this)
            {
                if (item.Visible && item.Bounds.Right > max)
                {
                    max = item.Bounds.Right;
                }
            }
            if (max == int.MinValue) { max = 0; }

            return max;
        }

        /// <summary>
        /// Gets the top of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsTop()
        {
            if (Count == 0) return 0;

            int min = int.MaxValue;

            foreach (RibbonItem item in this)
            {
                if (item.Bounds.Y < min)
                {
                    min = item.Bounds.Y;
                }
            }

            return min;
        }

        /// <summary>
        /// Gets the bottom of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsBottom()
        {
            if (Count == 0) return 0;

            int max = int.MinValue;

            foreach (RibbonItem item in this)
            {
                if (item.Visible && item.Bounds.Bottom > max)
                {
                    max = item.Bounds.Bottom;
                }
            }
            if (max == int.MinValue) { max = 0; }

            return max;
        }

        /// <summary>
        /// Gets the width of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsWidth()
        {
            return GetItemsRight() - GetItemsLeft();
        }

        /// <summary>
        /// Gets the height of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal int GetItemsHeight()
        {
            return GetItemsBottom() - GetItemsTop();
        }

        /// <summary>
        /// Gets the bounds of items as a group of shapes
        /// </summary>
        /// <returns></returns>
        internal Rectangle GetItemsBounds()
        {
            return Rectangle.FromLTRB(GetItemsLeft(), GetItemsTop(), GetItemsRight(), GetItemsBottom());
        }

        /// <summary>
        /// Moves the bounds of items as a group of shapes
        /// </summary>
        /// <param name="p"></param>
        internal void MoveTo(Point p)
        {
            MoveTo(this, p);
        }

        /// <summary>
        /// Moves the bounds of items as a group of shapes
        /// </summary>
        /// <param name="items"></param>
        /// <param name="p"></param>
        internal void MoveTo(IEnumerable<RibbonItem> items, Point p)
        {
            Rectangle oldBounds = GetItemsBounds(items);

            foreach (RibbonItem item in items)
            {
                int dx = item.Bounds.X - oldBounds.Left;
                int dy = item.Bounds.Y - oldBounds.Top;

                item.SetBounds(new Rectangle(new Point(p.X + dx, p.Y + dy), item.Bounds.Size));
            }
        }

        /// <summary>
        /// Centers the items on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsInto(Rectangle rectangle)
        {
            CenterItemsInto(this, rectangle);
        }

        /// <summary>
        /// Centers the items vertically on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsVerticallyInto(Rectangle rectangle)
        {
            CenterItemsVerticallyInto(this, rectangle);
        }

        /// <summary>
        /// Centers the items horizontally on the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        internal void CenterItemsHorizontallyInto(Rectangle rectangle)
        {
            CenterItemsHorizontallyInto(this, rectangle);
        }

        /// <summary>
        /// Centers the items on the specified rectangle
        /// </summary>
        /// <param name="items"></param>
        /// <param name="rectangle"></param>
        internal void CenterItemsInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
        {
            int x = rectangle.Left + (rectangle.Width - GetItemsWidth()) / 2;
            int y = rectangle.Top + (rectangle.Height - GetItemsHeight()) / 2;

            MoveTo(items, new Point(x, y));
        }

        /// <summary>
        /// Centers the items vertically on the specified rectangle
        /// </summary>
        /// <param name="items"></param>
        /// <param name="rectangle"></param>
        internal void CenterItemsVerticallyInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
        {
            int x = GetItemsLeft(items);
            int y = rectangle.Top + (rectangle.Height - GetItemsHeight(items)) / 2;

            MoveTo(items, new Point(x, y));
        }

        /// <summary>
        /// Centers the items horizontally on the specified rectangle
        /// </summary>
        /// <param name="items"></param>
        /// <param name="rectangle"></param>
        internal void CenterItemsHorizontallyInto(IEnumerable<RibbonItem> items, Rectangle rectangle)
        {
            int x = rectangle.Left + (rectangle.Width - GetItemsWidth(items)) / 2;
            int y = GetItemsTop(items);

            MoveTo(items, new Point(x, y));
        }

        /// <summary>
        /// Checks for the restrictions that buttons should have on the RibbonButton List
        /// </summary>
        /// <param name="item"></param>
        private void CheckRestrictions(RibbonItem item)
        {
            if (OwnerItem != null)
            {
                // A DropDown menu
                //if (item is RibbonDescriptionMenuItem)
                //{
                //    throw new ArgumentException("The only style supported by the RibbonButtonList is Normal", nameof(item));
                //}
            }
            else
            {
                // A Panel
                if (item is RibbonDescriptionMenuItem)
                {
                    throw new ArgumentException("The RibbonDescriptionMenuItem item is not supported on a panel", nameof(item));
                }
            }
        }

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public override void Add(RibbonItem item)
        {
            CheckRestrictions(item);

            item.SetOwner(Owner);
            item.SetOwnerPanel(OwnerPanel);
            item.SetOwnerTab(OwnerTab);
            item.SetOwnerItem(OwnerItem);

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
                CheckRestrictions(item);

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
            CheckRestrictions(item);

            item.SetOwner(Owner);
            item.SetOwnerPanel(OwnerPanel);
            item.SetOwnerTab(OwnerTab);
            item.SetOwnerItem(OwnerItem);

            base.Insert(index, item);
        }

        #endregion
    }
}