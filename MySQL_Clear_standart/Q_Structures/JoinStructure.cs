using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart
{
    class JoinStructure
    {
        private string _leftColumnString;
        private string _rightColumnString;
        private string _name;
        private string _output;
        private string _comparisonOperator;
        private string _indexColumnName;
        private string _createTableColumnNames;
        private bool _isFirst = false;
        private bool _switched = false;
        private bool _isJoined = false;
        private ColumnStructure _leftColumn;
        private ColumnStructure _rightColumn;
        private TableStructure _outTable;
        private SelectStructure _leftSelect;
        private SelectStructure _rightSelect;
        private JoinStructure _leftJoin;
        private List<ColumnStructure> _columns = new List<ColumnStructure>();

        public JoinStructure(string leftColumn, string rightColumn, string comparisonOperator)
        {
            _leftColumnString = leftColumn;
            _rightColumnString = rightColumn;
            _comparisonOperator = comparisonOperator;
        }

        #region Свойства

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string LeftColumnString
        {
            get { return _leftColumnString; }
        }
        
        public string RightColumnString
        {
            get { return _rightColumnString; }
        }

        public string IndexColumnName
        {
            get { return _indexColumnName; }
            set { _indexColumnName = value; }
        }

        public string CreateTableColumnNames
        {
            get { return _createTableColumnNames; }
        }

        public ColumnStructure LeftColumn
        {
            get { return _leftColumn; }
            set { _leftColumn = value; }
        }

        public ColumnStructure RightColumn
        {
            get { return _rightColumn; }
            set { _rightColumn = value; }
        }
       
        public TableStructure OutTable
        {
            get { return _outTable; }
        }
        
        public SelectStructure LeftSelect
        {
            get { return _leftSelect; }
            set { _leftSelect = value; }
        }

        public SelectStructure RightSelect
        {
            get { return _rightSelect; }
            set { _rightSelect = value; }
        }

        public List<ColumnStructure> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public string Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public JoinStructure LeftJoin
        {
            get { return _leftJoin; }
            set { _leftJoin = value; }
        }

        public bool IsFirst
        {
            get { return _isFirst; }
            set { _isFirst = value; }
        }
        
        public bool Switched
        {
            get { return _switched; }
            set { _switched = value; }
        }

        public bool IsJoined
        {
            get { return _isJoined; }
            set { _isJoined = value; }
        }

        #endregion

        public void CreateQuerry()
        {
            ColumnCounterDelete();
            if (_leftJoin != null)
            {
                _columns.AddRange(_leftJoin.Columns);
                if (_switched)
                {
                    if (_leftSelect != null)
                        _columns.AddRange(_leftSelect.OutColumn);
                }
                else
                {
                    if (_rightSelect != null)
                        _columns.AddRange(_rightSelect.OutColumn);
                }
            }
            else
            {
                if (_leftSelect != null)
                {
                    _columns.AddRange(_leftSelect.OutColumn);
                }

                if (_rightSelect != null)
                {
                    _columns.AddRange(_rightSelect.OutColumn);

                }
            }

            List<ColumnStructure> tmpColumns = new List<ColumnStructure>();
            foreach (ColumnStructure column in _columns)
            {
                if (column.IsForSelect)
                {
                    tmpColumns.Add(column);
                }
                else
                {
                    if (_leftColumn != null && _rightColumn != null)
                    {
                        if (column.Name != _leftColumn.Name && column.Name != _rightColumn.Name)
                        {
                            tmpColumns.Add(column);
                        }
                    }
                }
            }

            _columns = tmpColumns;
            _outTable = new TableStructure(_name + "_TB", _columns.ToArray());
            _output = "SELECT";
            bool commaPointer = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                if (_columns[i].UsageCounter > 0 || _columns[i].IsForSelect)
                {

                    if (!commaPointer)
                    {
                        _output += "\r\n\t" + _columns[i].Name;
                        commaPointer = true;
                    }
                    else
                    {
                        _output += ",\r\n\t" + _columns[i].Name;
                    }
                }
            }

            _output += "\r\nFROM\r\n\t";

            if (_leftJoin != null)
            {
                if (_leftSelect != null || _rightSelect != null)
                {
                    _output += _leftJoin.Name + ",\r\n\t";
                }
                else
                {
                    _output += _leftJoin.Name + "\r\n";
                }

                if (_switched)
                {
                    if (_leftSelect != null)
                    {
                        _output += _leftSelect.Name + "\r\n";
                    }
                }
                else
                {
                    if (_rightSelect != null)
                    {
                        _output += _rightSelect.Name + "\r\n";
                    }
                }
            }
            else
            {
                if (_leftSelect != null)
                {
                    _output += _leftSelect.Name + ",\r\n\t";
                }
                else
                {
                    _output += "\r\n";
                }

                if (_rightSelect != null)
                {
                    _output += _rightSelect.Name + "\r\n";
                }
                else
                {
                    _output += "\r\n";
                }
            }

            if (_leftColumn != null && _rightColumn != null)
            {
                _output += "WHERE\r\n\t" + _leftColumn.Name + " " + _comparisonOperator + " " + _rightColumn.Name;
            }

            SetIndex();
            SetCreateTableColumnList();
        }

        public void CreateQuerry(string left, string right)
        {
            ColumnCounterDelete();
            if (_leftJoin != null)
            {
                _columns.AddRange(_leftJoin.Columns);
                if (_switched)
                {
                    if (_leftSelect != null)
                        _columns.AddRange(_leftSelect.OutColumn);
                }
                else
                {
                    if (_rightSelect != null)
                        _columns.AddRange(_rightSelect.OutColumn);
                }
            }
            else
            {
                if (_leftSelect != null)
                {
                    _columns.AddRange(_leftSelect.OutColumn);
                }

                if (_rightSelect != null)
                {
                    _columns.AddRange(_rightSelect.OutColumn);

                }
            }

            List<ColumnStructure> tmpColumns = new List<ColumnStructure>();
            foreach (ColumnStructure column in _columns)
            {
                if (column.IsForSelect)
                {
                    tmpColumns.Add(column);
                }
                else
                {
                    if (_leftColumn != null && _rightColumn != null)
                    {
                        if (column.Name != _leftColumn.Name && column.Name != _rightColumn.Name)
                        {
                            tmpColumns.Add(column);
                        }
                    }
                }
            }

            _columns = tmpColumns;
            _outTable = new TableStructure(_name + "_TB", _columns.ToArray());
            _output = "SELECT";
            bool commaPointer = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                if (_columns[i].UsageCounter > 0 || _columns[i].IsForSelect)
                {

                    if (!commaPointer)
                    {
                        _output += "\r\n\t" + _columns[i].Name;
                        commaPointer = true;
                    }
                    else
                    {
                        _output += ",\r\n\t" + _columns[i].Name;
                    }
                }
            }

            _output += "\r\nFROM\r\n\t";

            #region old4
            //if (_leftJoin != null)
            //{
            //    if (_leftSelect != null || _rightSelect != null)
            //    {
            //        _output += _leftJoin.Name + ",\r\n\t";
            //    }
            //    else
            //    {
            //        _output += _leftJoin.Name + "\r\n";
            //    }

            //    if (_switched)
            //    {
            //        if (_leftSelect != null)
            //        {
            //            _output += _leftSelect.Name + "\r\n";
            //        }
            //    }
            //    else
            //    {
            //        if (_rightSelect != null)
            //        {
            //            _output += _rightSelect.Name + "\r\n";
            //        }
            //    }
            //}
            //else
            //{
            //    if (_leftSelect != null)
            //    {
            //        _output += _leftSelect.Name + ",\r\n\t";
            //    }
            //    else
            //    {
            //        _output += "\r\n";
            //    }

            //    if (_rightSelect != null)
            //    {
            //        _output += _rightSelect.Name + "\r\n";
            //    }
            //    else
            //    {
            //        _output += "\r\n";
            //    }
            //}
            #endregion

            _output += left + Environment.NewLine + right;
            if (_leftColumn != null && _rightColumn != null)
            {
                _output += Environment.NewLine + "WHERE\r\n\t" + _leftColumn.Name + " " + _comparisonOperator + " " + _rightColumn.Name;
            }

            SetIndex();
            SetCreateTableColumnList();
        }


        private void SetIndex()
        {
            if (_leftJoin != null)
            {
                foreach (ColumnStructure column in _leftJoin.Columns)
                {
                    if (column.Name == LeftColumnString)
                    {
                        _leftJoin.IndexColumnName = column.Name;
                    }
                    else
                    {
                        if (column.Name == RightColumnString)
                        {
                            _leftJoin.IndexColumnName = column.Name;
                        }
                    }
                }
                
                if (_switched)
                {
                    if (_leftSelect != null)
                    {
                        foreach (ColumnStructure column in _leftSelect.OutColumn)
                        {
                            if (column.Name == LeftColumnString)
                            {
                                _leftSelect.IndexColumnName = column.Name;
                            }
                            else
                            {
                                if (column.Name == RightColumnString)
                                {
                                    _leftSelect.IndexColumnName = column.Name;
                                }
                            }
                        }
                    }
                       
                }
                else
                {
                    if (_rightSelect != null)
                    {
                        foreach (ColumnStructure column in _rightSelect.OutColumn)
                        {
                            if (column.Name == LeftColumnString)
                            {
                                _rightSelect.IndexColumnName = column.Name;
                            }
                            else
                            {
                                if (column.Name == RightColumnString)
                                {
                                    _rightSelect.IndexColumnName = column.Name;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (_leftSelect != null)
                {
                    foreach (ColumnStructure column in _leftSelect.OutColumn)
                    {
                        if (column.Name == LeftColumnString)
                        {
                            _leftSelect.IndexColumnName = column.Name;
                        }
                        else
                        {
                            if (column.Name == RightColumnString)
                            {
                                _leftSelect.IndexColumnName = column.Name;
                            }
                        }
                    }
                   
                }

                if (_rightSelect != null)
                {
                   
                    foreach (ColumnStructure column in _rightSelect.OutColumn)
                    {
                        if (column.Name == LeftColumnString)
                        {
                            _rightSelect.IndexColumnName = column.Name;
                        }
                        else
                        {
                            if (column.Name == RightColumnString)
                            {
                                _rightSelect.IndexColumnName = column.Name;
                            }
                        }
                    }
                 
                }
            }
        }

        private void SetCreateTableColumnList()
        {
            for (int i = 0; i < _outTable.Columns.Length; i++)
            {
                _createTableColumnNames += _outTable.Columns[i].Name + " " + _outTable.Columns[i].Type.Name;
                if (i < _outTable.Columns.Length - 1)
                {
                    _createTableColumnNames += ",\r\n";
                }
            }
        
        }

        private void ColumnCounterDelete()
        {
            if (_leftColumn != null)
            {
                _leftColumn.UsageCounter--;
            }

            if (_rightColumn != null)
            {
                _rightColumn.UsageCounter--;
            }
        }
    }
}
