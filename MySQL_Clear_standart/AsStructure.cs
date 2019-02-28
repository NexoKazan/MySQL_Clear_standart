using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart
{
    class AsStructure
    {
        private string _asRightName;
        private string _table;
        private string _string; //переименовать
        private bool _isSelectPart = false;
        private List<string> _asColumnList;
        private List<TableStructure> _asTables;

        public AsStructure(List<string> asColumns, string asString, string asRightName)
        {
            _asColumnList = asColumns;
            _string = asString;
            _asRightName = asRightName;
        }

        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public string GetAsString
        {
            get { return _string; }
        }

        public string GetAsRightName
        {
            get { return _asRightName; }
        }

        public bool IsSelectPart
        {
            get { return _isSelectPart; }
            set { _isSelectPart = value; }
        }

        public void FindeTable(DataBaseStructure db)
        {//кривой цикл продумать лучше. Попробовать сделать через Column.Table
            _asTables = new List<TableStructure>();
            foreach (var col in _asColumnList)
            {
                foreach (var tb in db.Tables)
                {
                    foreach (var cl in tb.Columns)
                    {
                        if (col == cl.Name)
                        {
                            _asTables.Add(tb);
                        }                        
                    }
                }
            }

            _asTables =_asTables.Distinct().ToList();
            if (_asTables.Count == 1)
            {
                _isSelectPart = true;
                _table = _asTables[0].Name;
                _asRightName = _asTables[0].ShortName + _asRightName;
            }
            else
            {
                _isSelectPart = false;
                _table = "Error! _asTables.Count";
            }
        }
    }
}
