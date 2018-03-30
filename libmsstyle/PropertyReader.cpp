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
			const char* cursor = source;
			while (!IsValidHeader(cursor))
				cursor += 1;


			// copy the header
			memcpy(&(prop->header), cursor, sizeof(PropertyHeader));
			cursor += sizeof(PropertyHeader);


			if (IsValidHeader(cursor))
			{
				// Minimal-sized property (just the header) ?
				// Never encountered one tho. Most we can do,
				// is initialize to zero
				memset(&(prop->data), 0, sizeof(PropertyData));
			}
			else
			{
				// Copying the data bytes. There may not be enough data to completely
				// fill the PropertyData because of short-properties. Take what we get.
				size_t bytesUntilNext = 0;
				const char* scanCursor = cursor;
				while (!IsValidHeader(scanCursor) && scanCursor < end)
				{
					bytesUntilNext++;
					scanCursor++;
				}


				// Note that im not incrementing the cursor, because i still need it
				// at the beginning of the data section in the following switch-case.
				memcpy(&(prop->data), cursor, std::min(sizeof(PropertyData), bytesUntilNext));
				prop->bytesAfterHeader = bytesUntilNext;


				// Copy overlength data (integer list, string properties ...)
				switch (prop->header.typeID)
				{
					case IDENTIFIER::INTLIST:
					{
						cursor += 16;
						int32_t numValues = prop->data.intlist.numints;

						prop->intlist.reserve(numValues);
						for (int32_t i = 0; i < numValues; ++i)
						{
							const int32_t* valuePtr = reinterpret_cast<const int32_t*>(cursor);
							prop->intlist.push_back(*valuePtr);
							cursor += sizeof(int32_t);
						}

						prop->bytesAfterHeader = 16 + (numValues * sizeof(int32_t));
					} break;
					case IDENTIFIER::STRING:
					{
						cursor += 12;
						int32_t szLen = prop->data.texttype.sizeInBytes / 2;

						prop->text.reserve(szLen);
						for (int32_t i = 0; i < szLen-1; ++i) // dont need the NULL term.
						{
							const wchar_t* valuePtr = reinterpret_cast<const wchar_t*>(cursor);
							prop->text.push_back(*valuePtr);
							cursor += sizeof(wchar_t);
						}

						prop->bytesAfterHeader = 12 + prop->data.texttype.sizeInBytes;
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
						// Finally increment the cursor
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
			}

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
			if(	header->nameID < IDENTIFIER::COLORSCHEMES ||
				header->nameID > 10000)
				return false;

			// I hope those are sensible boundaries
			if (header->partID < 0 ||
				header->partID > 255)
				return false;

			if (header->stateID < 0 || 
				header->stateID > 255)
				return false;

			// Not a known class
			if (header->classID < 0 ||
				header->classID > m_numClasses)
				return false;

			return true;
		}
	}
}