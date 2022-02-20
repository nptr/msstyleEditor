using libmsstyle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace msstyleEditor
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
        public override string Description => m_info.Description;
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
                    if (enumInfo.Count <= (int)m_property.GetValue())
                    {
                        // invalid enums, not just unknown values, do occur unfortunately
                        return base.Converter;
                    }

                    return new StyleEnumConverter(enumInfo);
                }
                else if(m_property.Header.typeID == (int)IDENTIFIER.FONT)
                {
                    var fonts = StringTable.FilterFonts(m_style.StringTable);
                    if(fonts.Count == 0)
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
            m_property.SetValue(value);
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

    public class StyleEnumConverter : TypeConverter
    {
        private List<VisualStyleEnumEntry> m_enumInfo;

        public StyleEnumConverter(List<VisualStyleEnumEntry> enumInfo)
        {
            m_enumInfo = enumInfo;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
            {
                return m_enumInfo.Find((e) => e.Name == s).Value;
            }
            else return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is int i)
            {
                return m_enumInfo[i].Name; // TODO: BGTYPE is OOB, Toolbar, SPlitbutton, BGTYPE @ Soft 7
            }
            else return value.ToString();
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<int> lst = new List<int>();
            foreach(var enm in m_enumInfo)
            {
                lst.Add(enm.Value);
            }
            return new StandardValuesCollection(lst);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    public class StyleStringResourceConverter : TypeConverter
    {
        private Dictionary<int, string> m_enumInfo;

        public StyleStringResourceConverter(Dictionary<int, string> enumInfo)
        {
            m_enumInfo = enumInfo;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
            {
                foreach(var kvp in m_enumInfo)
                {
                    if (kvp.Value == s) return kvp.Key;
                }

                return "INVALID";
            }
            else return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is int i)
            {
                return m_enumInfo[i];
            }
            else return value.ToString();
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<int> lst = new List<int>();
            foreach (var enm in m_enumInfo)
            {
                lst.Add(enm.Key);
            }
            return new StandardValuesCollection(lst);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    // TODO: Font property may be ENUM, populated from available resources
}
