#pragma once

#include "wx\treebase.h"

namespace libmsstyle
{
	class StyleClass;
	class StylePart;
	class StyleProperty;
}

// Instances of these classes are attached to treeview nodes.
// They reference class/part/property items in the style data.
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