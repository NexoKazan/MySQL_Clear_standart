using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart
{   // добавить структуры БД
    class WhereStructure
    {
        private string _string; //переименовать!
        private string _leftColumn;
        private string _rightExpr;
        private string _table;
        private string _comparisionOperator;


        public WhereStructure(string fullString, string leftColumn)
        {
            _string = fullString;
            _leftColumn = leftColumn;
        }

        public WhereStructure(string leftColumn, string comparisionOperator, string rightColumn)
        {
            _comparisionOperator = comparisionOperator;
            _leftColumn = leftColumn;
            _rightExpr = rightColumn;
            _string = _leftColumn + " " + _comparisionOperator + " " + _rightExpr;
        }

        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public string getWhereString
        {
            get { return _string; }
        }

        public string LeftColumn
        {
            get { return _leftColumn; }
        }

        public string RightExpr
        {
            get { return _rightExpr; }
            set { _rightExpr = value; }
        }

        public string ComparisionOperator
        {
            get { return _comparisionOperator; }
            set { _comparisionOperator = value; }
        }
    }
}
