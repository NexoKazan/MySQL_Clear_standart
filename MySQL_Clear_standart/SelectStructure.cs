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
        private List<string> _outColumnNamesList;
        private List<WhereStructure> _whereList;
        private List<AsStructure> _asList;
        private TableStructure[] _outTable;
        private ColumnStructure[] _outColumn;
        
        public SelectStructure(string name, string tableName, List<string> columnsList, List<WhereStructure> whereList, List<AsStructure> asList)
        {
            _name = name;
            _tableName = tableName;
            _columnsList = columnsList;
            _whereList = whereList;
            _asList = asList;
            _outColumn = new ColumnStructure[columnsList.Count + asList.Count];
            for (int i = 0; i < columnsList.Count + asList.Count;)
            {
                foreach (string column in columnsList)
                {
                    _outColumn[i] = new ColumnStructure(column);
                    i++;
                }

                foreach (AsStructure structure in asList)
                {
                    _outColumn[i] = new ColumnStructure(structure.GetAsRightName);
                    i++;
                }
            } 
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
