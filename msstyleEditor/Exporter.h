#pragma once

#include <string>

namespace libmsstyle
{
	class VisualStyle;
}

class Exporter
{
public:
	static void ExportLogicalStructure(const std::wstring& path, libmsstyle::VisualStyle& style);
};

