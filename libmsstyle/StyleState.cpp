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

		StyleProperty* FindPropertyByAddress(const StyleProperty* prop) const
		{
			for (auto& it = m_properties.begin(); it != m_properties.end(); ++it)
				if (prop == *it)
					return *it;

			return nullptr;
		}

		StyleProperty* FindPropertyByValue(const StyleProperty& prop) const
		{
			for (auto& it = m_properties.begin(); it != m_properties.end(); ++it)
				if (prop == **it)
					return *it;

			return nullptr;
		}

		void RemoveProperty(int index)
		{
			m_properties.erase(m_properties.begin() + index);
		}

		void RemoveProperty(const StyleProperty* prop)
		{
			for (auto& it = m_properties.begin(); it != m_properties.end(); ++it)
			{
				if (*it == prop)
				{
					delete *it;
					m_properties.erase(it);
					return;
				}
			}
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

	StyleProperty* StyleState::FindPropertyByAddress(const StyleProperty* prop) const
	{
		return impl->FindPropertyByAddress(prop);
	}

	StyleProperty* StyleState::FindPropertyByValue(const StyleProperty& prop) const
	{
		return impl->FindPropertyByValue(prop);
	}

	void StyleState::RemoveProperty(int index)
	{
		impl->RemoveProperty(index);
	}

	void StyleState::RemoveProperty(const StyleProperty* prop)
	{
		impl->RemoveProperty(prop);
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