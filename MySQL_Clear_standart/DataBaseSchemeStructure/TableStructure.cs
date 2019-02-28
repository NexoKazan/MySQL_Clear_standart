using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MySQL_Clear_standart.DataBaseSchemeStructure
{
    
    public class TableStructure
    {
        private ColumnStructure[] _columns;
        private string _name;
        private string _shortName;

        public TableStructure() { }

        public TableStructure(string name, ColumnStructure[] columns)
        {
            _name = name;
            _columns = columns;
        }

        [XmlArray]
        public ColumnStructure[] Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set { _name = value; }

        }

        [XmlAttribute]
        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }
    }
}
