#include "VisualStyle.h"
#include "StringConvert.h"

#include "VisualStyleEnums.h"
#include "VisualStyleParts.h"
#include "VisualStyleProps.h"
#include "VisualStyleStates.h"

using namespace libmsstyle;

#define MSSTYLE_ARRAY_LENGTH(name) (sizeof(name) / sizeof(name[0]))

namespace libmsstyle
{
	class VisualStyle::Impl
	{
	public:
		Impl()
		{

		}

		StyleClass* GetClass(int index)
		{
			return &(m_classes.at(index));
		}

		int GetClassCount()
		{
			return m_classes.size();
		}

		void Load(const std::string& path)
		{
			m_stylePath = path;
			m_moduleHandle = OpenModule(path);
			if (m_moduleHandle != 0)
			{
				Resource cmap = GetResource(m_moduleHandle, L"CMAP", L"CMAP");
				LoadClassmap(cmap);

				m_stylePlatform = DeterminePlatform();

				Resource pmap = GetResource(m_moduleHandle, L"NORMAL", L"VARIANT");
				LoadProperties(pmap);
			}
			else throw std::runtime_error("Couldn't open file as PE resource!");
		}

		void Save(const std::string& path)
		{

		}

		Platform GetCompatiblePlatform() const
		{
			return m_stylePlatform;
		}

		std::string GetPath() const
		{
			return m_stylePath;
		}

