using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart.Q_Structures
{
    class OrderByStructure
    {
        private ColumnStructure _column;
        private string _columnName;
        private bool _isDESC = false;

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public bool IsDESC  
        {
            get { return _isDESC; }
            set { _isDESC = value; }
        }

        public ColumnStructure Column
        {
            get { return _column; }
            set { _column = value; }
        }
    }
}
