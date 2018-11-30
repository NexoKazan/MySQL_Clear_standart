using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL_Clear_standart
{
    class MyMySQLListener : MySqlParserBaseListener
    {
        public List<string> TableNames = new List<string>();
        public List<string> ColumnNames = new List<string>();
        public List<string> ExprColumnNames = new List<string>();
        public string _return = "Return: ";
        public string _asString;

        public override void EnterUid([NotNull] MySqlParser.UidContext context) {
            //nodeText += context.GetText() + " ";
        }
        public override void EnterFullColumnName([NotNull] MySqlParser.FullColumnNameContext context)
        {
            ColumnNames.Add(context.GetText());
        }
        public override void EnterTableSourceBase([NotNull] MySqlParser.TableSourceBaseContext context) {
            TableNames.Add(context.GetText());
        }
        public override void ExitSelectFunctionElement([NotNull] MySqlParser.SelectFunctionElementContext context) {
            _asString += context.uid().GetText();
        }
       
        public override void EnterBinaryComparasionPredicate([NotNull] MySqlParser.BinaryComparasionPredicateContext context) {
            //_return += context.right.GetChild(0).GetType().Name + " \r\n";
            //_return += context.right.GetText() + " \r\n";
            if (context.Stop.Type != 968)
            {
                ExprColumnNames.Add(context.left.GetText());
            }
        }
    }
}
