using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQL_Clear_standart.DataBaseSchemeStructure;
using MySQL_Clear_standart.Listeners;

namespace MySQL_Clear_standart.Q_Part_Structures
{
    public enum PredicateType : int
    {
        simple = 1,
        join = 2,
        subQ = 3
    }

    class BinaryComparisionPredicateStructure
    {
        private int _type; // 1-simple predicate, 2-join predicate, 3-sub q predicate;

        private int _subQID;

        private string _leftType;
        private string _rightType;

        private string _leftString;
        private string _rightString;

        private string _comparisionSymphol;

        public BinaryComparisionPredicateStructure(string leftString, string comparisionSymphol, string rightString)
        {
            _leftString = leftString;
            _rightString = rightString;
            _comparisionSymphol = comparisionSymphol;
        }

        public int Type
        {
            get => _type;
            set => _type = value;
        }

        public string LeftType
        {
            get => _leftType;
            set => _leftType = value;
        }

        public string RightType
        {
            get => _rightType;
            set => _rightType = value;
        }

        public string LeftString
        {
            get => _leftString;
            set => _leftString = value;
        }

        public string RightString
        {
            get => _rightString;
            set => _rightString = value;
        }

        public string ComparisionSymphol
        {
            get => _comparisionSymphol;
            set => _comparisionSymphol = value;
        }

        public int SubQid
        {
            get { return _subQID; }
            set { _subQID = value; }
        }
    }
}
