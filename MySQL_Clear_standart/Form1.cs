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
using System.Xml.Serialization;
using MySQL_Clear_standart.DataBaseSchemeStructure;
using System.IO;
using System.Runtime.InteropServices;

namespace MySQL_Clear_standart
{
    public partial class Form1 : Form
    {
        #region Создание схемы БД
       /*C_CUSTKEY
         C_MKTSEGMENT
         L_DISCOUNT
         L_EXTENDEDPRICE
         L_ORDERKEY
         L_SHIPDATE
         O_CUSTKEY
         O_ORDERDATE
         O_ORDERKEY
         O_SHIPPRIORITY
       */

        private DataBaseStructure _dbName;
        private TableStructure[] _table;
        private ColumnStructure[] _orderColumns;
        private ColumnStructure[] _lineitemColumns;
        private ColumnStructure[] _customerColumns;

        private void FillScheme()
        {
            #region Устаревший метод заполнения схемы БД.
            //_orderColumns = new ColumnStructure[4];
            //_orderColumns[0] = new ColumnStructure("O_CUSTKEY");
            //_orderColumns[1] = new ColumnStructure("O_ORDERDATE");
            //_orderColumns[2] = new ColumnStructure("O_ORDERKEY");
            //_orderColumns[3] = new ColumnStructure("O_SHIPPRIORITY");

            //_lineitemColumns = new ColumnStructure[4];
            //_lineitemColumns[0] = new ColumnStructure("L_DISCOUNT");
            //_lineitemColumns[1] = new ColumnStructure("L_EXTENDEDPRICE");
            //_lineitemColumns[2] = new ColumnStructure("L_ORDERKEY");
            //_lineitemColumns[3] = new ColumnStructure("L_SHIPDATE");

            //_customerColumns = new ColumnStructure[2];
            //_customerColumns[0] = new ColumnStructure("C_CUSTKEY");
            //_customerColumns[1] = new ColumnStructure("C_MKTSEGMENT");

            //_table = new TableStructure[3];
            //_table[0] = new TableStructure("CUSTOMER", _customerColumns);
            //_table[1] = new TableStructure("LINEITEM", _lineitemColumns);
            //_table[2] = new TableStructure("ORDERS", _orderColumns);

            //_dbName = new DataBaseStructure("TPCH", _table); 
            #endregion

            using (FileStream dbCreateFileStream = new FileStream("db.xml", FileMode.Open, FileAccess.ReadWrite))  // Заполнение БД из XML
            {
                XmlSerializer dbCreateSerializer = new XmlSerializer(typeof(DataBaseStructure));
                _dbName = (DataBaseStructure) dbCreateSerializer.Deserialize(dbCreateFileStream);
            }
        }


        #endregion
        private SelectStructure[] _selectQuerry;
       
        #region baseDefinition объявляются переменный для построения дерева
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
            InitializeComponent();
            GetTree();
        }

        #region чек сериализации
        protected void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            MessageBox.Show("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        protected void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            MessageBox.Show("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            //Кнопка для картинки
            DefaultOutput();
           // output = 
            textBox1.Text = output;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //кнопка для сохранения картинки
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
            //восстановление Запроса
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

        private void button4_Click(object sender, EventArgs e)
        {
            //составление запросов SELECT
            GetTree();
            _selectQuerry = new SelectStructure[listener.TableNames.Count];
            listener.TableNames.Sort();
            foreach (WhereStructure ws in listener.WhereList)
            {
                ws.FindeTable(_dbName);
            }
            for (int i = 0; i < listener.TableNames.Count; i++)
            {
                _selectQuerry[i] = new SelectStructure("s" + i.ToString(), listener.TableNames[i], 
                    GetClearColumns(listener.ColumnNames, listener.ExprColumnNames, _dbName.Tables[i]),
                    listener.WhereList);
            }

            textBox3.Clear();
            for (int i = 0; i < _selectQuerry.Length; i++)
            {
                textBox3.Text += "\r\n========" + _selectQuerry[i].Name + "=========\r\n";
                textBox3.Text += _selectQuerry[i].Output + "\r\n";
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ReSize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillScheme();
            ReSize();
            using (FileStream fs = new FileStream("db_result.xml", FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer dbSerializer = new XmlSerializer(typeof(DataBaseStructure));
                dbSerializer.Serialize(fs, _dbName);
            }
            
           
        }



        private void GetTree()
        {
            textBox2.Text = textBox1.Text;
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
        private void ReSize()
        {
            textBox2.Width = textBox3.Width = (tabPage2.Width - 212) / 2;
            textBox3.Location = new Point(textBox2.Location.X + 200 + textBox2.Width, textBox3.Location.Y);
        }
        private List<string> GetClearColumns(List<string> allColumns, List<string> removeColumns, TableStructure table)
        {
            List<string> inList = allColumns;
            List<string> outList = new List<string>();
            int j = 0;
            for (int i = 0; i < inList.Count; i++)
            {
                if (j < removeColumns.Count && inList[i] == removeColumns[j])
                {
                    inList.Remove(inList[i]);
                    i = 0;
                    j++;
                }
            }
            inList = inList.Distinct().ToList();
            foreach (string allColumn in inList)
            {
                for (int i = 0; i < table.Columns.Length; i++)
                {
                    if (allColumn == table.Columns[i].Name)
                    {
                        outList.Add(allColumn);
                        break;
                    }
                }
            }
            return outList;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text;
        }
    }
}
