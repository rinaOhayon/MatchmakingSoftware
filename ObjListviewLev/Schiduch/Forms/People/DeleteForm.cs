using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schiduch.Forms
{
    public partial class DeleteForm : Form
    {
        public DeleteForm()
        {
            InitializeComponent();
        }
        public string ReasonDelete { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (radOther.Checked == true && textBox1.Text.Length <= 0)
            {
                MessageBox.Show("נא למלא סיבה");
                ActiveControl = textBox1;
                textBox1.Focus();
                return;
            }
            ReasonDelete += textBox1.Text;
            Close();
        }

        private void radOther_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as RadioButton) == radOther)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
                textBox1.Text = "";
            }
            ReasonDelete = (sender as RadioButton).Tag.ToString();
        }
    }
}
