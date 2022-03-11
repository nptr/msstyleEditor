using libmsstyle;
using msstyleEditor.Properties;
using msstyleEditor.PropView;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace msstyleEditor
{
    public partial class MainWindow : Form
    {
        private VisualStyle m_style;
        private Selection m_selection = new Selection();
        private StyleResource m_selectedImage = null;
        private StyleProperty m_selectedProp = null;
        private ThemeManager m_themeManager = null;
        private SearchDialog m_searchDialog = new SearchDialog();

        public MainWindow()
        {
            InitializeComponent();

            try 
            {
                m_themeManager = new ThemeManager();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nIs Aero/DWM disabled? Starting without \"Test Theme\" feature.", 
                    "Themeing API unavailable", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                btTestTheme.Enabled = false;
            }

            classView.TreeViewNodeSorter = new TreeRootSorter();
            SetTestThemeButtonState(false);
            CloseStyle();

            m_searchDialog.StartPosition = FormStartPosition.CenterParent;
            m_searchDialog.OnSearch += this.OnSearchNextItem;
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
            if(img == null)
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
            catch(Exception ex)
            {
                MessageBox.Show(this, $"Are you sure this is a Windows Vista or higher visual style?\r\n\r\nDetails: {ex.Message}", "Error loading style!", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            FillClassView();

            btFileSave.Enabled = true;
            btFileInfoExport.Enabled = true;
            btImageExport.Enabled = true;
            btImageImport.Enabled = true;
            //btPropertyExport.Enabled = true;
            //btPropertyImport.Enabled = true;
            btPropertyAdd.Enabled = true;
            btPropertyRemove.Enabled = true;
            btTestTheme.Enabled = true;

            if (m_style.StringTable?.Count == 0)
                btFileSave.Style = RibbonButtonStyle.Normal;
            else btFileSave.Style = RibbonButtonStyle.SplitDropDown;

            lbStylePlatform.Text = m_style.Platform.ToDisplayString();
        }

        private void CloseStyle()
        {
            classView.BeginUpdate();
            classView.Nodes.Clear();
            classView.EndUpdate();

            propertyView.SelectedObject = null;

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

        private void FillClassView()
        {
            classView.BeginUpdate();
            foreach (var cls in m_style.Classes)
            {
                var clsNode = new TreeNode(cls.Value.ClassName);
                clsNode.Tag = cls.Value;

                foreach (var part in cls.Value.Parts)
                {
                    var partNode = new TreeNode(part.Value.PartName);
                    partNode.Tag = part.Value;
                    
                    clsNode.Nodes.Add(partNode);
                }
                classView.Nodes.Add(clsNode);
            }
            classView.Sort();
            classView.EndUpdate();
        }

        private void UpdatePropertyView(StylePart part)
        {
            propertyView.SelectedObject = (part != null)
                ? new TypeDescriptor(part, m_style)
                : null;
        }

        private StyleResource UpdateImageView(StyleProperty prop)
        {
            if (prop == null)
            {
                imageView.BackgroundImage = null;
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

            if (!String.IsNullOrEmpty(file))
            {
                imageView.BackgroundImage = Image.FromFile(file);
            }
            else
            {
                if (resource?.Data != null)
                {
                    imageView.BackgroundImage = Image.FromStream(new MemoryStream(resource.Data));
                }
            }

            UpdateImageInfo(imageView.BackgroundImage);
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

            if(sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                if(sender == btFileSave)
                    m_style.Save(sfd.FileName, true);
                else if(sender == btFileSaveWithMUI)
                    m_style.Save(sfd.FileName, false);

                lbStatusMessage.Text = "Style saved successfully!";
            }
            catch(Exception ex)
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
                UpdateItemSelection(cls);
                UpdatePropertyView(null);
                m_selectedImage = UpdateImageView(null);
                return;
            }

            StylePart part = e.Node.Tag as StylePart;
            if (part != null)
            {
                cls = e.Node.Parent.Tag as StyleClass;
                Debug.Assert(cls != null);

                UpdateItemSelection(cls, part);
                UpdatePropertyView(part);

                // Select first image, or search parent
                // If ATLASRECT, set highlight
                var def = default(KeyValuePair<int, StyleState>);
                var state = part.States.FirstOrDefault();
                if(!state.Equals(def))
                {
                    var rectProp = state.Value.Properties.Find((p) => p.Header.nameID == (int)IDENTIFIER.ATLASRECT);
                    if (rectProp != null)
                    {
                        var mt = rectProp.GetValueAs<Margins>();
                        imageView.HighlightArea = new Rectangle(
                            new Point(mt.Left, mt.Top),
                            new Size(mt.Right - mt.Left, mt.Bottom - mt.Top)
                        );
                    }
                    else imageView.HighlightArea = null;
                }
                else imageView.HighlightArea = null;


                StyleProperty imagePropToShow = null;
                var imgProps = part.GetImageProperties().ToList();
                if (imgProps.Count > 0)
                {
                    imagePropToShow = imgProps[0];
                }
                else if(imagePropToShow == null && cls.Parts[0] != part)
                {
                    imagePropToShow = cls.Parts[0].GetImageProperties().FirstOrDefault();
                }

                m_selectedImage = UpdateImageView(imagePropToShow);
                imageTabs.SetActiveTabs(0, imgProps.Count);

                // TODO: move this elsewhere. experimental code.
                var renderer = new PartRenderer(m_style, part);
                imageView.BackgroundImage = renderer.RenderPreview();

                return;
            }

            StyleProperty prop = e.Node.Tag as StyleProperty;
            if (prop != null)
            {
                part = e.Node.Parent.Tag as StylePart;
                Debug.Assert(part != null);

                cls = e.Node.Parent.Parent.Tag as StyleClass;
                Debug.Assert(cls != null);

                UpdateItemSelection(cls, part, null, prop.Header.shortFlag);
                UpdatePropertyView(part);
                m_selectedImage = UpdateImageView(prop);
                return;
            }
        }

        private void OnTreeExpandClick(object sender, EventArgs e)
        {
            classView.ExpandAll();
        }

        private void OnTreeCollapseClick(object sender, EventArgs e)
        {
            classView.CollapseAll();
        }

        private void OnTestTheme(object sender, EventArgs e)
        {
            if(m_themeManager.IsThemeInUse)
            {
                try
                {
                    m_themeManager.Rollback();
                    SetTestThemeButtonState(m_themeManager.IsThemeInUse);
                    System.Threading.Thread.Sleep(250); // prevent doubleclicks
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
                version.dwMinorVersion >= 0 &&
                m_style.Platform != Platform.Win10)
            {
                needConfirmation = true;
            }

            if (version.dwMajorVersion == 11 &&
                version.dwMinorVersion >= 0 &&
                m_style.Platform != Platform.Win11)
            {
                needConfirmation = true;
            }

            if(needConfirmation)
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
                m_themeManager.ApplyTheme(m_style);
                SetTestThemeButtonState(m_themeManager.IsThemeInUse);
                System.Threading.Thread.Sleep(250); // prevent doubleclicks
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "msstyleEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnImageViewBackgroundChange(object sender, EventArgs e)
        {
            whiteToolStripMenuItem.Checked 
                = greyToolStripMenuItem.Checked 
                = blackToolStripMenuItem.Checked 
                = checkerToolStripMenuItem.Checked 
                = false;

            if (sender == btImageBgWhite || sender == whiteToolStripMenuItem)
            {
                imageView.BackColor = Color.White;
                imageView.Background = ImageControl.BackgroundStyle.Color;
                btImageBgWhite.Checked = whiteToolStripMenuItem.Checked = true;
            }
            else if (sender == btImageBgGrey || sender == greyToolStripMenuItem)
            {
                imageView.BackColor = Color.LightGray;
                imageView.Background = ImageControl.BackgroundStyle.Color;
                btImageBgGrey.Checked = greyToolStripMenuItem.Checked = true;
            }
            else if (sender == btImageBgBlack || sender == blackToolStripMenuItem)
            {
                imageView.BackColor = Color.Black;
                imageView.Background = ImageControl.BackgroundStyle.Color;
                btImageBgBlack.Checked = blackToolStripMenuItem.Checked = true;
            }
            else if (sender == btImageBgChecker || sender == checkerToolStripMenuItem)
            {
                imageView.Background = ImageControl.BackgroundStyle.Chessboard;
                btImageBgChecker.Checked = checkerToolStripMenuItem.Checked = true;
            }
            imageView.Refresh();
        }

        private void OnImageExport(object sender, EventArgs e)
        {
            if(m_selectedImage == null)
            {
                MessageBox.Show("Select an image first!", "Export Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                string suggestedName = String.Format("{0}_{1}_{2}.png",
                    m_selection.Class.ClassName,
                    m_selection.Part.PartName,
                    m_selectedImage.ResourceId.ToString());
                
                foreach(var c in Path.GetInvalidFileNameChars())
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
                    using (var fs = File.Create(sfd.FileName))
                    {
                        fs.Write(m_selectedImage.Data, 0, m_selectedImage.Data.Length);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error saving image!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UpdateStatusText("Image exported successfully!");
            }
        }

        private void OnImageImport(object sender, EventArgs e)
        {
            if(m_selectedImage == null)
            {
                MessageBox.Show("Select an image to replace first!", "Replace Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                m_style.QueueResourceUpdate(m_selection.ResourceId, m_selectedImage.Type, ofd.FileName);
                // TODO: update image view?
            }
        }

        private void OnPropertyAdd(object sender, EventArgs e)
        {
            if(m_selection.State == null)
            {
                MessageBox.Show("Select a state or property within this state first!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dlg = new PropertyDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.Text = "Add Property to " + m_selection.State.StateName;
            var newProp = dlg.ShowDialog();
            if(newProp != null)
            {
                // Add the prop where requested. TODO: test this
                newProp.Header.classID = m_selection.Class.ClassId;
                newProp.Header.partID = m_selection.Part.PartId;
                newProp.Header.stateID = m_selection.State.StateId;

                m_selection.State.Properties.Add(newProp);
                propertyView.Refresh();
            }
        }

        private void OnPropertyRemove(object sender, EventArgs e)
        {
            if (m_selection.State == null ||
                m_selectedProp == null)
            {
                return;
            }

            m_selection.State.Properties.Remove(m_selectedProp);
            propertyView.Refresh();
        }

        private void OnPropertySelected(object sender, SelectedGridItemChangedEventArgs e)
        {
            const int CTX_ADD = 0;
            const int CTX_REM = 1;

            bool haveSelection = e.NewSelection != null;
            propViewContextMenu.Items[CTX_ADD].Enabled = haveSelection;
            propViewContextMenu.Items[CTX_REM].Enabled = haveSelection;
            if (!haveSelection)
                return;

            var propDesc = e.NewSelection.PropertyDescriptor as StylePropertyDescriptor;
            if (propDesc != null)
            {
                propViewContextMenu.Items[CTX_ADD].Text = "Add Property to [" + propDesc.Category + "]";
                propViewContextMenu.Items[CTX_REM].Text = "Remove " + propDesc.Name;

                UpdateItemSelection(m_selection.Class, m_selection.Part, propDesc.StyleState, m_selection.ResourceId);
                m_selectedProp = propDesc.StyleProperty;
                return;
            }

            var dummyDesc = e.NewSelection.PropertyDescriptor as PlaceHolderPropertyDescriptor;
            if (dummyDesc != null)
            {
                propViewContextMenu.Items[CTX_ADD].Enabled = true;
                propViewContextMenu.Items[CTX_ADD].Text = "Add Property to [" + dummyDesc.Category + "]";
                propViewContextMenu.Items[CTX_REM].Enabled = false;
                propViewContextMenu.Items[CTX_REM].Text = "Remove";
                UpdateItemSelection(m_selection.Class, m_selection.Part, dummyDesc.StyleState, m_selection.ResourceId);
                return;
            }

            if (e.NewSelection.GridItemType == GridItemType.Category)
            {
                OnPropertySelected(sender, new SelectedGridItemChangedEventArgs(null, e.NewSelection.GridItems[0])); // select child
                return;
            }

            if (e.NewSelection.GridItemType == GridItemType.Property)
            {
                OnPropertySelected(sender, new SelectedGridItemChangedEventArgs(null, e.NewSelection.Parent)); // select parent
                return;
            }
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            if(!m_searchDialog.Visible)
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

        bool m_endReached = false;
        private void OnSearchNextItem(SearchDialog.SearchMode mode, IDENTIFIER type, string search)
        {
            if (m_style == null)
                return;

            if (String.IsNullOrEmpty(search))
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

            var startItem = classView.SelectedNode;
            if (startItem == null || m_endReached)
            {
                m_endReached = false;
                if (classView.Nodes.Count > 0)
                {
                    startItem = classView.Nodes[0];
                }
                else return;
            }

            var node = TreeViewSearch.FindNextNode(classView, startItem, (_node) =>
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

            if (node != null)
            {
                classView.SelectedNode = node;
            }
            else
            {
                MessageBox.Show($"No further match for \"{search}\" !\nSearch will begin from top again.", ""
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
                m_endReached = true;
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
            catch(Exception)
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

            return base.ProcessDialogKey(keyData);
        }

        private void OnMainWindowLoad(object sender, EventArgs e)
        {
            RegistrySettings settings = new RegistrySettings();
            if(!settings.HasConfirmedWarning)
            {
                if(MessageBox.Show("Modifying themes can break the operating system!\r\n\r\n" +
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
            if(m_selection.Part == null)
            {
                return;
            }

            var it = m_selection.Part.GetImageProperties();
            var imgProp = it.ElementAtOrDefault(imageTabs.SelectedIndex);
            if(imgProp != null)
            {
                UpdateImageView(imgProp);
            }
        }

        private void classView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar >= '1' && e.KeyChar <= '9')
            {
                imageTabs.SetActiveTabIndex(e.KeyChar - 0x30 - 1);
            }
        }
    }
}
