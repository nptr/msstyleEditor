#include "StyleClass.h"


StyleClass::StyleClass()
{
}


StyleClass::~StyleClass()
{
}


StylePart* StyleClass::AddPart(const StylePart& part)
{
	auto it = m_parts.insert(std::make_pair(part.partID, part));
	return &(it.first->second);
}

int StyleClass::GetPartCount()
{
	return m_parts.size();
}

const StylePart* StyleClass::GetPart(int index)
{
	return &(m_parts.at(index));
}

StylePart* StyleClass::FindPart(int partId)
{
	const auto& res = m_parts.find(partId);
	if (res != m_parts.end())
		return &(res->second);
	else return nullptr;
}
