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

using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class RibbonHost : RibbonItem
    {
        private Control ctl;
        private Font ctlFont;
        private Size ctlSize;
        private RibbonElementSizeMode _lastSizeMode;
        public event MouseEventHandler ClientMouseMove;

        /// <summary>
        /// Occurs when the SizeMode of the controls container is changing. if you manually set the size of the control you need to set the Handled flag to true.
        /// </summary>
        [Description("Occurs when the SizeMode of the Controls container is changing. if you manually set the size of the control you need to set the Handled flag to true.")]
        public event RibbonHostSizeModeHandledEventHandler SizeModeChanging;
        public delegate void RibbonHostSizeModeHandledEventHandler(object sender, RibbonHostSizeModeHandledEventArgs e);

        /// <summary>
        /// Gets or sets the control that this item will host
        /// </summary>
        public Control HostedControl
        {
            get => ctl;
            set
            {
                ctl = value;
                NotifyOwnerRegionsChanged();

                //_mouseHook = new RibbonHelpers.GlobalHook(RibbonHelpers.GlobalHook.HookTypes.Mouse);
                //_mouseHook.MouseDown += new MouseEventHandler(_mouseHook_MouseDown);
                //_mouseHook.MouseUp += new MouseEventHandler(_mouseHook_MouseUp);

                if (ctl != null && Site == null)
                {
                    //changing the owner changes the font so let save it for future use
                    ctlFont = ctl.Font;
                    ctlSize = ctl.Size;

                    //hook into some needed events
                    ctl.MouseMove += ctl_MouseMove;
                    CanvasChanged += RibbonHost_CanvasChanged;
                    //we must know when our tab changes so we can hide the control
                    if (OwnerTab != null)
                        Owner.ActiveTabChanged += Owner_ActiveTabChanged;

                    //the control must always have the same parent as the host item so set it here.
                    if (Owner != null)
                        Owner.Controls.Add(ctl);

                    ctl.Font = ctlFont;

                    /*Initially set the control to be hidden. If it needs to be displayed immediately it will get set in the
                     * placecontrol function. This was located at the top of the function, however it caused the control 
                     * if located on a dropdown to not appear the correct size. A second showing fixed the issue. Seems to
                     * because the control is added to the ribbon while invisible??? */
                    ctl.Visible = false;

                }
            }
        }

        /// <summary>
        /// Raises the paint event and draws the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            if (Owner != null)
            {
                StringFormat f = StringFormatFactory.CenterNoWrap(StringTrimming.None);
                if (Site != null && Site.DesignMode)
                {
                    Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(Owner, e.Graphics, Bounds, this, Bounds, Site.Name, f));
                }
                else
                {
                    Owner.Renderer.OnRenderRibbonItemText(new RibbonTextEventArgs(Owner, e.Graphics, Bounds, this, Bounds, Text, f));
                    if (ctl != null)
                    {
                        if (ctl.Parent != Canvas)
                            Canvas.Controls.Add(ctl);

                        //time to show our control
                        ctl.Location = new Point(Bounds.Left, (SizeMode == RibbonElementSizeMode.DropDown) ? Bounds.Top : Bounds.Top);
                        ctl.Visible = true;
                        ctl.BringToFront();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the bounds of the panel
        /// </summary>
        /// <param name="bounds"></param>
        public override void SetBounds(Rectangle bounds)
        {
            base.SetBounds(bounds);
        }
        /// <summary>
        /// Measures the size of the panel on the mode specified by the event object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        /// 
        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {
            if (Site != null && Site.DesignMode && Owner != null)
            {
                //when in design mode just paint the name of this control
                int Width = Convert.ToInt32(e.Graphics.MeasureString(Site.Name, Owner.Font).Width);
                int Height = 20;
                SetLastMeasuredSize(new Size(Width, Height));
            }
            else if (ctl == null || !Visible)
                SetLastMeasuredSize(new Size(0, 0));
            else
            {
                ctl.Visible = false;
                if (_lastSizeMode != e.SizeMode)
                {
                    _lastSizeMode = e.SizeMode;
                    RibbonHostSizeModeHandledEventArgs hev = new RibbonHostSizeModeHandledEventArgs(e.Graphics, e.SizeMode);
                    OnSizeModeChanging(ref hev);
                }
                SetLastMeasuredSize(new Size(ctl.Size.Width, ctl.Size.Height));
            }
            return LastMeasuredSize;
        }

        /// <summary>
        /// Call this method when you need to close a popup that the control is contained in
        /// </summary>
        public void HostCompleted()
        {
            //Kevin Carbis - Clear everything by simulating the click event on the parent item
            //just in case we are in a popup window
            OnClick(new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0));
        }

        /// <summary>
        /// Raises the <see cref="SizeModeChanging"/> event
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnSizeModeChanging(ref RibbonHostSizeModeHandledEventArgs e)
        {
            if (SizeModeChanging != null)
            {
                SizeModeChanging(this, e);
            }
        }

        private void PlaceControls()
        {
            if (ctl != null && Site == null)
            {
                ctl.Location = new Point(Bounds.Left + 1, Bounds.Top + 1);
                //if we are located directly in a panel then we need to make sure the panel is not in overflow
                //mode or we will look bad showing on the panel when we shouldn't be
                if ((Canvas is Ribbon) && (OwnerPanel != null && OwnerPanel.SizeMode == RibbonElementSizeMode.Overflow))
                    ctl.Visible = false;
            }
        }

        private void ctl_MouseMove(object sender, MouseEventArgs e)
        {
            // Note: Unlike all other Ribbon components, e.X and e.Y coordinates are based on the controls Top and Left, not the Ribbons.

            //Console.WriteLine(e.Location.ToString());

            /* If control is on the Ribbon send the mouse move event to the Ribbon directly, converting from
             * control coordinates to Ribbon coordinates. */
            if (OwnerItem == null)
            {
                MouseEventArgs eRibbonArgs = new MouseEventArgs(e.Button, e.Clicks,
                    Owner.PointToClient(ctl.PointToScreen(e.Location)).X,
                    Owner.PointToClient(ctl.PointToScreen(e.Location)).Y, e.Delta);
                /* This should probably raise an event, however there is no reference to hosted controls on the Ribbon.
                 * In the future, probably better to have a control collection for hosted controls on the Ribbon and monitor the
                 * mousemove there and filter down. */
                Owner.OnRibbonHostMouseMove(eRibbonArgs);
            }
            else
            {
                // Raise MouseMove event to any container (i.e. containing DropDown).
                MouseEventArgs eContArgs = new MouseEventArgs(e.Button, e.Clicks, Bounds.Left + e.X, Bounds.Top + e.Y, e.Delta);
                if (ClientMouseMove != null)
                    ClientMouseMove(this, eContArgs);
            }

            OnMouseMove(e);

        }

        private void Owner_ActiveTabChanged(object sender, EventArgs e)
        {
            //hide this control if our tab is not the active tab
            if (ctl != null && OwnerTab != null && Owner.ActiveTab != OwnerTab)
                ctl.Visible = false;
        }
        private void RibbonHost_CanvasChanged(object sender, EventArgs e)
        {
            if (ctl != null)
            {
                Canvas.Controls.Add(ctl);
                ctl.Font = ctlFont;
                //ctl.Location = new System.Drawing.Point(Bounds.Left + 1, Bounds.Top + 1);
            }
        }
        internal override void SetSizeMode(RibbonElementSizeMode sizeMode)
        {
            base.SetSizeMode(sizeMode);
            if (ctl != null && OwnerPanel != null && OwnerPanel.SizeMode == RibbonElementSizeMode.Overflow)
            {
                ctl.Visible = false;
            }
        }
    }
}