		static EnumMap* GetEnumMapFromNameID(int32_t nameID, int32_t* out_size)
		{
			EnumMap* out_map = nullptr;
			if (nameID == IDENTIFIER::BGTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_BGTYPE;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_BGTYPE);
			}
			else if (nameID == IDENTIFIER::BORDERTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_BORDERTYPE;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_BORDERTYPE);
			}
			else if (nameID == IDENTIFIER::FILLTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_FILLTYPE;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_FILLTYPE);
			}
			else if (nameID == IDENTIFIER::SIZINGTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_SIZINGTYPE;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_SIZINGTYPE);
			}
			else if (nameID == IDENTIFIER::HALIGN)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_ALIGNMENT_H;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_ALIGNMENT_H);
			}
			else if (nameID == IDENTIFIER::CONTENTALIGNMENT)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_ALIGNMENT_H; // same as the real contentalignment..
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_ALIGNMENT_H);
			}
			else if (nameID == IDENTIFIER::VALIGN)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_ALIGNMENT_V;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_ALIGNMENT_V);
			}
			else if (nameID == IDENTIFIER::OFFSETTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_OFFSET;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_OFFSET);
			}
			else if (nameID == IDENTIFIER::IMAGELAYOUT)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_IMAGELAYOUT;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_IMAGELAYOUT);
			}
			else if (nameID == IDENTIFIER::ICONEFFECT)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_ICONEFFECT;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_ICONEFFECT);
			}
			else if (nameID == IDENTIFIER::GLYPHTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_GLYPHTYPE;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_GLYPHTYPE);
			}
			else if (nameID == IDENTIFIER::IMAGESELECTTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_IMAGESELECT;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_IMAGESELECT);
			}
			else if (nameID == IDENTIFIER::GLYPHFONTSIZINGTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_GLYPHFONTSCALING;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_GLYPHFONTSCALING);
			}
			else if (nameID == IDENTIFIER::TRUESIZESCALINGTYPE)
			{
				out_map = (EnumMap*)&libmsstyle::ENUM_TRUESIZESCALING;
				*out_size = MSSTYLE_ARRAY_LENGTH(libmsstyle::ENUM_TRUESIZESCALING);
			}
			else
			{
				*out_size = 0;
			}

			return out_map;
		}

		void LoadClassmap(Resource classResource)
		{
			int first = 0;
			int last = 1;
			int numFound = 0;

			const wchar_t* data = static_cast<const wchar_t*>(classResource.data);
			int numChars = classResource.size / 2;

			for (int i = 0; i < numChars; ++i, ++last)
			{
				if (data[i] == 0)
				{
					// we found the terminator and
					// have a non-empty string
					if (last - first > 1)
					{
						StyleClass cls;
						cls.classID = numFound;
						cls.className = WideToUTF8(data + first);
						m_classes[numFound] = cls;
						numFound++;
					}

					first = last;
				}
			}
		}

		void LoadProperties(Resource propResource)
		{
			StyleProperty* prevprop;
			StyleProperty* tmpProp;
			const char* dataPtr = static_cast<const char*>(propResource.data);

			while ((dataPtr - propResource.data) < propResource.size)
			{
				tmpProp = (StyleProperty*)dataPtr;
				if (tmpProp->IsPropertyValid())
				{
					// See if we have created a "Style Class" object already for
					// this classID that we can use. If not, create one.
					StyleClass* cls;
					const auto& result = m_classes.find(tmpProp->classID);
					if (result == m_classes.end())
					{
						printf("No class with id: %d\r\n", tmpProp->classID);
					}
					else cls = &(result->second);


					// See if we have created a "Style Part" object for this
					// partID inside the current "Style Class". If not, create one.
					const PartMap partInfo = FindParts(cls->className.c_str(), m_stylePlatform);
					StylePart* part = cls->FindPart(tmpProp->partID);
					if (part == nullptr)
					{
						StylePart newPart;
						newPart.partID = tmpProp->partID;

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

						part = cls->AddPart(newPart);
					}


					// See if we have created a "Style State" object for this
					// stateID inside the current "Style Part". If not, create one.
					StyleState* state = part->FindState(tmpProp->stateID);
					if (state == nullptr)
					{
						StyleState newState;
						newState.stateID = tmpProp->stateID;

						if (tmpProp->partID < partInfo.numParts &&
							tmpProp->stateID < partInfo.parts[tmpProp->partID].numStates)
						{
							newState.stateName = partInfo.parts[tmpProp->partID].states[tmpProp->stateID].stateName;
						}
						else
						{
							if (tmpProp->stateID == 0)
							{
								newState.stateName = "Common";
							}
							else
							{
								char txt[16];
								sprintf(txt, "State %d", tmpProp->stateID);
								state->stateName = std::string(txt);
							}
						}

						state = part->AddState(newState);
					}


					// problem: i saved just ptrs before. now i need real data!!
					state->AddProperty(tmpProp);
					prevprop = tmpProp;
					// the sizes are known, so jump right to the next prop
					dataPtr += tmpProp->GetPropertySize();
				}
				else
				{
					// Look one integer back, just in case.
					// Main focus is looking forward tho..
					StyleProperty* findback = (StyleProperty*)(dataPtr - 4);
					if (findback->IsPropertyValid())
					{
						dataPtr -= 4;
					}
					else
					{
						StyleProperty* prop;

						do
						{
							dataPtr += 4;
							prop = (StyleProperty*)(dataPtr);
						} while (!prop->IsPropertyValid() && (dataPtr - (const char*)propResource.data < propResource.size));
					}
				}
			}
		}

		Platform DeterminePlatform()
		{
			// dirty style-platform check
			bool foundDWMTouch = false;
			bool foundDWMPen = false;
			bool foundW8Taskband = false;
			for (auto& cls : m_classes)
			{
				if (!cls.second.className.compare("DWMTouch"))
					foundDWMTouch = true;
				if (!cls.second.className.compare("DWMPen"))
					foundDWMPen = true;
				if (!cls.second.className.compare("W8::TaskbandExtendedUI"))
					foundW8Taskband = true;
			}

			if (foundW8Taskband)
				return Platform::WIN81;
			else if (foundDWMTouch || foundDWMPen)
				return Platform::WIN10;
			else return Platform::WIN7;
		}

		const PartMap FindParts(const char* className, Platform platform)
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

		ModuleHandle m_moduleHandle;
		Platform m_stylePlatform;

		std::string m_stylePath;
		std::unordered_map<int32_t, StyleClass> m_classes;
	};


	typedef struct
	{
		int32_t numParts;
		const NameMap* parts;
	}PartMap;


	//
	// Impl. Wrapper Class
	//

	VisualStyle::VisualStyle()
		: impl(new Impl())
	{
	}

	VisualStyle::~VisualStyle()
	{
		if (impl)
		{
			delete impl;
		}
	}

	int VisualStyle::GetClassCount()
	{
		return impl->GetClassCount();
	}

	StyleClass* VisualStyle::GetClass(int index)
	{
		return impl->GetClass(index);
	}

	Platform VisualStyle::GetCompatiblePlatform() const
	{
		return impl->GetCompatiblePlatform();
	}

	std::string VisualStyle::GetPath() const
	{
		return impl->GetPath();
	}

	void VisualStyle::Load(const std::string& path)
	{
		impl->Load(path);
	}

	void VisualStyle::Save(const std::string& path)
	{
		impl->Save(path);
	}
}