#pragma once

#include <stdint.h>

namespace libmsstyle
{
	typedef struct _EnumMap
	{
		int32_t key;
		const char* value;
	} EnumMap;

	extern const EnumMap ENUM_BGTYPE[3];
	extern const EnumMap ENUM_IMAGELAYOUT[2];
	extern const EnumMap ENUM_BORDERTYPE[3];
	extern const EnumMap ENUM_FILLTYPE[5];
	extern const EnumMap ENUM_SIZINGTYPE[3];
	extern const EnumMap ENUM_ALIGNMENT_H[3];
	extern const EnumMap ENUM_ALIGNMENT_V[3];
	extern const EnumMap ENUM_OFFSET[13];
	extern const EnumMap ENUM_ICONEFFECT[5];
	extern const EnumMap ENUM_TEXTSHADOW[3];
	extern const EnumMap ENUM_GLYPHTYPE[3];
	extern const EnumMap ENUM_IMAGESELECT[3];
	extern const EnumMap ENUM_TRUESIZESCALING[3];
	extern const EnumMap ENUM_GLYPHFONTSCALING[3];
}