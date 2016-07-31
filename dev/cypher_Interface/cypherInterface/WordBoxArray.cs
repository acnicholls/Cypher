using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace cypher.GUI
{
    public class WordBoxArray : System.Collections.CollectionBase
    {

        private readonly System.Windows.Forms.Form HostForm;

        private int _height = 187;
        public int Height
        {
            get { return _height; }
        }
        private int _width = 156;
        public int Width
        {
            get { return _width; }
        }
        private int _columnsPerRow = cypher.GUI.frmDecrypted.ColumnsPerRow;
        private int _currentTopValue = 20;
        private int _currentLeftValue = 0;
        private int _spacing = 0;
        public int Spacing
        {
            get { return _spacing; }
        }

        public WordBoxArray(System.Windows.Forms.Form host)
        {
            HostForm = host;
            this.AddNewControl();
        }

        public cypher.GUI.ucWords AddNewControl()
        {
            cypher.GUI.ucWords aWord = new ucWords();
            this.List.Add(aWord);
            HostForm.Controls.Add(aWord);

            // set basic properties for the control
            aWord.Height = _height;
            aWord.Width = _width;
            aWord.Top = SetTopValue();
            aWord.Left = SetLeftValue();
            aWord.Tag = this.Count;
            aWord.TabIndex = this.Count - 1;

            return aWord;
        }

        private int SetTopValue()
        {
            int test = this.List.Count;
            int returnValue = 0;
            // calcutlate the remainder of the number of items divided by the number of columns per row
            decimal remainder = decimal.Remainder((decimal)this.Count, (decimal)_columnsPerRow);
            if (remainder == 1 & this.Count > _columnsPerRow)
            {
                // same top value as last control
                _currentTopValue += _height + _spacing;
                returnValue = _currentTopValue;
            }
            else
            {
                // add the new row height + row spacing to the control top
                returnValue = _currentTopValue;
            }
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "SetTopValue", "Top Value : " + returnValue.ToString(), LogEnum.Debug);
            return returnValue;
        }

        private int SetLeftValue()
        {
            int returnValue = 0;
            // calcutlate the remainder of the number of items divided by the number of columns per row
            decimal remainder = decimal.Remainder((decimal)this.Count, (decimal)_columnsPerRow);
            if (remainder != 1)
            {
                // start a new row
                _currentLeftValue += _width + _spacing;
                returnValue = _currentLeftValue;
            }
            else
            {
                // start a new row
                _currentLeftValue = 0;
                returnValue = _currentLeftValue;
            }
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "SetLeftValue", "Left Value : " + returnValue.ToString(), LogEnum.Debug);
            return returnValue;
        }

        public cypher.GUI.ucWords this[int index]
        {
            get
            {
                return (cypher.GUI.ucWords)this.List[index];
            }
        }

        public void UpdateControlValues(int index, int messID, int wordVal)
        {
            cypher.GUI.ucWords aWord = (cypher.GUI.ucWords)this.List[index];
            aWord.UpdateChildControls(messID, wordVal);
        }
    }
}
