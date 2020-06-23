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
    public partial class UNumericUpDown : UserControl
    {
        public UNumericUpDown()
        {
            InitializeComponent();
        }
        public int DecimalPlaces
        {
            get { return numericUpDown1.DecimalPlaces; }
            set { numericUpDown1.DecimalPlaces = value; }
        }

        public decimal Value
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }
        public decimal Maximum
        {
            get { return numericUpDown1.Maximum; }
            set { numericUpDown1.Maximum = value; }
        }




    }
}
