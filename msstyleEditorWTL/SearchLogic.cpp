#include "SearchLogic.h"

#include "TreeItemData.h"
#include "libmsstyle\VisualStyle.h"
#include "Dialogs\SearchDlg.h"

bool ContainsProperty(const SearchProperties& search, TreeItemData* treeItemData)
{
	USES_CONVERSION;

	if (treeItemData == nullptr)
		return false;

	// If its a part node, check its properties
	if (treeItemData->type != ITEM_PART)
		return false;

	libmsstyle::StylePart* part = reinterpret_cast<libmsstyle::StylePart*>(treeItemData->object);

	for (auto& state : *part)
	{
		for (auto& prop : state.second)
		{
			// if its a property of the desired type, do a comparison
			if (prop->header.typeID != search.type)
				continue;

			// comparison
			switch (prop->header.typeID)
			{
			case libmsstyle::IDENTIFIER::POSITION:
			{
				char propPos[32];
				sprintf(propPos, "%d,%d",
					prop->data.positiontype.x,
					prop->data.positiontype.y);

				if (stricmp(propPos, W2A(search.text)) == 0)
					return true;

			} break;
			case libmsstyle::IDENTIFIER::COLOR:
			{
				char propColor[32];
				sprintf(propColor, "%d,%d,%d",
					prop->data.colortype.r,
					prop->data.colortype.g,
					prop->data.colortype.b);

				if (stricmp(propColor, W2A(search.text)) == 0)
					return true;
			} break;
			case libmsstyle::IDENTIFIER::MARGINS:
			{
				char propMargin[32];
				sprintf(propMargin, "%d,%d,%d,%d",
					prop->data.margintype.left,
					prop->data.margintype.top,
					prop->data.margintype.right,
					prop->data.margintype.bottom);

				if (stricmp(propMargin, W2A(search.text)) == 0)
					return true;
			} break;
			case libmsstyle::IDENTIFIER::RECTTYPE:
			{
				char propRect[32];
				sprintf(propRect, "%d,%d,%d,%d",
					prop->data.recttype.left,
					prop->data.recttype.top,
					prop->data.recttype.right,
					prop->data.recttype.bottom);

				if (stricmp(propRect, W2A(search.text)) == 0)
					return true;
			} break;
			case libmsstyle::IDENTIFIER::SIZE:
			{
				try
				{
					int size = std::stoi(search.text);
					if (size == prop->data.sizetype.size)
						return true;
				}
				catch (...) {}
			} break;
			}
		}
	}

	return false;
}

bool ContainsName(LPCWSTR str, TreeItemData* treeItemData)
{
	USES_CONVERSION;

	if (treeItemData == nullptr)
		return false;

	// Class Node
	if (treeItemData->type == ITEM_CLASS)
	{
		libmsstyle::StyleClass* cls = reinterpret_cast<libmsstyle::StyleClass*>(treeItemData);
		return stricmp(cls->className.c_str(), W2A(str)) == 0;
	}

	// Part Node
	if (treeItemData->type == ITEM_PART)
	{
		libmsstyle::StylePart* part = reinterpret_cast<libmsstyle::StylePart*>(treeItemData);
		return stricmp(part->partName.c_str(), W2A(str)) == 0;
	}

	// Image Node
	if (treeItemData->type == ITEM_PROPERTY)
	{
		libmsstyle::StyleProperty* prop = reinterpret_cast<libmsstyle::StyleProperty*>(treeItemData);
		return stricmp(prop->LookupName(), W2A(str)) == 0;
	}

	return false;
}