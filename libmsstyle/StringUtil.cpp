#include "StringUtil.h"

#include <cstdarg>
#include <string>
#include <Windows.h>

namespace libmsstyle
{

	std::string WideToUTF8(const std::wstring& str)
	{
		int size_needed = WideCharToMultiByte(CP_UTF8, 0, str.c_str(), str.length(), NULL, 0, NULL, NULL);

		char* tmp = (char*)alloca(size_needed);
		WideCharToMultiByte(CP_UTF8, 0, &str[0], str.length(), tmp, size_needed, NULL, NULL);
		return std::string(tmp, size_needed);
	}

	std::wstring UTF8ToWide(const std::string& str)
	{
		int size_needed = MultiByteToWideChar(CP_UTF8, 0, str.c_str(), str.length(), NULL, 0);

		wchar_t* tmp = (wchar_t*)alloca(size_needed * sizeof(wchar_t));
		MultiByteToWideChar(CP_UTF8, 0, &str[0], str.length(), tmp, size_needed);
		return std::wstring(tmp, size_needed);
	}

	std::string format_string(const char* format, ...)
	{
		char textbuffer[256];

		va_list args;
		va_start(args, format);
		vsnprintf(textbuffer, sizeof(textbuffer), format, args);
		va_end(args);

		// force termination to guard against buggy and non-standard snprintf() impls.
		textbuffer[sizeof(textbuffer) - 1] = '\0';
		return textbuffer;
	}

}