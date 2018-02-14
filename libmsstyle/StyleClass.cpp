#include "StyleClass.h"


StyleClass::StyleClass()
{
}


StyleClass::~StyleClass()
{
}


void StyleClass::AddPart(const StylePart& part)
{
	m_parts[part.partID] = part;
}


int StyleClass::GetPartCount()
{
	return m_parts.size();
}


const StylePart* StyleClass::GetPart(int index)
{
	return &(m_parts.at(index));
}
