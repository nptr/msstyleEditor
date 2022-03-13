using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace msstyleEditor
{
	class ImageControl : Panel
	{
		public enum BackgroundStyle
		{
			Color,
			Chessboard
		}

		public BackgroundStyle Background { get; set; } = BackgroundStyle.Chessboard;

        private Size m_cellSize = new Size(16, 16);

        public float MinZoom { get; set; } = 0.5f;
		public float MaxZoom { get; set; } = 8.0f;
		
		private float m_zoomFactor = 1.0f;
		public float ZoomFactor {
			get
			{
				return m_zoomFactor;
			}
			set 
			{ 
				if (value >= MinZoom && value <= MaxZoom)
                {
					m_zoomFactor = value;
				}
			}
		}

        public Rectangle? HighlightArea { get; set; }


		public ImageControl()
        {
            DoubleBuffered = true;
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Control);

            // adjust size
            if (BackgroundImage != null)
            {
                AutoScrollMinSize = new Size(
                    Math.Max(ClientSize.Width, (int)(BackgroundImage.Width * ZoomFactor)),
                    Math.Max(ClientSize.Height, (int)(BackgroundImage.Height * ZoomFactor))
                );
            }
            else
            {
                AutoScrollMinSize = ClientSize;
            }

            // draw background
            if (Background == BackgroundStyle.Chessboard)
            {
                int numXCells = (int)(AutoScrollMinSize.Width / m_cellSize.Width + 1);
                int numYCells = (int)(AutoScrollMinSize.Height / m_cellSize.Height + 1);

                e.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);
                for (int x = 0; x < numXCells; ++x)
                {
                    for (int y = 0; y < numYCells; ++y)
                    {
                        Brush brush;
                        if ((x + y) % 2 > 0)
                            brush = Brushes.White;
                        else brush = Brushes.LightGray;

                        e.Graphics.FillRectangle(brush,
                            new Rectangle(
                                x * m_cellSize.Width,
                                y * m_cellSize.Height,
                                m_cellSize.Width,
                                m_cellSize.Height)
                        );
                    }
                }
                e.Graphics.TranslateTransform(-this.AutoScrollPosition.X, -this.AutoScrollPosition.Y);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), e.ClipRectangle);
            }

            // draw image
            if (BackgroundImage == null)
            {
                return;
            }

            int xMargin = (ClientSize.Width / 2) - (int)(BackgroundImage.Width / 2 * ZoomFactor);
            int yMargin = (ClientSize.Height / 2) - (int)(BackgroundImage.Height / 2 * ZoomFactor);
            xMargin = Math.Max(xMargin, 0);
            yMargin = Math.Max(yMargin, 0);

            Rectangle src = e.ClipRectangle;
            src.X -= AutoScrollPosition.X;
            src.Y -= AutoScrollPosition.Y;

            Rectangle dst = e.ClipRectangle;
            dst.X += (int)xMargin;
            dst.Y += (int)yMargin;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(BackgroundImage, dst, src, GraphicsUnit.Pixel);
            e.Graphics.InterpolationMode = InterpolationMode.Default;

            if(HighlightArea != null)
            {
                int hl = HighlightArea.Value.Left + xMargin;
                int ht = HighlightArea.Value.Top + yMargin;
                int hr = hl + HighlightArea.Value.Width;
                int hb = ht + HighlightArea.Value.Height;

                e.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

                using (Pen p = new Pen(Color.Violet, 2.0f))
                {
                    e.Graphics.DrawLine(p, hl, 0, hl, AutoScrollMinSize.Height);
                    e.Graphics.DrawLine(p, hr, 0, hr, AutoScrollMinSize.Height);

                    e.Graphics.DrawLine(p, 0, ht, AutoScrollMinSize.Width, ht);
                    e.Graphics.DrawLine(p, 0, hb, AutoScrollMinSize.Width, hb);
                }
            }
        }
    }
}
