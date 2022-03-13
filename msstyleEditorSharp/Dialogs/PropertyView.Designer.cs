
namespace msstyleEditor.Dialogs
{
    partial class PropertyViewWindow
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
            this.components = new System.ComponentModel.Container();
            this.propertyView = new System.Windows.Forms.PropertyGrid();
            this.propViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newPropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propViewContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyView
            // 
            this.propertyView.ContextMenuStrip = this.propViewContextMenu;
            this.propertyView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyView.HelpBorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.propertyView.Location = new System.Drawing.Point(0, 0);
            this.propertyView.Name = "propertyView";
            this.propertyView.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyView.Size = new System.Drawing.Size(302, 417);
            this.propertyView.TabIndex = 3;
            this.propertyView.ToolbarVisible = false;
            this.propertyView.ViewBorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.propertyView.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.OnPropertySelected);
            // 
            // propViewContextMenu
            // 
            this.propViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newPropertyToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.propViewContextMenu.Name = "contextMenuStrip1";
            this.propViewContextMenu.Size = new System.Drawing.Size(145, 48);
            // 
            // newPropertyToolStripMenuItem
            // 
            this.newPropertyToolStripMenuItem.Name = "newPropertyToolStripMenuItem";
            this.newPropertyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.newPropertyToolStripMenuItem.Text = "Add Property";
            this.newPropertyToolStripMenuItem.Click += new System.EventHandler(this.OnPropertyAdd);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.OnPropertyRemove);
            // 
            // PropertyViewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 417);
            this.Controls.Add(this.propertyView);
            this.HideOnClose = true;
            this.Name = "PropertyViewWindow";
            this.Text = "Property View";
            this.propViewContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyView;
        private System.Windows.Forms.ContextMenuStrip propViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem newPropertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}