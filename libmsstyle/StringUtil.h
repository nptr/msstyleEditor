#pragma once

#include <string>

namespace libmsstyle
{
    std::string WideToUTF8(const std::wstring& str);
    std::wstring UTF8ToWide(const std::string& str);

    std::string format_string(const char* format, ...);
}
