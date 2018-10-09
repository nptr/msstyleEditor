#pragma once

#define ITEM_CLASS			1	
#define ITEM_PART			2
#define ITEM_STATE			3
#define ITEM_PROPERTY		4

struct TreeItemData
{
	TreeItemData(void* pObject, int objectType)
		: object(pObject)
		, type(objectType)
	{}

	~TreeItemData()
	{
		int xx = 0;
	}

	void* object;
	int type;
};

