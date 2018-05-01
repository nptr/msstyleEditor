#pragma once



namespace libmsstyle
{
	class StyleProperty;

	namespace rw
	{
		class PropertyWriter
		{
		public:
			char* WriteProperty(char* dest, StyleProperty& prop);
		};
	}
}