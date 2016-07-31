using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace acnicholls.data
{
	/// <summary>
	/// Connection Info holds information regarding the data connection
	/// </summary>
	public class ConnectionInfo
	{

		public ConnectionInfo()
		{
			//
			//
		}

		#region hashCode
		private static string hashCode = "a;fkdjf098f7q340o5nsdkjnb,xcvmybz[c0vbmvq234vc12.,mcvnkj[vx0n89n2c309ch";
		#endregion
		public static string HashCode
		{
			get{return hashCode;}
			set{hashCode = value;}
		}

		public static string ConnectionString
		{
			get
			{
				string conn = "";
				conn = DecryptConnectionString();
				return conn;
			}
		}

		public static string dataServer
		{
			get
			{
				string conn = "";
				conn = DecryptDataServer();
				return conn;
			}
		}

		public static bool Testing
		{
			get
			{
				if(dataServer == "test" || dataServer =="local")
					return true;
				return false;
			}
		}

		public static string WebServer
		{
			get{return GetWebServer();}
		}

		// This method is used to decrypt the connection string 
		private static string DecryptConnectionString()
		{
			Byte[] b = Convert.FromBase64String(ConfigurationSettings.AppSettings["ConnectionString"]);
			string decryptedConnectionString = System.Text.ASCIIEncoding.ASCII.GetString(b); 
			return decryptedConnectionString;
		}

		// This method is used to decrypt the connection string 
		private static string DecryptDataServer()
		{
			Byte[] b = Convert.FromBase64String(ConfigurationSettings.AppSettings["dataServer"]);
			string decryptedDataServer = System.Text.ASCIIEncoding.ASCII.GetString(b); 
			return decryptedDataServer;
		}

		private static string GetWebServer()
		{
			string tagValue = ConfigurationSettings.AppSettings["webServer"].ToString();
			return tagValue;
		}


		[DllImport("wininet.dll")]
		private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
		public static bool IsConnected()
		{
			int Desc;
			return InternetGetConnectedState(out Desc, 0); 
		}
	}
}
