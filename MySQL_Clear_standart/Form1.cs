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
        List<string> Lineitem = new List<string>();        
        public Form1()
        {
            Lineitem.Add("L_ORDERKEY");
            Lineitem.Add("L_SHIPDATE");
            Lineitem.Add("L_EXTENDEDPRICE");
            Lineitem.Add("L_DISCOUNT");
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
            var treeNodeDrawable = new CommonNode(tree);
            TreeVisitor vTree = new TreeVisitor(treeNodeDrawable);

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
            
            //Image image = vTree.Draw();
            vTree.VisitTree(treeNodeDrawable);
            foreach (string str in vTree.GetTablesName(treeNodeDrawable))
            {
                output += str.ToString()+"\r\n";
            }
            output += "\r\n========================\r\n";
            vTree.GetColumnNames(treeNodeDrawable);
            foreach (string str in vTree.ColumnNames)
            {
                output += str + "\r\n";
            }
            output += "\r\n========================\r\n";
            vTree.GetCompairColumnNames(treeNodeDrawable);
            foreach (string str in vTree.CompairColumnNames)
            {
                output += str + "\r\n";
            }
            output += "\r\n========================\r\n";
            output += Select(vTree.ColumnNames, vTree.CompairColumnNames);

            Image image = vTree.Draw();
            textBox1.Text = output;
            pictureBox1.Image = image;
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
    }
}
