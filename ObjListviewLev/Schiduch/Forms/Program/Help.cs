using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schiduch.Forms
{
    public partial class Help : Form
    {
        public Help(string error,string solutions)
        {
            InitializeComponent();
            txterror.Text = error;
            txtsolution.Text = solutions;
        }

        private void Help_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Help_Load(object sender, EventArgs e)
        {

        }
    }
}
