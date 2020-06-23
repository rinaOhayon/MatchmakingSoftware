using Schiduch.Classes.Program;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schiduch
{
    public partial class Report : Form
    {
        string url = Application.StartupPath + "/Rep.Xinfo";
        public Report()
        {
            InitializeComponent();
          
        }
        public Report(string surl)
        {
            InitializeComponent();
            this.url = surl;
        }

        private void Report_Load(object sender, EventArgs e)
        {
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin) btnprintme.Visible = true;
            webBrowser1.Navigate(url);
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void btnprintme_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }
    }
}
