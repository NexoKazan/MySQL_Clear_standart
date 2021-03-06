﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MySQL_Clear_standart.DataBaseSchemeStructure
{
    
    public class DataBaseStructure
    {
        private TableStructure[] _tables;
        private S_Type[] _types;
        private string _name;

        public DataBaseStructure()
        { }

        public DataBaseStructure(string name, TableStructure[] tables)
        {
            _tables = tables;
            _name = name;
        }

        public DataBaseStructure(string name, TableStructure[] tables, S_Type[] types)
        {
            _tables = tables;
            _name = name;
            _types = types;
        }

        [XmlArray]
        public TableStructure[] Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }

        [XmlArray]
        public S_Type[] Types
        {
            get { return _types; }
            set { _types = value; }
        }
        [XmlAttribute]
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public string GetText()
        {
            string output = _name + "\r\n";
            for (int i = 0; i < _tables.Length; i++)
            {
                output += _tables[i].Name + "\r\n";
                for (int j = 0; j < _tables[i].Columns.Length; j++)
                {
                    output +="\t" + _tables[i].Columns[j].Name + "\r\n";
                }
            }

            return output;
        }
    }
}
