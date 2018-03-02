#pragma once

#include "StyleState.h"

#include <string>
#include <unordered_map>
#include <stdint.h>

namespace libmsstyle
{

	class StylePart
	{
	public:
		StylePart();
		~StylePart();

		StyleState* AddState(const StyleState& state);
		int GetStateCount();
		const StyleState* GetState(int index);
		StyleState* FindState(int stateId);

		int32_t partID;
		std::string partName;

	private:
		std::unordered_map<int32_t, StyleState> m_states;
	};

}

