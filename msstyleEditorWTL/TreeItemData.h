#pragma once


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

