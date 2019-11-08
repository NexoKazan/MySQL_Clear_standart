using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart
{
    class AsStructure
    {
        private string _asRightName;
        private string _tableName;
        private string _clearString; //переименовать
        private string _asString;
        private string _functionString;
        private string _aggregateFunctionName;
        private bool _isSelectPart = false;
        private bool _isSingleColumn = false;
        private bool _isSortPart = false;
        private ColumnStructure _asRightColumn;
        private List<string> _asColumnList;
        private List<TableStructure> _asTables;

        public AsStructure(List<string> asColumns, string asString, string functionString, string asRightName, string aggregateFunctionName)
        {
            _asColumnList = asColumns;
            _clearString = asString;
            _asRightName = asRightName;
            _functionString = functionString;
            _asString = asString;
            _aggregateFunctionName = aggregateFunctionName;
        }

        public string OldTableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public string AsString
        {
            get { return _asString; }
        }

        public string GetAsRightName
        {
            get { return _asRightName; }
        }

        public string AggregateFunctionName
        {
            get { return _aggregateFunctionName; }
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

        public ColumnStructure AsRightColumn
        {
            get { return _asRightColumn; }
            set { _asRightColumn = value; }
        }

        public List<string> ColumnNames
        {
            get { return _asColumnList; }
        }

        public List<TableStructure> Tables
        {
            get { return _asTables; }
            set { _asTables = value; }
        }
       
    }
}
