using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Schiduch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Timer IdleTimer = new Timer();
        const int MinuteMicroseconds = 1000 * 60 * 60;//אם לא מתעסקים עם התוכנה שעה
        static Forms.SplachScreen f = null;
        public static Timer getImageTimer = new Timer();
        [STAThread]
        static void Main()
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LeaveIdleMessageFilter limf = new LeaveIdleMessageFilter();
            Application.AddMessageFilter(limf);
            Application.Idle += new EventHandler(Application_Idle);
            IdleTimer.Interval = MinuteMicroseconds;    // One minute; change as needed
            IdleTimer.Tick += TimeDone;
            IdleTimer.Start();
            getImageTimer.Interval = 20000;
            getImageTimer.Tick += GetImageTimer_Tick;
            getImageTimer.Start();
            f = new Forms.SplachScreen();
           // Application.Run(new DetailRequiredfields());
           Application.Run(f);
           // Application.Run(new Forms.Manager.ActivitySumForm());
            Application.Idle -= new EventHandler(Application_Idle);
            // Application.Run(new Forms.SplachScreen());
            // Application.Run(new ShiduchActivityForm());
        }

        private static void GetImageTimer_Tick(object sender, EventArgs e)
        {
            // Process[] arrProcess = Process.;


            if (Clipboard.ContainsImage())
                Clipboard.Clear();


        }

        static private void Application_Idle(Object sender, EventArgs e)
        {
            if (!IdleTimer.Enabled)     // not yet idling?
                IdleTimer.Start();
        }

        static private void TimeDone(object sender, EventArgs e)
        {
            IdleTimer.Stop();   // not really necessary
            MessageBox.Show("יצאת מהמערכת");

            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm is MainForm)
                    frm.Close();
            }
            //Environment.Exit(0);

        }

    }
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public class LeaveIdleMessageFilter : IMessageFilter
    {
        const int WM_NCLBUTTONDOWN = 0x00A1;
        const int WM_NCLBUTTONUP = 0x00A2;
        const int WM_NCRBUTTONDOWN = 0x00A4;
        const int WM_NCRBUTTONUP = 0x00A5;
        const int WM_NCMBUTTONDOWN = 0x00A7;
        const int WM_NCMBUTTONUP = 0x00A8;
        const int WM_NCXBUTTONDOWN = 0x00AB;
        const int WM_NCXBUTTONUP = 0x00AC;
        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int WM_MOUSEMOVE = 0x0200;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_RBUTTONUP = 0x0205;
        const int WM_MBUTTONDOWN = 0x0207;
        const int WM_MBUTTONUP = 0x0208;
        const int WM_XBUTTONDOWN = 0x020B;
        const int WM_XBUTTONUP = 0x020C;

        // The Messages array must be sorted due to use of Array.BinarySearch
        static int[] Messages = new int[] {WM_NCLBUTTONDOWN,
            WM_NCLBUTTONUP, WM_NCRBUTTONDOWN, WM_NCRBUTTONUP, WM_NCMBUTTONDOWN,
            WM_NCMBUTTONUP, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, WM_KEYDOWN, WM_KEYUP,
            WM_LBUTTONDOWN, WM_LBUTTONUP, WM_RBUTTONDOWN, WM_RBUTTONUP,
            WM_MBUTTONDOWN, WM_MBUTTONUP, WM_XBUTTONDOWN, WM_XBUTTONUP};

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_MOUSEMOVE)  // mouse move is high volume
                return false;
            if (!Program.IdleTimer.Enabled)     // idling?
                return false;           // No
            if (Array.BinarySearch(Messages, m.Msg) >= 0)
                Program.IdleTimer.Stop();
            return false;
        }
    }
}
