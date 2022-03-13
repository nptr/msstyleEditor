
namespace msstyleEditor.Dialogs
{
    partial class ClassViewWindow
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
            this.classView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // classView
            // 
            this.classView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.classView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classView.HideSelection = false;
            this.classView.Location = new System.Drawing.Point(0, 0);
            this.classView.Name = "classView";
            this.classView.Size = new System.Drawing.Size(222, 427);
            this.classView.TabIndex = 1;
            this.classView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeItemSelected);
            // 
            // ClassViewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 427);
            this.Controls.Add(this.classView);
            this.HideOnClose = true;
            this.Name = "ClassViewWindow";
            this.Text = "Class View";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView classView;
    }
}