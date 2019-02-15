using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private void CreateQuerry()
        {
            _output = "SELECT ";
            foreach (string s in _columnsList)
            {
                _output +="\r\n\t" + s + " ";
            }

            foreach (var asStructure in _asList)
            {
                _output += "\r\n\t" + asStructure.GetAsString + " AS " +asStructure.GetAsRightName;
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
