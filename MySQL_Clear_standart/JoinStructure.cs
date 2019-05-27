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
        private string _output;
        private string _comparisonOperator;
        private bool _isFirst = false;
        private bool _switched = false;
        private ColumnStructure _leftColumn;
        private ColumnStructure _rightColumn;
        private SelectStructure _leftSelect;
        private SelectStructure _rightSelect;
        private JoinStructure _leftJoin;
        private List<ColumnStructure> _columns = new List<ColumnStructure>();

        public JoinStructure(string leftColumn, string rightColumn, string comparisonOperator)
        {
            _leftColumnString = leftColumn;
            _rightColumnString = rightColumn;
            _comparisonOperator = comparisonOperator;
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

        public List<ColumnStructure> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public string Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public JoinStructure LeftJoin {
            get { return _leftJoin; } set => _leftJoin = value; }

        public bool IsFirst
        {
            get { return _isFirst; }
            set { _isFirst = value; }
        }
        public bool Switched
        {
            get { return _switched; }
            set { _switched = value; }
        }
        public void CreateQuerry()
        {
            if (_leftJoin != null)
            {
                _columns.AddRange(_leftJoin.Columns);
                if (Switched)
                {
                    _columns.AddRange(_leftSelect.OutColumn);
                }
                else
                {
                    _columns.AddRange(_rightSelect.OutColumn);
                }
            }
            else
            {
                _columns.AddRange(_leftSelect.OutColumn);
                _columns.AddRange(_rightSelect.OutColumn);
            }


            _output = "SELECT\r\n\t";
            for (int i = 0; i < _columns.Count; i++)
            {
                if (i < _columns.Count - 1)
                {
                    _output += _columns[i].Name + ",\r\n\t";
                }
                else
                {
                    _output += Columns[i].Name + "\r\n";
                }
            }
            _output += "FROM\r\n\t" + LeftSelect.Name + ",\r\n\t" + RightSelect.Name + "\r\n" +
                       "WHERE\r\n\t" + _leftColumn.Name + " " + _comparisonOperator + " " + _rightColumn.Name;
        }
    }
}
