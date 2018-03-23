#include "VisualStyle.h"
#include "StringConvert.h"

#include "VisualStyleEnums.h"
#include "VisualStyleParts.h"
#include "VisualStyleStates.h"
#include "VisualStyleDefinitions.h"
#include <string.h>
#include <fstream>
using namespace libmsstyle;

#define MSSTYLE_ARRAY_LENGTH(name) (sizeof(name) / sizeof(name[0]))

namespace libmsstyle
{
	struct ResourceHasher
	{
		std::size_t operator()(const StyleResource& r) const
		{
			return ((std::hash<int>()(r.GetNameID())
				  ^ (std::hash<int>()(r.GetType()) << 1)) >> 1)
				  ^ (std::hash<int>()(r.GetSize()) << 1);
		}
	};

	class VisualStyle::Impl
	{
	public:
		Impl()
			: m_propsFound(0)
			, m_moduleHandle(0)
		{
		}

		StyleClass* GetClass(int index)
		{
			return &(m_classes.at(index));
		}

		size_t GetClassCount()
		{
			return m_classes.size();
		}

		void Load(const std::string& path)
		{
			m_stylePath = path;
			m_moduleHandle = OpenModule(path);
			if (m_moduleHandle != 0)
			{
				Resource cmap = GetResource(m_moduleHandle, "CMAP", "CMAP");
				LoadClassmap(cmap);

				m_stylePlatform = DeterminePlatform();

				Resource pmap = GetResource(m_moduleHandle, "NORMAL", "VARIANT");
				LoadProperties(pmap);
			}
			else throw std::runtime_error("Couldn't open file as PE resource!");
		}

		void SaveResources(UpdateHandle updateHandle)
		{
			for (auto& resource : m_resourceUpdates)
			{
				std::ifstream newImg(resource.second, std::ios::binary);
				newImg.seekg(0, std::ios::end);
				std::streampos size = newImg.tellg();
				newImg.seekg(0, std::ios::beg);

				if (size > UINT32_MAX)
					throw std::runtime_error("Replacement file is to big!");

				char* imgBuffer = new char[size];
				newImg.read(imgBuffer, size);
				newImg.close();

				const char* resName;

				switch (resource.first.GetType())
				{
				case StyleResourceType::IMAGE:
					resName = "IMAGE";
					break;
				case StyleResourceType::ATLAS:
					resName = "STREAM";
					break;
				default:
					continue;
				}

				bool success = libmsstyle::UpdateStyleResource(updateHandle, resName,
					resource.first.GetNameID(),
					resource.first.GetData(),
					resource.first.GetSize());
				if (!success)
				{
					throw std::runtime_error("Could not update IMAGE/STREAM resource!");
					delete[] imgBuffer;
					return;
				}

				delete[] imgBuffer;
			}
		}

