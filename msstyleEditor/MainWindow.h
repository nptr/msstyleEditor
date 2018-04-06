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
	static const int ID_FOPEN = 444;
	static const int ID_FSAVE = 445;
	static const int ID_IEXPORT = 446;
	static const int ID_IREPLACE = 447;
	static const int ID_ABOUT = 448;
	static const int ID_HELP = 449;
	static const int ID_EXPAND_TREE = 450;
	static const int ID_COLLAPSE_TREE = 451;
	static const int ID_RESOURCEDLG = 452;
	static const int ID_THEMEFOLDER = 453;
	static const int ID_BG_WHITE = 454;
	static const int ID_BG_GREY = 455;
	static const int ID_BG_BLACK = 456;
	static const int ID_BG_CHESS = 457;
	static const int ID_EXPORT_TREE = 458;
	static const int ID_THEME_APPLY = 459;
	static const int ID_FIND = 460;
	static const int ID_PROP_CREATE = 461;
	static const int ID_PROP_DELETE = 462;

	static const int ID_PROP_BASE = 2000;

	libmsstyle::VisualStyle* currentStyle;

	libmsstyle::StyleResource selectedImage;
	const libmsstyle::StyleProperty* selectedImageProp;

protected:
	wxTreeCtrl* classView;
	ImageViewCtrl* imageView;
	wxPropertyGrid* propView;
	wxMenu* imageViewMenu;
	wxMenu* propContextMenu;
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
	void OnPropertyGridItemRightClick(wxContextMenuEvent& event);
	void OnPropertyGridItemDelete(wxCommandEvent& event);
	void OnPropertyGridItemCreate(wxCommandEvent& event);
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
	void OnImageViewBgSelect(wxCommandEvent& event);
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