using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleExample
{
	/// <summary>
	/// Summary description for SimpleExample.
	/// </summary>
	public class SimpleExample : System.Windows.Forms.Form
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new SimpleExample());
		}

		private System.Windows.Forms.ComboBox cboTemperature;
		private System.Windows.Forms.Label lblTemperature;
		private System.Windows.Forms.Label lblPrecipitation;
		private System.Windows.Forms.ComboBox cboPrecipitation;
		private System.Windows.Forms.ComboBox cboTiming;
		private System.Windows.Forms.Label lblTiming;
		private System.Windows.Forms.Label lblOutcome;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnOutcome;

		private SharpEntropy.GisModel mModel;
		private int mUmbrellaOutcomeId;

		public SimpleExample()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			cboTemperature.SelectedIndex = 0;
			cboPrecipitation.SelectedIndex = 0;
			cboTiming.SelectedIndex = 0;

			string trainingDataFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\data.txt";
			trainingDataFile = new System.Uri(trainingDataFile).LocalPath;

			System.IO.StreamReader trainingStreamReader = new System.IO.StreamReader(trainingDataFile);
			SharpEntropy.ITrainingEventReader eventReader = new SharpEntropy.BasicEventReader(new SharpEntropy.PlainTextByLineDataReader(trainingStreamReader));
			SharpEntropy.GisTrainer trainer = new SharpEntropy.GisTrainer();
			trainer.TrainModel(eventReader);
			mModel = new SharpEntropy.GisModel(trainer);
	
			mUmbrellaOutcomeId = mModel.GetOutcomeIndex("Umbrella");

			//if we were saving the model to disk, we could use this code
			//string modelDataFile = trainingDataFile.Substring(0,trainingDataFile.LastIndexOf('.')) + "Model.txt";
			//SharpEntropy.IO.PlainTextGisModelWriter writer = new SharpEntropy.IO.PlainTextGisModelWriter();
			//writer.Persist(mModel, modelDataFile);

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
			this.cboTemperature = new System.Windows.Forms.ComboBox();
			this.lblTemperature = new System.Windows.Forms.Label();
			this.lblPrecipitation = new System.Windows.Forms.Label();
			this.cboPrecipitation = new System.Windows.Forms.ComboBox();
			this.cboTiming = new System.Windows.Forms.ComboBox();
			this.lblTiming = new System.Windows.Forms.Label();
			this.lblOutcome = new System.Windows.Forms.Label();
			this.btnOutcome = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cboTemperature
			// 
			this.cboTemperature.Cursor = System.Windows.Forms.Cursors.Default;
			this.cboTemperature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTemperature.Items.AddRange(new object[] {
																"Unknown",
																"Cold",
																"Warm"});
			this.cboTemperature.Location = new System.Drawing.Point(120, 24);
			this.cboTemperature.Name = "cboTemperature";
			this.cboTemperature.Size = new System.Drawing.Size(121, 21);
			this.cboTemperature.TabIndex = 0;
			// 
			// lblTemperature
			// 
			this.lblTemperature.Location = new System.Drawing.Point(8, 24);
			this.lblTemperature.Name = "lblTemperature";
			this.lblTemperature.TabIndex = 1;
			this.lblTemperature.Text = "Temperature";
			// 
			// lblPrecipitation
			// 
			this.lblPrecipitation.Location = new System.Drawing.Point(8, 56);
			this.lblPrecipitation.Name = "lblPrecipitation";
			this.lblPrecipitation.TabIndex = 2;
			this.lblPrecipitation.Text = "Precipitation";
			// 
			// cboPrecipitation
			// 
			this.cboPrecipitation.Cursor = System.Windows.Forms.Cursors.Default;
			this.cboPrecipitation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPrecipitation.Items.AddRange(new object[] {
																  "Unknown",
																  "Dry",
																  "Rainy"});
			this.cboPrecipitation.Location = new System.Drawing.Point(120, 56);
			this.cboPrecipitation.Name = "cboPrecipitation";
			this.cboPrecipitation.Size = new System.Drawing.Size(121, 21);
			this.cboPrecipitation.TabIndex = 3;
			// 
			// cboTiming
			// 
			this.cboTiming.Cursor = System.Windows.Forms.Cursors.Default;
			this.cboTiming.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTiming.Items.AddRange(new object[] {
														   "Unknown",
														   "Early",
														   "Late"});
			this.cboTiming.Location = new System.Drawing.Point(120, 88);
			this.cboTiming.Name = "cboTiming";
			this.cboTiming.Size = new System.Drawing.Size(121, 21);
			this.cboTiming.TabIndex = 5;
			// 
			// lblTiming
			// 
			this.lblTiming.Location = new System.Drawing.Point(8, 88);
			this.lblTiming.Name = "lblTiming";
			this.lblTiming.TabIndex = 4;
			this.lblTiming.Text = "Timing";
			// 
			// lblOutcome
			// 
			this.lblOutcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblOutcome.Location = new System.Drawing.Point(128, 120);
			this.lblOutcome.Name = "lblOutcome";
			this.lblOutcome.Size = new System.Drawing.Size(100, 24);
			this.lblOutcome.TabIndex = 7;
			// 
			// btnOutcome
			// 
			this.btnOutcome.Location = new System.Drawing.Point(8, 120);
			this.btnOutcome.Name = "btnOutcome";
			this.btnOutcome.Size = new System.Drawing.Size(96, 23);
			this.btnOutcome.TabIndex = 8;
			this.btnOutcome.Text = "Take umbrella?";
			this.btnOutcome.Click += new System.EventHandler(this.btnOutcome_Click);
			// 
			// SimpleExample
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(256, 158);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnOutcome,
																		  this.lblOutcome,
																		  this.cboTiming,
																		  this.lblTiming,
																		  this.cboPrecipitation,
																		  this.lblPrecipitation,
																		  this.lblTemperature,
																		  this.cboTemperature});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SimpleExample";
			this.Text = "SimpleExample";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOutcome_Click(object sender, System.EventArgs e)
		{
			ArrayList context = new ArrayList();

			if (cboTemperature.Text != "Unknown")
			{
				context.Add(cboTemperature.Text);
			}
			if (cboPrecipitation.Text != "Unknown")
			{
				context.Add(cboPrecipitation.Text);
			}
			if (cboTiming.Text != "Unknown")
			{
				context.Add(cboTiming.Text);
			}

			double[] probabilities = mModel.Evaluate((string[])context.ToArray(typeof(string)));

			lblOutcome.Text = probabilities[mUmbrellaOutcomeId].ToString("N5");
		}

		

		
	}
}
