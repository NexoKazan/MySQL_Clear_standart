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

        public TableStructure(ColumnStructure[] columns)
        {
            _columns = columns;
        }

        public ColumnStructure[] Columns
        {
            get { return _columns; }
        }
    }
}
