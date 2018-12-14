using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Clear_standart.DataBaseSchemeStructure
{
    class ColumnStructure
    {
        private string _name;
        private string _type;
        private bool _isPrimary;

        public ColumnStructure(string name, string type, bool isPrimary)
        {
            _isPrimary = isPrimary;
            _name = name;
            _type = type;
        }

        public ColumnStructure(string name, string type)
        {
            _isPrimary = false;
            _name = name;
            _type = type;
        }

        public ColumnStructure(string name)
        {
            _name = name;
        }

        public bool IsPrimary
        {
            get { return _isPrimary; }
            set { _isPrimary = value; }
        }
        public string Name
        {
            get { return _name; }
        }

        public string Type
        {
            get { return _type; }
        }
    }
}
