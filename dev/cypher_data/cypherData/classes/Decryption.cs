using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using OpenNLP.Tools.Parser;
using opennlp.tools.sentdetect;
using System.IO;
using java.io;
using System.Collections.Generic;

namespace cypher.data.classes
{
    public class Decryption
    {
        private static SqlConnection connection = new SqlConnection(info.ConnectionInfo.ConnectionString);
        /// <summary>
        /// creates a table in the database to save the message permutations to
        /// </summary>
        /// <param name="tableName">name of the table to create</param>
        /// <param name="words">number of words the message contains</param>
        /// <param name="drop">whether or not to drop the table first</param>
        public static void CreateTABLE(string tableName, int words, bool drop)
        {
            try
            {
                string sqlsc;
                if (drop)
                {
                    sqlsc = "DROP TABLE " + tableName;
                    SqlCommand dropComm = connection.CreateCommand();
                    dropComm.CommandType = CommandType.Text;
                    dropComm.CommandText = sqlsc;
                    connection.Open();
                    cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CreateTABLE", sqlsc, LogEnum.Debug);
                    dropComm.ExecuteNonQuery();
                    connection.Close();
                }
                sqlsc = "CREATE TABLE " + tableName + "(";
                sqlsc += "\n [PermID] int IDENTITY(1,1) PRIMARY KEY NOT NULL,";
                sqlsc += "\n [Stat] float NULL,";
                sqlsc += "\n [ParsedSentence] varchar(max) NUll,";
                for (int i = 0; i < words; i++)
                {
                    sqlsc += "\n [Box" + i.ToString() + "] ";
                    sqlsc += " nvarchar(50) ";
                    sqlsc += " NULL ";
                    sqlsc += ",";
                }
                sqlsc = sqlsc.Substring(0, sqlsc.Length - 1) + ")";
                //// now open the connection and create the table for this decryption
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sqlsc;
                connection.Open();
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CreateTABLE", sqlsc, LogEnum.Debug);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CreateTABLE", x, LogEnum.Critical);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// fills the table one column at a time
        /// </summary>
        /// <param name="tableName">name of the table to save the data to</param>
        /// <param name="itemList">words to save to the table</param>
        /// <param name="columnNo">the number pertaining to placement in the 'sentence'</param>
        /// <param name="permutations">the number of permutations to save</param>
        public static void FillPermutationsTable(string tableName, ArrayList itemList, int columnNo, double permutations)
        {
            try
            {
                double maxP = Math.Floor(permutations / itemList.Count);
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "FillPermutationsTable", "MaxP = " + maxP.ToString(), LogEnum.Debug);
                int ID = 1;
                for (double p = 1; p <= maxP; p++)
                {
                    foreach (object o in itemList)
                    {
                        string strSQL = "";
                        if (columnNo == 0)
                        {
                            strSQL = "INSERT INTO " + tableName + " (Box" + columnNo.ToString() + ") VALUES ";
                            strSQL += "('" + o.ToString() + "')";
                        }
                        else
                        {
                            strSQL = "UPDATE " + tableName + " SET Box" + columnNo.ToString() + "='" + o.ToString() + "'";
                            strSQL += " WHERE PermID=" + ID.ToString();
                        }
                        SqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = strSQL;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        // add one to the next ID
                        ID++;
                    }
                }
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "FillPermutationsTable", x, LogEnum.Critical);
                throw x;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }

