using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySQL_Clear_standart
{
    class Pares
    {
        private string _left;
        private string _right;
        private bool _isForDelete;

        public Pares(string left, string right)
        {
            _left = left;
            _right = right;
            _isForDelete = false;
        }

        public string Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public string Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public bool IsForDelete
        {
            get { return _isForDelete; }
            set { _isForDelete = value; }
        }
    }
}
