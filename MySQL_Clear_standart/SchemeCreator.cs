using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;

namespace MySQL_Clear_standart
{
    class SchemeCreator
    {
        private string _return = "Return:\r\n";
        private DataBaseStructure _inDatabase;
        private List<SelectStructure> _inSelectStructures;

        #region Propirties

        public string Return { get; }
        #endregion

        #region Constructors

        public SchemeCreator(DataBaseStructure inDataBase, List<SelectStructure> inSelectStructures)
        {
            _inDatabase = inDataBase;
            _inSelectStructures = inSelectStructures;
        }

        #endregion

        public void TestMethod()
        {
            foreach (SelectStructure inSelectStructure in _inSelectStructures)
            {
                _return += inSelectStructure;
            }
        }
    }
}
