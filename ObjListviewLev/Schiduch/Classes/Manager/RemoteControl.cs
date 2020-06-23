using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Principal;

namespace Schiduch
{
    class RemoteDesktopClass
    {
        public static Process p;
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        private delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumedWindow callback, ArrayList lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        private static string Getname(IntPtr hwnd)
        {
            StringBuilder ClassName = new StringBuilder(256);
            //Get the window class name
            int nRet = GetClassName(hwnd, ClassName, ClassName.Capacity);
            return ClassName.ToString();
        }
        private static bool GetWindowHandle(IntPtr windowHandle, ArrayList windowHandles)
        {
            windowHandles.Add(windowHandle);
            return true;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [Out] StringBuilder lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);


        static void HideHandle(IntPtr hl)
        {
            bool x = SetWindowPos(hl, (IntPtr)0, 0, 0, 0, 0, 0x0080);
        }
        public static string GetWindowTextRaw(IntPtr hwnd)
        {
            // Allocate correct string length first
            int length = (int)SendMessage(hwnd, 0x000E, IntPtr.Zero, null);
            StringBuilder sb = new StringBuilder(length + 1);
            SendMessage(hwnd, 0x000D, (IntPtr)sb.Capacity, sb);
            return sb.ToString();
        }


        public static bool DownloadFile()
        {
            if (File.Exists(Application.StartupPath + "\\Tvq.exe"))
                return true;
            WebClient sclient = new WebClient();
            sclient.DownloadFile("http://www.150.co.il/TeamViewerQS.exe", Application.StartupPath + "\\Tvq.exe");
            return true;
        }
        public static void RunTheProgram()
        {
            p = new Process();
            p.StartInfo = new ProcessStartInfo();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            p.StartInfo.FileName = Application.StartupPath + "\\Tvq.exe";
            p.StartInfo.Verb = "runas";

            p.Start();


        }
        public static void CloseTheProgram()
        {
            try
            {
                p.Kill();
            }
            catch { MessageBox.Show(Lang.MsgRemoteCloseErr, "", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        public static string GetTheIds()
        {
            IntPtr handle = (IntPtr)0;
            while (handle == (IntPtr)0)
            {
                handle = FindWindowByCaption((IntPtr)0, "TeamViewer");
            }
            HideHandle(handle);
            string ids = "";
            EnumedWindow callBackPtr = GetWindowHandle;
            ArrayList arr = new ArrayList();
            EnumChildWindows(handle, callBackPtr, arr);
            foreach (IntPtr p in arr)
            {
                if (Getname(p) == "Edit")
                {
                    ids += GetWindowTextRaw(p).Replace(" ", "");
                    ids += "-";
                }
            }
            ids = ids.Remove(ids.Length - 1);
            return ids;
        }
        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                    .IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
