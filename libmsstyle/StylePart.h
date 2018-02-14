#pragma once

#include "StyleState.h"

#include <string>
#include <unordered_map>
#include <stdint.h>

class StylePart
{
public:
	StylePart();
	~StylePart();

	void AddState(const StyleState& state);
	int GetStateCount();
	const StyleState* GetState(int index);

	int32_t partID;
	std::string partName;

private:
	std::unordered_map<int32_t, StyleState> m_states;
};

