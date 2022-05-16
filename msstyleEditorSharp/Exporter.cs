using libmsstyle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace msstyleEditor
{
	class Exporter
	{
		private static void WriteProperty(XmlWriter writer, StyleProperty prop)
        {
			writer.WriteStartElement("Prop");
			writer.WriteAttributeString("NameId", prop.Header.nameID.ToString());
			writer.WriteAttributeString("TypeId", prop.Header.typeID.ToString());

			PropertyInfo propInfo;
			if (VisualStyleProperties.PROPERTY_INFO_MAP.TryGetValue(prop.Header.nameID, out propInfo))
			{
				writer.WriteAttributeString("Name", propInfo.Name);
			}

			PropertyInfo typeInfo;
			if (VisualStyleProperties.PROPERTY_INFO_MAP.TryGetValue(prop.Header.typeID, out typeInfo))
			{
				writer.WriteAttributeString("Type", typeInfo.Name);
			}

			switch ((IDENTIFIER)prop.Header.typeID)
			{
				case IDENTIFIER.INTLIST:
					{
						writer.WriteStartElement("IntList");

						var l = (List<Int32>)prop.GetValue();
						foreach (var v in l)
                        {
							writer.WriteStartElement("Int");
							writer.WriteString(v.ToString());
							writer.WriteEndElement();
						}

						writer.WriteEndElement();
					}
					break;
				case IDENTIFIER.COLORLIST:
					{
						writer.WriteStartElement("ColorList");

						var l = (List<System.Drawing.Color>)prop.GetValue();
						foreach (var v in l)
						{
							writer.WriteStartElement("Color");
							writer.WriteAttributeString("R", v.R.ToString());
							writer.WriteAttributeString("G", v.G.ToString());
							writer.WriteAttributeString("B", v.B.ToString());
							writer.WriteEndElement();
						}

						writer.WriteEndElement();
					}
					break;
				case IDENTIFIER.STRING:
					{
						writer.WriteStartElement("String");
						writer.WriteValue((string)prop.GetValue());
						writer.WriteEndElement();
					}
					break;
				case IDENTIFIER.FILENAME:
				case IDENTIFIER.FILENAME_LITE:
				case IDENTIFIER.DISKSTREAM:
				case IDENTIFIER.FONT:
				case IDENTIFIER.INT:
				case IDENTIFIER.SIZE:
                    {
						writer.WriteStartElement("Int");
						writer.WriteValue(prop.GetValue().ToString());
						writer.WriteEndElement();
					}
					break;
				case IDENTIFIER.ENUM:
				case IDENTIFIER.HIGHCONTRASTCOLORTYPE:
					{
						int index = (int)prop.GetValue();
						var list = VisualStyleEnums.Find(prop.Header.nameID);
						if (list != null)
						{
							if (index >= list.Count)
							{
								writer.WriteStartElement("Int");
								writer.WriteValue(index.ToString());
								writer.WriteEndElement();
							}
                            else
                            {
								writer.WriteStartElement("Enum");
								writer.WriteValue(list[index].Name);
								writer.WriteEndElement();
							}
						}
					}
					break;
				case IDENTIFIER.BOOLTYPE:
                    {
						writer.WriteStartElement("Bool");
						writer.WriteValue((bool)prop.GetValue() ? "true" : "false");
						writer.WriteEndElement();
					}
					break;
				case IDENTIFIER.COLOR:
					{
						var v = (System.Drawing.Color)prop.GetValue();
						writer.WriteStartElement("Color");
						writer.WriteAttributeString("R", v.R.ToString());
						writer.WriteAttributeString("G", v.G.ToString());
						writer.WriteAttributeString("B", v.B.ToString());
						writer.WriteEndElement();
					}
					break;
				case IDENTIFIER.POSITION:
					{
						var v = (Size)prop.GetValue();
						writer.WriteStartElement("Position");
						writer.WriteAttributeString("X", v.Width.ToString());
						writer.WriteAttributeString("Y", v.Height.ToString());
						writer.WriteEndElement();
					}
					break;
				case IDENTIFIER.RECTTYPE:
				case IDENTIFIER.MARGINS:
					{
						var m = (Margins)prop.GetValue();
						writer.WriteStartElement("Rect");
						writer.WriteAttributeString("Left", m.Left.ToString());
						writer.WriteAttributeString("Top", m.Top.ToString());
						writer.WriteAttributeString("Right", m.Right.ToString());
						writer.WriteAttributeString("Bottom", m.Bottom.ToString());
						writer.WriteEndElement();
					}
					break;
				default:
                    {
						writer.WriteStartElement("Unknown");
						writer.WriteString(prop.GetValue().ToString());
						writer.WriteEndElement();
					}
					break;
			}

			writer.WriteEndElement();
		}

		public static void ExportLogicalStructure(string path, VisualStyle style)
		{
			XmlWriterSettings settings = new XmlWriterSettings()
			{
				Indent = true,
				IndentChars = "\t",
				OmitXmlDeclaration = true,
			};
			XmlWriter writer = XmlWriter.Create(path, settings);

			writer.WriteStartElement("VisualStyle");

			writer.WriteStartElement("Info");
            {
				writer.WriteStartElement("File");
				writer.WriteString(Path.GetFileName(style.Path));
				writer.WriteEndElement();
				writer.WriteStartElement("Platform");
				writer.WriteString(style.Platform.ToDisplayString());
				writer.WriteEndElement();
				writer.WriteStartElement("NumProperties");
				writer.WriteString(style.NumProperties.ToString());
				writer.WriteEndElement();
			}
			writer.WriteEndElement();

			foreach (var cls in style.Classes)
			{
				writer.WriteStartElement("Class");
				//writer.WriteAttributeString("Id", cls.Value.ClassId.ToString());
				writer.WriteAttributeString("Name", cls.Value.ClassName);

				foreach (var part in cls.Value.Parts)
				{
					writer.WriteStartElement("Part");
					writer.WriteAttributeString("Id", part.Value.PartId.ToString());
					writer.WriteAttributeString("Name", part.Value.PartName);

					foreach (var state in part.Value.States)
					{
						writer.WriteStartElement("State");
						writer.WriteAttributeString("Id", state.Value.StateId.ToString());
						writer.WriteAttributeString("Name", state.Value.StateName);

						foreach (var prop in state.Value.Properties)
						{
							WriteProperty(writer, prop);
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.Close();
			writer.Dispose();
			return;
		}
	}
}
