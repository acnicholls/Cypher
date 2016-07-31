using System.Windows.Forms;

namespace cypher
{
	/// <summary>
	/// this form shows the user that the dictionary is loading
	/// </summary>
	public class frmDialog : System.Windows.Forms.Form
    {
        public Label lblText;
        public TextBox txtInput;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmDialog(bool txt)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            if (txt)
            {
                this.lblText.Visible = false;
                this.txtInput.Visible = true;
            }
            else
            {
                this.lblText.Visible = true;
                this.txtInput.Visible = false;
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDialog));
            this.lblText = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(12, 9);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(192, 13);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Loading Dictionary, Please be patient...";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(15, 6);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(191, 20);
            this.txtInput.TabIndex = 1;
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInput_KeyPress);
            // 
            // frmDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(224, 35);
            this.ControlBox = false;
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Wait...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        /// <summary>
        /// load form's default values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void frmDialog_Load(object sender, System.EventArgs e)
		{
		}

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                this.Hide();
            }
        }
	}
}