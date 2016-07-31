using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;

namespace cypher.GUI
{
    public partial class frmDecrypted : Form
    {
        
        public static int ColumnsPerRow = 8;
        cypher.GUI.WordBoxArray Boxes;
        cypher.data.classes.EncryptedMessage EncMess;
        cypher.data.classes.MessageWords mWords;
        BackgroundWorker bWorker = new BackgroundWorker();
        frmDialog dialog = new frmDialog(false);

        public frmDecrypted(cypher.data.classes.EncryptedMessage message)
        {
            EncMess = message;
            mWords = new data.classes.MessageWords(message);
            InitializeComponent();
        }

        private void frmDecrypted_Load(object sender, EventArgs e)
        {
            LoadBoxes();
        }

        private void SetFormSize()
        {
            int numOfBoxes = Boxes.Count;
            if (numOfBoxes < ColumnsPerRow)
            {
                this.Height = 20 + Boxes.Spacing + Boxes.Height + 35;
                this.Width = (numOfBoxes * Boxes.Width) + 15;
            }
            else
            {
                // calc the numofboxes per row times the width plus spacing
                // plus space at the beginning of each row
                this.Width = (ColumnsPerRow * Boxes.Width) + 15;
                // how many rows
                decimal rows = decimal.Floor((decimal)numOfBoxes/(decimal)ColumnsPerRow);
                // add the height of each row of boxes, plus spacing.
                // plus the height of the menu strip plus spacing
                // plus spacing at the bottom of the form.
                this.Height = (Boxes.Height *(int)rows) + 20 + 35;
            }
        }

        private void SetFormStartPosition()
        {
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            string tableName = "tblPermutations" + EncMess.ID.ToString();
            DialogResult result = DialogResult.Yes;
            dialog.Show(this);
            bool dialogClosed = false;
            bool tableComplete = false;
            try
            {
                // create the table to save the data to
                if (cypher.data.classes.Decryption.CheckForPermutationTable(tableName))
                {
                    double perms = GetPermutations();
                    double tablePerms = data.classes.Decryption.CheckSizeOfPermutationTable(tableName);
                    string message = "A permutations table exists for this message.\r\n\r\n";
                    message += "There are " + perms.ToString() + " permutations for this message.\r\n";
                    message += "There are " + tablePerms.ToString() + " permutations saved in the table.\r\n";
                    message += "\r\n Do you want to drop and re-create the table?";
                    result = MessageBox.Show(message, "Are you sure...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        cypher.data.classes.Decryption.CreateTABLE(tableName, this.Boxes.Count, true);
                        tableComplete = true;
                    }
                }
                else
                {
                    cypher.data.classes.Decryption.CreateTABLE(tableName, this.Boxes.Count, false);
                    tableComplete = true;
                }
                if (tableComplete)
                {
                    bWorker.WorkerReportsProgress = true;
                    bWorker.WorkerSupportsCancellation = false;
                    bWorker.RunWorkerAsync(tableName);
                    Log.WriteToLog(info.ProjectInfo.ProjectLogType, "miSave_Click", "All Permutations saved!", LogEnum.Debug);
                    dialog.Close();
                    dialogClosed = true;
                    result = MessageBox.Show("All Permutations Saved, Continue with calculating sentence probablility?", "Finished current task...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        CalculatePermutations(tableName);
                }
            }
            catch (Exception x)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "miSave_Click", x, LogEnum.Critical);
                MessageBox.Show(x.Message.ToString(), "An Error Occurred...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if(!dialogClosed)
                    dialog.Close();
            }
        }

