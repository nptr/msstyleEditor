#pragma once

#include <memory>

namespace libmsstyle
{
	enum StyleResourceType
	{
		rtNone,
		rtImage,	// image
		rtAtlas,	// image atlas
	};

	class StyleResource
	{
	public:
		StyleResource();
		StyleResource(const StyleResource& other);
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
		std::shared_ptr<Impl> impl;
	};
}