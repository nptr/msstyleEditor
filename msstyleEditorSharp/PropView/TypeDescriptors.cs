using libmsstyle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;

namespace msstyleEditor.PropView
{
    public class TypeDescriptor : ICustomTypeDescriptor
    {
        StylePart m_part;
        VisualStyle m_style;

        public TypeDescriptor(StylePart part, VisualStyle style)
        {
            m_part = part;
            m_style = style;
        }

        public void Add(int stateId, StyleProperty value)
        {
            m_part.States[stateId].Properties
                .Add(value);
        }

        public void Remove(int stateId, int nameId)
        {
            var state = m_part.States[stateId].Properties
                .RemoveAll((p) => nameId == p.Header.nameID);
        }

        #region "TypeDescriptor Implementation"

        public String GetClassName()
        {
            return System.ComponentModel.TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return System.ComponentModel.TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return System.ComponentModel.TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return System.ComponentModel.TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// GetEditor
        /// </summary>
        /// <param name="editorBaseType">editorBaseType</param>
        /// <returns>object</returns>
        public object GetEditor(Type editorBaseType)
        {
            return System.ComponentModel.TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, true);
        }

        internal static string MakeCategoryNameWithSortingHack(StyleState state)
        {
            // We append a lot of \t for the category that shall be shown on top.
            // Subsequent categories have each one \t less then the one above.
            int numNonPrintable = 30 - state.StateId; // 30 ought to be enough
            StringBuilder sb = new StringBuilder(30);
            for (int i = 0; i < numNonPrintable; ++i)
            {
                sb.Append('\t');
            }
            sb.AppendFormat("{0} - {1}", state.StateId, state.StateName);
            return sb.ToString();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyDescriptor> propDesc = new List<PropertyDescriptor>();
            foreach (var state in m_part.States)
            {
                // dummy prop for empty states
                if (state.Value.Properties.Count == 0)
                {
                    propDesc.Add(new PlaceHolderPropertyDescriptor(state.Value, attributes));
                }

                // real prop
                foreach (var prop in state.Value.Properties)
                {
                    PropertyInfo info;
                    if (!VisualStyleProperties.PROPERTY_INFO_MAP.TryGetValue(prop.Header.nameID, out info))
                    {
                        info = new PropertyInfo("UNKNOWN", prop.Header.nameID, "");
                    }

                    propDesc.Add(new StylePropertyDescriptor(info, prop, state.Value, m_style, attributes));
                }
            }

            return new PropertyDescriptorCollection(propDesc.ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return System.ComponentModel.TypeDescriptor.GetProperties(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        #endregion
    }
    public class AnimationTypeDescriptor : ICustomTypeDescriptor
    {
        public List<Animation> Animations = new List<Animation>();
        public AnimationTypeDescriptor(Animation animation)
        {
            Animations.Add(animation);
        }

        public void AddState(Animation animation)
        {
            Animations.Add(animation);
        }
        public String GetClassName()
        {
            return System.ComponentModel.TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return System.ComponentModel.TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return System.ComponentModel.TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return System.ComponentModel.TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// GetEditor
        /// </summary>
        /// <param name="editorBaseType">editorBaseType</param>
        /// <returns>object</returns>
        public object GetEditor(Type editorBaseType)
        {
            return System.ComponentModel.TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, true);
        }

        internal static string MakeCategoryNameWithSortingHack(StyleState state)
        {
            // We append a lot of \t for the category that shall be shown on top.
            // Subsequent categories have each one \t less then the one above.
            int numNonPrintable = 30 - state.StateId; // 30 ought to be enough
            StringBuilder sb = new StringBuilder(30);
            for (int i = 0; i < numNonPrintable; ++i)
            {
                sb.Append('\t');
            }
            sb.AppendFormat("{0} - {1}", state.StateId, state.StateName);
            return sb.ToString();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyDescriptor> propDesc = new List<PropertyDescriptor>();
            foreach (var state in Animations)
            {
                foreach (var item in state.GetType().GetProperties())
                {
                    propDesc.Add(new AnimationPropertyDescriptior(item, state, attributes));
                }
            }

            return new PropertyDescriptorCollection(propDesc.ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return System.ComponentModel.TypeDescriptor.GetProperties(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }
    public class StylePropertyDescriptor : PropertyDescriptor
    {
        PropertyInfo m_info;
        StyleProperty m_property;
        StyleState m_state;
        VisualStyle m_style;
        string m_category;

        public StylePropertyDescriptor(PropertyInfo info, StyleProperty property, StyleState state, VisualStyle style, Attribute[] attrs)
            : base(info.Name, attrs)
        {
            m_info = info;
            m_property = property;
            m_state = state;
            m_style = style;
            m_category = TypeDescriptor.MakeCategoryNameWithSortingHack(state);
        }

        public override Type ComponentType => null;
        public override string Description
        {
            get
            {
                return $"{m_info.Description} ID {m_property.Header.nameID}.".TrimStart();
            }
        }
        public override string Category => m_category;
        public override string DisplayName => m_info.Name;
        public override bool IsReadOnly => false;
        public override Type PropertyType => m_property.GetValue().GetType();

        public override TypeConverter Converter
        {
            get
            {
                if (m_property.Header.typeID == (int)IDENTIFIER.ENUM)
                {
                    var enumInfo = VisualStyleEnums.Find(m_property.Header.nameID);
                    if (enumInfo == null ||
                        enumInfo.Count <= (int)m_property.GetValue())
                    {
                        // invalid enums, not just unknown values, do occur unfortunately
                        return base.Converter;
                    }

                    return new StyleEnumConverter(enumInfo);
                }
                else if (m_property.Header.typeID == (int)IDENTIFIER.FONT)
                {
                    var fonts = StringTable.FilterFonts(m_style.PreferredStringTable);
                    if (fonts.Count == 0)
                    {
                        return base.Converter;
                    }

                    return new StyleStringResourceConverter(fonts);
                }
                else
                {
                    return base.Converter;
                }
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override object GetValue(object component)
        {
            return m_property.GetValue();
        }

        public override void SetValue(object component, object value)
        {
            m_property.SetValue((dynamic)value);
        }



        public PropertyInfo Info
        {
            get { return m_info; }
        }

        public StyleProperty StyleProperty
        {
            get { return m_property; }
        }

        public StyleState StyleState
        {
            get { return m_state; }
        }
    }

    public class PlaceHolderPropertyDescriptor : PropertyDescriptor
    {
        StyleState m_State;
        string m_Category;

        public PlaceHolderPropertyDescriptor(StyleState state, Attribute[] attrs)
            : base("No properties", attrs)
        {
            m_State = state;
            m_Category = TypeDescriptor.MakeCategoryNameWithSortingHack(state);
        }

        public override Type ComponentType => null;
        public override string Description => "";
        public override string Category => m_Category;
        public override string DisplayName => "No properties";
        public override bool IsReadOnly => true;
        public override Type PropertyType => typeof(string);

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override object GetValue(object component)
        {
            return "";
        }

        public override void SetValue(object component, object value)
        {
        }


        public StyleState StyleState
        {
            get { return m_State; }
        }
    }

    public class AnimationPropertyDescriptior : PropertyDescriptor
    {
        public override Type ComponentType => null;

        public override bool IsReadOnly => !m_fi.CanWrite;

        public override Type PropertyType => m_fi.PropertyType;
        private System.Reflection.PropertyInfo m_fi;
        public Animation m_animation;
        private string m_category;
        public override string Category => m_category;

        public AnimationPropertyDescriptior(System.Reflection.PropertyInfo fi, Animation animation, Attribute[] attrs)
          : base(fi.Name, attrs)
        {
            this.m_fi = fi;
            this.m_animation = animation;

            AnimationStates map = VisualStyleAnimations.FindAnimStates(animation.Header.partID);
            if (map != null)
            {
                string state;
                if (map.AnimationStateDict.TryGetValue(animation.Header.stateID, out state))
                {
                    this.m_category = $"{animation.Header.stateID} - {state}";
                }
                else
                {
                    this.m_category = "State " + animation.Header.stateID;
                }
            }
            else
            {
                this.m_category = "State " + animation.Header.stateID;
            }
            List<Attribute> newAtribs = new List<Attribute>();
            if (fi.Name == "AnimationFlags")
            {
                newAtribs.Add(new EditorAttribute(typeof(EnumFlagUIEditor), typeof(UITypeEditor)));
            }
            //add description
            var desc = fi.GetCustomAttributes(false);
            foreach (var item in desc)
            {
                if(item is DescriptionAttribute)
                {
                    newAtribs.Add(item as DescriptionAttribute);
                }
            }
            this.AttributeArray = newAtribs.ToArray();
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return m_fi.GetValue(m_animation);
        }

        public override void ResetValue(object component)
        {

        }

        public override void SetValue(object component, object value)
        {
            m_fi.SetValue(m_animation, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
