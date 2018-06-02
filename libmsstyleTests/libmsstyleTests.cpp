#include "stdafx.h"

#include "libmsstyle\VisualStyle.h"
#include <cstdarg>

#define NORMAL	(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE)
#define OK		(FOREGROUND_GREEN)
#define ERROR	(FOREGROUND_RED)

void printfc(WORD color, const char* format, ...)
{
	SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), color);
	
	va_list args;
	va_start(args, format);
	vprintf(format, args);
	va_end(args);
}


bool CompareStyles(libmsstyle::VisualStyle& s1, libmsstyle::VisualStyle& s2)
{
	bool result = true;

	// CHECK EVERY SINGLE CLASS, PART, STATE AND PROPERTY
	// IF IT WAS SAVED AND IS AVAILABLE IN THE NEW STYLE

	// for all classes in the original style
	for (auto& cls = s1.begin(); cls != s1.end(); ++cls)
	{
		// see if there is one in the reloaded one
		libmsstyle::StyleClass* fcls = s2.FindClass(cls->first);
		if (fcls)
		{
			// foreach part in the original classes
			for (auto& part = cls->second.begin(); part != cls->second.end(); ++part)
			{
				// see if there is an equivalent one in the reloaded classes
				libmsstyle::StylePart* fpart = fcls->FindPart(part->first);
				if (fpart)
				{
					// foreach state in all original parts
					for (auto& state = part->second.begin(); state != part->second.end(); ++state)
					{
						// see if it exists in the reloaded parts as well
						libmsstyle::StyleState* fstate = fpart->FindState(state->second.stateID);
						if (fstate)
						{
							// foreach properties in all original states
							for (auto& prop = state->second.begin(); prop != state->second.end(); ++prop)
							{
								// see if it exists in the reloaded states as well
								libmsstyle::StyleProperty* fprop = fstate->FindPropertyByValue(**prop);
								if (fprop)
								{
									// we are fine; nothing todo
								}
								else
								{
									result = false;
									printfc(ERROR, "Missing property [N: %d, T: %d] in\r\n", (**prop).header.nameID, (**prop).header.typeID);
									printfc(ERROR, "State %d: %s\r\n", state->second.stateID, state->second.stateName);
									printfc(ERROR, "Part %d: %s\r\n", part->second.partID, part->second.partName);
									printfc(ERROR, "Class %d: %s\r\n", cls->second.classID, cls->second.className);
								}
							}
						}
						else
						{
							result = false;
							printfc(ERROR, "Missing state %d: %s, in\r\n", state->second.stateID, state->second.stateName);
							printfc(ERROR, "Part %d: %s\r\n", part->second.partID, part->second.partName);
							printfc(ERROR, "Class %d: %s\r\n", cls->second.classID, cls->second.className);
						}
					}
				}
				else
				{
					result = false;
					printfc(ERROR, "Missing part %d: %s, in\r\n", part->second.partID, part->second.partName);
					printfc(ERROR, "Class %d: %s\r\n", cls->second.classID, cls->second.className);
				}
			}
		}
		else
		{
			result = false;
			printfc(ERROR, "Missing class %d: %s\r\n", cls->second.classID, cls->second.className);
		}
	}

	return result;
}


bool SaveReloadCompareTest(libmsstyle::VisualStyle& s1)
{
	// SAVE IN A NEW ORDER
	std::string dstFile = "tmp.msstyle";
	s1.Save(dstFile);

	// RELOAD
	libmsstyle::VisualStyle s2;
	s2.Load(dstFile);

	return CompareStyles(s1, s2);
}



int _tmain(int argc, _TCHAR* argv[])
{
	const char* INPUT_DIR[] =
	{
		"W7",
		"W8",
		"W81",
		"W10",
		NULL
	};

	

	libmsstyle::Platform EXPECTED_PLATFORMS[] =
	{
		libmsstyle::Platform::WIN7,
		libmsstyle::Platform::WIN8,
		libmsstyle::Platform::WIN81,
		libmsstyle::Platform::WIN10,
	};

	WIN32_FIND_DATAA ffd;
	const char* dir = NULL;
	char filter[128];
	char fileName[128];

	for (int i = 0; INPUT_DIR[i] != NULL; ++i)
	{
		dir = INPUT_DIR[i];
		printfc(NORMAL, "\r\nDirectory: %s\r\n", dir);

		sprintf(filter, "%s\\*.msstyles", dir);
		HANDLE hFirst = FindFirstFileA(filter, &ffd);
		if (hFirst == INVALID_HANDLE_VALUE)
		{
			printfc(NORMAL, "Directory not found %s\r\n", dir);
			continue;
		}

		do
		{
			if (!(ffd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)) // just files
			{
				sprintf(fileName, "%s\\%s", dir, ffd.cFileName);

				printfc(NORMAL, "Testing with: %s\r\n", fileName);

				libmsstyle::VisualStyle currentStyle;

				//
				// Save and Reload
				//
				try
				{
					currentStyle.Load(fileName);

					if (SaveReloadCompareTest(currentStyle))
						printfc(OK, "%s - Save and Reload - OK\r\n", fileName);
					else printfc(ERROR, "%s - Save and Reload - FAILED\r\n", fileName);
				}
				catch (std::exception& ex)
				{
					printfc(ERROR, "ERROR - %s\r\n", ex.what());
				}

				//
				// Platform Determination
				//
				if (currentStyle.GetCompatiblePlatform() == EXPECTED_PLATFORMS[i])
					printfc(OK, "%s - DeterminePlatformTest - OK\r\n", fileName);
				else printfc(ERROR, "%s - DeterminePlatformTest - FAILED\r\n", fileName);
			}
		}
		while (FindNextFileA(hFirst, &ffd) != 0);
	}

	getchar();
	return 0;
}

