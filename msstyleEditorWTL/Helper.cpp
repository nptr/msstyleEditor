#include "Helper.h"

#include <atlctrls.h>

#include "ItemData.h"

#include "libmsstyle\Lookup.h"

#include <codecvt>	// codecvt_utf8_utf16
#include <locale>	// wstring_convert

using namespace libmsstyle;

LPVOID MkItemData(StyleProperty* prop)
{
	return new ItemData(prop, ITEM_PROPERTY);
}


TCHAR* BuildFontList(TCHAR* dst, libmsstyle::StyleProperty& prop)
{
	USES_CONVERSION;

	TCHAR* dstPtr = dst;
	auto it = libmsstyle::FONT_MAP.begin();
	for (int i = 0; it != libmsstyle::FONT_MAP.end(); ++it, ++i)
	{
		TCHAR* str = A2T(it->second);
		while (*str)
			*dstPtr++ = *str++;
		*dstPtr++ = NULL;
	}
	*dstPtr = NULL;

	return dst;
}

TCHAR* BuildEnumList(TCHAR* dst, libmsstyle::StyleProperty& prop)
{
	USES_CONVERSION;

	libmsstyle::lookup::EnumList result = libmsstyle::lookup::FindEnums(prop.header.nameID);
	if (result.enums == nullptr || result.numEnums == 0)
		return NULL;

	TCHAR* dstPtr = dst;
	for(int i = 0; i < result.numEnums; ++i)
	{
		TCHAR* str = A2T(result.enums[i].value);
		while (*str)
			*dstPtr++ = *str++;
		*dstPtr++ = NULL;
	}
	*dstPtr = NULL;

	return dst;
}

bool InitPropgridItemSpecial(PROPGRIDITEM& item, StyleProperty& prop)
{
	if (prop.GetNameID() == IDENTIFIER::COLORIZATIONCOLOR)
	{
		item.iItemType = PIT_COLOR;
		item.lpCurValue = (LPARAM)RGB(prop.data.colortype.r, prop.data.colortype.g, prop.data.colortype.b);
		return true;
	}

	return false;
}

void SetInitPropgridItem(HWND grid, PROPGRIDITEM& item, StyleProperty& prop, int index)
{
	USES_CONVERSION;

	wchar_t str[512];

	wchar_t* propName = A2W(prop.LookupName());
	item.lpszPropName = propName;

	if (InitPropgridItemSpecial(item, prop))
		return;

	switch (prop.header.typeID)
	{
	case IDENTIFIER::FILENAME:
	case IDENTIFIER::FILENAME_LITE:
	case IDENTIFIER::DISKSTREAM:
	{
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)_itot(prop.GetResourceID(), str, 10);
	} break;
	case IDENTIFIER::ENUM:
	case IDENTIFIER::HIGHCONTRASTCOLORTYPE:
	{
		TCHAR* enumList = BuildEnumList(str, prop);
		if (enumList)
		{
			item.iItemType = PIT_COMBO;
			item.lpCurValue = (LPARAM)A2T(libmsstyle::lookup::GetEnumAsString(prop.header.nameID, prop.data.enumtype.enumvalue));
			item.lpszzCmbItems = enumList;
		}
		else
		{
			item.iItemType = PIT_EDIT;
			item.lpCurValue = (LPARAM)_itot(prop.data.enumtype.enumvalue, str, 10);
		}
	} break;
	case IDENTIFIER::SIZE:
	{
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)_itot(prop.data.sizetype.size, str, 10);
	} break;
	case IDENTIFIER::COLOR:
	{
		item.iItemType = PIT_COLOR;
		item.lpCurValue = (LPARAM)RGB(prop.data.colortype.r, prop.data.colortype.g, prop.data.colortype.b);
	} break;
	case IDENTIFIER::INT:
	{
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)_itot(prop.data.inttype.value, str, 10);
	} break;
	case IDENTIFIER::BOOLTYPE:
	{
		item.iItemType = PIT_CHECK;
		item.lpCurValue = (LPARAM)prop.data.booltype.boolvalue;
	} break;
	case IDENTIFIER::MARGINS:
	{
		wsprintf(str, L"%d, %d, %d, %d", prop.data.margintype.left, prop.data.margintype.top, prop.data.margintype.right, prop.data.margintype.bottom);
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)str;
	} break;
	case IDENTIFIER::RECTTYPE:
	{
		wsprintf(str, L"%d, %d, %d, %d", prop.data.recttype.left, prop.data.recttype.top, prop.data.recttype.right, prop.data.recttype.bottom);
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)str;
	} break;
	case IDENTIFIER::POSITION:
	{
		wsprintf(str, L"%d, %d", prop.data.positiontype.x, prop.data.positiontype.y);
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)str;
	} break;
	case IDENTIFIER::FONT:
	{
		TCHAR* fontList = BuildFontList(str, prop);
		if (fontList)
		{
			item.iItemType = PIT_COMBO;
			item.lpCurValue = (LPARAM)A2T(libmsstyle::lookup::FindFontName(prop.GetResourceID()).c_str());
			item.lpszzCmbItems = fontList;
		}
		else
		{
			item.iItemType = PIT_EDIT;
			item.lpCurValue = (LPARAM)_itot(prop.GetResourceID(), str, 10);
		}
	} break;
	case IDENTIFIER::STRING:
	{
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)W2T(&prop.data.texttype.firstChar);
	} break;
	case IDENTIFIER::INTLIST:
	{
		if (prop.data.intlist.numInts >= 3)
		{
			wsprintf(str, L"%d, %d, %d, .. (%d)", prop.data.intlist.numInts
				, *(&prop.data.intlist.firstInt + 0)
				, *(&prop.data.intlist.firstInt + 1)
				, *(&prop.data.intlist.firstInt + 2));
		}
		else wsprintf(str, L"Len: %d, Values omitted", prop.data.intlist.numInts);
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)str;
	} break;
	default:
	{
		item.iItemType = PIT_EDIT;
		item.lpCurValue = (LPARAM)_T("UNKNOWN");
	} break;
	}

	if (index < 0)
	{
		PropGrid_AddItem(grid, &item);
	}
	else
	{
		PropGrid_SetItemData(grid, index, &item);
	}
}