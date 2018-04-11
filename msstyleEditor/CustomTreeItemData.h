#pragma once
#include "wx\treebase.h"
#include "libmsstyle\VisualStyle.h"


// Instances of these classes are attached to treeview nodes.
// They link to a class/part/property the the style data
class ClassTreeItemData : public wxTreeItemData
{
protected:
	libmsstyle::StyleClass* propClass;

public:
	ClassTreeItemData(libmsstyle::StyleClass* classPtr);
	virtual ~ClassTreeItemData();

	libmsstyle::StyleClass* GetClass();
};

class PropTreeItemData : public wxTreeItemData
{
protected:
	libmsstyle::StyleProperty* property;

public:
	PropTreeItemData(libmsstyle::StyleProperty* classPtr);
	virtual ~PropTreeItemData();

	libmsstyle::StyleProperty* GetProperty();
};

class PartTreeItemData : public wxTreeItemData
{
protected:
	libmsstyle::StylePart* part;

public:
	PartTreeItemData(libmsstyle::StylePart* partptr);
	virtual ~PartTreeItemData();

	libmsstyle::StylePart* GetPart();
};