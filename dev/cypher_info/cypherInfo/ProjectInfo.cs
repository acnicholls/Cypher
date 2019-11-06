using System;
using System.Reflection;
using System.Web;

namespace cypher.info
{
	/// <summary>
	/// solution wide information
	/// </summary>
	public class ProjectInfo
	{
		public ProjectInfo()
		{}

        public static bool Testing
        {
            get
            {
#if DEBUG
                return true;
#endif
#if !DEBUG
                return false;
#endif
            }
        }
        public static bool EncryptCancel = false;
		public static string Version
		{
			get
			{
				Assembly a = Assembly.GetExecutingAssembly();
				AssemblyName an = a.GetName();
				return Convert.ToString(an.Version);
			}
		}
		public static string Language = "C#";
		public static string Author = "AC Nicholls";
		public static string Name = "Reddit Decryption Attempt";
		public static string Manufacturer = "AC Nicholls";
		public static string Copyright = "2013";
		public static string Description = "DESCRIPTION: my attempt to decrypt a code I saw on reddit";


		// Program variables
        public static LogTypeEnum ProjectLogType = LogTypeEnum.File;
		public static string ProjectLogProcedure = "cypher_insertLog";
		public static string acnLogLocation = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\log";

        public static string ModelLocation
        {
            get
            {
                string conn = "";
                conn = cypher.info.AppSettings.GetAppSetting("ModelLocation", false);
                return conn;
            }
        }

	} 
}
