namespace cypher.GUI
{
    partial class ucWords
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblWordValue = new System.Windows.Forms.Label();
            this.lbWords = new System.Windows.Forms.ListBox();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblWordValue
            // 
            this.lblWordValue.AutoSize = true;
            this.lblWordValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordValue.Location = new System.Drawing.Point(6, 5);
            this.lblWordValue.Name = "lblWordValue";
            this.lblWordValue.Size = new System.Drawing.Size(103, 13);
            this.lblWordValue.TabIndex = 0;
            this.lblWordValue.Text = "Word Value : xxx";
            // 
            // lbWords
            // 
            this.lbWords.FormattingEnabled = true;
            this.lbWords.Location = new System.Drawing.Point(3, 51);
            this.lbWords.Name = "lbWords";
            this.lbWords.Size = new System.Drawing.Size(149, 134);
            this.lbWords.TabIndex = 2;
            // 
            // txtFilter
            // 
            this.txtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter.ForeColor = System.Drawing.Color.DimGray;
            this.txtFilter.Location = new System.Drawing.Point(3, 25);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(95, 20);
            this.txtFilter.TabIndex = 0;
            this.txtFilter.Text = "<Filter...>";
            this.txtFilter.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyUp);
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Location = new System.Drawing.Point(105, 21);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(47, 23);
            this.btnClearSearch.TabIndex = 1;
            this.btnClearSearch.Text = "Clear";
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
            // 
            // ucWords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClearSearch);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.lbWords);
            this.Controls.Add(this.lblWordValue);
            this.Name = "ucWords";
            this.Size = new System.Drawing.Size(156, 187);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.ListBox lbWords;
        private System.Windows.Forms.Label lblWordValue;
    }
}
