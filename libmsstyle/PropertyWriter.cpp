#include "PropertyWriter.h"
#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"

namespace libmsstyle
{
	namespace rw
	{
		char* PropertyWriter::WriteProperty(char* dest, StyleProperty& prop)
		{
			switch (prop.header.typeID)
			{
			// 32 bytes
			case IDENTIFIER::FILENAME:
			case IDENTIFIER::DISKSTREAM:
			case IDENTIFIER::FONT:
			{
				memcpy(dest, &prop.header, sizeof(PropertyHeader));
				dest += sizeof(PropertyHeader);

				memcpy(dest, &prop.data, 12);
				dest += 12;
			} break;
			case IDENTIFIER::INT:
			case IDENTIFIER::SIZE:
			case IDENTIFIER::BOOL:
			case IDENTIFIER::COLOR:
			case IDENTIFIER::ENUM:
			case IDENTIFIER::POSITION:
			{
				memcpy(dest, &prop.header, sizeof(PropertyHeader));
				dest += sizeof(PropertyHeader);

				memcpy(dest, &prop.data, 20);
				dest += 20;
			} break;
			case IDENTIFIER::RECT:
			case IDENTIFIER::MARGINS:
			{
				memcpy(dest, &prop.header, sizeof(PropertyHeader));
				dest += sizeof(PropertyHeader);

				memcpy(dest, &prop.data, 20);
				dest += 28;
			} break;
			case IDENTIFIER::INTLIST:
			{
				memcpy(dest, &prop.header, sizeof(PropertyHeader));
				dest += sizeof(PropertyHeader);

				// reserved & length field
				memcpy(dest, &prop.data, 16);
				dest += 16;

				for (auto& num : prop.intlist)
				{
					// copy data, inc dest
				}

				// nullterminator?
			} break;
			case IDENTIFIER::STRING:
			{
				memcpy(dest, &prop.header, sizeof(PropertyHeader));
				dest += sizeof(PropertyHeader);

				// reserved & length field
				memcpy(dest, &prop.data, 12);
				dest += 12;
			} break;
			default:
				break;
				// ??
			}

			return dest;
		}
	}
}