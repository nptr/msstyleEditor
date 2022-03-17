
namespace msstyleEditor.Dialogs
{
    partial class ImageView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageView));
            this.imageControl = new msstyleEditor.ImageControl();
            this.imageViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.whiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbImage1 = new System.Windows.Forms.ToolStripButton();
            this.tsbImage2 = new System.Windows.Forms.ToolStripButton();
            this.tsbImage3 = new System.Windows.Forms.ToolStripButton();
            this.tsbImage4 = new System.Windows.Forms.ToolStripButton();
            this.tsbImage5 = new System.Windows.Forms.ToolStripButton();
            this.tsbImage6 = new System.Windows.Forms.ToolStripButton();
            this.tsbImage7 = new System.Windows.Forms.ToolStripButton();
            this.tsbImage8 = new System.Windows.Forms.ToolStripButton();
            this.imageViewContextMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageControl
            // 
            this.imageControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageControl.AutoScroll = true;
            this.imageControl.AutoScrollMinSize = new System.Drawing.Size(243, 188);
            this.imageControl.Background = msstyleEditor.ImageControl.BackgroundStyle.Chessboard;
            this.imageControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageControl.ContextMenuStrip = this.imageViewContextMenu;
            this.imageControl.HighlightArea = new System.Drawing.Rectangle(2, 2, 10, 10);
            this.imageControl.Location = new System.Drawing.Point(0, 0);
            this.imageControl.MaxZoom = 8F;
            this.imageControl.MinZoom = 0.5F;
            this.imageControl.Name = "imageControl";
            this.imageControl.Size = new System.Drawing.Size(245, 190);
            this.imageControl.TabIndex = 2;
            this.imageControl.TabStop = true;
            this.imageControl.ZoomFactor = 1F;
            this.imageControl.BackColorChanged += new System.EventHandler(this.OnImageControlBackColorChanged);
            // 
            // imageViewContextMenu
            // 
            this.imageViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whiteToolStripMenuItem,
            this.greyToolStripMenuItem,
            this.blackToolStripMenuItem,
            this.checkerToolStripMenuItem});
            this.imageViewContextMenu.Name = "imageViewContextMenu";
            this.imageViewContextMenu.Size = new System.Drawing.Size(118, 92);
            // 
            // whiteToolStripMenuItem
            // 
            this.whiteToolStripMenuItem.CheckOnClick = true;
            this.whiteToolStripMenuItem.Name = "whiteToolStripMenuItem";
            this.whiteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.whiteToolStripMenuItem.Text = "White";
            this.whiteToolStripMenuItem.Click += new System.EventHandler(this.OnImageViewBackgroundChange);
            // 
            // greyToolStripMenuItem
            // 
            this.greyToolStripMenuItem.CheckOnClick = true;
            this.greyToolStripMenuItem.Name = "greyToolStripMenuItem";
            this.greyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.greyToolStripMenuItem.Text = "Grey";
            this.greyToolStripMenuItem.Click += new System.EventHandler(this.OnImageViewBackgroundChange);
            // 
            // blackToolStripMenuItem
            // 
            this.blackToolStripMenuItem.CheckOnClick = true;
            this.blackToolStripMenuItem.Name = "blackToolStripMenuItem";
            this.blackToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.blackToolStripMenuItem.Text = "Black";
            this.blackToolStripMenuItem.Click += new System.EventHandler(this.OnImageViewBackgroundChange);
            // 
            // checkerToolStripMenuItem
            // 
            this.checkerToolStripMenuItem.Checked = true;
            this.checkerToolStripMenuItem.CheckOnClick = true;
            this.checkerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkerToolStripMenuItem.Name = "checkerToolStripMenuItem";
            this.checkerToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.checkerToolStripMenuItem.Text = "Checker";
            this.checkerToolStripMenuItem.Click += new System.EventHandler(this.OnImageViewBackgroundChange);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbImage1,
            this.tsbImage2,
            this.tsbImage3,
            this.tsbImage4,
            this.tsbImage5,
            this.tsbImage6,
            this.tsbImage7,
            this.tsbImage8});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 196);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(245, 25);
            this.toolStrip.TabIndex = 5;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbImage1
            // 
            this.tsbImage1.Checked = true;
            this.tsbImage1.CheckOnClick = true;
            this.tsbImage1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbImage1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImage1.Image = ((System.Drawing.Image)(resources.GetObject("tsbImage1.Image")));
            this.tsbImage1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImage1.Name = "tsbImage1";
            this.tsbImage1.Size = new System.Drawing.Size(23, 22);
            this.tsbImage1.Tag = 1;
            this.tsbImage1.Text = "1";
            this.tsbImage1.Click += new System.EventHandler(this.OnToolButtonClicked);
            // 
            // tsbImage2
            // 
            this.tsbImage2.CheckOnClick = true;
            this.tsbImage2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImage2.Image = ((System.Drawing.Image)(resources.GetObject("tsbImage2.Image")));
            this.tsbImage2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImage2.Name = "tsbImage2";
            this.tsbImage2.Size = new System.Drawing.Size(23, 22);
            this.tsbImage2.Tag = 2;
            this.tsbImage2.Text = "2";
            this.tsbImage2.Click += new System.EventHandler(this.OnToolButtonClicked);
            // 
            // tsbImage3
            // 
            this.tsbImage3.CheckOnClick = true;
            this.tsbImage3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImage3.Image = ((System.Drawing.Image)(resources.GetObject("tsbImage3.Image")));
            this.tsbImage3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImage3.Name = "tsbImage3";
            this.tsbImage3.Size = new System.Drawing.Size(23, 22);
            this.tsbImage3.Tag = 3;
            this.tsbImage3.Text = "3";
            this.tsbImage3.Click += new System.EventHandler(this.OnToolButtonClicked);
            // 
            // tsbImage4
            // 
            this.tsbImage4.CheckOnClick = true;
            this.tsbImage4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImage4.Image = ((System.Drawing.Image)(resources.GetObject("tsbImage4.Image")));
            this.tsbImage4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImage4.Name = "tsbImage4";
            this.tsbImage4.Size = new System.Drawing.Size(23, 22);
            this.tsbImage4.Tag = 4;
            this.tsbImage4.Text = "4";
            this.tsbImage4.Click += new System.EventHandler(this.OnToolButtonClicked);
            // 
            // tsbImage5
            // 
            this.tsbImage5.CheckOnClick = true;
            this.tsbImage5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImage5.Image = ((System.Drawing.Image)(resources.GetObject("tsbImage5.Image")));
            this.tsbImage5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImage5.Name = "tsbImage5";
            this.tsbImage5.Size = new System.Drawing.Size(23, 22);
            this.tsbImage5.Tag = 5;
            this.tsbImage5.Text = "5";
            this.tsbImage5.Click += new System.EventHandler(this.OnToolButtonClicked);
            // 
            // tsbImage6
            // 
            this.tsbImage6.CheckOnClick = true;
            this.tsbImage6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImage6.Image = ((System.Drawing.Image)(resources.GetObject("tsbImage6.Image")));
            this.tsbImage6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImage6.Name = "tsbImage6";
            this.tsbImage6.Size = new System.Drawing.Size(23, 22);
            this.tsbImage6.Tag = 6;
            this.tsbImage6.Text = "6";
            this.tsbImage6.Click += new System.EventHandler(this.OnToolButtonClicked);
            // 
            // tsbImage7
            // 
            this.tsbImage7.CheckOnClick = true;
            this.tsbImage7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImage7.Image = ((System.Drawing.Image)(resources.GetObject("tsbImage7.Image")));
            this.tsbImage7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImage7.Name = "tsbImage7";
            this.tsbImage7.Size = new System.Drawing.Size(23, 22);
            this.tsbImage7.Tag = 7;
            this.tsbImage7.Text = "7";
            this.tsbImage7.Click += new System.EventHandler(this.OnToolButtonClicked);
            // 
            // tsbImage8
            // 
            this.tsbImage8.CheckOnClick = true;
            this.tsbImage8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbImage8.Image = ((System.Drawing.Image)(resources.GetObject("tsbImage8.Image")));
            this.tsbImage8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImage8.Name = "tsbImage8";
            this.tsbImage8.Size = new System.Drawing.Size(23, 22);
            this.tsbImage8.Tag = 8;
            this.tsbImage8.Text = "8";
            this.tsbImage8.Click += new System.EventHandler(this.OnToolButtonClicked);
            // 
            // ImageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 221);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.imageControl);
            this.HideOnClose = true;
            this.Name = "ImageView";
            this.Text = "Image View";
            this.imageViewContextMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageControl imageControl;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbImage1;
        private System.Windows.Forms.ToolStripButton tsbImage2;
        private System.Windows.Forms.ToolStripButton tsbImage3;
        private System.Windows.Forms.ToolStripButton tsbImage4;
        private System.Windows.Forms.ToolStripButton tsbImage5;
        private System.Windows.Forms.ToolStripButton tsbImage6;
        private System.Windows.Forms.ToolStripButton tsbImage7;
        private System.Windows.Forms.ToolStripButton tsbImage8;
        private System.Windows.Forms.ContextMenuStrip imageViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem whiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkerToolStripMenuItem;
    }
}