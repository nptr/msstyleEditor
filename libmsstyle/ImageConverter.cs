using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace libmsstyle
{
    public class ImageConverter
    {
        public static void PremultiplyAlpha(Stream instream, out Bitmap dst)
        {
            var src = new Bitmap(instream);
            // Premultiply the alpha channel.
            var data = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            // Here, we lie. The data is actually premultiplied, not straight. If we don't lie,
            // the bitmap class will convert the data from PArgb back to Argb when saving as PNG.
            // It does so, because PNG has per specification always straight alpha.
            dst = new Bitmap(src.Width, src.Height, data.Stride, PixelFormat.Format32bppArgb, data.Scan0);
        }

        public static void PremulToStraightAlpha(Stream instream, out Bitmap dst) 
        {
            var src = new Bitmap(instream);
            // Bitmap `src` will have format Argb, because PNGs are always supposed to be Argb and there is no
            // field in a PNG image describing this. LockBits(Argb) will thus perform no conversion.
            var data = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            // This bitmap, we tell the truth about `data` having premultiplied alpha. When saving the
            // bitmap as PNG (done by the caller of this function), the conversion to straight alpha will happen.
            dst = new Bitmap(src.Width, src.Height, data.Stride, PixelFormat.Format32bppPArgb, data.Scan0);
        }
    }
}
