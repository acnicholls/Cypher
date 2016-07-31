namespace cypher.GUI
{
    partial class frmPare
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
            this.lbWordList = new System.Windows.Forms.ListBox();
            this.btnRTL = new System.Windows.Forms.Button();
            this.btnLTR = new System.Windows.Forms.Button();
            this.lbWordsToRemove = new System.Windows.Forms.ListBox();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWordNumber = new System.Windows.Forms.Label();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbWordList
            // 
            this.lbWordList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbWordList.FormattingEnabled = true;
            this.lbWordList.Location = new System.Drawing.Point(11, 102);
            this.lbWordList.Name = "lbWordList";
            this.lbWordList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbWordList.Size = new System.Drawing.Size(200, 355);
            this.lbWordList.TabIndex = 0;
            // 
            // btnRTL
            // 
            this.btnRTL.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRTL.Location = new System.Drawing.Point(217, 102);
            this.btnRTL.Name = "btnRTL";
            this.btnRTL.Size = new System.Drawing.Size(75, 170);
            this.btnRTL.TabIndex = 1;
            this.btnRTL.Text = ">>>";
            this.btnRTL.UseVisualStyleBackColor = true;
            this.btnRTL.Click += new System.EventHandler(this.btnLTR_Click);
            // 
            // btnLTR
            // 
            this.btnLTR.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnLTR.Location = new System.Drawing.Point(218, 278);
            this.btnLTR.Name = "btnLTR";
            this.btnLTR.Size = new System.Drawing.Size(75, 180);
            this.btnLTR.TabIndex = 2;
            this.btnLTR.Text = "<<<";
            this.btnLTR.UseVisualStyleBackColor = true;
            this.btnLTR.Click += new System.EventHandler(this.btnRTL_Click);
            // 
            // lbWordsToRemove
            // 
            this.lbWordsToRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbWordsToRemove.FormattingEnabled = true;
            this.lbWordsToRemove.Location = new System.Drawing.Point(302, 102);
            this.lbWordsToRemove.Name = "lbWordsToRemove";
            this.lbWordsToRemove.Size = new System.Drawing.Size(200, 355);
            this.lbWordsToRemove.TabIndex = 3;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExit,
            this.miRemove});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(514, 24);
            this.mnuMain.TabIndex = 4;
            this.mnuMain.Text = "menuStrip1";
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(37, 20);
            this.miExit.Text = "Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miRemove
            // 
            this.miRemove.Name = "miRemove";
            this.miRemove.Size = new System.Drawing.Size(99, 20);
            this.miRemove.Text = "Remove Words";
            this.miRemove.Click += new System.EventHandler(this.miRemove_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(489, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "Use this form to remove words from the list of matches to pare down the possible " +
    "permutations of the message.";
            // 
            // lblWordNumber
            // 
            this.lblWordNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordNumber.Location = new System.Drawing.Point(11, 60);
            this.lblWordNumber.Name = "lblWordNumber";
            this.lblWordNumber.Size = new System.Drawing.Size(491, 39);
            this.lblWordNumber.TabIndex = 6;
            this.lblWordNumber.Text = "label2";
            // 
            // frmPare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 470);
            this.Controls.Add(this.lblWordNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbWordsToRemove);
            this.Controls.Add(this.btnLTR);
            this.Controls.Add(this.btnRTL);
            this.Controls.Add(this.lbWordList);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmPare";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paring Form";
            this.Load += new System.EventHandler(this.frmPare_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbWordList;
        private System.Windows.Forms.Button btnRTL;
        private System.Windows.Forms.Button btnLTR;
        private System.Windows.Forms.ListBox lbWordsToRemove;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem miRemove;
        private System.Windows.Forms.Label lblWordNumber;
    }
}