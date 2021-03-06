﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using System.Windows.Forms;

namespace MySQL_Clear_standart
{
    public class CommonNode : ICommonNode
    {
        IParseTree _tree;        
        static int _index;        
        public CommonNode(IParseTree tree)
        {                     
            _tree = tree;
            _index++;
        }             
        public int Index
        {
            get { return _index; }           
        }
        public string Text
        {
            get
            {
                if (_tree.ChildCount != 0)
                {
                    //return _index.ToString() + " " + _tree.GetType().Name.Replace("Context","");
                    return _tree.GetType().Name.Replace("Context", "");                    
                }
                else
                {
                    //return _index.ToString() +" " + _tree.ToString();
                    return _tree.ToString();
                }
            }
        }
        public string BranchText
        {
            get
            {
                return "wait for branchTextRealization";
            }
        }
        public int Count
        {
            get { return _tree.ChildCount; }
        }
        
        public IEnumerable<ICommonNode> Children
        {
            get
            {
                for (int i = 0; i < _tree.ChildCount; ++i)
                {
                    yield return new CommonNode(_tree.GetChild(i));
                }
            }
        }
        public string Type
        {
            get
            {
                switch (_tree.ChildCount)
                {
                    case 0: return "Leaf";
                    case 1: return "Branch";
                    default: return "gRoot";
                }
                         
            }
        }

    }


}