        public static void DetectSentence(string tableName, double permutations)
        {
            try
            {
                if (CheckForPermutationTable(tableName))
                {
                    cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "DetectSentence", "Loading Parser...", LogEnum.Message);
                    // first load the laguage processing tools
                    string modelPath = cypher.info.ProjectInfo.ModelLocation;
                    var modelIn = new FileInputStream(Path.Combine(modelPath, "EnglishSD.nbin"));
                    var model = new SentenceModel(modelIn);
                    var sentenceDetector = new SentenceDetectorME(model);
                    // now load sentences from the database
                    cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "DetectSentence", "Parser Loaded, beginning checking sentences.", LogEnum.Message);
                    for (double id = 1; id <= permutations; id++)
                    {
                        SqlCommand grabSentence = connection.CreateCommand();
                        grabSentence.CommandType = CommandType.Text;
                        grabSentence.CommandText = "Select * from " + tableName + " where PermID=" + id.ToString();
                        SqlDataAdapter adap = new SqlDataAdapter(grabSentence);
                        DataSet ds = new DataSet();
                        connection.Open();
                        adap.Fill(ds);
                        connection.Close();
                        int cols = ds.Tables[0].Columns.Count - 3;
                        string sentence = "";
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            for (int colNo = 0; colNo <= cols; colNo++)
                            {
                                if(colNo == cols-1)
                                    sentence += row["Box" + colNo.ToString()].ToString() + " ";
                                else
                                    sentence += row["Box" + colNo.ToString()].ToString() + ".";
                            }
                            // sentence is built now parse it
                            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "DetectSentence", "Sentence Built : " + sentence, LogEnum.Debug);
                            List<string> sentences = sentenceDetector.sentDetect(sentence).ToList();
                            List<double> probablilities = sentenceDetector.getSentenceProbabilities().ToList();
                            foreach (string sent in sentences)
                            {
                                string parsedSentence = sent;
                                double parsedProb = probablilities[sentences.IndexOf(sent)];
                                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "DetectSentence", "Probability of this being a sentence : " + parsedProb.ToString(), LogEnum.Debug);
                                // now put it in the database
                                SqlCommand detectionInfo = connection.CreateCommand();
                                detectionInfo.CommandType = CommandType.StoredProcedure;
                                detectionInfo.CommandText = "dbo.cypher_insertPermutationSentence";
                                SqlParameter param1 = new SqlParameter("@messageId", tableName.Remove(0, "tblPermutations".Length));
                                SqlParameter param2 = new SqlParameter("@permId", id);
                                SqlParameter param3 = new SqlParameter("@content", sent);
                                SqlParameter param4 = new SqlParameter("@prob", parsedProb);
                                detectionInfo.Parameters.Add(param1);
                                detectionInfo.Parameters.Add(param2);
                                detectionInfo.Parameters.Add(param3);
                                detectionInfo.Parameters.Add(param4);
                                connection.Open();
                                detectionInfo.ExecuteNonQuery();
                                connection.Close();
                            }
                            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "DetectSentence", "Sentence Check Complete.", LogEnum.Debug);
                        }
                    }
                }
                else
                {
                    Exception newEx = new Exception("No table exists to check permutations for, please 'Save All Permutations' first!");
                    throw newEx;
                }
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "DetectSentence", x, LogEnum.Critical);
                throw x;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }

        public static bool CheckForPermutationTable(string tableName)
        {
            bool returnValue = false;
            try
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckForPermutationTable", "Checking for table : " + tableName, LogEnum.Debug);
                SqlCommand comm = connection.CreateCommand();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "cypher_CheckIfTableExists";
                SqlParameter param = new SqlParameter("@tableName", tableName);
                SqlParameter param1 = new SqlParameter("@output", returnValue);
                param1.Direction = ParameterDirection.Output;
                comm.Parameters.Add(param);
                comm.Parameters.Add(param1);
                connection.Open();
                comm.ExecuteNonQuery();
                connection.Close();
                returnValue = Convert.ToBoolean(comm.Parameters["@output"].Value);
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckForPermutationTable", x, LogEnum.Critical);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                { 
                    connection.Close(); 
                }
            }
            return returnValue;
        }

        public static int CheckSizeOfPermutationTable(string tableName)
        {
            int returnValue = 0;
            try
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckSizeOfPermutationTable", "Checking table : " + tableName, LogEnum.Debug);
                SqlCommand comm = connection.CreateCommand();
                comm.CommandType = CommandType.Text;
                string TSQL = "SELECT Total_Rows = SUM(st.row_count) FROM sys.dm_db_partition_stats st WHERE object_name(object_id) = '" + tableName + "' AND (index_id < 2)";
                comm.CommandText = TSQL;
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckSizeOfPermutationTable", "Checking table : " + tableName + " : " + comm.CommandText.ToString(), LogEnum.Message);
                connection.Open();
                object val = comm.ExecuteScalar();
                connection.Close();
                returnValue = Convert.ToInt32(val);
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckSizeOfPermutationTable", x, LogEnum.Critical);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return returnValue;
        }

    }
}
