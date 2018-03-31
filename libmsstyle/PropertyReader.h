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
			
			// Does a few range and sanity checks to see if the data could 
			// be valid. It does not do a typeID or nameID lookup, so yet 
			// unknown properties have a chance to work as well.
			bool IsProbablyValidHeader(const char* source);

		private:
			int m_numClasses;
		};


	}
}