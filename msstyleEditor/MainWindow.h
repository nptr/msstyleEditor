#pragma once

#include "ImageViewCtrl.h"
#include "SearchDialog.h"
#include "ThemeManager.h"

#include "libmsstyle\VisualStyle.h"

#include <unordered_map>

#include <wx\wx.h>
#include <wx\propgrid\propgrid.h>
#include <wx\propgrid\advprops.h>
#include <wx\splitter.h>
#include <wx\treectrl.h>
#include <wx\aui\aui.h>

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

	static const int ID_BG_FIRST = 454;
	static const int ID_BG_WHITE = 454;
	static const int ID_BG_GREY = 455;
	static const int ID_BG_BLACK = 456;
	static const int ID_BG_CHESS = 457;
	static const int ID_BG_LAST = 457;

	static const int ID_EXPORT_TREE = 458;
	static const int ID_THEME_APPLY = 459;
	static const int ID_FIND = 460;
	static const int ID_PROP_CREATE = 461;
	static const int ID_PROP_DELETE = 462;
	static const int ID_STRING_RESOURCES = 463;

	static const int ID_PANE_FIRST = 464;
	static const int ID_PANE_CLASSVIEW = 464;
	static const int ID_PANE_IMAGEVIEW = 465;
	static const int ID_PANE_PROPVIEW = 466;
	static const int ID_PANE_LAST = 466;

	libmsstyle::VisualStyle* currentStyle;
	libmsstyle::StyleResource selectedImage;
	const libmsstyle::StyleProperty* selectedImageProp;
	ThemeManager themeManager;

	struct SelectionModel
	{
		SelectionModel()
			: ClassId(-1)
			, PartId(-1)
			, StateId(-1)
		{}

		int ClassId;
		int PartId;
		int StateId;
	} selection;

protected:
	wxAuiManager* m_auiManager;
	wxAuiPaneInfo m_paneInfoClassView;
	wxAuiPaneInfo m_paneInfoImageView;
	wxAuiPaneInfo m_paneInfoPropView;


	wxTreeCtrl* classView;
	ImageViewCtrl* imageView;
	wxPropertyGrid* propView;

	// 1st level
	wxMenuBar* mainmenu;
	wxMenu* fileMenu;
	wxMenu* editMenu;
	wxMenu* viewMenu;
	wxMenu* themeMenu;
	wxMenu* aboutMenu;

	// 2nd level & context menus
	wxMenu* imageViewMenu;
	wxMenu* windowMgmtMenu;
	wxMenu* propContextMenu;

	wxStatusBar* statusBar;
	SearchDialog* searchDlg;

	void OnFileOpenMenuClicked(wxCommandEvent& event);
	void OnFileSaveMenuClicked(wxCommandEvent& event);
	void OnClassViewTreeSelChanged(wxTreeEvent& event);
	void OnPropertyGridChanging(wxPropertyGridEvent& event);
	void OnPropertyGridItemDelete(wxCommandEvent& container);
	void OnPropertyGridItemCreate(wxCommandEvent& container);
	void OnImageExportClicked(wxCommandEvent& event);
	void OnImageReplaceClicked(wxCommandEvent& event);
	void OnEditStringResources(wxCommandEvent& event);
	void OnAboutClicked(wxCommandEvent& event);
	void OnHelpClicked(wxCommandEvent& event);
	void OnCollapseClicked(wxCommandEvent& event);
	void OnExpandClicked(wxCommandEvent& event);
	void OnResourceDlgClicked(wxCommandEvent& event);
	void OnOpenThemeFolder(wxCommandEvent& event);
	void OnOpenSearchDlg(wxCommandEvent& event);
	void OnImageViewContextMenuTriggered(wxContextMenuEvent& event);
	void OnImageViewBgSelect(wxCommandEvent& event);
	void OnTogglePane(wxCommandEvent& event);
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