using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace cypher.GUI
{
    public partial class frmPare : Form
    {
        cypher.data.classes.EncryptedMessage _message;
        SqlConnection parCon = new SqlConnection(info.ConnectionInfo.ConnectionString);

        public frmPare(cypher.data.classes.EncryptedMessage inMessage)
        {
            InitializeComponent();
            _message = inMessage;
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRTL_Click(object sender, EventArgs e)
        {
            object item = this.lbWordsToRemove.SelectedItem;
            this.lbWordList.Items.Add(item);
            this.lbWordsToRemove.Items.Remove(item);
        }

        private void btnLTR_Click(object sender, EventArgs e)
        {
            foreach (object selectedItem in this.lbWordList.SelectedItems)
            {
                this.lbWordsToRemove.Items.Add(selectedItem);
            }
            foreach (object item in this.lbWordsToRemove.Items)
            {
                this.lbWordList.Items.Remove(item);
            }
        }

        private void frmPare_Load(object sender, EventArgs e)
        {
            // load all the words associated with this message into the left box
            try
            {
                SqlCommand parComm = parCon.CreateCommand();
                parComm.CommandType = CommandType.StoredProcedure;
                parComm.CommandText = "cypher_selectMessageWords";
                SqlParameter param1 = new SqlParameter("@filter", false);
                SqlParameter param2 = new SqlParameter("@value", 1);
                SqlParameter param3 = new SqlParameter("@messageID", _message.ID);
                parComm.Parameters.Add(param1);
                parComm.Parameters.Add(param2);
                parComm.Parameters.Add(param3);
                SqlDataAdapter adap = new SqlDataAdapter(parComm);
                DataSet ds = new DataSet();
                parCon.Open();
                adap.Fill(ds);
                // fill the listbox with the values from the database
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    this.lbWordList.Items.Add(row["fldWord_word"]);
                }
                setWordNumberText();
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "frmPare_Load", x, LogEnum.Critical);
            }
            finally
            {
                parCon.Close();
            }
        }

        private void miRemove_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand parComm = parCon.CreateCommand();
                parComm.CommandType = CommandType.StoredProcedure;
                parComm.CommandText = "cypher_deleteMessageWord";
                SqlParameter param = new SqlParameter("@messageID", this._message.ID);
                parCon.Open();
                foreach (object word in this.lbWordsToRemove.Items)
                {
                    parComm.Parameters.Clear();
                    SqlParameter param1 = new SqlParameter("@word", word.ToString());
                    parComm.Parameters.Add(param1);
                    parComm.Parameters.Add(param);
                    parComm.ExecuteNonQuery();
                }
                this.lbWordsToRemove.Items.Clear();
                setWordNumberText();
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "miRemove_Click", x, LogEnum.Critical);
            }
            finally
            {
                parCon.Close();
            }
        }

        private void setWordNumberText()
        {
            this.lblWordNumber.Text = "There are " + this.lbWordList.Items.Count.ToString() + " words in the list.";
        }
    }
}
