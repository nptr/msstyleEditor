#include "PropertyReader.h"
#include "StyleProperty.h"
#include "VisualStyleDefinitions.h"

#include <algorithm>

namespace libmsstyle
{
	namespace rw
	{
		PropertyReader::PropertyReader(int numClasses)
			: m_numClasses(numClasses)
		{
		}

		
		const char* PropertyReader::ReadNextProperty(const char* source, Result& result, StyleProperty* prop)
		{
			const char* cursor = source;
			while (!IsProbablyValidHeader(cursor))
				cursor += 1;

			// Notify if not the usual padding
			ptrdiff_t diff = cursor - source;
			if (diff > 4)
			{
				result = Result::SkippedBytes;
				return cursor;
			}

			// Copy the property header
			memcpy(&(prop->header), cursor, sizeof(PropertyHeader));
			cursor += sizeof(PropertyHeader);

			// Copy the data
			switch (prop->header.typeID)
			{
			// Arbitrary
			case IDENTIFIER::INTLIST:
			{
                if (prop->header.sizeInBytes == 0)
				{
                    prop->data.intlist.numInts = 0;
				}
				else
				{
					// Copy the "numInts" field
					memcpy(&(prop->data), cursor, 4);
					cursor += 4;
					prop->bytesAfterHeader += 4;
				}

				// Copy the list
				prop->intlist.reserve(prop->data.intlist.numInts);
				for (int32_t i = 0; i < prop->data.intlist.numInts; ++i)
				{
					const int32_t* valuePtr = reinterpret_cast<const int32_t*>(cursor);
					prop->intlist.push_back(*valuePtr);
					cursor += sizeof(int32_t);
				}

				prop->bytesAfterHeader += prop->data.intlist.numInts * sizeof(int32_t);
			} break;
			case IDENTIFIER::COLORLIST:
			{
				int32_t numColors = prop->header.sizeInBytes / 4;

				// Copy the list
				prop->intlist.reserve(numColors);
				for (int32_t i = 0; i < numColors; ++i)
				{
					const int32_t* valuePtr = reinterpret_cast<const int32_t*>(cursor);
					prop->intlist.push_back(*valuePtr);
					cursor += sizeof(int32_t);
				}

				prop->bytesAfterHeader += prop->header.sizeInBytes;
			} break;
			case IDENTIFIER::STRING:
			{
				int32_t szLen = prop->header.sizeInBytes / 2;

				prop->text.reserve(szLen);
				for (int32_t i = 0; i < szLen - 1; ++i) // dont need the NULL term.
				{
					const wchar_t* valuePtr = reinterpret_cast<const wchar_t*>(cursor);
					prop->text.push_back(*valuePtr);
					cursor += sizeof(wchar_t);
				}

				prop->bytesAfterHeader += prop->header.sizeInBytes;
			} break;
			// 32 bytes
			case IDENTIFIER::FILENAME:
			case IDENTIFIER::FILENAME_LITE:
			case IDENTIFIER::DISKSTREAM:
			case IDENTIFIER::FONT:
			// 40 bytes
			case IDENTIFIER::INT:
			case IDENTIFIER::SIZE:
			case IDENTIFIER::BOOLTYPE:
			case IDENTIFIER::COLOR:
			case IDENTIFIER::ENUM:
			case IDENTIFIER::POSITION:
			case IDENTIFIER::HIGHCONTRASTCOLORTYPE:
			// 48 bytes
			case IDENTIFIER::RECTTYPE:
			case IDENTIFIER::MARGINS:
			{
				// Copy the data of known props to the PropertyData field.
				if (prop->header.shortFlag == 0)
				{
					int sizeOfPayload = prop->header.sizeInBytes;
					if (sizeOfPayload > 0 && sizeOfPayload <= sizeof(PropertyData))
					{
						memcpy(&(prop->data), cursor, sizeOfPayload);
						cursor += sizeOfPayload;
						prop->bytesAfterHeader += prop->header.sizeInBytes;
					}
				}

				result = Result::Ok;
				return cursor;
			} break;
			default:
			{
				// Copy the data of known props to an opaque memory block.
				if (prop->header.shortFlag == 0)
				{
					int sizeOfPayload = prop->header.sizeInBytes;
					if (sizeOfPayload > 0)
					{
						prop->unknown = new char[sizeOfPayload];
						memcpy(prop->unknown, cursor, sizeOfPayload);
						cursor += sizeOfPayload;
						prop->bytesAfterHeader += prop->header.sizeInBytes;
					}
				}

				result = Result::UnknownType;
				return cursor;
			} break;
			}

			result = Result::Ok;
			return cursor;
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
				header->nameID > 25000)
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