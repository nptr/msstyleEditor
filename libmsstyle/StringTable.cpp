#include "StringTable.h"

#include <map>
#include <stdint.h>

namespace libmsstyle
{
	class StringTable::Impl
	{
	public:
		std::map<int32_t, std::string> m_entries;
	};



	StringTable::StringTable()
		: impl(std::make_shared<Impl>())
	{
	}

	StringTable::~StringTable()
	{
	}

	StringTable::StringTableIterator StringTable::begin()
	{
		return impl->m_entries.begin();
	}

	StringTable::StringTableIterator StringTable::end()
	{
		return impl->m_entries.end();
	}

	StringTable::StringTableIterator StringTable::find(int32_t id) const
	{
		return impl->m_entries.find(id);
	}

	size_t StringTable::size() const
	{
		return impl->m_entries.size();
	}

	void StringTable::Set(int id, const std::string& resourceString)
	{
		impl->m_entries[id] = resourceString;
	}

	void StringTable::Remove(int id)
	{
		auto& it = impl->m_entries.find(id);
		if (it != impl->m_entries.end())
		{
			impl->m_entries.erase(it);
		}
	}
}