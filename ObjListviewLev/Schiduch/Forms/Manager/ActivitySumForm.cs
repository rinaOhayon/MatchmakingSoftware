using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schiduch.Forms.Manager
{
    public partial class ActivitySumForm : Form
    {
        public ActivitySumForm()
        {
            InitializeComponent();
        }

        private void ActivitySumForm_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(Application.StartupPath + "\\index.html");
        }
    }
}
