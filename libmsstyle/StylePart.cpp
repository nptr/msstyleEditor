#include "StylePart.h"


StylePart::StylePart()
{
}


StylePart::~StylePart()
{
}


void StylePart::AddState(const StyleState& state)
{
	m_states[state.stateID] = state;
}


int StylePart::GetStateCount()
{
	return m_states.size();
}


const StyleState* StylePart::GetState(int index)
{
	return &(m_states.at(index));
}
