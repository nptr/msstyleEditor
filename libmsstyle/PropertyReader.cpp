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
				size_t remainingBytes = end - cursor;

				// Copying the most impartant bytes. The PropertyData struct may overlap
				// with the next property though, when it's in a short-format.
				// I don't mind the overlapped data, so ill copy tho whole thing, but
				// depending on the property type, the cursor is advanced differently
				// so i dont miss the overlapping property.
				memcpy(&(prop->data), cursor, std::min(sizeof(PropertyData), remainingBytes));
				
				// DEBUG: Scan for a header, instead of jumping
				//const char* debugCursor = cursor;
				//for (int i = 0; i < 24; ++i, ++debugCursor)
				//{
				//	if (IsValidHeader(debugCursor))
				//	{
				//		char textbuffer[64];
				//		sprintf(textbuffer, "next header %d bytes after data\n", i);
				//		OutputDebugStringA(textbuffer);
				//	}
				//}

				// Copy overlength data, and determine the real length of the property.
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
					case IDENTIFIER::INT:
					case IDENTIFIER::BOOL:
					case IDENTIFIER::COLOR:
					case IDENTIFIER::MARGINS:
					case IDENTIFIER::FILENAME:
					case IDENTIFIER::SIZE:
					case IDENTIFIER::POSITION:
					case IDENTIFIER::RECT:
					case IDENTIFIER::FONT:
					default:
					{
						int bytesAfterHeader = 0;
						while (!IsValidHeader(cursor) && cursor < end)
						{
							bytesAfterHeader++;
							cursor++;
						}

						prop->bytesAfterHeader = bytesAfterHeader;
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