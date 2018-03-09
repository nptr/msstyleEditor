#pragma once

#include "StylePart.h"

#include <string>
#include <stdint.h>

namespace libmsstyle
{

	class StyleClass
	{
	public:
		StyleClass();
		~StyleClass();

		StylePart* AddPart(const StylePart& part);

		int GetPartCount();
		StylePart* GetPart(int index);
		StylePart* FindPart(int partId);

		int32_t classID;
		std::string className;

	private:
		class Impl;
		Impl* impl;
	};

}
