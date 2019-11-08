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
        private string _createTableColumnNames;
        private TableStructure _inputTable;
        private List<WhereStructure> _whereList;
        private List<AsStructure> _asList;
        private TableStructure _outTable;
        private ColumnStructure[] _outColumn;
        
        public SelectStructure(string name, TableStructure table, List<WhereStructure> whereList, List<AsStructure> asList)
        {
            _name = name;
            _tableName = table.Name;
            _inputTable = table;
            _whereList = whereList;
            _asList = asList;
        }

        public string Output
        {
            get { return _output; }
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

        public string CreateTableColumnNames
        {
            get { return _createTableColumnNames; }
            set { _createTableColumnNames = value; }
        }

        public ColumnStructure[] OutColumn
        {
            get { return _outColumn; }
        }

        public TableStructure OutTable
        {
            get { return _outTable; }
        }

        public TableStructure InputTable
        {
            get { return _inputTable; }
        }
        

        public void CreateQuerry()
        {
            //выдернуто из свойства OutTable
            List<ColumnStructure> tempList = new List<ColumnStructure>();
            foreach (ColumnStructure column in _inputTable.Columns)
            {
                if(column.IsForSelect)
                {
                    tempList.Add(column);
                }
                //else if()
                //{
                    
                //}
            }

            foreach (AsStructure asStructure in _asList)
            {//Сосздать конструктор для новых столбцов
                if (!asStructure.AsRightColumn.IsRenamed && asStructure.AsRightColumn.OldName!=null)
                {
                    string tmpNameHolder = asStructure.AsRightColumn.OldName;
                    asStructure.AsRightColumn.OldName = asStructure.AsRightColumn.Name;
                    asStructure.AsRightColumn.Name = tmpNameHolder;
                    asStructure.AsRightColumn.IsRenamed = true;
                    tempList.Add(asStructure.AsRightColumn);
                }
                else
                {
                    tempList.Add(asStructure.AsRightColumn);
                }
            }
            
            _outColumn = new ColumnStructure[tempList.Count];
            for (int i = 0; i < _outColumn.Length; i++)
            {
                _outColumn[i] = tempList[i];
            }
            _outTable = new TableStructure(_name + "_TB", _outColumn.ToArray());

            _output = "SELECT ";
            for (int i = 0; i < _inputTable.Columns.Length; i++)
            {
                _output += "\r\n\t" + _inputTable.Columns[i].Name + " ";
                if (i!= _inputTable.Columns.Length-1)
                {
                    _output += ",";
                }
            }

            foreach (var asStructure in _asList)
            {
                if (_output != "SELECT ")
                {
                    _output += ",";
                    _output += "\r\n\t" + asStructure.AsString + " AS " + asStructure.AsRightColumn.Name;
                }
                else
                {
                    _output += "\r\n\t" + asStructure.AsString + " AS " + asStructure.AsRightColumn.Name;
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

            for (int i = 0; i < _outColumn.Length; i++)
            {
                _createTableColumnNames += _outColumn[i].Name + " " + _outColumn[i].Type.Name;
                if (i < _outColumn.Length - 1)
                {
                    _createTableColumnNames += ",\r\n";
                }
            }
        }
    }
}
