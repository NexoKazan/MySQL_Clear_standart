using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace MySQL_Clear_standart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputString = textBox1.Text;
            string output = " ";
            //output = textBox1.Text;
            ICharStream inputStream = new AntlrInputStream(inputString.ToString());
            ITokenSource mySqlLexer = new MySqlLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(mySqlLexer);
            MySqlParser mySqlParser = new MySqlParser(commonTokenStream);
            mySqlParser.BuildParseTree = true;
            IParseTree tree = mySqlParser.root();
            var treeNodeDrawable = new CommonNode(tree, 0);
            TreeVisitor vTree = new TreeVisitor(treeNodeDrawable);

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
            
            Image image = vTree.Draw();
            vTree.VisitTree(treeNodeDrawable);
            foreach (string str in vTree.GetTablesName(treeNodeDrawable))
            {
                output += str.ToString()+"\r\n";
            }
            output += "\r\n========================";
            foreach (ICommonNode node in vTree.GetTerminalNodes(treeNodeDrawable))
            {
                output += node.Index.ToString() + "\r\n";
            }
            //Image image = vTree.Draw();
            textBox1.Text = output;
            pictureBox1.Image = image;
        }
    }
}
