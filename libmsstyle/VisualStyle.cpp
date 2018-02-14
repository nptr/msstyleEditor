#include "VisualStyle.h"
#include "StringConvert.h"


VisualStyle::VisualStyle()
{
}


VisualStyle::~VisualStyle()
{
}


int VisualStyle::GetClassCount()
{
	return m_classes.size();
}


const StyleClass* VisualStyle::GetClass(int index)
{
	return &(m_classes.at(index));
}

void VisualStyle::Load(const std::string& path)
{
	m_stylePath = path;
	m_moduleHandle = OpenModule(path);
	if (m_moduleHandle != 0)
	{
		Resource cmap = GetResource(m_moduleHandle, L"CMAP", L"CMAP");
		LoadClassmap(cmap);

		Resource pmap = GetResource(m_moduleHandle, L"NORMAL", L"VARIANT");
		LoadProperties(pmap);
	}


	// open CMAP
	// populate the m_classes map
	// log

	// open VARIANT
	// populate the rest


}

void VisualStyle::Save(const std::string& path)
{

}

void VisualStyle::LoadClassmap(Resource classResource)
{
	int first = 0;
	int last = 1;
	int numFound = 0;

	const wchar_t* data = static_cast<const wchar_t*>(classResource.data);
	int numChars = classResource.size / 2;

	for (int i = 0; i < numChars; ++i, ++last)
	{
		if (data[i] == 0)
		{
			// we found the terminator and
			// have a non-empty string
			if(last - first > 1)
			{
				StyleClass cls;
				cls.classID = numFound;
				cls.className = WideToUTF8(data + first);
				m_classes[numFound] = cls;
				numFound++;
			}

			first = last;
		}
	}
}

void VisualStyle::LoadProperties(Resource propResource)
{

}