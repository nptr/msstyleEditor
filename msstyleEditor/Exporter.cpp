#include "Exporter.h"

#include <fstream>
#include <iostream>
#include <string>
#include <codecvt>

#include "libmsstyle\VisualStyle.h"

std::string WStringToUTF8(const std::wstring& str)
{
	std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
	return myconv.to_bytes(str);
}

void Exporter::ExportPropertyCSV(const libmsstyle::VisualStyle& style)
{

}

void Exporter::ExportLogicalStructure(const std::string& path, libmsstyle::VisualStyle& style)
{
	std::ofstream file(path);
	if (!file.is_open())
		throw new std::runtime_error("Can't open or create the file!");

	char buffer[128];

	std::string txt;
	txt.reserve(1024 * 1024 * 4);

	txt.append("File: "); txt.append(style.GetPath());
	sprintf(buffer, "\nProperties: %d", style.GetPropertyCount());
	txt.append(buffer);

	switch (style.GetCompatiblePlatform())
	{
		case libmsstyle::WIN7:
		{
			txt.append("\nPlatform: Windows 7\n\n");
		} break;
		case libmsstyle::WIN8:
		case libmsstyle::WIN81:
		{
			txt.append("\nPlatform: Windows 8 / 8.1\n\n");
		} break;
		case libmsstyle::WIN10:
		{
			txt.append("\nPlatform: Windows 10\n\n");
		} break;
		default:
		{
			txt.append("\nPlatform: This should never happen.\n\n");
		} break;
	}

	txt.append("BEGIN STRUCTURE"); txt.append("\n");

	//const std::unordered_map<int32_t, libmsstyle::StyleClass*>* classes = style.GetClasses();

	for (auto& cls : style)
	{
		txt.append("Class: "); txt.append(cls.second.className);
		txt.append("\n");

		for (auto& part : cls.second)
		{
			txt.append("\tPart: "); txt.append(part.second.partName);
			txt.append("\n");

			for (auto& state : part.second)
			{
				txt.append("\t\tState: "); txt.append(state.second.stateName);
				txt.append("\n");

				for (auto& prop : state.second)
				{
					sprintf(buffer, "\t\t\tProp %s (%s) (%s)"
						, prop->LookupName()
						, prop->LookupTypeName()
						, prop->GetValueAsString().c_str());
					txt.append(buffer);
					txt.append("\n");
				}
			}
			txt.append("\n");
		}
		txt.append("\n");
	}

	txt.append("END STRUCTURE");

	file.write(txt.c_str(), txt.length());
	file.close();
	return;
}