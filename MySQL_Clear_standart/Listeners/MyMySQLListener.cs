using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using MySQL_Clear_standart.Listeners;

namespace MySQL_Clear_standart
{
    class MyMySQLListener : MySqlParserBaseListener
    {
        public List<string> TableNames = new List<string>();
        public List<string> ColumnNames = new List<string>();
        public List<string> ExprColumnNames = new List<string>();
        public List<JoinStructure> JoinStructures = new List<JoinStructure>();
        public List<WhereStructure> WhereList = new List<WhereStructure>();
        public List<AsStructure> AsList = new List<AsStructure>();
        
        private bool _triggerEnterSelectFunctionElemenAsExist = false;

        public string _return = "Return:\r\n";

        public override void EnterFullColumnName([NotNull] MySqlParser.FullColumnNameContext context)
        {
            ColumnNames.Add(context.GetText());
        }
        public override void EnterTableSourceBase([NotNull] MySqlParser.TableSourceBaseContext context)
        {
            TableNames.Add(context.GetText());
        }

        public override void EnterSelectFunctionElement([NotNull] MySqlParser.SelectFunctionElementContext context)
        {
            if (context.AS() != null)
            {
                AsListener asl = new AsListener();
                ParseTreeWalker wlk = new ParseTreeWalker();
                wlk.Walk(asl, context);
                AsList.Add(new AsStructure(asl.AsColumnList, asl._output, asl._functionOutput, context.uid().GetText()));
                ExprColumnNames.AddRange(asl.AsColumnList);
            }
        }

        public override void EnterBinaryComparasionPredicate([NotNull] MySqlParser.BinaryComparasionPredicateContext context)
        {
            if (context.Stop.Type != 968)
            {
                ExprColumnNames.Add(context.left.GetText());
                WhereList.Add(new WhereStructure(context.GetText(), context.left.GetText()));
            }
            else
            {
                JoinStructures.Add(new JoinStructure(context.Start.Text, context.Stop.Text));
            }
        }
        
    }
}
