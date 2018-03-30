#include "PropertyWriter.h"
#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"

namespace libmsstyle
{
	namespace rw
	{
		char* PropertyWriter::WriteProperty(char* dest, StyleProperty& prop)
		{
			memcpy(dest, &prop.header, sizeof(PropertyHeader));
			dest += sizeof(PropertyHeader);

			switch (prop.header.typeID)
			{
			// 32 bytes
			case IDENTIFIER::FILENAME:
			case IDENTIFIER::DISKSTREAM:
			case IDENTIFIER::FONT:
			{
				memcpy(dest, &prop.data, 12);
				dest += 12;
			} break;
			// 40 bytes
			case IDENTIFIER::INT:
			case IDENTIFIER::SIZE:
			case IDENTIFIER::BOOL:
			case IDENTIFIER::COLOR:
			case IDENTIFIER::ENUM:
			case IDENTIFIER::POSITION:
			{
				if (prop.data.booltype.shortFlag != 0)
				{
					memcpy(dest, &prop.data, 12);
					dest += 12;
				}
				else
				{
					memcpy(dest, &prop.data, 20);
					dest += 20;
				}
			} break;
			// 48 bytes
			case IDENTIFIER::RECT:
			case IDENTIFIER::MARGINS:
			{
				if (prop.data.recttype.shortFlag != 0)
				{
					memcpy(dest, &prop.data, 12);
					dest += 12;
				}
				else
				{
					memcpy(dest, &prop.data, 28);
					dest += 28;
				}
			} break;
			// arbitrary
			case IDENTIFIER::INTLIST:
			{
				// shortFlag, unknown 8 bytes & length field
				memcpy(dest, &prop.data, 16);
				dest += 16;

				for (auto& num : prop.intlist)
				{
					// copy data, inc dest
					*dest++ = (num >> 0) & 0xFF;
					*dest++ = (num >> 8) & 0xFF;
					*dest++ = (num >> 16) & 0xFF;
					*dest++ = (num >> 24) & 0xFF;
				}

				// padding the list to a multiple of eight
				// seems to be required.
				size_t ptr = reinterpret_cast<size_t>(dest);
				if (ptr % 8 != 0)
				{
					*dest++ = 0;
					*dest++ = 0;
					*dest++ = 0;
					*dest++ = 0;
				}
			} break;
			case IDENTIFIER::STRING:
			{
				// shortFlag, unknown 4 bytes & length field
				memcpy(dest, &prop.data, 12);
				dest += 12;

				for (auto& ch : prop.text)
				{
					// copy data, inc dest
					*dest++ = (ch >> 0) & 0xFF;
					*dest++ = (ch >> 8) & 0xFF;
				}

				// null terminator
				*dest++ = 0;
				*dest++ = 0;
			} break;
			default:
			{
				// blindly copy the data back that we found earlier
				int cnt = prop.GetPropertySizeAsFound() - sizeof(PropertyHeader);
				memcpy(dest, &prop.data, cnt);
				dest += cnt;
			} break;
			}

			return dest;
		}
	}
}