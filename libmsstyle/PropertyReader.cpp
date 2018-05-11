#include "PropertyReader.h"
#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"

#include <algorithm>

#undef min

namespace libmsstyle
{
	namespace rw
	{
		PropertyReader::PropertyReader(int numClasses)
			: m_numClasses(numClasses)
		{
		}

		PropertyReader::Result PropertyReader::ReadNextProperty(const char* source, const char* end, const char** out_next, StyleProperty* prop)
		{
			const char* cursor = source;
			while (!IsProbablyValidHeader(cursor))
				cursor += 1;

			ptrdiff_t diff = cursor - source;
			if (diff > 0)
			{
				*out_next = cursor;
				return Result::SkippedBytes;
			}

			// Copy the property header
			memcpy(&(prop->header), cursor, sizeof(PropertyHeader));
			cursor += sizeof(PropertyHeader);


			// Copy the data
			switch (prop->header.typeID)
			{
			// 32 bytes
			case IDENTIFIER::FILENAME:
			case IDENTIFIER::DISKSTREAM:
			case IDENTIFIER::FONT:
			case IDENTIFIER::UNKNOWN_243:
			{
				memcpy(&(prop->data), cursor, 12);
				cursor += 12;
				prop->bytesAfterHeader += 12;
			} break;
			// 40 bytes
			case IDENTIFIER::INT:
			case IDENTIFIER::SIZE:
			case IDENTIFIER::BOOL:
			case IDENTIFIER::COLOR:
			case IDENTIFIER::ENUM:
			case IDENTIFIER::POSITION:
			case IDENTIFIER::UNKNOWN_241:
			{
				memcpy(&(prop->data), cursor, 12);
				cursor += 12;
				prop->bytesAfterHeader += 12;

				// Copy the rest if not a short prop
				if (prop->data.booltype.shortFlag == 0)
				{
					char* dataPlusPSHDR = reinterpret_cast<char*>(&prop->data) + 12;
					memcpy(dataPlusPSHDR, cursor, 8);
					cursor += 8;
					prop->bytesAfterHeader += 8;
				}
			} break;
			// 48 bytes
			case IDENTIFIER::RECT:
			case IDENTIFIER::MARGINS:
			{
				memcpy(&(prop->data), cursor, 12);
				cursor += 12;
				prop->bytesAfterHeader += 12;

				// Copy the rest if not a short prop
				if (prop->data.recttype.shortFlag == 0)
				{
					char* dataPlusPSHDR = reinterpret_cast<char*>(&prop->data) + 12;
					memcpy(dataPlusPSHDR, cursor, 16);
					cursor += 16;
					prop->bytesAfterHeader += 16;
				}
			} break;
			// Arbitrary
			case IDENTIFIER::INTLIST:
			{
				// I encountered an INTLIST prop, that had the
				// short indicator NOT set, but still ended right
				// after it. That shouldn't be allowed from my understanding.
				// I'll reject it until i know more, not wanting this to get too hacky.
				if (IsProbablyValidHeader(cursor + 12))
				{
					memcpy(&(prop->data), cursor, 12);
					cursor += 12;
					prop->bytesAfterHeader += 12;
					
					*out_next = cursor;
					return Result::BadProperty;
				}
				else
				{
					memcpy(&(prop->data), cursor, 16);
					cursor += 16;
					prop->bytesAfterHeader += 16;
				}

				int32_t numValues = prop->data.intlist.numInts;

				prop->intlist.reserve(numValues);
				for (int32_t i = 0; i < numValues; ++i)
				{
					const int32_t* valuePtr = reinterpret_cast<const int32_t*>(cursor);
					prop->intlist.push_back(*valuePtr);
					cursor += sizeof(int32_t);
				}

				prop->bytesAfterHeader += prop->data.intlist.numInts * sizeof(int32_t);
			} break;
			case IDENTIFIER::COLORLIST:
			{
				memcpy(&(prop->data), cursor, 12);
				cursor += 12;
				prop->bytesAfterHeader += 12;

				int32_t numValues = prop->data.colorlist.sizeInBytes / 4;

				prop->intlist.reserve(numValues);
				for (int32_t i = 0; i < numValues; ++i)
				{
					const int32_t* valuePtr = reinterpret_cast<const int32_t*>(cursor);
					prop->intlist.push_back(*valuePtr);
					cursor += sizeof(int32_t);
				}

				prop->bytesAfterHeader += prop->data.intlist.numInts * sizeof(int32_t);
			} break;
			case IDENTIFIER::STRING:
			{
				memcpy(&(prop->data), cursor, 12);
				cursor += 12;
				prop->bytesAfterHeader += 12;

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
			default:
			{
				// Unknown type. What we can do is blindly copy the assumend
				// minimum of data every propert has and notify the caller.
				// This does not enable us to work with the style.
				memcpy(&(prop->data), cursor, 12);
				cursor += 12;
				prop->bytesAfterHeader += 12;

				// Alternatively, we could scan for the next property and
				// just store the data in between. In theory this should
				// allow us to handle unknown datatypes as well (limited).
				// <IMPL>

				*out_next = cursor;
				return Result::UnknownType;
			} break;
			}

			*out_next = cursor;
			return Result::Ok;
		}


		bool PropertyReader::IsProbablyValidHeader(const char* source)
		{
			const PropertyHeader* header = reinterpret_cast<const PropertyHeader*>(source);

			if (header->typeID < IDENTIFIER::ENUM || header->typeID >= IDENTIFIER::COLORSCHEMES)
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

			return true;
		}
	}
}