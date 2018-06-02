#include "UiHelper.h"
#include "libmsstyle\VisualStyle.h"
#include "libmsstyle\VisualStyleEnums.h"
#include "libmsstyle\VisualStyleDefinitions.h"

#include <wx\menu.h>
#include <wx\propgrid\property.h>
#include <wx\propgrid\advprops.h>

#include <string>
#include <codecvt>	// codecvt_utf8_utf16
#include <locale>	// wstring_convert


using namespace libmsstyle;

wxPGProperty* GetWXPropertySpecialProps(StyleProperty& prop)
{
	if (prop.GetNameID() == IDENTIFIER::COLORIZATIONCOLOR)
	{
		// ARGB -> wxColor(R,G,B,A)
		wxColor col((prop.data.inttype.value >> 16) & 0xFF,
			(prop.data.inttype.value >> 8) & 0xFF,
			(prop.data.inttype.value >> 0) & 0xFF,
			(prop.data.inttype.value >> 24) & 0xFF);

		wxColourProperty* p = new wxColourProperty(prop.LookupName(), *wxPGProperty::sm_wxPG_LABEL, col);
		p->SetClientData(&prop);
		return p;
	}
	else return nullptr;
}

wxPGProperty* GetWXPropertyFromMsStyleProperty(StyleProperty& prop)
{
	char str[64];
	const char* propName = prop.LookupName();

	wxPGProperty* special = GetWXPropertySpecialProps(prop);
	if (special != nullptr)
		return special;

	switch (prop.header.typeID)
	{
	case IDENTIFIER::FILENAME:
	case IDENTIFIER::FILENAME_LITE:
	case IDENTIFIER::DISKSTREAM:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.GetResourceID());
		p->SetClientData(const_cast<void*>(static_cast<const void*>(&prop)));
		return p;
	}
	case IDENTIFIER::ENUM:
	case IDENTIFIER::HIGHCONTRASTCOLORTYPE:
	{
		wxPGChoices* cp = GetEnumsFromMsStyleProperty(prop);
		wxPGProperty* p;
		
		if (cp != nullptr)
		{
			p = new wxEnumProperty(propName, *wxPGProperty::sm_wxPG_LABEL, *cp, prop.data.enumtype.enumvalue);
			delete cp;
		}
		else
		{
			p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.data.enumtype.enumvalue);
		}

		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::SIZE:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.data.sizetype.size);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::COLOR:
	{
		wxColourProperty* p = new wxColourProperty(propName, *wxPGProperty::sm_wxPG_LABEL, wxColor(prop.data.colortype.r, prop.data.colortype.g, prop.data.colortype.b));
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::INT:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.data.inttype.value);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::BOOL:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.data.booltype.boolvalue);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::MARGINS:
	{
		sprintf(str, "%d, %d, %d, %d", prop.data.margintype.left, prop.data.margintype.top, prop.data.margintype.right, prop.data.margintype.bottom);
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, str);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::RECT:
	{
		sprintf(str, "%d, %d, %d, %d", prop.data.recttype.left, prop.data.recttype.top, prop.data.recttype.right, prop.data.recttype.bottom);
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, str);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::POSITION:
	{
		sprintf(str, "%d, %d", prop.data.positiontype.x, prop.data.positiontype.y);
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, str);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::FONT:
	{
		wxPGChoices* cp = GetFontsFromMsStyleProperty(prop);
		wxPGProperty* p;

		if (cp != nullptr)
		{
			p = new wxEnumProperty(propName, *wxPGProperty::sm_wxPG_LABEL, *cp, prop.GetResourceID());
			delete cp;
		}
		else
		{
			p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.GetResourceID());
		}

		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::STRING:
	{
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, &prop.data.texttype.firstChar);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::INTLIST:
	{
		if (prop.data.intlist.numInts >= 3)
		{
			sprintf(str, "%d, %d, %d, .. (%d)", prop.data.intlist.numInts
				, *(&prop.data.intlist.firstInt + 0)
				, *(&prop.data.intlist.firstInt + 1)
				, *(&prop.data.intlist.firstInt + 2));
		}
		else sprintf(str, "Len: %d, Values omitted", prop.data.intlist.numInts);
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, str);
		p->SetClientData(&prop);
		return p;
	}
	default:
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, "VALUE");
		p->SetClientData(&prop);
		return p;
	}
}

wxPGChoices* GetEnumsFromMsStyleProperty(libmsstyle::StyleProperty& prop)
{
	wxPGChoices* choices = new wxPGChoices();

	libmsstyle::lookup::EnumList result = libmsstyle::lookup::FindEnums(prop.header.nameID);
	if (result.enums == nullptr || result.numEnums == 0)
		return nullptr;

	for (int i = 0; i < result.numEnums; ++i)
	{
		choices->Add(result.enums[i].value, result.enums[i].key);
	}

	return choices;
}

wxPGChoices* GetFontsFromMsStyleProperty(libmsstyle::StyleProperty& prop)
{
	wxPGChoices* choices = new wxPGChoices();

	for (auto it = libmsstyle::FONT_MAP.begin(); it != libmsstyle::FONT_MAP.end(); ++it)
	{
		choices->Add(it->second, it->first);
	}

	return choices;
}

std::string WideToUTF8(const std::wstring& str)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	return convert.to_bytes(str);
}

std::wstring UTF8ToWide(const std::string& str)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	return convert.from_bytes(str);
}