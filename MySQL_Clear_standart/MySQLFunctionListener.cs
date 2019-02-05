using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using System.Threading.Tasks;

namespace MySQL_Clear_standart
{
    class MySQLFunctionListener : MySqlParserBaseListener
    {
        public List<string> FunctionColumns = new List<string>();
        public override void EnterFullColumnName([NotNull] MySqlParser.FullColumnNameContext context)
        {
            FunctionColumns.Add(context.GetText());
        }
    }
}
