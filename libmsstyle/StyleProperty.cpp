#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"
#include "StringConvert.h"
#include "Lookup.h"

#include <string.h>
#include <assert.h>

using namespace libmsstyle;

namespace libmsstyle
{
	bool StyleProperty::IsPropertyValid() const
	{
		if (header.typeID < IDENTIFIER::ENUM || header.typeID >= IDENTIFIER::COLORSCHEMES)
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
		// Upper bound is ATLASRECT, but im leaving a bit of space
		// for unknown props.
		if (header.nameID < IDENTIFIER::COLORSCHEMES ||
			header.nameID > 10000)
			return false;

		// First attempt was 255, but yielded false-positives.
		// Smaller than 200 eliminates type & prop name ids.
		if (header.partID < 0 ||
			header.partID > 199)
			return false;

		if (header.stateID < 0 ||
			header.stateID > 199)
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
			return 20 + 12 + 4 + data.intlist.numInts * sizeof(int32_t);
		case IDENTIFIER::STRING:
			// string length in bytes including the null terminator
			return 20 + 8 + 4 + data.texttype.sizeInBytes;
		case 225:					  // Unknown type, since Win7, i think its a mistake. didn't see it again.
		case IDENTIFIER::UNKNOWN_241: // Unknown type, since Win10
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
		auto ret = libmsstyle::PROPERTY_INFO_MAP.find(header.nameID);
		if (ret != libmsstyle::PROPERTY_INFO_MAP.end())
			return ret->second.name;
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
		assert(	header.typeID == IDENTIFIER::FILENAME ||
				header.typeID == IDENTIFIER::DISKSTREAM );
		data.imagetype.imageID = imageID;

	}

	void StyleProperty::UpdateInteger(int intVal)
	{
		assert(header.typeID == IDENTIFIER::INT);
		data.inttype.value = intVal;
	}

	void StyleProperty::UpdateSize(int size)
	{
		assert(header.typeID == IDENTIFIER::SIZE);
		data.sizetype.size = size;
	}

	void StyleProperty::UpdateEnum(int enumVal)
	{
		assert(header.typeID == IDENTIFIER::ENUM);
		data.enumtype.enumvalue = enumVal;
	}

	void StyleProperty::UpdateBoolean(bool boolVal)
	{
		assert(header.typeID == IDENTIFIER::BOOL);
		data.booltype.boolvalue = boolVal;
	}

	void StyleProperty::UpdateColor(uint8_t r, uint8_t g, uint8_t b)
	{
		assert(header.typeID == IDENTIFIER::COLOR);
		data.colortype.r = r;
		data.colortype.g = g;
		data.colortype.b = b;
	}

	void StyleProperty::UpdateRectangle(int left, int top, int right, int bottom)
	{
		assert(header.typeID == IDENTIFIER::RECT);
		data.recttype.left = left;
		data.recttype.top = top;
		data.recttype.right = right;
		data.recttype.bottom = bottom;
	}

	void StyleProperty::UpdateMargin(int left, int top, int right, int bottom)
	{
		assert(header.typeID == IDENTIFIER::MARGINS);
		data.margintype.left = left;
		data.margintype.top = top;
		data.margintype.right = right;
		data.margintype.bottom = bottom;
	}

	void StyleProperty::UpdatePosition(int x, int y)
	{
		assert(header.typeID == IDENTIFIER::POSITION);
		data.positiontype.x = x;
		data.positiontype.y = y;
	}

	void StyleProperty::UpdateFont(int fontID)
	{
		assert(header.typeID == IDENTIFIER::FONT);
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
			return lookup::FindFontName(data.fonttype.fontID);
		} break;
		case IDENTIFIER::INTLIST:
		{
			if (data.intlist.numInts >= 3)
			{
				sprintf(textbuffer, "Len: %d, Values: %d, %d, %d, ...", data.intlist.numInts
					, *(&data.intlist.firstInt + 0)
					, *(&data.intlist.firstInt + 1)
					, *(&data.intlist.firstInt + 2));
			}
			else sprintf(textbuffer, "Len: %d, Values omitted", data.intlist.numInts);
			return std::string(textbuffer);
		} break;
		default:
		{
			return "Unsupported";
		}
		}
	}


	void StyleProperty::Initialize(libmsstyle::IDENTIFIER type, libmsstyle::IDENTIFIER ident)
	{
		this->header.nameID = ident;
		this->header.typeID = type;

		// Initialize fields with values which purpose i don't know yet.
		// They are required, otherwise windows rejects the style.
		switch (type)
		{
		case libmsstyle::DIBDATA:
			break;
		case libmsstyle::GLYPHDIBDATA:
			break;
		case libmsstyle::ENUM:
			this->data.enumtype.sizeInBytes = 0x4;
			break;
		case libmsstyle::STRING:
			// dynamic !!
			break;
		case libmsstyle::INT:
			this->data.inttype.sizeInBytes = 0x4;
			break;
		case libmsstyle::BOOL:
			this->data.booltype.sizeInBytes = 0x4;
			break;
		case libmsstyle::COLOR:
			this->data.colortype.sizeInBytes = 0x4;
			break;
		case libmsstyle::MARGINS:
			this->data.margintype.sizeInBytes = 0x10;
			break;
		case libmsstyle::FILENAME:
			this->data.imagetype.sizeInBytes = 0x10;
			break;
		case libmsstyle::SIZE:
			this->data.sizetype.sizeInBytes = 0x4;
			break;
		case libmsstyle::POSITION:
			this->data.positiontype.sizeInBytes = 0x8;
			break;
		case libmsstyle::RECT:
			this->data.recttype.sizeInBytes = 0x10;
			break;
		case libmsstyle::FONT:
			this->data.fonttype.sizeInBytes = 0x5C;
			break;
		case libmsstyle::INTLIST:
			// dynamic !!
			break;
		case libmsstyle::HBITMAP:
			break;
		case libmsstyle::DISKSTREAM:
			break;
		case libmsstyle::STREAM:
			break;
		case libmsstyle::BITMAPREF:
			break;
		case libmsstyle::FLOAT:
			break;
		default:
			break;
		}
	}
}