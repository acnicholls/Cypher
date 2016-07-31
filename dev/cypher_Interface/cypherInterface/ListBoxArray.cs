
namespace cypher.GUI
{
    public class ListBoxArray : System.Collections.CollectionBase
    {

        private readonly System.Windows.Forms.Form HostForm;

        public System.Windows.Forms.ListBox AddNewListBox()
        {
            System.Windows.Forms.ListBox aListBox = new System.Windows.Forms.ListBox();
            this.List.Add(aListBox);
            HostForm.Controls.Add(aListBox);
            
            // set basic properties for the control
            aListBox.Height = _height;
            aListBox.Width = _width; ;
            aListBox.Top = SetTopValue();
            aListBox.Left = SetLeftValue();
            aListBox.Tag = this.Count;
            aListBox.TabIndex = this.Count - 1;

            return aListBox;
        }

        private int _height = 95;
        private int _width = 120;
        private int _columnsPerRow = cypher.GUI.frmDecrypted.ColumnsPerRow;
        private int _currentTopValue = 30;
        private int _currentLeftValue = 13;
        private int _spacing = 5;
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
                _currentLeftValue = 13;
                returnValue = _currentLeftValue;
            }
            cypher.Log.WriteToLog(info.ProjectInfo.ProjectLogType, "SetLeftValue", "Left Value : " + returnValue.ToString(), LogEnum.Debug);
            return returnValue;
        }

        public ListBoxArray(System.Windows.Forms.Form host)
        {
            HostForm = host;
            this.AddNewListBox();
        }

        public System.Windows.Forms.ListBox this[int Index]
        {
            get
            {
                return (System.Windows.Forms.ListBox)this.List[Index];
            }
        }

        public void Remove(int Index)
        {
            // check to make sure there is a ListBox to Remove
            if (this.Count > 0)
            {
                HostForm.Controls.Remove(this[Index]);
                this.List.RemoveAt(Index);
            }
        }
    }
}
