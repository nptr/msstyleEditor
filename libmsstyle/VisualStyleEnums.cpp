#pragma once

#include "VisualStyleEnums.h"

namespace libmsstyle
{
	const EnumMap ENUM_BGTYPE[] =
	{
		{ 0, "IMAGEFILE" },
		{ 1, "BORDERFILL" },
		{ 2, "NONE" }
	};

	const EnumMap ENUM_IMAGELAYOUT[] =
	{
		{ 0, "VERTICAL" },
		{ 1, "HORIZONTAL" }
	};

	const EnumMap ENUM_BORDERTYPE[] =
	{
		{ 0, "RECT" },
		{ 1, "ROUNDRECT" },
		{ 2, "ELLIPSE" }
	};

	const EnumMap ENUM_FILLTYPE[] =
	{
		{ 0, "SOLID" },
		{ 1, "VERTGRADIENT" },
		{ 2, "HORIZONTALGRADIENT" },
		{ 3, "RADIALGRADIENT" },
		{ 4, "TILEIMAGE" }
	};

	const EnumMap ENUM_SIZINGTYPE[] =
	{
		{ 0, "TRUESIZE" },
		{ 1, "STRETCH" },
		{ 2, "TILE" }
	};

	const EnumMap ENUM_ALIGNMENT_H[] =
	{
		{ 0, "LEFT" },
		{ 1, "CENTER" },
		{ 2, "RIGHT" }
	};

	const EnumMap ENUM_ALIGNMENT_V[] =
	{
		{ 0, "TOP" },
		{ 1, "CENTER" },
		{ 2, "BOTTOM" }
	};

	const EnumMap ENUM_OFFSET[] =
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

	const EnumMap ENUM_ICONEFFECT[] =
	{
		{ 0, "NONE" },
		{ 1, "GLOW" },
		{ 2, "SHADOW" },
		{ 3, "PULSE" },
		{ 4, "ALPHA" }
	};

	const EnumMap ENUM_TEXTSHADOW[] =
	{
		{ 0, "NONE" },
		{ 1, "SINGLE" },
		{ 2, "CONTINUOUS" }
	};

	const EnumMap ENUM_GLYPHTYPE[] =
	{
		{ 0, "NONE" },
		{ 1, "IMAGEGLYPH" },
		{ 2, "FONTGLYPH" }
	};

	const EnumMap ENUM_IMAGESELECT[] =
	{
		{ 0, "NONE" },
		{ 1, "SIZE" },
		{ 2, "DPI" }
	};

	const EnumMap ENUM_TRUESIZESCALING[] =
	{
		{ 0, "NONE" },
		{ 1, "SIZE" },
		{ 2, "DPI" }
	};

	const EnumMap ENUM_GLYPHFONTSCALING[] =
	{
		{ 0, "NONE" },
		{ 1, "SIZE" },
		{ 2, "DPI" }
	};

	const EnumMap ENUM_HIGHCONTRASTTYPE[] =
	{
		{ 0, "ACTIVECAPTION" },
		{ 1, "CAPTIONTEXT" },
		{ 2, "BTNFACE" },
		{ 3, "BTNTEXT" },
		{ 4, "DESKTOP" },
		{ 5, "GRAYTEXT" },
		{ 6, "HOTLIGHT" },
		{ 7, "INACTIVECAPTION" },
		{ 8, "INACTIVECAPTIONTEXT" },
		{ 9, "HIGHLIGHT" },
		{ 10, "HIGHLIGHTTEXT" },
		{ 11, "WINDOW" },
		{ 12, "WINDOWTEXT" }
	};
}