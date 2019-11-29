using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart.Q_Part_Structures
{
    class LikeStructure
    {
        private string _columnName;
        private string _rightExpression;
        private bool _isNot = false;
        private TableStructure _table;
        private ColumnStructure _leftColumn;

        public LikeStructure(string rightExpression, string columnName)
        {
            _rightExpression = rightExpression;
            _columnName = columnName;
        }
        
        public string RightExpression
        {
            get { return _rightExpression; }
        }

        public string ColumnName
        {
            get { return _columnName; }
        }

        public bool IsNot
        {
            get { return _isNot; }
            set { _isNot = value; }
        }

        public TableStructure Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public ColumnStructure LeftColumn
        {
            get { return _leftColumn; }
            set { _leftColumn = value; }
        }
    }
}
