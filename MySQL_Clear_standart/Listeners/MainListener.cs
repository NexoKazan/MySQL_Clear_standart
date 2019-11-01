﻿using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using MySQL_Clear_standart.Listeners;
using MySQL_Clear_standart.Q_Structures;
namespace MySQL_Clear_standart.Listeners
{
    class MainListener : MySqlParserBaseListener
    {
        private int _depth;
        private int _tmpDepth;
        private IVocabulary _vocabulary;
        private List<string> _columnNames = new List<string>();
        private List<string> _tableNames = new List<string>();
        private List<string> _selectColumnNames = new List<string>();

        public MainListener(int depth)
        {
            _tmpDepth = _depth = depth;
        }

        public int Depth
        {
            get { return _depth; }
        }
        
        public IVocabulary Vocabulary
        {
            get { return _vocabulary; }
            set { _vocabulary = value; }
        }

        public List<string> ColumnNames
        {
            get { return _columnNames; }
        }

        public List<string> TableNames
        {
            get { return _tableNames; }
        }

        public List<string> SelectColumnNames
        {
            get { return _selectColumnNames; }
        }

        public override void EnterFullColumnName([NotNull] MySqlParser.FullColumnNameContext context)
        {
            _columnNames.Add(context.GetText());
        }

        public override void EnterTableName([NotNull] MySqlParser.TableNameContext context)
        {
            _tableNames.Add(context.GetText());
        }

        public override void EnterSelectColumnElement([NotNull] MySqlParser.SelectColumnElementContext context)
        {
            if(_depth == _tmpDepth)
                _selectColumnNames.Add(context.GetText());
        }

        public override void EnterLogicalExpression([NotNull] MySqlParser.LogicalExpressionContext context)
        {

        }

        public IVocabulary voc;
        //public List<string> TableNames = new List<string>();
        //public List<string> ColumnNames = new List<string>();
        public List<string> ExprColumnNames = new List<string>();
        public List<string> SelectColumns = new List<string>();
        public List<string> GroupByColumnsNames = new List<string>();
        public List<JoinStructure> JoinStructures = new List<JoinStructure>();
        public List<WhereStructure> WhereList = new List<WhereStructure>();
        public List<AsStructure> AsList = new List<AsStructure>();
        public List<OrderByStructure> OrderByList = new List<OrderByStructure>();

        public List<MainListener> SubQueryListeners = new List<MainListener>();
        
        private bool _triggerEnterSelectFunctionElemenAsExist = false;
     

        public string _return = "Return:\r\n";

        
        
        public override void EnterTableSourceBase([NotNull] MySqlParser.TableSourceBaseContext context)
        { 
            if(_depth == _tmpDepth)
            TableNames.Add(context.GetText());
        }

        public override void EnterSelectFunctionElement([NotNull] MySqlParser.SelectFunctionElementContext context)
        {
            if (_depth == _tmpDepth)
            {
                if (context.AS() != null)
                {
                    AsListener asl = new AsListener();
                    ParseTreeWalker wlk = new ParseTreeWalker();
                    wlk.Walk(asl, context);
                    AsList.Add(new AsStructure(asl.AsColumnList, asl._output, asl._functionOutput,
                        context.uid().GetText(), asl._functionName));
                    ExprColumnNames.AddRange(asl.AsColumnList);
                }
            }
        }

        public override void EnterBinaryComparasionPredicate([NotNull] MySqlParser.BinaryComparasionPredicateContext context)
        {//обернуть в два листенера

            if (_depth == _tmpDepth)
            {
                if (!context.GetChild(2).GetChild(0).GetType().ToString().Contains("SubqueryExpessionAtom"))
                {
                    if (context.Stop.Type != 968)
                    {
                        ExprColumnNames.Add(context.left.GetText());
                        WhereList.Add(new WhereStructure(context.GetText(), context.left.GetText()));
                    }
                    else
                    {
                        if (context.Stop.Type == 968)
                        {
                            JoinListener tmpJoinListener = new JoinListener();
                            ParseTreeWalker wlk = new ParseTreeWalker();
                            wlk.Walk(tmpJoinListener, context);
                            JoinStructures.Add(new JoinStructure(context.Start.Text, context.Stop.Text,
                                tmpJoinListener.Output));
                        }
                    }
                }
            }
        }

       

        public override void EnterGroupByItem([NotNull] MySqlParser.GroupByItemContext context)
        {
            if(_depth == _tmpDepth)
            GroupByColumnsNames.Add(context.GetText());
        }

        public override void EnterOrderByExpression([NotNull] MySqlParser.OrderByExpressionContext context)
        {
            if (_depth == _tmpDepth)
            {
                OrderByStructure tmpOrder = new OrderByStructure();
                tmpOrder.ColumnName = context.expression().GetText();
                if (context.order != null)
                {
                    if (context.order.Text == "DESC")
                    {
                        tmpOrder.IsDESC = true;
                    }
                }

                OrderByList.Add(tmpOrder);
            }
        }

        public override void EnterSubqueryExpessionAtom([NotNull] MySqlParser.SubqueryExpessionAtomContext context)
        {
            _depth++;
            MainListener tmpSubListener = new MainListener(_depth);
            ParseTreeWalker walker = new ParseTreeWalker();
            walker.Walk(tmpSubListener, context.selectStatement());
            SubQueryListeners.Add(tmpSubListener);
            
        }

        public override void ExitSubqueryExpessionAtom([NotNull] MySqlParser.SubqueryExpessionAtomContext context)
        {
            _depth--;
        }
    }
}