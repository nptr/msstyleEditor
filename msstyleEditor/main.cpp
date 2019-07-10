#include <wx/wx.h>
#include "MainWindow.h"

class wxMyApp : public wxApp
{
public:
	virtual bool OnInit();
};

IMPLEMENT_APP(wxMyApp)
bool wxMyApp::OnInit()
{
	wxDisableAsserts();
	wxImage::AddHandler(new wxPNGHandler());

	MainWindow* frame = new MainWindow(NULL, wxID_ANY, wxT("msstyleEditor"), wxDefaultPosition, wxSize(900, 600));
	frame->Show(true);

	if (wxApp::argc > 1)
	{
		if (wxFile::Exists(wxApp::argv[1]))
		{
			frame->OpenStyle(wxApp::argv[1]);
		}
	}

	return true;
}