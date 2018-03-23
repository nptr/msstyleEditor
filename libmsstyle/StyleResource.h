#pragma once

namespace libmsstyle
{
	enum StyleResourceType
	{
		NONE,
		IMAGE,	// image
		ATLAS	// image atlas
	};

	class StyleResource
	{
	public:
		StyleResource();
		StyleResource(const void* data, int size, int nameId, StyleResourceType type);
		~StyleResource();

		StyleResource& operator=(const StyleResource&);
		bool operator==(const StyleResource& rhs) const;

		int GetNameID() const;
		StyleResourceType GetType() const;
		const char* GetData() const;
		int GetSize() const;

	private:
		class Impl;
		Impl* impl;
	};
}