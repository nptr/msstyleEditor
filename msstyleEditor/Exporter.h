#pragma once

#include <string>

namespace libmsstyle
{
	class VisualStyle;
}

class Exporter
{
public:
	static void ExportPropertyCSV(const libmsstyle::VisualStyle& style);
	static void ExportLogicalStructure(const std::string& path, libmsstyle::VisualStyle& style);
};

