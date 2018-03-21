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
		typedef std::unordered_map<int, StyleState>::iterator StateIterator;

		StylePart();
		~StylePart();

		StyleState* AddState(const StyleState& state);
		StyleState* FindState(int stateId) const;
		size_t GetStateCount() const;

		StateIterator begin();
		StateIterator end();

		int32_t partID;
		std::string partName;

	private:
		class Impl;
		std::shared_ptr<Impl> impl;
	};

}

