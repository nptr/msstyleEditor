#pragma once
#include "wx\treebase.h"
#include "VisualStyle.h"


// Instances of these classes are attached to treeview nodes.
// They link to a class/part/property the the style data
class PropClassTreeItemData : public wxTreeItemData
{
protected:
	msstyle::MsStyleClass* propClass;

public:
	PropClassTreeItemData(msstyle::MsStyleClass* classPtr);
	virtual ~PropClassTreeItemData();

	msstyle::MsStyleClass* GetClass();
};

class PropTreeItemData : public wxTreeItemData
{
protected:
	msstyle::MsStyleProperty* property;

public:
	PropTreeItemData(msstyle::MsStyleProperty* classPtr);
	virtual ~PropTreeItemData();

	msstyle::MsStyleProperty* GetMSStyleProp();
};

class PartTreeItemData : public wxTreeItemData
{
protected:
	msstyle::MsStylePart* part;

public:
	PartTreeItemData(msstyle::MsStylePart* partptr);
	virtual ~PartTreeItemData();

	msstyle::MsStylePart* GetMsStylePart();
};

