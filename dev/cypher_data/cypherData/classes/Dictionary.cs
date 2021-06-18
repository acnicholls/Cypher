using System;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace cypher.data.classes
{
    public class Dictionary
    {
        public cypher.data.datasets.WordsAndValues WAV = new datasets.WordsAndValues();
        SqlConnection cypherCon = new SqlConnection(cypher.info.ConnectionInfo.ConnectionString);

       public Dictionary()
        {
            // open the dictionary
            ReadFileOrDatabase();
        }

        /// <summary>
        /// reads the dictionary file into a table of words and corresponding integer values
        /// </summary>
        private void ReadFileOrDatabase()
        {
            try
            {
                object numOfRows = 0;
                SqlCommand rowCount = cypherCon.CreateCommand();
                rowCount.CommandType = System.Data.CommandType.Text;
                rowCount.CommandText = "select count(*) from tblWords";
                cypherCon.Open();
                numOfRows = rowCount.ExecuteScalar();
                cypherCon.Close();
                if ((int)numOfRows == 0)
                {
                    string FilePath = cypher.info.AppSettings.GetAppSetting("dictionary", false);
                    cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "ReadFileOrDatabase", FilePath, LogEnum.Debug);
                    // open the file
                    StreamReader sr = new StreamReader(FilePath);
                    do
                    {
                        string word = sr.ReadLine();
                        AddWordToDatabase(word);
                    } while (sr.Peek() != -1);
                    // close the file
                    sr.Close();
                    cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "ReadFileOrDatabase", "File Closed", LogEnum.Debug);
                }
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "ReadFileOrDatabase", x, LogEnum.Critical);
                throw x;
            }
        }

        /// <summary>
        /// Adds a specified word to the database
        /// </summary>
        /// <param name="word">string value containing the word to add</param>
        private void AddWordToDatabase(string word)
        {
            if (!CheckDatabaseForWord(word))
            {
                try
                {
                    cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "AddWordToDatabase", "Adding Word : " + word, LogEnum.Debug);
                    // pass the data to the database
                    int wordValue = CalcWordValue(word);
                    SqlCommand cypherCom = cypherCon.CreateCommand();
                    cypherCom.CommandText = "cypher_insertWord";
                    cypherCom.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter inputWord = new SqlParameter("@word", word);
                    SqlParameter inputWordValue = new SqlParameter("@value", wordValue);
                    cypherCom.Parameters.Add(inputWord);
                    cypherCom.Parameters.Add(inputWordValue);
                    cypherCon.Open();
                    int result = cypherCom.ExecuteNonQuery();
                    cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "AddWordToDatabase", "Row Added : " + word + " : " + wordValue + " : Result : " + result.ToString(), LogEnum.Debug);
                }
                catch (Exception x)
                {
                    Log.WriteToLog(info.ProjectInfo.ProjectLogType, "AddWordToDatabase", x, LogEnum.Critical);
                }
                finally
                {
                    cypherCon.Close();
                }
            }
        }

        /// <summary>
        /// Checks the database to see if the word is in the dictionary already
        /// </summary>
        /// <param name="word">string value containing the word to check for</param>
        /// <returns>true if word exists</returns>
        private bool CheckDatabaseForWord(string word)
        {
            bool returnValue = false;
            try
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckDatabaseForWord", "Checking Word : " + word, LogEnum.Debug);
                // pass the data to the database
                SqlCommand cypherCom = cypherCon.CreateCommand();
                cypherCom.CommandText = "Select count(*) from tblWords where fldWord_word='" + word + "'";
                cypherCom.CommandType = System.Data.CommandType.Text;
                cypherCon.Open();
                object result = cypherCom.ExecuteScalar();
                if ((int)result == 1)
                    returnValue = true;
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckDatabaseForWord", x, LogEnum.Critical);
            }
            finally
            {
                cypherCon.Close();
            }
            return returnValue;
        }

        /// <summary>
        /// finds the integer value related to the input word
        /// </summary>
        /// <param name="word">word to retrieve the value for</param>
        /// <returns>integer value corresponding to the sum of all letter values</returns>
        public int FindWordValue(string word)
        {
            object returnValue = 0;
            try
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "FindWordValue", "Searching for word : " + word, LogEnum.Debug);
                SqlCommand wordFind = cypherCon.CreateCommand();
                wordFind.CommandType = System.Data.CommandType.Text;
                wordFind.CommandText = "select fldWord_value from tblWords where fldWord_word='" + word + "'";
                cypherCon.Open();
                returnValue = wordFind.ExecuteScalar();
                if (returnValue == null)
                {
                    DialogResult result = MessageBox.Show("The word '" + word + "' was not found.  Would you like to add it to the database? Click Cancel to return to your message.", "Word Not Found...", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        AddWordToDatabase(word);
                        returnValue = FindWordValue(word);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        cypher.info.ProjectInfo.EncryptCancel = true;
                        returnValue = 0;
                    }
                    else if (result == DialogResult.No)
                    {
                        returnValue = 0;
                    }
                }
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "FindWordValue", "Found Value : " + Convert.ToInt16(returnValue).ToString(), LogEnum.Debug);
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "FindWordValue", x, LogEnum.Critical);
            }
            finally
            {
                cypherCon.Close();
            }
            return (int)returnValue;
        }

        /// <summary>
        /// finds the words associated with a certain value
        /// </summary>
        /// <param name="value">integer representing a word</param>
        /// <returns>words from the dictionary with the corresponding integer value</returns>
        public ArrayList FindValueWord(int value)
        {
            ArrayList returnValue = new ArrayList();
            int result = 0;
            try
            {
                SqlCommand wordFind = cypherCon.CreateCommand();
                SqlDataAdapter adapFind = new SqlDataAdapter(wordFind);
                wordFind.CommandType = System.Data.CommandType.Text;
                wordFind.CommandText = "select * from tblWords where fldWord_value=" + value;
                cypherCon.Open();
                adapFind.Fill(WAV, "tblWords");
                foreach (cypher.data.datasets.WordsAndValues.tblWordsRow row in WAV.tblWords)
                {
                    if (Convert.ToInt16(row["fldWord_value"]) == value)
                    {
                        string input = row["fldWord_word"].ToString();
                        result = returnValue.Add(input);
                    }
                }
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "FindValueWord", "Found " + result.ToString() + " words when searching for value " + value.ToString(), LogEnum.Debug);
                WAV.Clear();
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "FindValueWord", x, LogEnum.Critical);
            }
            finally
            {
                cypherCon.Close();
            }
            return returnValue;
        }

        /// <summary>
        /// calculates the sum of the letters of the word
        /// </summary>
        /// <param name="inputWord"></param>
        public int CalcWordValue(string inputWord)
        {
            try
            {
                int wordSum = 0;
                foreach (char c in inputWord.ToCharArray())
                {
                    wordSum += CheckForLetterValue(c);
                }
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CalcWordValue", "Word Value for : " + inputWord + " : " + wordSum.ToString(), LogEnum.Debug);

                return wordSum;
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CalcWordValue", x, LogEnum.Critical);
                return 0;
            }
        }

        /// <summary>
        /// returns the numeric value of the input letter
        /// </summary>
        /// <param name="inputLetter">char value of letter to transpose to numeric</param>
        /// <returns>numeric value of input letter</returns>
        private int CheckForLetterValue(char inputLetter)
        {
            //cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckForLetterValue", inputLetter.ToString(), LogEnum.Debug);
            letterValue x = (letterValue)inputLetter;
            //cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckForLetterValue", x.ToString(), LogEnum.Debug);
            int returnValue = (int)x - 96;
            //cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "CheckForLetterValue", returnValue.ToString(), LogEnum.Debug);
            return returnValue;
        }
    }
}
