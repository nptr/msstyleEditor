#include "StyleState.h"
#include "StyleProperty.h"

#include <vector>

namespace libmsstyle
{
	class StyleState::Impl
	{
	public:
		Impl()
		{

		}

		StyleProperty* AddProperty(StyleProperty* prop)
		{
			m_properties.push_back(prop);
			return m_properties[m_properties.size() - 1];
		}

		void RemoveProperty(int index)
		{
			m_properties.erase(m_properties.begin() + index);
		}

		size_t GetPropertyCount() const
		{
			return m_properties.size();
		}

		StyleProperty* GetProperty(int index) const
		{
			return m_properties[index];
		}

		std::vector<StyleProperty*> m_properties;
	};


	StyleState::StyleState()
		: impl(std::make_shared<Impl>())
	{
	}


	StyleState::~StyleState()
	{
	}


	StyleProperty* StyleState::AddProperty(StyleProperty* prop)
	{
		return impl->AddProperty(prop);
	}

	StyleProperty* StyleState::GetProperty(int index) const
	{
		return impl->GetProperty(index);
	}

	void StyleState::RemoveProperty(int index)
	{
		impl->RemoveProperty(index);
	}

	size_t StyleState::GetPropertyCount() const
	{
		return impl->GetPropertyCount();
	}

	StyleState::PropertyIterator StyleState::begin()
	{
		return impl->m_properties.begin();
	}

	StyleState::PropertyIterator StyleState::end()
	{
		return impl->m_properties.end();
	}

}