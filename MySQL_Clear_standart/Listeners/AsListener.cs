using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace MySQL_Clear_standart.Listeners
{
    class AsListener: MySqlParserBaseListener
    {
        public string _output;

        public List<string> AsColumnList= new List<string>();

        public override void EnterFunctionArg([NotNull] MySqlParser.FunctionArgContext context)
        {
            _output += context.GetText();
        }

        public override void EnterFullColumnName([NotNull] MySqlParser.FullColumnNameContext context)
        {
            AsColumnList.Add(context.GetText());
        }
    }
}
