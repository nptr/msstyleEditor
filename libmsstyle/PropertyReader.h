#pragma once

#include "StyleProperty.h"

namespace libmsstyle
{
	namespace rw
	{
		class PropertyReader
		{
		public:
			PropertyReader(int numClasses);
			const char* ReadNextProperty(const char* source, const char* end, StyleProperty* prop);
			bool IsValidHeader(const char* source);

		private:
			int m_numClasses;
		};


	}
}