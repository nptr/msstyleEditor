using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace System.Windows.Forms
{
    public class ProToolstripRenderer : ToolStripProfessionalRenderer
    {
        //private RibbonProfesionalRendererColorTable _colorTable;

        //public RibbonProfesionalRendererColorTable ColorTable
        //{
        //    get { return _colorTable; }
        //    set { _colorTable = value; }
        //}

        //NOTE: THIS IS ALL PROVIDED BY MICROSOFT FOR FREE I CHANGED TO FIT OUR NEEDS --MS 08/01/2009
        //http://msdn.microsoft.com/en-us/library/ms233664.aspx

        #region "Enums"
        public enum Corners
        {
            None = 0,
            NorthWest = 2,
            NorthEast = 4,
            SouthEast = 8,
            SouthWest = 16,
            All = NorthWest | NorthEast | SouthEast | SouthWest,
            North = NorthWest | NorthEast,
            South = SouthEast | SouthWest,
            East = NorthEast | SouthEast,
            West = NorthWest | SouthWest
        }

        public enum ButtonTextAlign
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3,
            UpperLeft = 4,
            UpperRight = 5,
            BottomLeft = 6,
            BottomRight = 7,
            Center = 8,
            MiddleLeft = 9
        }
        #endregion

        /// <summary>
        /// Creates a rectangle with the specified corners rounded
        /// </summary>
        /// <param name="r"></param>
        /// <param name="radius"></param>
        /// <param name="corners"></param>
        /// <returns></returns>
        public static GraphicsPath RoundRectangle(Rectangle r, int radius, Corners corners)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            int nw = (corners & Corners.NorthWest) == Corners.NorthWest ? d : 0;
            int ne = (corners & Corners.NorthEast) == Corners.NorthEast ? d : 0;
            int se = (corners & Corners.SouthEast) == Corners.SouthEast ? d : 0;
            int sw = (corners & Corners.SouthWest) == Corners.SouthWest ? d : 0;

            path.AddLine(r.Left + nw, r.Top, r.Right - ne, r.Top);

            if (ne > 0)
            {
                path.AddArc(Rectangle.FromLTRB(r.Right - ne, r.Top, r.Right, r.Top + ne),
                     -90, 90);
            }

            path.AddLine(r.Right, r.Top + ne, r.Right, r.Bottom - se);

            if (se > 0)
            {
                path.AddArc(Rectangle.FromLTRB(r.Right - se, r.Bottom - se, r.Right, r.Bottom),
                     0, 90);
            }

            path.AddLine(r.Right - se, r.Bottom, r.Left + sw, r.Bottom);

            if (sw > 0)
            {
                path.AddArc(Rectangle.FromLTRB(r.Left, r.Bottom - sw, r.Left + sw, r.Bottom),
                     90, 90);
            }

            path.AddLine(r.Left, r.Bottom - sw, r.Left, r.Top + nw);

            if (nw > 0)
            {
                path.AddArc(Rectangle.FromLTRB(r.Left, r.Top, r.Left + nw, r.Top + nw),
                     180, 90);
            }

            path.CloseFigure();

            return path;
        }

        //Michael Spradlin - 05/03/2013 Office 2013 Style Changes
        public static GraphicsPath FlatRectangle(Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddLine(r.Left, r.Top, r.Right, r.Top);
            path.AddLine(r.Right, r.Top, r.Right, r.Bottom);
            path.AddLine(r.Right, r.Bottom, r.Left, r.Bottom);
            path.AddLine(r.Left, r.Bottom, r.Left, r.Top);
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Gets the corners to round on the specified button
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Corners ToolStripItemCorners(ToolStripItem item)
        {
            if (!(item.Owner is ToolStrip))
            {
                return Corners.All;
            }

            ToolStrip g = item.Owner;
            int iLast = item.Owner.Items.Count - 1;
            Corners c = Corners.None;
            if (item.Owner.Items.IndexOf(item) == 0)
            {
                c |= Corners.West;
            }

            if (item.Owner.Items.IndexOf(item) == iLast)
            {
                c |= Corners.East;
            }

            return c;
        }

        #region "Gripper"

        private static Bitmap titleBarGripBmp;
        //MS 07/27/2009 WHO WOULD HAVE EVER THOUGHT THAT THIS CHARACTER STRING WOULD ACTUALLY DRAW A GRIPPER TOOL BAR
        private static readonly string titleBarGripEnc = "Qk16AQAAAAAAADYAAAAoAAAAIwAAAAMAAAABABgAAAAAAAAAAADEDgAAxA4AAAAAAAAAAAAAuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5ANj+RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5ANj+RzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMANj+";

        private void DrawTitleBar(Graphics g, Rectangle rect)
        {
            // Assign the image for the grip.
            Image titlebarGrip = titleBarGripBmp;

            // Fill the titlebar. 
            SolidBrush b = new SolidBrush(Theme.Standard.RendererColorTable.RibbonBackground);

            g.FillRectangle(b, rect);

            // Center the titlebar grip.
            g.DrawImage(titlebarGrip, new Point(Convert.ToInt32(rect.X + (rect.Width / 2 - titlebarGrip.Width / 2)), rect.Y + 1));
        }

        // This method handles the RenderGrip event.
        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            DrawTitleBar(e.Graphics, new Rectangle(0, 0, e.ToolStrip.Width, 7));
        }

        //' This method handles the RenderToolStripBorder event.

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
        }

        // This utility method cretes a bitmap from 
        // a Base64-encoded string. 
        internal static Bitmap DeserializeFromBase64(string data)
        {
            // Decode the string and create a memory stream 
            // on the decoded string data.
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(data));

            // Create a new bitmap from the stream.
            Bitmap b = new Bitmap(stream);

            return b;
        }
        #endregion

        public ProToolstripRenderer(bool Gripper)
        {
            titleBarGripBmp = DeserializeFromBase64(titleBarGripEnc);
        }

        //RENDER CONTAINER BACKGROUND GRADIANT
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);

            try
            {
                if (Theme.Standard.Style == RibbonOrbStyle.Office_2013)
                {
                    SolidBrush b = new SolidBrush(Theme.Standard.RendererColorTable.RibbonBackground_2013);

                    Rectangle rect = new Rectangle(0, e.ToolStrip.Height - 2, e.ToolStrip.Width, 1);
                    e.Graphics.FillRectangle(b, e.AffectedBounds);
                    e.Graphics.FillRectangle(new SolidBrush(Theme.Standard.RendererColorTable.clrVerBG_Shadow), rect);
                }
                else
                {
                    Rectangle rect = new Rectangle(0, e.ToolStrip.Height - 2, e.ToolStrip.Width, 1);
                    SolidBrush b = new SolidBrush(Theme.Standard.RendererColorTable.RibbonBackground);
                    e.Graphics.FillRectangle(b, e.AffectedBounds);
                    e.Graphics.FillRectangle(new SolidBrush(Theme.Standard.RendererColorTable.clrVerBG_Shadow), rect);
                }

            }
            catch
            {
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderMenuItemBackground(e);

            RenderBackground(e);

            DrawText(e, e.Graphics);
        }

        //RENDER DROP DOWN BUTTON SELECTED, CHECKED, OR UNSELECTED
        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderDropDownButtonBackground(e);

            RenderBackground(e);

            DrawText(e, e.Graphics);
        }

        //RENDER BUTTON SELECTED, CHECKED, OR UNSELECTED
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderButtonBackground(e);

            RenderBackground(e);

            DrawText(e, e.Graphics);
        }

        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderItemBackground(e);

            RenderBackground(e);

            DrawText(e, e.Graphics);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = Theme.Standard.RendererColorTable.Arrow;
            base.OnRenderArrow(e);
        }

        protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderLabelBackground(e);

            RenderBackground(e);

            DrawText(e, e.Graphics);
        }

        private void RenderBackground(ToolStripItemRenderEventArgs e)
        {
            //IF ITEM IS SELECTED OR CHECKED
            var button = e.Item as ToolStripButton;
            if (e.Item.Selected || (button != null && button.Checked))
            {
                RenderItemBackgroundSelected(e);
                return;
            }

            //IF ITEM IS PRESSED
            if (e.Item.Pressed)
            {
                RenderItemBackgroundPressed(e);
                return;
            }

            //DEFAULT BACKGROUND
            RenderItemBackgroundDefault(e);
        }

        private void RenderItemBackgroundSelected(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Bounds.Height <= 0 || e.Item.Bounds.Width <= 0) return;

            if (Theme.Standard.Style == RibbonOrbStyle.Office_2013)
            {
                //Flat Effect
                Rectangle rectBorder = new Rectangle(1, 1, e.Item.Width - 1, e.Item.Height - 1);
                Rectangle rect = new Rectangle(2, 2, e.Item.Width - 2, e.Item.Height - 2);

                using (SolidBrush b = new SolidBrush(Theme.Standard.RendererColorTable.ButtonSelected_2013))
                {
                    using (SolidBrush sb = new SolidBrush(Theme.Standard.RendererColorTable.ButtonBorderIn))
                    {
                        e.Graphics.FillRectangle(sb, rectBorder);
                    }

                    e.Graphics.FillRectangle(b, rect);
                }
            }
            else
            {
                Rectangle rectBorder = new Rectangle(1, 1, e.Item.Width - 1, e.Item.Height - 1);
                Rectangle rect = new Rectangle(2, 2, e.Item.Width - 2, e.Item.Height - 2);

                Rectangle innerR = Rectangle.FromLTRB(1, 1, e.Item.Width - 2, e.Item.Height - 2);
                Rectangle glossyR = Rectangle.FromLTRB(1, 1, e.Item.Width - 2, 1 + Convert.ToInt32(e.Item.Bounds.Height * .36));

                using (SolidBrush brus = new SolidBrush(Theme.Standard.RendererColorTable.ButtonSelectedBgOut))
                {
                    e.Graphics.FillRectangle(brus, rectBorder);
                }

                //Border
                using (Pen p = new Pen(Theme.Standard.RendererColorTable.ButtonSelectedBorderOut))
                {
                    e.Graphics.DrawRectangle(p, rectBorder);
                }

                //Inner border
                Rectangle RoundedRect = Rectangle.Round(innerR);
                using (Pen p = new Pen(Theme.Standard.RendererColorTable.ButtonSelectedBorderIn))
                {
                    e.Graphics.DrawRectangle(p, RoundedRect);
                }

                #region Main Bg
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(new Rectangle(1, 1, e.Item.Width, e.Item.Height * 2));
                    path.CloseFigure();
                    using (PathGradientBrush gradient = new PathGradientBrush(path))
                    {
                        gradient.WrapMode = WrapMode.Clamp;
                        gradient.CenterPoint = new PointF(Convert.ToSingle(1 + e.Item.Width / 2), Convert.ToSingle(e.Item.Bounds.Height));
                        gradient.CenterColor = Theme.Standard.RendererColorTable.ButtonSelectedBgCenter;
                        gradient.SurroundColors = new[] { Theme.Standard.RendererColorTable.ButtonSelectedBgOut };

                        Blend blend = new Blend(3)
                        {
                            Factors = new[] { 0f, 0.8f, 0f },
                            Positions = new[] { 0f, 0.30f, 1f }
                        };


                        Region lastClip = e.Graphics.Clip;
                        Region newClip = new Region(rectBorder);
                        newClip.Intersect(lastClip);
                        e.Graphics.SetClip(newClip.GetBounds(e.Graphics));
                        e.Graphics.FillPath(gradient, path);
                        e.Graphics.Clip = lastClip;
                    }
                }
                #endregion

                //Glossy effect
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddRectangle(Rectangle.Round(glossyR));
                    using (LinearGradientBrush b = new LinearGradientBrush(glossyR, Theme.Standard.RendererColorTable.ButtonSelectedGlossyNorth, Theme.Standard.RendererColorTable.ButtonSelectedGlossySouth, 90))
                    {
                        b.WrapMode = WrapMode.TileFlipXY;
                        e.Graphics.FillPath(b, path);
                    }
                }
            }
        }

        private void RenderItemBackgroundPressed(ToolStripItemRenderEventArgs e)
        {
            if (Theme.Standard.Style == RibbonOrbStyle.Office_2013)
            {
                Rectangle rectBorder = new Rectangle(1, 1, e.Item.Width - 1, e.Item.Height - 1);
                Rectangle rect = new Rectangle(2, 2, e.Item.Width - 2, e.Item.Height - 2);

                using (SolidBrush b = new SolidBrush(Theme.Standard.RendererColorTable.ButtonPressed_2013))
                {
                    using (SolidBrush sb = new SolidBrush(Theme.Standard.RendererColorTable.ButtonBorderOut))
                    {
                        e.Graphics.FillRectangle(sb, rectBorder);
                    }

                    e.Graphics.FillRectangle(b, rect);
                }
            }
            else
            {
                Rectangle rectBorder = new Rectangle(1, 1, e.Item.Width - 1, e.Item.Height - 1);
                Rectangle rect = new Rectangle(2, 2, e.Item.Width - 2, e.Item.Height - 2);

                Rectangle innerR = Rectangle.FromLTRB(1, 1, e.Item.Width - 2, e.Item.Height - 2);
                Rectangle glossyR = Rectangle.FromLTRB(1, 1, e.Item.Width - 2, 1 + Convert.ToInt32(e.Item.Bounds.Height * .36));

                using (SolidBrush brus = new SolidBrush(Theme.Standard.RendererColorTable.ButtonPressedBgOut))
                {
                    e.Graphics.FillRectangle(brus, rectBorder);
                }

                //Border
                using (Pen p = new Pen(Theme.Standard.RendererColorTable.ButtonPressedBorderOut))
                {
                    e.Graphics.DrawRectangle(p, rectBorder);
                }

                //Inner border
                Rectangle RoundedRect = Rectangle.Round(innerR);
                using (Pen p = new Pen(Theme.Standard.RendererColorTable.ButtonPressedBorderIn))
                {
                    e.Graphics.DrawRectangle(p, RoundedRect);
                }

                #region Main Bg
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(new Rectangle(1, 1, e.Item.Width, e.Item.Height * 2));
                    path.CloseFigure();
                    using (PathGradientBrush gradient = new PathGradientBrush(path))
                    {
                        gradient.WrapMode = WrapMode.Clamp;
                        gradient.CenterPoint = new PointF(Convert.ToSingle(1 + e.Item.Width / 2), Convert.ToSingle(e.Item.Bounds.Height));
                        gradient.CenterColor = Theme.Standard.RendererColorTable.ButtonPressedBgCenter;
                        gradient.SurroundColors = new[] { Theme.Standard.RendererColorTable.ButtonPressedBgOut };

                        Blend blend = new Blend(3)
                        {
                            Factors = new[] { 0f, 0.8f, 0f },
                            Positions = new[] { 0f, 0.30f, 1f }
                        };


                        Region lastClip = e.Graphics.Clip;
                        Region newClip = new Region(rectBorder);
                        newClip.Intersect(lastClip);
                        e.Graphics.SetClip(newClip.GetBounds(e.Graphics));
                        e.Graphics.FillPath(gradient, path);
                        e.Graphics.Clip = lastClip;
                    }
                }
                #endregion

                //Glossy effect
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddRectangle(Rectangle.Round(glossyR));
                    using (LinearGradientBrush b = new LinearGradientBrush(glossyR, Theme.Standard.RendererColorTable.ButtonPressedGlossyNorth, Theme.Standard.RendererColorTable.ButtonPressedGlossySouth, 90))
                    {
                        b.WrapMode = WrapMode.TileFlipXY;
                        e.Graphics.FillPath(b, path);
                    }
                }
            }
        }

        private void RenderItemBackgroundDefault(ToolStripItemRenderEventArgs e)
        {
            if (Theme.Standard.Style == RibbonOrbStyle.Office_2013)
            {
                Rectangle rect = new Rectangle(0, 0, e.Item.Width, e.Item.Height);

                using (SolidBrush b = new SolidBrush(Theme.Standard.RendererColorTable.RibbonBackground_2013))
                {
                    e.Graphics.FillRectangle(b, rect);
                }
            }
            else
            {
                Rectangle rect = new Rectangle(0, 0, e.Item.Width, e.Item.Height);

                using (SolidBrush b2 = new SolidBrush(Theme.Standard.RendererColorTable.RibbonBackground))
                {
                    e.Graphics.FillRectangle(b2, rect);
                }
            }
        }

        private void DrawText(ToolStripItemRenderEventArgs e, Graphics graphics)
        {
            try
            {
                Font font = e.Item.Font;

                if (Theme.Standard.Style == RibbonOrbStyle.Office_2013)
                {
                    if (e.Item.Text != string.Empty)
                    {
                        if (e.Item.Enabled)
                        {
                            var button = e.Item as ToolStripButton;
                            if (button != null)
                            {
                                if (button.Checked)
                                {
                                    button.ForeColor = Theme.Standard.RendererColorTable.ToolStripItemTextSelected_2013;
                                }
                                else if (button.Selected || button.Pressed)
                                {
                                    button.ForeColor = Theme.Standard.RendererColorTable.ToolStripItemTextPressed_2013;
                                }
                                else
                                {
                                    button.ForeColor = Theme.Standard.RendererColorTable.ToolStripItemText_2013;
                                }
                            }
                            else if (e.Item is ToolStripLabel)
                            {
                                e.Item.ForeColor = Theme.Standard.RendererColorTable.ToolStripItemText_2013;
                            }
                        }
                        else
                        {
                            if (e.Item.ForeColor != Color.DarkGray)
                            {
                                e.Item.ForeColor = Color.DarkGray;
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(e.Item.Text))
                    {
                        if (e.Item.Enabled)
                        {
                            var button = e.Item as ToolStripButton;
                            if (button != null)
                            {
                                if (button.Checked)
                                {
                                    button.ForeColor = Theme.Standard.RendererColorTable.ToolStripItemTextSelected;
                                }
                                else if (button.Selected || button.Pressed)
                                {
                                    button.ForeColor = Theme.Standard.RendererColorTable.ToolStripItemTextPressed;
                                }
                                else
                                {
                                    button.ForeColor = Theme.Standard.RendererColorTable.ToolStripItemText;
                                }
                            }
                            else
                            {
                                e.Item.ForeColor = Theme.Standard.RendererColorTable.ToolStripItemText;
                            }
                        }
                        else
                        {
                            if (e.Item.ForeColor != Color.DarkGray)
                            {
                                e.Item.ForeColor = Color.DarkGray;
                            }
                        }
                    }
                }

                if (e.Item is ToolStripButton | e.Item is ToolStripMenuItem | e.Item is ToolStripDropDownButton | e.Item is ToolStripLabel)
                {
                    switch (e.Item.DisplayStyle)
                    {
                        case ToolStripItemDisplayStyle.Image:
                            if (!e.Item.AutoSize)
                            {
                                if (e.Item.GetCurrentParent().TextDirection == ToolStripTextDirection.Vertical90)
                                {
                                    e.Item.Size = new Size(e.Item.Image.Height + 2, e.Item.Image.Width + 2);
                                }
                                else if (e.Item.GetCurrentParent().TextDirection == ToolStripTextDirection.Horizontal)
                                {
                                    e.Item.Size = new Size(e.Item.Image.Width + 2, e.Item.Image.Height + 2);
                                }
                            }
                            break;
                        case ToolStripItemDisplayStyle.ImageAndText:
                            if (!e.Item.AutoSize)
                            {
                                if (e.Item.GetCurrentParent().TextDirection == ToolStripTextDirection.Vertical90)
                                {
                                    e.Item.Size = new Size(Convert.ToInt32(graphics.MeasureString(e.Item.Text, font).Height + e.Item.Image.Height + 10), Convert.ToInt32(graphics.MeasureString(e.Item.Text, font).Width + 8));
                                }
                                else if (e.Item.GetCurrentParent().TextDirection == ToolStripTextDirection.Horizontal)
                                {
                                    e.Item.Size = new Size(Convert.ToInt32(graphics.MeasureString(e.Item.Text, font).Width + e.Item.Image.Width + 10), Convert.ToInt32(graphics.MeasureString(e.Item.Text, font).Height + 8));
                                }
                            }

                            e.Item.ImageAlign = ContentAlignment.MiddleLeft;
                            e.Item.TextAlign = ContentAlignment.MiddleRight;
                            break;
                        case ToolStripItemDisplayStyle.None:
                            break;
                        case ToolStripItemDisplayStyle.Text:
                            if (!e.Item.AutoSize)
                            {
                                if (e.Item.GetCurrentParent().TextDirection == ToolStripTextDirection.Vertical90)
                                {
                                    e.Item.Size = new Size(Convert.ToInt32(graphics.MeasureString(e.Item.Text, font).Height + 8), Convert.ToInt32(graphics.MeasureString(e.Item.Text, font).Width + 8));
                                }
                                else if (e.Item.GetCurrentParent().TextDirection == ToolStripTextDirection.Horizontal)
                                {
                                    e.Item.Size = new Size(Convert.ToInt32(graphics.MeasureString(e.Item.Text, font).Width + 8), Convert.ToInt32(graphics.MeasureString(e.Item.Text, font).Height + 8));
                                }
                            }
                            break;
                    }

                }
                else if (e.Item is ToolStripSeparator)
                {

                }
                else if (e.Item is ToolStripTextBox)
                {
                }
            }
            catch
            {
            }
        }

        //Public Sub SetUpThemeColors(ByRef f As Form, ByVal blnRenderOnly As Boolean)
        //    If Not blnRenderOnly Then
        //        Select Case System.Windows.Forms.ToolStripColors.Theme
        //            Case System.Windows.Forms.ToolStripColors.Themes.Black
        //                System.Windows.Forms.ToolStripColors.Theme(False) = Themes.Black

        //                'ForeColor
        //                clrBtn_FColorPressed_Selected = Color.FromArgb(0, 0, 0)
        //                clrBtn_FColor = Color.FromArgb(255, 255, 255)
        //                clrLBL_FColor = Color.FromArgb(0, 0, 0)

        //                'Window Gradiants
        //                clrWinGrad_Light = Color.FromArgb(255, 244, 244, 244)
        //                clrWinGrad_Dark = Color.FromArgb(255, 191, 191, 191)

        //                'Button Gradiants
        //                clrToolstripBtnGrad_Light = Color.FromArgb(255, 122, 122, 122)
        //                clrToolstripBtnGrad_Dark = Color.FromArgb(255, 71, 71, 71)

        //                'Button Border
        //                clrToolstripBtnBorder = Color.FromArgb(255, 46, 46, 46)

        //                'Selected Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Selected = Color.FromArgb(255, 255, 245, 192)
        //                clrToolstripBtnGrad_Gold_Selected = Color.FromArgb(255, 255, 216, 80)
        //                clrToolstripBtnBorder_Gold_Selected = Color.FromArgb(255, 194, 169, 120)

        //                'Pressed Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Pressed = Color.FromArgb(255, 252, 194, 131)
        //                clrToolstripBtnGrad_Gold_Pressed = Color.FromArgb(255, 249, 144, 46)
        //                clrToolstripBtnBorder_Gold_Pressed = Color.FromArgb(255, 142, 129, 101)

        //            Case System.Windows.Forms.ToolStripColors.Themes.Blue
        //                System.Windows.Forms.ToolStripColors.Theme(False) = Themes.Blue

        //                'ForeColor
        //                clrBtn_FColorPressed_Selected = Color.FromArgb(0, 25, 56)
        //                clrBtn_FColor = Color.FromArgb(255, 255, 255)
        //                clrLBL_FColor = Color.FromArgb(0, 0, 0)

        //                'Window Gradiants
        //                clrWinGrad_Light = Color.FromArgb(255, 219, 235, 246)
        //                clrWinGrad_Dark = Color.FromArgb(255, 180, 202, 229)

        //                'Button Gradiants
        //                clrToolstripBtnGrad_Light = Color.FromArgb(255, 31, 95, 183)
        //                clrToolstripBtnGrad_Dark = Color.FromArgb(255, 13, 41, 79)

        //                'Button Border
        //                clrToolstripBtnBorder = Color.FromArgb(255, 133, 158, 191)

        //                'Selected Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Selected = Color.FromArgb(255, 255, 245, 192)
        //                clrToolstripBtnGrad_Gold_Selected = Color.FromArgb(255, 255, 216, 80)
        //                clrToolstripBtnBorder_Gold_Selected = Color.FromArgb(255, 194, 169, 120)

        //                'Pressed Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Pressed = Color.FromArgb(255, 252, 194, 131)
        //                clrToolstripBtnGrad_Gold_Pressed = Color.FromArgb(255, 249, 144, 46)
        //                clrToolstripBtnBorder_Gold_Pressed = Color.FromArgb(255, 142, 129, 101)

        //            Case System.Windows.Forms.ToolStripColors.Themes.Green
        //                System.Windows.Forms.ToolStripColors.Theme(False) = Themes.Green

        //                'ForeColor
        //                clrBtn_FColorPressed_Selected = Color.FromArgb(0, 25, 56)
        //                clrBtn_FColor = Color.FromArgb(255, 255, 255)
        //                clrLBL_FColor = Color.FromArgb(0, 0, 0)

        //                'Window Gradiants
        //                clrWinGrad_Light = Color.FromArgb(255, 220, 246, 219)
        //                clrWinGrad_Dark = Color.FromArgb(255, 180, 229, 182)

        //                'Button Gradiants
        //                clrToolstripBtnGrad_Light = Color.FromArgb(255, 36, 206, 44)
        //                clrToolstripBtnGrad_Dark = Color.FromArgb(255, 15, 79, 13)

        //                'Button Border
        //                clrToolstripBtnBorder = Color.FromArgb(255, 57, 222, 65)

        //                'Selected Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Selected = Color.FromArgb(255, 255, 245, 192)
        //                clrToolstripBtnGrad_Gold_Selected = Color.FromArgb(255, 255, 216, 80)
        //                clrToolstripBtnBorder_Gold_Selected = Color.FromArgb(255, 194, 169, 120)

        //                'Pressed Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Pressed = Color.FromArgb(255, 252, 194, 131)
        //                clrToolstripBtnGrad_Gold_Pressed = Color.FromArgb(255, 249, 144, 46)
        //                clrToolstripBtnBorder_Gold_Pressed = Color.FromArgb(255, 142, 129, 101)

        //            Case System.Windows.Forms.ToolStripColors.Themes.Purple
        //                System.Windows.Forms.ToolStripColors.Theme(False) = Themes.Purple

        //                'ForeColor
        //                clrBtn_FColorPressed_Selected = Color.FromArgb(255, 63, 6, 120)
        //                clrBtn_FColor = Color.FromArgb(255, 255, 255)
        //                clrLBL_FColor = Color.FromArgb(0, 0, 0)

        //                'Window Gradiants
        //                clrWinGrad_Light = Color.FromArgb(255, 194, 158, 227)
        //                clrWinGrad_Dark = Color.FromArgb(255, 144, 94, 188)

        //                'Button Gradiants
        //                clrToolstripBtnGrad_Light = Color.FromArgb(255, 163, 133, 190)
        //                clrToolstripBtnGrad_Dark = Color.FromArgb(255, 13, 41, 79)

        //                'Button Border
        //                clrToolstripBtnBorder = Color.FromArgb(255, 133, 158, 191)

        //                'Selected Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Selected = Color.FromArgb(255, 255, 245, 192)
        //                clrToolstripBtnGrad_Gold_Selected = Color.FromArgb(255, 255, 216, 80)
        //                clrToolstripBtnBorder_Gold_Selected = Color.FromArgb(255, 194, 169, 120)

        //                'Pressed Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Pressed = Color.FromArgb(255, 252, 194, 131)
        //                clrToolstripBtnGrad_Gold_Pressed = Color.FromArgb(255, 249, 144, 46)
        //                clrToolstripBtnBorder_Gold_Pressed = Color.FromArgb(255, 142, 129, 101)

        //            Case System.Windows.Forms.ToolStripColors.Themes.JellyBelly
        //                System.Windows.Forms.ToolStripColors.Theme(False) = Themes.JellyBelly

        //                'ForeColor
        //                clrBtn_FColorPressed_Selected = Color.FromArgb(255, 235, 235, 235)
        //                clrBtn_FColor = Color.FromArgb(255, 235, 235, 235)
        //                clrLBL_FColor = Color.FromArgb(255, 235, 235, 235)

        //                'Window Gradiants
        //                clrWinGrad_Light = Color.FromArgb(255, 72, 72, 72)
        //                clrWinGrad_Dark = Color.FromArgb(255, 40, 40, 40)

        //                'Button Gradiants
        //                clrToolstripBtnGrad_Light = Color.FromArgb(255, 72, 72, 72)
        //                clrToolstripBtnGrad_Dark = Color.FromArgb(255, 40, 40, 40)

        //                'Button Border
        //                clrToolstripBtnBorder = Color.FromArgb(255, 133, 158, 191)

        //                'Selected Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Selected = Color.FromArgb(255, 48, 180, 228)
        //                clrToolstripBtnGrad_Gold_Selected = Color.FromArgb(255, 48, 180, 228)
        //                clrToolstripBtnBorder_Gold_Selected = Color.FromArgb(255, 58, 141, 181)

        //                'Pressed Button Gradiants
        //                clrToolstripBtnGrad_WhiteGold_Pressed = Color.FromArgb(255, 11, 73, 86)
        //                clrToolstripBtnGrad_Gold_Pressed = Color.FromArgb(255, 11, 73, 86)
        //                clrToolstripBtnBorder_Gold_Pressed = Color.FromArgb(255, 58, 141, 181)
        //        End Select
        //    End If

        //    'Ribbon
        //    Select Case System.Windows.Forms.ToolStripColors.Theme(False)
        //        Case System.Windows.Forms.ToolStripColors.Themes.Black
        //            TryCast(oFormMagic.MainForm.rbnAmTrustManager.Renderer, RibbonProfessionalRenderer).ColorTable = New RibbonProfesionalRendererColorTableBlack

        //        Case System.Windows.Forms.ToolStripColors.Themes.Blue
        //            TryCast(oFormMagic.MainForm.rbnAmTrustManager.Renderer, RibbonProfessionalRenderer).ColorTable = New RibbonProfesionalRendererColorTable

        //        Case System.Windows.Forms.ToolStripColors.Themes.Green
        //            TryCast(oFormMagic.MainForm.rbnAmTrustManager.Renderer, RibbonProfessionalRenderer).ColorTable = New RibbonProfesionalRendererColorTableGreen

        //        Case System.Windows.Forms.ToolStripColors.Themes.Purple
        //            TryCast(oFormMagic.MainForm.rbnAmTrustManager.Renderer, RibbonProfessionalRenderer).ColorTable = New RibbonProfesionalRendererColorTablePurple

        //        Case System.Windows.Forms.ToolStripColors.Themes.JellyBelly
        //            TryCast(oFormMagic.MainForm.rbnAmTrustManager.Renderer, RibbonProfessionalRenderer).ColorTable = New RibbonProfesionalRendererColorTableJellyBelly
        //    End Select

        //    'ToolStrips
        //    RenderToolStrips(f.Controls)
        //End Sub

        //Public Sub RenderToolStrips(ByRef ctlc As Control.ControlCollection)
        //    For Each c As Control In ctlc
        //        If TypeOf c Is ToolStrip Then
        //            Dim tls As ToolStrip = CType(c, ToolStrip)
        //            tls.Renderer = New ToolstripRenderer(False)
        //        End If
        //        RenderToolStrips(c.Controls)
        //    Next
        //End Sub
    }
}