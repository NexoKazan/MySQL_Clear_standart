using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using MySQL_Clear_standart.DataBaseSchemeStructure;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ClusterixN.Common.Data.Query.Relation;
using ClusterixN.Common.Utils;
using ClusterixN.Network.Packets;
using MySQL_Clear_standart.Listeners;
using MySQL_Clear_standart.Network;
using MySQL_Clear_standart.Properties;
using MySQL_Clear_standart.Q_Structures;


namespace MySQL_Clear_standart
{
    public partial class Form1 : Form
    {
        #region v1
       
        private SelectStructure[] _selectQuery;
        private List<JoinStructure> _joinQuery;
        private SortStructure _sortQuery;

        private SelectStructure[] _subSelectQuery;
        private List<JoinStructure> _subJoinQuery;
        private SortStructure _subSortQuery;
        
        private bool _toDoTextFlag;
        
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

        #region TAB_2

        private void btn_CreateSelect_Click(object sender, EventArgs e)
        {
            //составление запросов SELECT
            

            textBox3.Clear();
            _selectQuery = MakeSelect(CreateSubDatabase(_dbName, _listener.TableNames.ToArray(), _listener.ColumnNames.ToArray()), _listener);
            for (int i = 0; i < _selectQuery.Length; i++)
            {
                textBox3.Text += "\r\n========" + _selectQuery[i].Name + "=========\r\n";
                textBox3.Text += _selectQuery[i].Output + "\r\n";
            }

            if (_listener.SubQueryListeners.Count != 0)
            {
                textBox3.Text += "\r\n========SUB_Q=========\r\n";
                foreach (var subQlistener in _listener.SubQueryListeners)
                {
                    MakeSubSelect(subQlistener);
                }

                foreach (SelectStructure subSelect in _subSelectQuery)
                {
                    textBox3.Text += "\r\n========" + subSelect.Name + "=========\r\n";
                    textBox3.Text += subSelect.Output + "\r\n";
                }
            }
        }

        private void btn_CreateJoin_Click(object sender, EventArgs e)
        {
            MakeJoin();
            textBox5.Clear();
            foreach (var join in _joinQuery)
            {
                textBox5.Text += "\r\n========" + join.Name + "========\r\n" + join.Output + "\r\n";
            }
            if (_listener.SubQueryListeners.Count != 0)
            {
                textBox5.Text += "\r\n========SUB_Q=========\r\n";
                foreach (var subQlistener in _listener.SubQueryListeners)
                {
                    MakeSubJoin(subQlistener);
                }

                foreach (var join in _subJoinQuery)
                {
                    textBox5.Text += "\r\n========" + join.Name + "========\r\n" + join.Output + "\r\n";
                }
            }
        }

        private void btn_SelectQuerry_tab2_Click(object sender, EventArgs e)
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

            if (Convert.ToInt16(comboBox2.Text) < 14)
            {
                comboBox2.Text = (Convert.ToInt16(comboBox2.Text) + 1).ToString();
            }
            else
            {
                comboBox2.Text = "0";
            }
        }
        
        private void btn_CreateSort_Click(object sender, EventArgs e)
        {
            MakeSort();
            textBox6.Clear();
            textBox6.Text = "\r\n========" + _sortQuery.Name + "========\r\n" + _sortQuery.Output + "\r\n";
        }
        
        private void btn_CreateTest_Click(object sender, EventArgs e)
        {
            MakeSort();

            string dropSyntax = "DROP TABLE {0};\r\n" + Environment.NewLine;
            string createTableSyntax = "CREATE TABLE {0} (\r\n{1} {2} ) ENGINE=MEMORY\r\n\r\n";
            string createIndexSyntax = ",\r\n INDEX {0} ( {2} ) \r\n\r\n";
            string querSyntax = "{0};\r\n";
            var dropBuilder = new StringBuilder();
            var testQuery = new StringBuilder();

            foreach (var select in _selectQuery)
            {
                testQuery.Append("\r\n -- ========" + select.Name + "=========\r\n");
                //testQuery.Append(string.Format(dropSyntax, select.Name));
                testQuery.Append(string.Format(createTableSyntax, select.Name, select.CreateTableColumnNames, string.Format(createIndexSyntax, select.Name + "_index", select.Name, select.IndexColumnName)));
                testQuery.Append(string.Format(querSyntax, select.Output));
                //testQuery.Append(string.Format(createIndexSyntax, select.Name + "_index", select.Name, select.IndexColumnName));
                dropBuilder.Append(string.Format(dropSyntax, select.Name));
            }
            foreach (var join in _joinQuery)
            {
                testQuery.Append("\r\n -- ========" + join.Name + "=========\r\n");
               // testQuery.Append(string.Format(dropSyntax, join.Name));
                testQuery.Append(string.Format(createTableSyntax, join.Name, join.CreateTableColumnNames, 
                    join != _joinQuery.LastOrDefault() ?
                    string.Format(createIndexSyntax, join.Name + "_index", join.Name,
                        join.IndexColumnName) : ""));
                testQuery.Append(string.Format(querSyntax, join.Output));

                dropBuilder.Append(string.Format(dropSyntax, join.Name));
            }
            testQuery.Append("\r\n -- ========" + _sortQuery.Name + "=========\r\n");
            //testQuery.Append(string.Format(dropSyntax, _sortQuery.Name));
            testQuery.Append(string.Format(createTableSyntax, _sortQuery.Name, _sortQuery.CreateTableColumnNames, ""));
            testQuery.Append(string.Format(querSyntax, _sortQuery.Output));
            dropBuilder.Append(string.Format(dropSyntax, _sortQuery.Name));

            testQueryTb.Text = testQuery.ToString();
            testQueryTb.Text += "SELECT * FROM So_1;";
            testQueryTb.Text += dropBuilder.ToString();

            //QueryBuilder qb = new QueryBuilder((new Random()).Next(0, 1000000));
            //var sel1 = qb.AddSelectQuery(qb.CreateSelectQuery(_selectQuery[0].Output, 0));
            //var sel2 = qb.AddSelectQuery(qb.CreateSelectQuery(_selectQuery[1].Output, 0));

            //var j1 = qb.AddJoinQuery
            //(
            //    qb.CreateJoinQuery
            //    (
            //        _joinQuery[0].Output, qb.CreateRelationSchema(
            //            _joinQuery[0].OutTable.Columns.Select(j => new Field() {Name = j.Name, Params = j.Type.Name})
            //                .ToList(),
            //            new List<Index>()
            //                {new Index() {FieldNames = new List<string>() {_joinQuery[0].IndexColumnName}}}), 0,
            //        qb.CreateRelation(sel1, "PS", qb.CreateRelationSchema(_selectQuery[0].OutTable.Columns.Select(j => new Field() {Name = j.Name, Params = j.Type.Name})
            //                .ToList(),
            //            new List<Index>()
            //                {new Index() {FieldNames = new List<string>() {_selectQuery[0].IndexColumnName}}})),
            //        qb.CreateRelation(sel2, "s2", qb.CreateRelationSchema())
            //    )
            //);

            //var clinet = new ClusterixClient("127.0.0.1", 1234);
            //clinet.Send(new XmlQueryPacket() { XmlQuery = qb.GetQuery().ToString() });
        }

        #endregion

       

        #region СлужебныеМетоды

