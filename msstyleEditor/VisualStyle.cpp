#include "VisualStyle.h"
#include <Windows.h>
#include <exception>
#include <fstream>
#include <codecvt>
#include <assert.h>

#include "VisualStyleEnumMaps.h"
#include "VisualStylePartMaps.h"

#define MSSTYLE_ARRAY_LENGTH(name) (sizeof(name) / sizeof(name[0]))

namespace msstyle
{
	// used internally to keep track of the style
	typedef struct MsStyleData
	{
		wchar_t* filePath;
		HMODULE moduleHandle;
		EmbRessource amapResource;
		EmbRessource classmapResource;
		EmbRessource propertyResource;
		EmbRessource stringTableResource;
		std::unordered_map<int32_t, MsStyleClass*> classes;
	} MsStyleData;


	typedef struct PartMap
	{
		int32_t numParts;
		const NameMap* parts;
	} PartMap;


	#pragma region String Helper

	void SplitStringW(wchar_t* data, int size, std::vector<wchar_t*>& splittedString)
	{
		int first = 0;
		int last = 1;
		
		for (int i = 0; i < size; ++i, ++last)
		{
			if (data[i] == NULL)
			{
				int entryLen = last - first;
				if (entryLen > 1)
				{
					splittedString.push_back(data + first);
				}

				first = last;
			}
		}
	}

	std::string WStringToUTF8(const std::wstring& str)
	{
		std::wstring_convert<std::codecvt_utf8<wchar_t>> myconv;
		return myconv.to_bytes(str);
	}

	#pragma endregion

	VisualStyle::VisualStyle()
	{
		
	}


	VisualStyle::~VisualStyle()
	{
		if (styleData == nullptr)
			return;

		delete[] styleData->filePath;
		delete[] styleData->propertyResource.data; // delete all properties

		// delete all classes
		for (auto it = styleData->classes.begin(); it != styleData->classes.end(); ++it)
		{
			// delete all parts
			for (auto partIt = it->second->parts.begin(); partIt != it->second->parts.end(); ++partIt)
			{
				// delete all states
				for (auto stateIt = partIt->second->states.begin(); stateIt != partIt->second->states.end(); ++stateIt)
				{
					delete stateIt->second;
				}

				delete partIt->second;
			}

			//delete[] it->second->className;
			delete it->second;
		}

		for (auto qIt = imageReplaceQueue.begin(); qIt != imageReplaceQueue.end(); ++qIt)
		{
			delete qIt->second;
		}

		FreeLibrary(styleData->moduleHandle);

		delete styleData;
	}


	void VisualStyle::Load(const wchar_t* path)
	{
		styleData = new MsStyleData();
		styleData->moduleHandle = LoadLibraryExW(path, NULL, LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE);
		
		// copy it, otherwise we can't guarantee ptr stays valid
		int strSize = lstrlenW(path) + 1;
		styleData->filePath = new wchar_t[strSize];
		lstrcpyW(styleData->filePath, path);

		std::vector<wchar_t*> classNames;
		LoadClassMap(classNames);			// extract the list of class names
		LoadProperties(classNames);			// load all probperties and build the style tree
	}


	/**
	* The save routine is very alpha at the moment. I am just changing the properties
	* that were specified and patch the file. Building the style from ground up is not
	* possible for me, since i dont fully understand the structure and layout of .msstyle files yet.
	*/
	void VisualStyle::Save(const wchar_t* path)
	{
		// if source != destination
		if (lstrcmpW(path, styleData->filePath))
		{
			// copy the source file and modify the new one
			// since we cant create a file from scratch
			std::ifstream src(styleData->filePath, std::ios::binary);
			std::ofstream dst(path, std::ios::binary);
			dst << src.rdbuf();
		}

		HANDLE updHandle = BeginUpdateResourceW(path, false);
		if (updHandle == NULL)
		{
			throw std::runtime_error("Could not open the file for writing!");
		}

		for (auto& img : imageReplaceQueue)
		{
			// load new image data
			std::ifstream newImg(img.second, std::ios::binary);
			newImg.seekg(0, std::ios::end);
			std::streampos size = newImg.tellg();
			newImg.seekg(0, std::ios::beg);

			if (size > MAXDWORD)
				throw std::runtime_error("Replacement file is to big!");

			char* imgBuffer = new char[(DWORD)size];
			newImg.read(imgBuffer, size);
			newImg.close();

			const wchar_t* resName;
			if (img.first->typeID == IDENTIFIER::FILENAME)
				resName = L"IMAGE";
			else if (img.first->typeID == IDENTIFIER::DISKSTREAM)
				resName = L"STREAM";
			else continue;

			if (!UpdateResourceW(updHandle, resName, MAKEINTRESOURCEW(img.first->variants.imagetype.imageID), MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), imgBuffer, (DWORD)size))
			{
				throw std::runtime_error("Could not update IMAGE/STREAM resource!");
				delete[] imgBuffer;
				return;
			}

			delete[] imgBuffer;
		}

