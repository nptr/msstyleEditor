using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    /// Helper functions for RTL and LTR layout
    /// </summary>
    public class LayoutHelper
    {
        private readonly Ribbon _ribbon;

        public LayoutHelper(Ribbon ribbon)
        {
            _ribbon = ribbon;
        }

        /// <summary>
        /// Layout position. Near to or further from the Ribbon Orb. 
        /// </summary>
        public enum RTLLayoutPosition
        {
            /// <summary>
            /// Closer to the Ribbon Orb (Left when not in RTL mode).
            /// </summary>
            Near,
            /// <summary>
            /// Further from the Ribbon Orb (right when in RTL mode).
            /// </summary>
            Far
        }

        /// <summary>
        /// Calculate the new horizontal position for a Rectangle against a given reference Rectangle.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="rect"></param>
        /// <param name="type"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Rectangle CalcNewPosition(Rectangle reference, Rectangle rect, RTLLayoutPosition type, int distance)
        {
            if (((_ribbon.RightToLeft == RightToLeft.No) && (type == RTLLayoutPosition.Near)) || ((_ribbon.RightToLeft == RightToLeft.Yes) && (type == RTLLayoutPosition.Far)))
            {
                //Object on left
                return new Rectangle(reference.Left - distance - rect.Width, rect.Y, rect.Width, rect.Height);
            }

            //Object on right
            return new Rectangle(reference.Right + distance, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Calculate the new horizontal position for a Point against a given reference Rectangle.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="point"></param>
        /// <param name="type"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Point CalcNewPosition(Rectangle reference, Point point, RTLLayoutPosition type, int distance)
        {
            if (((_ribbon.RightToLeft == RightToLeft.No) && (type == RTLLayoutPosition.Near)) || ((_ribbon.RightToLeft == RightToLeft.Yes) && (type == RTLLayoutPosition.Far)))
            {
                //Object on left
                return new Point(reference.Left - distance, point.Y);
            }

            //Object on right
            return new Point(reference.Right + distance, point.Y);
        }

        /// <summary>
        /// Calculate the new horizontal position for a Point against a given reference Rectangle.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="rect"></param>
        /// <param name="type"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Rectangle CalcNewPosition(Point reference, Rectangle rect, RTLLayoutPosition type, int distance)
        {
            if (((_ribbon.RightToLeft == RightToLeft.No) && (type == RTLLayoutPosition.Near)) || ((_ribbon.RightToLeft == RightToLeft.Yes) && (type == RTLLayoutPosition.Far)))
            {
                //Object on left
                return new Rectangle(reference.X - distance - rect.Width, rect.Y, rect.Width, rect.Height);
            }

            //Object on right
            return new Rectangle(reference.X + distance, rect.Y, rect.Width, rect.Height);
        }

    }
}
