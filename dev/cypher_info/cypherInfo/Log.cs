using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;


namespace cypher
{	//////	Debug = 0,
	//////	Message = 1,
	//////	Important = 2,
	//////	Access = 3,
	//////	Warning = 4,
	//////	Error = 5,
	//////	Critical = 6

	/// <summary>
    /// contains methods for writing to log
	///</summary>
	public class Log
	{
		// sends debug messages to file if LogEnum = Debug
		private static bool debugToFile = true;
		// will only allow to send messages to log if LogEnum >= this number
		private static int maxLogEnum = 0;

		private static SqlConnection conACN;
		private static SqlCommand comACN;

		public static bool SetDebug
		{
			set
			{
				   debugToFile = value;
			 }
		}
		
		public Log()
		{}
		public static void WriteToLog(LogTypeEnum type, string caller, string message, LogEnum import)
		{
#if !DEBUG

            if (import != LogEnum.Debug)
            {
#endif
			    switch(type)
			    {
				    case LogTypeEnum.File:
				    {
					    WriteToFile(caller, message, import);
					    break;
				    }
				    case LogTypeEnum.Database:
				    {
					    WriteToSQL(caller, message, import);
					    break;
				    }
			    }
#if !DEBUG
            }
#endif
#if DEBUG
                if (import != LogEnum.Debug)
                    WriteToLog(type, caller, message, LogEnum.Debug);
#endif
		}

		public static void WriteToLog(LogTypeEnum type, string caller, Exception ex, LogEnum import)
		{
#if !DEBUG
			if(import != LogEnum.Debug)
          {
#endif
			    switch(type)
			    {
				    case LogTypeEnum.File:
				    {
					    WriteToFile(caller, ex, import);
					    break;
				    }
				    case LogTypeEnum.Database:
				    {
					    WriteToSQL(caller, ex, import);
					    break;
				    }
			    }
#if !DEBUG
            }
#endif
#if DEBUG
            if(import != LogEnum.Debug)
                WriteToLog(type, caller, ex, LogEnum.Debug);
#endif
		}

		private static void WriteToSQL(string caller, Exception ex, LogEnum import)
		{
			try
			{
				string strMessage = ex.Message.ToString() + "\r\n";
				strMessage += ex.StackTrace.ToString();
				WriteToDatabase(caller, strMessage, Convert.ToInt32(import));
			}
			catch(Exception x)
			{
                cypher.Log.WriteToFile("WriteToSQL with exception", x.Message, LogEnum.Error);
			}
		}

		private static void WriteToDatabase(string caller, string message, int import)
		{
			try
			{
				conACN = new SqlConnection(cypher.info.ConnectionInfo.ConnectionString);
				comACN = conACN.CreateCommand();
				comACN.CommandType = CommandType.StoredProcedure;
				comACN.CommandText = cypher.info.ProjectInfo.ProjectLogProcedure;
				comACN.Parameters.AddWithValue("@caller", caller);
				comACN.Parameters.AddWithValue("@message", message);
				comACN.Parameters.AddWithValue("@import", import);
				if(conACN.State != ConnectionState.Open)
					conACN.Open();
				int result = comACN.ExecuteNonQuery();
				conACN.Close();		
			}
			catch(Exception x)
			{
                cypher.Log.WriteToFile("WriteToDatabase", x.Message, LogEnum.Error);
			}

		}


		private static void WriteToSQL(string caller, string message, LogEnum import)
		{
			try
			{
				WriteToDatabase(caller, message, Convert.ToInt32(import));
			}
			catch(Exception x)
			{
				cypher.Log.WriteToFile( "WriteToSQL with message", x.Message, LogEnum.Error);
			}
		}

        private static void WriteToFile(string caller, string message, LogEnum import)
        {
            string fileName = cypher.info.ProjectInfo.acnLogLocation + @"\" + import.ToString() + ".txt";
            CheckFolderExists(fileName);

            StreamWriter logFile = new StreamWriter(fileName, true);
            logFile.WriteLine(DateTime.Now + ", " + caller + ", " + message);
            logFile.Flush();
            logFile.Close();
        }

        private static void WriteToFile(string caller, Exception ex, LogEnum import)
		{
			string fileName = cypher.info.ProjectInfo.acnLogLocation + @"\" + import.ToString() + ".txt";
			CheckFolderExists(fileName);

			StreamWriter logFile = new StreamWriter(fileName,true);
            logFile.WriteLine(DateTime.Now + ", " + caller + ", " + "-------------------------------------------------------------------\r\n");
            logFile.WriteLine(DateTime.Now + ", " + caller + ", " + ex.Message + "\r\n");
            logFile.WriteLine(DateTime.Now + ", " + caller + ", " + ex.StackTrace.ToString() + "\r\n");
            logFile.WriteLine(DateTime.Now + ", " + caller + ", " + "-------------------------------------------------------------------\r\n");
			logFile.Flush();
			logFile.Close();
		}





		private static bool CheckFolderExists(string fileName)
		{	
			FileInfo file = new FileInfo(fileName);
			DirectoryInfo folder = file.Directory;
			try
			{
				if(folder.Exists)
					return true;
				else
				{
					folder.Create();
					return true;
				}
			}
			catch(Exception x)
			{
				Log.WriteToFile("Check for Folder", x.Message,LogEnum.Error);
			}
			return false;

		}

	}
}
