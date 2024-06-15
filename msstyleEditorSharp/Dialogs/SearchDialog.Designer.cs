
namespace msstyleEditor
{
    partial class SearchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchDialog));
            this.cbItemType = new System.Windows.Forms.ComboBox();
            this.cbDataType = new System.Windows.Forms.ComboBox();
            this.tbSearchText = new System.Windows.Forms.TextBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.tbReplaceText = new System.Windows.Forms.TextBox();
            this.btReplaceAll = new System.Windows.Forms.Button();
            this.btReplaceNext = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbItemType
            // 
            this.cbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItemType.FormattingEnabled = true;
            this.cbItemType.ItemHeight = 13;
            this.cbItemType.Items.AddRange(new object[] {
            "Class or Part",
            "Property"});
            this.cbItemType.Location = new System.Drawing.Point(7, 12);
            this.cbItemType.Name = "cbItemType";
            this.cbItemType.Size = new System.Drawing.Size(108, 21);
            this.cbItemType.TabIndex = 0;
            this.cbItemType.SelectedIndexChanged += new System.EventHandler(this.OnItemTypeChanged);
            // 
            // cbDataType
            // 
            this.cbDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataType.FormattingEnabled = true;
            this.cbDataType.Items.AddRange(new object[] {
            "COLOR",
            "MARGINS",
            "SIZE",
            "POSITION",
            "RECTANGLE",
            "FILENAME",
            "FONT"});
            this.cbDataType.Location = new System.Drawing.Point(121, 12);
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.Size = new System.Drawing.Size(94, 21);
            this.cbDataType.TabIndex = 1;
            this.cbDataType.SelectedIndexChanged += new System.EventHandler(this.OnDataTypeChanged);
            // 
            // tbSearchText
            // 
            this.tbSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchText.Location = new System.Drawing.Point(7, 52);
            this.tbSearchText.Name = "tbSearchText";
            this.tbSearchText.Size = new System.Drawing.Size(137, 20);
            this.tbSearchText.TabIndex = 2;
            // 
            // btSearch
            // 
            this.btSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSearch.Image = ((System.Drawing.Image)(resources.GetObject("btSearch.Image")));
            this.btSearch.Location = new System.Drawing.Point(150, 51);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(65, 22);
            this.btSearch.TabIndex = 3;
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.OnSearchClicked);
            // 
            // tbReplaceText
            // 
            this.tbReplaceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReplaceText.Location = new System.Drawing.Point(7, 78);
            this.tbReplaceText.Name = "tbReplaceText";
            this.tbReplaceText.Size = new System.Drawing.Size(137, 20);
            this.tbReplaceText.TabIndex = 4;
            // 
            // btReplaceAll
            // 
            this.btReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btReplaceAll.Image = ((System.Drawing.Image)(resources.GetObject("btReplaceAll.Image")));
            this.btReplaceAll.Location = new System.Drawing.Point(183, 77);
            this.btReplaceAll.Name = "btReplaceAll";
            this.btReplaceAll.Size = new System.Drawing.Size(32, 22);
            this.btReplaceAll.TabIndex = 6;
            this.btReplaceAll.UseVisualStyleBackColor = true;
            this.btReplaceAll.Visible = false;
            this.btReplaceAll.Click += new System.EventHandler(this.OnReplaceAllClicked);
            // 
            // btReplaceNext
            // 
            this.btReplaceNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btReplaceNext.Image = ((System.Drawing.Image)(resources.GetObject("btReplaceNext.Image")));
            this.btReplaceNext.Location = new System.Drawing.Point(150, 77);
            this.btReplaceNext.Name = "btReplaceNext";
            this.btReplaceNext.Size = new System.Drawing.Size(65, 22);
            this.btReplaceNext.TabIndex = 5;
            this.btReplaceNext.UseVisualStyleBackColor = true;
            this.btReplaceNext.Click += new System.EventHandler(this.OnReplaceNextClicked);
            // 
            // SearchDialog
            // 
            this.AcceptButton = this.btSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 109);
            this.Controls.Add(this.btReplaceNext);
            this.Controls.Add(this.btReplaceAll);
            this.Controls.Add(this.tbReplaceText);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.tbSearchText);
            this.Controls.Add(this.cbDataType);
            this.Controls.Add(this.cbItemType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SearchDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Find";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbItemType;
        private System.Windows.Forms.ComboBox cbDataType;
        private System.Windows.Forms.TextBox tbSearchText;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.TextBox tbReplaceText;
        private System.Windows.Forms.Button btReplaceAll;
        private System.Windows.Forms.Button btReplaceNext;
    }
}