		void SaveProperties(UpdateHandle updateHandle)
		{
			// assume that twice the size common properties require is enough
			int estimatedSize = m_propsFound * 48 * 2;
			char* data = new char[estimatedSize];
			char* dataptr = data;

			// for all classes
			for (auto it = m_classes.begin(); it != m_classes.end(); ++it)
			{
				// for all parts
				for (auto partIt = it->second.begin(); partIt != it->second.end(); ++partIt)
				{
					// for all states
					for (auto stateIt = partIt->second.begin(); stateIt != partIt->second.end(); ++stateIt)
					{
						// for all properties
						for (auto propIt = stateIt->second.begin(); propIt != stateIt->second.end(); ++propIt)
						{
							int propSize = (*propIt)->GetPropertySize();
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

			LanguageId lid = GetFirstLanguageId(m_moduleHandle, "NORMAL", "VARIANT");
			unsigned int length = static_cast<unsigned int>(dataptr - data);

			if (!UpdateStyleResource(updateHandle, "VARIANT", "NORMAL", lid, data, length))
			{
				throw std::runtime_error("Could not update properties!");
				return;
			}
		}

		void Save(const std::string& path)
		{
			// if source != destination
			if (path != m_stylePath)
			{
				// copy the source file and modify the new one
				// since we cant create a file from scratch
				const std::string& originalFile = m_stylePath;
				std::ifstream src(originalFile, std::ios::binary);

				const std::string& newFile = path;
				std::ofstream dst(newFile, std::ios::binary);
				dst << src.rdbuf();
			}

			UpdateHandle updHandle = libmsstyle::BeginUpdate(path);
			if (updHandle == NULL)
			{
				throw std::runtime_error("Could not open the file for writing!");
			}

			SaveResources(updHandle);
			SaveProperties(updHandle);

			if (!libmsstyle::EndUpdate(updHandle))
			{
				throw std::runtime_error("Could not write the changes to the file!");
				return;
			}
		}

		Platform GetCompatiblePlatform() const
		{
			return m_stylePlatform;
		}

		std::string GetPath() const
		{
			return m_stylePath;
		}

		StyleResource GetResourceFromProperty(const StyleProperty& prop)
		{
			Resource r;

			if (prop.GetTypeID() == IDENTIFIER::FILENAME)
			{
				r = libmsstyle::GetResource(m_moduleHandle, prop.variants.imagetype.imageID, "IMAGE");
				return StyleResource(r.data, r.size, prop.nameID , StyleResourceType::IMAGE);
			}
			else if (prop.GetTypeID() == IDENTIFIER::DISKSTREAM)
			{
				r = libmsstyle::GetResource(m_moduleHandle, prop.variants.imagetype.imageID, "STREAM");
				return StyleResource(r.data, r.size, prop.nameID, StyleResourceType::ATLAS);
			}

			return StyleResource(nullptr, 0, 0, StyleResourceType::IMAGE);
		}


		std::string GetQueuedResourceUpdate(int nameId, StyleResourceType type)
		{
			StyleResource key(nullptr, 0, nameId, type);
			auto res = m_resourceUpdates.find(key);
			if (res != m_resourceUpdates.end())
			{
				return res->second;
			}
			else return std::string();
		}


		void QueueResourceUpdate(int nameId, StyleResourceType type, std::string pathToNew)
		{
			StyleResource key(nullptr, 0, nameId, type);
			m_resourceUpdates[key] = pathToNew;
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
						continue;
					}
					else cls = &(result->second);


					// See if we have created a "Style Part" object for this
					// partID inside the current "Style Class". If not, create one.
					lookup::PartList partInfo = lookup::FindParts(cls->className.c_str(), m_stylePlatform);
					StylePart* part = cls->FindPart(tmpProp->partID);
					if (part == nullptr)
					{
						StylePart newPart;
						newPart.partID = tmpProp->partID;

						if (tmpProp->partID < partInfo.numParts)
						{
							newPart.partName = partInfo.parts[tmpProp->partID].partName;
						}
						else
						{
							char txt[16];
							sprintf(txt, "Part %d", tmpProp->partID);
							newPart.partName = std::string(txt);
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
								newState.stateName = std::string(txt);
							}
						}

						state = part->AddState(newState);
					}


					// problem: i saved just ptrs before. now i need real data!!
					state->AddProperty(tmpProp);
					m_propsFound++;
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

		
		int m_propsFound;
		ModuleHandle m_moduleHandle;
		Platform m_stylePlatform;

		std::string m_stylePath;
		std::unordered_map<int32_t, StyleClass> m_classes;
		std::unordered_map<StyleResource, std::string, ResourceHasher> m_resourceUpdates;
	};





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

	size_t VisualStyle::GetClassCount()
	{
		return impl->GetClassCount();
	}

	Platform VisualStyle::GetCompatiblePlatform() const
	{
		return impl->GetCompatiblePlatform();
	}

	int VisualStyle::GetPropertyCount() const
	{
		return impl->m_propsFound;
	}

	StyleResource VisualStyle::GetResourceFromProperty(const StyleProperty& prop)
	{
		return impl->GetResourceFromProperty(prop);
	}

	std::string VisualStyle::GetQueuedResourceUpdate(int nameId, StyleResourceType type)
	{
		return impl->GetQueuedResourceUpdate(nameId, type);
	}

	void VisualStyle::QueueResourceUpdate(int nameId, StyleResourceType type, const std::string& newResource)
	{
		return impl->QueueResourceUpdate(nameId, type, newResource);
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

	VisualStyle::ClassIterator VisualStyle::begin()
	{
		return impl->m_classes.begin();
	}

	VisualStyle::ClassIterator VisualStyle::end()
	{
		return impl->m_classes.end();
	}
}