#include "StyleClass.h"

#include <unordered_map>


namespace libmsstyle
{
	class StyleClass::Impl
	{
	public:
		Impl()
		{

		}

		StylePart* AddPart(const StylePart& part)
		{
			auto it = m_parts.insert(std::make_pair(part.partID, part));
			return &(it.first->second);
		}

		int GetPartCount()
		{
			return m_parts.size();
		}

		StylePart* GetPart(int index)
		{
			return &(m_parts.at(index));
		}

		StylePart* FindPart(int partId)
		{
			const auto& res = m_parts.find(partId);
			if (res != m_parts.end())
				return &(res->second);
			else return nullptr;
		}

		std::unordered_map<int32_t, StylePart> m_parts;
	};



	StyleClass::StyleClass()
		: impl(new Impl())
	{
	}

	StyleClass::~StyleClass()
	{
		if (impl)
		{
			delete impl;
		}
	}

	StylePart* StyleClass::AddPart(const StylePart& part)
	{
		return impl->AddPart(part);
	}

	int StyleClass::GetPartCount()
	{
		return impl->GetPartCount();
	}

	StylePart* StyleClass::GetPart(int index)
	{
		return impl->GetPart(index);
	}

	StylePart* StyleClass::FindPart(int partId)
	{
		return impl->FindPart(partId);
	}

}