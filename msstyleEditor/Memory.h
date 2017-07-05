#pragma once

#include <cstddef>

// stores information about the caller
// of the allocation function
class Caller
{
public:
	bool operator == (void* mem)
	{
		if (memory == mem)
			return true;
		else return false;
	}

	int line;
	int size;
	const char* name;
	void* memory;
};

void printMemTrackResult();

void* operator new(std::size_t n, const char* filename, int line);

void operator delete(void * p);
void operator delete(void * p, const char* filename, int line);
void operator delete[](void * p);

#define new new(__FILE__,__LINE__)