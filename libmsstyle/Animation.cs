using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libmsstyle
{
    public class Animation
    {
        public int SizeInBytes;
        public int PropertiesIndex;
        public int TransformsIndex;
       
        public AnimationFlags AnimationFlags { get; set; }
        public int TransformCount { get; set; }
        public int StaggerDelay { get; set; }
        public int StaggerDelayCap { get; set; }
        public float StaggerDelayFactor { get; set; }
        public int ZOrder { get; set; }
        public int BackgroundPartID { get; set; }
        public int TuningLevel { get; set; }
        public float Perspective { get; set; }
        private List<Transform> _transforms = new List<Transform>();
        public List<Transform> Transforms { get { return _transforms; } }
        public PropertyHeader Header;
        public static Dictionary<int, AnimationStates> AnimationNameMap = new Dictionary<int, AnimationStates>()
        {
             { 1, new AnimationStates("Expand", new Dictionary<int, string>
        {
            { 1, "Affected" },
            { 2, "Revealed" }
        })
    },
    { 2, new AnimationStates("Collapse", new Dictionary<int, string>
        {
            { 1, "Hidden" },
            { 2, "Affected" }
        })
    },
    { 3, new AnimationStates("Reposition",new Dictionary<int, string>()) },
    { 4, new AnimationStates("FadeIn", new Dictionary<int, string>
        {
            { 1, "Shown" }
        })
    },
    { 5, new AnimationStates("FadeOut", new Dictionary<int, string>
        {
            { 1, "Hidden" }
        })
    },
    { 6, new AnimationStates("AddToList", new Dictionary<int, string>
        {
            { 1, "Added" },
            { 2, "Affected" }
        })
    },
    { 7, new AnimationStates("DeleteFromList", new Dictionary<int, string>
        {
            { 1, "Deleted" },
            { 2, "Remaining" }
        })
    },
    { 8, new AnimationStates("AddToGrid", new Dictionary<int, string>
        {
            { 1, "Added" },
            { 2, "Affected" },
            { 3, "RowOut" },
            { 4, "RowIn" }
        })
    },
    { 9, new AnimationStates("DeleteFromGrid", new Dictionary<int, string>
        {
            { 1, "Deleted" },
            { 2, "Remaining" },
            { 3, "RowOut" },
            { 4, "RowIn" }
        })
    },
    { 10, new AnimationStates("AddToSearchGrid", new Dictionary<int, string>
        {
            { 1, "Added" },
            { 2, "Affected" },
            { 3, "RowOut" },
            { 4, "RowIn" }
        })
    },
    { 11, new AnimationStates("DeleteFromSearchGrid", new Dictionary<int, string>
        {
            { 1, "Deleted" },
            { 2, "Remaining" },
            { 3, "RowOut" },
            { 4, "RowIn" }
        })
    },
    { 12, new AnimationStates("AddToSearchList", new Dictionary<int, string>
        {
            { 1, "Added" },
            { 2, "Affected" }
        })
    },
    { 13, new AnimationStates("DeleteFromSearchList", new Dictionary<int, string>
        {
            { 1, "Deleted" },
            { 2, "Remaining" }
        })
    },
    { 14, new AnimationStates("ShowEdgeUI",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 15, new AnimationStates("ShowPanel",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 16, new AnimationStates("HideEdgeUI",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 17, new AnimationStates("HidePanel",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 18, new AnimationStates("ShowPopup",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 19, new AnimationStates("HidePopup",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 20, new AnimationStates("PointerDown",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 21, new AnimationStates("PointerUp",new Dictionary<int, string>() { { 1, "Target" }, }) },
    { 22, new AnimationStates("DragSourceStart", new Dictionary<int, string>
        {
            { 1, "DragSource" },
            { 2, "Affected" }
        })
    },
    { 23, new AnimationStates("DragSourceEnd", new Dictionary<int, string>
        {
            { 1, "DragSource" },
            { 2, "Affected" }
        })
    },
    { 24, new AnimationStates("TransitionContent", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" }
        })
    },
    { 25, new AnimationStates("Reveal", new Dictionary<int, string>
        {
            { 1, "Background" },
            { 2, "Content" },
            { 3, "Outline" },
            { 4, "Tapped" }
        })
    },
    { 26, new AnimationStates("Hide", new Dictionary<int, string>
        {
            { 1, "Background" },
            { 2, "Content" },
            { 3, "Outline" },
            { 4, "Tapped" }
        })
    },
    { 27, new AnimationStates("DragBetweenEnter", new Dictionary<int, string>
        {
            { 1, "Affected" }
        })
    },
    { 28, new AnimationStates("DragBetweenLeave", new Dictionary<int, string>
        {
            { 1, "Affected" }
        })
    },
    { 29, new AnimationStates("SwipeSelect", new Dictionary<int, string>
        {
            { 1, "Selected" },
            { 2, "Selection" }
        })
    },
    { 30, new AnimationStates("SwipeDeselect", new Dictionary<int, string>
        {
            { 1, "Deselected" },
            { 2, "Selection" }
        })
    },
    { 31, new AnimationStates("SwipeReveal",new Dictionary<int, string>()) },
    { 32, new AnimationStates("EnterPage",new Dictionary<int, string>()) },
    { 33, new AnimationStates("TransitionPage", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" }
        })
    },
    { 34, new AnimationStates("CrossFade", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" }
        })
    },
    { 35, new AnimationStates("Peek",new Dictionary<int, string>()) },
    { 36, new AnimationStates("UpdateBadge", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" }
        })
    },
    { 49, new AnimationStates("WindowOpen",new Dictionary<int, string>()
    {
        { 1, "Target" },
    }) },
    { 50, new AnimationStates("WindowClose",new Dictionary<int, string>() {
        { 1, "Target" },
    }) },
    { 51, new AnimationStates("WindowMinimize",new Dictionary<int, string>() {
        { 1, "Target" },
    }) },
    { 52, new AnimationStates("WindowMaximize",new Dictionary<int, string>() { { 1, "Target" }, }) },
    { 53, new AnimationStates("WindowRestoreFromMinimized", new Dictionary<int, string>
        {
            { 1, "Target" },
            { 2, "UnknownState(2)" },
            { 3, "UnknownState(3)" },
            { 4, "UnknownState(4)" },
            { 5, "UnknownState(5)" },
            { 6, "UnknownState(6)" },
            { 7, "UnknownState(7)" },
            { 8, "UnknownState(8)" },
            { 9, "UnknownState(9)" },
            { 10, "UnknownState(10)" }
        })
    },
    { 54, new AnimationStates("WindowRestoreFromMaximized", new Dictionary<int, string>
        {
            { 1, "Target" },
            { 2, "UnknownState(2)" },
            { 3, "UnknownState(3)" },
            { 4, "UnknownState(4)" },
            { 5, "UnknownState(5)" },
            { 6, "UnknownState(6)" }
        })
    },
    { 55, new AnimationStates("Login", new Dictionary<int, string>
        {
            { 1, "LoginImage" },
            { 2, "LoginText" },
            { 3, "LauncherTiles" },
            { 4, "LauncherTileColumns" },
            { 5, "LauncherIcon" },
            { 6, "LauncherIconText" }
        })
    },
    { 56, new AnimationStates("LauncherLaunch", new Dictionary<int, string>
        {
            { 1, "Desktop" },
            { 2, "Launcher" },
            { 3, "LauncherTiles" },
            { 4, "LauncherText" },
            { 5, "LauncherIcon" },
            { 6, "LauncherSubheaders" },
            { 7, "Flyout" },
            { 8, "FullscreenFlyout" },
            { 9, "JumpList" }
        })
    },
    { 57, new AnimationStates("LauncherDismiss", new Dictionary<int, string>
        {
            { 1, "Desktop" },
            { 2, "Launcher" },
            { 3, "LauncherTiles" },
            { 4, "LauncherText" },
            { 5, "LauncherIcon" },
            { 6, "Timer" },
            { 7, "Flyout" }
        })
    },
    { 58, new AnimationStates("AppLaunch", new Dictionary<int, string>
        {
            { 1, "Activated" },
            { 2, "Remaining" }
        })
    },
    { 59, new AnimationStates("AppSwitch", new Dictionary<int, string>
        {
            { 1, "PageOut" }
        })
    },
    { 60, new AnimationStates("TapDown3D", new Dictionary<int, string>()) },
    { 61, new AnimationStates("TapUp3D", new Dictionary<int, string>
        {
            { 1, "Target" },
            { 2, "UnknownState(2)" },
            { 3, "UnknownState(3)" },
            { 4, "UnknownState(4)" },
            { 5, "UnknownState(5)" },
            { 6, "UnknownState(6)" },
            { 7, "UnknownState(7)" }
        })
    },
    { 62, new AnimationStates("Thumbnail", new Dictionary<int, string>
        {
            { 1, "Animate" },
            { 2, "AnimateBounce" }
        })
    },
    { 63, new AnimationStates("Resize", new Dictionary<int, string>
        {
            { 1, "Before" }
        })
    },
    { 64, new AnimationStates("NoAnimation", new Dictionary<int, string>
        {
            { 1, "Target" },
            { 2, "UnknownState(2)" }
        })
    },
    { 65, new AnimationStates("Unlock", new Dictionary<int, string>
        {
            { 1, "Incoming" }
        })
    },
    { 66, new AnimationStates("Tdbn", new Dictionary<int, string>
        {
            { 1, "Target" },
            { 2, "UnknownState(2)" },
            { 3, "UnknownState(3)" },
            { 4, "UnknownState(4)" },
            { 5, "UnknownState(5)" },
            { 6, "UnknownState(6)" },
            { 7, "UnknownState(7)" },
            { 8, "UnknownState(8)" },
            { 9, "UnknownState(9)" },
            { 10, "UnknownState(10)" },
            { 11, "UnknownState(11)" },
            { 12, "UnknownState(12)" },
            { 13, "UnknownState(13)" }
        })
    },
    { 67, new AnimationStates("AppArrangementImmediate", new Dictionary<int, string>
        {
            { 1, "AppOut" },
            { 2, "Thumbnail" },
            { 3, "Static" },
            { 4, "Slide" },
            { 5, "SlideToPlace" },
            { 6, "AppClosed" },
            { 7, "AppOutMaximize" },
            { 8, "ReplaceFromTilt" },
            { 9, "ReplaceNoTilt" }
        })
    },
    { 69, new AnimationStates("AppLaunchSwitch", new Dictionary<int, string>
        {
            { 1, "UnknownState(1)" },
            { 2, "UnknownState(2)" }
        })
    },
    { 70, new AnimationStates("ChangePanel", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" }
        })
    },
    { 71, new AnimationStates("GrowPanel", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" }
        })
    },
    { 72, new AnimationStates("ShrinkPanel", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" },
            { 3, "UnknownState(3)" },
            { 4, "UnknownState(4)" }
        })
    },
    { 73, new AnimationStates("TileNotification", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" },
            { 3, "IncomingBranding" },
            { 4, "OutgoingBranding" },
            { 5, "UnknownState(5)" },
            { 6, "UnknownState(6)" }
        })
    },
    { 74, new AnimationStates("ImmersiveBackground", new Dictionary<int, string>
        {
            { 1, "FadeInBackground" },
            { 2, "FadeInAccent" },
            { 3, "FadeOutAccent" },
            { 4, "FadeInAccentLauncherLaunch" },
            { 5, "FadeInTranslateAccent" },
            { 6, "DelayedHideAccent" }
        })
    },
    { 75, new AnimationStates("SSCrossfade", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" }
        })
    },
    { 76, new AnimationStates("ScreenRotation", new Dictionary<int, string>
        {
            { 1, "Before" },
            { 2, "BeforeOutgoing" },
            { 3, "After" }
        })
    },
    { 77, new AnimationStates("InplaceResize", new Dictionary<int, string>
        {
            { 1, "AnchorTop" },
            { 2, "AnchorBottom" },
            { 3, "AnchorRight" },
            { 4, "AnchorLeft" }
        })
    },
    { 78, new AnimationStates("DialogSwitch", new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 79, new AnimationStates("LogonEnterPage", new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 80, new AnimationStates("CrossfadeInPlace", new Dictionary<int, string>
        {
            { 1, "Incoming" },
            { 2, "Outgoing" },
            { 3, "UnknownState(3)" },
            { 4, "UnknownState(4)" },
            { 5, "UnknownState(5)" },
            { 6, "UnknownState(6)" },
            { 7, "UnknownState(7)" }
        })
    },
    { 81, new AnimationStates("ScreenshotFadeOut", new Dictionary<int, string>
        {
            { 1, "Target" },
            { 2, "UnknownState(2)" }
        })
    },
    { 82, new AnimationStates("ShowCharm",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 83, new AnimationStates("HideCharm",new Dictionary<int, string>(){ { 1, "Target" }, }) },
    { 84, new AnimationStates("FadeInLong", new Dictionary<int, string>
        {
            { 1, "Shown" }
        })
    },
    { 85, new AnimationStates("FadeOutLong", new Dictionary<int, string>
        {
            { 1, "Hidden" }
        })
    },
    { 86, new AnimationStates("AddToGridStaggered", new Dictionary<int, string>
        {
            { 1, "Added" }
        })
    },
    { 87, new AnimationStates("DeleteFromGridStaggered", new Dictionary<int, string>
        {
            { 1, "Deleted" }
        })
    },
    { 88, new AnimationStates("DragSourceStartMulti", new Dictionary<int, string>
        {
            { 1, "DragSource" },
            { 2, "Affected" },
            { 3, "Content" },
            { 4, "Adornment" }
        })
    },
    { 89, new AnimationStates("DragSourceEndMulti", new Dictionary<int, string>
        {
            { 1, "DragSource" },
            { 2, "Affected" },
            { 3, "Content" },
            { 4, "Adornment" }
        })
    },
    { 90, new AnimationStates("VirtualDesktopSwitch", new Dictionary<int, string>
        {
            { 1, "Target" },
            { 2, "UnknownState(2)" },
            { 3, "UnknownState(3)" },
            { 4, "UnknownState(4)" },
            { 5, "UnknownState(5)" }
        })
    },
    { 91, new AnimationStates("TabletModeTransition", new Dictionary<int, string>
        {
            { 1, "DesktopBefore" },
            { 2, "UnknownState(2)" },
            { 3, "UnknownState(3)" },
            { 4, "UnknownState(4)" }
        })
    },
    { 94, new AnimationStates("WindowMinimizeWin11", new Dictionary<int, string>
        {
            { 1, "Target" },
        })
    },
    { 95, new AnimationStates("WindowRestoreWin11", new Dictionary<int, string>
        {
            { 1, "Target" },
        })
    }
        };
        public Animation(byte[] data, ref int start, PropertyHeader header)
        {
            this.Header = header;
            SizeInBytes = BitConverter.ToInt32(data, start + 0); //copy of sizeInBytes of the VS record
            PropertiesIndex = BitConverter.ToInt32(data, start + 4);
            TransformsIndex = BitConverter.ToInt32(data, start + 8);
            start += 16; //skip past padding

            //Read properties structure
            AnimationFlags = (AnimationFlags)BitConverter.ToInt32(data, start + 0);
            TransformCount = BitConverter.ToInt32(data, start + 4);
            StaggerDelay = BitConverter.ToInt32(data, start + 8);
            StaggerDelayCap = BitConverter.ToInt32(data, start + 12);
            StaggerDelayFactor = BitConverter.ToSingle(data, start + 16);
            ZOrder = BitConverter.ToInt32(data, start + 20);
            BackgroundPartID = BitConverter.ToInt32(data, start + 24);
            TuningLevel = BitConverter.ToInt32(data, start + 28);
            Perspective = BitConverter.ToSingle(data, start + 32);

            start += 40; //skip past padding

            for (int i = 0; i < TransformCount; i++)
            {
                var t = new Transform(data, ref start);
                Transforms.Add(t);
                start += GetPaddingForSize(t.StructureSize);
            }
        }
        /// <summary>
        /// Create a new animation
        /// </summary>
        /// <param name="animationsHeader">Header of the animations class</param>
        /// <param name="part">Animation part</param>
        /// <param name="state">Animation state</param>
        public Animation(PropertyHeader animationsHeader, int part, int state)
        {
            PropertiesIndex = 16;
            TransformsIndex = 56;
            Header = new PropertyHeader(20000, animationsHeader.typeID);
            Header.stateID = state;
            Header.partID = part;
        }

        int GetPaddingForSize(int size)
        {
            const int sizeOfRecordHeader = 32;
            return ((size + 39) & -8) - sizeOfRecordHeader - size;
        }

        public void Write(BinaryWriter bw)
        {
            //Update the total size
            int total_size = 56;
            foreach (var item in Transforms)
            {
                total_size += item.StructureSize + GetPaddingForSize(item.StructureSize);
            }

            Header.sizeInBytes = total_size;

            bw.Write(Header.Serialize());
            WriteData(bw);
        }
        public void WriteData(BinaryWriter bw)
        {
            TransformCount = Transforms.Count;

            //write animation header
            bw.Write(Header.sizeInBytes);
            bw.Write(16); //Property index (always seems to be the same)
            bw.Write(56);
            bw.Write(0);

            //write animation structure
            bw.Write((int)AnimationFlags);
            bw.Write(Transforms.Count);
            bw.Write(StaggerDelay);
            bw.Write(StaggerDelayCap);
            bw.Write(StaggerDelayFactor);
            bw.Write(ZOrder);
            bw.Write(BackgroundPartID);
            bw.Write(TuningLevel);
            bw.Write(Perspective);
            bw.Write(0); //Write padding

            foreach (var item in Transforms)
            {
                item.Write(bw);

                //write padding
                var padding = new byte[GetPaddingForSize(item.StructureSize)];
                bw.Write(padding);
            }
        }
    }
    [Flags]
    public enum AnimationFlags
    {
        None = 0,
        HasStagger = 1,
        IsRtlAware = 2,
        HasBackground = 3,
        HasPerspective = 4
    }

    public class AnimationStates
    {
        public string AnimationName;
        public Dictionary<int, string> AnimationStateDict;
        public AnimationStates(string name, Dictionary<int, string> states)
        {
            AnimationName = name;
            AnimationStateDict = states;
        }
    }
}
