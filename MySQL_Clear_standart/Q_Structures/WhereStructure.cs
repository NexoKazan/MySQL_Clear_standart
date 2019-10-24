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
        private string _column;
        private string _table;

        public WhereStructure(string fullString, string leftColumn)
        {
            _string = fullString;
            _column = leftColumn;
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

        public string getLeftColumn
        {
            get { return _column; }
        }
    }
}
