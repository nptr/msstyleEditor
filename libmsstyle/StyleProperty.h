#pragma once

#include "VisualStyleDefinitions.h"
#include <stdint.h>

namespace libmsstyle
{
	#pragma pack(push, 1)
	struct PropertyHeader
	{
		int32_t nameID;		// Offset: 0, Size: 4,	ID for the property name, described in MSDN
		int32_t typeID;		// Offset: 4, Size: 4,	ID for the type of the property, described in MSDN
		int32_t classID;	// Offset: 8, Size: 4,	Index to the class from CMAP the propery belongs to
		int32_t partID;		// Offset: 12, Size: 4	ID for the part of the class the property belongs to, see vsstyle.h
		int32_t stateID;	// Offset: 16, Size: 4	ID for the state map, see vsstyle.h
	};
	#pragma pack(pop)


	#pragma pack(push, 1)
	union PropertyData		// Offset: 20, Size: 20
	{
		struct
		{
			int32_t imageID;		// the images resource id, for MAKEINTRESOURCE()
			int32_t reserved;
			int32_t sizeInBytes;	// 0x10 - wrong, no data follows!
		}imagetype;
		struct
		{
			int32_t fontID;			// the font-strings resource id, for MAKEINTRESOURCE(), load from <en-US>/XXXX.msstyles.mui
			int32_t reserved;
			int32_t sizeInBytes;	// 0x5C - wrong, no data follows!
		}fonttype;
		struct
		{
			int32_t shortFlag;
			int32_t reserved;
			int32_t sizeInBytes;	// 0x4 - never includes padding
			int32_t value;
			char padding[4];		// 8 byte alignment
		}inttype;
		struct
		{
			int32_t shortFlag;
			int32_t reserved;
			int32_t sizeInBytes;	// 0x4 - never includes padding
			int32_t size;
			char padding[4];		// 8 byte alignment
		}sizetype;
		struct
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes". Was observed to be 0x7C
			int32_t reserved;
			int32_t sizeInBytes;	// 0x04 - never includes padding
			int32_t boolvalue;
			char padding[4];		// 8 byte alignment
		}booltype;
		struct
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes".
			int32_t reserved;
			int32_t sizeInBytes;	// 0x4 - never includes padding
			unsigned char r;
			unsigned char g;
			unsigned char b;
			unsigned char a;		// not used
			char padding[4];		// 8 byte alignment
		}colortype;
		struct
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes". Values observed: 0x7C, 0x73, 0x76, 0x77, 0x7B
			int32_t reserved;
			int32_t sizeInBytes;	// 0x10 - never includes padding
			int32_t left;
			int32_t top;
			int32_t right;
			int32_t bottom;
		}recttype;
		struct
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes".
			int32_t reserved;
			int32_t sizeInBytes;	// 0x10 - never includes padding
			int32_t left;
			int32_t top;
			int32_t right;
			int32_t bottom;
		}margintype;
		struct
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes".
			int32_t reserved;
			int32_t sizeInBytes;	// 0x4 - never includes padding
			int32_t enumvalue;
			char padding[4];		// 8 byte alignment
		}enumtype;
		struct
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes".
			int32_t reserved;
			int32_t sizeInBytes;	// 0x8 - never includes padding
			int32_t x;
			int32_t y;
		}positiontype;
		struct // INTLIST struct (uxtheme.h)
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes".
			int32_t reserved;
			int32_t sizeInBytes;
			int32_t numInts;
			int32_t firstInt;
		}intlist;
		struct // Type 240, analog to INTLIST?
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes".
			int32_t reserved;
			int32_t sizeInBytes;	// Unlikes others, this seems to include the 4 padding bytes if: size % 8 != 0
			int32_t numColors;		// number of RGBA quadruplets. does not include the possible padding field
			int32_t firstColorBGR;	// first BGR color, probably a COLORREF -> 0x00bbggrr
		}colorlist;
		struct
		{
			int32_t shortFlag;		// if != 0, no data after "sizeInBytes".
			int32_t reserved;
			int32_t sizeInBytes;	// seems to include the padding if: size % 8 != 0
			wchar_t firstChar;		// first UTF16 character of the string. Not sure if null terminated...
		}texttype;
	};
	#pragma pack(pop)


	class StyleProperty
	{
	public:
		StyleProperty()
			: bytesAfterHeader(0)
			, unknown(nullptr)
		{
			memset(&header, 0, sizeof(PropertyHeader));
			memset(&data, 0, sizeof(PropertyData));
		}

		bool operator==(const StyleProperty& other) const;

		PropertyHeader header;
		PropertyData data;

		std::vector<int32_t> intlist;
		std::wstring text;
		void* unknown;

		int bytesAfterHeader;

		bool IsPropertyValid() const;
		int GetRegularPropertySize() const;
		int GetPropertySizeAsFound() const;
		bool IsNameMatchingType() const;
		bool IsContentMatchingType() const;
		
		IDENTIFIER GetTypeID() const;
		IDENTIFIER GetNameID() const;
		const char* LookupName() const;
		const char* LookupTypeName() const;
		std::string GetValueAsString() const;

		void Initialize(libmsstyle::IDENTIFIER type, libmsstyle::IDENTIFIER ident);

		void UpdateImageLink(int imageID);
		void UpdateInteger(int intVal);
		void UpdateSize(int size);
		void UpdateEnum(int enumVal);
		void UpdateBoolean(bool boolVal);
		void UpdateColor(uint8_t r, uint8_t g, uint8_t b);
		void UpdateRectangle(int left, int top, int right, int bottom);
		void UpdateMargin(int left, int top, int right, int bottom);
		void UpdatePosition(int x, int y);
		void UpdateFont(int fontID);
	};
}