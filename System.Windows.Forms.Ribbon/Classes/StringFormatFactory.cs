using System.Drawing;

namespace System.Windows.Forms
{
    internal static class StringFormatFactory
    {
        /// <summary>
        /// <see cref="StringFormat"/> with options:
        /// Alignment = StringAlignment.Near;
        /// LineAlignment = StringAlignment.Center;
        /// </summary>
        /// <returns>new <see cref="StringFormat"/> instance</returns>
        public static StringFormat NearCenter()
        {
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            return sf;
        }

        /// <summary>
        /// <see cref="StringFormat"/> with options:
        /// Alignment = StringAlignment.Near;
        /// LineAlignment = StringAlignment.Center;
        /// Trimming = "trim";
        /// FormatFlags |= StringFormatFlags.NoWrap;
        /// </summary>
        /// <returns>new <see cref="StringFormat"/> instance</returns>
        public static StringFormat NearCenterNoWrap(StringTrimming trim)
        {
            StringFormat sf = NearCenter();

            sf.Trimming = trim;
            sf.FormatFlags |= StringFormatFlags.NoWrap;

            return sf;
        }

        /// <summary>
        /// <see cref="StringFormat"/> with options:
        /// Alignment = StringAlignment.Center;
        /// LineAlignment = StringAlignment.Near;
        /// sf.Trimming = StringTrimming.Character;
        /// </summary>
        /// <returns>new <see cref="StringFormat"/> instance</returns>
        public static StringFormat CenterNearTrimChar()
        {
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Near,
                Trimming = StringTrimming.Character
            };

            return sf;
        }

        /// <summary>
        /// <see cref="StringFormat"/> with options:
        /// Alignment = StringAlignment.Center;
        /// LineAlignment = StringAlignment.Center;
        /// </summary>
        /// <returns>new <see cref="StringFormat"/> instance</returns>
        public static StringFormat Center()
        {
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            return sf;
        }

        /// <summary>
        /// <see cref="StringFormat"/> with options:
        /// Alignment = StringAlignment.Center;
        /// LineAlignment = StringAlignment.Center;
        /// Trimming = "trim";
        /// </summary>
        /// <returns>new <see cref="StringFormat"/> instance</returns>
        public static StringFormat Center(StringTrimming trim)
        {
            StringFormat sf = Center();

            sf.Trimming = trim;

            return sf;
        }

        /// <summary>
        /// <see cref="StringFormat"/> with options:
        /// Alignment = StringAlignment.Center;
        /// LineAlignment = StringAlignment.Center;
        /// Trimming = "trim";
        /// FormatFlags |= StringFormatFlags.NoWrap;
        /// </summary>
        /// <returns>new <see cref="StringFormat"/> instance</returns>
        public static StringFormat CenterNoWrap(StringTrimming trim)
        {
            StringFormat sf = Center(trim);

            sf.FormatFlags |= StringFormatFlags.NoWrap;

            return sf;
        }

        /// <summary>
        /// <see cref="StringFormat"/> with options:
        /// Alignment = StringAlignment.Center;
        /// LineAlignment = StringAlignment.Center;
        /// Trimming = StringTrimming.EllipsisCharacter;
        /// FormatFlags |= StringFormatFlags.NoWrap;
        /// </summary>
        /// <returns>new <see cref="StringFormat"/> instance</returns>
        public static StringFormat CenterNoWrapTrimEllipsis()
        {
            return CenterNoWrap(StringTrimming.EllipsisCharacter);
        }
    }
}
