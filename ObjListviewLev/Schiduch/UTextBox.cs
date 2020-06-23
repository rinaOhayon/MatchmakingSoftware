using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schiduch
{
    public partial class UTextBox : UserControl
    {
        public UTextBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        

        public char PasswordChar
        {
            get { return textBox1.PasswordChar; }
            set { textBox1.PasswordChar = value; }
        }

    }
}
