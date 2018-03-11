#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"

using namespace libmsstyle;

namespace libmsstyle
{

	bool StyleProperty::IsPropertyValid()
	{
		// Not a known type
		if (typeID < 200 || typeID >= IDENTIFIER::COLORSCHEMES)
			return false;

		// Some color and font props use an type id as name id.
		// They seem to contain valid data, so ill include them.
		if (nameID == IDENTIFIER::COLOR &&
			typeID == IDENTIFIER::COLOR)
			return true;
		if (nameID == IDENTIFIER::FONT &&
			typeID == IDENTIFIER::FONT)
			return true;
		if (nameID == IDENTIFIER::DISKSTREAM &&
			typeID == IDENTIFIER::DISKSTREAM)
			return true;
		if (nameID == IDENTIFIER::STREAM &&
			typeID == IDENTIFIER::STREAM)
			return true;

		// Not sure where the line for valid name ids is.
		if (nameID < IDENTIFIER::COLORSCHEMES)
			return false;

		// Not a known class
		if (classID > 8002)
			return false;

		return true;
	}


	int StyleProperty::GetPropertySize()
	{
		switch (typeID)
		{
		case IDENTIFIER::FILENAME:
		case IDENTIFIER::DISKSTREAM:
			return 32;
		case IDENTIFIER::FONT:
			return 32;
		case IDENTIFIER::INT:
			return 40;
		case IDENTIFIER::SIZE:
			return 40;
		case IDENTIFIER::BOOL:
			return 40;
		case IDENTIFIER::COLOR:
			return 40;
		case IDENTIFIER::RECT:
			return 48;
		case IDENTIFIER::MARGINS:
			return 48;
		case IDENTIFIER::ENUM:
			return 40;
		case IDENTIFIER::POSITION:
			return 40;
		case IDENTIFIER::INTLIST:
			// header, reserved, numints, intlist, nullterminator
			return 20 + 12 + 4 + variants.intlist.numints * sizeof(int32_t);
		case IDENTIFIER::STRING:
			// string length in bytes including the null terminator
			return 20 + 8 + 4 + variants.texttype.sizeInBytes;
			// return 20 + 8 + 4 + (wcslen(&prop.variants.texttype.firstchar) + 1) * sizeof(wchar_t);
		case 225: // Unknown or wrong prop, since Win7 ?
		case 241: // Unknown or wrong prop, since Win10 ?
			return 40;
		default:
			return 40;
		}
	}


	IDENTIFIER StyleProperty::GetTypeID() const
	{
		return static_cast<IDENTIFIER>(typeID);
	}


	IDENTIFIER StyleProperty::GetNameID() const
	{
		return static_cast<IDENTIFIER>(nameID);
	}


	const char* StyleProperty::LookupName()
	{
		auto ret = libmsstyle::PROPERTY_MAP.find(nameID);
		if (ret != libmsstyle::PROPERTY_MAP.end())
			return ret->second;
		else return "UNKNOWN";
	}


	void StyleProperty::UpdateImageLink(int imageID)
	{
		variants.imagetype.imageID = imageID;
	}

	void StyleProperty::UpdateInteger(int intVal)
	{
		variants.inttype.value = intVal;
	}

	void StyleProperty::UpdateSize(int size)
	{
		variants.sizetype.size = size;
	}

	void StyleProperty::UpdateEnum(int enumVal)
	{
		variants.enumtype.enumvalue = enumVal;
	}

	void StyleProperty::UpdateBoolean(bool boolVal)
	{
		variants.booltype.boolvalue = boolVal;
	}

	void StyleProperty::UpdateColor(int r, int g, int b)
	{
		variants.colortype.r = r;
		variants.colortype.g = g;
		variants.colortype.b = b;
	}

	void StyleProperty::UpdateRectangle(int left, int top, int right, int bottom)
	{
		variants.recttype.left = left;
		variants.recttype.top = top;
		variants.recttype.right = right;
		variants.recttype.bottom = bottom;
	}

	void StyleProperty::UpdateMargin(int left, int top, int right, int bottom)
	{
		variants.margintype.left = left;
		variants.margintype.top = top;
		variants.margintype.right = right;
		variants.margintype.bottom = bottom;
	}

	void StyleProperty::UpdatePosition(int x, int y)
	{
		variants.positiontype.x = x;
		variants.positiontype.y = y;
	}

	void StyleProperty::UpdateFont(int fontID)
	{
		variants.fonttype.fontID = fontID;
	}
}