#pragma once

#include "StyleState.h"

#include <string>
#include <memory>

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
		StyleState* GetState(int index);
		StyleState* FindState(int stateId);

		int32_t partID;
		std::string partName;

	private:
		class Impl;
		std::shared_ptr<Impl> impl;
	};

}

