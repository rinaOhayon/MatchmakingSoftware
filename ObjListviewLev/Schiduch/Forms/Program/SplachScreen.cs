using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;



namespace Schiduch.Forms
{
    public partial class SplachScreen : Form
    {
        public static bool Dev = true;
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        public SplachScreen()
        {
            InitializeComponent();

        }

        //public void AppendText(string text, Color txtcolor)
        //{

        //    RichTextBox box = txtstatus;
        //    box.SelectionStart = box.TextLength;
        //    box.SelectionLength = 0;
        //    box.SelectionColor = txtcolor;
        //    box.AppendText(text);
        //    box.SelectionColor = box.ForeColor;
        //}



        private void SplachScreen_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }


        private void StartUpLoad()
        {

            StartUp.splscreen = this;
            txtstatus.Text = Lang.MsgSetStartupConfig;
            StartUp.SetAppLocation();
            StartUp.CheckOldFiles();
            txtstatus.Text = Lang.MsgInternetConnectionLabel;

            StartUp.ConnectToServer();
            txtstatus.Text = Lang.MsgCorrect;


            /*    AppendText("\n" + Lang.MsgCheckForUpdate, Color.Black);
                if (File.Exists("Update.exe")) { 
                    Process pupd=Process.Start("Update.exe",Application.ExecutablePath);
                    pupd.WaitForExit();
                    int ext=pupd.ExitCode;
                    if (ext != 5 && ext!=6)
                    {
                        MessageBox.Show("אירעה שגיאה במערכת העדכון", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                    if (ext == 5)
                    {
                        string sfilename = Process.GetCurrentProcess().Modules[0].FileName;
                        File.Move(sfilename, "old.del");
                        File.Move("n.exe", sfilename);
                        Process.Start(sfilename);
                        Environment.Exit(Environment.ExitCode);
                    }
                }
                else
                {
                    MessageBox.Show("אירעה שגיאה במערכת העדכון", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }


            AppendText(Lang.MsgUpdated, Color.Green);
           
            AppendText("\n" + Lang.MsgCheckRegistredSw, Color.Black);
                StartUp.CheckRegisredSw();
            AppendText(Lang.MsgCorrect, Color.Green); */



            txtstatus.Text = Lang.MsgAvailable;


            txtstatus.Text = Lang.MsgLoadServices;

            txtstatus.Text = Lang.MsgAvailable;


            txtstatus.Text = Lang.MsgOpenLogin;

           
        }

        private void SplachScreen_Shown(object sender, EventArgs e)
        {

            BackgroundWorker thr = new BackgroundWorker();
            thr.DoWork += Thr_DoWork;
            thr.RunWorkerAsync();
            thr.RunWorkerCompleted += Thr_RunWorkerCompleted;
            //  ThreadPool.QueueUserWorkItem(new WaitCallback(StartUpLoad));
        }

        private void Thr_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Login log = new Login();
            bool ret = log.MakeTheLogin(null, null,  this);
            if (!ret)
            {
                this.Hide();
                log.Show();
            }
        }

        private void Thr_DoWork(object sender, DoWorkEventArgs e)
        {
            StartUpLoad();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            try
            {
                Environment.Exit(Environment.ExitCode);
            }
            catch { };
        }

        private void picload_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Finds the MAC address of the first operation NIC found.
        /// </summary>
        /// <returns>The MAC address.</returns>
        private string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                     nic.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx ||
                     nic.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();

                    break;
                }
            }
            //string strHostName = "";
            //strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            //IPAddress[] addr = ipEntry.AddressList;
            //string dddd = addr[addr.Length - 1].ToString();
            return macAddresses;
        }
    }
}
