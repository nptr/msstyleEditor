#include "MainWindow.h"
#include "AboutDialog.h"
#include "HelpDialog.h"

#include "CustomFileDropTarget.h"
#include "CustomTreeItemData.h"
#include "UiHelper.h"
#include <wx\mstream.h>
#include <wx\wfstream.h>
#include <wx\wupdlock.h>
#include <fstream>

using namespace msstyle;


MainWindow::MainWindow(wxWindow* parent, wxWindowID id, const wxString& title, const wxPoint& pos, const wxSize& size, long style) : wxFrame(parent, id, title, pos, size, style)
{	
	this->SetSizeHints(wxSize(900, 600), wxDefaultSize);
	this->SetBackgroundColour(wxSystemSettings::GetColour(wxSystemColour::wxSYS_COLOUR_LISTBOX));

	wxBoxSizer* bSizer2;
	bSizer2 = new wxBoxSizer(wxHORIZONTAL);

	classView = new wxTreeCtrl(this, wxID_ANY, wxDefaultPosition, wxDefaultSize, wxTR_DEFAULT_STYLE | wxTR_HIDE_ROOT | wxHSCROLL | wxNO_BORDER | wxVSCROLL | wxTAB_TRAVERSAL);
	classView->SetMinSize(wxSize(310, -1));

	bSizer2->Add(classView, 0, wxEXPAND, 5);

	wxBoxSizer* bSizer21;
	bSizer21 = new wxBoxSizer(wxVERTICAL);

	wxBoxSizer* bSizer3;
	bSizer3 = new wxBoxSizer(wxHORIZONTAL);

	imgView = new ImageViewCtrl(this, wxID_ANY, wxDefaultPosition, wxSize(-1, -1), 0);
	imgView->SetBackgroundColour(wxSystemSettings::GetColour(wxSystemColour::wxSYS_COLOUR_BACKGROUND));
	bSizer3->Add(imgView, 1, wxEXPAND, 5);

	propView = new wxPropertyGrid(this, wxID_ANY, wxDefaultPosition, wxSize(300, -1), wxPG_DEFAULT_STYLE | wxTAB_TRAVERSAL | wxNO_BORDER);
	propView->SetMinSize(wxSize(300, -1));

	bSizer3->Add(propView, 0, wxEXPAND, 5);
	bSizer21->Add(bSizer3, 1, wxEXPAND, 5);
	bSizer2->Add(bSizer21, 1, wxEXPAND, 5);

	this->SetSizer(bSizer2);
	this->Layout();

	mainmenu = new wxMenuBar(0);
	aboutMenu = new wxMenu();
	fileMenu = new wxMenu();
	imageMenu = new wxMenu();
	viewMenu = new wxMenu();

	fileMenu->Append(ID_FOPEN, wxT("&Open"));
	fileMenu->Append(ID_FSAVE, wxT("&Save"));
	fileMenu->Enable(ID_FSAVE, false);
	
	imageMenu->Append(ID_IEXPORT, wxT("&Export"));
	imageMenu->Append(ID_IREPLACE, wxT("&Replace"));
	imageMenu->Enable(ID_IEXPORT, false);
	imageMenu->Enable(ID_IREPLACE, false);
	
	aboutMenu->Append(ID_HELP, wxT("&License"));
	aboutMenu->Append(ID_ABOUT, wxT("&About"));

	viewMenu->Append(ID_EXPAND_TREE, wxT("&Expand All"));
	viewMenu->Append(ID_COLLAPSE_TREE, wxT("&Collapse All"));
	viewMenu->AppendSeparator();
	viewMenu->Append(ID_THEMEFOLDER, wxT("&Theme Folder"));
	viewMenu->Enable(ID_RESOURCEDLG, false);

	mainmenu->Append(fileMenu, wxT("&File"));
	mainmenu->Append(imageMenu, wxT("&Image"));
	mainmenu->Append(viewMenu, wxT("&View"));
	mainmenu->Append(aboutMenu, wxT("I&nfo"));

	this->SetMenuBar(mainmenu);

	statusBar = this->CreateStatusBar(1, wxST_SIZEGRIP, wxID_ANY);

	// Setup Eventhanndler
	Connect(ID_FOPEN, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnFileOpenMenuClicked));
	Connect(ID_FSAVE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnFileSaveMenuClicked));
	Connect(ID_IEXPORT, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnImageExportClicked));
	Connect(ID_IREPLACE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnImageReplaceClicked));
	Connect(ID_ABOUT, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnAboutClicked));
	Connect(ID_HELP, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnHelpClicked));
	Connect(ID_COLLAPSE_TREE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnCollapseClicked));
	Connect(ID_EXPAND_TREE, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnExpandClicked));
	Connect(ID_THEMEFOLDER, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnOpenThemeFolder));

	Connect(wxEVT_COMMAND_TREE_SEL_CHANGED, wxTreeEventHandler(MainWindow::OnClassViewTreeSelChanged), NULL, this);
	propView->Connect(wxEVT_PG_CHANGING, wxPropertyGridEventHandler(MainWindow::OnPropertyGridChanging), NULL, this);

	// Make sure selected image struct is 0
	selectedImage = { 0 };
	currentStyle = nullptr;

	// Looks like the resource has to be on top alphabetically or it wont be used as caption image..
	wxFrame::SetIcon(wxICON(APPICON));

	this->SetDropTarget(new CustomFileDropTarget(*this));
}


