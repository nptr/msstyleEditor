#include <wx/dnd.h>


class CustomFileDropTarget : public wxFileDropTarget{
private:
	MainWindow& wnd;

	bool OnDropFiles(wxCoord x, wxCoord y, const wxArrayString &filenames)
	{
		if (filenames.IsEmpty())
			return false;

		wxString extension = wxFileName(filenames[0]).GetExt();
		if (extension.Contains("msstyles"))
		{
			wnd.CloseStyle();
			wnd.OpenStyle(filenames[0]);
			return true;
		}
		else return false;
	};

public:
	CustomFileDropTarget(MainWindow& window)
		: wnd(window)
	{
	};
};