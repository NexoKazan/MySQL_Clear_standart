using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart
{
    class JoinStructure
    {
        private string _leftColumnString;
        private string _rightColumnString;
        private string _name;
        private ColumnStructure _leftColumn;
        private ColumnStructure _rightColumn;
        private SelectStructure _leftSelect;
        private SelectStructure _rightSelect;
        private string _output;

        public JoinStructure(string leftColumn, string rightColumn)
        {
            _leftColumnString = leftColumn;
            _rightColumnString = rightColumn;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string LeftColumnString
        {
            get { return _leftColumnString; }
        }
        public string RightColumnString
        {
            get { return _rightColumnString; }
        }
        public ColumnStructure LeftColumn
        {
            get { return _leftColumn; }
            set { _leftColumn = value; }
        }

        public ColumnStructure RightColumn
        {
            get { return _rightColumn; }
            set { _rightColumn = value; }
        }

        public SelectStructure LeftSelect
        {
            get { return _leftSelect; }
            set { _leftSelect = value; }
        }

        public SelectStructure RightSelect
        {
            get { return _rightSelect; }
            set { _rightSelect = value; }
        }

        public string Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public void CreateQuerry()
        {
            _output = "Left: " + _leftColumn.Name + " Right: " + _rightColumn.Name + "\r\nLeft_Select: " + LeftSelect.Name + " Right_Select: "+RightSelect.Name;
        }
    }
}
