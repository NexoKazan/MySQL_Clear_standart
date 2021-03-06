﻿using System;
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
        private string _typeID;
        private string _oldName;
        private int _size;
        private int _isPrimary; // 0-не ключ 1-единичный ключ, 2-составной ключ, 3-другое
        private bool _isForSelect = false;
        private bool _isRenamed = false;
        private S_Type _type;

        public ColumnStructure() {}

        public ColumnStructure(string name, string typeID, int isPrimary)
        {
            _isPrimary = isPrimary;
            _name = name;
            _typeID = typeID;
        }

        public ColumnStructure(string name, string typeID)
        {
            _isPrimary = 0;
            _name = name;
            _typeID = typeID;
        }
        
        public ColumnStructure(string name)
        {
            _name = name;
        }

        [XmlAttribute]
        public int IsPrimary {
            get { return _isPrimary; }
            set { _isPrimary = value; }
        }

        [XmlAttribute]
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        [XmlAttribute]
        public string TypeID
        {
            get { return _typeID; }
            set { _typeID = value; }
        }

        [XmlIgnore]
        public S_Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [XmlIgnore]
        public bool IsForSelect
        {
            get { return _isForSelect; }
            set { _isForSelect = value; }
        }

        [XmlIgnore]
        public bool IsRenamed
        {
            get { return _isRenamed; }
            set { _isRenamed = value; }
        }

        [XmlIgnore]
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        [XmlIgnore]
        public string OldName
        {
            get { return _oldName; }
            set { _oldName = value; }
        }
    }
}
