#include "StylePart.h"

#include <unordered_map>


namespace libmsstyle
{
	class StylePart::Impl
	{
	public:
		Impl()
		{

		}

		StyleState* AddState(const StyleState& state)
		{
			auto it = m_states.insert(std::make_pair(state.stateID, state));
			return &(it.first->second);
		}


		int GetStateCount()
		{
			return m_states.size();
		}


		StyleState* GetState(int index)
		{
			return &(m_states.at(index));
		}

		StyleState* FindState(int stateId)
		{
			const auto& res = m_states.find(stateId);
			if (res != m_states.end())
				return &(res->second);
			else return nullptr;
		}

		std::unordered_map<int32_t, StyleState> m_states;
	};



	StylePart::StylePart()
		: impl(std::make_shared<Impl>())
	{
	}


	StylePart::~StylePart()
	{
	}


	StyleState* StylePart::AddState(const StyleState& state)
	{
		return impl->AddState(state);
	}


	int StylePart::GetStateCount()
	{
		return impl->GetStateCount();
	}


	StyleState* StylePart::GetState(int index)
	{
		return impl->GetState(index);
	}

	StyleState* StylePart::FindState(int stateId)
	{
		return impl->FindState(stateId);
	}

}