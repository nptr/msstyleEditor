using libmsstyle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msstyleEditor
{
	class Exporter
	{
		public static void ExportLogicalStructure(string path, VisualStyle style)
		{
			StringBuilder txt = new StringBuilder(1024 * 4);
			txt.Append("File: "); txt.Append(style.Path);
			txt.AppendFormat("\nProperties: {0}", style.NumProperties);
			txt.AppendFormat("\nPlatform: {0}\n\n", style.Platform.ToDisplayString());

			txt.Append("BEGIN STRUCTURE"); txt.Append("\n");

			foreach (var cls in style.Classes)
			{
				txt.Append("Class: "); txt.Append(cls.Value.ClassName);
				txt.Append("\n");

				foreach (var part in cls.Value.Parts)
				{
					txt.Append("\tPart: "); txt.Append(part.Value.PartName);
					txt.Append("\n");

					foreach (var state in part.Value.States)
					{
						txt.Append("\t\tState: "); txt.Append(state.Value.StateName);
						txt.Append("\n");

						foreach (var prop in state.Value.Properties)
						{
							PropertyInfo propInfo;
							bool hp = VisualStyleProperties.PROPERTY_INFO_MAP.TryGetValue(prop.Header.nameID, out propInfo);
							
							PropertyInfo typeInfo;
							bool ht = VisualStyleProperties.PROPERTY_INFO_MAP.TryGetValue(prop.Header.typeID, out typeInfo);

							if (!hp || !ht)
                            {
								txt.AppendFormat("\t\t\tProp {0} ({1}) ({2})\n"
								, prop.Header.nameID
								, prop.Header.typeID
								, prop.GetValueAsString());
								continue;
							}

							txt.AppendFormat("\t\t\tProp {0} ({1}) ({2})\n"
								, propInfo.Name
								, typeInfo.Name
								, prop.GetValueAsString());
						}
					}
					txt.Append("\n");
				}
				txt.Append("\n");
			}

			txt.Append("END STRUCTURE");

			using (var fs = File.Open(path, FileMode.Create))
			using(var sw = new StreamWriter(fs))
			{
				sw.Write(txt.ToString());
			}

			return;
		}
	}
}
