namespace MySQL_Clear_standart
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_CreateSort = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.btn_CreateJoin = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.btn_SelectQuerry_tab2 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btn_CreateSelect = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btn_Debug = new System.Windows.Forms.Button();
            this.btn_SelectQuerry_tab1 = new System.Windows.Forms.Button();
            this.btn_SaveTree = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_CreateTree = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1128, 557);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "TO DO";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.Location = new System.Drawing.Point(6, 6);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(545, 406);
            this.textBox4.TabIndex = 0;
            this.textBox4.Text = resources.GetString("textBox4.Text");
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_CreateSort);
            this.tabPage2.Controls.Add(this.textBox6);
            this.tabPage2.Controls.Add(this.textBox5);
            this.tabPage2.Controls.Add(this.btn_CreateJoin);
            this.tabPage2.Controls.Add(this.comboBox2);
            this.tabPage2.Controls.Add(this.btn_SelectQuerry_tab2);
            this.tabPage2.Controls.Add(this.textBox3);
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Controls.Add(this.btn_CreateSelect);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1128, 557);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_CreateSort
            // 
            this.btn_CreateSort.Location = new System.Drawing.Point(368, 6);
            this.btn_CreateSort.Name = "btn_CreateSort";
            this.btn_CreateSort.Size = new System.Drawing.Size(75, 71);
            this.btn_CreateSort.TabIndex = 12;
            this.btn_CreateSort.Text = "Составить запросы SORT";
            this.btn_CreateSort.UseVisualStyleBackColor = true;
            this.btn_CreateSort.Click += new System.EventHandler(this.btn_CreateSort_Click);
            // 
            // textBox6
            // 
            this.textBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox6.Location = new System.Drawing.Point(635, 83);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox6.Size = new System.Drawing.Size(204, 468);
            this.textBox6.TabIndex = 11;
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.Location = new System.Drawing.Point(215, 83);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox5.Size = new System.Drawing.Size(204, 468);
            this.textBox5.TabIndex = 10;
            // 
            // btn_CreateJoin
            // 
            this.btn_CreateJoin.Location = new System.Drawing.Point(287, 6);
            this.btn_CreateJoin.Name = "btn_CreateJoin";
            this.btn_CreateJoin.Size = new System.Drawing.Size(75, 71);
            this.btn_CreateJoin.TabIndex = 9;
            this.btn_CreateJoin.Text = "Составить запросы JOIN";
            this.btn_CreateJoin.UseVisualStyleBackColor = true;
            this.btn_CreateJoin.Click += new System.EventHandler(this.btn_CreateJoin_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14"});
            this.comboBox2.Location = new System.Drawing.Point(8, 6);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(33, 21);
            this.comboBox2.TabIndex = 8;
            this.comboBox2.Text = "0";
            // 
            // btn_SelectQuerry_tab2
            // 
            this.btn_SelectQuerry_tab2.Location = new System.Drawing.Point(8, 37);
            this.btn_SelectQuerry_tab2.Name = "btn_SelectQuerry_tab2";
            this.btn_SelectQuerry_tab2.Size = new System.Drawing.Size(93, 40);
            this.btn_SelectQuerry_tab2.TabIndex = 5;
            this.btn_SelectQuerry_tab2.Text = "Выбрать запрос";
            this.btn_SelectQuerry_tab2.UseVisualStyleBackColor = true;
            this.btn_SelectQuerry_tab2.Click += new System.EventHandler(this.btn_SelectQuerry_tab2_Click);
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Location = new System.Drawing.Point(425, 83);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(204, 468);
            this.textBox3.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.Location = new System.Drawing.Point(6, 83);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 3, 50, 3);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(204, 466);
            this.textBox2.TabIndex = 1;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btn_CreateSelect
            // 
            this.btn_CreateSelect.Location = new System.Drawing.Point(206, 6);
            this.btn_CreateSelect.Name = "btn_CreateSelect";
            this.btn_CreateSelect.Size = new System.Drawing.Size(75, 71);
            this.btn_CreateSelect.TabIndex = 0;
            this.btn_CreateSelect.Text = "Составить запросы SELECT";
            this.btn_CreateSelect.UseVisualStyleBackColor = true;
            this.btn_CreateSelect.Click += new System.EventHandler(this.btn_CreateSelect_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1128, 557);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.btn_Debug);
            this.panel1.Controls.Add(this.btn_SelectQuerry_tab1);
            this.panel1.Controls.Add(this.btn_SaveTree);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btn_CreateTree);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1122, 551);
            this.panel1.TabIndex = 0;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoCheck = false;
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(185, 30);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(66, 17);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "NoSubQ";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Запрос:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14"});
            this.comboBox1.Location = new System.Drawing.Point(185, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(67, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "0";
            // 
            // btn_Debug
            // 
            this.btn_Debug.Location = new System.Drawing.Point(86, 49);
            this.btn_Debug.Name = "btn_Debug";
            this.btn_Debug.Size = new System.Drawing.Size(93, 36);
            this.btn_Debug.TabIndex = 5;
            this.btn_Debug.Text = "Отладка";
            this.btn_Debug.UseVisualStyleBackColor = true;
            this.btn_Debug.Click += new System.EventHandler(this.btn_Debug_Click);
            // 
            // btn_SelectQuerry_tab1
            // 
            this.btn_SelectQuerry_tab1.Location = new System.Drawing.Point(5, 3);
            this.btn_SelectQuerry_tab1.Name = "btn_SelectQuerry_tab1";
            this.btn_SelectQuerry_tab1.Size = new System.Drawing.Size(93, 40);
            this.btn_SelectQuerry_tab1.TabIndex = 4;
            this.btn_SelectQuerry_tab1.Text = "Выбрать запрос";
            this.btn_SelectQuerry_tab1.UseVisualStyleBackColor = true;
            this.btn_SelectQuerry_tab1.Click += new System.EventHandler(this.btn_SelectQuerry_tab1_Click);
            // 
            // btn_SaveTree
            // 
            this.btn_SaveTree.Location = new System.Drawing.Point(5, 49);
            this.btn_SaveTree.Name = "btn_SaveTree";
            this.btn_SaveTree.Size = new System.Drawing.Size(75, 36);
            this.btn_SaveTree.TabIndex = 3;
            this.btn_SaveTree.Text = "Сохранить дерево";
            this.btn_SaveTree.UseVisualStyleBackColor = true;
            this.btn_SaveTree.Click += new System.EventHandler(this.btn_SaveTree_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(293, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(175, 159);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // btn_CreateTree
            // 
            this.btn_CreateTree.Location = new System.Drawing.Point(104, 3);
            this.btn_CreateTree.Name = "btn_CreateTree";
            this.btn_CreateTree.Size = new System.Drawing.Size(75, 40);
            this.btn_CreateTree.TabIndex = 1;
            this.btn_CreateTree.Text = "Построить дерево";
            this.btn_CreateTree.UseVisualStyleBackColor = true;
            this.btn_CreateTree.Click += new System.EventHandler(this.btn_CreateTree_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(5, 91);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(283, 413);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1136, 583);
            this.tabControl1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1136, 583);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button btn_SelectQuerry_tab2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btn_CreateSelect;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btn_Debug;
        private System.Windows.Forms.Button btn_SelectQuerry_tab1;
        private System.Windows.Forms.Button btn_SaveTree;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_CreateTree;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btn_CreateJoin;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button btn_CreateSort;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

