using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MySQL_Clear_standart
{
    public class S_Type
    {
        private string _name;
        private int _size;
        private string _ID;

        public S_Type(){}

        public S_Type(string name, int size, string ID)
        {
            _name = name;
            _size = size;
            _ID = ID;
        }

        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlAttribute]
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        [XmlAttribute]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
    }
}
