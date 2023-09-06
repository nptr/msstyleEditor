using libmsstyle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace msstyleEditor.PropView
{    /// <summary>
     /// This class is needed to avoid the depencency of System.Windows.Forms in libmsstyle at the animation flags enum
     /// </summary>
    public class AnimationWrapper
    {
        private Animation m_animation;
        [Editor(typeof(EnumFlagUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("The animation flags to use")]
        public AnimationFlags AnimationFlags
        {
            get
            {
                return m_animation.AnimationFlags;
            }
            set
            {
                m_animation.AnimationFlags = value;
            }
        }

        public int StaggerDelay
        {
            get
            {
                return m_animation.StaggerDelay;
            }
            set
            {
                m_animation.StaggerDelay = value;
            }
        }

        public int StaggerDelayCap
        {
            get
            {
                return m_animation.StaggerDelayCap;
            }
            set
            {
                m_animation.StaggerDelayCap = value;
            }
        }

        public float StaggerDelayFactor
        {
            get
            {
                return m_animation.StaggerDelayFactor;
            }
            set
            {
                m_animation.StaggerDelayFactor = value;
            }
        }

        public int ZOrder
        {
            get
            {
                return m_animation.ZOrder;
            }
            set
            {
                m_animation.ZOrder = value;
            }
        }
        [Description("The background part ID. Used if AnimationFlags.HasBackground is set")]
        public int BackgroundPartID
        {
            get
            {
                return m_animation.BackgroundPartID;
            }
            set
            {
                m_animation.BackgroundPartID = value;
            }
        }

        public int TuningLevel
        {
            get
            {
                return m_animation.TuningLevel;
            }
            set
            {
                m_animation.TuningLevel = value;
            }
        }

        public float Perspective
        {
            get
            {
                return m_animation.Perspective;
            }
            set
            {
                m_animation.Perspective = value;
            }
        }

        [Description("The list of transforms. Can be zero.")]
        public List<Transform> Transforms { get { return m_animation.Transforms; } }

        public AnimationWrapper(Animation wrapper)
        {
            this.m_animation = wrapper;
        }
    }
}
