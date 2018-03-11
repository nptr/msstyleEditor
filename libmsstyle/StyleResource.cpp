#pragma once

#include "StyleResource.h"

namespace libmsstyle
{
	class StyleResource::Impl
	{
	public:
		const void* m_data;
		int m_size;
		int m_name;
		StyleResourceType m_type;
	};

	StyleResource::StyleResource()
		: impl(new Impl())
	{
		impl->m_data = nullptr;
		impl->m_size = 0;
		impl->m_name = 0;
		impl->m_type = StyleResourceType::IMAGE;
	}

	StyleResource::StyleResource(const void* data, int size, int nameId, StyleResourceType type)
		: impl(new Impl())
	{
		impl->m_data = data;
		impl->m_size = size;
		impl->m_name = nameId;
		impl->m_type = type;
	}

	StyleResource::~StyleResource()
	{
		if (impl)
		{
			delete impl;
		}
	}

	int StyleResource::GetNameID()
	{
		return impl->m_name;
	}

	StyleResourceType StyleResource::GetType()
	{
		return impl->m_type;
	}

	const char* StyleResource::GetData()
	{
		return static_cast<const char*>(impl->m_data);
	}

	int StyleResource::GetSize()
	{
		return impl->m_size;
	}
}