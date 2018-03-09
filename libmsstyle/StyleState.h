#pragma once

#include "StyleProperty.h"

#include <string>
#include <stdint.h>

namespace libmsstyle
{

	class StyleState
	{
	public:
		StyleState();
		~StyleState();

		StyleProperty* AddProperty(StyleProperty* prop);

		void RemoveProperty(int index);
		int GetPropertyCount() const;
		StyleProperty* GetProperty(int index) const;

		int32_t stateID;
		std::string stateName;

	private:
		class Impl;
		Impl* impl;
	};

}

