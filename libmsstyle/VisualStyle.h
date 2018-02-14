#pragma once

#include "StyleClass.h"
#include "StylePart.h"
#include "StyleState.h"
#include "StyleProperty.h"

#include "ResourceUtil.h"

#include <vector>
#include <string>

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

	ModuleHandle m_moduleHandle;

	std::string m_stylePath;
	std::unordered_map<int32_t, StyleClass> m_classes;
};