//////////////////////////////////////////////////////////////////////////
// EVENT HANDLER
//////////////////////////////////////////////////////////////////////////

void MainWindow::OnFileOpenMenuClicked(wxCommandEvent& event)
{
	wxFileDialog openFileDialog(this, _("Open Visual Style"), "", "","Visual Style (*.msstyles)|*.msstyles|All Files (*.*)|*.*", wxFD_OPEN | wxFD_FILE_MUST_EXIST);
	if (openFileDialog.ShowModal() == wxID_CANCEL)
		return;

	CloseStyle();
	OpenStyle(openFileDialog.GetPath());
}

void MainWindow::OnFileSaveMenuClicked(wxCommandEvent& event)
{
	wxFileDialog saveFileDialog(this, _("Save Visual Style"), "", "", "Visual Style (*.msstyles)|*.msstyles", wxFD_SAVE | wxFD_OVERWRITE_PROMPT);
	if (saveFileDialog.ShowModal() == wxID_CANCEL)
		return;

	wchar_t newPath[1024];
	lstrcpyW(newPath, (wchar_t*)saveFileDialog.GetPath().wc_str());

	try
	{
		currentStyle->Save(newPath);
	}
	catch (std::runtime_error err)
	{
		wxMessageBox(err.what(), "Error saving file!");
	}


	statusBar->SetStatusText("Style saved successfully!");
}

void MainWindow::OnClassViewTreeSelChanged(wxTreeEvent& event)
{
	auto treeItemID = event.GetItem();
	auto treeItemData = classView->GetItemData(treeItemID);
	
	selectedImageProp = nullptr;
	imgView->RemoveImage();

	// Class Node
	PropClassTreeItemData* classData = dynamic_cast<PropClassTreeItemData*>(treeItemData);
	if (classData != nullptr)
	{
		return;
	}

	// Part Node
	PartTreeItemData* partData = dynamic_cast<PartTreeItemData*>(treeItemData);
	if (partData != nullptr)
	{
		FillPropertyView(*partData->GetMsStylePart());
		return;
	}

	// Image Node
	PropTreeItemData* propData = dynamic_cast<PropTreeItemData*>(treeItemData);
	if (propData != nullptr)
	{
		selectedImageProp = propData->GetMSStyleProp();

		const wchar_t* file = currentStyle->IsReplacementImageQueued(propData->GetMSStyleProp());
		if (file != nullptr)
		{
			ShowImageFromFile(wxString(file));
			statusBar->SetStatusText(wxString("Image ID: ") << propData->GetMSStyleProp()->variants.imagetype.imageID << "*");
		}
		else
		{
			ShowImageFromResource(propData->GetMSStyleProp());
			statusBar->SetStatusText(wxString("Image ID: ") << propData->GetMSStyleProp()->variants.imagetype.imageID);
		}

		return;
	}

	return;
}

