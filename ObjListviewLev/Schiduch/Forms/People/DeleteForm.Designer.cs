namespace Schiduch.Forms
{
    partial class DeleteForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radOther = new System.Windows.Forms.RadioButton();
            this.radNoWant = new System.Windows.Forms.RadioButton();
            this.radWedding = new System.Windows.Forms.RadioButton();
            this.radmistake = new System.Windows.Forms.RadioButton();
            this.radTwice = new System.Windows.Forms.RadioButton();
            this.btnstatuschg = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 13.84615F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(112, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(267, 35);
            this.label1.TabIndex = 5;
            this.label1.Text = "נא לבחור סיבת מחיקה:\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(123, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 31);
            this.label3.TabIndex = 25;
            this.label3.Text = "מזל טוב - התארסו";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(261, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 31);
            this.label6.TabIndex = 26;
            this.label6.Text = "אחר";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(95, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(224, 31);
            this.label5.TabIndex = 27;
            this.label5.Text = "רוצים לצאת מהמאגר";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(171, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 31);
            this.label4.TabIndex = 28;
            this.label4.Text = "נרשמו בטעות";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(236, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 31);
            this.label2.TabIndex = 29;
            this.label2.Text = "כפולים";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("FbShiri Light", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(25, 192);
            this.textBox1.Name = "textBox1";
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBox1.Size = new System.Drawing.Size(215, 34);
            this.textBox1.TabIndex = 24;
            // 
            // radOther
            // 
            this.radOther.AutoSize = true;
            this.radOther.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radOther.Location = new System.Drawing.Point(325, 201);
            this.radOther.Name = "radOther";
            this.radOther.Size = new System.Drawing.Size(17, 16);
            this.radOther.TabIndex = 19;
            this.radOther.TabStop = true;
            this.radOther.Tag = "אחר ";
            this.radOther.UseVisualStyleBackColor = true;
            this.radOther.CheckedChanged += new System.EventHandler(this.radOther_CheckedChanged);
            // 
            // radNoWant
            // 
            this.radNoWant.AutoSize = true;
            this.radNoWant.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radNoWant.Location = new System.Drawing.Point(325, 159);
            this.radNoWant.Name = "radNoWant";
            this.radNoWant.Size = new System.Drawing.Size(17, 16);
            this.radNoWant.TabIndex = 20;
            this.radNoWant.TabStop = true;
            this.radNoWant.Tag = "רוצים לצאת מהמאגר";
            this.radNoWant.UseVisualStyleBackColor = true;
            this.radNoWant.CheckedChanged += new System.EventHandler(this.radOther_CheckedChanged);
            // 
            // radWedding
            // 
            this.radWedding.AutoSize = true;
            this.radWedding.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radWedding.Location = new System.Drawing.Point(326, 39);
            this.radWedding.Name = "radWedding";
            this.radWedding.Size = new System.Drawing.Size(17, 16);
            this.radWedding.TabIndex = 21;
            this.radWedding.TabStop = true;
            this.radWedding.Tag = "מזל טוב - התארסו";
            this.radWedding.UseVisualStyleBackColor = true;
            this.radWedding.CheckedChanged += new System.EventHandler(this.radOther_CheckedChanged);
            // 
            // radmistake
            // 
            this.radmistake.AutoSize = true;
            this.radmistake.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radmistake.Location = new System.Drawing.Point(325, 119);
            this.radmistake.Name = "radmistake";
            this.radmistake.Size = new System.Drawing.Size(17, 16);
            this.radmistake.TabIndex = 22;
            this.radmistake.TabStop = true;
            this.radmistake.Tag = "נרשמו בטעות";
            this.radmistake.UseVisualStyleBackColor = true;
            this.radmistake.CheckedChanged += new System.EventHandler(this.radOther_CheckedChanged);
            // 
            // radTwice
            // 
            this.radTwice.AutoSize = true;
            this.radTwice.Font = new System.Drawing.Font("Segoe UI", 12.18462F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTwice.Location = new System.Drawing.Point(325, 79);
            this.radTwice.Name = "radTwice";
            this.radTwice.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radTwice.Size = new System.Drawing.Size(17, 16);
            this.radTwice.TabIndex = 23;
            this.radTwice.TabStop = true;
            this.radTwice.Tag = "כפולים";
            this.radTwice.UseVisualStyleBackColor = true;
            this.radTwice.CheckedChanged += new System.EventHandler(this.radOther_CheckedChanged);
            // 
            // btnstatuschg
            // 
            this.btnstatuschg.BackColor = System.Drawing.Color.White;
            this.btnstatuschg.Font = new System.Drawing.Font("Segoe UI Semibold", 12.18462F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnstatuschg.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnstatuschg.ImageIndex = 2;
            this.btnstatuschg.Location = new System.Drawing.Point(169, 352);
            this.btnstatuschg.Margin = new System.Windows.Forms.Padding(4);
            this.btnstatuschg.Name = "btnstatuschg";
            this.btnstatuschg.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.btnstatuschg.Size = new System.Drawing.Size(119, 68);
            this.btnstatuschg.TabIndex = 31;
            this.btnstatuschg.Text = "מחק";
            this.btnstatuschg.UseVisualStyleBackColor = false;
            this.btnstatuschg.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radOther);
            this.groupBox1.Controls.Add(this.radNoWant);
            this.groupBox1.Controls.Add(this.radWedding);
            this.groupBox1.Controls.Add(this.radmistake);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.radTwice);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(57, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 255);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            // 
            // DeleteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(481, 433);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnstatuschg);
            this.Controls.Add(this.label1);
            this.Name = "DeleteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "סיבת מחיקה מהמאגר";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radOther;
        private System.Windows.Forms.RadioButton radNoWant;
        private System.Windows.Forms.RadioButton radWedding;
        private System.Windows.Forms.RadioButton radmistake;
        private System.Windows.Forms.RadioButton radTwice;
        public System.Windows.Forms.Button btnstatuschg;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}