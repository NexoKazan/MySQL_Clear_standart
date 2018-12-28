using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MySQL_Clear_standart.DataBaseSchemeStructure
{
    
    public class ColumnStructure
    {
        private string _name;
        private string _type;
        private bool _isPrimary;

        public ColumnStructure() {}

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

        [XmlAttribute]
        public bool IsPrimary {
            get { return _isPrimary; }
            set { _isPrimary = value; }
        }

        [XmlAttribute]
        public string Name {
            get { return _name; }
            set { _name = value; }
        }


        //public string Type
        //{
        //    get { return _type; }
        //    set { value = _type; }
        //}
    }
}
