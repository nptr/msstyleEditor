#pragma once

#include "StyleClass.h"
#include "StylePart.h"
#include "StyleState.h"
#include "StyleProperty.h"
#include "StyleResource.h"

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
	class VisualStyle
	{
	public:
		typedef std::unordered_map<int32_t, StyleClass>::iterator ClassIterator;

		VisualStyle();
		~VisualStyle();

		size_t GetClassCount();

		ClassIterator begin();
		ClassIterator end();

		void Load(const std::string& path);
		void Save(const std::string& path);

		int GetPropertyCount() const;
		Platform GetCompatiblePlatform() const;
		std::string GetPath() const;

		std::string GetQueuedResourceUpdate(int nameId, StyleResourceType type);
		void QueueResourceUpdate(int nameId, StyleResourceType type, const std::string& pathToNew);
		StyleResource GetResourceFromProperty(const StyleProperty& prop);

	private:
		class Impl;
		Impl* impl;
	};
}