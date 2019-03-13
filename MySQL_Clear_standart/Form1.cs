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
using MySQL_Clear_standart.Properties;


namespace MySQL_Clear_standart
{
    public partial class Form1 : Form
    {
        #region Создание схемы БД
       

        private DataBaseStructure _dbName;
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
            using (FileStream dbCreateFileStream = new FileStream("res//db.xml", FileMode.Open, FileAccess.ReadWrite))  // Заполнение БД из XML
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
            //Кнопка для картинки(построить дерево)
            GetTree();
            DefaultOutput();
            //output = 
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
            //выбрать запрос
            pictureBox1.Visible = true;
            textBox1.Width = 283;
            //запросы TPC-H (убраны Date)
            switch (Convert.ToInt16(comboBox1.Text))
            {
                case 1:
                    textBox1.Text =
                        "SELECT\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS,\r\n\tSUM(L_QUANTITY) AS SUM_QTY,\r\n\tSUM(L_EXTENDEDPRICE) AS SUM_BASE_PRICE,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS SUM_DISC_PRICE,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT) * (1 + L_TAX)) AS SUM_CHARGE,\r\n\tAVG(L_QUANTITY) AS AVG_QTY,\r\n\tAVG(L_EXTENDEDPRICE) AS AVG_PRICE,\r\n\tAVG(L_DISCOUNT) AS AVG_DISC,\r\n\tCOUNT(*) AS COUNT_ORDER\r\nFROM\r\n\tLINEITEM\r\nWHERE\r\n\tL_SHIPDATE <='1998-12-01' - INTERVAL '90' DAY\r\nGROUP BY\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS\r\nORDER BY\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS;\r\n";
                    break;
                case 2:
                    textBox1.Text =
                        "SELECT\r\n\tS_ACCTBAL,\r\n\tS_NAME,\r\n\tN_NAME,\r\n\tP_PARTKEY,\r\n\tP_MFGR,\r\n\tS_ADDRESS,\r\n\tS_PHONE,\r\n\tS_COMMENT\r\nFROM\r\n\tPART,\r\n\tSUPPLIER,\r\n\tPARTSUPP,\r\n\tNATION,\r\n\tREGION\r\nWHERE\r\n\tP_PARTKEY = PS_PARTKEY\r\n\tAND S_SUPPKEY = PS_SUPPKEY\r\n\tAND P_SIZE = 48\r\n\tAND P_TYPE LIKE '%NICKEL'\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_REGIONKEY = R_REGIONKEY\r\n\tAND R_NAME = 'AMERICA'\r\n\tAND PS_SUPPLYCOST = (\r\n\t\tSELECT\r\n\t\t\tMIN(PS_SUPPLYCOST)\r\n\t\tFROM\r\n\t\t\tPARTSUPP,\r\n\t\t\tSUPPLIER,\r\n\t\t\tNATION,\r\n\t\t\tREGION\r\n\t\tWHERE\r\n\t\t\tP_PARTKEY = PS_PARTKEY\r\n\t\t\tAND S_SUPPKEY = PS_SUPPKEY\r\n\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\tAND N_REGIONKEY = R_REGIONKEY\r\n\t\t\tAND R_NAME = 'AMERICA'\r\n\t)\r\nORDER BY\r\n\tS_ACCTBAL DESC,\r\n\tN_NAME,\r\n\tS_NAME,\r\n\tP_PARTKEY;\r\n";
                    break;
                case 3:
                    textBox1.Text =
                        "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = 'HOUSEHOLD'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < '1995-03-31'\r\n\tAND L_SHIPDATE > '1995-03-31'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
                    break;
                case 4:
                    textBox1.Text =
                        "SELECT\r\n\tO_ORDERPRIORITY,\r\n\tCOUNT(*) AS ORDER_COUNT\r\nFROM\r\n\tORDERS\r\nWHERE\r\n\tO_ORDERDATE >= '1996-02-01'\r\n\tAND O_ORDERDATE < '1996-02-01' + INTERVAL '3' MONTH\r\n\tAND EXISTS (\r\n\t\tSELECT\r\n\t\t\t*\r\n\t\tFROM\r\n\t\t\tLINEITEM\r\n\t\tWHERE\r\n\t\t\tL_ORDERKEY = O_ORDERKEY\r\n\t\t\tAND L_COMMITDATE < L_RECEIPTDATE\r\n\t)\r\nGROUP BY\r\n\tO_ORDERPRIORITY\r\nORDER BY\r\n\tO_ORDERPRIORITY;\r\n";
                    break;
                case 5:
                    textBox1.Text =
                        "SELECT\r\n\tN_NAME,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM,\r\n\tSUPPLIER,\r\n\tNATION,\r\n\tREGION\r\nWHERE\r\n\tC_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND L_SUPPKEY = S_SUPPKEY\r\n\tAND C_NATIONKEY = S_NATIONKEY\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_REGIONKEY = R_REGIONKEY\r\n\tAND R_NAME = 'MIDDLE EAST'\r\n\tAND O_ORDERDATE >= '1995-01-01'\r\n\tAND O_ORDERDATE < '1995-01-01' + INTERVAL '1' YEAR\r\nGROUP BY\r\n\tN_NAME\r\nORDER BY\r\n\tREVENUE DESC;\r\n";
                    break;
                case 6:
                    textBox1.Text =
                        "SELECT\r\n\tSUM(L_EXTENDEDPRICE * L_DISCOUNT) AS REVENUE\r\nFROM\r\n\tLINEITEM\r\nWHERE\r\n\tL_SHIPDATE >= '1997-01-01'\r\n\tAND L_SHIPDATE < '1997-01-01' + INTERVAL '1' YEAR\r\n\tAND L_DISCOUNT BETWEEN 0.07 - 0.01 AND 0.07 + 0.01\r\n\tAND L_QUANTITY < 24;\r\n";
                    break;
                case 7:
                    textBox1.Text =
                        "SELECT\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR,\r\n\tSUM(VOLUME) AS REVENUE\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tN1.N_NAME AS SUPP_NATION,\r\n\t\t\tN2.N_NAME AS CUST_NATION,\r\n\t\t\tEXTRACT(YEAR FROM L_SHIPDATE) AS L_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) AS VOLUME\r\n\t\tFROM\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tORDERS,\r\n\t\t\tCUSTOMER,\r\n\t\t\tNATION N1,\r\n\t\t\tNATION N2\r\n\t\tWHERE\r\n\t\t\tS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND O_ORDERKEY = L_ORDERKEY\r\n\t\t\tAND C_CUSTKEY = O_CUSTKEY\r\n\t\t\tAND S_NATIONKEY = N1.N_NATIONKEY\r\n\t\t\tAND C_NATIONKEY = N2.N_NATIONKEY\r\n\t\t\tAND (\r\n\t\t\t\t(N1.N_NAME = 'IRAQ' AND N2.N_NAME = 'ALGERIA')\r\n\t\t\t\tOR (N1.N_NAME = 'ALGERIA' AND N2.N_NAME = 'IRAQ')\r\n\t\t\t)\r\n\t\t\tAND L_SHIPDATE BETWEEN '1995-01-01' AND '1996-12-31'\r\n\t) AS SHIPPING\r\nGROUP BY\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR\r\nORDER BY\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR;\r\n";
                    break;
                case 8:
                    textBox1.Text =
                        "SELECT\r\n\tO_YEAR,\r\n\tSUM(CASE\r\n\t\tWHEN NATION = 'IRAN' THEN VOLUME\r\n\t\tELSE 0\r\n\tEND) / SUM(VOLUME) AS MKT_SHARE\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tEXTRACT(YEAR FROM O_ORDERDATE) AS O_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) AS VOLUME,\r\n\t\t\tN2.N_NAME AS NATION\r\n\t\tFROM\r\n\t\t\tPART,\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tORDERS,\r\n\t\t\tCUSTOMER,\r\n\t\t\tNATION N1,\r\n\t\t\tNATION N2,\r\n\t\t\tREGION\r\n\t\tWHERE\r\n\t\t\tP_PARTKEY = L_PARTKEY\r\n\t\t\tAND S_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND L_ORDERKEY = O_ORDERKEY\r\n\t\t\tAND O_CUSTKEY = C_CUSTKEY\r\n\t\t\tAND C_NATIONKEY = N1.N_NATIONKEY\r\n\t\t\tAND N1.N_REGIONKEY = R_REGIONKEY\r\n\t\t\tAND R_NAME = 'MIDDLE EAST'\r\n\t\t\tAND S_NATIONKEY = N2.N_NATIONKEY\r\n\t\t\tAND O_ORDERDATE BETWEEN '1995-01-01' AND '1996-12-31'\r\n\t\t\tAND P_TYPE = 'STANDARD BRUSHED BRASS'\r\n\t) AS ALL_NATIONS\r\nGROUP BY\r\n\tO_YEAR\r\nORDER BY\r\n\tO_YEAR;\r\n";
                    break;
                case 9:
                    textBox1.Text =
                        "SELECT\r\n\tNATION,\r\n\tO_YEAR,\r\n\tSUM(AMOUNT) AS SUM_PROFIT\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tN_NAME AS NATION,\r\n\t\t\tEXTRACT(YEAR FROM O_ORDERDATE) AS O_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) - PS_SUPPLYCOST * L_QUANTITY AS AMOUNT\r\n\t\tFROM\r\n\t\t\tPART,\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tPARTSUPP,\r\n\t\t\tORDERS,\r\n\t\t\tNATION\r\n\t\tWHERE\r\n\t\t\tS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND PS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND PS_PARTKEY = L_PARTKEY\r\n\t\t\tAND P_PARTKEY = L_PARTKEY\r\n\t\t\tAND O_ORDERKEY = L_ORDERKEY\r\n\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\tAND P_NAME LIKE '%SNOW%'\r\n\t) AS PROFIT\r\nGROUP BY\r\n\tNATION,\r\n\tO_YEAR\r\nORDER BY\r\n\tNATION,\r\n\tO_YEAR DESC;\r\n";
                    break;
                case 10:
                    textBox1.Text =
                        "SELECT\r\n\tC_CUSTKEY,\r\n\tC_NAME,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tC_ACCTBAL,\r\n\tN_NAME,\r\n\tC_ADDRESS,\r\n\tC_PHONE,\r\n\tC_COMMENT\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM,\r\n\tNATION\r\nWHERE\r\n\tC_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE >= '1994-04-01'\r\n\tAND O_ORDERDATE < '1994-04-01' + INTERVAL '3' MONTH\r\n\tAND L_RETURNFLAG = 'R'\r\n\tAND C_NATIONKEY = N_NATIONKEY\r\nGROUP BY\r\n\tC_CUSTKEY,\r\n\tC_NAME,\r\n\tC_ACCTBAL,\r\n\tC_PHONE,\r\n\tN_NAME,\r\n\tC_ADDRESS,\r\n\tC_COMMENT\r\nORDER BY\r\n\tREVENUE DESC;\r\n";
                    break;
                case 11:
                    textBox1.Text =
                        "SELECT\r\n\tPS_PARTKEY,\r\n\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) AS VALUE\r\nFROM\r\n\tPARTSUPP,\r\n\tSUPPLIER,\r\n\tNATION\r\nWHERE\r\n\tPS_SUPPKEY = S_SUPPKEY\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_NAME = 'ALGERIA'\r\nGROUP BY\r\n\tPS_PARTKEY HAVING\r\n\t\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) > (\r\n\t\t\tSELECT\r\n\t\t\t\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) * 0.0001000000\r\n\t\t\tFROM\r\n\t\t\t\tPARTSUPP,\r\n\t\t\t\tSUPPLIER,\r\n\t\t\t\tNATION\r\n\t\t\tWHERE\r\n\t\t\t\tPS_SUPPKEY = S_SUPPKEY\r\n\t\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\t\tAND N_NAME = 'ALGERIA'\r\n\t\t)\r\nORDER BY\r\n\tVALUE DESC;\r\n";
                    break;
                case 12:
                    textBox1.Text =
                        "SELECT\r\n\tL_SHIPMODE,\r\n\tSUM(CASE\r\n\t\tWHEN O_ORDERPRIORITY = '1-URGENT'\r\n\t\t\tOR O_ORDERPRIORITY = '2-HIGH'\r\n\t\t\tTHEN 1\r\n\t\tELSE 0\r\n\tEND) AS HIGH_LINE_COUNT,\r\n\tSUM(CASE\r\n\t\tWHEN O_ORDERPRIORITY <> '1-URGENT'\r\n\t\t\tAND O_ORDERPRIORITY <> '2-HIGH'\r\n\t\t\tTHEN 1\r\n\t\tELSE 0\r\n\tEND) AS LOW_LINE_COUNT\r\nFROM\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tO_ORDERKEY = L_ORDERKEY\r\n\tAND L_SHIPMODE IN ('AIR', 'SHIP')\r\n\tAND L_COMMITDATE < L_RECEIPTDATE\r\n\tAND L_SHIPDATE < L_COMMITDATE\r\n\tAND L_RECEIPTDATE >= '1994-01-01'\r\n\tAND L_RECEIPTDATE < '1994-01-01' + INTERVAL '1' YEAR\r\nGROUP BY\r\n\tL_SHIPMODE\r\nORDER BY\r\n\tL_SHIPMODE;\r\n";
                    break;
                case 13:
                    textBox1.Text =
                        "SELECT\r\n\tC_COUNT,\r\n\tCOUNT(*) AS CUSTDIST\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tC_CUSTKEY,\r\n\t\t\tCOUNT(O_ORDERKEY) AS C_COUNT\r\n\t\tFROM\r\n\t\t\tCUSTOMER LEFT OUTER JOIN ORDERS ON\r\n\t\t\t\tC_CUSTKEY = O_CUSTKEY\r\n\t\t\t\tAND O_COMMENT NOT LIKE '%SPECIAL%REQUESTS%'\r\n\t\tGROUP BY\r\n\t\t\tC_CUSTKEY\r\n\t) AS C_ORDERS\r\nGROUP BY\r\n\tC_COUNT\r\nORDER BY\r\n\tCUSTDIST DESC,\r\n\tC_COUNT DESC;\r\n";
                    break;
                case 14:
                    textBox1.Text =
                        "SELECT\r\n\t100.00 * SUM(CASE\r\n\t\tWHEN P_TYPE LIKE 'PROMO%'\r\n\t\t\tTHEN L_EXTENDEDPRICE * (1 - L_DISCOUNT)\r\n\t\tELSE 0\r\n\tEND) / SUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS PROMO_REVENUE\r\nFROM\r\n\tLINEITEM,\r\n\tPART\r\nWHERE\r\n\tL_PARTKEY = P_PARTKEY\r\n\tAND L_SHIPDATE >= '1995-01-01'\r\n\tAND L_SHIPDATE < '1995-01-01' + INTERVAL '1' MONTH;\r\n";
                    break;
                default:
                    textBox1.Text =
                        "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tSUM(C_MKTSEGMENT * (1 - L_DISCOUNT)) AS REVENUE2,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = \'HOUSEHOLD\'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < \'1995-03-31\'\r\n\tAND L_SHIPDATE  > \'1995-03-31\'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
                    break;
            }

