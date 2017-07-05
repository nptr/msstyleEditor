#include "CustomTreeItemData.h"


////////////////////////////////////////////////////////////////////////////////
PropClassTreeItemData::PropClassTreeItemData(msstyle::MsStyleClass* classPtr)
{
	propClass = classPtr;
}


PropClassTreeItemData::~PropClassTreeItemData()
{
}

msstyle::MsStyleClass* PropClassTreeItemData::GetClass()
{
	return propClass;
}


////////////////////////////////////////////////////////////////////////////////
PartTreeItemData::PartTreeItemData(msstyle::MsStylePart* partptr)
{
	part = partptr;
}

PartTreeItemData::~PartTreeItemData()
{

}

msstyle::MsStylePart* PartTreeItemData::GetMsStylePart()
{
	return part;
}


////////////////////////////////////////////////////////////////////////////////
PropTreeItemData::PropTreeItemData(msstyle::MsStyleProperty* propPtr)
{
	property = propPtr;
}

PropTreeItemData::~PropTreeItemData()
{

}

msstyle::MsStyleProperty* PropTreeItemData::GetMSStyleProp()
{
	return property;
}