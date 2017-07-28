#pragma once

#include <string>

class SearchProperties
{
public:
	static const int MODE_NAME = 0;
	static const int MODE_PROPERTY = 1;

	std::string value;
	int mode;
	int type;
};

class ISearchDialogListener
{
public:
	virtual void OnFindNext(const SearchProperties& props) = 0;
};

