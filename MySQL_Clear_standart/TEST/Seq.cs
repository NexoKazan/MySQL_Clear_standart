using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Clear_standart.TEST
{
    class Seq
    {
        List<string> _sequence = new List<string>(); //1,2

        public Seq(List<string> sequence)
        {
            _sequence = sequence;
        }
        public List<List<string>> MakeSequence(List<Pare> pares)
        {
            List<string> tmpList = new List<string>();
            List<List<string>> outputlList = new List<List<string>>();
            foreach (Pare pare in pares)
            {
                tmpList.AddRange(_sequence);
                tmpList.Add(pare.Left); //2
                tmpList.Add(pare.Right); //3
                tmpList.Reverse();
                tmpList = tmpList.Distinct().ToList();
                tmpList.Reverse();
                if (tmpList.Count == _sequence.Count + 1)
                {
                    outputlList.Add(tmpList);
                }
            }

            return outputlList;
        }
    }

}
