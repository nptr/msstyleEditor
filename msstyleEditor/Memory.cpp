#include "Memory.h"

#include <vector>
#include <algorithm>
#include <exception>

#undef new

// list of "new()" callers
std::vector<Caller> memList;

// custom allocation to track memory
void* operator new(std::size_t n, const char* filename, int line)
{
	void* ptr = malloc(n);
	if (ptr == nullptr)
		throw std::bad_alloc();

	Caller c;
	c.line = line;
	c.size = n;
	c.name = filename;
	c.memory = ptr;

	memList.push_back(c);

	return ptr;
}

// custom allocation to track memory
void* operator new[](std::size_t n, const char* filename, int line)
{
	void* ptr = malloc(n);
	if (ptr == nullptr)
		throw std::bad_alloc();

	Caller c;
	c.line = line;
	c.size = n;
	c.name = filename;
	c.memory = ptr;

	memList.push_back(c);

	return ptr;
}

// custom deletion func to track memory - called by user
void operator delete(void * p)
{
	if (memList.empty())
		return;

	memList.erase(std::remove(memList.begin(), memList.end(), p), memList.end());
	free(p);
}

// custom deletion func to track memory - in case of exception this one is called
void operator delete(void * p, const char* filename, int line)
{
	if (memList.empty())
		return;

	memList.erase(std::remove(memList.begin(), memList.end(), p), memList.end());
	free(p);
}

void operator delete[](void * p)
{
	if (memList.empty())
		return;

	memList.erase(std::remove(memList.begin(), memList.end(), p), memList.end());
	free(p);
}

void printMemTrackResult()
{
	int leakedBytes = 0;

	printf("Memory Track Result\n-------------------\n");
	for (auto it = memList.begin(); it != memList.end(); ++it)
	{
		const char* name = strrchr(it->name, '\\');
		if (name++ == nullptr)
			name = it->name;

		leakedBytes += it->size;

		printf("leak on line: %d  file: %s\n",it->line, name);
	}
	printf("Leaked %d bytes.\n", leakedBytes);
}