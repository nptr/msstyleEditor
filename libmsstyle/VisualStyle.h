#pragma once

#include "StyleClass.h"
#include "StylePart.h"
#include "StyleState.h"
#include "StyleProperty.h"

#include "VisualStyleParts.h"
#include "VisualStyleEnums.h"
#include "VisualStyleProps.h"

#include "ResourceUtil.h"

#include <vector>
#include <string>

namespace libmsstyle
{

	enum Platform
	{
		WIN7,
		WIN8,
		WIN81,
		WIN10
	};

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

		ResourceItem GetResourceById();

		void UpdateImageResource(const StyleProperty* prop, const std::string& newFilePath);
		std::string IsReplacementImageQueued(const StyleProperty* prop) const;


		static EnumMap* GetEnumMapFromNameID(int32_t nameID, int32_t* out_size);

	private:
		void LoadClassmap(Resource classResource);
		void LoadProperties(Resource propResource);
		Platform DeterminePlatform();

		ModuleHandle m_moduleHandle;
		Platform m_stylePlatform;

		std::string m_stylePath;
		std::unordered_map<int32_t, StyleClass> m_classes;
	};

}

