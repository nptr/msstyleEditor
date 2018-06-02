#include "StringUtil.h"

#include <cstdarg>

std::string format_string(const char* format, ...)
{
	char textbuffer[128];

	va_list args;
	va_start(args, format);
	vsnprintf(textbuffer, sizeof(textbuffer), format, args);
	va_end(args);

	// force termination to guard against buggy and non-standard snprintf() impls.
	textbuffer[sizeof(textbuffer) - 1] = '\0';
	return textbuffer;
}