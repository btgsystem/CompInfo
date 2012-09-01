namespace CITreport
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.corpusBox = new System.Windows.Forms.ComboBox();
            this.kabinet = new System.Windows.Forms.TextBox();
            this.inventory = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(407, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "СТАРТ!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Корпус";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Кабинет";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Инвентарный";
            // 
            // corpusBox
            // 
            this.corpusBox.FormattingEnabled = true;
            this.corpusBox.Items.AddRange(new object[] {
            "Главного учебно-административного корпуса",
            "Факультет зоотехнологии и менеджмента",
            "Факультета защиты растений",
            "Факультет ветеринарной медицины",
            "Экономического факультета",
            "Факультета энергетики и эликтрификации",
            "Факультет механизации сельского хозяйства",
            "Факультета водохозяйственного строительства и мелиорации",
            "Факультет заочного обучения",
            "Учебно–лабораторный корпус",
            "НИИ «Биотехнологии и сертификации пищевой продукции»"});
            this.corpusBox.Location = new System.Drawing.Point(103, 10);
            this.corpusBox.Name = "corpusBox";
            this.corpusBox.Size = new System.Drawing.Size(286, 21);
            this.corpusBox.TabIndex = 4;
            // 
            // kabinet
            // 
            this.kabinet.Location = new System.Drawing.Point(103, 40);
            this.kabinet.Name = "kabinet";
            this.kabinet.Size = new System.Drawing.Size(63, 20);
            this.kabinet.TabIndex = 5;
            // 
            // inventory
            // 
            this.inventory.Location = new System.Drawing.Point(103, 67);
            this.inventory.Name = "inventory";
            this.inventory.Size = new System.Drawing.Size(184, 20);
            this.inventory.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(236, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(407, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 30);
            this.button2.TabIndex = 8;
            this.button2.Text = "Выход";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 99);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inventory);
            this.Controls.Add(this.kabinet);
            this.Controls.Add(this.corpusBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчет ЦИТ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox corpusBox;
        private System.Windows.Forms.TextBox kabinet;
        private System.Windows.Forms.TextBox inventory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
    }
}

