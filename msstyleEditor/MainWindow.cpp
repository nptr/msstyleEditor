#include "MainWindow.h"
#include "AboutDialog.h"
#include "HelpDialog.h"

#include "CustomFileDropTarget.h"
#include "CustomTreeItemData.h"
#include "SearchDialog.h"
#include "UiHelper.h"
#include <wx\mstream.h>
#include <wx\wfstream.h>
#include <wx\wupdlock.h>
#include <fstream>
#include <algorithm>
#include <string>
#include <cctype>

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
	imgView->SetBackgroundColour(wxColour(0xEFEFEF));
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
	Connect(ID_FIND, wxEVT_COMMAND_MENU_SELECTED, wxCommandEventHandler(MainWindow::OnOpenSearchDlg));

	Connect(wxEVT_COMMAND_TREE_SEL_CHANGED, wxTreeEventHandler(MainWindow::OnClassViewTreeSelChanged), NULL, this);
	propView->Connect(wxEVT_PG_CHANGING, wxPropertyGridEventHandler(MainWindow::OnPropertyGridChanging), NULL, this);

	// Make sure selected image struct is 0
	selectedImage = { 0 };
	currentStyle = nullptr;

	// Looks like the resource has to be on top alphabetically or it wont be used as caption image..
	wxFrame::SetIcon(wxICON(APPICON));

	this->SetDropTarget(new CustomFileDropTarget(*this));

	wxAcceleratorEntry entries[1];
	entries[0].Set(wxAcceleratorEntryFlags::wxACCEL_CTRL, wxKeyCode::WXK_CONTROL_F, ID_FIND);
	wxAcceleratorTable table(1, entries);
	SetAcceleratorTable(table);
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

	wchar_t newPath[256]; // max path lenght for pretty much every FS
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

void MainWindow::OnOpenSearchDlg(wxCommandEvent& event)
{
	if (searchDlg == nullptr)
	{
		searchDlg = new SearchDialog(this);
		searchDlg->SetSearchHandler((ISearchDialogListener*)this);
	}

	if (!searchDlg->IsShown())
		searchDlg->Show();
}

bool endReached = false;
void MainWindow::OnFindNext(const SearchProperties& search)
{
	if (currentStyle == nullptr)
		return;

	if (search.value.length() == 0)
		return;

	wxTreeItemId startItem = classView->GetSelection();
	if (!startItem.IsOk() || endReached)
	{
		endReached = false;
		startItem = classView->GetRootItem();
		if (!startItem.IsOk())
			return;
	}

	wxTreeItemId item = FindNext(search, startItem);
	if (item.IsOk())
		classView->SelectItem(item);
	else
	{
		wxMessageBox(wxT("No further match for \"")
			+ search.value
			+ wxT("\" !\n")
			+ wxT("Search will begin from top again."));
		endReached = true;
	}
}


bool ContainsStringInvariant(const std::string& text, const std::string& toFind)
{
	auto& it = std::search(text.begin(), text.end(),
		toFind.begin(), toFind.end(),
		[](char c1, char c2) -> bool
	{
		return std::toupper(c1) == std::toupper(c2);
	});

	return (it != text.end());
}

