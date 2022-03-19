using libmsstyle;
using msstyleEditor.Dialogs;
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

        public MainWindow()
        {
            InitializeComponent();

            dockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            //dockPanel.Theme.ColorPalette.ToolWindowCaptionActive.Background = SystemColors.ActiveCaption;

            m_classView = new ClassViewWindow();
            m_classView.Show(dockPanel, DockState.DockLeft);
            m_classView.CloseButtonVisible = false;
            m_classView.OnSelectionChanged += OnTreeItemSelected;
            m_classView.Controls[0].KeyPress += OnTreeKeyPress; // very hacky

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

            m_classView.SetVisualStyle(m_style);

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

        private StyleResource UpdateImageView(StyleProperty prop)
        {
            if (prop == null)
            {
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
                m_propertyView.SetStylePart(null, null, null);
                m_selectedImage = UpdateImageView(null);
                return;
            }

            StylePart part = e.Node.Tag as StylePart;
            if (part != null)
            {
                cls = e.Node.Parent.Tag as StyleClass;
                Debug.Assert(cls != null);

                UpdateItemSelection(cls, part);
                m_propertyView.SetStylePart(m_style, cls, part);

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
                        var ha = new Rectangle(
                            new Point(mt.Left, mt.Top),
                            new Size(mt.Right - mt.Left, mt.Bottom - mt.Top)
                        );
                        m_imageView.ViewHighlightArea = ha;
                    }
                    else m_imageView.ViewHighlightArea = null;
                }
                else m_imageView.ViewHighlightArea = null;


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
                m_imageView.SetActiveTabs(0, imgProps.Count);

                // TODO: ugly code..
                var renderer = new PartRenderer(m_style, part);
                m_renderView.Image = renderer.RenderPreview();

                return;
            }
        }

        private void OnTreeKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '1' && e.KeyChar <= '9')
            {
                m_imageView.SetActiveTabIndex(e.KeyChar - 0x30 - 1);
            }
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

                m_style.QueueResourceUpdate(m_selectedImage.ResourceId, m_selectedImage.Type, ofd.FileName);
                // TODO: update image view?
            }
        }

        private void OnPropertyAdd(object sender, EventArgs e)
        {
            m_propertyView.ShowPropertyAddDialog();
        }

        private void OnPropertyRemove(object sender, EventArgs e)
        {
            m_propertyView.RemoveSelectedProperty();
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

        private void OnSearchNextItem(SearchDialog.SearchMode mode, IDENTIFIER type, string search)
        {
            if (m_style == null)
                return;

            if (String.IsNullOrEmpty(search))
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

            m_classView.FindNextNode(mode, type, search, searchObj);
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
            var imgProp = it.ElementAtOrDefault(m_imageView.SelectedIndex);
            if(imgProp != null)
            {
                UpdateImageView(imgProp);
            }
        }
    }
}
