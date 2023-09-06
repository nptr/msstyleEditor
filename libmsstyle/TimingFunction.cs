using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libmsstyle
{
    //[Serializable]
    public class TimingFunction
    {
        public TimingFunctionType Type { get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CubicBezierTimingFunction CubicBezier { get; set; }

        public PropertyHeader Header;
        public static Dictionary<int, string> TimingFunctionNameMap = new Dictionary<int, string>()
        {
            {1 , "Linear"},
            {2 , "EaseIn"},
            {3 , "EaseOut"},
{4 , "FastIn"},
{5 , "Exponential"},
{6 , "FastInFortySevenPercent"},
{49 , "ExponetialReversed"},
{50 , "DmBoundary"},
{51 , "DmSnAppoint"},
{52 , "LogInPathX"},
{53 , "LogInPathY"},
{54 , "LogInPathZ"},
{58 , "CustomFlipping"},
{59 , "AppLaunchScale"},
{60 , "AppLaunchDrift"},
{61 , "AppLaunchFastIn"},
{62 , "FastInTenPercent"},
{63 , "AppLaunchRotateBounce"},
{64 , "AppLaunchRotateBounceDelayed"},
{65 , "DesktopWithPop"},
        };
        public TimingFunction(byte[] data, int start, PropertyHeader header)
        {
            this.Header = header;
            Type = (TimingFunctionType)BitConverter.ToInt32(data, start + 0);
            start += 4;
            if (Type == TimingFunctionType.CubicBezier)
            {
                //Cubic Bezier function
                CubicBezier = new CubicBezierTimingFunction(data, start);
            }
            else
            {
                throw new Exception("Unknown timing function type: " + Type);
            }
        }
        public void Write(BinaryWriter bw)
        {
            bw.Write(Header.Serialize());
            WriteData(bw);
        }
        public void WriteData(BinaryWriter w)
        {
            w.Write((int)Type);
            if(Type == TimingFunctionType.CubicBezier)
            {
                CubicBezier.Write(w);
            }
            w.Write(0); //Write padding
        }
        public override string ToString()
        {
            return "Timing function";
        }
    }
    public enum TimingFunctionType : uint
    {
        Undefined = 0,
        CubicBezier = 1,
    }
}
