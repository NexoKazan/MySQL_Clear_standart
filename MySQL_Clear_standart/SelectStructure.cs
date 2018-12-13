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
        private List<string> _whereList;
        
        public SelectStructure(string name, string tableName, List<string> columnsList, List<string> whereList)
        {
            _name = name;
            _tableName = tableName;
            _columnsList = columnsList;
            _whereList = whereList;
        }

        public string Output
        {
            get
            {
                CreateQuerry();
                return _output;
            }
        }

        private void CreateQuerry()
        {
            _output = "SELECT ";
            foreach (string s in _columnsList)
            {
                _output += s + " ";
            }

            _output += "\r\n" + "FROM " + _tableName + "\r\n" + "WHERE " + "\r\n";
            foreach (string s in _whereList)
            {
                _output += s + "\r\n";
            }

        }
    }
}
