#pragma once


namespace libmsstyle
{
	class StyleProperty;

	namespace rw
	{
		class PropertyWriter
		{
		public:
			char* PadToMultipleOf(char* source, char* cursor, int align);
			char* WriteProperty(char* dest, StyleProperty& prop);
		};
	}
}