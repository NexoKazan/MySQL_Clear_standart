using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace MySQL_Clear_standart.Listeners
{
    class JoinListener : MySqlParserBaseListener
    {
        private string _output = "Error!";

        public string Output
        {
            get
            {
                return _output; 
            }
        }

        public override void EnterComparisonOperator([NotNull] MySqlParser.ComparisonOperatorContext context)
        {
            _output = context.GetText();
        }
    }
}