        private TableStructure GetCorrectTable(string _listenerTable, DataBaseStructure db)
        {
            TableStructure outTable = new TableStructure();
            foreach (TableStructure table in db.Tables)
            {
                if (table.Name == _listenerTable)
                {
                    outTable = table;
                    break;
                }
            }

            return outTable;
        }

        

        private void ReSize()
        {
            textBox2.Width = textBox3.Width = textBox5.Width = textBox6.Width = testQueryTb.Width = (tabPage2.Width - 100) / 5;
            textBox3.Location = new Point(textBox2.Location.X + 40 + textBox2.Width, textBox3.Location.Y);
            textBox5.Location = new Point(textBox3.Location.X + 20 + textBox3.Width, textBox3.Location.Y);
            textBox6.Location = new Point(textBox5.Location.X + 20 + textBox3.Width, textBox3.Location.Y);
            testQueryTb.Location = new Point(textBox6.Location.X + 20 + textBox3.Width, textBox3.Location.Y);
        }

        private void MakeJoin()
        {
            MakeSelect(CreateSubDatabase(_dbName, _listener.TableNames.ToArray(), _listener.ColumnNames.ToArray()), _listener);
            _joinQuery = _listener.JoinStructures;
            FillJoins(_joinQuery, _dbName, _selectQuery.ToList());
            GetJoinSequence(_joinQuery, _listener.Depth);
            _joinQuery = SortJoin(_joinQuery, _listener.Depth);
            foreach (var join in _joinQuery)
            {
                join.CreateQuerry();
            }

            CreateScheme(_joinQuery);
        }

        private void MakeSort()
        {
            MakeJoin();
            _sortQuery = new SortStructure("So_1");
            List<OrderByStructure> orderByStructures = _listener.OrderByList;
            List<ColumnStructure> inputColumns;

            if (_joinQuery.Count != 0)
            {
                inputColumns = _joinQuery.LastOrDefault().Columns;
            }
            else
            {
                inputColumns = _selectQuery.LastOrDefault().OutColumn.ToList();
            }

            if (orderByStructures != null)
            foreach (OrderByStructure orderByStructure in orderByStructures)
            {
                orderByStructure.Column =
                    GetCorrectOrderByColumn(inputColumns, orderByStructure.ColumnName);
            }

            _sortQuery.Select = _selectQuery.LastOrDefault();
            _sortQuery.Join = _joinQuery.LastOrDefault();
            _sortQuery.AsSortList = _listener.AsList;
            _sortQuery.GroupByColumnList = _listener.GroupByColumnsNames;
            _sortQuery.OrderByStructures = orderByStructures;
            _sortQuery.CreateQuerry();
            CreateScheme(_sortQuery);

        }

        private void MakeSubSelect(MainListener subQuery_listener)
        {
            _subSelectQuery = new SelectStructure[subQuery_listener.TableNames.Count];
            FindeWhereStructureTable(subQuery_listener.WhereList, _dbName);
            FillAsStructures(_dbName, subQuery_listener.AsList); //переделать _dbName;

            for (int i = 0; i < subQuery_listener.TableNames.Count; i++)
            {
                //_subSelectQuery[i] = new SelectStructure
                //(
                //    "S_" + subQuery_listener.Depth + "_" + i.ToString(), //name(s)
                   
                //    GetCorrectWhereStructure(subQuery_listener.WhereList,
                //        subQuery_listener.TableNames[i]), //WhereStructure(WhereStructure)
                //    GetCorrectAsStructure(subQuery_listener.AsList, subQuery_listener.TableNames[i]) //asStructure(asStructure)
                //);
            }

            foreach (SelectStructure select in _subSelectQuery)
            {
                select.CreateQuerry();
            }
            CreateScheme(_subSelectQuery);
        }

        private void MakeSubJoin(MainListener subQuery_listener)
        {
            
            MakeSubSelect(subQuery_listener);
            _subJoinQuery = subQuery_listener.JoinStructures;
           
            List<SelectStructure> tmpSelects = new List<SelectStructure>();
            tmpSelects.AddRange(_subSelectQuery);
            bool tmpSelect = true;
            foreach (SelectStructure select in _selectQuery)
            {
                foreach (var subSelect in _subSelectQuery)
                {
                    if (select.TableName == subSelect.TableName)
                    {
                        tmpSelect = false;
                        break;
                    }
                }

                if (tmpSelect)
                {
                    tmpSelects.Add(select);
                }
            }

            FillJoins(_subJoinQuery, _dbName, tmpSelects);
            GetJoinSequence(_subJoinQuery, subQuery_listener.Depth);
            _subJoinQuery = SortJoin(_subJoinQuery, subQuery_listener.Depth);
            foreach (var join in _subJoinQuery)
            {
                join.CreateQuerry();
            }

            CreateScheme(_subJoinQuery);
        }

