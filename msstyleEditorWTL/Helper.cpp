#include "stdafx.h"
#include "Helper.h"

#include <atlctrls.h>

#include "Controls\PropertyItemEditors.h"
#include "Controls\PropertyItemImpl.h"

#include "libmsstyle\Lookup.h"

#include <codecvt>	// codecvt_utf8_utf16
#include <locale>	// wstring_convert

using namespace libmsstyle;

std::string StdWideToUTF8(const std::wstring& str)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	return convert.to_bytes(str);
}

std::wstring StdUTF8ToWide(const std::string& str)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	return convert.from_bytes(str);
}

char* WideToUTF8(const wchar_t* str)
{
	if (str == nullptr)
		return nullptr;

	int size_needed = WideCharToMultiByte(CP_UTF8, 0, &str[0], -1, NULL, 0, NULL, NULL);
	
	char* resultBuffer = new char[size_needed];
	WideCharToMultiByte(CP_UTF8, 0, &str[0], -1, &resultBuffer[0], size_needed, NULL, NULL);
	return resultBuffer;
}

wchar_t* UTF8ToWide(const char* str)
{
	if (str == nullptr)
		return nullptr;

	int size_needed = MultiByteToWideChar(CP_UTF8, 0, &str[0], -1, NULL, 0);
	
	wchar_t* resultBuffer = new wchar_t[size_needed];
	MultiByteToWideChar(CP_UTF8, 0, &str[0], -1, &resultBuffer[0], size_needed);
	return resultBuffer;
}

HPROPERTY GetPropertyItemSpecialCases(StyleProperty& prop)
{
	if (prop.GetNameID() == IDENTIFIER::COLORIZATIONCOLOR)
	{
		std::wstring tmp = UTF8ToWide(prop.LookupName());
		HPROPERTY p = new CPropertyColorItem(tmp.c_str(), prop.data.inttype.value, (LPARAM)&prop);
		return p;
	}
	else return nullptr;
}

HPROPERTY GetEnumPropertyItem(libmsstyle::StyleProperty& prop)
{
	USES_CONVERSION;

	libmsstyle::lookup::EnumList result = libmsstyle::lookup::FindEnums(prop.header.nameID);
	if (result.enums == nullptr || result.numEnums == 0)
		return nullptr;

	LPCWSTR* list = new LPCWSTR[result.numEnums + 1];

	int i = 0;
	for ( ; i < result.numEnums; ++i)
	{
		
		list[i] = A2W(result.enums[i].value);
	}
	list[i] = NULL;

	HPROPERTY p = new CPropertyListItem(A2W(prop.LookupName()), list, 0, (LPARAM)&prop);

	delete[] list;
	return p;
}

HPROPERTY GetFontsPropertyItem(libmsstyle::StyleProperty& prop)
{
	USES_CONVERSION;

	LPCWSTR* list = new LPCWSTR[libmsstyle::FONT_MAP.size() + 1];

	int i = 0;
	auto it = libmsstyle::FONT_MAP.begin();

	for ( ; it != libmsstyle::FONT_MAP.end(); ++it, ++i)
	{
		list[i] = A2W(it->second);
	}
	list[i] = NULL;

	HPROPERTY p = new CPropertyListItem(A2W(prop.LookupName()), list, 0, (LPARAM)&prop);

	delete[] list;
	return p;
}

HPROPERTY GetPropertyItemFromStyleProperty(StyleProperty& prop)
{
	USES_CONVERSION;

	wchar_t str[64];

	const wchar_t* propName = A2W(prop.LookupName());

	HPROPERTY special = GetPropertyItemSpecialCases(prop);
	if (special != nullptr)
		return special;

	switch (prop.header.typeID)
	{
	case IDENTIFIER::FILENAME:
	case IDENTIFIER::FILENAME_LITE:
	case IDENTIFIER::DISKSTREAM:
	{
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(prop.GetResourceID()), (LPARAM)&prop);
		return p;
	}
	case IDENTIFIER::ENUM:
	case IDENTIFIER::HIGHCONTRASTCOLORTYPE:
	{
		HPROPERTY p = GetEnumPropertyItem(prop);
		if (p == nullptr)
		{
			p = new CPropertyEditItem(propName, CComVariant(prop.data.enumtype.enumvalue), (LPARAM)&prop);
		}
		return p;
	}
	case IDENTIFIER::SIZE:
	{
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(prop.data.sizetype.size), (LPARAM)&prop);
		return p;
	}
	case IDENTIFIER::COLOR:
	{
		HPROPERTY p = new CPropertyColorItem(propName, RGB(prop.data.colortype.r, prop.data.colortype.g, prop.data.colortype.b), (LPARAM)&prop);
		return p;
	}
	case IDENTIFIER::INT:
	{
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(prop.data.inttype.value), (LPARAM)&prop);
		return p;
	}
	case IDENTIFIER::BOOLTYPE:
	{
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(prop.data.booltype.boolvalue), (LPARAM)&prop);
		return p;
	}
	case IDENTIFIER::MARGINS:
	{
		wsprintf(str, L"%d, %d, %d, %d", prop.data.margintype.left, prop.data.margintype.top, prop.data.margintype.right, prop.data.margintype.bottom);
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(str), (LPARAM)&prop);
		return p;
	}
	case IDENTIFIER::RECTTYPE:
	{
		wsprintf(str, L"%d, %d, %d, %d", prop.data.recttype.left, prop.data.recttype.top, prop.data.recttype.right, prop.data.recttype.bottom);
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(str), (LPARAM)&prop);
		return p;
	}
	case IDENTIFIER::POSITION:
	{
		wsprintf(str, L"%d, %d", prop.data.positiontype.x, prop.data.positiontype.y);
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(str), (LPARAM)&prop);
		return p;
	}
	case IDENTIFIER::FONT:
	{
		HPROPERTY p = GetFontsPropertyItem(prop);
		if (p == nullptr)
		{
			p = new CPropertyEditItem(propName, CComVariant(prop.GetResourceID()), (LPARAM)&prop);
		}
		return p;
	}
	case IDENTIFIER::STRING:
	{
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(&prop.data.texttype.firstChar), (LPARAM)&prop);
		return p;
	}
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
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(str), (LPARAM)&prop);
		return p;
	}
	default:
		HPROPERTY p = new CPropertyEditItem(propName, CComVariant(L"VALUE"), (LPARAM)&prop);
		return p;
	}
}