        private void CalculatePermutations(string tableName)
        {
            try
            {
                cypher.data.classes.Decryption.DetectSentence(tableName, GetPermutations());
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        private void miView_Click(object sender, EventArgs e)
        {
            string result = "";
            for (int x = 0; x < Boxes.Count; x++)
            {
                result += Boxes[x].SelectedItem + " ";
            }
            MessageBox.Show(result, "Choosen Message...", MessageBoxButtons.OK);                
        }

        private void miInfo_Click(object sender, EventArgs e)
        {
            string message = "";
            double possible = GetPermutations();
            message += "Number of possible permutations : " + possible;
            MessageBox.Show(message, "Information regarding this message...", MessageBoxButtons.OK);
        }

        private double GetPermutations()
        {
            int[] items = new int[this.Boxes.Count];
            // how many items per box
            foreach (ucWords b in Boxes)
            {
                cypher.data.classes.MessageWord currentWord = new data.classes.MessageWord(EncMess.ID, b.WordValue);
                items[Convert.ToInt16(b.Tag) - 1] = currentWord.words.Count;
            }

            // number of possible permutations
            double returnValue = 1;
            foreach (int i in items)
            {
                returnValue = returnValue * i;
            }
            Log.WriteToLog(info.ProjectInfo.ProjectLogType, "GetPermutations", "Number of permutations " + returnValue.ToString(), LogEnum.Debug);
            return returnValue;
        }

        private void miPare_Click(object sender, EventArgs e)
        {
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "miPare_Click", "Begin...", LogEnum.Debug);
            frmPare pareForm = new frmPare(this.EncMess);
            pareForm.ShowDialog(this);
            RefreshBoxes();
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "miPare_Click", "End...", LogEnum.Debug);
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void miCalc_Click(object sender, EventArgs e)
        {
            if (dialog == null)
                dialog = new frmDialog(false);
            bool dialogClosed = false;
            dialog.lblText.Text = "Calculating sentence probability of permutations...";
            dialog.Show();
            try
            {
                string messageID = EncMess.ID.ToString();
                Log.WriteToLog(cypher.info.ProjectInfo.ProjectLogType, "Calculate Permutations", "Calculating Permutations for message with ID : " + messageID, LogEnum.Debug);
                string tableName = "tblPermutations" + messageID;
                this.CalculatePermutations(tableName);
            }
            catch (Exception x)
            {
                string errMessage = x.Message;
                MessageBox.Show(errMessage, "Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!dialogClosed)
                    dialog.Close();
            }
        }

        private void LoadBoxes()
        {
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "LoadBoxes", "Begin...", LogEnum.Debug);
            try
            {
                // load the message words into each list box
                Boxes = new WordBoxArray(this);
                int boxCount = 0;
                foreach (object obj in mWords.wordValues)
                {
                    if (boxCount >= 1)
                    {
                        Boxes.AddNewControl();
                        SetFormSize();
                        SetFormStartPosition();
                    }
                    int wv = Convert.ToInt16(obj);
                    Boxes.UpdateControlValues(boxCount, EncMess.ID, wv);
                    boxCount++;
                }
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "LoadBoxes", "Number of Boxes Loaded : " + boxCount.ToString(), LogEnum.Debug);
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "LoadBoxes", "Number of Boxes : " + Boxes.Count.ToString(), LogEnum.Debug);
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "LoadBoxes", x, LogEnum.Critical);
            }
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "LoadBoxes", "End...", LogEnum.Debug);
        }

        private void RefreshBoxes()
        {
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "RefreshBoxes", "Begin...", LogEnum.Debug);
            try
            {
                for (int a = 0; a < Boxes.Count; a++)
                {
                    cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "RefreshBoxes", "Refreshing Box : " + a.ToString(), LogEnum.Debug);
                    Boxes[a].UpdateChildControls(EncMess.ID, Boxes[a].WordValue);
                }
            }
            catch (Exception x)
            {
                cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "RefreshBoxes", x, LogEnum.Critical);
            }
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "RefreshBoxes", "End...", LogEnum.Debug);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                for (int x = 0; x < Boxes.Count; x++)
                {
                    int y = (x / Boxes.Count) * 100;
                    bWorker.ReportProgress(y, y.ToString() + " percent complete. Saving words for Column " + x.ToString() + "...");
                    ArrayList words = new ArrayList();
                    cypher.data.classes.MessageWord currentWord = new data.classes.MessageWord(this.EncMess.ID, Boxes[x].WordValue);
                    foreach (object item in currentWord.words)
                    {
                        words.Add((string)item);
                    }
                    cypher.data.classes.Decryption.FillPermutationsTable(e.Argument.ToString(), words, x, GetPermutations());
                    Log.WriteToLog(info.ProjectInfo.ProjectLogType, "BackgroundWorker_DoWork", "All Permutations saved for column " + x.ToString(), LogEnum.Debug);
                }
            }
            catch (Exception ex)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "BackgroundWorker_DoWork", ex, LogEnum.Critical);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dialog.lblText.Text = e.UserState.ToString();
        }
    }
}
