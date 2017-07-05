#pragma once

#include <stdint.h>

namespace msstyle
{
	typedef struct
	{
		int32_t key;
		const char* value;
	} EnumMap;

	static const EnumMap ENUM_BGTYPE[] =
	{
		{ 0, "IMAGEFILE" },
		{ 1, "BORDERFILL" },
		{ 2, "NONE" }
	};

	static const EnumMap ENUM_BORDERTYPE[] =
	{
		{ 0, "RECT" },
		{ 1, "ROUNDRECT" },
		{ 2, "ELLIPSE" }
	};

	static const EnumMap ENUM_FILLTYPE[] =
	{
		{ 0, "SOLID" },
		{ 1, "VERTGRADIENT" },
		{ 2, "HORIZONTALGRADIENT" },
		{ 3, "RADIALGRADIENT" },
		{ 4, "TILEIMAGE" }
	};

	static const EnumMap ENUM_SIZINGTYPE[] =
	{
		{ 0, "TRUESIZE" },
		{ 1, "STRETCH" },
		{ 2, "TILE" }
	};

	static const EnumMap ENUM_ALIGNMENT[] =
	{
		{ 0, "LEFT" },
		{ 1, "RIGHT" },
		{ 2, "CENTER" }
	};

	static const EnumMap ENUM_ICONEFFECT[] =
	{
		{ 0, "NONE" },
		{ 1, "GLOW" },
		{ 2, "SHADOW" },
		{ 3, "PULSE" },
		{ 4, "ALPHA" }
	};

	static const EnumMap ENUM_TEXTSHADOW[] =
	{
		{ 0, "NONE" },
		{ 1, "SINGLE" },
		{ 2, "CONTINUOUS" }
	};

	static const EnumMap ENUM_IMAGELAYOUT[] =
	{
		{ 0, "VERTICAL" },
		{ 1, "HORIZONTAL" }
	};

	static const EnumMap ENUM_GLYPHTYPE[] =
	{
		{ 0, "NONE" },
		{ 1, "IMAGEGLYPH" },
		{ 2, "FONTGLYPH" }
	};

	static const EnumMap ENUM_OFFSET[] =
	{
		{ 0, "TOPLEFT" },
		{ 1, "TOPRIGHT" },
		{ 2, "TOPMIDDLE" },
		{ 3, "BOTTOMLEFT" },
		{ 4, "BOTTOMRIGHT" },
		{ 5, "BOTTOMMIDDLE" },
		{ 6, "MIDDLERIGHT" },
		{ 7, "LEFTOFCAPTION" },
		{ 8, "RIGHTOFCAPTION" },
		{ 9, "LEFTOFLASTBUTTON" },
		{ 10, "RIGHTOFLASTBUTTON" },
		{ 11, "ABOVELASTBUTTON" },
		{ 12, "BELOWLASTBUTTON" }
	};
}