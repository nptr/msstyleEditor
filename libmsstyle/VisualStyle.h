#pragma once

#include "StyleClass.h"
#include "StylePart.h"
#include "StyleState.h"
#include "StyleProperty.h"

#include "VisualStyleParts.h"
#include "VisualStyleEnums.h"
#include "VisualStyleDefinitions.h"
#include "ResourceUtil.h"
#include "Lookup.h"

#include <vector>
#include <string>
#include <memory>

namespace libmsstyle
{
	struct ResourceItem
	{
		const void* data;
		unsigned long size;
	};

	class VisualStyle
	{
	public:
		VisualStyle();
		~VisualStyle();

		StyleClass* GetClass(int index);
		int GetClassCount();

		void Load(const std::string& path);
		void Save(const std::string& path);

		Platform GetCompatiblePlatform() const;
		std::string GetPath() const;

		ResourceItem GetResourceFromProperty();

	private:
		class Impl;
		Impl* impl;
	};
}