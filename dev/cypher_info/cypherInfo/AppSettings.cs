using System;
using System.Configuration;

namespace cypher.info
{
	/// <summary>
	/// contains methods for reading/writing values to config files
	/// </summary>
	public class AppSettings
	{
		public AppSettings()
		{
		}

		private static string DecryptAppSetting(string settingName)
		{
			Byte[] b = Convert.FromBase64String(ConfigurationManager.AppSettings[settingName]);
			string decryptedConnectionString = System.Text.ASCIIEncoding.ASCII.GetString(b); 
			return decryptedConnectionString;
		}

		public static string GetAppSetting(string settingName, bool encrypted)
		{
			string returnValue = "";
			if(encrypted)
			{
				returnValue = DecryptAppSetting(settingName);
			}
			else
			{
                returnValue = ConfigurationManager.AppSettings[settingName].ToString();
			}
			return returnValue;
		}

		public static void SetAppSetting(string keyName, string keyValue)
		{
            ConfigurationManager.AppSettings.Set(keyName, keyValue);
		}
	}
}
