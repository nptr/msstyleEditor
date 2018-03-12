#pragma once

#include <string>
#include <codecvt>	// codecvt_utf8_utf16
#include <locale>	// wstring_convert

inline static std::string WideToUTF8(const std::wstring& str)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	return convert.to_bytes(str);
}

inline static std::wstring UTF8ToWide(const std::string& str)
{
	std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>, wchar_t> convert;
	return convert.from_bytes(str);
}

