
namespace msstyleEditor
{
    partial class LicenseDialog
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("msstyleEditor");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("RibbonWinForms");
            this.tbLicense = new System.Windows.Forms.TextBox();
            this.lvSelection = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // tbLicense
            // 
            this.tbLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLicense.BackColor = System.Drawing.SystemColors.Control;
            this.tbLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLicense.Location = new System.Drawing.Point(176, 13);
            this.tbLicense.Multiline = true;
            this.tbLicense.Name = "tbLicense";
            this.tbLicense.ReadOnly = true;
            this.tbLicense.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLicense.Size = new System.Drawing.Size(396, 267);
            this.tbLicense.TabIndex = 1;
            // 
            // lvSelection
            // 
            this.lvSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvSelection.FullRowSelect = true;
            this.lvSelection.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSelection.HideSelection = false;
            this.lvSelection.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvSelection.Location = new System.Drawing.Point(13, 13);
            this.lvSelection.Name = "lvSelection";
            this.lvSelection.Scrollable = false;
            this.lvSelection.Size = new System.Drawing.Size(157, 267);
            this.lvSelection.TabIndex = 0;
            this.lvSelection.UseCompatibleStateImageBehavior = false;
            this.lvSelection.View = System.Windows.Forms.View.Tile;
            this.lvSelection.SelectedIndexChanged += new System.EventHandler(this.OnSelectionChanged);
            // 
            // LicenseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 292);
            this.Controls.Add(this.lvSelection);
            this.Controls.Add(this.tbLicense);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "LicenseDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "License";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbLicense;
        private System.Windows.Forms.ListView lvSelection;
    }
}