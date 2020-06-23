using Schiduch.Classes.Program;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Schiduch
{

    
    class UpdateManager
    {
        static SqlDataReader reader;
        private static bool FirstTry = true;
        private static string UpdateFile = Application.StartupPath + "\\Upd.bin";
        private static int FilesToBeUpdate = 0;
        private static UpdateWindow winupdate;
        public static bool ThreadFinsihLoading = false;
        private static List<FileAndVersion> FileList;
        private static List<FileAndVersion> UpdatedList;
      
      
      
        public static void CheckForUpdate()
        {
            Thread updatethread;
            int tempid;
           // SqlDataReader tempreader;
            LoadLastUpdateFromFileAndDb();
            if (GLOBALVARS.LastSwUpdateDB <= GLOBALVARS.LastSwUpdateFile)
                return;
            GLOBALVARS.LastSwUpdateFile = DateTime.Now;
            //START CHECK WHICH FILES ARE UPDATED
            OpenUpdateFile();
            reader = DBFunction.ExecuteReader("Select active,filename,version,[date],id,data from FileUpdates");

            string sfilename = Process.GetCurrentProcess().Modules[0].FileName;
            if (reader.HasRows)
            {
                //ThreadPool.QueueUserWorkItem(new WaitCallback(ShowUpdateDialog));
                updatethread = new Thread(new ThreadStart(ShowUpdateDialog));
                updatethread.IsBackground = true;
                updatethread.Start();
            }
            UpdatedList = new List<FileAndVersion>();
            while (!ThreadFinsihLoading) { } //Loop Until Thread Loaded
            while (reader.Read())
            {


                FileAndVersion ftemp = new FileAndVersion();
                ftemp.Filename = (string)reader["Filename"];
                ftemp.Date = (DateTime)reader["Date"];
                tempid = (int)reader["id"];
                UpdatedList.Add(ftemp);
                if (!SerachInTheListFiles(ftemp))
                {

                 //   tempreader= DBFunction.ExecuteReader("Select data from FileUpdates where id=" + tempid);
                   // tempreader.Read();
                    byte[] data = (byte[])reader["data"];
                 //   tempreader.Close();
                    winupdate.PbUpdate.Value++;
                    if (((bool)reader["Active"]))
                    { // Check if this is the sw itself
                        File.WriteAllBytes(Application.StartupPath + "//temp.ex", data);
                        File.Move(sfilename, "old.del");
                        File.Move("temp.ex", sfilename);
                    }
                    else
                        File.WriteAllBytes(Application.StartupPath + "//" + ftemp.Filename, data);

                }
                else
                {
                    winupdate.PbUpdate.Value++;
                }
            }
            winupdate.PbUpdate.Value++;
            reader.Close();
            UpdateTheFile();
            UpdateLastTimeCheck(DateTime.Now);
            Process.Start(sfilename);
            Environment.Exit(Environment.ExitCode);

        }

        private static void LoadLastUpdateFromFileAndDb()
        {
            //START OF CHECK IF THERE IS A UPDATE
            string LastupdateSql = "select top 1 Status,Lastupdate,FilesToBeUpdate,LastAlertsChanges,LastPeopleChanges,LastUserChange,LastChatChange from general";
            GetLastUpdate();

            reader = DBFunction.ExecuteReader(LastupdateSql);
            if (reader.HasRows)
            {
                reader.Read();
                GLOBALVARS.LastSwUpdateDB = DateTime.Parse(reader["Lastupdate"].ToString());
                GLOBALVARS.LastAlertsCheckDB = DateTime.Parse(reader["LastAlertsChanges"].ToString());
                GLOBALVARS.LastPeopleCheckDB = DateTime.Parse(reader["LastPeopleChanges"].ToString());
                GLOBALVARS.LastUserChangeDB = DateTime.Parse(reader["LastUserChange"].ToString());
                GLOBALVARS.LastChatChangeDB = DateTime.Parse(reader["LastChatChange"].ToString());
                FilesToBeUpdate = (int)reader["FilesToBeUpdate"];
                GLOBALVARS.StatusSw = (int)reader["Status"];
                
            }
            FilesToBeUpdate++;
            reader.Close();
            //END OF CHECK
        }

        private static void GetLastUpdate()
        {
            string Sfile = Application.StartupPath + "\\LUSW";
            string pasttime = DateTime.Now.AddYears(-3).ToString();
            if (!File.Exists(Sfile)) { 
                File.WriteAllText(Sfile, pasttime + "\r\n" + pasttime + "\r\n" + pasttime + "\r\n" +pasttime + "\r\n" + pasttime);
            }
            string[] dates = File.ReadAllLines(Sfile);
            GLOBALVARS.LastSwUpdateFile= DateTime.Parse(dates[0]);
            GLOBALVARS.LastAlertsCheckFile = DateTime.Parse(dates[1]);
            GLOBALVARS.LastPeopleCheckFile = DateTime.Parse(dates[2]);
            GLOBALVARS.LastUserChangeFile = DateTime.Parse(dates[3]);
            GLOBALVARS.LastChatChangeFile = DateTime.Parse(dates[4]);
        }

        public static void UpdateLastTimeCheck(DateTime sw=default(DateTime),DateTime alerts = default(DateTime), DateTime people = default(DateTime), DateTime user = default(DateTime),DateTime chat_time=default(DateTime))
        {
            if (sw == default(DateTime))
                sw = GLOBALVARS.LastSwUpdateFile;
            if (alerts == default(DateTime))
                alerts = GLOBALVARS.LastAlertsCheckFile;
            if (people == default(DateTime))
                people = GLOBALVARS.LastPeopleCheckFile;
            if (user == default(DateTime))
                user = GLOBALVARS.LastUserChangeFile;
            if (chat_time == default(DateTime))
                chat_time = GLOBALVARS.LastChatChangeFile;
            File.WriteAllText(Application.StartupPath + "\\LUSW", sw.ToString() + "\r\n"
                +alerts.ToString() + "\r\n" + people.ToString() + "\r\n" + user.ToString() + "\r\n" + 
                chat_time.ToString());
        }
        public static void UpdateLastTimeCheckToDb()
        {
            SqlParameter[] prms=new SqlParameter[5];
            SqlParameter sw = new SqlParameter("@sw", DateTime.Now);
            SqlParameter alert = new SqlParameter("@alert", DateTime.Now);
            SqlParameter people = new SqlParameter("@people", DateTime.Now);
            SqlParameter user = new SqlParameter("@user", DateTime.Now);
            SqlParameter chat = new SqlParameter("@chat", DateTime.Now);
            prms[0] = sw;
            prms[1] =alert;
            prms[2] = people;
            prms[3] = user;
            prms[4] = chat;
            DBFunction.Execute("Update general set LastUpdate=@sw,LastAlertsChanges=@alert,LastPeopleChanges=@people,LastUserChange=@user,LastChatChange=@chat", prms);
        }
        private static void ShowUpdateDialog()
        {

            winupdate = new UpdateWindow();
            winupdate.PbUpdate.Maximum = FilesToBeUpdate;
            winupdate.ShowDialog();

        }

        private static void OpenUpdateFile()
        {

            CheckFileExist();
            try
            {
                string[] files = File.ReadAllLines(UpdateFile);
                FileList = new List<FileAndVersion>();
                foreach (string line in files)
                {
                    string[] spl = line.Split('|');
                    FileAndVersion file = new FileAndVersion();
                    file.Filename = spl[0];
                    file.Date = DateTime.Parse(spl[1]);
                    FileList.Add(file);
                }
            }
            catch (IOException)
            {
                if (FirstTry)
                {
                    FirstTry = false;
                    Thread.Sleep(1000);
                    OpenUpdateFile();
                }
                else
                {
                    MessageBox.Show("עדכון נכשל שגיאה מספר 421\r\nהפעלה מחדש של התוכנה עשויה לפתור את הבעיה");
                    Environment.Exit(Environment.ExitCode);
                }

            }
        }
        private static void CheckFileExist()
        {
            if (!File.Exists(UpdateFile))
                File.Create(UpdateFile).Close();
            if (!Directory.Exists("Msgs"))
                Directory.CreateDirectory("Msgs");

        }

        private static void LoadLastUpdates()
        {
            
        }
        private static bool SerachInTheListFiles(FileAndVersion file)
        {
            foreach (FileAndVersion f in FileList)
            {
                if (DateTime.Compare(f.Date,file.Date) >=0 && f.Filename.Trim() == file.Filename.Trim())
                    return true;
            }
            return false;
        }

        private static void UpdateTheFile()
        {
            string update = "";
            foreach (FileAndVersion file in UpdatedList)
                update += file.Filename + "|" + file.Date.ToString() + "\r\n";
            File.WriteAllText(UpdateFile, update);
        }

    }
    class FileAndVersion
    {
        public string Filename = "";
        public DateTime Date;
    }
}
