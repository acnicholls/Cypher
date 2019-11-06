using System;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace cypher.GUI
{
	/// <summary>
	/// this form allows the user to enter a code to encrypt/decrypt
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
    {
        private IContainer components;
        private System.Windows.Forms.Button btnDecrypt;
        private Panel pnlDecrypt;
        private Panel pnlEncrypt;
        private Button btnEncrypt;
        private TextBox txtOutputCode;
        private TextBox txtInputPhrase;
        private MenuStrip mnuMain;
        private ToolStripMenuItem miMain;
        private ToolStripMenuItem miExit;
        private ToolStripMenuItem miMode;
        private ToolStripMenuItem miEncrypt;
        private ToolStripMenuItem miDecrypt;
        private TextBox txtInputCode;
        private ToolStripMenuItem miLoad;
        private cypher.data.classes.Dictionary dict;

        private int messageID = 0;


		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		public frmMain(string[] arguments)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtInputCode = new System.Windows.Forms.TextBox();
            this.pnlDecrypt = new System.Windows.Forms.Panel();
            this.pnlEncrypt = new System.Windows.Forms.Panel();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtOutputCode = new System.Windows.Forms.TextBox();
            this.txtInputPhrase = new System.Windows.Forms.TextBox();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.miMain = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miMode = new System.Windows.Forms.ToolStripMenuItem();
            this.miEncrypt = new System.Windows.Forms.ToolStripMenuItem();
            this.miDecrypt = new System.Windows.Forms.ToolStripMenuItem();
            this.miLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlDecrypt.SuspendLayout();
            this.pnlEncrypt.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecrypt.Location = new System.Drawing.Point(4, 203);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(557, 42);
            this.btnDecrypt.TabIndex = 8;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // txtInputCode
            // 
            this.txtInputCode.Location = new System.Drawing.Point(4, 3);
            this.txtInputCode.Multiline = true;
            this.txtInputCode.Name = "txtInputCode";
            this.txtInputCode.Size = new System.Drawing.Size(558, 194);
            this.txtInputCode.TabIndex = 9;
            this.txtInputCode.Text = "116 61 111 80 77 104 91 81 78 104 89 88 93 56 66 55 35 79 108 101 81 82 42 127 12" +
    "4 40 48 47 90 87 45 88 51 114 116 118 112 63 58 64";
            this.txtInputCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressNumeric);
            // 
            // pnlDecrypt
            // 
            this.pnlDecrypt.Controls.Add(this.txtInputCode);
            this.pnlDecrypt.Controls.Add(this.btnDecrypt);
            this.pnlDecrypt.Location = new System.Drawing.Point(9, 27);
            this.pnlDecrypt.Name = "pnlDecrypt";
            this.pnlDecrypt.Size = new System.Drawing.Size(565, 253);
            this.pnlDecrypt.TabIndex = 10;
            // 
            // pnlEncrypt
            // 
            this.pnlEncrypt.Controls.Add(this.btnEncrypt);
            this.pnlEncrypt.Controls.Add(this.txtOutputCode);
            this.pnlEncrypt.Controls.Add(this.txtInputPhrase);
            this.pnlEncrypt.Location = new System.Drawing.Point(9, 27);
            this.pnlEncrypt.Name = "pnlEncrypt";
            this.pnlEncrypt.Size = new System.Drawing.Size(565, 253);
            this.pnlEncrypt.TabIndex = 11;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEncrypt.Location = new System.Drawing.Point(49, 133);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(449, 40);
            this.btnEncrypt.TabIndex = 2;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // txtOutputCode
            // 
            this.txtOutputCode.Location = new System.Drawing.Point(3, 179);
            this.txtOutputCode.Multiline = true;
            this.txtOutputCode.Name = "txtOutputCode";
            this.txtOutputCode.Size = new System.Drawing.Size(558, 65);
            this.txtOutputCode.TabIndex = 1;
            // 
            // txtInputPhrase
            // 
            this.txtInputPhrase.Location = new System.Drawing.Point(4, 3);
            this.txtInputPhrase.Multiline = true;
            this.txtInputPhrase.Name = "txtInputPhrase";
            this.txtInputPhrase.Size = new System.Drawing.Size(558, 124);
            this.txtInputPhrase.TabIndex = 0;
            this.txtInputPhrase.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPressAlpha);
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMain,
            this.miMode,
            this.miLoad});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(582, 24);
            this.mnuMain.TabIndex = 12;
            this.mnuMain.Text = "menuStrip1";
            // 
            // miMain
            // 
            this.miMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExit});
            this.miMain.Name = "miMain";
            this.miMain.Size = new System.Drawing.Size(46, 20);
            this.miMain.Text = "Main";
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(93, 22);
            this.miExit.Text = "Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miMode
            // 
            this.miMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEncrypt,
            this.miDecrypt});
            this.miMode.Name = "miMode";
            this.miMode.Size = new System.Drawing.Size(50, 20);
            this.miMode.Text = "Mode";
            // 
            // miEncrypt
            // 
            this.miEncrypt.Name = "miEncrypt";
            this.miEncrypt.Size = new System.Drawing.Size(115, 22);
            this.miEncrypt.Text = "Encrypt";
            this.miEncrypt.Click += new System.EventHandler(this.miEncrypt_Click);
            // 
            // miDecrypt
            // 
            this.miDecrypt.Name = "miDecrypt";
            this.miDecrypt.Size = new System.Drawing.Size(115, 22);
            this.miDecrypt.Text = "Decrypt";
            this.miDecrypt.Click += new System.EventHandler(this.miDecrypt_Click);
            // 
            // miLoad
            // 
            this.miLoad.Name = "miLoad";
            this.miLoad.Size = new System.Drawing.Size(142, 20);
            this.miLoad.Text = "Load Previous Message";
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(582, 287);
            this.Controls.Add(this.pnlEncrypt);
            this.Controls.Add(this.mnuMain);
            this.Controls.Add(this.pnlDecrypt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.mnuMain;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Decryption Attempt";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlDecrypt.ResumeLayout(false);
            this.pnlDecrypt.PerformLayout();
            this.pnlEncrypt.ResumeLayout(false);
            this.pnlEncrypt.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Application.Exit();
        }

        public void btnDecrypt_Click(object sender, EventArgs e)
        {
            ArrayList wordValues = new ArrayList();
            string inputDecrypt = this.txtInputCode.Text.Trim();
            cypher.data.classes.EncryptedMessage mess = new data.classes.EncryptedMessage(inputDecrypt);
            mess.Save();

            frmDecrypted dec = new frmDecrypted(mess);
            dec.ShowDialog(this);
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void miEncrypt_Click(object sender, EventArgs e)
        {
            this.pnlDecrypt.Visible = false;
            this.pnlEncrypt.Visible = true;
            this.miLoad.Visible = false;
        }

        private void miDecrypt_Click(object sender, EventArgs e)
        {
            this.pnlEncrypt.Visible = false;
            this.pnlDecrypt.Visible = true;
            this.miLoad.Visible = true;

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            this.txtOutputCode.Text = "";
            try
            {
                // grab all the words entered
                string inputPhrase = this.txtInputPhrase.Text.Trim();
                string[] phrase = inputPhrase.Split(Convert.ToChar(" "));
                // calculate the word values
                foreach (string s in phrase)
                {
                    int value = dict.FindWordValue(s);
                    if (info.ProjectInfo.EncryptCancel)
                    {
                        info.ProjectInfo.EncryptCancel = false;
                        this.txtOutputCode.Text = "";
                        break;
                    }
                    // output the values to the result box
                    this.txtOutputCode.Text += value.ToString() + " ";
               }

            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "btnEncrypt_Click", x, LogEnum.Critical);
            }
        }


        #region control text entry
        // This event occurs after the KeyDown event and can be used to prevent
        // characters from entering the control.
        private void textBox_KeyPressAlpha(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z\s\b]"))
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }
        // This event occurs after the KeyDown event and can be used to prevent
        // characters from entering the control.
        private void textBox_KeyPressNumeric(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[0-9\s\b]"))
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }
        #endregion

        private void frmMain_Load(object sender, EventArgs e)
        {
            frmDialog dialog = new frmDialog(false);
            dialog.lblText.Text = "Loading Dictionary.  Please Wait...";
            dialog.Show(this);
            dict = new data.classes.Dictionary();
            dialog.Close();
            try
            {
                // now check the DB for messages and add to menu bar
                SqlConnection conn = new SqlConnection(info.ConnectionInfo.ConnectionString);
                SqlCommand comm = conn.CreateCommand();
                DataSet ds = new DataSet();
                SqlDataAdapter adap = new SqlDataAdapter(comm);
                comm.CommandType = CommandType.Text;
                comm.CommandText = "Select fldMessage_id from tblMessages";
                conn.Open();
                adap.Fill(ds);
                conn.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ToolStripMenuItem mi = new ToolStripMenuItem(row[0].ToString());
                    mi.Click += smiLoad_Click;
                    miLoad.DropDownItems.Add(mi);
                }
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "frmMain_Load", x, LogEnum.Critical);
            }
        }

        private void smiLoad_Click(object sender, EventArgs e)
        {
            //Log.WriteToLog(info.ProjectInfo.ProjectLogType, "smiLoad_Click", sender.ToString(), LogEnum.Debug);
            //Log.WriteToLog(info.ProjectInfo.ProjectLogType, "smiLoad_Click", e.ToString(), LogEnum.Debug);

            try
            {
                // load the message from the DB and paste into the text box
                SqlConnection conn = new SqlConnection(info.ConnectionInfo.ConnectionString);
                SqlCommand comm = conn.CreateCommand();
                comm.CommandType = CommandType.Text;
                comm.CommandText = "Select fldMessage_content from tblMessages where fldMessage_id=" + sender.ToString();
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "smiLoad_Click", comm.CommandText, LogEnum.Debug);
                conn.Open();
                object result = comm.ExecuteScalar();
                conn.Close();
                this.txtInputCode.Text = result.ToString();
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "smiLoad_Click", x, LogEnum.Critical);
            }

        }


    }
}
