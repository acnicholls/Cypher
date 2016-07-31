//Copyright (C) 2005 Richard J. Northedge
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace EnglishTokenizer
{
	/// <summary>
	/// Summary description for TokenizerForm.
	/// </summary>
	public class TokenizerForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Button btnTokenize;
		private System.Windows.Forms.TextBox txtOutput;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;

		private MaxentTokenizer mTokenizer;

		private const string mModelPath = @"C:\Projects\DotNet\OpenNLP\OpenNLP\Models\";

		public TokenizerForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            string modelFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\EnglishTok.nbin";
            modelFile = new System.Uri(modelFile).LocalPath;
            mTokenizer = new MaxentTokenizer(modelFile);
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
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnTokenize = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Location = new System.Drawing.Point(8, 16);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(368, 80);
            this.txtInput.TabIndex = 0;
            this.txtInput.Text = "SharpEntropy is based upon the latest version (2.3.0) of the MaxEnt toolkit, rele" +
                "ased in August 2004.";
            // 
            // btnTokenize
            // 
            this.btnTokenize.Location = new System.Drawing.Point(8, 104);
            this.btnTokenize.Name = "btnTokenize";
            this.btnTokenize.Size = new System.Drawing.Size(75, 23);
            this.btnTokenize.TabIndex = 1;
            this.btnTokenize.Text = "Tokenize";
            this.btnTokenize.Click += new System.EventHandler(this.btnTokenize_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(8, 136);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(368, 288);
            this.txtOutput.TabIndex = 2;
            // 
            // TokenizerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(384, 430);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnTokenize);
            this.Controls.Add(this.txtInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TokenizerForm";
            this.Text = "English Tokenizer Example";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new TokenizerForm());
		}

		private void btnTokenize_Click(object sender, System.EventArgs e)
		{      
            string[] tokens = mTokenizer.Tokenize(txtInput.Text);
			txtOutput.Text = string.Join("\r\n", tokens);
		}		
	}
}
