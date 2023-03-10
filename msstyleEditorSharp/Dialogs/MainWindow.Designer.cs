
namespace msstyleEditor
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.ribbonMenu = new System.Windows.Forms.Ribbon();
            this.tabTheme = new System.Windows.Forms.RibbonTab();
            this.ribbonPanelFile = new System.Windows.Forms.RibbonPanel();
            this.btFileOpen = new System.Windows.Forms.RibbonButton();
            this.btFileSave = new System.Windows.Forms.RibbonButton();
            this.btFileSaveWithMUI = new System.Windows.Forms.RibbonButton();
            this.ribbonPanelExtras = new System.Windows.Forms.RibbonPanel();
            this.btFileInfoExport = new System.Windows.Forms.RibbonButton();
            this.btThemeFolder = new System.Windows.Forms.RibbonButton();
            this.ribbonPanelTheme = new System.Windows.Forms.RibbonPanel();
            this.btTestTheme = new System.Windows.Forms.RibbonButton();
            this.btOpenPreview = new System.Windows.Forms.RibbonButton();
            this.ribbonPanelImages = new System.Windows.Forms.RibbonPanel();
            this.btImageImport = new System.Windows.Forms.RibbonButton();
            this.btImageExport = new System.Windows.Forms.RibbonButton();
            this.ribbonPanelProps = new System.Windows.Forms.RibbonPanel();
            this.btPropertyAdd = new System.Windows.Forms.RibbonButton();
            this.btPropertyRemove = new System.Windows.Forms.RibbonButton();
            this.btPropertyImport = new System.Windows.Forms.RibbonButton();
            this.btPropertyExport = new System.Windows.Forms.RibbonButton();
            this.ribbonPanelSearch = new System.Windows.Forms.RibbonPanel();
            this.btSearch = new System.Windows.Forms.RibbonButton();
            this.tabView = new System.Windows.Forms.RibbonTab();
            this.ribbonPanelClassView = new System.Windows.Forms.RibbonPanel();
            this.btExpandTree = new System.Windows.Forms.RibbonButton();
            this.btCollapseTree = new System.Windows.Forms.RibbonButton();
            this.ribbonPanelImageBg = new System.Windows.Forms.RibbonPanel();
            this.btImageBgWhite = new System.Windows.Forms.RibbonButton();
            this.btImageBgGrey = new System.Windows.Forms.RibbonButton();
            this.btImageBgBlack = new System.Windows.Forms.RibbonButton();
            this.btImageBgChecker = new System.Windows.Forms.RibbonButton();
            this.ribbonPanelWindows = new System.Windows.Forms.RibbonPanel();
            this.btShowRenderView = new System.Windows.Forms.RibbonButton();
            this.btShowImageView = new System.Windows.Forms.RibbonButton();
            this.tabInfo = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel15 = new System.Windows.Forms.RibbonPanel();
            this.btDocumentation = new System.Windows.Forms.RibbonButton();
            this.btAbout = new System.Windows.Forms.RibbonButton();
            this.btLicense = new System.Windows.Forms.RibbonButton();
            this.ribbonPanel3 = new System.Windows.Forms.RibbonPanel();
            this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonTab4 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel4 = new System.Windows.Forms.RibbonPanel();
            this.ribbonPanel7 = new System.Windows.Forms.RibbonPanel();
            this.ribbonPanel8 = new System.Windows.Forms.RibbonPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lbStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbImageInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbStylePlatform = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.ribbonButton3 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton2 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton1 = new System.Windows.Forms.RibbonButton();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonMenu
            // 
            this.ribbonMenu.BorderMode = System.Windows.Forms.RibbonWindowMode.InsideWindow;
            this.ribbonMenu.CaptionBarVisible = false;
            this.ribbonMenu.Cursor = System.Windows.Forms.Cursors.Default;
            this.ribbonMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ribbonMenu.Location = new System.Drawing.Point(0, 0);
            this.ribbonMenu.Minimized = false;
            this.ribbonMenu.Name = "ribbonMenu";
            // 
            // 
            // 
            this.ribbonMenu.OrbDropDown.BorderRoundness = 2;
            this.ribbonMenu.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbonMenu.OrbDropDown.Name = "";
            this.ribbonMenu.OrbDropDown.Size = new System.Drawing.Size(527, 447);
            this.ribbonMenu.OrbDropDown.TabIndex = 0;
            this.ribbonMenu.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2013;
            this.ribbonMenu.OrbVisible = false;
            this.ribbonMenu.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.ribbonMenu.Size = new System.Drawing.Size(884, 119);
            this.ribbonMenu.TabIndex = 0;
            this.ribbonMenu.Tabs.Add(this.tabTheme);
            this.ribbonMenu.Tabs.Add(this.tabView);
            this.ribbonMenu.Tabs.Add(this.tabInfo);
            this.ribbonMenu.TabSpacing = 4;
            this.ribbonMenu.Text = "ribbon1";
            // 
            // tabTheme
            // 
            this.tabTheme.Name = "tabTheme";
            this.tabTheme.Panels.Add(this.ribbonPanelFile);
            this.tabTheme.Panels.Add(this.ribbonPanelExtras);
            this.tabTheme.Panels.Add(this.ribbonPanelTheme);
            this.tabTheme.Panels.Add(this.ribbonPanelImages);
            this.tabTheme.Panels.Add(this.ribbonPanelProps);
            this.tabTheme.Panels.Add(this.ribbonPanelSearch);
            this.tabTheme.Text = "Theme";
            // 
            // ribbonPanelFile
            // 
            this.ribbonPanelFile.ButtonMoreVisible = false;
            this.ribbonPanelFile.Items.Add(this.btFileOpen);
            this.ribbonPanelFile.Items.Add(this.btFileSave);
            this.ribbonPanelFile.Name = "ribbonPanelFile";
            this.ribbonPanelFile.Text = "File";
            // 
            // btFileOpen
            // 
            this.btFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("btFileOpen.Image")));
            this.btFileOpen.LargeImage = ((System.Drawing.Image)(resources.GetObject("btFileOpen.LargeImage")));
            this.btFileOpen.Name = "btFileOpen";
            this.btFileOpen.SmallImage = ((System.Drawing.Image)(resources.GetObject("btFileOpen.SmallImage")));
            this.btFileOpen.Text = "Open";
            this.btFileOpen.Click += new System.EventHandler(this.OnFileOpenClick);
            // 
            // btFileSave
            // 
            this.btFileSave.DropDownItems.Add(this.btFileSaveWithMUI);
            this.btFileSave.Image = ((System.Drawing.Image)(resources.GetObject("btFileSave.Image")));
            this.btFileSave.LargeImage = ((System.Drawing.Image)(resources.GetObject("btFileSave.LargeImage")));
            this.btFileSave.Name = "btFileSave";
            this.btFileSave.SmallImage = ((System.Drawing.Image)(resources.GetObject("btFileSave.SmallImage")));
            this.btFileSave.Style = System.Windows.Forms.RibbonButtonStyle.SplitDropDown;
            this.btFileSave.Text = "Save";
            this.btFileSave.Click += new System.EventHandler(this.OnFileSaveClick);
            // 
            // btFileSaveWithMUI
            // 
            this.btFileSaveWithMUI.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.btFileSaveWithMUI.Image = ((System.Drawing.Image)(resources.GetObject("btFileSaveWithMUI.Image")));
            this.btFileSaveWithMUI.LargeImage = ((System.Drawing.Image)(resources.GetObject("btFileSaveWithMUI.LargeImage")));
            this.btFileSaveWithMUI.Name = "btFileSaveWithMUI";
            this.btFileSaveWithMUI.SmallImage = ((System.Drawing.Image)(resources.GetObject("btFileSaveWithMUI.SmallImage")));
            this.btFileSaveWithMUI.Text = "Save with MUI";
            this.btFileSaveWithMUI.Click += new System.EventHandler(this.OnFileSaveClick);
            // 
            // ribbonPanelExtras
            // 
            this.ribbonPanelExtras.ButtonMoreVisible = false;
            this.ribbonPanelExtras.Items.Add(this.btFileInfoExport);
            this.ribbonPanelExtras.Items.Add(this.btThemeFolder);
            this.ribbonPanelExtras.Name = "ribbonPanelExtras";
            this.ribbonPanelExtras.Text = "Extras";
            // 
            // btFileInfoExport
            // 
            this.btFileInfoExport.Image = ((System.Drawing.Image)(resources.GetObject("btFileInfoExport.Image")));
            this.btFileInfoExport.LargeImage = ((System.Drawing.Image)(resources.GetObject("btFileInfoExport.LargeImage")));
            this.btFileInfoExport.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btFileInfoExport.Name = "btFileInfoExport";
            this.btFileInfoExport.SmallImage = ((System.Drawing.Image)(resources.GetObject("btFileInfoExport.SmallImage")));
            this.btFileInfoExport.Text = "Export Info";
            this.btFileInfoExport.Click += new System.EventHandler(this.OnExportStyleInfo);
            // 
            // btThemeFolder
            // 
            this.btThemeFolder.Image = ((System.Drawing.Image)(resources.GetObject("btThemeFolder.Image")));
            this.btThemeFolder.LargeImage = ((System.Drawing.Image)(resources.GetObject("btThemeFolder.LargeImage")));
            this.btThemeFolder.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btThemeFolder.Name = "btThemeFolder";
            this.btThemeFolder.SmallImage = ((System.Drawing.Image)(resources.GetObject("btThemeFolder.SmallImage")));
            this.btThemeFolder.Text = "Theme Folder";
            this.btThemeFolder.Click += new System.EventHandler(this.OnOpenThemeFolder);
            // 
            // ribbonPanelTheme
            // 
            this.ribbonPanelTheme.ButtonMoreVisible = false;
            this.ribbonPanelTheme.Items.Add(this.btTestTheme);
            this.ribbonPanelTheme.Items.Add(this.btOpenPreview);
            this.ribbonPanelTheme.Name = "ribbonPanelTheme";
            this.ribbonPanelTheme.Text = "Theme";
            // 
            // btTestTheme
            // 
            this.btTestTheme.Image = ((System.Drawing.Image)(resources.GetObject("btTestTheme.Image")));
            this.btTestTheme.LargeImage = ((System.Drawing.Image)(resources.GetObject("btTestTheme.LargeImage")));
            this.btTestTheme.Name = "btTestTheme";
            this.btTestTheme.SmallImage = ((System.Drawing.Image)(resources.GetObject("btTestTheme.SmallImage")));
            this.btTestTheme.Text = "Test";
            this.btTestTheme.Click += new System.EventHandler(this.OnTestTheme);
            // 
            // btOpenPreview
            // 
            this.btOpenPreview.Image = ((System.Drawing.Image)(resources.GetObject("btOpenPreview.Image")));
            this.btOpenPreview.LargeImage = ((System.Drawing.Image)(resources.GetObject("btOpenPreview.LargeImage")));
            this.btOpenPreview.Name = "btOpenPreview";
            this.btOpenPreview.SmallImage = ((System.Drawing.Image)(resources.GetObject("btOpenPreview.SmallImage")));
            this.btOpenPreview.Text = "Show Controls";
            this.btOpenPreview.Click += new System.EventHandler(this.OnControlPreview);
            // 
            // ribbonPanelImages
            // 
            this.ribbonPanelImages.ButtonMoreVisible = false;
            this.ribbonPanelImages.Items.Add(this.btImageImport);
            this.ribbonPanelImages.Items.Add(this.btImageExport);
            this.ribbonPanelImages.Name = "ribbonPanelImages";
            this.ribbonPanelImages.Text = "Images";
            // 
            // btImageImport
            // 
            this.btImageImport.Image = ((System.Drawing.Image)(resources.GetObject("btImageImport.Image")));
            this.btImageImport.LargeImage = ((System.Drawing.Image)(resources.GetObject("btImageImport.LargeImage")));
            this.btImageImport.Name = "btImageImport";
            this.btImageImport.SmallImage = ((System.Drawing.Image)(resources.GetObject("btImageImport.SmallImage")));
            this.btImageImport.Text = "Replace";
            this.btImageImport.Click += new System.EventHandler(this.OnImageImport);
            // 
            // btImageExport
            // 
            this.btImageExport.Image = ((System.Drawing.Image)(resources.GetObject("btImageExport.Image")));
            this.btImageExport.LargeImage = ((System.Drawing.Image)(resources.GetObject("btImageExport.LargeImage")));
            this.btImageExport.Name = "btImageExport";
            this.btImageExport.SmallImage = ((System.Drawing.Image)(resources.GetObject("btImageExport.SmallImage")));
            this.btImageExport.Text = "Export";
            this.btImageExport.Click += new System.EventHandler(this.OnImageExport);
            // 
            // ribbonPanelProps
            // 
            this.ribbonPanelProps.ButtonMoreVisible = false;
            this.ribbonPanelProps.Items.Add(this.btPropertyAdd);
            this.ribbonPanelProps.Items.Add(this.btPropertyRemove);
            this.ribbonPanelProps.Items.Add(this.btPropertyImport);
            this.ribbonPanelProps.Items.Add(this.btPropertyExport);
            this.ribbonPanelProps.Name = "ribbonPanelProps";
            this.ribbonPanelProps.Text = "Properties";
            // 
            // btPropertyAdd
            // 
            this.btPropertyAdd.Image = ((System.Drawing.Image)(resources.GetObject("btPropertyAdd.Image")));
            this.btPropertyAdd.LargeImage = ((System.Drawing.Image)(resources.GetObject("btPropertyAdd.LargeImage")));
            this.btPropertyAdd.Name = "btPropertyAdd";
            this.btPropertyAdd.SmallImage = ((System.Drawing.Image)(resources.GetObject("btPropertyAdd.SmallImage")));
            this.btPropertyAdd.Text = "Add";
            this.btPropertyAdd.Click += new System.EventHandler(this.OnPropertyAdd);
            // 
            // btPropertyRemove
            // 
            this.btPropertyRemove.Image = ((System.Drawing.Image)(resources.GetObject("btPropertyRemove.Image")));
            this.btPropertyRemove.LargeImage = ((System.Drawing.Image)(resources.GetObject("btPropertyRemove.LargeImage")));
            this.btPropertyRemove.Name = "btPropertyRemove";
            this.btPropertyRemove.SmallImage = ((System.Drawing.Image)(resources.GetObject("btPropertyRemove.SmallImage")));
            this.btPropertyRemove.Text = "Remove";
            this.btPropertyRemove.Click += new System.EventHandler(this.OnPropertyRemove);
            // 
            // btPropertyImport
            // 
            this.btPropertyImport.Image = ((System.Drawing.Image)(resources.GetObject("btPropertyImport.Image")));
            this.btPropertyImport.LargeImage = ((System.Drawing.Image)(resources.GetObject("btPropertyImport.LargeImage")));
            this.btPropertyImport.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btPropertyImport.MinSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btPropertyImport.Name = "btPropertyImport";
            this.btPropertyImport.SmallImage = ((System.Drawing.Image)(resources.GetObject("btPropertyImport.SmallImage")));
            this.btPropertyImport.Text = "Import";
            // 
            // btPropertyExport
            // 
            this.btPropertyExport.Image = ((System.Drawing.Image)(resources.GetObject("btPropertyExport.Image")));
            this.btPropertyExport.LargeImage = ((System.Drawing.Image)(resources.GetObject("btPropertyExport.LargeImage")));
            this.btPropertyExport.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btPropertyExport.MinSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btPropertyExport.Name = "btPropertyExport";
            this.btPropertyExport.SmallImage = ((System.Drawing.Image)(resources.GetObject("btPropertyExport.SmallImage")));
            this.btPropertyExport.Text = "Export";
            // 
            // ribbonPanelSearch
            // 
            this.ribbonPanelSearch.ButtonMoreVisible = false;
            this.ribbonPanelSearch.Items.Add(this.btSearch);
            this.ribbonPanelSearch.Name = "ribbonPanelSearch";
            this.ribbonPanelSearch.Text = "Search";
            // 
            // btSearch
            // 
            this.btSearch.Image = ((System.Drawing.Image)(resources.GetObject("btSearch.Image")));
            this.btSearch.LargeImage = ((System.Drawing.Image)(resources.GetObject("btSearch.LargeImage")));
            this.btSearch.Name = "btSearch";
            this.btSearch.SmallImage = ((System.Drawing.Image)(resources.GetObject("btSearch.SmallImage")));
            this.btSearch.Text = "Search";
            this.btSearch.Click += new System.EventHandler(this.OnSearchClicked);
            // 
            // tabView
            // 
            this.tabView.Name = "tabView";
            this.tabView.Panels.Add(this.ribbonPanelClassView);
            this.tabView.Panels.Add(this.ribbonPanelImageBg);
            this.tabView.Panels.Add(this.ribbonPanelWindows);
            this.tabView.Text = "View";
            // 
            // ribbonPanelClassView
            // 
            this.ribbonPanelClassView.ButtonMoreVisible = false;
            this.ribbonPanelClassView.Items.Add(this.btExpandTree);
            this.ribbonPanelClassView.Items.Add(this.btCollapseTree);
            this.ribbonPanelClassView.Name = "ribbonPanelClassView";
            this.ribbonPanelClassView.Text = "Class View";
            // 
            // btExpandTree
            // 
            this.btExpandTree.Image = ((System.Drawing.Image)(resources.GetObject("btExpandTree.Image")));
            this.btExpandTree.LargeImage = ((System.Drawing.Image)(resources.GetObject("btExpandTree.LargeImage")));
            this.btExpandTree.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btExpandTree.Name = "btExpandTree";
            this.btExpandTree.SmallImage = ((System.Drawing.Image)(resources.GetObject("btExpandTree.SmallImage")));
            this.btExpandTree.Text = "Expand All";
            this.btExpandTree.Click += new System.EventHandler(this.OnTreeExpandClick);
            // 
            // btCollapseTree
            // 
            this.btCollapseTree.Image = ((System.Drawing.Image)(resources.GetObject("btCollapseTree.Image")));
            this.btCollapseTree.LargeImage = ((System.Drawing.Image)(resources.GetObject("btCollapseTree.LargeImage")));
            this.btCollapseTree.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btCollapseTree.Name = "btCollapseTree";
            this.btCollapseTree.SmallImage = ((System.Drawing.Image)(resources.GetObject("btCollapseTree.SmallImage")));
            this.btCollapseTree.Text = "Collapse All";
            this.btCollapseTree.Click += new System.EventHandler(this.OnTreeCollapseClick);
            // 
            // ribbonPanelImageBg
            // 
            this.ribbonPanelImageBg.ButtonMoreVisible = false;
            this.ribbonPanelImageBg.Items.Add(this.btImageBgWhite);
            this.ribbonPanelImageBg.Items.Add(this.btImageBgGrey);
            this.ribbonPanelImageBg.Items.Add(this.btImageBgBlack);
            this.ribbonPanelImageBg.Items.Add(this.btImageBgChecker);
            this.ribbonPanelImageBg.Name = "ribbonPanelImageBg";
            this.ribbonPanelImageBg.Text = "Image Background";
            // 
            // btImageBgWhite
            // 
            this.btImageBgWhite.CheckedGroup = "BgColorCheckGroup";
            this.btImageBgWhite.CheckOnClick = true;
            this.btImageBgWhite.Image = ((System.Drawing.Image)(resources.GetObject("btImageBgWhite.Image")));
            this.btImageBgWhite.LargeImage = ((System.Drawing.Image)(resources.GetObject("btImageBgWhite.LargeImage")));
            this.btImageBgWhite.Name = "btImageBgWhite";
            this.btImageBgWhite.SmallImage = ((System.Drawing.Image)(resources.GetObject("btImageBgWhite.SmallImage")));
            this.btImageBgWhite.Text = "White";
            this.btImageBgWhite.Click += new System.EventHandler(this.OnImageViewBackgroundChange);
            // 
            // btImageBgGrey
            // 
            this.btImageBgGrey.CheckedGroup = "BgColorCheckGroup";
            this.btImageBgGrey.CheckOnClick = true;
            this.btImageBgGrey.Image = ((System.Drawing.Image)(resources.GetObject("btImageBgGrey.Image")));
            this.btImageBgGrey.LargeImage = ((System.Drawing.Image)(resources.GetObject("btImageBgGrey.LargeImage")));
            this.btImageBgGrey.Name = "btImageBgGrey";
            this.btImageBgGrey.SmallImage = ((System.Drawing.Image)(resources.GetObject("btImageBgGrey.SmallImage")));
            this.btImageBgGrey.Text = "Grey";
            this.btImageBgGrey.Click += new System.EventHandler(this.OnImageViewBackgroundChange);
            // 
            // btImageBgBlack
            // 
            this.btImageBgBlack.CheckedGroup = "BgColorCheckGroup";
            this.btImageBgBlack.CheckOnClick = true;
            this.btImageBgBlack.Image = ((System.Drawing.Image)(resources.GetObject("btImageBgBlack.Image")));
            this.btImageBgBlack.LargeImage = ((System.Drawing.Image)(resources.GetObject("btImageBgBlack.LargeImage")));
            this.btImageBgBlack.Name = "btImageBgBlack";
            this.btImageBgBlack.SmallImage = ((System.Drawing.Image)(resources.GetObject("btImageBgBlack.SmallImage")));
            this.btImageBgBlack.Text = "Black";
            this.btImageBgBlack.Click += new System.EventHandler(this.OnImageViewBackgroundChange);
            // 
            // btImageBgChecker
            // 
            this.btImageBgChecker.Checked = true;
            this.btImageBgChecker.CheckedGroup = "BgColorCheckGroup";
            this.btImageBgChecker.CheckOnClick = true;
            this.btImageBgChecker.Image = ((System.Drawing.Image)(resources.GetObject("btImageBgChecker.Image")));
            this.btImageBgChecker.LargeImage = ((System.Drawing.Image)(resources.GetObject("btImageBgChecker.LargeImage")));
            this.btImageBgChecker.Name = "btImageBgChecker";
            this.btImageBgChecker.SmallImage = ((System.Drawing.Image)(resources.GetObject("btImageBgChecker.SmallImage")));
            this.btImageBgChecker.Text = "Checker";
            this.btImageBgChecker.Click += new System.EventHandler(this.OnImageViewBackgroundChange);
            // 
            // ribbonPanelWindows
            // 
            this.ribbonPanelWindows.ButtonMoreEnabled = false;
            this.ribbonPanelWindows.ButtonMoreVisible = false;
            this.ribbonPanelWindows.Items.Add(this.btShowRenderView);
            this.ribbonPanelWindows.Items.Add(this.btShowImageView);
            this.ribbonPanelWindows.Name = "ribbonPanelWindows";
            this.ribbonPanelWindows.Text = "Windows";
            // 
            // btShowRenderView
            // 
            this.btShowRenderView.CheckedGroup = "rv";
            this.btShowRenderView.CheckOnClick = true;
            this.btShowRenderView.Image = ((System.Drawing.Image)(resources.GetObject("btShowRenderView.Image")));
            this.btShowRenderView.LargeImage = ((System.Drawing.Image)(resources.GetObject("btShowRenderView.LargeImage")));
            this.btShowRenderView.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btShowRenderView.Name = "btShowRenderView";
            this.btShowRenderView.SmallImage = ((System.Drawing.Image)(resources.GetObject("btShowRenderView.SmallImage")));
            this.btShowRenderView.Text = "Render View";
            this.btShowRenderView.Click += new System.EventHandler(this.OnToggleRenderView);
            // 
            // btShowImageView
            // 
            this.btShowImageView.CheckedGroup = "iv";
            this.btShowImageView.CheckOnClick = true;
            this.btShowImageView.Image = ((System.Drawing.Image)(resources.GetObject("btShowImageView.Image")));
            this.btShowImageView.LargeImage = ((System.Drawing.Image)(resources.GetObject("btShowImageView.LargeImage")));
            this.btShowImageView.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Medium;
            this.btShowImageView.Name = "btShowImageView";
            this.btShowImageView.SmallImage = ((System.Drawing.Image)(resources.GetObject("btShowImageView.SmallImage")));
            this.btShowImageView.Text = "Image View";
            this.btShowImageView.Click += new System.EventHandler(this.OnToggleImageView);
            // 
            // tabInfo
            // 
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Panels.Add(this.ribbonPanel15);
            this.tabInfo.Text = "Info";
            // 
            // ribbonPanel15
            // 
            this.ribbonPanel15.ButtonMoreVisible = false;
            this.ribbonPanel15.Items.Add(this.btDocumentation);
            this.ribbonPanel15.Items.Add(this.btAbout);
            this.ribbonPanel15.Items.Add(this.btLicense);
            this.ribbonPanel15.Name = "ribbonPanel15";
            this.ribbonPanel15.Text = "Help";
            // 
            // btDocumentation
            // 
            this.btDocumentation.Image = ((System.Drawing.Image)(resources.GetObject("btDocumentation.Image")));
            this.btDocumentation.LargeImage = ((System.Drawing.Image)(resources.GetObject("btDocumentation.LargeImage")));
            this.btDocumentation.Name = "btDocumentation";
            this.btDocumentation.SmallImage = ((System.Drawing.Image)(resources.GetObject("btDocumentation.SmallImage")));
            this.btDocumentation.Text = "Online Help";
            this.btDocumentation.Click += new System.EventHandler(this.OnDocumentationClicked);
            // 
            // btAbout
            // 
            this.btAbout.Image = ((System.Drawing.Image)(resources.GetObject("btAbout.Image")));
            this.btAbout.LargeImage = ((System.Drawing.Image)(resources.GetObject("btAbout.LargeImage")));
            this.btAbout.Name = "btAbout";
            this.btAbout.SmallImage = ((System.Drawing.Image)(resources.GetObject("btAbout.SmallImage")));
            this.btAbout.Text = "About";
            this.btAbout.Click += new System.EventHandler(this.OnAboutClicked);
            // 
            // btLicense
            // 
            this.btLicense.Image = ((System.Drawing.Image)(resources.GetObject("btLicense.Image")));
            this.btLicense.LargeImage = ((System.Drawing.Image)(resources.GetObject("btLicense.LargeImage")));
            this.btLicense.Name = "btLicense";
            this.btLicense.SmallImage = ((System.Drawing.Image)(resources.GetObject("btLicense.SmallImage")));
            this.btLicense.Text = "Legal";
            this.btLicense.Click += new System.EventHandler(this.OnLicenseClicked);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatusMessage,
            this.lbImageInfo,
            this.lbStylePlatform});
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(884, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lbStatusMessage
            // 
            this.lbStatusMessage.Name = "lbStatusMessage";
            this.lbStatusMessage.Size = new System.Drawing.Size(49, 17);
            this.lbStatusMessage.Text = "C: 0 P: 0";
            this.lbStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbImageInfo
            // 
            this.lbImageInfo.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lbImageInfo.Name = "lbImageInfo";
            this.lbImageInfo.Size = new System.Drawing.Size(4, 17);
            this.lbImageInfo.Visible = false;
            // 
            // lbStylePlatform
            // 
            this.lbStylePlatform.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lbStylePlatform.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lbStylePlatform.Name = "lbStylePlatform";
            this.lbStylePlatform.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbStylePlatform.Size = new System.Drawing.Size(4, 17);
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockLeftPortion = 0.35D;
            this.dockPanel.DockRightPortion = 0.35D;
            this.dockPanel.Location = new System.Drawing.Point(0, 117);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(884, 423);
            this.dockPanel.TabIndex = 4;
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.Image")));
            this.ribbonButton3.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.LargeImage")));
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.SmallImage")));
            this.ribbonButton3.Text = "Import";
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.Image")));
            this.ribbonButton2.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.LargeImage")));
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.SmallImage")));
            this.ribbonButton2.Text = "Export";
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.Image")));
            this.ribbonButton1.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.LargeImage")));
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
            this.ribbonButton1.Text = "Test";
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.ribbonMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainWindow";
            this.Text = "msstyleEditor";
            this.Load += new System.EventHandler(this.OnMainWindowLoad);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.SystemColorsChanged += new System.EventHandler(this.OnSystemColorsChanged);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Ribbon ribbonMenu;
        private System.Windows.Forms.RibbonPanel ribbonPanel3;
        private System.Windows.Forms.RibbonPanel ribbonPanel2;
        private System.Windows.Forms.RibbonButton ribbonButton3;
        private System.Windows.Forms.RibbonButton ribbonButton2;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.RibbonButton ribbonButton1;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonTab tabTheme;
        private System.Windows.Forms.RibbonTab ribbonTab4;
        private System.Windows.Forms.RibbonPanel ribbonPanelFile;
        private System.Windows.Forms.RibbonButton btFileOpen;
        private System.Windows.Forms.RibbonButton btFileSave;
        private System.Windows.Forms.RibbonPanel ribbonPanel4;
        private System.Windows.Forms.RibbonPanel ribbonPanel7;
        private System.Windows.Forms.RibbonPanel ribbonPanel8;
        private System.Windows.Forms.RibbonPanel ribbonPanelExtras;
        private System.Windows.Forms.RibbonButton btFileInfoExport;
        private System.Windows.Forms.RibbonPanel ribbonPanelTheme;
        private System.Windows.Forms.RibbonButton btTestTheme;
        private System.Windows.Forms.RibbonPanel ribbonPanelImages;
        private System.Windows.Forms.RibbonButton btImageExport;
        private System.Windows.Forms.RibbonButton btImageImport;
        private System.Windows.Forms.RibbonPanel ribbonPanelProps;
        private System.Windows.Forms.RibbonButton btPropertyExport;
        private System.Windows.Forms.RibbonButton btPropertyImport;
        private System.Windows.Forms.RibbonButton btPropertyAdd;
        private System.Windows.Forms.RibbonButton btPropertyRemove;
        private System.Windows.Forms.RibbonButton btOpenPreview;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.RibbonTab tabView;
        private System.Windows.Forms.RibbonPanel ribbonPanelClassView;
        private System.Windows.Forms.RibbonButton btExpandTree;
        private System.Windows.Forms.RibbonButton btCollapseTree;
        private System.Windows.Forms.RibbonPanel ribbonPanelImageBg;
        private System.Windows.Forms.ToolStripStatusLabel lbStatusMessage;
        private System.Windows.Forms.RibbonButton btImageBgWhite;
        private System.Windows.Forms.RibbonButton btImageBgGrey;
        private System.Windows.Forms.RibbonButton btImageBgBlack;
        private System.Windows.Forms.RibbonButton btImageBgChecker;
        private System.Windows.Forms.RibbonTab tabInfo;
        private System.Windows.Forms.RibbonButton btThemeFolder;
        private System.Windows.Forms.ToolStripStatusLabel lbStylePlatform;
        private System.Windows.Forms.RibbonPanel ribbonPanel15;
        private System.Windows.Forms.RibbonButton btAbout;
        private System.Windows.Forms.RibbonButton btDocumentation;
        private System.Windows.Forms.RibbonButton btLicense;
        private System.Windows.Forms.RibbonPanel ribbonPanelSearch;
        private System.Windows.Forms.RibbonButton btSearch;
        private System.Windows.Forms.ToolStripStatusLabel lbImageInfo;
        private System.Windows.Forms.RibbonButton btFileSaveWithMUI;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.RibbonPanel ribbonPanelWindows;
        private System.Windows.Forms.RibbonButton btShowImageView;
        private System.Windows.Forms.RibbonButton btShowRenderView;
    }
}