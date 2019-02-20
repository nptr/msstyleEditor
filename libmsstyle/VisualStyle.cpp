#include "VisualStyle.h"
#include "StringUtil.h"

#include "VisualStyleEnums.h"
#include "VisualStyleParts.h"
#include "VisualStyleStates.h"
#include "VisualStyleDefinitions.h"

#include "PropertyReader.h"
#include "PropertyWriter.h"

#include <string.h>
#include <fstream>
#include <algorithm>
#include <cstdarg>

#ifndef NDEBUG
#include <Windows.h>
#endif

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

	struct ResourceComparer
	{
		bool operator()(const StyleResource& r1, const StyleResource& r2) const
		{
			return r1 == r2;
		}
	};

	struct PropertySortAscending
	{
		inline bool operator() (const StyleProperty* p1, const StyleProperty* p2)
		{
			return (p1->header.nameID < p2->header.nameID);
		}
	};

	class VisualStyle::Impl
	{
	public:
		Impl()
			: m_propsFound(0)
			, m_moduleHandle(0)
            , m_canSaveStringTable(false)
		{
		}

		~Impl()
		{
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
							// free memory
							delete *propIt;
						}
					}
				}
			}

            priv::CloseModule(m_moduleHandle);
		}

		void Log(const char* format, ...)
		{
#ifndef NDEBUG
			char textbuffer[100];

			va_list args;
			va_start(args, format);
			vsnprintf(textbuffer, 99, format, args);
			va_end(args);

			OutputDebugStringA(textbuffer);
#endif
		}

		StyleClass* GetClass(int index)
		{
			return &(m_classes.at(index));
		}

		size_t GetClassCount()
		{
			return m_classes.size();
		}

        void BuildStyleTree(std::map<int32_t, StyleClass>& classes, Platform os)
        {
            for (auto& cls : classes)
            {
                lookup::PartList partList = lookup::FindParts(cls.second.className.c_str(), os);
                for (int i = 0; i < partList.numParts; ++i)
                {
                    PartMap partEntry = partList.parts[i];
                    
                    StylePart newPart;
                    newPart.partID = partEntry.partID;
                    newPart.partName = partEntry.partName;
                    StylePart* addedPart = cls.second.AddPart(newPart);

                    for (int j = 0; j < partEntry.numStates; ++j)
                    {
                        const StateMap stateEntry = partEntry.states[j];

                        StyleState newState;
                        newState.stateID = stateEntry.stateID;
                        newState.stateName = stateEntry.stateName;
                        addedPart->AddState(newState);
                    }
                }
                
            }
        }

		void Load(const std::string& path)
		{
			m_stylePath = path;
            m_moduleHandle = priv::OpenModule(path);
			if (m_moduleHandle != 0)
			{
                priv::Resource cmap = priv::GetResource(m_moduleHandle, "CMAP", "CMAP");
				if (cmap.size != 0)
					LoadClassmap(cmap);
				else throw std::runtime_error("Style contains no class map!");

				m_stylePlatform = DeterminePlatform();
                BuildStyleTree(m_classes, m_stylePlatform);

                priv::Resource pmap = priv::GetResource(m_moduleHandle, "NORMAL", "VARIANT");
				if (pmap.size != 0)
					LoadProperties(pmap);
				else throw std::runtime_error("Style contains no properties!");

                priv::LoadStringTable(m_moduleHandle, m_stringTable);
			}
			else throw std::runtime_error("Couldn't open file as PE resource!");
		}

        bool SaveResources(priv::UpdateHandle updateHandle, std::runtime_error& error)
		{
			for (auto& resource : m_resourceUpdates)
			{
                char* buffer;
                unsigned int length;
                const char* resName;

                if (!priv::FileReadAllBytes(resource.second, &buffer, &length))
				{
                    char errorMsg[512];
                    sprintf(errorMsg, "Replacing resource with id '%d' failed because '%s' is not accessible or missing!", resource.first.GetNameID(), resource.second.c_str());
                    error = std::runtime_error(errorMsg);
                    return false;
				}

				switch (resource.first.GetType())
				{
				case StyleResourceType::rtImage:
					resName = "IMAGE";
					break;
				case StyleResourceType::rtAtlas:
					resName = "STREAM";
					break;
				default:
					continue;
				}

				bool success = priv::UpdateStyleResource(updateHandle, resName, resource.first.GetNameID(), buffer, length);
                priv::FileFreeBytes(buffer);

				if (!success)
				{
                    error = std::runtime_error("Could not update IMAGE/STREAM resource!");
                    return false;
				}
			}

            return true;
		}

        bool SavePropertiesSorted(priv::UpdateHandle updateHandle, std::runtime_error& error)
		{
			// assume that twice the size common properties require is enough
			int estimatedSize = m_propsFound * 48 * 2;
			char* data = new char[estimatedSize];
			char* dataptr = data;

			libmsstyle::rw::PropertyWriter writer;

			//
			// Properties have to be stored sorted by: classId, partId, stateId, nameId
			// Otherwise the OS will not accept them! Actually, nameId isn't required
			// but microsoft's styles are sorted by it as well, so mine will be too.
			//
			// Since the classes, parts and states are stored in (ordered) maps, im fine
			//

			// for all classes
			for (auto it = m_classes.begin(); it != m_classes.end(); ++it)
			{
				// for all parts
				for (auto partIt = it->second.begin(); partIt != it->second.end(); ++partIt)
				{
					// for all states
					for (auto stateIt = partIt->second.begin(); stateIt != partIt->second.end(); ++stateIt)
					{
						// sort properties by nameId
						std::sort(stateIt->second.begin(), stateIt->second.end(), PropertySortAscending());

						// for all properties
						for (auto propIt = stateIt->second.begin(); propIt != stateIt->second.end(); ++propIt)
						{
							dataptr = writer.WriteProperty(dataptr, *(*propIt));

                            if (dataptr - data > estimatedSize)
                            {
                                error = std::runtime_error("I haven't allocated enough memory to save the file..sorry for that!");
                                return false;
                            }
						}
					}
				}

				// we would save the classnames in the CMAP resource
				// here, but as long as we dont modify the classID of
				// the properties, this shouldn't be necessary
			}

            priv::LanguageId lid = priv::GetFirstLanguageId(m_moduleHandle, "NORMAL", "VARIANT");
			unsigned int length = static_cast<unsigned int>(dataptr - data);

            if (!priv::UpdateStyleResource(updateHandle, "VARIANT", "NORMAL", lid, data, length))
			{
				error = std::runtime_error("Could not update properties!");
                return false;
			}

            return true;
		}


        void Save(const std::string& path)
        {
            // if source != destination
            if (path != m_stylePath)
            {
                // copy the source file and modify the new one
                // since we cant create a file from scratch
                const std::string& originalFile = m_stylePath;
                const std::string& newFile = path;

                priv::DuplicateFile(originalFile, newFile);
            }
            else
            {
                throw std::runtime_error("Cannot overwrite the original file!");
            }

            priv::UpdateHandle updHandle = priv::BeginUpdate(path);
            if (updHandle == NULL)
            {
                priv::RemoveFile(path);
                throw std::runtime_error("Could not open the file for writing! (BeginUpdateResource)");
            }

            std::runtime_error error("");

            if (!SaveResources(updHandle, error))
            {
                priv::EndUpdate(updHandle, true);
                priv::RemoveFile(path);
                throw error;
            }

            if (!SavePropertiesSorted(updHandle, error))
            {
                priv::EndUpdate(updHandle, true);
                priv::RemoveFile(path);
                throw error;
            }

            if (m_canSaveStringTable)
            {
                priv::ModuleHandle modHandle = priv::OpenModule(path);
                if (modHandle == NULL)
                {
                    priv::EndUpdate(updHandle, true);
                    priv::RemoveFile(path);
                    throw std::runtime_error("Could not open the file for writing! (LoadLibraryEx)");
                }

                if (!priv::UpdateStringTable(modHandle, updHandle, m_stringTable, error))
                {
                    priv::CloseModule(modHandle);
                    priv::EndUpdate(updHandle, true);
                    priv::RemoveFile(path);
                    throw error;
                }
                else
                {
                    // close the module before calling EndUpdate(). If not
                    // updating fails because the file is in use.
                    priv::CloseModule(modHandle);
                }
            }

            int updateError = priv::EndUpdate(updHandle, false);
			if (updateError)
			{
				std::string msg = format_string("Could not write the changes to the file! Error Code: %d", updateError);
                priv::RemoveFile(path);
				throw std::runtime_error(msg);
				return;
			}

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


		void QueueResourceUpdate(int nameId, StyleResourceType type, const std::string& pathToNew)
		{
			StyleResource key(nullptr, 0, nameId, type);
			m_resourceUpdates[key] = pathToNew;
		}


        StyleResource GetResource(int id, StyleResourceType type)
        {
            priv::Resource r;
            switch (type)
            {
            case rtImage:
                r = priv::GetResource(m_moduleHandle, id, "IMAGE");
                return StyleResource(r.data, r.size, 0, StyleResourceType::rtImage);
            case rtAtlas:
                r = priv::GetResource(m_moduleHandle, id, "STREAM");
                return StyleResource(r.data, r.size, 0, StyleResourceType::rtAtlas);
            default:
                return StyleResource(nullptr, 0, 0, StyleResourceType::rtNone);
            }
        }


        StyleResource GetResourceFromProperty(const StyleProperty& prop)
        {
            priv::Resource r;
            switch (prop.GetTypeID())
            {
            case FILENAME:
            case FILENAME_LITE:
                r = priv::GetResource(m_moduleHandle, prop.header.shortFlag, "IMAGE");
                return StyleResource(r.data, r.size, 0, StyleResourceType::rtImage);
            case DISKSTREAM:
                r = priv::GetResource(m_moduleHandle, prop.header.shortFlag, "STREAM");
                return StyleResource(r.data, r.size, 0, StyleResourceType::rtAtlas);
            default:
                return StyleResource(nullptr, 0, 0, StyleResourceType::rtNone);
            }
        }


		void LoadClassmap(priv::Resource classResource)
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

		void LoadProperties(priv::Resource propResource)
		{
			libmsstyle::rw::PropertyReader reader(m_classes.size());

			const char* start = reinterpret_cast<const char*>(propResource.data);
			const char* end = start + propResource.size;
			const char* current = start;
			const char* next = start;
			const StyleProperty* prev = 0;
			char txtBuffer[100];

			libmsstyle::rw::PropertyReader::Result result;

			while (current < end - 4)
			{
				StyleProperty* tmpProp = new StyleProperty();
				next = reader.ReadNextProperty(current, result, tmpProp);

				switch (result)
				{
				case rw::PropertyReader::Ok:
					Log("[N: %d, T: %d, C: %d, P: %d, S: %d]\r\n", tmpProp->header.nameID, tmpProp->header.typeID, tmpProp->header.classID, tmpProp->header.partID, tmpProp->header.stateID);
					prev = reinterpret_cast<const StyleProperty*>(current);
					break;
				case rw::PropertyReader::UnknownType:
					Log("Unknown: [N: %d, T: %d, C: %d, P: %d, S: %d] @ 0x%08x\r\n", tmpProp->header.nameID, tmpProp->header.typeID, tmpProp->header.classID, tmpProp->header.partID, tmpProp->header.stateID, current - start);
					prev = reinterpret_cast<const StyleProperty*>(current);
					break;
				case rw::PropertyReader::SkippedBytes:
					Log("Skipped %d bytes after prop [N: %d, T: %d]\r\n", next - current, prev->header.nameID, prev->header.typeID);
					current = next;
					delete tmpProp;
					continue;
				case rw::PropertyReader::BadProperty:
					sprintf(txtBuffer, "Bad property [N: %d, T: %d] @ 0x%08x\r\n", tmpProp->header.nameID, tmpProp->header.typeID, current - start);
					delete tmpProp;
					throw std::runtime_error(txtBuffer);
					return;
				default:
					throw std::runtime_error("ReadNextProperty(). Unknown result. Should never happen.");
					return;
				}

				current = next;

				// See if we have created a "Style Class" object already for
				// this classID that we can use. If not, create one.
				StyleClass* cls;
				const auto& result = m_classes.find(tmpProp->header.classID);
				if (result == m_classes.end())
				{
					throw std::runtime_error("Found property with unknown class ID");
				}
				else cls = &(result->second);


				// See if we have created a "Style Part" object for this
				// partID inside the current "Style Class". If not, create one.
				lookup::PartList partInfo = lookup::FindParts(cls->className.c_str(), m_stylePlatform);
				StylePart* part = cls->FindPart(tmpProp->header.partID);
				if (part == nullptr)
				{
					StylePart newPart;
					newPart.partID = tmpProp->header.partID;

					if (tmpProp->header.partID < partInfo.numParts)
					{
						newPart.partName = partInfo.parts[tmpProp->header.partID].partName;
					}
					else
					{
						newPart.partName = format_string("Part %d", tmpProp->header.partID);
					}

					part = cls->AddPart(newPart);
				}


				// See if we have created a "Style State" object for this
				// stateID inside the current "Style Part". If not, create one.
				StyleState* state = part->FindState(tmpProp->header.stateID);
				if (state == nullptr)
				{
					StyleState newState;
					newState.stateID = tmpProp->header.stateID;

					if (tmpProp->header.partID < partInfo.numParts &&
						tmpProp->header.stateID < partInfo.parts[tmpProp->header.partID].numStates)
					{
						newState.stateName = partInfo.parts[tmpProp->header.partID].states[tmpProp->header.stateID].stateName;
					}
					else
					{
						if (tmpProp->header.stateID == 0)
						{
							newState.stateName = "Common";
						}
						else
						{
							newState.stateName = format_string("State %d", tmpProp->header.stateID);
						}
					}

					state = part->AddState(newState);
				}

				state->AddProperty(tmpProp);
				m_propsFound++;
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

        bool m_canSaveStringTable;
		int m_propsFound;
		priv::ModuleHandle m_moduleHandle;
		Platform m_stylePlatform;
        StringTable m_stringTable;

		std::string m_stylePath;
		std::map<int32_t, StyleClass> m_classes;
		std::unordered_map<StyleResource, std::string, ResourceHasher, ResourceComparer> m_resourceUpdates;
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
		return impl->m_stylePlatform;
	}

	int VisualStyle::GetPropertyCount() const
	{
		return impl->m_propsFound;
	}

    StyleResource VisualStyle::GetResource(int id, StyleResourceType type)
    {
        return impl->GetResource(id, type);
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

	const std::string& VisualStyle::GetPath() const
	{
		return impl->m_stylePath;
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

	StyleClass* VisualStyle::FindClass(int id) const
	{
		const auto res = impl->m_classes.find(id);
		if (res != impl->m_classes.end())
			return &(res->second);
		else return nullptr;
	}

    StringTable& VisualStyle::GetStringTable()
    {
        return impl->m_stringTable;
    }
}