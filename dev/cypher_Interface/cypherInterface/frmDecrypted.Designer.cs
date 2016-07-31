namespace cypher.GUI
{
    partial class frmDecrypted
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
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.miMain = new System.Windows.Forms.ToolStripMenuItem();
            this.miInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.miPare = new System.Windows.Forms.ToolStripMenuItem();
            this.miView = new System.Windows.Forms.ToolStripMenuItem();
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miCalc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMain});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1027, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip1";
            // 
            // miMain
            // 
            this.miMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miInfo,
            this.miPare,
            this.miView,
            this.miSave,
            this.miCalc,
            this.toolStripSeparator1,
            this.miExit});
            this.miMain.Name = "miMain";
            this.miMain.Size = new System.Drawing.Size(46, 20);
            this.miMain.Text = "Main";
            // 
            // miInfo
            // 
            this.miInfo.Name = "miInfo";
            this.miInfo.Size = new System.Drawing.Size(196, 22);
            this.miInfo.Text = "View Information Form";
            this.miInfo.Click += new System.EventHandler(this.miInfo_Click);
            // 
            // miPare
            // 
            this.miPare.Name = "miPare";
            this.miPare.Size = new System.Drawing.Size(196, 22);
            this.miPare.Text = "View Paring Form";
            this.miPare.Click += new System.EventHandler(this.miPare_Click);
            // 
            // miView
            // 
            this.miView.Name = "miView";
            this.miView.Size = new System.Drawing.Size(196, 22);
            this.miView.Text = "View Chosen Message";
            this.miView.Click += new System.EventHandler(this.miView_Click);
            // 
            // miSave
            // 
            this.miSave.Name = "miSave";
            this.miSave.Size = new System.Drawing.Size(196, 22);
            this.miSave.Text = "Save All Permutations";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // miCalc
            // 
            this.miCalc.Name = "miCalc";
            this.miCalc.Size = new System.Drawing.Size(196, 22);
            this.miCalc.Text = "Calculate Possibilities";
            this.miCalc.Click += new System.EventHandler(this.miCalc_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(196, 22);
            this.miExit.Text = "Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // frmDecrypted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 603);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmDecrypted";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDecrypted";
            this.Load += new System.EventHandler(this.frmDecrypted_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem miMain;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miInfo;
        private System.Windows.Forms.ToolStripMenuItem miView;
        private System.Windows.Forms.ToolStripMenuItem miPare;
        private System.Windows.Forms.ToolStripMenuItem miSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miCalc;

    }
}