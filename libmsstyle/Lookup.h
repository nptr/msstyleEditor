#pragma once

#include "VisualStyleDefinitions.h"

namespace libmsstyle
{
	namespace lookup
	{
		typedef struct
		{
			int32_t numParts;
			const PartMap* parts;
		} PartList;

		typedef struct
		{
			int32_t numEnums;
			const EnumMap* enums;
		} EnumList;

		EnumList FindEnums(int32_t nameID);
		PartList FindParts(const char* className, Platform platform);
	}
}
