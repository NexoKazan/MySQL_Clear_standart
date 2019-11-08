using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;
using MySQL_Clear_standart.Q_Structures;

namespace MySQL_Clear_standart
{
    class SortStructure
    {
        private string _name;
        private string _fromName;
        private string _output;
        private string _createTableColumnNames;
        private SelectStructure _select;
        private JoinStructure _join;
        private TableStructure _outTable;
        private List<string> _groupByColumnList;
        private List<AsStructure> _asSortList;
        private List<OrderByStructure> _orderByStructures;

        public SortStructure(string name)
        {
            _name = name;
        }

        public List<AsStructure> AsSortList
        {
            set { _asSortList = value; }
        }

        public List<string> GroupByColumnList
        {
            set { _groupByColumnList = value; }
        }
        
        public List<OrderByStructure> OrderByStructures
        {
            set { _orderByStructures = value; }
        }

        public JoinStructure Join
        {
            set { _join = value; }
        }

        public SelectStructure Select
        {
            set { _select = value; }
        }

        public TableStructure OutTable
        {
            get { return _outTable; }
        }

        public string Output
        {
            get { return _output; }
        }

        public string Name
        {
            get { return _name; }
        }


        public string CreateTableColumnNames
        {
            get { return _createTableColumnNames; }
        }

        public void CreateQuerry()
        {
            _output = "SELECT \r\n\t";
            List<ColumnStructure> tmpSelectColumns = new List<ColumnStructure>();
            if (_join != null)
            {
                _fromName = _join.Name;
                foreach (ColumnStructure column in _join.Columns)
                {
                    if (column.IsForSelect)
                    {
                        tmpSelectColumns.Add(column);
                    }
                }
            }
            else
            {
                _fromName = _select.Name;
                foreach (ColumnStructure column in _select.OutColumn)
                {
                    if (column.IsForSelect)
                    {
                        tmpSelectColumns.Add(column);
                    }
                }
            }

            foreach (ColumnStructure column in tmpSelectColumns)
            {
                if (column != tmpSelectColumns.Last())
                {
                    _output += column.Name + ",\r\n\t";
                }
                else
                {
                    _output += column.Name + "\r\n\t";
                }
            }

            if (_asSortList.Count != 0 && tmpSelectColumns.Count!=0)
            {
                _output = _output.Insert(_output.Length - 3, ",");
            }

            foreach (AsStructure asStructure in _asSortList)
            {
                if (asStructure.AsRightColumn.IsRenamed)
                {
                    string tmpHolder = asStructure.AsRightColumn.Name;
                    asStructure.AsRightColumn.Name = asStructure.AsRightColumn.OldName;
                    asStructure.AsRightColumn.IsRenamed = false;
                    asStructure.AsRightColumn.OldName = tmpHolder;
                }
                if(asStructure.AsRightColumn.OldName!=null)
                {
                    _output += asStructure.AggregateFunctionName + "(" + asStructure.AsRightColumn.OldName + ")" + " AS " +
                           asStructure.AsRightColumn.Name;
                }
                else
                {
                    _output += asStructure.AggregateFunctionName + "(" + asStructure.AsString + ")" + " AS " +
                               asStructure.AsRightColumn.Name;
                }
                tmpSelectColumns.Add(asStructure.AsRightColumn);
                if (asStructure != _asSortList.Last())
                {
                    _output += ",\r\n\t";
                }
                else
                {
                    _output += "\r\n\t";
                }
            }

            _outTable = new TableStructure(_name+"_TB", tmpSelectColumns.ToArray());
            
            _output = _output.Remove(_output.Length - 1, 1);
            _output += "FROM\r\n\t" + _fromName + "\r\n";
            if (_groupByColumnList.Count != 0)
            {
                _output += "GROUP BY\r\n\t";
            }

            foreach (string column in _groupByColumnList)
            {
                _output += column;
                if (column != _groupByColumnList.Last())
                {
                    _output += ",\r\n\t";
                }
                else
                {
                    _output += "\r\n";
                }
            }

            if(_orderByStructures.Count!=0)
            {
                _output += "ORDER BY\r\n\t";
            }

            foreach (OrderByStructure orderBy in _orderByStructures)
            {
                if (!orderBy.Column.IsRenamed)
                {
                    _output += orderBy.Column.Name;
                }
                else
                {
                    _output += orderBy.Column.OldName;
                }

                if (orderBy.IsDESC)
                {
                    _output += " DESC";
                }

                if (orderBy != _orderByStructures.Last())
                {
                    _output += ",\r\n\t";
                }
                else
                {
                    _output += "\r\n";
                }

                
            }
            SetCreateTableColumnList();
        }

        private void SetCreateTableColumnList()
        {
            for (int i = 0; i < _outTable.Columns.Length; i++)
            {
                if (_outTable.Columns[i].Type != null)
                {
                    _createTableColumnNames += _outTable.Columns[i].Name + " " + _outTable.Columns[i].Type.Name;
                }
                else
                {
                    _createTableColumnNames += _outTable.Columns[i].Name + " " + "INTEGER";
                }

                if (i < _outTable.Columns.Length - 1)
                {
                    _createTableColumnNames += ",\r\n";
                }
            }
        }
        
    }
}
