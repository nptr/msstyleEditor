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
		int32_t shortFlag;	// Offset: 20, Size: 4	If not 0, ignore 'sizeInBytes' as no data follows. Instead this field may contain data.
		int32_t reserved;	// Offset: 24, Size: 4	Seems to be always zero
		int32_t sizeInBytes;// Offset: 28, Size: 4	The size of the data that follows. Does not include padding
	};
#pragma pack(pop)


#pragma pack(push, 1)
	union PropertyData
	{
		struct
		{
			int32_t value;
		}inttype;
		struct
		{
			int32_t size;
		}sizetype;
		struct
		{
			int32_t boolvalue;
		}booltype;
		struct
		{
			unsigned char r;
			unsigned char g;
			unsigned char b;
			unsigned char a;		// not used
		}colortype;
		struct
		{
			int32_t left;
			int32_t top;
			int32_t right;
			int32_t bottom;
		}recttype;
		struct
		{
			int32_t left;
			int32_t top;
			int32_t right;
			int32_t bottom;
		}margintype;
		struct
		{
			int32_t enumvalue;
		}enumtype;
		struct
		{
			int32_t x;
			int32_t y;
		}positiontype;
		struct // INTLIST struct (uxtheme.h)
		{
			int32_t numInts;
			int32_t firstInt;
		}intlist;
		struct // Type 240
		{
			int32_t firstColorBGR;	// first BGR color, probably a COLORREF -> 0x00bbggrr
		}colorlist;
		struct
		{
			wchar_t firstChar;		// first wide character of the string. Not sure if null terminated...
		}texttype;
	};
#pragma pack(pop)


	class StyleProperty
	{
	public:
		StyleProperty();
		~StyleProperty();

		bool operator==(const StyleProperty& other) const;

		PropertyHeader header;
		PropertyData data;

		std::vector<int32_t> intlist;
		std::wstring text;
		void* unknown;

		int bytesAfterHeader;

		bool IsPropertyValid() const;
		int GetRegularPropertySize() const;
		bool IsNameMatchingType() const;

		IDENTIFIER GetTypeID() const;
		IDENTIFIER GetNameID() const;
		const char* LookupName() const;
		const char* LookupTypeName() const;
		std::string GetValueAsString() const;

		int GetResourceID() const;

		void Initialize(libmsstyle::IDENTIFIER type, libmsstyle::IDENTIFIER ident);

		void UpdateImageLink(int imageID);
		void UpdateInteger(int intVal);
		void UpdateIntegerUnchecked(int intVal);
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