		// replace properties
		if (!UpdateResourceW(updHandle, L"VARIANT", L"NORMAL", 0, (LPVOID*)styleData->propertyResource.data, styleData->propertyResource.size))
		{
			throw std::runtime_error("Could not update properties!");
			return;
		}

		if (!EndUpdateResourceW(updHandle, false))
		{
			throw std::runtime_error("Could not write the changes to the file!");
			return;
		}
	}


	/**
	* Loads the classmap (CMAP) resource and builds the class list.
	*/
	void VisualStyle::LoadClassMap(std::vector<wchar_t*>& outClassNames)
	{
		styleData->classmapResource = GetResource("CMAP", "CMAP");

		wchar_t* data = (wchar_t*)styleData->classmapResource.data;
		int count = styleData->classmapResource.size / 2;			// "size" amount of chars, but "size / 2" wchars

		SplitStringW(data, count, outClassNames);
	}


	/**
	* The loading works as follows: Open the VARIANT/NORMAL resource.
	* In 8 byte steps, try to interpret the data as "MsStyleProperty".
	* If successful, create objects from the IDs that are located in the prop,
	* and build a tree structure like this:
	* MsStyleClass > MsStylePart > MsStyleState > MsStyleProperty
	* Lookup the names where necessary
	*/
	void VisualStyle::LoadProperties(const std::vector<wchar_t*>& classNames)
	{
		EmbRessource variantRes = GetResource("NORMAL", "VARIANT");

		// Copy, to make the memory writeable. Needed so i can edit it later on.
		char* tmpMem = new char[variantRes.size];
		memcpy(tmpMem, variantRes.data, variantRes.size);

		// Store it
		styleData->propertyResource.data = tmpMem;
		styleData->propertyResource.size = variantRes.size;

		int found = 0;
		MsStyleProperty* tmpProp;
		char* limit = tmpMem + variantRes.size;
		for (char* variantData = tmpMem; variantData < limit-4; variantData += 4)
		{
			// Check whether we found a valid property at this address
			tmpProp = (MsStyleProperty*)variantData;
			if (tmpProp->nameID != 0 && 
				tmpProp->typeID >= 200 && 
				tmpProp->typeID <= 217 && 
				tmpProp->classID <= 8002)
			{
				// Check whether the ID corresponds to a known class
				MsStyleClass* cls;
				const auto& result = styleData->classes.find(tmpProp->classID);
				if (result == styleData->classes.end())
				{
					// Check whether the class exists; if not, create it and add it
					const auto& result2 = msstyle::PROPERTY_MAP.find(tmpProp->nameID);
					if (result2 != msstyle::PROPERTY_MAP.end())
					{
						cls = new MsStyleClass();
						cls->classID = tmpProp->classID;
						cls->className = WStringToUTF8(classNames[tmpProp->classID]);
						styleData->classes[tmpProp->classID] = cls;
					}
					else continue; // next prop
				}
				else cls = result->second;


				const PartMap partInfo = FindPartMap(cls->className.c_str());

				// Lookup the part ID
				MsStylePart* part;
				const auto& partRes = cls->parts.find(tmpProp->partID);
				if (partRes == cls->parts.end())
				{
					part = new MsStylePart();
					part->partID = tmpProp->partID;

					if (tmpProp->partID < partInfo.numParts)
						part->partName = partInfo.parts[tmpProp->partID].partName;
					else
					{
						char txt[16];
						sprintf(txt, "Part %d", tmpProp->partID);
						part->partName = std::string(txt);
					}

					cls->parts[tmpProp->partID] = part;
				}
				else part = partRes->second;


				// Lookup the state ID
				MsStyleState* state;
				const auto& stateRes = cls->parts[tmpProp->partID]->states.find(tmpProp->stateID);
				if (stateRes == cls->parts[tmpProp->partID]->states.end())
				{
					state = new MsStyleState();
					state->stateID = tmpProp->stateID;

					if (tmpProp->partID < partInfo.numParts &&
						tmpProp->stateID < partInfo.parts[tmpProp->partID].numStates)
					{
						state->stateName = partInfo.parts[tmpProp->partID].states[tmpProp->stateID].stateName;
					}
					else
					{
						if (tmpProp->stateID == 0)
							state->stateName = "State Independent";
						else
						{
							char txt[16];
							sprintf(txt, "State %d", tmpProp->stateID);
							state->stateName = std::string(txt);
						}
					}

					part->states[tmpProp->stateID] = state;
				}
				else state = stateRes->second;


				// Add the prop to the state
				state->properties.push_back(tmpProp);
				found++;
			}
		}
	}


	const std::unordered_map<int32_t, MsStyleClass*>* VisualStyle::GetClasses() const
	{
		return &styleData->classes;
	}

	EmbRessource VisualStyle::GetResource(const char* resName, const char* resType) const
	{
		EmbRessource res;
		HRSRC resHandle = FindResourceA(styleData->moduleHandle, resName, resType);
		HGLOBAL dataHandle = LoadResource(styleData->moduleHandle, resHandle);
		res.data = LockResource(dataHandle);
		res.size = SizeofResource(styleData->moduleHandle, resHandle);
		return res;
	}


	const char* VisualStyle::FindPropName(int propertyID)
	{
		auto& ret = msstyle::PROPERTY_MAP.find(propertyID);
		if (ret != msstyle::PROPERTY_MAP.end())
			return ret->second;
		else return "UNKNOWN";
	}

	const PartMap VisualStyle::FindPartMap(const char* className)
	{
		PartMap m;

		if (strstr(className, "Toolbar"))	// Toolbar is often inherited, so find it first. It also has to be matched before "Button", 
		{									// because otherwise the SearchButton::Toolbar class would use the Button parts instead of the toolbar ones.
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TOOLBAR);
			m.parts = PARTS_TOOLBAR;
		}
		else if (strstr(className, "Button"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_BUTTON);
			m.parts = PARTS_BUTTON;
		}
		else if (strstr(className, "Edit"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_EDIT);
			m.parts = PARTS_EDIT;
		}
		else if (strstr(className, "Rebar"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_REBAR);
			m.parts = PARTS_REBAR;
		}
		else if (strstr(className, "Combobox"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_COMBOBOX);
			m.parts = PARTS_COMBOBOX;
		}
		else if (strstr(className, "ControlPanel"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_CONTROLPANEL);
			m.parts = PARTS_CONTROLPANEL;
		}
		else if (strstr(className, "ExplorerBar"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_EXPLORERBAR);
			m.parts = PARTS_EXPLORERBAR;
		}
		else if (strstr(className, "Listbox"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_LISTBOX);
			m.parts = PARTS_LISTBOX;
		}
		else if (strstr(className, "ListView"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_LISTVIEW);
			m.parts = PARTS_LISTVIEW;
		}
		else if (strstr(className, "Menu"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_MENU);
			m.parts = PARTS_MENU;
		}
		else if (strstr(className, "TreeView"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TREEVIEW);
			m.parts = PARTS_TREEVIEW;
		}
		else if (strstr(className, "Window"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_WINDOW);
			m.parts = PARTS_WINDOW;
		}
		else if (strstr(className, "TaskDialog"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKDIALOG);
			m.parts = PARTS_TASKDIALOG;
		}
		else if (strstr(className, "Header"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_HEADER);
			m.parts = PARTS_HEADER;
		}
		else if (strstr(className, "AeroWizard"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_AEROWIZARD);
			m.parts = PARTS_AEROWIZARD;
		}
		else if (strstr(className, "Progress"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_PROGRESS);
			m.parts = PARTS_PROGRESS;
		}
		else if (strstr(className, "TrackBar"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TRACKBAR);
			m.parts = PARTS_TRACKBAR;
		}
		else if (strstr(className, "Tab"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TAB);
			m.parts = PARTS_TAB;
		}
		else if (strstr(className, "ToolTip") || strstr(className, "Tooltip")) // overcome inconsitencies in the naming
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TOOLTIP);
			m.parts = PARTS_TOOLTIP;
		}
		else if (strstr(className, "TaskBar"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBAR);
			m.parts = PARTS_TASKBAR;
		}
		else if (strstr(className, "TextStyle"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TEXTSTYLE);
			m.parts = PARTS_TEXTSTYLE;
		}
		else if (strstr(className, "Spin"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_SPIN);
			m.parts = PARTS_SPIN;
		}
		else if (strstr(className, "ScrollBar") || strstr(className, "Scrollbar")) // overcome inconsitencies in the naming
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_SCROLLBAR);
			m.parts = PARTS_SCROLLBAR;
		}
		else if (strstr(className, "Flyout"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_FLYOUT);
			m.parts = PARTS_FLYOUT;
		}
		else if (strstr(className, "DragDrop"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DRAGDROP);
			m.parts = PARTS_DRAGDROP;
		}
		else if (strstr(className, "DatePicker"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DATEPICKER);
			m.parts = PARTS_DATEPICKER;
		}
		else if (strstr(className, "StartPanel"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_STARTPANEL);
			m.parts = PARTS_STARTPANEL;
		}
		else if (strstr(className, "TaskBand"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBAND);
			m.parts = PARTS_TASKBAND;
		}
		else
		{
			m.numParts = 0;
			m.parts = NULL;
		}

		return m;
	}

	
	void VisualStyle::UpdateImageProperty(const MsStyleProperty* prop, int imageID)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.imagetype.imageID = imageID;
	}

	void VisualStyle::UpdateIntegerProperty(const MsStyleProperty* prop, int intVal)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.inttype.value = intVal;
	}

	void VisualStyle::UpdateSizeProperty(const MsStyleProperty* prop, int size)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.sizetype.size = size;
	}

	void VisualStyle::UpdateEnumProperty(const MsStyleProperty* prop, int enumVal)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.enumtype.enumvalue = enumVal;
	}

	void VisualStyle::UpdateBooleanProperty(const MsStyleProperty* prop, bool boolVal)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.booltype.boolvalue = boolVal;
	}

	void VisualStyle::UpdateColorProperty(const MsStyleProperty* prop, int r, int g, int b)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.colortype.r = r;
		((MsStyleProperty*)prop)->variants.colortype.g = g;
		((MsStyleProperty*)prop)->variants.colortype.b = b;
	}

	void VisualStyle::UpdateRectangleProperty(const MsStyleProperty* prop, int left, int top, int right, int bottom)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.recttype.left = left;
		((MsStyleProperty*)prop)->variants.recttype.top = top;
		((MsStyleProperty*)prop)->variants.recttype.right = right;
		((MsStyleProperty*)prop)->variants.recttype.bottom = bottom;
	}

	void VisualStyle::UpdateMarginProperty(const MsStyleProperty* prop, int left, int top, int right, int bottom)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.margintype.left = left;
		((MsStyleProperty*)prop)->variants.margintype.top = top;
		((MsStyleProperty*)prop)->variants.margintype.right = right;
		((MsStyleProperty*)prop)->variants.margintype.bottom = bottom;
	}

	void VisualStyle::UpdatePositionProperty(const MsStyleProperty* prop, int x, int y)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.positiontype.x = x;
		((MsStyleProperty*)prop)->variants.positiontype.y = y;
	}

	void VisualStyle::UpdateFontProperty(const MsStyleProperty* prop, int fontID)
	{
		assert(prop != nullptr);
		((MsStyleProperty*)prop)->variants.fonttype.fontID = fontID;
	}

	void VisualStyle::UpdateImageResource(const MsStyleProperty* prop, const wchar_t* newFilePath)
	{
		assert(prop != nullptr);

		int len = lstrlenW(newFilePath);
		wchar_t* newStr = new wchar_t[len + 1];
		lstrcpyW(newStr, newFilePath);

		imageReplaceQueue[prop] = newStr;
	}

	const wchar_t* VisualStyle::IsReplacementImageQueued(const MsStyleProperty* prop) const
	{
		auto& res = imageReplaceQueue.find(prop);
		if (res != imageReplaceQueue.end())
		{
			return res->second;
		}
		else return NULL;
	}

	const wchar_t* VisualStyle::GetFileName() const
	{
		return styleData->filePath;
	}
}