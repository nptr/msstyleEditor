#pragma once

#include "StyleResource.h"

namespace libmsstyle
{
	class StyleResource::Impl
	{
	public:

		bool operator==(const Impl& rhs)
		{
			if (this->m_name == rhs.m_name &&
				this->m_type == rhs.m_type &&
				this->m_size == rhs.m_size &&
				this->m_data == rhs.m_data)
				return true;
			else return false;
		}

		const void* m_data;
		int m_size;
		int m_name;
		StyleResourceType m_type;
	};

	StyleResource::StyleResource()
		: impl(std::make_shared<Impl>())
	{
		impl->m_data = nullptr;
		impl->m_size = 0;
		impl->m_name = 0;
		impl->m_type = StyleResourceType::rtImage;
	}

	StyleResource::StyleResource(const StyleResource& other)
		: impl(std::make_shared<Impl>())
	{
		impl->m_data = other.impl->m_data;
		impl->m_size = other.impl->m_size;
		impl->m_name = other.impl->m_name;
		impl->m_type = other.impl->m_type;
	}

	StyleResource::StyleResource(const void* data, int size, int nameId, StyleResourceType type)
		: impl(std::make_shared<Impl>())
	{
		impl->m_data = data;
		impl->m_size = size;
		impl->m_name = nameId;
		impl->m_type = type;
	}

	StyleResource::~StyleResource()
	{
	}

	StyleResource& StyleResource::operator=(const StyleResource& other)
	{
		if (&other == this)
			return *this;

		// use others data, but dont copy the impl ptr!
		impl->m_data = other.impl->m_data;
		impl->m_size = other.impl->m_size;
		impl->m_name = other.impl->m_name;
		impl->m_type = other.impl->m_type;

		return *this;
	}

	bool StyleResource::operator==(const StyleResource& rhs) const
	{
		return *impl == *rhs.impl;
	}

	int StyleResource::GetNameID() const
	{
		return impl->m_name;
	}

	StyleResourceType StyleResource::GetType() const
	{
		return impl->m_type;
	}

	const char* StyleResource::GetData() const
	{
		return static_cast<const char*>(impl->m_data);
	}
	
	int StyleResource::GetSize() const
	{
		return impl->m_size;
	}
}