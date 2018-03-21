#pragma once

#include "StylePart.h"

#include <string>
#include <memory>

#include <stdint.h>

namespace libmsstyle
{

	class StyleClass
	{
	public:
		typedef std::unordered_map<int, StylePart>::iterator PartIterator;

		StyleClass();
		~StyleClass();

		StylePart* AddPart(const StylePart& part);
		StylePart* FindPart(int partId);
		size_t GetPartCount();

		PartIterator begin();
		PartIterator end();

		int32_t classID;
		std::string className;

	private:
		class Impl;
		std::shared_ptr<Impl> impl;
	};

}
