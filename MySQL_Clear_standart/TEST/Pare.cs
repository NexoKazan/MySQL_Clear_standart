using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Clear_standart.TEST
{
    class Pare
    {
        private string _left;
        private string _right;
        private bool _isCopy = false;

        public Pare(string left, string right)
        {
            _left = left;
            _right = right;
        }

        public bool IsCopy
        {
            get { return _isCopy;}
            set { _isCopy = value; }
        }

        public string Left {
            get { return _left; }
            set { _left = value; }
        }
        public string Right
        {
            get { return _right; }
            set { _right = value; }

        }
    }
}
