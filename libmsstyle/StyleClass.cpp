#include "StyleClass.h"

#include <unordered_map>


namespace libmsstyle
{
	class StyleClass::Impl
	{
	public:
		std::unordered_map<int32_t, StylePart> m_parts;
	};



	StyleClass::StyleClass()
		: impl(std::make_shared<Impl>())
	{
	}

	StyleClass::~StyleClass()
	{
	}

	StylePart* StyleClass::AddPart(const StylePart& part)
	{
		auto it = impl->m_parts.insert(std::make_pair(part.partID, part));
		return &(it.first->second);
	}

	StylePart* StyleClass::FindPart(int partId)
	{
		const auto res = impl->m_parts.find(partId);
		if (res != impl->m_parts.end())
			return &(res->second);
		else return nullptr;
	}

	size_t StyleClass::GetPartCount()
	{
		return impl->m_parts.size();
	}

	StyleClass::PartIterator StyleClass::begin()
	{
		return impl->m_parts.begin();
	}

	StyleClass::PartIterator StyleClass::end()
	{
		return impl->m_parts.end();
	}
}