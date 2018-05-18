#pragma once

#include <string>

#define MAX_THEMECHARS 255
#define MAX_COLORCHARS 128
#define MAX_SIZECHARS 128

namespace libmsstyle
{
	class VisualStyle;
}

class ThemeManager
{
public:
	ThemeManager();
	~ThemeManager();

	void ApplyTheme(libmsstyle::VisualStyle& style);
	void Rollback();

	void GetActiveTheme(std::string& theme, std::string& color, std::string& size);

private:
	bool m_valid;
	bool m_custom;
	wchar_t m_customTheme[MAX_THEMECHARS];
	wchar_t m_prevTheme[MAX_THEMECHARS];
	wchar_t m_prevColor[MAX_COLORCHARS];
	wchar_t m_prevSize[MAX_SIZECHARS];
};

