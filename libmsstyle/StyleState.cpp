#include "StyleProperty.h"
#include "StyleState.h"

StyleState::StyleState()
{
}


StyleState::~StyleState()
{
}


void StyleState::AddProperty(const StyleProperty& prop)
{
	m_properties.push_back(prop);
}


void StyleState::RemoveProperty(int index)
{
	m_properties.erase(m_properties.begin() + index);
}


int StyleState::GetPropertyCount()
{
	return m_properties.size();
}


const StyleProperty* StyleState::GetProperty(int index)
{
	return &m_properties[index];
}
