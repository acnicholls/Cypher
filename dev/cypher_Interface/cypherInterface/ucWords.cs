using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace cypher.GUI
{
    public partial class ucWords : UserControl
    {
        private int _messID = 0;
        private int _wordValue = 0;
        private float fontSize = 8.25F;

        public ucWords()
        {
            InitializeComponent();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
            txtFilter.Text = "";
            txtFilter.ForeColor = System.Drawing.Color.Black;
        }

        private void txtFilter_Clear()
        {
            this.txtFilter.Text = "<Filter...>";
            this.txtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", fontSize, FontStyle.Italic);
            this.txtFilter.ForeColor = System.Drawing.Color.DimGray;
        }

        public string SelectedItem
        {
            get 
            {
                return this.lbWords.SelectedItem.ToString();
            }
        }

        public int WordValue
        {
            get
            {
                return _wordValue;
            }
        }
        public void UpdateChildControls(int messID, int wordValue)
        {
            try
            {
                _messID = messID;
                _wordValue = wordValue;
                this.lbWords.Items.Clear();
                cypher.data.classes.MessageWord mw = new data.classes.MessageWord(_messID, _wordValue);
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "UpdateChildControls", "Number of words : " + mw.words.Count.ToString(), LogEnum.Debug);
                foreach (string w in mw.words)
                {
                    this.lbWords.Items.Add(w);
                }
                this.lbWords.Sorted = true;

                this.lblWordValue.Text = this._wordValue.ToString() + " : " + mw.words.Count.ToString() + " word(s)";
                this.Invalidate(true);
            }
            catch (Exception ex)
            {
                Log.WriteToLog(info.ProjectInfo.ProjectLogType, "UpdateChildControls", ex, LogEnum.Critical);
            }

        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            // filter the list by words containing the entered text
            string searchString = txtFilter.Text.Trim();
            this.lbWords.Items.Clear();
            cypher.data.classes.MessageWord mw = new data.classes.MessageWord(_messID, _wordValue);
            foreach (object obj in mw.words)
            {
                string word = obj.ToString();
                if (word.Contains(searchString))
                    this.lbWords.Items.Add(obj);
            }
            this.lblWordValue.Text = _wordValue.ToString() + " : " + lbWords.Items.Count.ToString() + " word(s)";
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtFilter_Clear();
            UpdateChildControls(_messID, _wordValue);
        }
    }
}
