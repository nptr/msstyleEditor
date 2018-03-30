#pragma once

#include <stdint.h>

#include "VisualStyleDefinitions.h"

namespace libmsstyle
{
	#pragma pack(push, 1)
	struct PropertyHeader
	{
		int32_t nameID;		// Offset: 0, Size: 4,	ID for the property name, described in MSDN
		int32_t typeID;		// Offset: 4, Size: 4,	ID for the type of the property, described in MSDN
		int32_t classID;	// Offset: 8, Size: 4,	Index to the class from CMAP the propery belongs to
		int32_t partID;		// Offset: 12, Size: 4	ID for the part of the class the property belongs to, see vsstyle.h
		int32_t stateID;	// Offset: 16, Size: 4	ID for the state map, see see vsstyle.h
	};
	#pragma pack(pop)


	union PropertyData			// Offset: 20, Size: 20
	{
		struct
		{
			int32_t imageID;	// for MAKEINTRESOURCE()
			char reserved[16];
		}imagetype;
		struct
		{
			int32_t fontID;		// for MAKEINTRESOURCE() too?
			int32_t important1;
			int32_t important2;
		}fonttype;
		struct
		{
			int32_t shortFlag;
			char reserved[8];
			int32_t value;
			char reserved2[4];
		}inttype;
		struct
		{
			int32_t shortFlag;
			char reserved[8];
			int32_t size;
			char reserved2[4];
		}sizetype;
		struct
		{
			int32_t shortFlag; // 0x7C
			int32_t alwaysZero;
			int32_t anUnknownValue; // 0x04
			int32_t boolvalue;
			char reserved2[4];
		}booltype;
		struct
		{
			int32_t shortFlag;
			char reserved[8];
			unsigned char r;
			unsigned char g;
			unsigned char b;
			char reserved2[5];
		}colortype;
		struct
		{
			int32_t shortFlag; // 0x73, 0x76, 0x77, 0x7B
			int32_t alwaysZero;
			int32_t anUnknownValue; // 0x10
			int32_t left;
			int32_t top;
			int32_t right;
			int32_t bottom;
		}recttype;
		struct
		{
			int32_t shortFlag; // same as rect i guess
			int32_t alwaysZero;
			int32_t anUnknownValue; // same as rect i guess
			int32_t left;
			int32_t top;
			int32_t right;
			int32_t bottom;
		}margintype;
		struct
		{
			int32_t shortFlag;
			char reserved[8];
			int32_t enumvalue;
			char reserved2[4];
		}enumtype;
		struct
		{
			int32_t shortFlag;
			char reserved[8];
			int32_t x;
			int32_t y;
		}positiontype;
		struct // INTLIST struct (uxtheme.h)
		{
			int32_t shortFlag;
			char reserved[8];
			int32_t numints;
			int32_t firstint;
		}intlist;
		struct
		{
			char reserved[8];
			int32_t sizeInBytes;
			wchar_t firstchar;
		}texttype;
	};

	class StyleProperty
	{
	public:
		StyleProperty()
		{
			memset(&header, 0, sizeof(PropertyHeader));
			memset(&data, 0, sizeof(PropertyData));
		}

		PropertyHeader header;
		PropertyData data;

		std::vector<int32_t> intlist;
		std::wstring text;

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