#pragma once

#include "VisualStylePropMaps.h"

#include <stdint.h>
#include <unordered_map>

// Gets the element count of a static array
#define MSSTYLE_ARRAY_LENGTH(name) (sizeof(name) / sizeof(name[0]))



namespace msstyle
{
	
	// ******* LAYOUT OF A .msstyle FILE *******
	// A .msstyle file is a Windows PE binary, it just doesn't contain code (or does it?, i actually never checked that)
	// In PE binaries, resources can be embedded, usually used to store images or string tables, but its not limited to
	// that. It can be arbitrary data.

	/*
	+-----------------+--------------------+---------------------------------------------------------------------------+
	| Resource Name   | Meaning            | Description                                                               |
	+-----------------+--------------------+---------------------------------------------------------------------------+
	| AMAP            | Animation Map      | Animation info for the STREAM resources?                                  |
	| BCMAP           | Base Class Map     | Info about inheritance                                                    |
	| CMAP            | Class Map          | Contains all class names in sequential order as UTF16                     |
	| VARIANT         |                    | Contains all properties as binary, that can be assocaiated with classes   |
	| PACKTHEM_VESION | Theme Version	   | The version number of the theme layout. WinXP: 03 00, newer: 04 00		   |
	| RMAP            | Root Map           | Global properties                                                         |
	| VMAP            | Variant Map        | Contains the list of variants. Usually there is only "Normal"             |
	| IMAGE           |                    | Image resources                                                           |
	| STREAM          |                    | Image resource for animations                                             |
	+-----------------+--------------------+---------------------------------------------------------------------------+
	*/

	// ******* Parsing of the resources*******
	// CMAP:	The names are split up and indexed. The order has to be the same as in the resource,
	//			otherwise the IDs in the property data (VARIANT) dont match.
	//
	// VARIANT:	Props are usually 40 bytes long, but not always. Traversing the data in 8 byte steps
	//			and checking the blocks for validity seems to find every property
	//


	// This is the structure that holds and interprets the data of a property.
	typedef struct
	{
		int32_t nameID;		// Offset: 0, Size: 4,	ID for the property name, described in MSDN
		int32_t typeID;		// Offset: 4, Size: 4,	ID for the type of the property, described in MSDN
		int32_t classID;	// Offset: 8, Size: 4,	Index to the class from CMAP the propery belongs to
		int32_t partID;		// Offset: 12, Size: 4	ID for the part of the class the property belongs to, see vsstyle.h
		int32_t stateID;	// Offset: 16, Size: 4	ID for the state map, see see vsstyle.h
		union				// Offset: 20, Size: 20
		{
			struct
			{
				int32_t imageID;	// for MAKEINTRESOURCE()
				char reserved[16];
			}imagetype;
			struct
			{
				int32_t fontID;		// for MAKEINTRESOURCE() too?
				char reserved[16];
			}fonttype;
			struct
			{
				char reserved[12];
				int32_t value;
				char reserved2[4];
			}inttype;
			struct
			{
				char reserved[12];
				int32_t size;
				char reserved2[4];
			}sizetype;
			struct
			{
				char reserved[12];
				int32_t boolvalue;
				char reserved2[4];
			}booltype;
			struct
			{
				char reserved[12];
				unsigned char r;
				unsigned char g;
				unsigned char b;
				char reserved2[5];
			}colortype;
			struct
			{
				char reserved[12];
				int32_t left;
				int32_t top;
				int32_t right;
				int32_t bottom;
			}recttype;
			struct
			{
				char reserved[12];
				int32_t left;
				int32_t top;
				int32_t right;
				int32_t bottom;
			}margintype;
			struct
			{
				char reserved[12];
				int32_t enumvalue;
				char reserved2[4];
			}enumtype;
			struct
			{
				char reserved[12];
				int32_t x;
				int32_t y;
			}positiontype;
			struct
			{
				char reserved[12];
				int32_t firstint;
			}intlist;
			struct
			{
				char reserved[12];
				wchar_t firstchar;
			}texttype;
		}variants;
	} MsStyleProperty;
	
	typedef struct
	{
		int32_t stateID;
		std::string stateName;
		std::vector<MsStyleProperty*> properties;
	} MsStyleState;

	typedef struct
	{
		int32_t partID;
		std::string partName;
		std::unordered_map<int32_t, MsStyleState*> states;
	} MsStylePart;

	typedef struct
	{
		int32_t classID;
		std::string className;
		std::unordered_map<int32_t, MsStylePart*> parts;
	} MsStyleClass;

	typedef struct
	{
		const void* data;
		int size;
	} EmbRessource;

	// Forward declaration of the internal data struct
	struct MsStyleData;
	struct PartMap;


	class VisualStyle
	{
	public:
		VisualStyle();
		~VisualStyle();

		void Load(const wchar_t* path);
		void Save(const wchar_t* path);

		const wchar_t* GetFileName() const;

		// Returns the class list and its children
		const std::unordered_map<int32_t, MsStyleClass*>* GetClasses() const;
		int GetPropertyCount() const;
		const void* GetPropertyBaseAddress() const;

		// Returns the specified resource (read-only)
		EmbRessource GetResource(const char* resName, const char* resType) const;

		// Tries to lookup the name to the given property id
		static const char* FindPropName(int propertyID);
		static bool IsPropertyValid(const MsStyleProperty&  prop);

		// Update routines for all supported properties
		void UpdateImageProperty(const MsStyleProperty* prop, int imageID);
		void UpdateIntegerProperty(const MsStyleProperty* prop, int intVal);
		void UpdateSizeProperty(const MsStyleProperty* prop, int size);
		void UpdateEnumProperty(const MsStyleProperty* prop, int enumVal);
		void UpdateBooleanProperty(const MsStyleProperty* prop, bool bollVal);
		void UpdateColorProperty(const MsStyleProperty* prop, int r, int g, int b);
		void UpdateRectangleProperty(const MsStyleProperty* prop, int left, int top, int right, int bottom);
		void UpdateMarginProperty(const MsStyleProperty* prop, int left, int top, int right, int bottom);
		void UpdatePositionProperty(const MsStyleProperty* prop, int x, int y);
		void UpdateFontProperty(const MsStyleProperty* prop, int fontID);
		void UpdateImageResource(const MsStyleProperty* prop, const wchar_t* filePath);

		const wchar_t* IsReplacementImageQueued(const MsStyleProperty* prop) const;
	private:
		int propsFound;

		void LoadClassMap(std::vector<wchar_t*>& outClassNames);
		void LoadProperties(const std::vector<wchar_t*>& classNames);

		MsStyleData* styleData;

		static const PartMap FindPartMap(const char* className);

		std::unordered_map<const MsStyleProperty*, const wchar_t*> imageReplaceQueue;
	};
}
