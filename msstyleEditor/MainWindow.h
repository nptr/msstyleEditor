#pragma once

#include <unordered_map>

#include <wx\wx.h>
#include <wx\propgrid\propgrid.h>
#include <wx\propgrid\advprops.h>
#include <wx\treectrl.h>

#include "ImageViewCtrl.h"
#include "libmsstyle\VisualStyle.h"
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
	const int ID_BG_WHITE = 454;
	const int ID_BG_GREY = 455;
	const int ID_BG_BLACK = 456;
	const int ID_BG_CHESS = 457;
	const int ID_EXPORT_TREE = 458;
	const int ID_THEME_APPLY = 459;

	const int ID_FIND = 460;

	libmsstyle::VisualStyle* currentStyle;

	libmsstyle::EmbRessource selectedImage;
	const libmsstyle::StyleProperty* selectedImageProp;

protected:
	wxTreeCtrl* classView;
	ImageViewCtrl* imageView;
	wxPropertyGrid* propView;
	wxMenu* imageViewMenu;
	wxMenuBar* mainmenu;
	wxMenu* fileMenu;
	wxMenu* themeMenu;
	wxMenu* aboutMenu;
	wxMenu* imageMenu;
	wxMenu* viewMenu;
	wxStatusBar* statusBar;
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
	void OnOpenSearchDlg(wxCommandEvent& event);
	void OnImageViewContextMenuTriggered(wxContextMenuEvent& event);
	void OnImageViewBgWhite(wxCommandEvent& event);
	void OnImageViewBgGrey(wxCommandEvent& event);
	void OnImageViewBgBlack(wxCommandEvent& event);
	void OnImageViewBgChess(wxCommandEvent& event);
	void OnExportLogicalStructure(wxCommandEvent& event);
	void OnThemeApply(wxCommandEvent& event);

	wxTreeItemId FindNext(const SearchProperties& props, wxTreeItemId node);

	void FillClassView();
	void FillPropertyView(libmsstyle::StylePart& part);
	
	char* GetValueFromProperty(libmsstyle::StyleProperty& prop) const;

	void ShowImageFromResource(const libmsstyle::StyleProperty* prop);
	void ShowImageFromFile(wxString& imgPath);
public:
	MainWindow(wxWindow* parent, wxWindowID id = wxID_ANY, const wxString& title = wxEmptyString, const wxPoint& pos = wxDefaultPosition, const wxSize& size = wxSize(800, 600), long style = wxDEFAULT_FRAME_STYLE | wxTAB_TRAVERSAL);
	~MainWindow();

	void OnFindNext(const SearchProperties& search);

	void OpenStyle(const wxString& file);
	void CloseStyle();
};