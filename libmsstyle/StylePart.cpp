#include "StylePart.h"

namespace libmsstyle
{

	StylePart::StylePart()
	{
	}


	StylePart::~StylePart()
	{
	}


	StyleState* StylePart::AddState(const StyleState& state)
	{
		auto it = m_states.insert(std::make_pair(state.stateID, state));
		return &(it.first->second);
	}


	int StylePart::GetStateCount()
	{
		return m_states.size();
	}


	const StyleState* StylePart::GetState(int index)
	{
		return &(m_states.at(index));
	}

	StyleState* StylePart::FindState(int stateId)
	{
		const auto& res = m_states.find(stateId);
		if (res != m_states.end())
			return &(res->second);
		else return nullptr;
	}

}