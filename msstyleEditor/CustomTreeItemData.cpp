#include "CustomTreeItemData.h"


////////////////////////////////////////////////////////////////////////////////
PropClassTreeItemData::PropClassTreeItemData(libmsstyle::StyleClass* classPtr)
{
	propClass = classPtr;
}


PropClassTreeItemData::~PropClassTreeItemData()
{
}

libmsstyle::StyleClass* PropClassTreeItemData::GetClass()
{
	return propClass;
}


////////////////////////////////////////////////////////////////////////////////
PartTreeItemData::PartTreeItemData(libmsstyle::StylePart* partptr)
{
	part = partptr;
}

PartTreeItemData::~PartTreeItemData()
{

}

libmsstyle::StylePart* PartTreeItemData::GetMsStylePart()
{
	return part;
}


////////////////////////////////////////////////////////////////////////////////
PropTreeItemData::PropTreeItemData(libmsstyle::StyleProperty* propPtr)
{
	property = propPtr;
}

PropTreeItemData::~PropTreeItemData()
{

}

libmsstyle::StyleProperty* PropTreeItemData::GetMSStyleProp()
{
	return property;
}