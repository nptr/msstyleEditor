#pragma once

#include <string>

namespace libmsstyle
{
	class VisualStyle;
}

class Exporter
{
public:
	static void ExportLogicalStructure(const std::string& path, libmsstyle::VisualStyle& style);
};

