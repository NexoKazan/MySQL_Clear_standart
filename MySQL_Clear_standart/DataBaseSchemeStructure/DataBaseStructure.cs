using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Clear_standart.DataBaseSchemeStructure
{
    class DataBaseStructure
    {
        private TableStructure[] _tables;

        public DataBaseStructure(TableStructure[] tables)
        {
            _tables = tables;
        }

        public TableStructure[] Tabless
        {
            get { return _tables; }
        }
    }
}
