#pragma once

#include "StyleClass.h"
#include "StylePart.h"
#include "StyleState.h"
#include "StyleProperty.h"

#include "VisualStyleParts.h"

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

	class VisualStyle
	{
	public:
		VisualStyle();
		~VisualStyle();

		const StyleClass* GetClass(int index);
		int GetClassCount();

		void Load(const std::string& path);
		void Save(const std::string& path);


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