void MainWindow::OnPropertyGridChanging(wxPropertyGridEvent& event)
{
	wxPGProperty* tmpProp = event.GetProperty();
	MsStyleProperty* styleProp = (MsStyleProperty*)tmpProp->GetClientData();

	// Update Data. TODO: Check data for validity!
	if (styleProp->typeID == IDENTIFIER::FILENAME)
		styleProp->variants.imagetype.imageID = event.GetValidationInfo().GetValue().GetInteger();
	else if (styleProp->typeID == IDENTIFIER::INT)
		styleProp->variants.inttype.value = event.GetValidationInfo().GetValue().GetInteger();
	else if (styleProp->typeID == IDENTIFIER::SIZE)
		styleProp->variants.sizetype.size = event.GetValidationInfo().GetValue().GetInteger();
	else if (styleProp->typeID == IDENTIFIER::ENUM)
		styleProp->variants.enumtype.enumvalue = event.GetValidationInfo().GetValue().GetInteger();
	else if (styleProp->typeID == IDENTIFIER::BOOL)
		styleProp->variants.booltype.boolvalue = event.GetValidationInfo().GetValue().GetBool();
	else if (styleProp->typeID == IDENTIFIER::COLOR)
	{
		wxAny value = event.GetValidationInfo().GetValue();
		wxColor color = value.As<wxColour>();
		styleProp->variants.colortype.r = color.Red();
		styleProp->variants.colortype.g = color.Green();
		styleProp->variants.colortype.b = color.Blue();
	}
	else if (styleProp->typeID == IDENTIFIER::RECT || styleProp->typeID == IDENTIFIER::MARGINS)
	{
		int l, t, r, b;
		if (sscanf(event.GetValidationInfo().GetValue().GetString().mb_str(), "%d, %d, %d, %d", &l, &t, &r, &b) != 4)
		{
			event.Veto();
			wxMessageBox("Invalid format! expected: a, b, c, d", "format error", wxICON_ERROR);
			return;
		}
		else
		{
			//margins and rect have the same memory layout
			styleProp->variants.recttype.left = l;
			styleProp->variants.recttype.top = t;
			styleProp->variants.recttype.right = r;
			styleProp->variants.recttype.bottom = b;
		}
	}
	else if (styleProp->typeID == IDENTIFIER::POSITION)
	{
		int x, y;
		if (sscanf(event.GetValidationInfo().GetValue().GetString().mb_str(), "%d, %d", &x, &y) != 4)
		{
			event.Veto();
			wxMessageBox("Invalid format! expected: a, b", "format error", wxICON_ERROR);
			return;
		}
		else
		{
			styleProp->variants.positiontype.x;
			styleProp->variants.positiontype.y;
		}
	}
	else if (styleProp->typeID == IDENTIFIER::FONT)
	{
		styleProp->variants.fonttype.fontID = event.GetValidationInfo().GetValue().GetInteger();
	}
}

void MainWindow::OnImageExportClicked(wxCommandEvent& event)
{
	if (selectedImage.size == 0 || selectedImage.data == 0 || selectedImageProp == nullptr)
	{
		wxMessageBox("Select an image first!", "Export Image", wxICON_ERROR);
		return;
	}

	wxFileDialog saveFileDialog(this, "Export Image", "", "","PNG Image (*.png)|*.png", wxFD_SAVE | wxFD_OVERWRITE_PROMPT);
	if (saveFileDialog.ShowModal() == wxID_CANCEL)
		return;

	wxFileOutputStream outputStream(saveFileDialog.GetPath());
	if (!outputStream.IsOk())
	{
		wxLogError("Cannot save current contents in file '%s'.", saveFileDialog.GetPath());
		return;
	}

	if (!outputStream.WriteAll(selectedImage.data, selectedImage.size))
	{
		wxLogError("Error while writing to the file!");
		return;
	}

	statusBar->SetStatusText("Image exported successfully!");
}

void MainWindow::OnImageReplaceClicked(wxCommandEvent& event)
{
	if (selectedImageProp == nullptr)
	{
		wxMessageBox("Select an image resource first!", "Replace Image", wxICON_ERROR);
		return;
	}

	wxFileDialog openFileDialog(this, "Replace Image", "", "", "PNG Image (*.png)|*.png", wxFD_OPEN | wxFD_FILE_MUST_EXIST);
	if (openFileDialog.ShowModal() == wxID_CANCEL)
		return;

	currentStyle->UpdateImageResource(selectedImageProp, openFileDialog.GetPath().wc_str());
}


void MainWindow::OnHelpClicked(wxCommandEvent& event)
{
	HelpDialog helpDlg(this, wxID_ANY, wxT("Help"));
	helpDlg.ShowModal();
}

void MainWindow::OnAboutClicked(wxCommandEvent& event)
{
	AboutDialog aboutDlg(this, wxID_ANY, wxT("About"));
	aboutDlg.ShowModal();
}

void MainWindow::OnCollapseClicked(wxCommandEvent& event)
{
	classView->HideWithEffect(wxShowEffect::wxSHOW_EFFECT_BLEND);
	classView->CollapseAll();
	classView->ShowWithEffect(wxShowEffect::wxSHOW_EFFECT_BLEND);

	// collapse centered around the currently selected item
	// also focus the treeview again, as the menu took the focus
	wxTreeItemIdValue cookie;
	wxTreeItemId selectedItem = classView->GetSelection();
	if (selectedItem != nullptr)
		classView->ScrollTo(selectedItem);
	else classView->ScrollTo(classView->GetFirstChild(classView->GetRootItem(), cookie));

	classView->SetFocus();
}

