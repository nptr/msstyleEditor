using libmsstyle;
using msstyleEditor.Dialogs;
using msstyleEditor.Properties;
using msstyleEditor.PropView;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace msstyleEditor
{
    public partial class MainWindow : Form
    {
        private VisualStyle m_style;
        private Selection m_selection = new Selection();
        private StyleResource m_selectedImage = null;
        private ThemeManager m_themeManager = null;

        private SearchDialog m_searchDialog = new SearchDialog();
        private ClassViewWindow m_classView;
        private PropertyViewWindow m_propertyView;
        private ImageView m_imageView;
        private RenderView m_renderView;

        private TimingFunction m_selectedTimingFunction;
        private AnimationTypeDescriptor m_selectedAnimation;
        public MainWindow(String[] args)
        {
            InitializeComponent();

            // Our ribbon "SystemColors" theme adapts itself to the active visual style
            ribbonMenu.ThemeColor = RibbonTheme.SystemColors;

            // Set the best matching theme dock theme. This can only be done when no windows
            // were added to the dock yet.
            float brightness = SystemColors.Control.GetBrightness();
            dockPanel.Theme = brightness < 0.5f
                ? dockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme()
                : dockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();

            OnSystemColorsChanged(null, null);

            m_classView = new ClassViewWindow();
            m_classView.Show(dockPanel, DockState.DockLeft);
            m_classView.CloseButtonVisible = false;
            m_classView.OnSelectionChanged += OnTreeItemSelected;

            m_imageView = new ImageView();
            m_imageView.SelectedIndexChanged += OnImageSelectIndex;
            m_imageView.OnViewBackColorChanged += OnImageViewBackgroundChanged;
            m_imageView.Show(dockPanel, DockState.Document);
            m_imageView.VisibleChanged += (s, e) => { btShowImageView.Checked = m_imageView.Visible; };
            m_imageView.SetActiveTabs(-1, 0);

            m_renderView = new RenderView();
            m_renderView.Show(m_imageView.Pane, DockAlignment.Top, 0.25);
            m_renderView.VisibleChanged += (s, e) => { btShowRenderView.Checked = m_renderView.Visible; };
            m_renderView.Visible = false;
            m_renderView.Hide();

            m_propertyView = new PropertyViewWindow();
            m_propertyView.Show(dockPanel, DockState.DockRight);
            m_propertyView.CloseButtonVisible = false;
            m_propertyView.OnPropertyAdded += OnPropertyAdded;
            m_propertyView.OnPropertyRemoved += OnPropertyRemoved;

            try
            {
                m_themeManager = new ThemeManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nIs Aero/DWM disabled? Starting without \"Test Theme\" feature.",
                    "Themeing API unavailable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btTestTheme.Enabled = false;
            }

            SetTestThemeButtonState(false);
            CloseStyle();

            m_searchDialog.StartPosition = FormStartPosition.CenterParent;
            m_searchDialog.OnSearch += this.OnSearchNextItem;
            m_searchDialog.OnReplace += this.OnReplaceItem;

            if (args.Length > 0)
            {
                OpenStyle(args[0]);
            }
        }

        #region State Management & Helper

        private void SetTestThemeButtonState(bool active)
        {
            if (active)
            {
                btTestTheme.LargeImage = Resources.Stop32;
                btTestTheme.Text = "Stop Test";
            }
            else
            {
                btTestTheme.LargeImage = Resources.Play32;
                btTestTheme.Text = "Test";
            }
        }

        private void UpdateItemSelection(StyleClass cls = null, StylePart part = null, StyleState state = null, int resId = -1)
        {
            m_selection.Class = cls;
            m_selection.Part = part;
            m_selection.State = state;
            m_selection.ResourceId = resId;

            lbStatusMessage.Text = "C: " + cls.ClassId;
            if (part != null) lbStatusMessage.Text += ", P: " + part.PartId;
            if (state != null) lbStatusMessage.Text += ", S: " + state.StateId;
            if (resId >= 0) lbStatusMessage.Text += ", R: " + resId;
        }

        private void UpdateWindowCaption(string text)
        {
            this.Text = "msstyleEditor - " + text;
        }

        private void UpdateStatusText(string text)
        {
            lbStatusMessage.Text = text;
        }

        private void UpdateImageInfo(Image img)
        {
            if (img == null)
            {
                lbImageInfo.Visible = false;
            }
            else
            {
                lbImageInfo.Text = $"{img.Width}x{img.Height}px";
                lbImageInfo.Visible = true;
            }
        }

        private void OpenStyle(string path)
        {
            try
            {
                m_style = new VisualStyle();
                m_style.Load(path);
                UpdateWindowCaption(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Are you sure this is a Windows Vista or higher visual style?\r\n\r\nDetails: {ex.Message}", "Error loading style!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            m_classView.SetVisualStyle(m_style);

            btFileSave.Enabled = true;
            btFileInfoExport.Enabled = true;
            btImageExport.Enabled = false;
            btImageImport.Enabled = false;
            //btPropertyExport.Enabled = false;
            //btPropertyImport.Enabled = false;
            btPropertyAdd.Enabled = true;
            btPropertyRemove.Enabled = true;
            btTestTheme.Enabled = m_themeManager != null ? true : false;

            if (m_style.PreferredStringTable?.Count == 0)
                btFileSave.Style = RibbonButtonStyle.Normal;
            else btFileSave.Style = RibbonButtonStyle.SplitDropDown;

            lbStylePlatform.Text = m_style.Platform.ToDisplayString();

            if (m_style.PreferredStringTable.Count == 0)
            {
                MessageBox.Show(this,
                    "Could not locate 'String Table' resource! Some features may not work as expected. " +
                    "Was the file renamed or moved before initially opening it?\r\n\r\n" +
                    "- Make sure the '.msstyles' file is next to its multilingual resource files: '[lang-id]/[name].msstyles.mui'.\r\n" +
                    "- Make sure the names match.\r\n\r\n" +
                    "Then, reopen the file. After properly loading and saving the style, relocating the file is safe."
                    , "Warning, resource is missing!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
        }

        private void CloseStyle()
        {
            m_classView.SetVisualStyle(null);
            m_propertyView.SetStylePart(null, null, null);

            btFileSave.Enabled = false;
            btFileInfoExport.Enabled = false;
            btImageExport.Enabled = false;
            btImageImport.Enabled = false;
            btPropertyExport.Enabled = false;
            btPropertyImport.Enabled = false;
            btPropertyAdd.Enabled = false;
            btPropertyRemove.Enabled = false;
            btTestTheme.Enabled = false;

            lbStylePlatform.Text = "";
            m_style?.Dispose();
        }

        private void DisplayClass(StyleClass cls)
        {
            // remember the shown class
            UpdateItemSelection(cls);
            // reset propery view
            m_propertyView.SetStylePart(m_style, cls, null);
            // reset image view
            m_selectedImage = UpdateImageView(null);
            // reset image selector
            m_imageView.SetActiveTabs(-1, 0);
        }

        private void DisplayPart(StyleClass cls, StylePart part)
        {
            // remember the shown class and part
            UpdateItemSelection(cls, part);
            // reset propery view
            m_propertyView.SetStylePart(m_style, cls, part);

            // find ATLASRECT property so we can set the part highlights
            var def = default(KeyValuePair<int, StyleState>);
            var state = part.States.FirstOrDefault();
            if (!state.Equals(def))
            {
                var rectProp = state.Value.Properties.Find((p) => p.Header.nameID == (int)IDENTIFIER.ATLASRECT);
                if (rectProp != null)
                {
                    var mt = rectProp.GetValueAs<Margins>();
                    var ha = new Rectangle(
                        new Point(mt.Left, mt.Top),
                        new Size(mt.Right - mt.Left, mt.Bottom - mt.Top)
                    );
                    m_imageView.ViewHighlightArea = ha;
                }
                else m_imageView.ViewHighlightArea = null;
            }
            else m_imageView.ViewHighlightArea = null;

            // select the first image property of this part.
            // if the part doesn't have one, we take the first image of the "Common" part.
            StyleProperty imagePropToShow = null;
            var imgProps = part.GetImageProperties().ToList();
            if (imgProps.Count > 0)
            {
                imagePropToShow = imgProps[0];
            }
            else
            {
                StylePart commonPart;
                if(cls.Parts.TryGetValue(0, out commonPart))
                {
                    imagePropToShow = commonPart.GetImageProperties().FirstOrDefault();
                }
            }

            // reset image view
            m_selectedImage = UpdateImageView(imagePropToShow);
            // reset image selector
            m_imageView.SetActiveTabs(0, imgProps.Count);

            // TODO: ugly code..
            var renderer = new PartRenderer(m_style, part);
            m_renderView.Image = renderer.RenderPreview();
        }

        private StyleResource UpdateImageView(StyleProperty prop)
        {
            if (prop == null)
            {
                btImageExport.Enabled = false;
                btImageImport.Enabled = false;
                m_imageView.ViewImage = null;
                UpdateImageInfo(null);
                return null;
            }

            // determine type for resource update
            StyleResourceType resType = StyleResourceType.None;
            if (prop.Header.typeID == (int)IDENTIFIER.FILENAME ||
               prop.Header.typeID == (int)IDENTIFIER.FILENAME_LITE)
            {
                resType = StyleResourceType.Image;
            }
            else if (prop.Header.typeID == (int)IDENTIFIER.DISKSTREAM)
            {
                resType = StyleResourceType.Atlas;
            }

            // see if there is a pending update to the resource
            string file = m_style.GetQueuedResourceUpdate(prop.Header.shortFlag, resType);

            // in any case, we have to store the update info of the real resource
            // we need that in order to export/replace?
            var resource = m_style.GetResourceFromProperty(prop);

            Image img = null;
            if (!String.IsNullOrEmpty(file))
            {
                img = Image.FromFile(file);
            }
            else
            {
                if (resource?.Data != null)
                {
                    img = Image.FromStream(new MemoryStream(resource.Data));
                }
            }

            btImageExport.Enabled = resource.Data != null;
            btImageImport.Enabled = true;
            m_imageView.ViewImage = img;
            UpdateImageInfo(img);
            return resource;
        }

        #endregion

        #region Functionality

        private void OnFileOpenClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Title = "Open Visual Style",
                Filter = "Visual Style (*.msstyles)|*.msstyles|All Files (*.*)|*.*"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            CloseStyle();
            OpenStyle(ofd.FileName);
        }

        private void OnFileSaveClick(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                Title = "Save Visual Style",
                Filter = "Visual Style (*.msstyles)|*.msstyles",
                OverwritePrompt = true
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                if (sender == btFileSave)
                    m_style.Save(sfd.FileName, true);
                else if (sender == btFileSaveWithMUI)
                    m_style.Save(sfd.FileName, false);

                lbStatusMessage.Text = "Style saved successfully!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0)
            {
                return;
            }

            var ext = Path.GetExtension(files[0]).ToLower();
            if (ext == ".msstyles")
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0)
            {
                return;
            }

            CloseStyle();
            OpenStyle(files[0]);
        }

        private void OnExportStyleInfo(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                Title = "Export Style Info",
                Filter = "Style Info (*.txt)|*.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Exporter.ExportLogicalStructure(sfd.FileName, m_style);
            }
        }

        private void OnOpenThemeFolder(object sender, EventArgs e)
        {
            try
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                var args = String.Format("{0}\\Resources\\Themes\\", folder);
                Process.Start("explorer", args);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error opening theme folder!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnControlPreview(object sender, EventArgs e)
        {
            var dlg = new ControlDemoDialog();
            dlg.Show();
        }

        private void OnTreeItemSelected(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }

            StyleClass cls = e.Node.Tag as StyleClass;
            if (cls != null)
            {
                DisplayClass(cls);
                return;
            }

            StylePart part = e.Node.Tag as StylePart;
            if (part != null)
            {
                cls = e.Node.Parent.Tag as StyleClass;
                Debug.Assert(cls != null);

                DisplayPart(cls, part);
                return;
            }

            TimingFunction func = e.Node.Tag as TimingFunction;
            if (func != null)
            {
                m_selectedTimingFunction = func;
                m_propertyView.SetTimingFunction(m_style, func);
                return;
            }

            AnimationTypeDescriptor animation = e.Node.Tag as AnimationTypeDescriptor;
            if (animation != null)
            {
                m_selectedAnimation = animation;
                m_propertyView.SetAnimation(m_style, animation);
                return;
            }

            //nothing valid is selected, clear the property grid
            m_propertyView.SetStylePart(null, null, null);
        }

        private void OnTreeExpandClick(object sender, EventArgs e)
        {
            m_classView.ExpandAll();
        }

        private void OnTreeCollapseClick(object sender, EventArgs e)
        {
            m_classView.CollapseAll();
        }

        private void OnToggleRenderView(object sender, EventArgs e)
        {
            m_renderView.IsHidden = !m_renderView.IsHidden;
        }

        private void OnToggleImageView(object sender, EventArgs e)
        {
            m_imageView.IsHidden = !m_imageView.IsHidden;
        }

        private void OnTestTheme(object sender, EventArgs e)
        {
            if (m_themeManager.IsThemeInUse)
            {
                try
                {
                    System.Threading.Thread.Sleep(250); // prevent doubleclicks
                    m_themeManager.Rollback();
                    SetTestThemeButtonState(m_themeManager.IsThemeInUse);
                    return;
                }
                catch (Exception) { }
            }

            Win32Api.OSVERSIONINFOEXW version = new Win32Api.OSVERSIONINFOEXW()
            {
                dwOSVersionInfoSize = Marshal.SizeOf(typeof(Win32Api.OSVERSIONINFOEXW))
            };
            Win32Api.RtlGetVersion(ref version);

            bool needConfirmation = false;
            if (version.dwMajorVersion == 6 &&
                version.dwMinorVersion == 0 &&
                m_style.Platform != Platform.Vista)
            {
                needConfirmation = true;
            }

            if (version.dwMajorVersion == 6 &&
                version.dwMinorVersion == 1 &&
                m_style.Platform != Platform.Win7)
            {
                needConfirmation = true;
            }

            if (version.dwMajorVersion == 6 &&
                (version.dwMinorVersion == 2 || version.dwMinorVersion == 3) &&
                m_style.Platform != Platform.Win8 &&
                m_style.Platform != Platform.Win81)
            {
                needConfirmation = true;
            }

            if (version.dwMajorVersion == 10 &&
                version.dwMinorVersion == 0 &&
                version.dwBuildNumber < 22000 &&
                m_style.Platform != Platform.Win10)
            {
                needConfirmation = true;
            }

            if (version.dwMajorVersion == 10 &&
                version.dwMinorVersion == 0 &&
                version.dwBuildNumber >= 22000 &&
                m_style.Platform != Platform.Win11)
            {
                needConfirmation = true;
            }

            if (needConfirmation)
            {
                if (MessageBox.Show("It looks like the style was not made for this windows version. Try to apply it anyways?"
                    , "msstyleEditor"
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            try
            {
                System.Threading.Thread.Sleep(250); // prevent doubleclicks
                m_themeManager.ApplyTheme(m_style);
                SetTestThemeButtonState(m_themeManager.IsThemeInUse);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "msstyleEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnImageViewBackgroundChange(object sender, EventArgs e)
        {
            // we want to change the color
            if (sender == btImageBgWhite)
            {
                m_imageView.ViewBackColor = Color.White;
                btImageBgWhite.Checked = true;
            }
            else if (sender == btImageBgGrey)
            {
                m_imageView.ViewBackColor = Color.LightGray;
                btImageBgGrey.Checked = true;
            }
            else if (sender == btImageBgBlack)
            {
                m_imageView.ViewBackColor = Color.Black;
                btImageBgBlack.Checked = true;
            }
            else if (sender == btImageBgChecker)
            {
                m_imageView.ViewBackColor = Color.MediumVioletRed;
                btImageBgChecker.Checked = true;
            }
        }

        private void OnImageViewBackgroundChanged(object sender, EventArgs e)
        {
            // we get notified because someone else changed the color
            if (m_imageView.ViewBackColor == Color.White) btImageBgWhite.Checked = true;
            if (m_imageView.ViewBackColor == Color.LightGray) btImageBgGrey.Checked = true;
            if (m_imageView.ViewBackColor == Color.Black) btImageBgBlack.Checked = true;
            if (m_imageView.ViewBackColor == Color.MediumVioletRed) btImageBgChecker.Checked = true; // hack
        }

        private void OnImageExport(object sender, EventArgs e)
        {
            if (m_selectedImage == null)
            {
                MessageBox.Show("Select an image first!", "Export Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (m_selectedImage.Data == null)
            {
                MessageBox.Show("This image resource doesn't exist yet!", "Export Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                string suggestedName = String.Format("{0}_{1}_{2}.png",
                    m_selection.Class.ClassName,
                    m_selection.Part.PartName,
                    m_selectedImage.ResourceId.ToString());

                foreach (var c in Path.GetInvalidFileNameChars())
                {
                    suggestedName = suggestedName.Replace(c, '-');
                }

                sfd.Title = "Export Image";
                sfd.Filter = "PNG Image (*.png)|*.png";
                sfd.FileName = suggestedName;
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    if(m_selectedImage.Type == StyleResourceType.Image)
                    {
                        // The IMAGE resources are non-standard PNGs images. The color channels
                        // are premultiplied by alpha already even though the PNG spec dictates
                        // straight alpha. We have to correct this, otherwise image viewers will
                        // be confused and results will be off. PREMUL -> STRAIGHT.
                        using (var ms = new MemoryStream(m_selectedImage.Data))
                        {
                            Bitmap bmp;
                            libmsstyle.ImageConverter.PremulToStraightAlpha(ms, out bmp);
                            bmp.Save(sfd.FileName);
                        }
                    }
                    else
                    {
                        // The images in the ATLAS resource are standard, straight-alpha, PNG images.
                        // This makes sense insofar, as those are used in the DWM classes which seem
                        // to be a bit more special. KEEP AS-IS.
                        using (var fs = File.Create(sfd.FileName + "asis.png"))
                    {
                        fs.Write(m_selectedImage.Data, 0, m_selectedImage.Data.Length);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error saving image!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UpdateStatusText("Image exported successfully!");
            }
        }

        private void OnImageImport(object sender, EventArgs e)
        {
            if (m_selectedImage == null)
            {
                MessageBox.Show("Select an image to replace first!", "Replace Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Replace Image";
                ofd.Filter = "PNG Image (*.png)|*.png";
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var res = ImageFormats.IsImageSupported(ofd.FileName);
                if (!res.Item1)
                {
                    MessageBox.Show("Bad image:\n" + res.Item2, "Replace Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                var fname = Path.GetRandomFileName() + Path.GetExtension(ofd.FileName);
                var tempFolder = Path.Combine(Path.GetTempPath(), "msstyleEditor");
                Directory.CreateDirectory(tempFolder);
                var tempFile = Path.Combine(tempFolder, fname);
                if (m_selectedImage.Type == StyleResourceType.Image)
                {
                    // IMAGE resources must be saved with premultiplied alpha channel.
                    using (var ifs = File.OpenRead(ofd.FileName))
                    using (var ofs = File.Create(tempFile))
                    {
                        Bitmap bmp;
                        libmsstyle.ImageConverter.PremultiplyAlpha(ifs, out bmp);
                        bmp.Save(ofs, ImageFormat.Png);
                    }
                }
                else
                {
                    // STREAM resources must be saved with straight alpha channel.
                    File.Copy(ofd.FileName, tempFile, true);
                }

                m_style.QueueResourceUpdate(m_selectedImage.ResourceId, m_selectedImage.Type, tempFile);
                // TODO: update image view?
            }
        }

        private void OnPropertyAdd(object sender, EventArgs e)
        {
            m_propertyView.ShowPropertyAddDialog();
        }

        private void OnPropertyAdded(object prop)
        {
            if (prop is StyleProperty styleProp)
            {
                // refresh gui to account for new image property
                if (styleProp.IsImageProperty())
                {
                    DisplayPart(m_selection.Class, m_selection.Part);
                }
            }

            if (prop is Animation animation)
            {
                m_selectedAnimation = new AnimationTypeDescriptor(animation);
                m_classView.Refresh();
                m_propertyView.SetStylePart(null, null, null);
            }

            if (prop is TimingFunction timingFunc)
            {
                m_selectedTimingFunction = timingFunc;
                m_classView.Refresh();
                m_propertyView.SetStylePart(null, null, null);
            }
        }

        private void OnPropertyRemove(object sender, EventArgs e)
        {
            m_propertyView.RemoveSelectedProperty();
        }

        private void OnPropertyRemoved(object prop)
        {
            if(prop is StyleProperty styleProp)
            {
                // refresh gui to account for removed image property
                if (styleProp.IsImageProperty())
                {
                    DisplayPart(m_selection.Class, m_selection.Part);
                }
            }
            else if (prop is Animation || prop is TimingFunction)
            {
                m_classView.Refresh();
                m_propertyView.SetStylePart(null, null, null);
            }
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            if (!m_searchDialog.Visible)
            {
                m_searchDialog.Show(this);
                if (m_searchDialog.StartPosition == FormStartPosition.CenterParent)
                {
                    var x = Location.X + (Width - m_searchDialog.Width) / 2;
                    var y = Location.Y + (Height - m_searchDialog.Height) / 2;
                    m_searchDialog.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
                }
            }
        }

        private void OnSearchNextItem(SearchDialog.SearchMode mode, IDENTIFIER type, string search)
        {
            if (m_style == null)
                return;

            object searchObj = null;
            if (mode == SearchDialog.SearchMode.Property)
            {
                searchObj = MakeObjectFromSearchString(type, search);
                if (searchObj == null)
                {
                    string typeString = VisualStyleProperties.PROPERTY_INFO_MAP[(int)type].Name;
                    MessageBox.Show($"\"{search}\" doesn't seem to be a valid {typeString} property!", ""
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Warning);
                    return;
                }
            }

            // includeSelectedNode = false, because we would get stuck.
            var node = m_classView.FindNextNode(false, (_node) =>
            {
                switch (mode)
                {
                    case SearchDialog.SearchMode.Name:
                        return _node.Text.ToUpper().Contains(search.ToUpper());
                    case SearchDialog.SearchMode.Property:
                        {
                            StylePart part = _node.Tag as StylePart;
                            if (part != null)
                            {
                                return part.States.Any((kvp) =>
                                {
                                    return kvp.Value.Properties.Any((p) =>
                                    {
                                        return p.Header.typeID == (int)type &&
                                            p.GetValue().Equals(searchObj);
                                    });
                                });
                            }
                        }
                        return false;
                    default: return false;
                }
            });

            if(node == null)
            {
                MessageBox.Show($"No further match for \"{search}\" !\nSearch will begin from top again.", ""
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
            }
        }

        private void OnReplaceItem(SearchDialog.ReplaceMode mode, IDENTIFIER type, string search, string replacement)
        {
            if (m_style == null)
                return;

            var searchObj = MakeObjectFromSearchString(type, search);
            if (searchObj == null)
            {
                string typeString = VisualStyleProperties.PROPERTY_INFO_MAP[(int)type].Name;
                MessageBox.Show($"\"{search}\" doesn't seem to be a valid {typeString} property!", ""
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Warning);
                return;
            }

            var replacementObj = MakeObjectFromSearchString(type, replacement);
            if (replacementObj == null)
            {
                string typeString = VisualStyleProperties.PROPERTY_INFO_MAP[(int)type].Name;
                MessageBox.Show($"\"{replacementObj}\" doesn't seem to be a valid {typeString} property!", ""
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Warning);
                return;
            }

            // includeSelectedNode = true, since we replace the matches we can't get stuck.
            // Also, we need to exhaust all matches of the nodes.
            var node = m_classView.FindNextNode(true, (_node) =>
            {
                StylePart part = _node.Tag as StylePart;
                if (part != null)
                {
                    return part.States.Any((kvp) =>
                    {
                        return kvp.Value.Properties.Any((p) =>
                        {
                            bool isMatch = p.Header.typeID == (int)type &&
                                           p.GetValue().Equals(searchObj);
                            if (isMatch)
                            {
                                p.SetValue(replacementObj);
                            }

                            return isMatch;
                        });
                    });
                }
                else return false;
            });

            if (node == null)
            {
                MessageBox.Show($"No further match for \"{search}\" !\nSearch & replace will begin from top again.", ""
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
            }
        }

        private object MakeObjectFromSearchString(IDENTIFIER type, string search)
        {
            string ns = search.Replace(" ", "");
            string[] components = ns.Split(new char[] { ',', ';' }); ;

            try
            {
                switch (type)
                {
                    case IDENTIFIER.SIZE:
                    case IDENTIFIER.FILENAME:
                    case IDENTIFIER.FONT:
                        {
                            if (components.Length != 1) return null;
                            return Convert.ToInt32(components[0]);
                        }
                    case IDENTIFIER.POSITION:
                        {
                            if (components.Length != 2) return null;
                            return new Size(
                                Convert.ToInt32(components[0]),
                                Convert.ToInt32(components[1]));
                        }
                    case IDENTIFIER.COLOR:
                        {
                            if (components.Length != 3) return null;
                            return Color.FromArgb(
                                Convert.ToInt32(components[0]),
                                Convert.ToInt32(components[1]),
                                Convert.ToInt32(components[2]));
                        }
                    case IDENTIFIER.MARGINS:
                    case IDENTIFIER.RECTTYPE:
                        {
                            if (components.Length != 4) return null;
                            return new Margins(
                                Convert.ToInt32(components[0]),
                                Convert.ToInt32(components[1]),
                                Convert.ToInt32(components[2]),
                                Convert.ToInt32(components[3]));
                        }
                    default: return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void OnDocumentationClicked(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/nptr/msstyleEditor/wiki/Introduction");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error opening help page!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLicenseClicked(object sender, EventArgs e)
        {
            var dlg = new LicenseDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog();
        }

        private void OnAboutClicked(object sender, EventArgs e)
        {
            var dlg = new AboutDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog();
        }

        #endregion

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                OnSearchClicked(this, null);
                return true;
            }
            else if (keyData >= (Keys.Control | Keys.D1) && keyData <= (Keys.Control | Keys.D9))
            {
                Keys numberKey = keyData & ~Keys.Control;
                m_imageView.SetActiveTabIndex((int)numberKey - 0x30 - 1);
            }

            return base.ProcessDialogKey(keyData);
        }

        private void OnMainWindowLoad(object sender, EventArgs e)
        {
            RegistrySettings settings = new RegistrySettings();
            if (!settings.HasConfirmedWarning)
            {
                if (MessageBox.Show("Modifying themes can break the operating system!\r\n\r\n" +
                    "Make sure you have a recent system restore point. Only proceed if you understand " +
                    "the risk and can deal with technical problems."

                    , "msstyleEditor - Risk Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    settings.HasConfirmedWarning = true;
                }
                else
                {
                    Close();
                }
            }
        }

        private void OnImageSelectIndex(object sender, EventArgs e)
        {
            if (m_selection.Part == null)
            {
                return;
            }

            var it = m_selection.Part.GetImageProperties();
            var imgProp = it.ElementAtOrDefault(m_imageView.SelectedIndex);
            if (imgProp != null)
            {
                m_selectedImage = UpdateImageView(imgProp);
            }
        }

        private void OnSystemColorsChanged(object sender, EventArgs e)
        {
            // HACK: In order to set the new theme for the dockpanel, we'd have to
            // close all windows and recreate them (with all the state). So instead,
            // we only fix the most prominent mismatch manually, which is the dockpanel
            // backcolor(s).
            dockPanel.Theme.ColorPalette.MainWindowActive.Background = SystemColors.Control;
            foreach (Control c in dockPanel.Controls)
            {
                c.BackColor = SystemColors.Control;
            }
        }
    }
}
