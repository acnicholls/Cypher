using System;
using System.Data.SqlClient;
using System.Data;

namespace cypher.data.classes
{
    /// <summary>
    /// string containing numbers relating to words
    /// </summary>
    public class EncryptedMessage
    {

        private int id;
        public int ID
        {
            get { return id; }
        }
        private string content;
        public string Content
        {
            get { return content; }
        }
        public EncryptedMessage() { }

        public EncryptedMessage(int id)
        {
            try
            {
                SqlConnection decConn = new SqlConnection(info.ConnectionInfo.ConnectionString);
                SqlCommand decComm = decConn.CreateCommand();
                /// insert the message into the database
                decComm.CommandType = CommandType.StoredProcedure;
                decComm.CommandText = "cypher_selectMessage";
                SqlParameter param = new SqlParameter("@id", id);
                decComm.Parameters.Add(param);
                decConn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(decComm);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                decConn.Close();
                this.content = ds.Tables[0].Rows[0]["fldMessage_content"].ToString();
                this.id = id;
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "EncryptedMessage /w id", x, LogEnum.Critical);
            }
        }
        public EncryptedMessage(string content)
        {
            this.content = content;
            // try to find this message in the database and pull back the ID if found
            try
            {
                SqlConnection decConn = new SqlConnection(info.ConnectionInfo.ConnectionString);
                SqlCommand decComm = decConn.CreateCommand();
                /// insert the message into the database
                decComm.CommandType = CommandType.Text;
                decComm.CommandText = "select fldMessage_id from tblMessages where LTRIM(RTRIM(fldMessage_content))='" + content.Trim() + "'";
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "EncryptedMessage /w content", decComm.CommandText.ToString(), LogEnum.Debug);

                decConn.Open();
                object returnValue = decComm.ExecuteScalar();
                decConn.Close();
                this.id = Convert.ToInt16(returnValue);
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "EncryptedMessage /w content", x, LogEnum.Critical);
            }
        }

        public void Save()
        {
            // should try to take care of update here...?   
            try
            {
                // add this message to the database
                SqlConnection decConn = new SqlConnection(info.ConnectionInfo.ConnectionString);
                SqlCommand decComm = decConn.CreateCommand();
                /// insert the message into the database
                decComm.CommandType = CommandType.StoredProcedure;
                decComm.CommandText = "cypher_insertMessage";
                SqlParameter param = new SqlParameter("@content", this.content);
                SqlParameter param1 = new SqlParameter("@id",this.id);
                param1.Direction = ParameterDirection.Output;
                decComm.Parameters.Add(param);
                decComm.Parameters.Add(param1);
                decConn.Open();
                decComm.ExecuteNonQuery();
                // now build a list of all the words that calc to values in this message
                decComm.CommandText = "cypher_insertMessageWords";
                param = new SqlParameter("@messageID", this.id);
                param1 = new SqlParameter("@array", this.content.Replace(" ", ","));
                decComm.Parameters.Clear();
                decComm.Parameters.Add(param);
                decComm.Parameters.Add(param1);
                decComm.ExecuteNonQuery();
                decConn.Close();
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "EncryptedMessage.Save", x, LogEnum.Critical);
            }
        }
    }
}
