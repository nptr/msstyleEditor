using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msstyleEditor
{
    class ImageFormats
    {
        // Workaround for .NET bug. The "Image" class reports a wrong bit
        // depth when a palette is used with alpha channel "tRNS" chunk.
        public static bool IsPngBitDepthSupported(string filePath)
        {
            //const int COLOR_TYPE_GRAYSCALE = 0;
            //const int COLOR_TYPE_RGB = 2;
            const int COLOR_TYPE_PALETTE = 3;
            //const int COLOR_TYPE_GRAYSCALE_ALPHA = 4;
            //const int COLOR_TYPE_RGB_ALPHA = 6;

            const int COLOR_INFO_OFFFSET = 24;

            try
            {
                using (var fs = File.OpenRead(filePath))
                {
                    fs.Position = COLOR_INFO_OFFFSET;

                    byte[] buffer = new byte[2];
                    fs.Read(buffer, 0, 2);

                    int colorDepth = buffer[0];
                    int colorType = buffer[1];

                    return colorType != COLOR_TYPE_PALETTE; // = 1,2,4,8 bits
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Tuple<bool, string> IsImageSupported(string file)
        {
            if (!File.Exists(file))
            {
                return new Tuple<bool, string>(false, "File doesn't exist or is not accessible.");
            }

            bool ok = IsPngBitDepthSupported(file);
            if(!ok)
            {
                return new Tuple<bool, string>(false, "Visual styles don't support PNGs with a color palette.");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}
