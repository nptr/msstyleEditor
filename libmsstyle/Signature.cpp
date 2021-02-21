#include "Signature.h"
#include "StringUtil.h"

#include <stdio.h>
#include <stdint.h>

#define SIGNATURE_TOTAL 144
#define SIGNATURE_SIZE 128
#define SIGNATURE_CONST1 0x84692426

struct StyleSignatureHeader
{
	uint32_t const1;
	uint32_t sigSize;
	uint32_t fileSize;
	uint32_t v4;
};


namespace libmsstyle
{
	bool ReadSignatureFromHandle(FILE* handle, StyleSignature* s)
	{
		if (fseek(handle, 0, SEEK_END) != 0)
			return false;

		long size = ftell(handle);

		if (fseek(handle, size - sizeof(StyleSignatureHeader), SEEK_SET) != 0)
			return false;

		StyleSignatureHeader hdr;
		if (fread((void*)&hdr, 1, sizeof(StyleSignatureHeader), handle) != sizeof(StyleSignatureHeader))
			return false;
		if (hdr.const1 != SIGNATURE_CONST1)
			return false;
		if (hdr.fileSize != size) // i have seen a file where this isn't true..what to do?
			return false;
		if (hdr.sigSize > UINT16_MAX) // probably garbage
			return false;
		if (fseek(handle, size - sizeof(StyleSignatureHeader) - hdr.sigSize, SEEK_SET) != 0)
			return false;
		if (fread((void*)s, 1, sizeof(StyleSignature), handle) != sizeof(StyleSignature))
			return false;

		return true;
	}


	bool Signature::ReadSignature(const std::string& file, StyleSignature* s)
	{
		std::wstring wfile = UTF8ToWide(file);
		FILE* handle = _wfopen(wfile.c_str(), L"rb");
		if (handle == NULL)
			return false;

		bool res = ReadSignatureFromHandle(handle, s);

		fclose(handle);
		return res;
	}


	bool Signature::WriteSignature(const std::string& file, const StyleSignature* s)
	{
		std::wstring wfile = UTF8ToWide(file);
		FILE* handle = _wfopen(wfile.c_str(), L"r+b");
		if (handle == NULL)
			return false;

		StyleSignature tmp;
		int writeOffset = ReadSignatureFromHandle(handle, &tmp) ? -144 : 0;

		if (fseek(handle, writeOffset, SEEK_END) != 0)
		{
			fclose(handle);
			return false;
		}

		long size = ftell(handle);

		StyleSignatureHeader hdr = { 0 };
		hdr.const1 = SIGNATURE_CONST1;
		hdr.sigSize = SIGNATURE_SIZE;
		hdr.fileSize = size + sizeof(StyleSignatureHeader) + sizeof(StyleSignature);

		fwrite((const void*)s, 1, sizeof(StyleSignature), handle);
		fwrite((const void*)&hdr, 1, sizeof(StyleSignatureHeader), handle);

		fclose(handle);
		return true;
	}
}