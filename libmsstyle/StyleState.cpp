#include "StyleProperty.h"
#include "StyleState.h"

namespace libmsstyle
{

	StyleState::StyleState()
	{
	}


	StyleState::~StyleState()
	{
	}


	StyleProperty* StyleState::AddProperty(StyleProperty* prop)
	{
		m_properties.push_back(prop);
		return m_properties[m_properties.size() - 1];
	}


	void StyleState::RemoveProperty(int index)
	{
		m_properties.erase(m_properties.begin() + index);
	}


	int StyleState::GetPropertyCount() const
	{
		return m_properties.size();
	}


	StyleProperty* StyleState::GetProperty(int index) const
	{
		return m_properties[index];
	}

}