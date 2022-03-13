
namespace msstyleEditor.Dialogs
{
    partial class RenderView
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
            this.imageControl = new msstyleEditor.ImageControl();
            this.SuspendLayout();
            // 
            // imageControl
            // 
            this.imageControl.AutoScroll = true;
            this.imageControl.AutoScrollMinSize = new System.Drawing.Size(377, 365);
            this.imageControl.Background = msstyleEditor.ImageControl.BackgroundStyle.Chessboard;
            this.imageControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imageControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageControl.HighlightArea = null;
            this.imageControl.Location = new System.Drawing.Point(0, 0);
            this.imageControl.MaxZoom = 8F;
            this.imageControl.MinZoom = 0.5F;
            this.imageControl.Name = "imageControl";
            this.imageControl.Size = new System.Drawing.Size(379, 367);
            this.imageControl.TabIndex = 3;
            this.imageControl.TabStop = true;
            this.imageControl.ZoomFactor = 1F;
            // 
            // RenderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 367);
            this.Controls.Add(this.imageControl);
            this.HideOnClose = true;
            this.Name = "RenderView";
            this.Text = "Render Preview";
            this.ResumeLayout(false);

        }

        #endregion

        private ImageControl imageControl;
    }
}