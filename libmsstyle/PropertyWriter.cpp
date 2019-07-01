#include "PropertyWriter.h"
#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"

namespace libmsstyle
{
	namespace rw
	{
		char* PropertyWriter::PadToMultipleOf(char* source, char* cursor, int align)
		{
			while ((cursor - source) % align != 0)
			{
				*cursor++ = 0;
			}
			return cursor;
		}

		char* PropertyWriter::WriteProperty(char* dest, StyleProperty& prop)
		{
			char* source = dest;
			memcpy(dest, &prop.header, sizeof(PropertyHeader));
			dest += sizeof(PropertyHeader);

			switch (prop.header.typeID)
			{
				// arbitrary
			case IDENTIFIER::INTLIST:
			{
				if (prop.header.sizeInBytes != 0)
				{
					// numInts field
					memcpy(dest, &prop.data, 4);
					dest += 4;

					for (auto& num : prop.intlist)
					{
						// copy data, inc dest
						*dest++ = (num >> 0) & 0xFF;
						*dest++ = (num >> 8) & 0xFF;
						*dest++ = (num >> 16) & 0xFF;
						*dest++ = (num >> 24) & 0xFF;
					}
				}

				dest = PadToMultipleOf(source, dest, 8);
			} break;
			case IDENTIFIER::COLORLIST:
			{
				for (auto& num : prop.intlist)
				{
					// copy data, inc dest
					*dest++ = (num >> 0) & 0xFF;
					*dest++ = (num >> 8) & 0xFF;
					*dest++ = (num >> 16) & 0xFF;
					*dest++ = (num >> 24) & 0xFF;
				}

				dest = PadToMultipleOf(source, dest, 8);
			} break;
			case IDENTIFIER::STRING:
			{
				for (auto& ch : prop.text)
				{
					// copy data, inc dest
					*dest++ = (ch >> 0) & 0xFF;
					*dest++ = (ch >> 8) & 0xFF;
				}

				dest = PadToMultipleOf(source, dest, 8);
			} break;
			// 32 bytes - padding included
			case IDENTIFIER::FILENAME:
			case IDENTIFIER::FILENAME_LITE:
			case IDENTIFIER::DISKSTREAM:
			case IDENTIFIER::FONT:
				// 40 bytes - padding included
			case IDENTIFIER::INT:
			case IDENTIFIER::SIZE:
			case IDENTIFIER::BOOLTYPE:
			case IDENTIFIER::COLOR:
			case IDENTIFIER::ENUM:
			case IDENTIFIER::POSITION:
			case IDENTIFIER::HIGHCONTRASTCOLORTYPE:
				// 48 bytes - padding included
			case IDENTIFIER::RECTTYPE:
			case IDENTIFIER::MARGINS:
			{
				// copy data from known prop
				if (prop.header.shortFlag == 0)
				{
					memcpy(dest, &prop.data, prop.header.sizeInBytes);
					dest += prop.header.sizeInBytes;
				}

				dest = PadToMultipleOf(source, dest, 8);
			} break;
			default:
			{
				// copy data from opaque memory
				if (prop.header.shortFlag == 0)
				{
					memcpy(dest, prop.unknown, prop.header.sizeInBytes);
					dest += prop.header.sizeInBytes;
				}

				dest = PadToMultipleOf(source, dest, 8);
			} break;
			}

			return dest;
		}
	}
}