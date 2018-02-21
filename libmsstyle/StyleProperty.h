#pragma once

#include <stdint.h>

struct StyleProperty
{
	int32_t nameID;		// Offset: 0, Size: 4,	ID for the property name, described in MSDN
	int32_t typeID;		// Offset: 4, Size: 4,	ID for the type of the property, described in MSDN
	int32_t classID;	// Offset: 8, Size: 4,	Index to the class from CMAP the propery belongs to
	int32_t partID;		// Offset: 12, Size: 4	ID for the part of the class the property belongs to, see vsstyle.h
	int32_t stateID;	// Offset: 16, Size: 4	ID for the state map, see see vsstyle.h
	union				// Offset: 20, Size: 20
	{
		struct
		{
			int32_t imageID;	// for MAKEINTRESOURCE()
			char reserved[16];
		}imagetype;
		struct
		{
			int32_t fontID;		// for MAKEINTRESOURCE() too?
			char reserved[16];
		}fonttype;
		struct
		{
			char reserved[12];
			int32_t value;
			char reserved2[4];
		}inttype;
		struct
		{
			char reserved[12];
			int32_t size;
			char reserved2[4];
		}sizetype;
		struct
		{
			char reserved[12];
			int32_t boolvalue;
			char reserved2[4];
		}booltype;
		struct
		{
			char reserved[12];
			unsigned char r;
			unsigned char g;
			unsigned char b;
			char reserved2[5];
		}colortype;
		struct
		{
			char reserved[12];
			int32_t left;
			int32_t top;
			int32_t right;
			int32_t bottom;
		}recttype;
		struct
		{
			char reserved[12];
			int32_t left;
			int32_t top;
			int32_t right;
			int32_t bottom;
		}margintype;
		struct
		{
			char reserved[12];
			int32_t enumvalue;
			char reserved2[4];
		}enumtype;
		struct
		{
			char reserved[12];
			int32_t x;
			int32_t y;
		}positiontype;
		struct // INTLIST struct (uxtheme.h)
		{
			char reserved[12];
			int32_t numints;
			int32_t firstint;
		}intlist;
		struct
		{
			char reserved[8];
			int32_t sizeInBytes;
			wchar_t firstchar;
		}texttype;
	}variants;

	bool IsPropertyValid();
	int GetPropertySize();

	void UpdateImageProperty(int imageID);
	void UpdateIntegerProperty(int intVal);
	void UpdateSizeProperty(int size);
	void UpdateEnumProperty(int enumVal);
	void UpdateBooleanProperty(bool boolVal);
	void UpdateColorProperty(int r, int g, int b);
	void UpdateRectangleProperty(int left, int top, int right, int bottom);
	void UpdateMarginProperty(int left, int top, int right, int bottom);
	void UpdatePositionProperty(int x, int y);
	void UpdateFontProperty(int fontID);
	void UpdateImageResource(const wchar_t* newFilePath);
};

