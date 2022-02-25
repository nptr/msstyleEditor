using System.Collections.Generic;
using System.Windows.Forms.RibbonHelpers;

namespace System.Windows.Forms
{
    /// <summary>
    /// Manages opened popups
    /// </summary>
    public static class RibbonPopupManager
    {
        #region Subclasses

        /// <summary>
        /// Specifies reasons why pop-ups can be dismissed
        /// </summary>
        public enum DismissReason
        {
            /// <summary>
            /// An item has been clicked
            /// </summary>
            ItemClicked,

            /// <summary>
            /// The app has been clicked
            /// </summary>
            AppClicked,

            /// <summary>
            /// A new popup is showing and will hide sibling's popups
            /// </summary>
            NewPopup,

            /// <summary>
            /// The aplication window has been deactivated
            /// </summary>
            AppFocusChanged,

            /// <summary>
            /// User has pressed escape on the keyboard
            /// </summary>
            EscapePressed
        }

        #endregion

        #region Events

        public static event EventHandler PopupRegistered;

        public static event EventHandler PopupUnRegistered;

        #endregion

        #region Fields

        private static readonly List<RibbonPopup> pops;

        #endregion

        #region Ctor

        static RibbonPopupManager()
        {
            pops = new List<RibbonPopup>();
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the last pop-up of the collection
        /// </summary>
        internal static RibbonPopup LastPopup
        {
            get
            {
                if (pops.Count > 0)
                {
                    return pops[pops.Count - 1];
                }

                return null;
            }
        }

        internal static int PopupCount => pops.Count;

        #endregion

        #region Methods

        /// <summary>
        /// Registers a popup existance
        /// </summary>
        /// <param name="p"></param>
        internal static void Register(RibbonPopup p)
        {
            if (!pops.Contains(p))
            {
                pops.Add(p);

                PopupRegistered(p, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Unregisters a popup from existance
        /// </summary>
        /// <param name="p"></param>
        internal static void Unregister(RibbonPopup p)
        {
            if (pops.Contains(p))
            {
                pops.Remove(p);

                PopupUnRegistered(p, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Feeds a click generated on the mouse hook
        /// </summary>
        /// <param name="e"></param>
        internal static bool FeedHookClick(MouseEventArgs e)
        {
            //Al-74 fix (see GitHub issue #10):
            //Starting from Windows 10 April 2018 Update (version 1803) with display scaling above 100%, e.Location is the physical mouse location,
            //not scaled according to display scaling, so the Contains function fails check and no events fires when clicking RibbonButtons dropdown items.
            //Use GetCursorPos api instead of e.Location seems to solve the problem.

            WinApi.POINT pos;
            if (WinApi.GetCursorPos(out pos))
            {
                foreach (RibbonPopup p in pops)
                {
                    if (p.WrappedDropDown.Bounds.Contains(pos.x, pos.y))
                    //if (p.WrappedDropDown.Bounds.Contains(e.Location))
                    {
                        return true;
                    }
                }
            }

            //If click was in no dropdown, let's go everyone
            Dismiss(DismissReason.AppClicked);
            return false;
        }

        /// <summary>
        /// Feeds mouse Wheel. If applied on a IScrollableItem it's sended to it
        /// </summary>
        /// <param name="e"></param>
        /// <returns>True if handled. False otherwise</returns>
        internal static bool FeedMouseWheel(MouseEventArgs e)
        {
            RibbonDropDown dd = LastPopup as RibbonDropDown;

            if (dd != null)
            {
                WinApi.POINT pos;
                if (WinApi.GetCursorPos(out pos))
                {
                    foreach (RibbonItem item in dd.Items)
                    {
                        if (dd.RectangleToScreen(item.Bounds).Contains(pos.x, pos.y))
                        //if (dd.RectangleToScreen(item.Bounds).Contains(e.Location))
                        {
                            IScrollableRibbonItem sc = item as IScrollableRibbonItem;

                            if (sc != null)
                            {
                                if (e.Delta < 0)
                                {
                                    sc.ScrollDown();
                                }
                                else
                                {
                                    sc.ScrollUp();
                                }

                                return true;
                            }
                        }
                    }
                }
            }
            //kevin carbis - added scrollbar support to dropdowns so we need to feed the mouse wheel to the 
            //actual dropdown item if it was not intended for a child item.
            if (dd != null)
            {
                if (e.Delta < 0)
                {
                    dd.ScrollDown();
                }
                else
                {
                    dd.ScrollUp();
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Closes all children of specified pop-up
        /// </summary>
        /// <param name="parent">Pop-up of which children will be closed</param>
        /// <param name="reason">Reason for dismissing</param>
        public static void DismissChildren(RibbonPopup parent, DismissReason reason)
        {
            int index = pops.IndexOf(parent);

            if (index >= 0)
            {
                Dismiss(index + 1, reason);
            }
        }

        /// <summary>
        /// Closes all currently registered pop-ups
        /// </summary>
        /// <param name="reason"></param>
        public static void Dismiss(DismissReason reason)
        {
            Dismiss(0, reason);
        }

        /// <summary>
        /// Closes specified pop-up and all its descendants
        /// </summary>
        /// <param name="startPopup">Pop-up to close (and its descendants)</param>
        /// <param name="reason">Reason for closing</param>
        public static void Dismiss(RibbonPopup startPopup, DismissReason reason)
        {
            int index = pops.IndexOf(startPopup);

            if (index >= 0)
            {
                Dismiss(index, reason);
            }
        }

        /// <summary>
        /// Closes pop-up of the specified index and all its descendants
        /// </summary>
        /// <param name="startPopup">Index of the pop-up to close</param>
        /// <param name="reason">Reason for closing</param>
        private static void Dismiss(int startPopup, DismissReason reason)
        {
            for (int i = pops.Count - 1; i >= startPopup; i--)
            {
                pops[i].Close();
            }
        }

        #endregion
    }
}
