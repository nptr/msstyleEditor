#include "UiHelper.h"
#include "libmsstyle\VisualStyle.h"
#include "libmsstyle\VisualStyleEnums.h"
#include "libmsstyle\VisualStyleDefinitions.h"

#include <string>
#include <codecvt>	// codecvt_utf8_utf16
#include <locale>	// wstring_convert

using namespace libmsstyle;

wxPGProperty* GetWXPropertyFromMsStyleProperty(StyleProperty& prop)
{
	char* str = new char[32];
	const char* propName = prop.LookupName();

	switch (prop.typeID)
	{
	case IDENTIFIER::FILENAME:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.variants.imagetype.imageID);
		p->SetClientData(const_cast<void*>(static_cast<const void*>(&prop)));
		return p;
	}
	case IDENTIFIER::ENUM:
	{
		wxPGChoices* cp = GetEnumsFromMsStyleProperty(prop);
		wxPGProperty* p;
		
		if (cp != nullptr)
		{
			p = new wxEnumProperty(propName, *wxPGProperty::sm_wxPG_LABEL, *cp, prop.variants.enumtype.enumvalue);
			delete cp;
		}
		else
		{
			p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.variants.enumtype.enumvalue);
		}

		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::SIZE:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.variants.sizetype.size);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::COLOR:
	{
		wxColourProperty* p = new wxColourProperty(propName, *wxPGProperty::sm_wxPG_LABEL, wxColor(prop.variants.colortype.r, prop.variants.colortype.g, prop.variants.colortype.b));
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::INT:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.variants.inttype.value);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::BOOL:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.variants.booltype.boolvalue);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::MARGINS:
	{
		sprintf(str, "%d, %d, %d, %d", prop.variants.margintype.left, prop.variants.margintype.top, prop.variants.margintype.right, prop.variants.margintype.bottom);
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, str);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::RECT:
	{
		sprintf(str, "%d, %d, %d, %d", prop.variants.recttype.left, prop.variants.recttype.top, prop.variants.recttype.right, prop.variants.recttype.bottom);
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, str);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::POSITION:
	{
		sprintf(str, "%d, %d", prop.variants.positiontype.x, prop.variants.positiontype.y);
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, str);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::FONT:
	{
		wxIntProperty* p = new wxIntProperty("FONT (ID)", *wxPGProperty::sm_wxPG_LABEL, prop.variants.fonttype.fontID);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::STRING:
	{
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, &prop.variants.texttype.firstchar);
		p->SetClientData(&prop);
		return p;
	}
	case IDENTIFIER::INTLIST:
	{
		if (prop.variants.intlist.numints >= 3)
		{
			sprintf(str, "%d, %d, %d, .. (%d)", prop.variants.intlist.numints
				, *(&prop.variants.intlist.firstint + 0)
				, *(&prop.variants.intlist.firstint + 1)
				, *(&prop.variants.intlist.firstint + 2));
		}
		else sprintf(str, "Len: %d, Values omitted", prop.variants.intlist.numints);
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

	libmsstyle::lookup::EnumList result = libmsstyle::lookup::FindEnums(prop.nameID);
	if (result.enums == nullptr || result.numEnums == 0)
		return nullptr;

	for (int i = 0; i < result.numEnums; ++i)
	{
		choices->Add(result.enums[i].value, result.enums[i].key);
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