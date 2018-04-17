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


bool SaveReloadCompareTest(libmsstyle::VisualStyle& s1)
{
	// SAVE ORIGINAL
	std::string dstFile = "tmp.msstyle";
	s1.Save(dstFile, false);

	// RELOAD
	libmsstyle::VisualStyle s2;
	s2.Load(dstFile);

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
									// state not found
								}
							}
						}
						else
						{
							// state not found
						}
					}
				}
				else
				{
					// part not found
				}
			}
		}
		else
		{
			// class not found
		}
	}

	return true;
}


int _tmain(int argc, _TCHAR* argv[])
{
	const char* INPUT_FILES[] =
	{
		"W7.msstyle",
		"W8.msstyle",
		"W81.msstyle",
		"W10.msstyle",
		NULL
	};

	const char** file = INPUT_FILES;
	while (*file != NULL)
	{
		printfc(NORMAL, "%s\r\n", *file);

		libmsstyle::VisualStyle currentStyle;
		try
		{
			currentStyle.Load(*file);

			if (SaveReloadCompareTest(currentStyle))
				printfc(OK, "%s - SaveReloadCompareTest - OK\r\n", *file);
			else printfc(ERROR, "%s - SaveReloadCompareTest - FAILED\r\n", *file);
		}
		catch (std::exception& ex)
		{
			printfc(ERROR, "ERROR - %s - %s\r\n", *file, ex.what());
		}

		file++;
	}

	return 0;
}

