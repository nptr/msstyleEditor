#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"
#include <string.h>
#include "StringConvert.h"
#include "Lookup.h"

using namespace libmsstyle;

namespace libmsstyle
{
	bool StyleProperty::IsPropertyValid() const
	{
		// Not a known type
		if (header.typeID < 200 || header.typeID >= IDENTIFIER::COLORSCHEMES)
			return false;

		// Some color and font props use an type id as name id.
		// They seem to contain valid data, so ill include them.
		if (header.nameID == IDENTIFIER::COLOR &&
			header.typeID == IDENTIFIER::COLOR)
			return true;
		if (header.nameID == IDENTIFIER::FONT &&
			header.typeID == IDENTIFIER::FONT)
			return true;
		if (header.nameID == IDENTIFIER::DISKSTREAM &&
			header.typeID == IDENTIFIER::DISKSTREAM)
			return true;
		if (header.nameID == IDENTIFIER::STREAM &&
			header.typeID == IDENTIFIER::STREAM)
			return true;

		// Not sure where the line for valid name ids is.
		if (header.nameID < IDENTIFIER::COLORSCHEMES)
			return false;

		// Not a known class
		if (header.classID > 8002)
			return false;

		return true;
	}

	bool StyleProperty::IsNameMatchingType() const
	{
		// lookup typemap if prop exists in there
		return true;
	}

	bool StyleProperty::IsContentMatchingType() const
	{
		return true;
	}

	int StyleProperty::GetRegularPropertySize() const
	{
		switch (header.typeID)
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
			return 20 + 12 + 4 + data.intlist.numints * sizeof(int32_t);
		case IDENTIFIER::STRING:
			// string length in bytes including the null terminator
			return 20 + 8 + 4 + data.texttype.sizeInBytes;
			// return 20 + 8 + 4 + (wcslen(&prop.data.texttype.firstchar) + 1) * sizeof(wchar_t);
		case 225: // Unknown or wrong prop, since Win7 ?
		case 241: // Unknown or wrong prop, since Win10 ?
			return 40;
		default:
			return 40;
		}
	}

	int StyleProperty::GetPropertySizeAsFound() const
	{
		return sizeof(PropertyHeader) + bytesAfterHeader;
	}


	IDENTIFIER StyleProperty::GetTypeID() const
	{
		return static_cast<IDENTIFIER>(header.typeID);
	}


	IDENTIFIER StyleProperty::GetNameID() const
	{
		return static_cast<IDENTIFIER>(header.nameID);
	}


	const char* StyleProperty::LookupName() const
	{
		auto ret = libmsstyle::PROPERTY_MAP.find(header.nameID);
		if (ret != libmsstyle::PROPERTY_MAP.end())
			return ret->second;
		else return "UNKNOWN";
	}

	const char* StyleProperty::LookupTypeName() const
	{
		auto ret = libmsstyle::DATATYPE_MAP.find(header.typeID);
		if (ret != libmsstyle::DATATYPE_MAP.end())
			return ret->second;
		else return "UNKNOWN";
	}


	bool StyleProperty::operator==(const StyleProperty& other) const
	{
		if (this->header.nameID == other.header.nameID &&
			this->header.typeID == other.header.typeID)
		{
			return this->GetValueAsString() == other.GetValueAsString();
		}
		else return false;
	}


	void StyleProperty::UpdateImageLink(int imageID)
	{
		data.imagetype.imageID = imageID;
	}

	void StyleProperty::UpdateInteger(int intVal)
	{
		data.inttype.value = intVal;
	}

	void StyleProperty::UpdateSize(int size)
	{
		data.sizetype.size = size;
	}

	void StyleProperty::UpdateEnum(int enumVal)
	{
		data.enumtype.enumvalue = enumVal;
	}

	void StyleProperty::UpdateBoolean(bool boolVal)
	{
		data.booltype.boolvalue = boolVal;
	}

	void StyleProperty::UpdateColor(uint8_t r, uint8_t g, uint8_t b)
	{
		data.colortype.r = r;
		data.colortype.g = g;
		data.colortype.b = b;
	}

	void StyleProperty::UpdateRectangle(int left, int top, int right, int bottom)
	{
		data.recttype.left = left;
		data.recttype.top = top;
		data.recttype.right = right;
		data.recttype.bottom = bottom;
	}

	void StyleProperty::UpdateMargin(int left, int top, int right, int bottom)
	{
		data.margintype.left = left;
		data.margintype.top = top;
		data.margintype.right = right;
		data.margintype.bottom = bottom;
	}

	void StyleProperty::UpdatePosition(int x, int y)
	{
		data.positiontype.x = x;
		data.positiontype.y = y;
	}

	void StyleProperty::UpdateFont(int fontID)
	{
		data.fonttype.fontID = fontID;
	}

	std::string StyleProperty::GetValueAsString() const
	{
		char textbuffer[64];
		switch (header.typeID)
		{
		case IDENTIFIER::ENUM:
		{
			const char* enumStr = lookup::GetEnumAsString(header.nameID, data.enumtype.enumvalue);
			if (enumStr != nullptr)
				return std::string(enumStr);
			else return std::string("UNKNOWN ENUM");
		} break;
		case IDENTIFIER::STRING:
		{
			return WideToUTF8(text);
		} break;
		case IDENTIFIER::INT:
		{
			return std::to_string(data.inttype.value);
		} break;
		case IDENTIFIER::BOOL:
		{
			if (data.booltype.boolvalue > 0)
				return std::string("true");
			else return std::string("false");
		} break;
		case IDENTIFIER::COLOR:
		{
			sprintf(textbuffer, "%d, %d, %d", data.colortype.r, data.colortype.g, data.colortype.b);
			return std::string(textbuffer);
		} break;
		case IDENTIFIER::MARGINS:
		{
			sprintf(textbuffer, "%d, %d, %d, %d", data.margintype.left, data.margintype.top, data.margintype.right, data.margintype.bottom);
			return std::string(textbuffer);
		} break;
		case IDENTIFIER::FILENAME:
		case IDENTIFIER::DISKSTREAM:
		{
			return std::to_string(data.imagetype.imageID);
		} break;
		case IDENTIFIER::SIZE:
		{
			return std::to_string(data.sizetype.size);
		} break;
		case IDENTIFIER::POSITION:
		{
			sprintf(textbuffer, "%d, %d", data.positiontype.x, data.positiontype.y);
			return std::string(textbuffer);
		} break;
		case IDENTIFIER::RECT:
		{
			sprintf(textbuffer, "%d, %d, %d, %d", data.recttype.left, data.recttype.top, data.recttype.right, data.recttype.bottom);
			return std::string(textbuffer);
		} break;
		case IDENTIFIER::FONT:
		{
			// todo: lookup resource id?
			return std::to_string(data.fonttype.fontID);
		} break;
		case IDENTIFIER::INTLIST:
		{
			if (data.intlist.numints >= 3)
			{
				sprintf(textbuffer, "Len: %d, Values: %d, %d, %d, ...", data.intlist.numints
					, *(&data.intlist.firstint + 0)
					, *(&data.intlist.firstint + 1)
					, *(&data.intlist.firstint + 2));
			}
			else sprintf(textbuffer, "Len: %d, Values omitted", data.intlist.numints);
			return std::string(textbuffer);
		} break;
		default:
		{
			return "Unsupported";
		}
		}
	}
}