using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Clear_standart
{
    class AsStructure
    {
        private string _table;
        private string _string; //переименовать

        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }
    }
}
