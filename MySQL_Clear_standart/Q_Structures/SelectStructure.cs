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
        private string _indexColumnName = null;
        private List<ColumnStructure> _columnsList;
        private List<WhereStructure> _whereList;
        private List<AsStructure> _asList;
        private TableStructure _outTable;
        private ColumnStructure[] _outColumn;
        
        public SelectStructure(string name, string tableName, List<ColumnStructure> columnsList, List<WhereStructure> whereList, List<AsStructure> asList)
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

        public string IndexColumnName
        {
            get { return _indexColumnName; }
            set { _indexColumnName = value; }
        }

        public ColumnStructure[] OutColumn
        {
            get { return _outColumn; }
        }

        public TableStructure OutTable
        {
            get
            {
                return _outTable;
            }
        }

        public void CreateQuerry()
        {
            //выдернуто из свойства OutTable
            List<ColumnStructure> tempList = new List<ColumnStructure>();
            foreach (ColumnStructure column in _columnsList)
            {
                tempList.Add(column);
            }

            foreach (AsStructure asStructure in _asList)
            {//Сосздать конструктор для новых столбцов
                if (!asStructure.GetAsRightColumn.IsRenamed && asStructure.GetAsRightColumn.OldName!=null)
                {
                    string tmpNameHolder = asStructure.GetAsRightColumn.OldName;
                    asStructure.GetAsRightColumn.OldName = asStructure.GetAsRightColumn.Name;
                    asStructure.GetAsRightColumn.Name = tmpNameHolder;
                    asStructure.GetAsRightColumn.IsRenamed = true;
                    tempList.Add(asStructure.GetAsRightColumn);
                }
                else
                {
                    tempList.Add(asStructure.GetAsRightColumn);
                }
            }

            //tempList = tempList.Distinct().ToList();
            _outColumn = new ColumnStructure[tempList.Count];
            for (int i = 0; i < _outColumn.Length; i++)
            {
                _outColumn[i] = tempList[i];
            }
            _outTable = new TableStructure(_name + "_TB", _outColumn.ToArray());

            _output = "SELECT ";
            for (int i = 0; i < _columnsList.Count; i++)
            {
                _output += "\r\n\t" + _columnsList[i].Name + " ";
                if (i!= _columnsList.Count-1)
                {
                    _output += ",";
                }
            }

            foreach (var asStructure in _asList)
            {
                if (_output != "SELECT ")
                {
                    _output += ",";
                    _output += "\r\n\t" + asStructure.AsString + " AS " + asStructure.GetAsRightColumn.Name;
                }
                else
                {
                    _output += "\r\n\t" + asStructure.AsString + " AS " + asStructure.GetAsRightColumn.Name;
                }
            }

            _output += "\r\n" + "FROM " + "\r\n\t" + _tableName + "\r\n" ;
            if (_whereList.Count != 0)
            {
                _output += "WHERE ";

                foreach (WhereStructure whereStructure in _whereList)
                {
                    if (whereStructure.Table == _tableName)
                    {
                        _output += "\r\n\t" + whereStructure.getWhereString;
                    }

                    if (whereStructure != _whereList.LastOrDefault())
                    {
                        _output += " AND ";
                    }

                }
            }

        }
    }
}