bool ContainsProperty(const SearchProperties& search, wxTreeItemData* treeItemData)
{
	if (treeItemData == nullptr)
		return false;

	// If its a part node, check its properties
	PartTreeItemData* partData = dynamic_cast<PartTreeItemData*>(treeItemData);
	if (partData == nullptr)
		return false;

	MsStylePart* part = partData->GetMsStylePart();
	
	for (auto& stateIt = part->states.begin(); stateIt != part->states.end(); ++stateIt)
	{
		for (auto& propIt : stateIt->second->properties)
		{
			// if its a property of the desired type, do a comparison
			if (propIt->typeID != search.type)
				continue;

			// comparison
			switch (propIt->typeID)
			{
				case IDENTIFIER::POSITION:
				{
					char propPos[32];
					sprintf(propPos, "%d,%d",
						propIt->variants.positiontype.x,
						propIt->variants.positiontype.y);

					std::string tmp = search.value;
					tmp.erase(std::remove_if(tmp.begin(), tmp.end(), std::isspace), tmp.end());

					if (ContainsStringInvariant(std::string(propPos), tmp))
						return true;
				} break;
				case IDENTIFIER::COLOR:
				{
					char propColor[32];
					sprintf(propColor, "%d,%d,%d",
						propIt->variants.colortype.r,
						propIt->variants.colortype.g,
						propIt->variants.colortype.b);

					std::string tmp = search.value;
					tmp.erase(std::remove_if(tmp.begin(), tmp.end(), std::isspace), tmp.end());

					if (ContainsStringInvariant(std::string(propColor), tmp))
						return true;
				} break;
				case IDENTIFIER::MARGINS:
				{
					char propMargin[32];
					sprintf(propMargin, "%d,%d,%d,%d",
						propIt->variants.margintype.left,
						propIt->variants.margintype.top,
						propIt->variants.margintype.right,
						propIt->variants.margintype.bottom);

					std::string tmp = search.value;
					tmp.erase(std::remove_if(tmp.begin(), tmp.end(), std::isspace), tmp.end());

					if (ContainsStringInvariant(std::string(propMargin), tmp))
						return true;
				} break;
				case IDENTIFIER::RECT:
				{
					char propRect[32];
					sprintf(propRect, "%d,%d,%d,%d",
						propIt->variants.recttype.left,
						propIt->variants.recttype.top,
						propIt->variants.recttype.right,
						propIt->variants.recttype.bottom);

					std::string tmp = search.value;
					tmp.erase(std::remove_if(tmp.begin(), tmp.end(), std::isspace), tmp.end());

					if (ContainsStringInvariant(std::string(propRect), tmp))
						return true;
				} break;
				case IDENTIFIER::SIZE:
				{
					try
					{
						int size = std::stoi(search.value);
						if (size == propIt->variants.sizetype.size)
							return true;
					}
					catch (...) {}
				} break;
			}
		}
	}

	return false;
}

bool ContainsName(const std::string& str, wxTreeItemData* treeItemData)
{
	if (treeItemData == nullptr)
		return false;

	// Class Node
	PropClassTreeItemData* classData = dynamic_cast<PropClassTreeItemData*>(treeItemData);
	if (classData != nullptr)
	{
		return ContainsStringInvariant(classData->GetClass()->className, str);
	}

	// Part Node
	PartTreeItemData* partData = dynamic_cast<PartTreeItemData*>(treeItemData);
	if (partData != nullptr)
	{
		return ContainsStringInvariant(partData->GetMsStylePart()->partName, str);
	}

	// Image Node
	PropTreeItemData* propData = dynamic_cast<PropTreeItemData*>(treeItemData);
	if (propData != nullptr)
	{
		const char* name = VisualStyle::FindPropName(propData->GetMSStyleProp()->nameID);
		if (name == nullptr)
			return false;
		else return ContainsStringInvariant(std::string(name), str);
	}

	return false;
}

wxTreeItemId MainWindow::FindNext(const SearchProperties& search, wxTreeItemId node)
{
	wxTreeItemIdValue cookie;
	wxTreeItemId originalNode = node;
	while (node.IsOk())
	{
		// see whether the node contains "str" somewhere.
		// skip the first node to not get stuck.
		if (node != originalNode)
		{
			wxTreeItemData* data = classView->GetItemData(node);
			switch (search.mode)
			{
				case SearchProperties::MODE_NAME:
				{
					if (ContainsName(search.value, data))
						return node;
				} break;
				case SearchProperties::MODE_PROPERTY:
				{
					if (ContainsProperty(search, data))
						return node;
				} break;
				default:
					break;
			}
		}


		// find nodes: depth
		wxTreeItemId nextNode = classView->GetFirstChild(node, cookie);
		if (!nextNode.IsOk())
		{
			// find nodes: breadth
			nextNode = classView->GetNextSibling(node);
			if (!nextNode.IsOk())
			{
				// back out and try finding a node in the breadth
				wxTreeItemId previous = node;
				nextNode = classView->GetItemParent(node);
				while (nextNode.IsOk() && nextNode != classView->GetRootItem() && !classView->GetNextSibling(nextNode).IsOk())
				{
					nextNode = classView->GetItemParent(nextNode);
				}

				if (nextNode == classView->GetRootItem())
					return wxTreeItemId();

				if (nextNode.IsOk()) // parent ok, so we have a sibling
					nextNode = classView->GetNextSibling(nextNode);
				//else parent was not ok, so we hit the root node -> no nodes left anymore -> search done
			}
		}

		node = nextNode;
	}

	return wxTreeItemId();
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