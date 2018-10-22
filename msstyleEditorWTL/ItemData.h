#pragma once

#define ITEM_CLASS			1	
#define ITEM_PART			2
#define ITEM_STATE			3
#define ITEM_PROPERTY		4

struct ItemData
{
	ItemData(void* pObject, int objectType)
		: object(pObject)
		, type(objectType)
	{}

	ItemData()
	{
		int xx = 0;
	}

	void* object;
	int type;
};

