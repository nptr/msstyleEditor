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
	msstyle::EnumMap* enums;
	int size;

	if (prop.nameID == IDENTIFIER::BGTYPE)
	{
		enums = (EnumMap*)&msstyle::ENUM_BGTYPE;
		size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_BGTYPE);
	}
	else if (prop.nameID == IDENTIFIER::BORDERTYPE)
	{
		enums = (EnumMap*)&msstyle::ENUM_BORDERTYPE;
		size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_BORDERTYPE);
	}
	else if (prop.nameID == IDENTIFIER::FILLTYPE)
	{
		enums = (EnumMap*)&msstyle::ENUM_FILLTYPE;
		size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_FILLTYPE);
	}
	else if (prop.nameID == IDENTIFIER::SIZINGTYPE)
	{
		enums = (EnumMap*)&msstyle::ENUM_SIZINGTYPE;
		size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_SIZINGTYPE);
	}
	else if (prop.nameID == IDENTIFIER::CONTENTALIGNMENT)
	{
		enums = (EnumMap*)&msstyle::ENUM_ALIGNMENT;
		size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_ALIGNMENT);
	}
	else if (prop.nameID == IDENTIFIER::IMAGELAYOUT)
	{
		enums = (EnumMap*)&msstyle::ENUM_IMAGELAYOUT;
		size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_IMAGELAYOUT);
	}
	else if (prop.nameID == IDENTIFIER::ICONEFFECT)
	{
		enums = (EnumMap*)&msstyle::ENUM_ICONEFFECT;
		size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_ICONEFFECT);
	}
	else if (prop.nameID == IDENTIFIER::GLYPHTYPE)
	{
		enums = (EnumMap*)&msstyle::ENUM_GLYPHTYPE;
		size = MSSTYLE_ARRAY_LENGTH(msstyle::ENUM_GLYPHTYPE);
	}
	else return nullptr;

	for (int i = 0; i < size; ++i){
		choices->Add(enums[i].value, enums[i].key);
	}

	return choices;
}