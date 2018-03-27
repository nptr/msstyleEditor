#pragma once



namespace libmsstyle
{
	class StyleProperty;

	namespace rw
	{
		class PropertyWriter
		{
			char* WriteProperty(char* dest, StyleProperty& prop);
		};
	}
}