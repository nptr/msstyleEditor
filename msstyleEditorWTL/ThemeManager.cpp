#include "ThemeManager.h"
#include "UxthemeUndocumented.h"

#include "libmsstyle\VisualStyle.h"

#include <shlobj.h>	// SHGetKnownFolderPath()

#include <string>
#include <codecvt>	// codecvt_utf8_utf16
#include <locale>	// wstring_convert


static std::string WideToUTF8(const std::wstring& str)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	return convert.to_bytes(str);
}

static std::wstring UTF8ToWide(const std::string& str)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	return convert.from_bytes(str);
}


ThemeManager::ThemeManager()
	: m_valid(true)
	, m_usertheme(false)
{
	if (uxtheme::GetCurrentThemeName(m_prevTheme, MAX_THEMECHARS,
		m_prevColor, MAX_COLORCHARS,
		m_prevSize, MAX_SIZECHARS) != S_OK)
	{
		m_valid = false;
	}

	srand(GetTickCount());
}


ThemeManager::~ThemeManager()
{
	try {
		Rollback();
	}
	catch (...)
	{ }
}

void ThemeManager::ApplyTheme(libmsstyle::VisualStyle& style)
{
	if (!m_valid)
		return;

	// Applying a style from any folder in /Users/<currentuser>/ breaks Win8.1 badly. (Issue #9)
	// This means /Desktop/, /AppData/, /Temp/, etc. cannot be used; weird...
	// A writeable alternative that doesn't crash Win8.1 is anything inside /Users/Public/.
	// TODO: What does happen when being logged in as "Public" ?
	wchar_t* publicFolder = nullptr;
	if (SHGetKnownFolderPath(FOLDERID_PublicDocuments, KF_FLAG_DEFAULT, NULL, &publicFolder) != S_OK)
	{
		throw std::runtime_error("SHGetKnownFolderPath(FOLDERID_PublicDocuments) failed!");
	}

	int rnum = rand() % 10000;

	wchar_t newPath[MAX_THEMECHARS];
	swprintf(newPath, MAX_THEMECHARS, L"%s\\tmp%05d.msstyles", publicFolder, rnum);
	CoTaskMemFree(publicFolder);

	std::wstring newPathUTF16(newPath);
	std::string newPathUTF8 = WideToUTF8(newPathUTF16);

	style.Save(newPathUTF8);

	// I saw the following values being recommended for the fourth parameter: 0, 32, 33, 65
	HRESULT res = uxtheme::SetSystemTheme(newPathUTF16.c_str(), L"NormalColor", L"NormalSize", 0);
	if (res != S_OK)
	{
		char textbuffer[512];
		sprintf(textbuffer, "Failed to apply the theme as the OS rejected it!\r\n\r\n"
			"SetSystemVisualStyle() returned 0x%x.", res);

		DeleteFileW(newPath);
		throw std::runtime_error(textbuffer);
	}
	else
	{
		// Remove the previous temporary theme now that it's not
		// in use by the OS anymore.
		if (m_usertheme)
		{
			DeleteFileW(m_customTheme);
		}

		// Update state
		memcpy(m_customTheme, newPath, MAX_THEMECHARS);
		m_usertheme = true;
	}
}

void ThemeManager::Rollback()
{
	if (!m_valid) // invalid state
		return;

	if (!m_usertheme) // same theme as when we started
		return;

	if (uxtheme::SetSystemTheme(m_prevTheme, m_prevColor, m_prevSize, 0) != S_OK)
	{
		throw std::runtime_error("Failed to switch back to the previous theme!");
	}

	DeleteFileW(m_customTheme);
	m_usertheme = false;
}

void ThemeManager::GetActiveTheme(std::string& theme, std::string& color, std::string& size)
{
	wchar_t actTheme[255];
	wchar_t actColor[255];
	wchar_t actSize[255];

	if (uxtheme::GetCurrentThemeName(actTheme, 255, actColor, 255, actSize, 255) == S_OK)
	{
		theme = WideToUTF8(actTheme);
		color = WideToUTF8(actColor);
		size = WideToUTF8(actSize);
	}
}

bool ThemeManager::IsThemeInUse() const
{
	return m_usertheme;
}