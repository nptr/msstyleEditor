#include "CustomTreeItemData.h"


////////////////////////////////////////////////////////////////////////////////
ClassTreeItemData::ClassTreeItemData(libmsstyle::StyleClass* classPtr)
{
	propClass = classPtr;
}


ClassTreeItemData::~ClassTreeItemData()
{
}

libmsstyle::StyleClass* ClassTreeItemData::GetClass()
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

libmsstyle::StylePart* PartTreeItemData::GetPart()
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

libmsstyle::StyleProperty* PropTreeItemData::GetProperty()
{
	return property;
}