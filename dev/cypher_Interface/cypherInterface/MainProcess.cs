using System;
using System.Windows.Forms;
namespace cypher.GUI
{
	/// <summary>
	/// Main Thread of Program.
	/// </summary>
	public class MainProcess
	{
		public MainProcess()
		{

		}
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
				Application.Run(new frmMain(args));
		}

	}
}
