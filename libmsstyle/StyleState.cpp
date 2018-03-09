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


		int GetPropertyCount() const
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
		: impl(new Impl())
	{
	}


	StyleState::~StyleState()
	{
		if (impl)
		{
			delete impl;
		}
	}


	StyleProperty* StyleState::AddProperty(StyleProperty* prop)
	{
		return impl->AddProperty(prop);
	}


	void StyleState::RemoveProperty(int index)
	{
		impl->RemoveProperty(index);
	}


	int StyleState::GetPropertyCount() const
	{
		return impl->GetPropertyCount();
	}


	StyleProperty* StyleState::GetProperty(int index) const
	{
		return impl->GetProperty(index);
	}

}