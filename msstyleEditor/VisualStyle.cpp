#include "VisualStyle.h"
#include <Windows.h>
#include <exception>
#include <fstream>
#include <codecvt>
#include <assert.h>
#include <locale> //wstring_convert

#include "VisualStyleEnums.h"
#include "VisualStyleParts.h"

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
			if (data[i] == 0)
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
		: propsFound(0)
	{
		
	}


	VisualStyle::~VisualStyle()
	{
		if (styleData == nullptr)
			return;

		delete[] styleData->filePath;
		delete[] (const char*)styleData->propertyResource.data; // delete all properties

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
		LoadPropertiesEx(classNames);			// load all probperties and build the style tree
	}


	void VisualStyle::SaveImages(HANDLE resHandle)
	{
		for (auto& img : imageReplaceQueue)
		{
			// load new image data
			std::string doNotRelyOnCompilerExtensions = WStringToUTF8(img.second);
			std::ifstream newImg(doNotRelyOnCompilerExtensions, std::ios::binary);
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

			if (!UpdateResourceW(resHandle, resName, MAKEINTRESOURCEW(img.first->variants.imagetype.imageID), MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), imgBuffer, (DWORD)size))
			{
				throw std::runtime_error("Could not update IMAGE/STREAM resource!");
				delete[] imgBuffer;
				return;
			}

			delete[] imgBuffer;
		}
	}

	void VisualStyle::SaveClasses(HANDLE resHandle)
	{

	}

	void VisualStyle::SaveProperties(HANDLE resHandle)
	{
		// assume that twice the size common properties require is enough
		int estimatedSize = GetPropertyCount() * 48 * 2;
		char* data = new char[estimatedSize];
		char* dataptr = data;

		// for all classes
		for (auto it = styleData->classes.begin(); it != styleData->classes.end(); ++it)
		{
			// for all parts
			for (auto partIt = it->second->parts.begin(); partIt != it->second->parts.end(); ++partIt)
			{
				// for all states
				for (auto stateIt = partIt->second->states.begin(); stateIt != partIt->second->states.end(); ++stateIt)
				{
					// for all properties
					for (auto propIt = stateIt->second->properties.begin(); propIt != stateIt->second->properties.end(); ++propIt)
					{
						int propSize = GetPropertySize(*(*propIt));
						memcpy(dataptr, &((*propIt)->nameID), propSize);
						dataptr += propSize;

						if (dataptr - data > estimatedSize)
							throw std::runtime_error("I haven't allocated enough memory to save the file..sorry for that!");
					}
				}
			}

			// we would save the classnames in the CMAP resource
			// but as long as we dont modify the classID of the
			// properties, this shouldn't be necessary
		}

		if (!UpdateResourceW(resHandle, L"VARIANT", L"NORMAL", 0, (LPVOID*)data, dataptr-data))
		{
			throw std::runtime_error("Could not update properties!");
			return;
		}
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
			std::string originalFile = WStringToUTF8(styleData->filePath);
			std::ifstream src(originalFile, std::ios::binary);

			std::string newFile = WStringToUTF8(path);
			std::ofstream dst(newFile, std::ios::binary);
			dst << src.rdbuf();
		}

		HANDLE updHandle = BeginUpdateResourceW(path, false);
		if (updHandle == NULL)
		{
			throw std::runtime_error("Could not open the file for writing!");
		}

		SaveImages(updHandle);
		SaveProperties(updHandle);

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

		// dirty style-platform check
		bool foundDWMTouch = false;
		bool foundDWMPen = false;
		bool foundW8Taskband = false;
		for (auto& clsName : outClassNames)
		{
			if (!lstrcmpW(clsName, L"DWMTouch"))
				foundDWMTouch = true;
			if (!lstrcmpW(clsName, L"DWMPen"))
				foundDWMPen = true;
			if (!lstrcmpW(clsName, L"W8::TaskbandExtendedUI"))
				foundW8Taskband = true;
		}

		if (foundW8Taskband)
			stylePlatform = Platform::WIN81;
		else if (foundDWMTouch || foundDWMPen)
			stylePlatform = Platform::WIN10;
		else stylePlatform = Platform::WIN7;
	}


	/**
	* The loading works as follows: Open the VARIANT/NORMAL resource.
	* In 4 byte steps, try to interpret the data as "MsStyleProperty".
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

		MsStyleProperty* tmpProp;
		char* limit = tmpMem + variantRes.size;
		for (char* variantData = tmpMem; variantData < limit-4; variantData += 4)
		{
			// Check whether we likely found a valid property at this address
			// This is not an exhaustive validation and may yield wrong results
			tmpProp = (MsStyleProperty*)variantData;
			if (IsPropertyValid(*tmpProp))
			{
				// Further check whether classID is valid
				if (tmpProp->classID >= classNames.size())
					continue;

				// Check whether the properties classID corresponds to a known class already
				MsStyleClass* cls;
				const auto& result = styleData->classes.find(tmpProp->classID);
				if (result == styleData->classes.end())
				{
					// Check the properties nameID too before adding the class
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


				const PartMap partInfo = FindPartMap(cls->className.c_str(), stylePlatform);

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
							state->stateName = "Common";
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
				propsFound++;

			} // if(IsPropertyValid(...))
		}
	}

	void VisualStyle::LoadPropertiesEx(const std::vector<wchar_t*>& classNames)
	{
		EmbRessource variantRes = GetResource("NORMAL", "VARIANT");

		// Copy, to make the memory writeable. Needed so i can edit it later on.
		char* tmpMem = new char[variantRes.size];
		memcpy(tmpMem, variantRes.data, variantRes.size);

		// Store it
		styleData->propertyResource.data = tmpMem;
		styleData->propertyResource.size = variantRes.size;

		MsStyleProperty* prevprop;
		MsStyleProperty* tmpProp;
		char* dataPtr = tmpMem;

		while ((dataPtr - tmpMem) < variantRes.size)
		{
			tmpProp = (MsStyleProperty*)dataPtr;
			if (IsPropertyValid(*tmpProp))
			{
				// See if we have created a "Style Class" object already for
				// this classID that we can use. If not, create one.
				MsStyleClass* cls;
				const auto& result = styleData->classes.find(tmpProp->classID);
				if (result == styleData->classes.end())
				{
					cls = new MsStyleClass();
					cls->classID = tmpProp->classID;
					if (tmpProp->classID < classNames.size())
					{
						cls->className = WStringToUTF8(classNames[tmpProp->classID]);
					}
					else
					{
						char txt[16];
						sprintf(txt, "Class %d", tmpProp->classID);
						cls->className = std::string(txt);
					}

					styleData->classes[tmpProp->classID] = cls;
				}
				else cls = result->second;


				// See if we have created a "Style Part" object for this
				// partID inside the current "Style Class". If not, create one.
				MsStylePart* part;
				const PartMap partInfo = FindPartMap(cls->className.c_str(), stylePlatform);
				const auto& partRes = cls->parts.find(tmpProp->partID);
				if (partRes == cls->parts.end())
				{
					part = new MsStylePart();
					part->partID = tmpProp->partID;

					if (tmpProp->partID < partInfo.numParts)
					{
						part->partName = partInfo.parts[tmpProp->partID].partName;
					}
					else
					{
						char txt[16];
						sprintf(txt, "Part %d", tmpProp->partID);
						part->partName = std::string(txt);
					}

					cls->parts[tmpProp->partID] = part;
				}
				else part = partRes->second;



				// See if we have created a "Style State" object for this
				// stateID inside the current "Style Part". If not, create one.
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
						{
							state->stateName = "Common";
						}
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

				// Now that we have the class > part > state 
				// structure for this property, add it.
				state->properties.push_back(tmpProp);
				propsFound++;
				prevprop = tmpProp;
				// the sizes are known, so jump right to the next prop
				dataPtr += GetPropertySize(*tmpProp);
			}
			else
			{
				// Look one integer back, just in case.
				// Main focus is looking forward tho..
				MsStyleProperty* findback = (MsStyleProperty*)(dataPtr - 4);
				if (IsPropertyValid(*findback))
				{
					dataPtr -= 4;
				}
				else
				{
					MsStyleProperty* prop;

					do
					{
						dataPtr += 4;
						prop = (MsStyleProperty*)(dataPtr);
					} while (!IsPropertyValid(*prop) && (dataPtr - tmpMem < variantRes.size));
				}
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

	bool VisualStyle::IsPropertyValid(const MsStyleProperty&  prop)
	{
		// Not a known type
		if (prop.typeID < 200 || prop.typeID >= IDENTIFIER::COLORSCHEMES)
			return false;

		// Some color and font props use an type id as name id.
		// They seem to contain valid data, so ill include them.
		if (prop.nameID == IDENTIFIER::COLOR &&
			prop.typeID == IDENTIFIER::COLOR)
			return true;
		if (prop.nameID == IDENTIFIER::FONT &&
			prop.typeID == IDENTIFIER::FONT)
			return true;
		if (prop.nameID == IDENTIFIER::DISKSTREAM &&
			prop.typeID == IDENTIFIER::DISKSTREAM)
			return true;
		if (prop.nameID == IDENTIFIER::STREAM &&
			prop.typeID == IDENTIFIER::STREAM)
			return true;

		// Not sure where the line for valid name ids is.
		if (prop.nameID < IDENTIFIER::COLORSCHEMES)
			return false;

		// Not a known class
		if (prop.classID > 8002)
			return false;

		return true;
	}


	const char* VisualStyle::FindPropName(int propertyID)
	{
		auto ret = msstyle::PROPERTY_MAP.find(propertyID);
		if (ret != msstyle::PROPERTY_MAP.end())
			return ret->second;
		else return "UNKNOWN";
	}

	const PartMap VisualStyle::FindPartMap(const char* className, Platform platform)
	{
		//
		// TODO: Use base class map (BCMAP) instead of relying on the naming
		//

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
		else if (strstr(className, "DWMWindow"))
		{
			switch (platform)
			{
				case Platform::WIN7:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN7);
					m.parts = PARTS_DWMWINDOW_WIN7;
				} break;
				// TODO: discover and use dedicated partlists
				// for the respective platforms..
				case Platform::WIN8:
				case Platform::WIN81:
				case Platform::WIN10:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN81);
					m.parts = PARTS_DWMWINDOW_WIN81;
				} break;
				default:
				{
					m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_DWMWINDOW_WIN81);
					m.parts = PARTS_DWMWINDOW_WIN81;
				} break;
			}

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
		else if (strstr(className, "TaskBand2"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBAND2);
			m.parts = PARTS_TASKBAND2;
		}
		else if (strstr(className, "TaskBand"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_TASKBAND);
			m.parts = PARTS_TASKBAND;
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
		else if (strstr(className, "StartPanelPriv"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_STARTPANELPRIV);
			m.parts = PARTS_STARTPANELPRIV;
		}
		else if (strstr(className, "StartPanel"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_STARTPANEL);
			m.parts = PARTS_STARTPANEL;
		}
		else if (strstr(className, "MonthCal"))
		{
			m.numParts = MSSTYLE_ARRAY_LENGTH(PARTS_MONTHCAL);
			m.parts = PARTS_MONTHCAL;
		}
		else
		{
			m.numParts = 0;
			m.parts = NULL;
		}

		return m;
	}

	EnumMap* VisualStyle::GetEnumMapFromNameID(int32_t nameID, int32_t* out_size)
	{
		EnumMap* out_map = nullptr;
		if (nameID == IDENTIFIER::BGTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_BGTYPE;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_BGTYPE);
		}
		else if (nameID == IDENTIFIER::BORDERTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_BORDERTYPE;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_BORDERTYPE);
		}
		else if (nameID == IDENTIFIER::FILLTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_FILLTYPE;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_FILLTYPE);
		}
		else if (nameID == IDENTIFIER::SIZINGTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_SIZINGTYPE;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_SIZINGTYPE);
		}
		else if (nameID == IDENTIFIER::HALIGN)
		{
			out_map = (EnumMap*)&msstyle::ENUM_ALIGNMENT_H;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_ALIGNMENT_H);
		}
		else if (nameID == IDENTIFIER::CONTENTALIGNMENT)
		{
			out_map = (EnumMap*)&msstyle::ENUM_ALIGNMENT_H; // same as the real contentalignment..
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_ALIGNMENT_H);
		}
		else if (nameID == IDENTIFIER::VALIGN)
		{
			out_map = (EnumMap*)&msstyle::ENUM_ALIGNMENT_V;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_ALIGNMENT_V);
		}
		else if (nameID == IDENTIFIER::OFFSETTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_OFFSET;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_OFFSET);
		}
		else if (nameID == IDENTIFIER::IMAGELAYOUT)
		{
			out_map = (EnumMap*)&msstyle::ENUM_IMAGELAYOUT;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_IMAGELAYOUT);
		}
		else if (nameID == IDENTIFIER::ICONEFFECT)
		{
			out_map = (EnumMap*)&msstyle::ENUM_ICONEFFECT;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_ICONEFFECT);
		}
		else if (nameID == IDENTIFIER::GLYPHTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_GLYPHTYPE;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_GLYPHTYPE);
		}
		else if (nameID == IDENTIFIER::IMAGESELECTTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_IMAGESELECT;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_IMAGESELECT);
		}
		else if (nameID == IDENTIFIER::GLYPHFONTSIZINGTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_GLYPHFONTSCALING;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_GLYPHFONTSCALING);
		}
		else if (nameID == IDENTIFIER::TRUESIZESCALINGTYPE)
		{
			out_map = (EnumMap*)&msstyle::ENUM_TRUESIZESCALING;
			*out_size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_TRUESIZESCALING);
		}
		else
		{
			*out_size = 0;
		}

		return out_map;
	}

	const char* VisualStyle::GetEnumAsString(int32_t nameID, int32_t enumValue)
	{
		int32_t size = 0;
		msstyle::EnumMap* enums = GetEnumMapFromNameID(nameID, &size);
		
		if (enums != nullptr && size > enumValue)
		{
			return enums[enumValue].value;
		}
		else return nullptr;
	}

	int VisualStyle::GetPropertySize(const MsStyleProperty& prop)
	{
		switch (prop.typeID)
		{
		case IDENTIFIER::FILENAME:
		case IDENTIFIER::DISKSTREAM:
			return 32;
		case IDENTIFIER::FONT:
			return 32;
		case IDENTIFIER::INT:
			return 40;
		case IDENTIFIER::SIZE:
			return 40;
		case IDENTIFIER::BOOL:
			return 40;
		case IDENTIFIER::COLOR:
			return 40;
		case IDENTIFIER::RECT:
			return 48;
		case IDENTIFIER::MARGINS:
			return 48;
		case IDENTIFIER::ENUM:
			return 40;
		case IDENTIFIER::POSITION:
			return 40;
		case IDENTIFIER::INTLIST:
			// header, reserved, numints, intlist, nullterminator
			return 20 + 12 + 4 + prop.variants.intlist.numints * sizeof(int32_t);
		case IDENTIFIER::STRING:
			// string length in bytes including the null terminator
			return 20 + 8 + 4 + prop.variants.texttype.sizeInBytes;
			// return 20 + 8 + 4 + (wcslen(&prop.variants.texttype.firstchar) + 1) * sizeof(wchar_t);
		case 225: // Unknown or wrong prop, since Win7 ?
		case 241: // Unknown or wrong prop, since Win10 ?
			return 40;
		default:
			return 40;
		}
	}

	std::string VisualStyle::GetPropertyValueAsString(const MsStyleProperty& prop)
	{
		char textbuffer[64];
		switch (prop.typeID)
		{
			case IDENTIFIER::ENUM:
			{
				const char* enumStr = GetEnumAsString(prop.nameID, prop.variants.enumtype.enumvalue);
				if (enumStr != nullptr)
					return std::string(enumStr);
				else return std::string("UNKNOWN ENUM");
			} break;
			case IDENTIFIER::STRING:
			{
				return WStringToUTF8(&prop.variants.texttype.firstchar);
			} break;
			case IDENTIFIER::INT:
			{
				return std::to_string(prop.variants.inttype.value);
			} break;
			case IDENTIFIER::BOOL:
			{
				if (prop.variants.booltype.boolvalue > 0)
					return std::string("true");
				else return std::string("false");
			} break;
			case IDENTIFIER::COLOR:
			{
				sprintf(textbuffer, "%d, %d, %d", prop.variants.colortype.r, prop.variants.colortype.g, prop.variants.colortype.b);
				return std::string(textbuffer);
			} break;
			case IDENTIFIER::MARGINS:
			{
				sprintf(textbuffer, "%d, %d, %d, %d", prop.variants.margintype.left, prop.variants.margintype.top, prop.variants.margintype.right, prop.variants.margintype.bottom);
				return std::string(textbuffer);
			} break;
			case IDENTIFIER::FILENAME:
			{
				return std::to_string(prop.variants.imagetype.imageID);
			} break;
			case IDENTIFIER::SIZE:
			{
				return std::to_string(prop.variants.sizetype.size);
			} break;
			case IDENTIFIER::POSITION:
			{
				sprintf(textbuffer, "%d, %d", prop.variants.positiontype.x, prop.variants.positiontype.y);
				return std::string(textbuffer);
			} break;
			case IDENTIFIER::RECT:
			{
				sprintf(textbuffer, "%d, %d, %d, %d", prop.variants.recttype.left, prop.variants.recttype.top, prop.variants.recttype.right, prop.variants.recttype.bottom);
				return std::string(textbuffer);
			} break;
			case IDENTIFIER::FONT:
			{
				// todo: lookup resource id?
				return std::to_string(prop.variants.fonttype.fontID);
			} break;
			case IDENTIFIER::INTLIST:
			{
				if (prop.variants.intlist.numints >= 3)
				{
					sprintf(textbuffer, "Len: %d, Values: %d, %d, %d, ...", prop.variants.intlist.numints
						, *(&prop.variants.intlist.firstint + 0)
						, *(&prop.variants.intlist.firstint + 1)
						, *(&prop.variants.intlist.firstint + 2));
				}
				else sprintf(textbuffer, "Len: %d, Values omitted");
				return std::string(textbuffer);
			} break;
			default:
			{
				return "Unsupported";
			}
		}
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
		auto res = imageReplaceQueue.find(prop);
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

	int VisualStyle::GetPropertyCount() const
	{
		return propsFound;
	}

	const void* VisualStyle::GetPropertyBaseAddress() const
	{
		return styleData->propertyResource.data;
	}

	Platform VisualStyle::GetCompatiblePlatform() const
	{
		return stylePlatform;
	}
}