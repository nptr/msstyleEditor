#pragma once

#include <string>

namespace msstyle
{
	class VisualStyle;
}

class Exporter
{
public:
	static void ExportPropertyCSV(const msstyle::VisualStyle& style);
	static void ExportLogicalStructure(const std::string& path, const msstyle::VisualStyle& style);
};

