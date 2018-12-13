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
using System.Drawing.Imaging;

namespace MySQL_Clear_standart
{
    public partial class Form1 : Form
    {
        #region baseImpl
        List<string> Lineitem = new List<string>();
        private string output = " ";
        private string inputString;
        private ICharStream inputStream;
        private ITokenSource mySqlLexer;
        private CommonTokenStream commonTokenStream;
        private MySqlParser mySqlParser;
        private IParseTree tree;
        private CommonNode treeNodeDrawable;
        private TreeVisitor vTree;
        private ParseTreeWalker walker;
        private MyMySQLListener listener;
        #endregion

        public Form1()
        {
            Lineitem.Add("L_ORDERKEY");
            Lineitem.Add("L_SHIPDATE");
            Lineitem.Add("L_EXTENDEDPRICE");
            Lineitem.Add("L_DISCOUNT");
            InitializeComponent();
            inputString = textBox1.Text;
            inputStream = new AntlrInputStream(inputString);
            mySqlLexer = new MySqlLexer(inputStream);
            commonTokenStream = new CommonTokenStream(mySqlLexer);
            mySqlParser = new MySqlParser(commonTokenStream);
            mySqlParser.BuildParseTree = true;
            tree = mySqlParser.root();
            treeNodeDrawable = new CommonNode(tree);
            vTree = new TreeVisitor(treeNodeDrawable);
            walker = new ParseTreeWalker();
            listener = new MyMySQLListener();
            walker.Walk(listener, tree);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DefaultOutput();
            textBox1.Text = output;
        }
        private string Select(List<string> columns, List<string> compairColumns)
        {
            columns.Distinct();
            compairColumns.Distinct();
            List<string> clearColumns = columns.Except(compairColumns).ToList();
            
            string selectString = "SELECT";
            //foreach (string str in clearColumns)
            //{
            //    selectString += " " + str;
            //}
            foreach (string col in Lineitem)
            {
                foreach (string str in clearColumns)
                {
                    if (col == str)
                    {
                        selectString += " " + str;
                    }
                }
            }
            selectString += " FROM Lineitem";
            return selectString;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Images|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(saveFileDialog1.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                pictureBox1.Image.Save(saveFileDialog1.FileName, format);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = \'HOUSEHOLD\'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < \'1995-03-31\'\r\n\tAND L_SHIPDATE  > \'1995-03-31\'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
        }

        private void DefaultOutput()
        {
            #region вынесено в зону видимости класса

            //string inputString = textBox1.Text;
            //string output = " ";
            //ICharStream inputStream = new AntlrInputStream(inputString);
            //ITokenSource mySqlLexer = new MySqlLexer(inputStream);
            //CommonTokenStream commonTokenStream = new CommonTokenStream(mySqlLexer);
            //MySqlParser mySqlParser = new MySqlParser(commonTokenStream);
            //mySqlParser.BuildParseTree = true;
            //IParseTree tree = mySqlParser.root();
            //var treeNodeDrawable = new CommonNode(tree);
            //TreeVisitor vTree = new TreeVisitor(treeNodeDrawable);
            //if (pictureBox1.Image != null)
            //{
            //    pictureBox1.Image.Dispose();
            //    pictureBox1.Image = null;
            //}
            //ParseTreeWalker walker = new ParseTreeWalker();
            //MyMySQLListener listener = new MyMySQLListener();
            //walker.Walk(listener, tree);
            #endregion
            

            output += "\r\n========TableNames============\r\n";
           
            foreach (string tableName in listener.TableNames)
            {
                output += tableName + "\r\n";
            }
            output += "\r\n========ColumnNames===========\r\n";
            List<string> col = listener.ColumnNames;
            col = col.Distinct().ToList();
            col.Sort();
            col.Remove(listener._asString);
            foreach (string columnName in col)
            {
                output += columnName + "\r\n";
            }
            output += "\r\n========ExprColumnNames========\r\n";
            List<string> exprCol = listener.ExprColumnNames;
            exprCol = exprCol.Distinct().ToList();
            exprCol.Sort();
            exprCol.Remove(listener._asString);
            foreach (string columnName in exprCol)
            {
                output += columnName + "\r\n";
            }
            output += "\r\n========Return================\r\n";
            output += listener._return;
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
            Image image = vTree.Draw();
            pictureBox1.Image = image;
        }
    }
}
