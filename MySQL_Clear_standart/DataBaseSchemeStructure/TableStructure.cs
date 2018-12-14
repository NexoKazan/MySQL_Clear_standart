using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Clear_standart.DataBaseSchemeStructure
{
    class TableStructure
    {
        private ColumnStructure[] _columns;
        private string _name;

        public TableStructure(string name, ColumnStructure[] columns)
        {
            _name = name;
            _columns = columns;
        }

        public ColumnStructure[] Columns
        {
            get { return _columns; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