        private void CreateScheme(SelectStructure[] selectQuery)
        {
            List<TableStructure> outTablesList = new List<TableStructure>();
            foreach (var selectStructure in selectQuery)
            {
                outTablesList.Add(selectStructure.OutTable);
            }

            DataBaseStructure outDB = new DataBaseStructure("SELECT_OUT_DB", outTablesList.ToArray());
            MatchColumns(_dbName, outDB);
            outDB.Name = _dbName.Name + "_Select";
            outDB.Types = _dbName.Types;
            using (FileStream fs = new FileStream(@"res\SelectOutDB.xml", FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer dbSerializer = new XmlSerializer(typeof(DataBaseStructure));
                dbSerializer.Serialize(fs, outDB);
            }
        }

        private void CreateScheme(List<JoinStructure> joinQuerry)
        {
            List<TableStructure> outTablesList = new List<TableStructure>();
            foreach (var joinStructure in joinQuerry)
            {
                outTablesList.Add(joinStructure.OutTable);
            }

            DataBaseStructure outDB = new DataBaseStructure("JOIN_OUT_DB", outTablesList.ToArray());
            MatchColumns(_dbName, outDB);
            outDB.Name = _dbName.Name + "_Join";
            outDB.Types = _dbName.Types;
            using (FileStream fs = new FileStream(@"res\JoinOutDB.xml", FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer dbSerializer = new XmlSerializer(typeof(DataBaseStructure));
                dbSerializer.Serialize(fs, outDB);
            }
        }

        private void CreateScheme(SortStructure sortQuerry)
        {
            List<TableStructure> outTables = new List<TableStructure>();
            outTables.Add(sortQuerry.OutTable);
            DataBaseStructure outDB = new DataBaseStructure("SORT_OUT_DB", outTables.ToArray());
            MatchColumns(_dbName, outDB);
            outDB.Name = _dbName.Name + "_Sort";
            outDB.Types = _dbName.Types;
            using (FileStream fs = new FileStream(@"res\SortOutDB.xml", FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer dbSerializer = new XmlSerializer(typeof(DataBaseStructure));
                dbSerializer.Serialize(fs, outDB);
            }
        }

        private void GetQuerryTreesScreens(string path, int start, int end)
        {
            for (int i = start; i < end + 1; i++)
            {
                string pt = path + i.ToString() + ".bmp";
                comboBox_tab1_QueryNumber.Text = i.ToString();
                btn_tab1_SelectQuerry.PerformClick();
                btn_tab1_CreateTree.PerformClick();
                pictureBox_tab1_Tree.Image.Save(pt, ImageFormat.Bmp);
            }
        }

        private void GetJoinSequence(List<JoinStructure> joinStructures, int joinDepth)
        {
            if (joinStructures.Count != 0)
            {
                List<Pares> j_list = new List<Pares>();

                #region Magic

                foreach (JoinStructure joinStructure in joinStructures)
                {
                    if (joinStructure.LeftSelect.Name[2] == joinStructure.RightSelect.Name[2])
                    {
                        Pares pr = new Pares(joinStructure.LeftSelect.Name, joinStructure.RightSelect.Name);
                        j_list.Add(pr);
                    }
                }

                bool razriv = true;
                List<List<string>> containers = new List<List<string>>();
                List<string> cont = new List<string>()
                {
                    j_list[0].Left,
                };
                for (int j = 0; j < cont.Count;)
                {
                    razriv = true;
                    for (int i = 0; i < j_list.Count; i++)
                    {
                        if (cont[j] == j_list[i].Left)
                        {
                            cont.Add(j_list[i].Right);
                            j_list[i].IsForDelete = true;
                            razriv = false;
                        }

                        if (cont[j] == j_list[i].Right)
                        {
                            cont.Add(j_list[i].Left);
                            j_list[i].IsForDelete = true;
                            razriv = false;
                        }
                    }

                    foreach (Pares pares in j_list)
                    {
                        bool haveLeft = false;
                        bool haveRight = false;
                        foreach (string s in cont)
                        {
                            if (pares.Left == s)
                            {
                                haveLeft = true;
                            }

                            if (pares.Right == s)
                            {
                                haveRight = true;
                            }
                        }

                        if (haveRight && haveLeft)
                        {
                            pares.IsForDelete = true;
                        }
                    }

                    List<Pares> tmp = new List<Pares>();
                    for (int i = 0; i < j_list.Count; i++)
                    {
                        if (!j_list[i].IsForDelete)
                        {
                            tmp.Add(j_list[i]);
                        }
                    }

                    j_list = tmp;
                    j++;
                    if (razriv && j_list.Count > 0 && j == cont.Count)
                    {
                        containers.Add(cont);
                        cont = new List<string>();
                        cont.Add(j_list[0].Left);
                        j = 0;
                    }
                }

                if (j_list.Count == 0)
                {
                    containers.Add(cont);
                }

                #endregion //создан контейнер селектов(s1,s3,s2,s5)

                //в containers поледовательности конвейров джойн, даже если они разделяются.
                //алгоритм помог придумать выпускник КАИ Алексей Казнаецев.
                foreach (List<string> container in containers)
                {
                    int i = 0;
                    List<JoinStructure> j_sequence = new List<JoinStructure>();
                    for (; i < container.Count;)
                    {
                        if (i == 0)
                        {
                            JoinStructure tmp = FindeJoin(container[0], container[1], joinStructures);
                            tmp.IsFirst = true;
                            j_sequence.Add(tmp);
                            i = 2;
                        }
                        else
                        {
                            int stopper = j_sequence.Count;
                            for (int j = 0; j < i; j++)
                            {
                                JoinStructure tmp = FindeJoin(container[j], container[i], joinStructures);
                                if (tmp.Name != "ERROR")
                                {
                                    j_sequence.Add(tmp);
                                }

                                if (j_sequence.Count == stopper + 1)
                                {
                                    break;
                                }
                            }

                            i++;
                        }

                        for (int j = 1; j < j_sequence.Count; j++)
                        {
                            j_sequence[j].LeftJoin = j_sequence[j - 1];
                            for (int k = 0; k < j; k++)
                            {
                                if (j_sequence[j].RightSelect == j_sequence[k].LeftSelect ||
                                    j_sequence[j].RightSelect == j_sequence[k].RightSelect)
                                {
                                    j_sequence[j].Switched = true;
                                }
                            }
                        }
                    }

                    for (int j = 0; j < j_sequence.Count; j++)
                    {
                        j_sequence[j].Name = "J_" + joinDepth + "_" + j;
                    }
                }
            }
        }

        private List<JoinStructure> SortJoin(List<JoinStructure> joinStructures, int joinDepth)
        {
            List<JoinStructure> tmp = new List<JoinStructure>();
            int notJoinedCount = 0;
            for (int i = 0; i < joinStructures.Count; i++)
            {
                string s = "J_" + joinDepth + "_"  + i;
                foreach (JoinStructure joinStructure in joinStructures)
                {
                    if (joinStructure.Name == s)
                    {
                        tmp.Add(joinStructure);
                    }
                }
            }

            foreach (JoinStructure joinStructure in joinStructures)
            {
                if (joinStructure.Name == null)
                {
                    joinStructure.Name = "J_" + joinDepth + "_"  + tmp.Count.ToString();
                    joinStructure.LeftJoin = tmp.Last();
                    joinStructure.LeftSelect = null;
                    joinStructure.RightSelect = null;
                    tmp.Add(joinStructure);
                }
            }
            return tmp;
        }

        private JoinStructure FindeJoin(string j1, string j2, List<JoinStructure> joinList)
        {
            //не должно быть ошибок
            JoinStructure output = new JoinStructure("ERROR", "ERROR", "ERROR");
            output.Name = "ERROR";
            foreach (JoinStructure structure in joinList)
            {
                if ((structure.LeftSelect.Name == j1 && structure.RightSelect.Name == j2) ||
                    (structure.LeftSelect.Name == j2 && structure.RightSelect.Name == j1))
                {
                    output = structure;
                    break;
                }
            }

            return output;
        }

        private ColumnStructure GetCorrectOrderByColumn(List<ColumnStructure> columns, string columnName)
        {
            ColumnStructure correctColumn = new ColumnStructure();
            foreach (ColumnStructure column in columns)
            {
                if (column.Name == columnName || column.OldName == columnName)
                {
                    correctColumn = column;
                }
            }
            return correctColumn;
        }

        private List<ColumnStructure> GetClearColumns(List<string> allColumns, List<string> removeColumns,
            TableStructure table)
        {
            List<string> inList = allColumns;
            List<ColumnStructure> outList = new List<ColumnStructure>();
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
                foreach (var t in table.Columns)
                {
                    if (allColumn == t.Name)
                    {
                        outList.Add(t);
                        break;
                    }
                }
            }

            foreach (ColumnStructure column in outList)
            {
                foreach (string selectColumn in _listener.SelectColumns)
                {
                    if (column.Name == selectColumn)
                    {
                        column.IsForSelect = true;
                    }
                }
            }

            return outList;
        }

        

        private List<WhereStructure> GetCorrectWhereStructure(List<WhereStructure> whereList, string tableName)
        {
            List<WhereStructure> outList = new List<WhereStructure>();
            foreach (var ws in whereList)
            {
                if (ws.Table == tableName)
                {
                    outList.Add(ws);
                }
            }

            return outList;
        }

        //Сопоставление типов столбцов с типами из базы данных.
        private void SetID(DataBaseStructure inDB)
        {
            foreach (TableStructure dbTable in inDB.Tables)
            {
                foreach (ColumnStructure dbColumn in dbTable.Columns)
                {
                    foreach (S_Type dbType in inDB.Types)
                    {
                        if (dbColumn.TypeID == dbType.ID)
                        {
                            dbColumn.Type = dbType;
                            dbColumn.Size = dbType.Size;
                            break;
                        }
                    }
                }
            }
        }

        //Присвоение типов полученной схемы после запроса(саб т) известным столбцам.
        private void MatchColumns(DataBaseStructure mainDataBase, DataBaseStructure subDataBase)
        {
            foreach (TableStructure subTable in subDataBase.Tables)
            {
                foreach (ColumnStructure subColumn in subTable.Columns)
                {
                    foreach (TableStructure mainTable in mainDataBase.Tables)
                    {
                        foreach (ColumnStructure mainColumn in mainTable.Columns)
                        {
                            if (subColumn.Name == mainColumn.Name)
                            {
                                subColumn.Type = mainColumn.Type;
                                break;
                            }
                        }
                    }
                }
            }
        }

        //"Филл Джонс"
        private void FillJoins(List<JoinStructure> joinList, DataBaseStructure dataBase,
            List<SelectStructure> selectQueries)
        {
            int i = 1;
            foreach (JoinStructure join in joinList)
            {
                i++;
                foreach (TableStructure table in dataBase.Tables)
                {
                    foreach (ColumnStructure column in table.Columns)
                    {
                        if (join.LeftColumnString == column.Name)
                        {
                            join.LeftColumn = column;
                            foreach (SelectStructure select in selectQueries)
                            {
                                if (select.TableName == table.Name)
                                {
                                    join.LeftSelect = select;
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }

                foreach (TableStructure table in dataBase.Tables)
                {
                    foreach (ColumnStructure column in table.Columns)
                    {
                        if (join.RightColumnString == column.Name)
                        {
                            join.RightColumn = column;
                            foreach (SelectStructure select in selectQueries)
                            {
                                if (select.TableName == table.Name)
                                {
                                    join.RightSelect = select;
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        private string GetCreateTableColumnString(TableStructure table)
        {
            string output = null;
            foreach (ColumnStructure column in table.Columns)
            {
                if (column.Type != null)
                {
                    output += "\r\n" + column.Name + " " + column.Type.Name + ",";
                }
                else
                {
                    output += "\r\n" + column.Name + " *INTEGER"+",";
                }
            }
            return output;
        }

        private string GetIndexName(SelectStructure select)
        {
            string output = null;
            string multiIndex = null;
            return output;
        } 

        private string GetIndexName(JoinStructure join)
        {
            string output = null;
            //string multiIndex = null;
            //foreach (ColumnStructure column in table.Columns)
            //{
            //    switch (column.IsPrimary)
            //    {
            //        case 1 : 
            //            output = column.Name;
            //            break;
            //        case 2:
            //            multiIndex += column.Name + ", ";
            //            break;
            //        default: break;
            //    }
            //}

            //if (multiIndex != null)
            //{
            //    output = multiIndex;
            //}
            return output;
        }

        private string GetIndexName(SortStructure sort)
        {
            string output = null;
            string multiIndex = null;
            return output;
        }
        #endregion
        
        #endregion

        #region v2

        #region Глобальные переменные
        
        private DataBaseStructure _dbName;
        // объявляются переменный для построения дерева
        private string _output = " ";
        private string _inputString;
        private ICharStream _inputStream;
        private ITokenSource _mySqlLexer;
        private CommonTokenStream _commonTokenStream;
        private MySqlParser _mySqlParser;
        private IParseTree _tree;
        private CommonNode _treeNodeDrawable;
        private TreeVisitor _vTree;
        private ParseTreeWalker _walker;
        private MainListener _listener;
       
        bool pictureSize = false;
        

        #endregion

        #region СлужебныеМетоды
        
        private void GetTree()
        {
            FillScheme(); //странный баг.
            textBox2.Text = textBox_tab1_Query.Text;
            _inputString = textBox_tab1_Query.Text;
            _inputStream = new AntlrInputStream(_inputString);
            _mySqlLexer = new MySqlLexer(_inputStream);
            _commonTokenStream = new CommonTokenStream(_mySqlLexer);
            _mySqlParser = new MySqlParser(_commonTokenStream);
            _mySqlParser.BuildParseTree = true;
            _tree = _mySqlParser.root();
            _treeNodeDrawable = new CommonNode(_tree);
            _vTree = new TreeVisitor(_treeNodeDrawable);
            _walker = new ParseTreeWalker();
            _listener = new MainListener(0);
            _listener.Vocabulary = _mySqlParser.Vocabulary;
            _walker.Walk(_listener, _tree);

        }
        
        private void FillScheme()
        {
            using (FileStream dbCreateFileStream = new FileStream("res//db.xml", FileMode.Open, FileAccess.ReadWrite)
            ) // Заполнение БД из XML
            {
                XmlSerializer dbCreateSerializer = new XmlSerializer(typeof(DataBaseStructure));
                _dbName = (DataBaseStructure) dbCreateSerializer.Deserialize(dbCreateFileStream);
            }

            SetID(_dbName);
        }

        private void SetIsForSelectFlags(DataBaseStructure dataBase, List<string> isForSelectColumnNames)
        {
            foreach (TableStructure table in dataBase.Tables)
            {
                foreach (ColumnStructure column in table.Columns)
                {
                    foreach (string columnName in isForSelectColumnNames)
                    {
                        if (columnName == column.Name)
                        {
                            column.IsForSelect = true;
                        }
                    }
                }
            }
        }

        private void FillAsStructures(DataBaseStructure dataBase, List<AsStructure> asStructures)
        {
            List<TableStructure> asTables = new List<TableStructure>();
            foreach (AsStructure asStructure in asStructures)
            {
                asTables = new List<TableStructure>();
                ColumnStructure rightColumn = new ColumnStructure(asStructure.GetAsRightName);
                rightColumn.Size = -1;
                foreach (var table in dataBase.Tables)
                {
                    foreach (ColumnStructure column in table.Columns)
                    {
                        foreach (string columnName in asStructure.ColumnNames)
                        {
                            if (column.Name == columnName)
                            {
                                if (column.Size > rightColumn.Size)
                                {
                                    rightColumn.Size = column.Size;
                                    rightColumn.Type = column.Type;
                                    rightColumn.TypeID = column.TypeID;
                                }
                                asTables.Add(table);
                            }
                        }
                    }
                }

                asStructure.AsRightColumn = rightColumn;
                
                asTables = asTables.Distinct().ToList();
                if (asTables.Count == 1)
                {
                    asStructure.IsSelectPart = true;
                    asStructure.Tables = asTables;
                    asStructure.AsRightColumn.OldName = asStructure.AsRightColumn.Name;
                    asStructure.AsRightColumn.Name =
                        asStructure.Tables[0].ShortName + asStructure.AsRightColumn.Name;
                    asStructure.AsRightColumn.IsRenamed = true;
                }
                else
                {
                    asStructure.IsSelectPart = false;
                    asStructure.Tables = asTables;
                }
            }
        }
        
        private string GetQuery(int queryNumber)
        {
            string query;
            if (!checkBox_tab1_DisableHeavyQuerry.Checked)
            {
                switch (queryNumber)
                {

                    case 1:
                        query =
                            "SELECT\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS,\r\n\tSUM(L_QUANTITY) AS SUM_QTY,\r\n\tSUM(L_EXTENDEDPRICE) AS SUM_BASE_PRICE,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS SUM_DISC_PRICE,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT) * (1 + L_TAX)) AS SUM_CHARGE,\r\n\tAVG(L_QUANTITY) AS AVG_QTY,\r\n\tAVG(L_EXTENDEDPRICE) AS AVG_PRICE,\r\n\tAVG(L_DISCOUNT) AS AVG_DISC,\r\n\tCOUNT(*) AS COUNT_ORDER\r\nFROM\r\n\tLINEITEM\r\nWHERE\r\n\tL_SHIPDATE <='1998-12-01' - INTERVAL '90' DAY\r\nGROUP BY\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS\r\nORDER BY\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS;\r\n";
                        break;
                    case 2:
                        query =
                            "SELECT\r\n\tS_ACCTBAL,\r\n\tS_NAME,\r\n\tN_NAME,\r\n\tP_PARTKEY,\r\n\tP_MFGR,\r\n\tS_ADDRESS,\r\n\tS_PHONE,\r\n\tS_COMMENT\r\nFROM\r\n\tPART,\r\n\tSUPPLIER,\r\n\tPARTSUPP,\r\n\tNATION,\r\n\tREGION\r\nWHERE\r\n\tP_PARTKEY = PS_PARTKEY\r\n\tAND S_SUPPKEY = PS_SUPPKEY\r\n\tAND P_SIZE = 48\r\n\tAND P_TYPE LIKE '%NICKEL'\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_REGIONKEY = R_REGIONKEY\r\n\tAND R_NAME = 'AMERICA'\r\n\tAND PS_SUPPLYCOST = (\r\n\t\tSELECT\r\n\t\t\tMIN(PS_SUPPLYCOST)\r\n\t\tFROM\r\n\t\t\tPARTSUPP,\r\n\t\t\tSUPPLIER,\r\n\t\t\tNATION,\r\n\t\t\tREGION\r\n\t\tWHERE\r\n\t\t\tP_PARTKEY = PS_PARTKEY\r\n\t\t\tAND S_SUPPKEY = PS_SUPPKEY\r\n\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\tAND N_REGIONKEY = R_REGIONKEY\r\n\t\t\tAND R_NAME = 'AMERICA'\r\n\t)\r\nORDER BY\r\n\tS_ACCTBAL DESC,\r\n\tN_NAME,\r\n\tS_NAME,\r\n\tP_PARTKEY;\r\n";
                        break;
                    case 3:
                        query =
                            "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = 'HOUSEHOLD'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < '1995-03-31'\r\n\tAND L_SHIPDATE > '1995-03-31'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
                        break;
                    case 4:
                        query =
                            "SELECT\r\n\tO_ORDERPRIORITY,\r\n\tCOUNT(*) AS ORDER_COUNT\r\nFROM\r\n\tORDERS\r\nWHERE\r\n\tO_ORDERDATE >= '1996-02-01'\r\n\tAND O_ORDERDATE < '1996-02-01' + INTERVAL '3' MONTH\r\n\tAND EXISTS (\r\n\t\tSELECT\r\n\t\t\t*\r\n\t\tFROM\r\n\t\t\tLINEITEM\r\n\t\tWHERE\r\n\t\t\tL_ORDERKEY = O_ORDERKEY\r\n\t\t\tAND L_COMMITDATE < L_RECEIPTDATE\r\n\t)\r\nGROUP BY\r\n\tO_ORDERPRIORITY\r\nORDER BY\r\n\tO_ORDERPRIORITY;\r\n";
                        break;
                    case 5:
                        query =
                            "SELECT\r\n\tN_NAME,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM,\r\n\tSUPPLIER,\r\n\tNATION,\r\n\tREGION\r\nWHERE\r\n\tC_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND L_SUPPKEY = S_SUPPKEY\r\n\tAND C_NATIONKEY = S_NATIONKEY\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_REGIONKEY = R_REGIONKEY\r\n\tAND R_NAME = 'MIDDLE EAST'\r\n\tAND O_ORDERDATE >= '1995-01-01'\r\n\tAND O_ORDERDATE < '1995-01-01' + INTERVAL '1' YEAR\r\nGROUP BY\r\n\tN_NAME\r\nORDER BY\r\n\tREVENUE DESC;\r\n";
                        break;
                    case 6:
                        query =
                            "SELECT\r\n\tSUM(L_EXTENDEDPRICE * L_DISCOUNT) AS REVENUE\r\nFROM\r\n\tLINEITEM\r\nWHERE\r\n\tL_SHIPDATE >= '1997-01-01'\r\n\tAND L_SHIPDATE < '1997-01-01' + INTERVAL '1' YEAR\r\n\tAND L_DISCOUNT BETWEEN 0.07 - 0.01 AND 0.07 + 0.01\r\n\tAND L_QUANTITY < 24;\r\n";
                        break;
                    case 7:
                        query =
                            "SELECT\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR,\r\n\tSUM(VOLUME) AS REVENUE\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tN1.N_NAME AS SUPP_NATION,\r\n\t\t\tN2.N_NAME AS CUST_NATION,\r\n\t\t\tEXTRACT(YEAR FROM L_SHIPDATE) AS L_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) AS VOLUME\r\n\t\tFROM\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tORDERS,\r\n\t\t\tCUSTOMER,\r\n\t\t\tNATION N1,\r\n\t\t\tNATION N2\r\n\t\tWHERE\r\n\t\t\tS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND O_ORDERKEY = L_ORDERKEY\r\n\t\t\tAND C_CUSTKEY = O_CUSTKEY\r\n\t\t\tAND S_NATIONKEY = N1.N_NATIONKEY\r\n\t\t\tAND C_NATIONKEY = N2.N_NATIONKEY\r\n\t\t\tAND (\r\n\t\t\t\t(N1.N_NAME = 'IRAQ' AND N2.N_NAME = 'ALGERIA')\r\n\t\t\t\tOR (N1.N_NAME = 'ALGERIA' AND N2.N_NAME = 'IRAQ')\r\n\t\t\t)\r\n\t\t\tAND L_SHIPDATE BETWEEN '1995-01-01' AND '1996-12-31'\r\n\t) AS SHIPPING\r\nGROUP BY\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR\r\nORDER BY\r\n\tSUPP_NATION,\r\n\tCUST_NATION,\r\n\tL_YEAR;\r\n";
                        break;
                    case 8:
                        query =
                            "SELECT\r\n\tO_YEAR,\r\n\tSUM(CASE\r\n\t\tWHEN NATION = 'IRAN' THEN VOLUME\r\n\t\tELSE 0\r\n\tEND) / SUM(VOLUME) AS MKT_SHARE\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tEXTRACT(YEAR FROM O_ORDERDATE) AS O_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) AS VOLUME,\r\n\t\t\tN2.N_NAME AS NATION\r\n\t\tFROM\r\n\t\t\tPART,\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tORDERS,\r\n\t\t\tCUSTOMER,\r\n\t\t\tNATION N1,\r\n\t\t\tNATION N2,\r\n\t\t\tREGION\r\n\t\tWHERE\r\n\t\t\tP_PARTKEY = L_PARTKEY\r\n\t\t\tAND S_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND L_ORDERKEY = O_ORDERKEY\r\n\t\t\tAND O_CUSTKEY = C_CUSTKEY\r\n\t\t\tAND C_NATIONKEY = N1.N_NATIONKEY\r\n\t\t\tAND N1.N_REGIONKEY = R_REGIONKEY\r\n\t\t\tAND R_NAME = 'MIDDLE EAST'\r\n\t\t\tAND S_NATIONKEY = N2.N_NATIONKEY\r\n\t\t\tAND O_ORDERDATE BETWEEN '1995-01-01' AND '1996-12-31'\r\n\t\t\tAND P_TYPE = 'STANDARD BRUSHED BRASS'\r\n\t) AS ALL_NATIONS\r\nGROUP BY\r\n\tO_YEAR\r\nORDER BY\r\n\tO_YEAR;\r\n";
                        break;
                    case 9:
                        query =
                            "SELECT\r\n\tNATION,\r\n\tO_YEAR,\r\n\tSUM(AMOUNT) AS SUM_PROFIT\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tN_NAME AS NATION,\r\n\t\t\tEXTRACT(YEAR FROM O_ORDERDATE) AS O_YEAR,\r\n\t\t\tL_EXTENDEDPRICE * (1 - L_DISCOUNT) - PS_SUPPLYCOST * L_QUANTITY AS AMOUNT\r\n\t\tFROM\r\n\t\t\tPART,\r\n\t\t\tSUPPLIER,\r\n\t\t\tLINEITEM,\r\n\t\t\tPARTSUPP,\r\n\t\t\tORDERS,\r\n\t\t\tNATION\r\n\t\tWHERE\r\n\t\t\tS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND PS_SUPPKEY = L_SUPPKEY\r\n\t\t\tAND PS_PARTKEY = L_PARTKEY\r\n\t\t\tAND P_PARTKEY = L_PARTKEY\r\n\t\t\tAND O_ORDERKEY = L_ORDERKEY\r\n\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\tAND P_NAME LIKE '%SNOW%'\r\n\t) AS PROFIT\r\nGROUP BY\r\n\tNATION,\r\n\tO_YEAR\r\nORDER BY\r\n\tNATION,\r\n\tO_YEAR DESC;\r\n";
                        break;
                    case 10:
                        query =
                            "SELECT\r\n\tC_CUSTKEY,\r\n\tC_NAME,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tC_ACCTBAL,\r\n\tN_NAME,\r\n\tC_ADDRESS,\r\n\tC_PHONE,\r\n\tC_COMMENT\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM,\r\n\tNATION\r\nWHERE\r\n\tC_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE >= '1994-04-01'\r\n\tAND O_ORDERDATE < '1994-04-01' + INTERVAL '3' MONTH\r\n\tAND L_RETURNFLAG = 'R'\r\n\tAND C_NATIONKEY = N_NATIONKEY\r\nGROUP BY\r\n\tC_CUSTKEY,\r\n\tC_NAME,\r\n\tC_ACCTBAL,\r\n\tC_PHONE,\r\n\tN_NAME,\r\n\tC_ADDRESS,\r\n\tC_COMMENT\r\nORDER BY\r\n\tREVENUE DESC;\r\n";
                        break;
                    case 11:
                        query =
                            "SELECT\r\n\tPS_PARTKEY,\r\n\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) AS VALUE\r\nFROM\r\n\tPARTSUPP,\r\n\tSUPPLIER,\r\n\tNATION\r\nWHERE\r\n\tPS_SUPPKEY = S_SUPPKEY\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_NAME = 'ALGERIA'\r\nGROUP BY\r\n\tPS_PARTKEY HAVING\r\n\t\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) > (\r\n\t\t\tSELECT\r\n\t\t\t\tSUM(PS_SUPPLYCOST * PS_AVAILQTY) * 0.0001000000\r\n\t\t\tFROM\r\n\t\t\t\tPARTSUPP,\r\n\t\t\t\tSUPPLIER,\r\n\t\t\t\tNATION\r\n\t\t\tWHERE\r\n\t\t\t\tPS_SUPPKEY = S_SUPPKEY\r\n\t\t\t\tAND S_NATIONKEY = N_NATIONKEY\r\n\t\t\t\tAND N_NAME = 'ALGERIA'\r\n\t\t)\r\nORDER BY\r\n\tVALUE DESC;\r\n";
                        break;
                    case 12:
                        query =
                            "SELECT\r\n\tL_SHIPMODE,\r\n\tSUM(CASE\r\n\t\tWHEN O_ORDERPRIORITY = '1-URGENT'\r\n\t\t\tOR O_ORDERPRIORITY = '2-HIGH'\r\n\t\t\tTHEN 1\r\n\t\tELSE 0\r\n\tEND) AS HIGH_LINE_COUNT,\r\n\tSUM(CASE\r\n\t\tWHEN O_ORDERPRIORITY <> '1-URGENT'\r\n\t\t\tAND O_ORDERPRIORITY <> '2-HIGH'\r\n\t\t\tTHEN 1\r\n\t\tELSE 0\r\n\tEND) AS LOW_LINE_COUNT\r\nFROM\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tO_ORDERKEY = L_ORDERKEY\r\n\tAND L_SHIPMODE IN ('AIR', 'SHIP')\r\n\tAND L_COMMITDATE < L_RECEIPTDATE\r\n\tAND L_SHIPDATE < L_COMMITDATE\r\n\tAND L_RECEIPTDATE >= '1994-01-01'\r\n\tAND L_RECEIPTDATE < '1994-01-01' + INTERVAL '1' YEAR\r\nGROUP BY\r\n\tL_SHIPMODE\r\nORDER BY\r\n\tL_SHIPMODE;\r\n";
                        break;
                    case 13:
                        query =
                            "SELECT\r\n\tC_COUNT,\r\n\tCOUNT(*) AS CUSTDIST\r\nFROM\r\n\t(\r\n\t\tSELECT\r\n\t\t\tC_CUSTKEY,\r\n\t\t\tCOUNT(O_ORDERKEY) AS C_COUNT\r\n\t\tFROM\r\n\t\t\tCUSTOMER LEFT OUTER JOIN ORDERS ON\r\n\t\t\t\tC_CUSTKEY = O_CUSTKEY\r\n\t\t\t\tAND O_COMMENT NOT LIKE '%SPECIAL%REQUESTS%'\r\n\t\tGROUP BY\r\n\t\t\tC_CUSTKEY\r\n\t) AS C_ORDERS\r\nGROUP BY\r\n\tC_COUNT\r\nORDER BY\r\n\tCUSTDIST DESC,\r\n\tC_COUNT DESC;\r\n";
                        break;
                    case 14:
                        query =
                            "SELECT\r\n\t100.00 * SUM(CASE\r\n\t\tWHEN P_TYPE LIKE 'PROMO%'\r\n\t\t\tTHEN L_EXTENDEDPRICE * (1 - L_DISCOUNT)\r\n\t\tELSE 0\r\n\tEND) / SUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS PROMO_REVENUE\r\nFROM\r\n\tLINEITEM,\r\n\tPART\r\nWHERE\r\n\tL_PARTKEY = P_PARTKEY\r\n\tAND L_SHIPDATE >= '1995-01-01'\r\n\tAND L_SHIPDATE < '1995-01-01' + INTERVAL '1' MONTH;\r\n";
                        break;
                    default:
                        query =
                            "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tSUM(C_MKTSEGMENT * (1 - L_DISCOUNT)) AS REVENUE2,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = \'HOUSEHOLD\'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < \'1995-03-31\'\r\n\tAND L_SHIPDATE  > \'1995-03-31\'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
                        break;
                }
            }
            else
            {
                switch (queryNumber)
                {
                    case 1:
                        query =
                            "SELECT\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS,\r\n\tSUM(L_QUANTITY) AS SUM_QTY,\r\n\tSUM(L_EXTENDEDPRICE) AS SUM_BASE_PRICE,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS SUM_DISC_PRICE,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT) * (1 + L_TAX)) AS SUM_CHARGE,\r\n\tAVG(L_QUANTITY) AS AVG_QTY,\r\n\tAVG(L_EXTENDEDPRICE) AS AVG_PRICE,\r\n\tAVG(L_DISCOUNT) AS AVG_DISC,\r\n\tCOUNT(*) AS COUNT_ORDER\r\nFROM\r\n\tLINEITEM\r\nWHERE\r\n\tL_SHIPDATE <='1998-12-01' - INTERVAL '90' DAY\r\nGROUP BY\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS\r\nORDER BY\r\n\tL_RETURNFLAG,\r\n\tL_LINESTATUS;\r\n";
                        comboBox_tab1_QueryNumber.Text = "3";
                        break;
                    case 3:
                        query =
                            "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = 'HOUSEHOLD'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < '1995-03-31'\r\n\tAND L_SHIPDATE > '1995-03-31'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
                        comboBox_tab1_QueryNumber.Text = "5";
                        break;
                    case 5:
                        query =
                            "SELECT\r\n\tN_NAME,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM,\r\n\tSUPPLIER,\r\n\tNATION,\r\n\tREGION\r\nWHERE\r\n\tC_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND L_SUPPKEY = S_SUPPKEY\r\n\tAND C_NATIONKEY = S_NATIONKEY\r\n\tAND S_NATIONKEY = N_NATIONKEY\r\n\tAND N_REGIONKEY = R_REGIONKEY\r\n\tAND R_NAME = 'MIDDLE EAST'\r\n\tAND O_ORDERDATE >= '1995-01-01'\r\n\tAND O_ORDERDATE < '1995-01-01' + INTERVAL '1' YEAR\r\nGROUP BY\r\n\tN_NAME\r\nORDER BY\r\n\tREVENUE DESC;\r\n";
                        comboBox_tab1_QueryNumber.Text = "6";
                        break;
                    case 6:
                        query =
                            "SELECT\r\n\tSUM(L_EXTENDEDPRICE * L_DISCOUNT) AS REVENUE\r\nFROM\r\n\tLINEITEM\r\nWHERE\r\n\tL_SHIPDATE >= '1997-01-01'\r\n\tAND L_SHIPDATE < '1997-01-01' + INTERVAL '1' YEAR\r\n\tAND L_DISCOUNT BETWEEN 0.07 - 0.01 AND 0.07 + 0.01\r\n\tAND L_QUANTITY < 24;\r\n";
                        comboBox_tab1_QueryNumber.Text = "10";
                        break;
                    case 10:
                        query =
                            "SELECT\r\n\tC_CUSTKEY,\r\n\tC_NAME,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tC_ACCTBAL,\r\n\tN_NAME,\r\n\tC_ADDRESS,\r\n\tC_PHONE,\r\n\tC_COMMENT\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM,\r\n\tNATION\r\nWHERE\r\n\tC_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE >= '1994-04-01'\r\n\tAND O_ORDERDATE < '1994-04-01' + INTERVAL '3' MONTH\r\n\tAND L_RETURNFLAG = 'R'\r\n\tAND C_NATIONKEY = N_NATIONKEY\r\nGROUP BY\r\n\tC_CUSTKEY,\r\n\tC_NAME,\r\n\tC_ACCTBAL,\r\n\tC_PHONE,\r\n\tN_NAME,\r\n\tC_ADDRESS,\r\n\tC_COMMENT\r\nORDER BY\r\n\tREVENUE DESC;\r\n";
                        comboBox_tab1_QueryNumber.Text = "12";
                        break;
                    case 12:
                        query =
                            "SELECT\r\n\tL_SHIPMODE,\r\n\tSUM(CASE\r\n\t\tWHEN O_ORDERPRIORITY = '1-URGENT'\r\n\t\t\tOR O_ORDERPRIORITY = '2-HIGH'\r\n\t\t\tTHEN 1\r\n\t\tELSE 0\r\n\tEND) AS HIGH_LINE_COUNT,\r\n\tSUM(CASE\r\n\t\tWHEN O_ORDERPRIORITY <> '1-URGENT'\r\n\t\t\tAND O_ORDERPRIORITY <> '2-HIGH'\r\n\t\t\tTHEN 1\r\n\t\tELSE 0\r\n\tEND) AS LOW_LINE_COUNT\r\nFROM\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tO_ORDERKEY = L_ORDERKEY\r\n\tAND L_SHIPMODE IN ('AIR', 'SHIP')\r\n\tAND L_COMMITDATE < L_RECEIPTDATE\r\n\tAND L_SHIPDATE < L_COMMITDATE\r\n\tAND L_RECEIPTDATE >= '1994-01-01'\r\n\tAND L_RECEIPTDATE < '1994-01-01' + INTERVAL '1' YEAR\r\nGROUP BY\r\n\tL_SHIPMODE\r\nORDER BY\r\n\tL_SHIPMODE;\r\n";
                        comboBox_tab1_QueryNumber.Text = "14";
                        break;
                    case 14:
                        query =
                            "SELECT\r\n\t100.00 * SUM(CASE\r\n\t\tWHEN P_TYPE LIKE 'PROMO%'\r\n\t\t\tTHEN L_EXTENDEDPRICE * (1 - L_DISCOUNT)\r\n\t\tELSE 0\r\n\tEND) / SUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS PROMO_REVENUE\r\nFROM\r\n\tLINEITEM,\r\n\tPART\r\nWHERE\r\n\tL_PARTKEY = P_PARTKEY\r\n\tAND L_SHIPDATE >= '1995-01-01'\r\n\tAND L_SHIPDATE < '1995-01-01' + INTERVAL '1' MONTH;\r\n";
                        break;
                    default:
                        query =
                            "SELECT\r\n\tL_ORDERKEY,\r\n\tSUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,\r\n\tSUM(C_MKTSEGMENT * (1 - L_DISCOUNT)) AS REVENUE2,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nFROM\r\n\tCUSTOMER,\r\n\tORDERS,\r\n\tLINEITEM\r\nWHERE\r\n\tC_MKTSEGMENT = \'HOUSEHOLD\'\r\n\tAND C_CUSTKEY = O_CUSTKEY\r\n\tAND L_ORDERKEY = O_ORDERKEY\r\n\tAND O_ORDERDATE < \'1995-03-31\'\r\n\tAND L_SHIPDATE  > \'1995-03-31\'\r\nGROUP BY\r\n\tL_ORDERKEY,\r\n\tO_ORDERDATE,\r\n\tO_SHIPPRIORITY\r\nORDER BY\r\n\tREVENUE DESC,\r\n\tO_ORDERDATE;\r\n";
                        comboBox_tab1_QueryNumber.Text = "1";
                        break;
                }
            }

            return query;
        }

        private string ShowDataBase(DataBaseStructure dataBase)
        {
            string output = "";
            foreach (TableStructure table in dataBase.Tables)
            {
                output += table.Name + Environment.NewLine;
                foreach (ColumnStructure column in table.Columns)
                {
                    output += "\t" + column.Name +"\t\t" + column.Type.Name + "\t" + column.Size + Environment.NewLine;
                }
            }
            return output;
        }

        private DataBaseStructure CreateSubDatabase(DataBaseStructure fullDataBase, string[] TableNames,
            string[] ColumnNames)
        {
            DataBaseStructure subDataBase;
            List<TableStructure> tmpTables = new List<TableStructure>();
            List<ColumnStructure> tmpColumns;

            foreach (TableStructure fullTable in fullDataBase.Tables)
            {
                foreach (string subTableName in TableNames)
                {
                    if (subTableName == fullTable.Name)
                    {
                        tmpTables.Add(fullTable);
                        break;
                    }
                }
            }

            foreach (TableStructure tmpTable in tmpTables)
            {
                tmpColumns = new List<ColumnStructure>();
                foreach (ColumnStructure tmpTableColumn in tmpTable.Columns)
                {
                    foreach (string subColumnName in ColumnNames)
                    {
                        if (subColumnName == tmpTableColumn.Name)
                        {
                            tmpColumns.Add(tmpTableColumn);
                            break;
                            
                        }
                    }
                }

                tmpTable.Columns = tmpColumns.ToArray();
            }

            subDataBase = new DataBaseStructure("sub_" + fullDataBase.Name ,tmpTables.ToArray(), fullDataBase.Types);
            return subDataBase;
        }

        private List<AsStructure> GetCorrectAsStructures(List<AsStructure> asStructures, TableStructure table)
        {
            List<AsStructure> outList = new List<AsStructure>();
            foreach (var asStructure in asStructures)
            {
                if (asStructure.Tables.Count==1)
                {
                    if (asStructure.Tables[0] == table && asStructure.IsSelectPart)
                    {
                        outList.Add(asStructure);
                    }
                }
            }

            return outList;
        }
       
        private void FindeWhereStructureTable(List<WhereStructure> whereList, DataBaseStructure dataBase)
        {
            foreach (WhereStructure ws in whereList)
            {
                foreach (TableStructure dataBaseTable in dataBase.Tables)
                {
                    foreach (ColumnStructure column in dataBaseTable.Columns)
                    {
                        if (column.Name == ws.getLeftColumn)
                        {
                            ws.Table = dataBaseTable.Name;
                        }
                    }

                }
            }
        }

        private SelectStructure[] MakeSelect(DataBaseStructure dataBase, MainListener listener)
        {
            GetTree();
            SetIsForSelectFlags(dataBase, listener.SelectColumnNames);
            SelectStructure[] selectQueries = new SelectStructure[dataBase.Tables.Length];

            FindeWhereStructureTable(listener.WhereList, dataBase);
            FillAsStructures(dataBase, listener.AsList);
            
            for (var i = 0; i < dataBase.Tables.Length; i++)
            {
                selectQueries[i] = new SelectStructure("S_" + listener.Depth + "_" + i, dataBase.Tables[i],
                    GetCorrectWhereStructure(listener.WhereList, dataBase.Tables[i].Name),
                    GetCorrectAsStructures(listener.AsList, dataBase.Tables[i])
                    );
            }
            foreach (SelectStructure select in selectQueries)
            {
                select.CreateQuerry();
            }
            CreateScheme(selectQueries);
            return selectQueries;
        }
        


        #endregion

        #region FormMethods
        
        public Form1()
        {
            InitializeComponent();
            GetTree();
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
            _toDoTextFlag = true;
            StreamReader sr = new StreamReader(@"res\ToDo.txt", System.Text.Encoding.Default);
            textBox4.Text = sr.ReadToEnd();
            sr.Close();
            _toDoTextFlag = false;
            using (FileStream fs = new FileStream(@"res\db_result.xml", FileMode.Create, FileAccess.ReadWrite))
            {
                XmlSerializer dbSerializer = new XmlSerializer(typeof(DataBaseStructure));
                dbSerializer.Serialize(fs, _dbName);
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox_tab1_Query.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox_tab1_Query.Text = textBox2.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!_toDoTextFlag)
            {
                using (StreamWriter sw = new StreamWriter(@"res\ToDo.txt", false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(textBox4.Text);
                    sw.Close();
                }
            }
        }

        private void allow_SelectAl(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
            }
        }

        
        #endregion
        
        #region TAB_1

        private void btn_CreateTree_Click(object sender, EventArgs e)
        {
            //Кнопка для картинки(построить дерево)
            GetTree();
            richTextBox_tab1_Query.Visible = false;
            Image image = _vTree.Draw();
            pictureBox_tab1_Tree.Image = image;

        }

        private void btn_SaveTree_Click(object sender, EventArgs e)
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

                pictureBox_tab1_Tree.Image.Save(saveFileDialog1.FileName, format);
            }
        }

        private void btn_SelectQuerry_tab1_Click(object sender, EventArgs e)
        {
            //выбрать запрос
            pictureBox_tab1_Tree.Visible = true;
            richTextBox_tab1_Query.Visible = false;
            textBox_tab1_Query.Width = 283;
            richTextBox_tab1_Query.Text = "ЗАПРОС " + comboBox_tab1_QueryNumber.Text + Environment.NewLine;
            //запросы TPC-H (убраны Date)
            if (!checkBox_tab1_DisableHeavyQuerry.Checked)
            {
                textBox_tab1_Query.Text = GetQuery(Convert.ToInt16(comboBox_tab1_QueryNumber.Text));
                if (Convert.ToInt16(comboBox_tab1_QueryNumber.Text) < 14)
                {
                    comboBox_tab1_QueryNumber.Text = (Convert.ToInt16(comboBox_tab1_QueryNumber.Text) + 1).ToString();
                }
                else
                {
                    comboBox_tab1_QueryNumber.Text = "0";
                }
            }
            else
            {
                textBox_tab1_Query.Text = GetQuery(Convert.ToInt16(comboBox_tab1_QueryNumber.Text));
                
                if (Convert.ToInt16(comboBox_tab1_QueryNumber.Text) == 14)
                {
                    comboBox_tab1_QueryNumber.Text = "0";
                }
            }

        }

        private void btn_Debug_Click(object sender, EventArgs e)
        {
            //GetQuerryTreesScreens(@"D:\!Studing\Скриншоты деревьев\Originals\",12,14);
            //отладка
            pictureBox_tab1_Tree.Visible = false;
            richTextBox_tab1_Query.Visible = true;
            richTextBox_tab1_Query.Text += textBox_tab1_Query.Text;
            textBox_tab1_Query.Width = Width - richTextBox_tab1_Query.Width - 50;
            richTextBox_tab1_Query.Height = textBox_tab1_Query.Height;
            richTextBox_tab1_Query.Location = new Point(textBox_tab1_Query.Location.X + textBox_tab1_Query.Width + 10,
                textBox_tab1_Query.Location.Y);
            GetTree();
            _output = "\r\n========Return================\r\n";

            List<string> columns = _listener.ColumnNames.Distinct().ToList();
            List<string> tables = _listener.TableNames.Distinct().ToList();
            _output = "\r\n========TABLES================\r\n";
            foreach (var table in tables)
            {
                _output += table + Environment.NewLine;
            }
            _output += "\r\n========COLUMNS================\r\n";
            foreach (var column in columns)
            {
                _output += column + Environment.NewLine;
            }
            DataBaseStructure subDbTest = CreateSubDatabase(_dbName, tables.ToArray(), columns.ToArray());
            _output = ShowDataBase(subDbTest);
            textBox_tab1_Query.Text = _output;
        }
        
        #endregion

        #endregion
    }
}