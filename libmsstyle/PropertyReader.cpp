#include "PropertyReader.h"
#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"

#include <algorithm>
#include <Windows.h>

#undef min

namespace libmsstyle
{
	namespace rw
	{
		PropertyReader::PropertyReader(int numClasses)
			: m_numClasses(numClasses)
		{
		}

		const char* PropertyReader::ReadNextProperty(const char* source, const char* end, StyleProperty* prop)
		{
			const size_t PSEUDOHEADERSIZE = 12;

			const char* cursor = source;
			while (!IsValidHeader(cursor))
				cursor += 1;

			if (cursor - source >= 32)
			{
				char textbuffer[64];
				const StyleProperty* tmp = reinterpret_cast<const StyleProperty*>(source);
				sprintf(textbuffer, "Skip? Name: %d, Type: %d\r\n", tmp->header.nameID, tmp->header.typeID);
				OutputDebugStringA(textbuffer);
			}

			// Copy the property header
			memcpy(&(prop->header), cursor, sizeof(PropertyHeader));
			cursor += sizeof(PropertyHeader);


			// We should be able to do 12 more bytes at once. Thats the minimal
			// property data size ive found so far. This completes the 32 byte props.
			memcpy(&(prop->data), cursor, PSEUDOHEADERSIZE);
			prop->bytesAfterHeader = PSEUDOHEADERSIZE;

			// Here, we could either check the type, the shortFlag, and jump
			// appropriately, or just scan to be more generic. Both approaches
			// have its ups and downs. Scan:
			size_t bytesUntilNext = 0;
			const char* scanCursor = cursor + PSEUDOHEADERSIZE;
			while (!IsValidHeader(scanCursor) && scanCursor < end)
			{
				bytesUntilNext++;
				scanCursor++;
			}


			// If there is data left (most of the time),
			// copy just enough to fill the PropertyData struct
			// This completes the 40 & 48 byte props.
			if (bytesUntilNext > 0)
			{
				size_t toCopy = std::min(sizeof(PropertyData) - PSEUDOHEADERSIZE, bytesUntilNext);
				char* dataPlusPSHDR = reinterpret_cast<char*>(&prop->data) + PSEUDOHEADERSIZE;

				memcpy(dataPlusPSHDR, cursor + PSEUDOHEADERSIZE, toCopy);
				prop->bytesAfterHeader += toCopy;
			}

			// Copy overlength data (integer list, string properties ...)
			// and move the cursor from its position after the header to
			// the end of the property.
			switch (prop->header.typeID)
			{
			case IDENTIFIER::INTLIST:
			{
				cursor += PSEUDOHEADERSIZE + 4;
				int32_t numValues = prop->data.intlist.numints;

				prop->intlist.reserve(numValues);
				for (int32_t i = 0; i < numValues; ++i)
				{
					const int32_t* valuePtr = reinterpret_cast<const int32_t*>(cursor);
					prop->intlist.push_back(*valuePtr);
					cursor += sizeof(int32_t);
				}

				prop->bytesAfterHeader += 4 + (numValues * sizeof(int32_t));
			} break;
			case IDENTIFIER::STRING:
			{
				cursor += PSEUDOHEADERSIZE;
				int32_t szLen = prop->data.texttype.sizeInBytes / 2;

				prop->text.reserve(szLen);
				for (int32_t i = 0; i < szLen - 1; ++i) // dont need the NULL term.
				{
					const wchar_t* valuePtr = reinterpret_cast<const wchar_t*>(cursor);
					prop->text.push_back(*valuePtr);
					cursor += sizeof(wchar_t);
				}

				prop->bytesAfterHeader += prop->data.texttype.sizeInBytes;
			} break;
			case IDENTIFIER::FILENAME:
			case IDENTIFIER::DISKSTREAM:
			case IDENTIFIER::FONT:
			case IDENTIFIER::INT:
			case IDENTIFIER::BOOL:
			case IDENTIFIER::COLOR:
			case IDENTIFIER::MARGINS:
			case IDENTIFIER::SIZE:
			case IDENTIFIER::POSITION:
			case IDENTIFIER::RECT:
			default:
			{
				cursor += prop->bytesAfterHeader;
			}
			}

			char textbuffer[64];
			if (prop->GetRegularPropertySize() < prop->GetPropertySizeAsFound())
			{
				sprintf(textbuffer, "next header %d bytes after data - bigger\n", prop->bytesAfterHeader);
			}
			if (prop->GetRegularPropertySize() > prop->GetPropertySizeAsFound())
			{
				sprintf(textbuffer, "next header %d bytes after data - smaller\n", prop->bytesAfterHeader);
			}
			if (prop->GetRegularPropertySize() == prop->GetPropertySizeAsFound())
			{
				sprintf(textbuffer, "next header %d bytes after data - exact\n", prop->bytesAfterHeader);
			}
			OutputDebugStringA(textbuffer);

			return cursor;
		}


		bool PropertyReader::IsValidHeader(const char* source)
		{
			const PropertyHeader* header = reinterpret_cast<const PropertyHeader*>(source);

			// Not a known type
			if (header->typeID < 200 || header->typeID > IDENTIFIER::FLOATLIST)
				return false;

			// Some color and font props use an type id as name id.
			// They seem to contain valid data, so ill include them.
			if (header->nameID == IDENTIFIER::COLOR &&
				header->typeID == IDENTIFIER::COLOR)
				return true;
			if (header->nameID == IDENTIFIER::FONT &&
				header->typeID == IDENTIFIER::FONT)
				return true;
			if (header->nameID == IDENTIFIER::DISKSTREAM &&
				header->typeID == IDENTIFIER::DISKSTREAM)
				return true;
			if (header->nameID == IDENTIFIER::STREAM &&
				header->typeID == IDENTIFIER::STREAM)
				return true;

			// Not sure where the line for valid name ids is.
			// Upper bound is ATLASRECT, but im leaving a bit of space
			// for unknown props.
			if (header->nameID < IDENTIFIER::COLORSCHEMES ||
				header->nameID > 10000)
				return false;

			// First attempt was 255, but yielded false-positives.
			// Smaller than 200 eliminates type & prop name ids.
			if (header->partID < 0 ||
				header->partID > 199)
				return false;

			if (header->stateID < 0 ||
				header->stateID > 199)
				return false;

			// Not a known class
			if (header->classID < 0 ||
				header->classID > m_numClasses)
				return false;

			// Last resort - map lookup
			// Problem: As long as the type is known, i could have handled
			// the property. With this check, im sacrificing forward compatiblity.
			// But some properties contain values that just look like a new prop header
			// and with this check
			auto& result = PROPERTY_MAP.find(header->nameID);
			if (result == PROPERTY_MAP.end())
				return false;

			return true;
		}
	}
}