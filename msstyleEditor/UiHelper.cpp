#include "UiHelper.h"
#include "VisualStyleEnums.h"

using namespace msstyle;

wxPGProperty* GetWXPropertyFromMsStyleProperty(MsStyleProperty& prop)
{
	char* str = new char[32];
	const char* propName = VisualStyle::FindPropName(prop.nameID);

	switch (prop.typeID)
	{
	case IDENTIFIER::FILENAME:
	{
		wxIntProperty* p = new wxIntProperty(propName, *wxPGProperty::sm_wxPG_LABEL, prop.variants.imagetype.imageID);
		p->SetClientData(&prop);
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
	default:
		wxStringProperty* p = new wxStringProperty(propName, *wxPGProperty::sm_wxPG_LABEL, "VALUE");
		p->SetClientData(&prop);
		return p;
	}
}

wxPGChoices* GetEnumsFromMsStyleProperty(msstyle::MsStyleProperty& prop)
{
	wxPGChoices* choices = new wxPGChoices();
	int size;
	msstyle::EnumMap* enums = VisualStyle::GetEnumMapFromNameID(prop.nameID, &size);

	if (enums == nullptr || size == 0)
		return nullptr;

	for (int i = 0; i < size; ++i){
		choices->Add(enums[i].value, enums[i].key);
	}

	return choices;
}