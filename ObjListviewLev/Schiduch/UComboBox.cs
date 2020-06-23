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
    public partial class UComboBox : UserControl
    {
        public UComboBox()
        {
            InitializeComponent();
        }

        public int SelectedIndex
        {
            get { return comboBox1.SelectedIndex; }
            set { comboBox1.SelectedIndex = value; }
        }

        public ComboBox.ObjectCollection Items
        {
            get { return comboBox1.Items; }
            set { comboBox1.Items.Add( value); }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
