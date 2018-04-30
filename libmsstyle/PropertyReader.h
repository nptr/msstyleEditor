#pragma once

#include "StyleProperty.h"

namespace libmsstyle
{
	namespace rw
	{
		class PropertyReader
		{
		public:

			enum Result
			{
				Ok,
				End,
				SkippedBytes,
				UnknownType,
				BadProperty
			};

			PropertyReader(int numClasses);

			//
			// Tries to read a property beginning at "src".
			// Returns:
			// - Ok: read a property
			// - End: reached the end. "out_prop" is not valid!
			// - SkippedBytes: At "src" was no property.
			//				   "out_prop" is not valid
			//				   "out_next" should be the next property
			// - UnknownProp:  Property at "src" was unknown.
			//				   "src" < "out_next"
			//				   "out_prop" is not valid
			// - BadProperty:  Property at "src" was invalid.
			//				   "src" < "out_next"
			//				   "out_prop" is not valid
			Result ReadNextProperty(const char* src, const char* end, const char** out_next, StyleProperty* out_prop);
			
			// Does a few range and sanity checks to see if the data could 
			// be valid. It does not do a typeID or nameID lookup, so yet 
			// unknown properties have a chance to work as well.
			bool IsProbablyValidHeader(const char* source);

		private:
			char m_textbuffer[64];
			int m_numClasses;
		};
	}
}