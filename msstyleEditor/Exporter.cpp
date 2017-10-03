#include "Exporter.h"

#include <fstream>
#include <iostream>
#include <string>
#include <codecvt>

#include "VisualStyle.h"

std::string WStringToUTF8(const std::wstring& str)
{
	std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
	return myconv.to_bytes(str);
}

void Exporter::ExportPropertyCSV(const msstyle::VisualStyle& style)
{

}

void Exporter::ExportLogicalStructure(const std::string& path, const msstyle::VisualStyle& style)
{
	std::ofstream file(path);
	if (!file.is_open())
		throw new std::runtime_error("Can't open or create the file!");

	char buffer[128];

	std::string txt;
	txt.reserve(1024 * 1024 * 4);

	txt.append("File: "); txt.append(WStringToUTF8(style.GetFileName()));
	sprintf(buffer, "\nProperties: %d", style.GetPropertyCount());
	txt.append(buffer);
	txt.append("\n\n");

	txt.append("BEGIN STRUCTURE"); txt.append("\n");

	const std::unordered_map<int32_t, msstyle::MsStyleClass*>* classes = style.GetClasses();

	for (auto& classIt : *classes)
	{
		txt.append("Class: "); txt.append(classIt.second->className);
		txt.append("\n");

		for (auto& partIt : classIt.second->parts)
		{
			txt.append("\tPart: "); txt.append(partIt.second->partName);
			txt.append("\n");

			for (auto& stateIt : partIt.second->states)
			{
				txt.append("\t\tState: "); txt.append(stateIt.second->stateName);
				txt.append("\n");

				for (auto& propIt : stateIt.second->properties)
				{
					sprintf(buffer, "\t\t\tProp @ 0x%.6x, %s (%s)"	, &propIt->nameID - style.GetPropertyBaseAddress()
																	, msstyle::VisualStyle::FindPropName(propIt->nameID)
																	, msstyle::VisualStyle::FindPropName(propIt->typeID));
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