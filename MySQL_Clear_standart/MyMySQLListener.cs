using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;

namespace MySQL_Clear_standart
{
    class MyMySQLListener : MySqlParserBaseListener
    {
        public List<string> TableNames = new List<string>();
        public List<string> ColumnNames = new List<string>();
        public List<string> ExprColumnNames = new List<string>();
        public List<WhereStructure> WhereList = new List<WhereStructure>();
        public string _return = "Return:\r\n";
        public string _asString;

        public override void EnterUid([NotNull] MySqlParser.UidContext context) {
            //nodeText += context.GetText() + " ";
            //_return += context.Stop.Text + "!! \r\n";
        }

        public override void EnterQuerySpecification([NotNull] MySqlParser.QuerySpecificationContext context)
        {
            //_return += context.Start.Text + " " + context.Start.Type + "!! \r\n";
            _return += context.getAltNumber() + "!! \r\n";
        }
        public override void EnterFullColumnName([NotNull] MySqlParser.FullColumnNameContext context)
        {
            ColumnNames.Add(context.GetText());
        }
        public override void EnterTableSourceBase([NotNull] MySqlParser.TableSourceBaseContext context)
        {
            TableNames.Add(context.GetText());
        }
        public override void ExitSelectFunctionElement([NotNull] MySqlParser.SelectFunctionElementContext context)
        {
            _asString += context.uid().GetText();
        }
       
        public override void EnterBinaryComparasionPredicate([NotNull] MySqlParser.BinaryComparasionPredicateContext context)
        {
            //_return += context.Stop.Text + context.Stop.Type + "!! \r\n";
            if (context.Stop.Type != 968)
            {
                ExprColumnNames.Add(context.left.GetText());
                WhereList.Add(new WhereStructure(context.Payload.GetText(), context.left.GetText()));
            }
        }
    }
}
