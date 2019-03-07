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
        private string _clearString; //переименовать
        private string _asString;
        private string _functionString;
        private bool _isSelectPart = false;
        private bool _isSingleColumn = false;
        private bool _isSortPart = false;
        private List<string> _asColumnList;
        private List<TableStructure> _asTables;

        public AsStructure(List<string> asColumns, string asString, string functionString, string asRightName)
        {
            _asColumnList = asColumns;
            _clearString = asString;
            _asRightName = asRightName;
            _functionString = functionString;
            _asString = asString;
        }

        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public string AsString
        {
            get { return _asString; }
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

        public bool IsSingleColumn
        {
            get { return _isSingleColumn; }
            set { _isSingleColumn = value; }
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
