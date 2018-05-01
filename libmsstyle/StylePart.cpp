#include "StylePart.h"

namespace libmsstyle
{
	class StylePart::Impl
	{
	public:
		std::map<int32_t, StyleState> m_states;
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
		auto it = impl->m_states.insert(std::make_pair(state.stateID, state));
		return &(it.first->second);
	}

	StyleState* StylePart::FindState(int stateId) const
	{
		const auto& res = impl->m_states.find(stateId);
		if (res != impl->m_states.end())
			return &(res->second);
		else return nullptr;
	}

	size_t StylePart::GetStateCount() const
	{
		return impl->m_states.size();
	}

	StylePart::StateIterator StylePart::begin()
	{
		return impl->m_states.begin();
	}

	StylePart::StateIterator StylePart::end()
	{
		return impl->m_states.end();
	}
}