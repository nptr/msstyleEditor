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
				SkippedBytes,
				UnknownType,
				BadProperty
			};

			PropertyReader(int numClasses);

			//
			// Tries to read a property beginning at "src".
			// Returns: pointer to the next location to read from
			// Result:
			// - Ok: read a property. "prop" is valid
			// - End: reached the end. "prop" is not valid!
			// - SkippedBytes: At "src" was no property. Had to skip more than 4 bytes.
			//				   "prop" is not valid
			// - UnknownType:  Property type at "src" was unknown.
			//				   "prop" header may be valid. data not!
			// - BadProperty:  Property at "src" was invalid.
			//				   "prop" header may be valid. data not!
			const char* ReadNextProperty(const char* src, Result& result, StyleProperty* prop);

			// Does a few range and sanity checks to see if the data could 
			// be valid. It does not do a typeID or nameID lookup, so yet 
			// unknown properties have a chance to work as well.
			bool IsProbablyValidHeader(const char* source);

		private:
			int m_numClasses;
		};
	}
}