#pragma once

#include "VisualStyleParts.h"
#include "VisualStyleEnums.h"
#include "VisualStyleDefinitions.h"

#include <string>

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
		const char* GetEnumAsString(int32_t nameID, int32_t enumValue);

		const char* FindPropertyName(int nameID);
		const char* FindTypeName(int typeID);
		std::string FindFontName(int fontID);
	}
}