            if (Convert.ToInt16(comboBox1.Text) < 14)
            {
                comboBox1.Text = (Convert.ToInt16(comboBox1.Text) + 1).ToString();
            }
            else
            {
                comboBox1.Text = "0";
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            //составление запросов SELECT
            GetTree();
            _selectQuerry = new SelectStructure[listener.TableNames.Count];
            foreach (WhereStructure ws in listener.WhereList)
            {
                ws.FindeTable(_dbName);
            }

            foreach (AsStructure asStructure in listener.AsList)
            {
                asStructure.FindeTable(_dbName);
            }

            for (int i = 0; i < listener.TableNames.Count; i++)
            {
                _selectQuerry[i] = new SelectStructure("s" + i.ToString(), listener.TableNames[i],
                                       GetClearColumns(listener.ColumnNames, listener.ExprColumnNames,
                                           GetCorrectTable(listener.TableNames[i], _dbName)),
                                            listener.WhereList, GetCorrectAsStructure(listener.AsList, listener.TableNames[i]));
            }

            textBox3.Clear();
            for (int i = 0; i < _selectQuerry.Length; i++)
            {
                textBox3.Text += "\r\n========" + _selectQuerry[i].Name + "=========\r\n";
                textBox3.Text += _selectQuerry[i].Output + "\r\n";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //GetQuerryTreesScreens(@"D:\!Studing\Скриншоты деревьев\Originals\",12,14);
            //отладка
            pictureBox1.Visible = false;
            textBox1.Width = Width - 8;
            GetTree();
            output = "\r\n========Return================\r\n";
            button4.PerformClick();

            foreach (var selectStructure in _selectQuerry)
            {
                output += selectStructure.Name + " " + selectStructure.OutColumn.Length;
                for (int i = 0; i < selectStructure.OutColumn.Length; i++)
                {
                    output += selectStructure.OutColumn[i].Name + "\r\n";
                }
            }
            textBox1.Text = output;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Выбрать запрос на 2й вкладке
            switch (Convert.ToInt16(comboBox2.Text))
            {
                case 1:
                    textBox2.Text =
                        "SELECT\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS,\r\n\tSUM(L_QUANTITY) AS SUM_QTY,\r\n\tSUM(L_EXTENDEDPRICE) AS SUM_BASE_PRICE,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS SUM_DISC_PRICE,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT) * (1 + L_TAX)) AS SUM_CHARGE,\r\n\tAVG(L_QUANTITY) AS AVG_QTY,\r\n\tAVG(L_EXTENDEDPRICE) AS AVG_PRICE,\r\n\tAVG(L_DISCOUNT) AS AVG_DISC,\r\n\tCOUNT(*) AS COUNT_ORDER\r\nFROM\r\n\tLINEITEM\r\nWHERE\r\n\tL_SHIPDATE <='1998-12-01' - INTERVAL '90' DAY\r\nGROUP BY\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS\r\nORDER BY\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS;\r\n";
                    break;
                case 2:
                    textBox2.Text =
                        "SELECT\r\n\tS_ACCTBAL,\r\n\tS_NAME,\r\n\tN_NAME,\r\n\tP_PARTKEY,\r\n\tP_MFGR,\r\n\tS_ADDRESS,\r\n\tS_PHONE,\r\n\tS_COMMENT\r\nFROM\r\n\tPART,\r\n\tSUPPLIER,\r\n\tPARTSUPP,\r\n\tNATION,\r\n\tREGION\r\nWHERE\r\n\tP_PARTKEY = PS_PARTKEY\r\n\tAND S_SUPPKEY = PS_SUPPKEY\r\n\tAND P_SIZE = 48\r\n\tAND P_TYPE LIKE '%NICKEL'\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_REGIONKEY = R_REGIONKEY\r\n\tAND R_NAME = 'AMERICA'\r\n\tAND PS_SUPPLYCOST = (\r\n\t\tSELECT\r\n\t\t\tMIN(PS_SUPPLYCOST)\r\n\t\tFROM\r\n\t\t\tPARTSUPP,\r\n\t\t\tSUPPLIER,\r\n\t\t\tNATION,\r\n\t\t\tREGION\r\n\t\tWHERE\r\n\t\t\tP_PARTKEY = PS_PARTKEY\r\n\t\t\tAND S_SUPPKEY = PS_SUPPKEY\r\n\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\tAND N_REGIONKEY = R_REGIONKEY\r\n\t\t\tAND R_NAME = 'AMERICA'\r\n\t)\r\nORDER BY\r\n\tS_ACCTBAL DESC,\r\n\tN_NAME,\r\n\tS_NAME,\r\n\tP_PARTKEY;\r\n";
                    break;
                case 3:
                    textBox2.Text =
                        "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = 'HOUSEHOLD'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < '1995-03-31'\r\n\tAND L_SHIPDATE > '1995-03-31'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
                    break;
                case 4:
                    textBox2.Text =
                        "SELECT\r\n\tO_ORDERPRIORITY,\r\n\tCOUNT(*) AS ORDER_COUNT\r\nFROM\r\n\tORDERS\r\nWHERE\r\n\tO_ORDERDATE >= '1996-02-01'\r\n\tAND O_ORDERDATE < '1996-02-01' + INTERVAL '3' MONTH\r\n\tAND EXISTS (\r\n\t\tSELECT\r\n\t\t\t*\r\n\t\tFROM\r\n\t\t\tLINEITEM\r\n\t\tWHERE\r\n\t\t\tL_ORDERKEY = O_ORDERKEY\r\n\t\t\tAND L_COMMITDATE < L_RECEIPTDATE\r\n\t)\r\nGROUP BY\r\n\tO_ORDERPRIORITY\r\nORDER BY\r\n\tO_ORDERPRIORITY;\r\n";
                    break;
                case 5:
                    textBox2.Text =
                        "SELECT\r\n\tN_NAME,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM,\r\n\tSUPPLIER,\r\n\tNATION,\r\n\tREGION\r\nWHERE\r\n\tC_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND L_SUPPKEY = S_SUPPKEY\r\n\tAND C_NATIONKEY = S_NATIONKEY\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_REGIONKEY = R_REGIONKEY\r\n\tAND R_NAME = 'MIDDLE EAST'\r\n\tAND O_ORDERDATE >= '1995-01-01'\r\n\tAND O_ORDERDATE < '1995-01-01' + INTERVAL '1' YEAR\r\nGROUP BY\r\n\tN_NAME\r\nORDER BY\r\n\tREVENUE DESC;\r\n";
                    break;
                case 6:
                    textBox2.Text =
                        "SELECT\r\n\tSUM(L_EXTENDEDPRICE * L_DISCOUNT) AS REVENUE\r\nFROM\r\n\tLINEITEM\r\nWHERE\r\n\tL_SHIPDATE >= '1997-01-01'\r\n\tAND L_SHIPDATE < '1997-01-01' + INTERVAL '1' YEAR\r\n\tAND L_DISCOUNT BETWEEN 0.07 - 0.01 AND 0.07 + 0.01\r\n\tAND L_QUANTITY < 24;\r\n";
                    break;
                case 7:
                    textBox2.Text =
                        "SELECT\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR,\r\n\tSUM(VOLUME) AS REVENUE\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tN1.N_NAME AS SUPP_NATION,\r\n\t\t\tN2.N_NAME AS CUST_NATION,\r\n\t\t\tEXTRACT(YEAR FROM L_SHIPDATE) AS L_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) AS VOLUME\r\n\t\tFROM\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tORDERS,\r\n\t\t\tCUSTOMER,\r\n\t\t\tNATION N1,\r\n\t\t\tNATION N2\r\n\t\tWHERE\r\n\t\t\tS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND O_ORDERKEY = L_ORDERKEY\r\n\t\t\tAND C_CUSTKEY = O_CUSTKEY\r\n\t\t\tAND S_NATIONKEY = N1.N_NATIONKEY\r\n\t\t\tAND C_NATIONKEY = N2.N_NATIONKEY\r\n\t\t\tAND (\r\n\t\t\t\t(N1.N_NAME = 'IRAQ' AND N2.N_NAME = 'ALGERIA')\r\n\t\t\t\tOR (N1.N_NAME = 'ALGERIA' AND N2.N_NAME = 'IRAQ')\r\n\t\t\t)\r\n\t\t\tAND L_SHIPDATE BETWEEN '1995-01-01' AND '1996-12-31'\r\n\t) AS SHIPPING\r\nGROUP BY\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR\r\nORDER BY\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR;\r\n";
                    break;
                case 8:
                    textBox2.Text =
                        "SELECT\r\n\tO_YEAR,\r\n\tSUM(CASE\r\n\t\tWHEN NATION = 'IRAN' THEN VOLUME\r\n\t\tELSE 0\r\n\tEND) / SUM(VOLUME) AS MKT_SHARE\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tEXTRACT(YEAR FROM O_ORDERDATE) AS O_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) AS VOLUME,\r\n\t\t\tN2.N_NAME AS NATION\r\n\t\tFROM\r\n\t\t\tPART,\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tORDERS,\r\n\t\t\tCUSTOMER,\r\n\t\t\tNATION N1,\r\n\t\t\tNATION N2,\r\n\t\t\tREGION\r\n\t\tWHERE\r\n\t\t\tP_PARTKEY = L_PARTKEY\r\n\t\t\tAND S_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND L_ORDERKEY = O_ORDERKEY\r\n\t\t\tAND O_CUSTKEY = C_CUSTKEY\r\n\t\t\tAND C_NATIONKEY = N1.N_NATIONKEY\r\n\t\t\tAND N1.N_REGIONKEY = R_REGIONKEY\r\n\t\t\tAND R_NAME = 'MIDDLE EAST'\r\n\t\t\tAND S_NATIONKEY = N2.N_NATIONKEY\r\n\t\t\tAND O_ORDERDATE BETWEEN '1995-01-01' AND '1996-12-31'\r\n\t\t\tAND P_TYPE = 'STANDARD BRUSHED BRASS'\r\n\t) AS ALL_NATIONS\r\nGROUP BY\r\n\tO_YEAR\r\nORDER BY\r\n\tO_YEAR;\r\n";
                    break;
                case 9:
                    textBox2.Text =
                        "SELECT\r\n\tNATION,\r\n\tO_YEAR,\r\n\tSUM(AMOUNT) AS SUM_PROFIT\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tN_NAME AS NATION,\r\n\t\t\tEXTRACT(YEAR FROM O_ORDERDATE) AS O_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) - PS_SUPPLYCOST * L_QUANTITY AS AMOUNT\r\n\t\tFROM\r\n\t\t\tPART,\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tPARTSUPP,\r\n\t\t\tORDERS,\r\n\t\t\tNATION\r\n\t\tWHERE\r\n\t\t\tS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND PS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND PS_PARTKEY = L_PARTKEY\r\n\t\t\tAND P_PARTKEY = L_PARTKEY\r\n\t\t\tAND O_ORDERKEY = L_ORDERKEY\r\n\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\tAND P_NAME LIKE '%SNOW%'\r\n\t) AS PROFIT\r\nGROUP BY\r\n\tNATION,\r\n\tO_YEAR\r\nORDER BY\r\n\tNATION,\r\n\tO_YEAR DESC;\r\n";
                    break;
                case 10:
                    textBox2.Text =
                        "SELECT\r\n\tC_CUSTKEY,\r\n\tC_NAME,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tC_ACCTBAL,\r\n\tN_NAME,\r\n\tC_ADDRESS,\r\n\tC_PHONE,\r\n\tC_COMMENT\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM,\r\n\tNATION\r\nWHERE\r\n\tC_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE >= '1994-04-01'\r\n\tAND O_ORDERDATE < '1994-04-01' + INTERVAL '3' MONTH\r\n\tAND L_RETURNFLAG = 'R'\r\n\tAND C_NATIONKEY = N_NATIONKEY\r\nGROUP BY\r\n\tC_CUSTKEY,\r\n\tC_NAME,\r\n\tC_ACCTBAL,\r\n\tC_PHONE,\r\n\tN_NAME,\r\n\tC_ADDRESS,\r\n\tC_COMMENT\r\nORDER BY\r\n\tREVENUE DESC;\r\n";
                    break;
                case 11:
                    textBox2.Text =
                        "SELECT\r\n\tPS_PARTKEY,\r\n\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) AS VALUE\r\nFROM\r\n\tPARTSUPP,\r\n\tSUPPLIER,\r\n\tNATION\r\nWHERE\r\n\tPS_SUPPKEY = S_SUPPKEY\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_NAME = 'ALGERIA'\r\nGROUP BY\r\n\tPS_PARTKEY HAVING\r\n\t\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) > (\r\n\t\t\tSELECT\r\n\t\t\t\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) * 0.0001000000\r\n\t\t\tFROM\r\n\t\t\t\tPARTSUPP,\r\n\t\t\t\tSUPPLIER,\r\n\t\t\t\tNATION\r\n\t\t\tWHERE\r\n\t\t\t\tPS_SUPPKEY = S_SUPPKEY\r\n\t\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\t\tAND N_NAME = 'ALGERIA'\r\n\t\t)\r\nORDER BY\r\n\tVALUE DESC;\r\n";
                    break;
                case 12:
                    textBox2.Text =
                        "SELECT\r\n\tL_SHIPMODE,\r\n\tSUM(CASE\r\n\t\tWHEN O_ORDERPRIORITY = '1-URGENT'\r\n\t\t\tOR O_ORDERPRIORITY = '2-HIGH'\r\n\t\t\tTHEN 1\r\n\t\tELSE 0\r\n\tEND) AS HIGH_LINE_COUNT,\r\n\tSUM(CASE\r\n\t\tWHEN O_ORDERPRIORITY <> '1-URGENT'\r\n\t\t\tAND O_ORDERPRIORITY <> '2-HIGH'\r\n\t\t\tTHEN 1\r\n\t\tELSE 0\r\n\tEND) AS LOW_LINE_COUNT\r\nFROM\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tO_ORDERKEY = L_ORDERKEY\r\n\tAND L_SHIPMODE IN ('AIR', 'SHIP')\r\n\tAND L_COMMITDATE < L_RECEIPTDATE\r\n\tAND L_SHIPDATE < L_COMMITDATE\r\n\tAND L_RECEIPTDATE >= '1994-01-01'\r\n\tAND L_RECEIPTDATE < '1994-01-01' + INTERVAL '1' YEAR\r\nGROUP BY\r\n\tL_SHIPMODE\r\nORDER BY\r\n\tL_SHIPMODE;\r\n";
                    break;
                case 13:
                    textBox2.Text =
                        "SELECT\r\n\tC_COUNT,\r\n\tCOUNT(*) AS CUSTDIST\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tC_CUSTKEY,\r\n\t\t\tCOUNT(O_ORDERKEY) AS C_COUNT\r\n\t\tFROM\r\n\t\t\tCUSTOMER LEFT OUTER JOIN ORDERS ON\r\n\t\t\t\tC_CUSTKEY = O_CUSTKEY\r\n\t\t\t\tAND O_COMMENT NOT LIKE '%SPECIAL%REQUESTS%'\r\n\t\tGROUP BY\r\n\t\t\tC_CUSTKEY\r\n\t) AS C_ORDERS\r\nGROUP BY\r\n\tC_COUNT\r\nORDER BY\r\n\tCUSTDIST DESC,\r\n\tC_COUNT DESC;\r\n";
                    break;
                case 14:
                    textBox2.Text =
                        "SELECT\r\n\t100.00 * SUM(CASE\r\n\t\tWHEN P_TYPE LIKE 'PROMO%'\r\n\t\t\tTHEN L_EXTENDEDPRICE * (1 - L_DISCOUNT)\r\n\t\tELSE 0\r\n\tEND) / SUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS PROMO_REVENUE\r\nFROM\r\n\tLINEITEM,\r\n\tPART\r\nWHERE\r\n\tL_PARTKEY = P_PARTKEY\r\n\tAND L_SHIPDATE >= '1995-01-01'\r\n\tAND L_SHIPDATE < '1995-01-01' + INTERVAL '1' MONTH;\r\n";
                    break;
                default:
                    textBox2.Text =
                        "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tSUM(C_MKTSEGMENT * (1 - L_DISCOUNT)) AS REVENUE2,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = \'HOUSEHOLD\'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < \'1995-03-31\'\r\n\tAND L_SHIPDATE  > \'1995-03-31\'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
                    break;
            }
            if (Convert.ToInt16(comboBox1.Text) < 14)
            {
                comboBox2.Text = (Convert.ToInt16(comboBox2.Text) + 1).ToString();
            }
            else
            {
                comboBox2.Text = "0";
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
            GetTree();
            using (FileStream fs = new FileStream("db_result.xml", FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer dbSerializer = new XmlSerializer(typeof(DataBaseStructure));
                dbSerializer.Serialize(fs, _dbName);
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text;
        }


        private TableStructure GetCorrectTable(string listenerTable, DataBaseStructure db)
        {
            TableStructure outTable = new TableStructure();
            foreach (TableStructure table in db.Tables)
            {
                if (table.Name == listenerTable)
                {
                    outTable = table;
                    break;
                }
            }

            return outTable;
        }

        private void DefaultOutput()
        {

            output = "\r\n========TableNames============\r\n";
            foreach (string tableName in listener.TableNames)
            {
                output += tableName + "\r\n";
            }
            output += "\r\n========ColumnNames===========\r\n";
            List<string> col = listener.ColumnNames;
            col = col.Distinct().ToList();
            col.Sort();
            foreach (string columnName in col)
            {
                output += columnName + "\r\n";
            }
            output += "\r\n========ExprColumnNames========\r\n";
            List<string> exprCol = listener.ExprColumnNames;
            exprCol = exprCol.Distinct().ToList();
            exprCol.Sort();
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

        private void GetQuerryTreesScreens(string path, int start, int end)
        {
            for (int i = start; i < end+1; i++)
            {
                string pt = path + i.ToString() + ".bmp";
                comboBox1.Text = i.ToString();
                button3.PerformClick();
                button1.PerformClick();
                pictureBox1.Image.Save(pt, ImageFormat.Bmp);
            }
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

        private List<AsStructure> GetCorrectAsStructure(List<AsStructure> asStructures, string tableName)
        {
            List<AsStructure> outList = new List<AsStructure>();
            foreach (var asStructure in asStructures)
            {
                if (asStructure.Table == tableName && asStructure.IsSelectPart)
                {
                    outList.Add(asStructure);
                }
            }
            return outList;
        }

    }
}
