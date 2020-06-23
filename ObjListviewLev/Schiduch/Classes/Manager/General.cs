using System;
using System.Windows.Forms;
using System.Text;
using System.Data.SqlClient;
using Schiduch.Properties;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using Schiduch.Classes.Program;

namespace Schiduch
{
    class General
    {
        public enum SwStatus {None=0,Construct=1}
        static Form frmload;
        public static bool IsSaveUser = false;
        public static void LoadLogo()
        {
            frmload = new Form();
            Label lbl = new Label();
            lbl.Text = "טוען תוכנה...";
            frmload.Controls.Add(lbl);
            frmload.Size = lbl.Size;
            frmload.FormBorderStyle = FormBorderStyle.None;
            frmload.StartPosition = FormStartPosition.CenterScreen;
            frmload.Show();
        }
        public static void CloseLogo() { frmload.Close(); }
        public static SwStatus Status()
        {

            return (SwStatus)GLOBALVARS.StatusSw ;
        }

        public static void CreateReport(People p = null)
        {
            string FutureLearn = "";
            
            
            if (p == null)
                return;
            if (p.FutureLearn)
                FutureLearn = "כן";
            string html = "<html lang='he'><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><style>.scrollable{    width: 20%;    height: 100%;    margin: 0;    padding: 0;    overflow: auto;}</style><hr><center><h2>" + p.FirstName + " " + p.Lasname + 
                "</h2><hr><table border=1 style='width:50%' dir=rtl><td style='width:30%'><table>" + 
                "<td>גיל</td><td>" + p.Age.ToString() + "</td><tr>"+
                "<td>גובה</td><td>" + p.Tall.ToString() + "</td><tr>" +
                "<td>מין</td><td>" + p.Sexs + "</td><tr>" +
                "<td>מצב</td><td>" + p.Status + "</td><tr>" +
                "<td>מראה כללי</td><td>" + p.Looks + "</td><tr>" +
                "<td>צבע פנים</td><td>" + p.FaceColor + "</td><tr>" +
                "<td>מראה חיצוני</td><td>" + p.Weight + "</td><tr>" +
                "<td>רקע</td><td>" + p.Background + "</td><tr>" +
                "<td>כתובת</td><td>" + p.Details.Street + "</td><tr>" +
                "<td>עיר</td><td>" + p.City + "</td><tr>" +
                "<td>עדה</td><td>" + p.Eda + "</td><tr>" +
                "<td>מוצא</td><td>" + p.Zerem + "</td><tr>" +
                "<td>עבודה</td><td>" + p.WorkPlace + "</td><tr>" +
                "<td>ג או עץ</td><td>" + p.GorTorN + "</td><tr>" +
                "<td>מתכונן להמשיך ללמוד</td><td>" + FutureLearn + "</td><tr>" +
                "<td>פתיחות</td><td>" + p.OpenHead + "</td><tr>" +
                "<td>פקס או מייל</td><td>" + "יש לבדוק בתוכנה עצמה" + "</td><tr>" +
                "</table></td><td style='width:30%'><table>"+
                "<td>כיסוי ראש</td><td>" + p.CoverHead + "</td><tr>" +
                "<td>זקן</td><td>" + p.Beard + "</td><tr>" +
                "<td>לומד</td><td>" + p.LearnStaus + "</td><tr>" +
                "<td>שם האבא</td><td>" + p.Details.DadName + "</td><tr>" +
                "<td>שם האמא</td><td>" + p.Details.MomName + "</td><tr>" +
                "<td>עיסוק האב</td><td>" + p.DadWork + "</td><tr>" +
                "<td>עיסוק האם</td><td>" + p.Details.MomWork + "</td><tr>" +
                "<td>נותנים</td><td>" + p.Details.MoneyGives.ToString() + "</td><tr>" +
                "<td>דורשים</td><td>" + p.Details.MoneyRequired.ToString() + "</td><tr>" +
                "<td>גמישות כספית</td><td>" + p.Details.MoneyNotesFlex + "</td><tr>" +
                "<td>מספר הילדים</td><td>" + p.Details.ChildrenCount.ToString() + "</td><tr>" +
                "<td>רב שהבית קשור אליו</td><td>" + p.Details.HomeRav + "</td><tr>" +
                "<td>שם משפחת האם</td><td>" + p.Details.MomLname + "</td><tr>" +
                "<td>ספרדים משתכנזים</td><td>" + p.StakeM + "</td><tr>" +
                "<td>תימנים כמה חשובה המסורת</td><td>" + p.TneedE + "</td><tr>" +
                "<td>דמי שדכנות</td><td>" + p.Details.MoneyToShadchan + "</td><tr>" +
                "<td>טלפון</td><td>" + "יש לבדוק בתוכנה עצמה" + "</td></table></td></table>" +
                "<br><table width='70%' border=1 dir=rtl><th>תכונות חשובות בי</th><th>תכונות חשובות שאני מחפש</th><th>מקומות לימוד</th><th>מקומות לימוד המשפחה</th><th>שמות וכתובות מחותנים</th><tr><td class='scrollable'>"+
                p.Details.WhoAmI+"</td><td class='scrollable'>"+p.Details.WhoIWant + "</td><td class='scrollable'>"+ p.Details.Schools +"</td><td class='scrollable'>"+p.Details.SiblingsSchools+"</td><td class='scrollable'>"+p.Details.MechutanimNames+"</td></table><br><table width='70%' border=1 dir=rtl>"+
                "<th>הערות</th><th>טלפון צוות</th><th>טלפות חברים</th><tr><td class='scrollable'>"+
                p.Details.Notes+ "</td><td class='scrollable'>"+ p.Details.ZevetInfo+ "</td><td class='scrollable'>"+ p.Details.FriendsInfo+"</td></table>";
            File.WriteAllText(Application.StartupPath + "\\Rep.Xinfo", html);
            
        }

        public static void RemeberUser(string Un,string Pass)
        {
            if (string.IsNullOrEmpty(Un))
            {
                string keyName = @"Software\Microsoft\Windows\CurrentVersion\Explorer";
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue("ExplorerObj");
                    }
                }
                return;
            }
           string savedata = Base64Encode(Un + "^" + Pass);
           Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ExplorerObj", savedata);
        }
        
        public static User GetSavedUser()
        {
            User myuser=null;
            try
            {
                string getdata = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ExplorerObj", null);
                string keyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer";
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue("ExplorerObj");
                    }
                }
                if (!string.IsNullOrEmpty(getdata))
                {
                    IsSaveUser = true;
                    getdata = Base64Decode(getdata);
                    string[] spldata = getdata.Split('^');
                    myuser = new User();
                    myuser.UserName = spldata[0];
                    myuser.Password = spldata[1];

                }
            }
            catch { }
            return myuser;
           
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static void RunAsAdmin()
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = true;
            proc.WorkingDirectory = Environment.CurrentDirectory;
            proc.FileName = Application.ExecutablePath;
            proc.Verb = "runas";
            try
            {
                Process.Start(proc);
            }
            catch
            {
                return;
            }
            Environment.Exit(0);
        }
    }

}
