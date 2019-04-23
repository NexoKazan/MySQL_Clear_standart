using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart
{
    class SelectStructure
    {
        private string _name;
        private string _output = "error";
        private string _tableName;
        private static int _id = 0;
        private List<string> _columnsList;
        private List<WhereStructure> _whereList;
        private List<AsStructure> _asList;
        private TableStructure _outTable;
        private ColumnStructure[] _outColumn;
        
        public SelectStructure(string name, string tableName, List<string> columnsList, List<WhereStructure> whereList, List<AsStructure> asList)
        {
            _name = name;
            _tableName = tableName;
            _columnsList = columnsList;
            _whereList = whereList;
            _asList = asList;
        }

        public string Output
        {
            get
            {
                CreateQuerry();
                return _output;
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public string TableName
        {
            get { return _tableName; }
        }

        public ColumnStructure[] OutColumn
        {
            get { return _outColumn; }
        }

        public TableStructure OutTable
        {
            get
            {
                List<string> tempList = new List<string>();
                foreach (string column in _columnsList)
                {
                    tempList.Add(column);
                }
                foreach (WhereStructure whereStructure in _whereList)
                {
                    tempList.Add(whereStructure.getLeftColumn);
                }

                foreach (AsStructure asStructure in _asList)
                {
                    tempList.Add(asStructure.GetAsRightName);
                }

                tempList = tempList.Distinct().ToList();
                _outColumn = new ColumnStructure[tempList.Count];
                for (int i = 0; i < _outColumn.Length; i++)
                {
                    _outColumn[i] = new ColumnStructure(tempList[i]);
                }
                _outTable = new TableStructure(_name + "_TB", _outColumn);
                return _outTable;
            }
        }

        private void CreateQuerry()
        {
            _output = "SELECT ";
            for (int i = 0; i < _columnsList.Count; i++)
            {
                _output += "\r\n\t" + _columnsList[i] + " ";
                if (i!= _columnsList.Count-1)
                {
                    _output += ",";
                }
            }

            foreach (var asStructure in _asList)
            {
                _output += "\r\n\t" + asStructure.AsString + " AS " + asStructure.GetAsRightName;
            }

            _output += "\r\n" + "FROM " + "\r\n\t" + _tableName + "\r\n" + "WHERE ";
            foreach (WhereStructure whereStructure in _whereList)
            {
                if (whereStructure.Table == _tableName)
                {
                    _output += "\r\n\t" + whereStructure.getWhereString;
                }

            }

        }
    }
}
