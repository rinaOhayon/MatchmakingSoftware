using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using System.Text;

using System.Windows.Forms;
using Schiduch.Classes.Program;

namespace Schiduch
{

    public partial class Login : Form
    {
        private static bool Working = true;

        public MainForm frm;
        public Login()
        {
            InitializeComponent();

        }


        public void LoadMainForm(object x) { frm = new MainForm(); Working = false; }
        
        public  bool MakeTheLogin(string username, string password, Forms.SplachScreen splach = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                User temp = General.GetSavedUser();
                if (temp != null)
                {
                    username = temp.UserName;
                    password = temp.Password;
                }
                else return false;
            }
            MainForm mainfrm;

            User myuser;
            myuser = User.GetUser(username, password);
            if (myuser == null)
                return false;
            else
            {
                GLOBALVARS.MyUser = myuser;
                switch (myuser.Control)
                {
                    case User.TypeControl.Lock:
                        MessageBox.Show("נחסמת על ידי המערכת", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Log.AddAction(Log.ActionType.LoginBlocked);
                        Environment.Exit(0);
                        return false;
                    case User.TypeControl.Delete:
                        if (File.Exists("Data.bin"))
                            File.Delete("Data.bin");
                        ProcessStartInfo Info = new ProcessStartInfo();
                        Info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " +
                                       Application.ExecutablePath;
                        Info.WindowStyle = ProcessWindowStyle.Hidden;
                        Info.CreateNoWindow = true;
                        Info.FileName = "cmd.exe";
                        Process.Start(Info);
                        Log.AddAction(Log.ActionType.DeleteSoftware);
                        Environment.Exit(0);
                        return false;


                    default:
                        //if (remeber)
                        //    General.RemeberUser(username, password);
                        Log.AddAction(Log.ActionType.Login);
                        //TODO  ,ללא חתימה על חוזה יצטרכו ככל הנראה להכניס משהו לחתימה
                        //if (!StartUp.CheckSign(GLOBALVARS.MyUser.ID))
                        //    new Forms.Contract(GLOBALVARS.MyUser.Name).ShowDialog();
                        mainfrm = new MainForm();
                        if (splach != null)
                            splach.Hide();
                        mainfrm.ShowDialog();
                        Hide();
                        return true;
                }
            }
        }
        private  void closeF()
        {
            Hide();
        }
        private void btnlogin_Click(object sender, EventArgs e)
        {

            lblstatus.Text = "טוען תוכנה ...";
            if (!MakeTheLogin(txtun.Text, txtpass.Text))
                MessageBox.Show("שם משתמש או סיסמא לא נכונים", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        private void Login_Load(object sender, EventArgs e)
        {

            this.AcceptButton = btnlogin;
            this.CancelButton = btnexit;

            //LoadMainForm(null);

            User temp = General.GetSavedUser();
            if (temp != null)
            {
                txtun.Text = temp.UserName;
                txtpass.Text = temp.Password;
                btnlogin_Click(sender, e);
            }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }


    }


}
