#pragma once

#include <string>

namespace libmsstyle
{
	struct StyleSignature
	{
		char signature[128];
	};

	class Signature
	{
	public:
		static bool ReadSignature(const std::string& file, StyleSignature* s);
		static bool WriteSignature(const std::string& file, const StyleSignature* s);
	};
}