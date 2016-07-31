using System;
using System.Runtime.InteropServices;

namespace cypher.info
{
	/// <summary>
	/// Connection Info holds information regarding the data connection
	/// </summary>
	public class ConnectionInfo
	{

		public ConnectionInfo()
		{
		}

        /// <summary>
        /// returns string representing database connection string
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string conn = "";
                conn = cypher.info.AppSettings.GetAppSetting("ConnectionString", false);
                return conn;
            }
        }

        /// <summary>
        /// returns boolean value representing connected state of network Internet connection
        /// </summary>
        /// <param name="Description"></param>
        /// <param name="ReservedValue"></param>
        /// <returns></returns>
		[DllImport("wininet.dll")]
		private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
		public static bool IsConnected()
		{
			int Desc;
			return InternetGetConnectedState(out Desc, 0); 
		}
	}
}
