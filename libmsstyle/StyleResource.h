#pragma once

namespace libmsstyle
{
	enum StyleResourceType
	{
		IMAGE,	// image
		ATLAS	// image atlas
	};

	class StyleResource
	{
	public:
		StyleResource();
		StyleResource(const void* data, int size, int nameId, StyleResourceType type);
		~StyleResource();

		int GetNameID();
		StyleResourceType GetType();
		const char* GetData();
		int GetSize();

	private:
		class Impl;
		Impl* impl;
	};
}