void MainWindow::OnExpandClicked(wxCommandEvent& event)
{
	classView->HideWithEffect(wxShowEffect::wxSHOW_EFFECT_BLEND);
	classView->ExpandAll();
	classView->ShowWithEffect(wxShowEffect::wxSHOW_EFFECT_BLEND);

	// expand centered around the currently selected item
	// also focus the treeview again, as the menu took the focus
	wxTreeItemIdValue cookie;
	wxTreeItemId selectedItem = classView->GetSelection();
	if (selectedItem != nullptr)
		classView->ScrollTo(selectedItem);
	else classView->ScrollTo(classView->GetFirstChild(classView->GetRootItem(), cookie));

	classView->SetFocus();
}

void MainWindow::OnOpenThemeFolder(wxCommandEvent& event)
{
	wxExecute("explorer C:\\Windows\\Resources\\Themes\\", wxEXEC_ASYNC, NULL);
}

//////////////////////////////////////////////////////////////////////////
// OTHER LOGIC
//////////////////////////////////////////////////////////////////////////

void MainWindow::OpenStyle(const wxString& file)
{
	currentStyle = new VisualStyle();
	currentStyle->Load(file);

	FillClassView(currentStyle->GetClasses());

	imageMenu->Enable(ID_IEXPORT, true);
	imageMenu->Enable(ID_IREPLACE, true);
	fileMenu->Enable(ID_FSAVE, true);
	viewMenu->Enable(ID_RESOURCEDLG, true);

	this->SetTitle(wxString("msstyleEditor - ") + wxString(currentStyle->GetFileName()));
}

void MainWindow::CloseStyle()
{
	if (currentStyle != nullptr)
	{
		// remove everything that could still point to the style data
		propView->Clear();
		imgView->RemoveImage();
		classView->Freeze();
		classView->DeleteAllItems();
		classView->Thaw();

		delete currentStyle;
		currentStyle = nullptr;
	}
}


void MainWindow::FillClassView(const std::unordered_map<int, MsStyleClass*>* classes)
{
	classView->Freeze();
	classView->DeleteAllItems();
	wxTreeItemId rootNode = classView->AddRoot(wxT("[StyleName]"));
	
	// Add classes
	for (auto& cls : *classes)
	{
		wxTreeItemId classNode = classView->AppendItem(rootNode, cls.second->className, -1, -1, static_cast<wxTreeItemData*>(new PropClassTreeItemData(cls.second)));

		// Add parts
		for (auto& part : cls.second->parts)
		{
			wxTreeItemId partNode = classView->AppendItem(classNode, part.second->partName, -1, -1, static_cast<wxTreeItemData*>(new PartTreeItemData(part.second)));

			// Add images
			for (auto& state : part.second->states)
			{
				for (auto& prop : state.second->properties)
				{
					if (prop->typeID == IDENTIFIER::FILENAME || prop->typeID == IDENTIFIER::DISKSTREAM)
					{
						const char* propName = VisualStyle::FindPropName(prop->nameID); // propnames have to be looked up, but thats fast
						classView->AppendItem(partNode, propName, -1, -1, dynamic_cast<wxTreeItemData*>(new PropTreeItemData(prop)));
					}
				}
			}
		}
	}
	classView->SortChildren(rootNode);
	classView->Thaw();
}

void MainWindow::FillPropertyView(MsStylePart& part)
{
	propView->Clear();

	for (auto& state : part.states)
	{
		wxPropertyCategory* category = new wxPropertyCategory(state.second->stateName);
		for (auto& prop : state.second->properties)
		{
			category->AppendChild(GetWXPropertyFromMsStyleProperty(*prop));
		}

		propView->Append(category);
	}
}


void MainWindow::ShowImageFromResource(const MsStyleProperty* prop)
{
	if (prop->typeID == IDENTIFIER::FILENAME)
		selectedImage = currentStyle->GetResource(MAKEINTRESOURCEA(prop->variants.imagetype.imageID), "IMAGE");
	else if (prop->typeID == IDENTIFIER::DISKSTREAM)
		selectedImage = currentStyle->GetResource(MAKEINTRESOURCEA(prop->variants.imagetype.imageID), "STREAM");
	else return;

	wxMemoryInputStream stream = wxMemoryInputStream(selectedImage.data, selectedImage.size);

	wxImage img(stream, wxBITMAP_TYPE_PNG);
	imgView->SetImage(img);
}

void MainWindow::ShowImageFromFile(wxString& imgPath)
{
	wxFileInputStream stream(imgPath);

	wxImage img(stream, wxBITMAP_TYPE_PNG);
	imgView->SetImage(img);
}


MainWindow::~MainWindow()
{
	propView->Disconnect(wxEVT_PG_CHANGED, wxPropertyGridEventHandler(MainWindow::OnPropertyGridChanging), NULL, this);
	if (currentStyle != nullptr)
	{
		delete currentStyle;
		currentStyle = nullptr;
	}
}