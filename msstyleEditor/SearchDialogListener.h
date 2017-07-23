#pragma once

#include <string>

class ISearchDialogListener
{
public:
	virtual void OnFindNext(const std::string& toFind) = 0;
};

