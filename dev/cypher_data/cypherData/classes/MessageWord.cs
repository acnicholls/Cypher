using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace cypher.data.classes
{
    /// <summary>
    /// list of words relating to one integer in the message
    /// </summary>
    public class MessageWord
    {
        public ArrayList words = new ArrayList();
        SqlConnection wordCon = new SqlConnection(info.ConnectionInfo.ConnectionString);

        public MessageWord(int messageID, int value)
        {
            // find all words for this value and fill the arraylist
            try
            {
                SqlCommand parComm = wordCon.CreateCommand();
                parComm.CommandType = CommandType.StoredProcedure;
                parComm.CommandText = "cypher_selectMessageWords";
                SqlParameter param1 = new SqlParameter("@filter", true);
                SqlParameter param2 = new SqlParameter("@value", value);
                SqlParameter param3 = new SqlParameter("@messageID", messageID);
                parComm.Parameters.Add(param1);
                parComm.Parameters.Add(param2);
                parComm.Parameters.Add(param3);
                SqlDataAdapter adap = new SqlDataAdapter(parComm);
                DataSet ds = new DataSet();
                wordCon.Open();
                adap.Fill(ds);
                wordCon.Close();
                // fill the listbox with the values from the database
                words.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    words.Add(row["fldWord_word"]);
                }
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "MessageWord", x, LogEnum.Critical);
            }
            finally
            {
                if (wordCon.State != ConnectionState.Closed)
                {
                    wordCon.Close();
                }
            }
        }
    }
}
