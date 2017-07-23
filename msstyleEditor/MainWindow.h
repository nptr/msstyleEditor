#pragma once

#include <unordered_map>

#include <wx\wx.h>
#include <wx\propgrid\propgrid.h>
#include <wx\propgrid\advprops.h>
#include <wx\treectrl.h>

#include "ImageViewCtrl.h"
#include "VisualStyle.h"
#include "SearchDialog.h"

class MainWindow : public wxFrame, public ISearchDialogListener
{
private:
	// The IDs for the menu items
	const int ID_FOPEN = 444;
	const int ID_FSAVE = 445;
	const int ID_IEXPORT = 446;
	const int ID_IREPLACE = 447;
	const int ID_ABOUT = 448;
	const int ID_HELP = 449;
	const int ID_EXPAND_TREE = 450;
	const int ID_COLLAPSE_TREE = 451;
	const int ID_RESOURCEDLG = 452;
	const int ID_THEMEFOLDER = 453;

	const int ID_FIND = 460;

	msstyle::VisualStyle* currentStyle;

	msstyle::EmbRessource selectedImage;
	const msstyle::MsStyleProperty* selectedImageProp;

protected:
	wxTreeCtrl* classView;
	wxStaticBitmap* imageView;
	wxPropertyGrid* propView;
	wxMenuBar* mainmenu;
	wxMenu* fileMenu;
	wxMenu* aboutMenu;
	wxMenu* imageMenu;
	wxMenu* viewMenu;
	wxStatusBar* statusBar;
	ImageViewCtrl* imgView;
	SearchDialog* searchDlg;

	void OnFileOpenMenuClicked(wxCommandEvent& event);
	void OnFileSaveMenuClicked(wxCommandEvent& event);
	void OnClassViewTreeSelChanged(wxTreeEvent& event);
	void OnPropertyGridChanging(wxPropertyGridEvent& event);
	void OnImageExportClicked(wxCommandEvent& event);
	void OnImageReplaceClicked(wxCommandEvent& event);
	void OnAboutClicked(wxCommandEvent& event);
	void OnHelpClicked(wxCommandEvent& event);
	void OnCollapseClicked(wxCommandEvent& event);
	void OnExpandClicked(wxCommandEvent& event);
	void OnResourceDlgClicked(wxCommandEvent& event);
	void OnOpenThemeFolder(wxCommandEvent& event);
	void OnFindHotkeyPressed(wxCommandEvent& event);

	wxTreeItemId FindNext(const std::string& str, wxTreeItemId node);

	void FillClassView(const std::unordered_map<int, msstyle::MsStyleClass*>* classes);
	void FillPropertyView(msstyle::MsStylePart& part);
	
	char* GetValueFromProperty(msstyle::MsStyleProperty& prop) const;

	void ShowImageFromResource(const msstyle::MsStyleProperty* prop);
	void ShowImageFromFile(wxString& imgPath);
public:
	MainWindow(wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxEmptyString, const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(800, 600), long style = wxDEFAULT_FRAME_STYLE | wxTAB_TRAVERSAL);
	~MainWindow();

	void OnFindNext(const std::string& toFind);

	void OpenStyle(const wxString& file);
	void CloseStyle();
};