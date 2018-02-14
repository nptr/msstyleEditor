#pragma once

#include "StyleProperty.h"

#include <vector>
#include <string>
#include <stdint.h>


class StyleState
{
public:
	StyleState();
	~StyleState();

	void AddProperty(const StyleProperty& prop);

	void RemoveProperty(int index);
	int GetPropertyCount();
	const StyleProperty* GetProperty(int index);

	int32_t stateID;
	std::string stateName;

private:
	std::vector<StyleProperty> m_properties;
};

