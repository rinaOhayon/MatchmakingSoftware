using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Drawing2D;
using System.Collections;
using BrightIdeasSoftware;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using BrightIdeasSoftware.Design;
using Schiduch.Classes.Program;
using Schiduch.Classes.Users;
using Schiduch.Forms.Manager;

namespace Schiduch
{
    public partial class MainForm : Form
    {


        public Forms.LoadProgress formloadmsg = new Forms.LoadProgress();
        public static bool Showloadprocess = true;

        private bool Tab1, Tab2, Tab3, Tab4 = true, Tab5, Tab6, Tab7, Tab8, Tab9, Tab10, Tab12;


        public MainForm()
        {
            InitializeComponent();
            //groupBoxSearch.Location = new Point(this.Size.Width - groupBoxSearch.Size.Width - 35, 4);
            //groupBox10.Location = new Point(this.Size.Width - groupBoxSearch.Size.Width - 35 - groupBox10.Width - 20, 4);
            // LoadSettings();
            //tabPage1.BackgroundImage = Properties.Resources.bgMain;
        }
        private void OpenReportForm(string titlestart, string titlemiddle, string titleend, Color titlem_color, string sfile)
        {
            new Forms.ReportForm(titlestart, titlemiddle, titleend, titlem_color, sfile).ShowDialog();
        }
        public object ChooseCoreectImageToObjLstPeople(object rowObject)
        {
            People p = (People)rowObject;
            // if (IsVIPClient(p.Details.MoneyToShadchan)) return 10;
            return p.Sexs;
        }
        public object MatchItemImage(object rowObject)
        {
            return 11;

        }
        public void MainForm_Load(object sender, EventArgs e)
        {

            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Text = "לב אחד איגוד השדכנים" + StartUp.Ver;
            StartUp.CheckOs(GLOBALVARS.MyUser.ID);
            olvColumn1.ImageGetter = new BrightIdeasSoftware.ImageGetterDelegate(ChooseCoreectImageToObjLstPeople);
            //olvColumn10.ImageGetter = new BrightIdeasSoftware.ImageGetterDelegate(ChooseCoreectImageToObjLstPeople);
            olvColumn8.ImageGetter = new ImageGetterDelegate(MatchItemImage);
            // cmb_accuracy.SelectedIndex = 0;

            this.AcceptButton = btnfilter;
            CheckForIllegalCrossThreadCalls = false;
            // LoadData();
            //  LoadReminder();
            CheckAccess();

        }




        private void Search(object obj)
        {

            SqlDataReader reader;


            SqlParameter[] prms = new SqlParameter[25];
            string Sql = "";
            string AgeSql = "";
            string whoami = "";
            string whoiwant = "";
            string sqlwhoiwant = "";
            string sqlwhoami = "";
            string noteswhoami = "";
            string noteswhoiwant = "";
            string LearnStatus = "";
            string Subscription = "";
            string sexs = "";
            int fromage = (int)txtfromage.Value;
            int tillage = (int)txttillage.Value;
            string show = " AND SHOW <> 8 AND (show <2 or (show=5 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%') or (show=4 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%'))";
            string IdFilter = "";
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Manger || GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
                show = " and show <> 8";

            if (txtlearnstatus.SelectedIndex != -1)
                LearnStatus = BuildSql.GetSql(out prms[16], txtlearnstatus.Text, "LearnStatus", BuildSql.SqlKind.EQUAL);
            if (tillage > 0)
                AgeSql = BuildSql.GetSql(out prms[0], fromage, "age", BuildSql.SqlKind.BETWEEN, true, tillage);
            // if(chksubscription.Checked)
            // Subscription= BuildSql.GetSql(out prms[17], chksubscription.Checked, "Subscription", BuildSql.SqlKind.EQUAL,false);
            if (txtid.Value != 0)
                IdFilter = " peoples.ID=" + txtid.Value + " AND ";

            if (txtwhoami.Text.Length > 0)
            {
                noteswhoami = whoami = "(";
                foreach (string s in splitwords(txtwhoami.Text))
                {
                    if (s.Trim().Length > 0)
                    {
                        whoami += " whoami like N'%" + s + "%' and";
                        noteswhoami += " notes like N'%" + s + "%' and";
                    }
                }
                whoami = whoami.Remove(whoami.Length - 3, 3);
                noteswhoami = noteswhoami.Remove(noteswhoami.Length - 3, 3);
                noteswhoami += ")";
                whoami += ")";
                sqlwhoami = "(" + whoami + " or " + noteswhoami + ")";
            }
            if (txtwhoiwant.Text.Length > 0)
            {
                whoiwant = noteswhoiwant = "(";
                foreach (string s in splitwords(txtwhoiwant.Text))
                {
                    if (s.Trim().Length > 0)
                    {
                        whoiwant += " whoiwant like N'%" + s + "%' and";
                        noteswhoiwant += " notes like N'%" + s + "%' and";
                    }
                }
                whoiwant = whoiwant.Remove(whoiwant.Length - 3, 3);
                noteswhoiwant = noteswhoiwant.Remove(noteswhoiwant.Length - 3, 3);
                noteswhoiwant += ")";
                whoiwant += ")";
                sqlwhoiwant = "(" + whoiwant + " or " + noteswhoiwant + ")";
            }
            if (!string.IsNullOrEmpty(whoiwant) && !string.IsNullOrEmpty(whoami))
                sqlwhoami += " and ";
            if (txtsexs.SelectedIndex != -1)
                sexs = BuildSql.GetSql(out prms[12], txtsexs.SelectedIndex + 1, "sexs", BuildSql.SqlKind.EQUAL);

            Sql = " select schools,sexs,firstname,lastname,tall,age,City,fat," +
                                "FaceColor,Looks,WorkPlace,Beard,Zerem,Eda,LearnStatus,DadWork," +
                                "Background,Status,Tz,KindChasidut,HealthStatus,ZeremMom,Street," +
                                "DadName,MomName,MomWork,"
             + "[peopledetails].[relatedid],[Peoples].[ID],Tel1,Tel2,Telephone,PhoneOfBachur from peoples inner join peopledetails on ID=relatedid  where temp='false' AND " +
            BuildSql.GetSql(out prms[1], txtfname.Text, "FirstName", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[2], txtlname.Text, "Lastname", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[3], txtbeard.Text, "beard", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[4], txtbg.Text, "background", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[5], txtcoverhead.Text, "coverhead", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[6], txtdadwork.Text, "dadwork", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[7], txtfacecolor.Text, "facecolor", BuildSql.SqlKind.LIKE) +
            IdFilter +
            BuildSql.GetSql(out prms[8], txtfat.Text, "fat", BuildSql.SqlKind.LIKE) +
            AgeSql +
            BuildSql.GetSql(out prms[9], txtlooks.Text, "looks", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[10], txtpeticut.Text, "openhead", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[11], txtschool.Text, "schools", BuildSql.SqlKind.LIKE) +
            sexs +
            BuildSql.GetSql(out prms[13], txtzerem.Text, "(eda", BuildSql.SqlKind.LIKE, false, null, true) +
            BuildSql.GetSql(out prms[14], txtzerem.Text, "zerem", BuildSql.SqlKind.LIKE, false, null, false, ") AND ") +
            BuildSql.GetSql(out prms[15], txtstatus.Text, "Status", BuildSql.SqlKind.EQUAL) +
            BuildSql.GetSql(out prms[17], txtCity.Text, "City", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[18], txtPhone.Text, "(Tel1", BuildSql.SqlKind.LIKE, false, null, true) +
            BuildSql.GetSql(out prms[19], txtPhone.Text, "Tel2", BuildSql.SqlKind.LIKE, false, null, true) +
            BuildSql.GetSql(out prms[20], txtPhone.Text, "Telephone", BuildSql.SqlKind.LIKE, false, null, true) +
            BuildSql.GetSql(out prms[21], txtPhone.Text, "PhoneOfBachur", BuildSql.SqlKind.LIKE, false, null, false, " ) AND ") +
            sqlwhoami + sqlwhoiwant +
            LearnStatus +
            Subscription;
            Sql = BuildSql.CheckForLastAnd(ref Sql);
            Sql += show;
            Sql += " ORDER BY ID DESC";
            reader = DBFunction.ExecuteReader(Sql, prms);

            // fs.Search(txtfreeserach.Text, (FreeSearch.accuracy)cmb_accuracy.SelectedIndex);
            List<People> lst = new List<People>();


            while (reader.Read())
            {
                People p = new People();

                PeopleManipulations.ReaderToPeople(ref p, ref reader, PeopleManipulations.RtpFor.ForSearch);
                lst.Add(p);

            }


            olstpeople.BeginUpdate();
            olstpeople.SetObjects(lst);
            olstpeople.EndUpdate();

            reader.Close();
            DBFunction.CloseConnections();
            //picload.Visible = false;
            btnfilter.Enabled = true;
        }
        private void btnfilter_Click(object sender, EventArgs e)
        {
            //picload.Visible = true;
            btnfilter.Enabled = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(Search));
        }

        private string[] splitwords(string str)
        {
            str = str.Replace("'", "").Replace(";", "");
            string[] starray = str.Split(' ');
            int x = 0;
            foreach (string s in starray)
            {
                if (s.StartsWith("ו"))
                    starray[x] = str.Remove(0, 1);
                x++;
            }
            return starray;
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
                Log.SetDurationLogin();
            for (int i = 0; i < 1; i++)
                WriteSettings();
            DBFunction.CloseConnections();
            Program.IdleTimer.Stop();
            Environment.Exit(0);
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtbeard.Text = "";
            txtbg.Text = "";
            txtcoverhead.Text = "";
            txtdadwork.Text = "";
            txtfacecolor.Text = "";
            txtfat.Text = "";
            txtfname.Text = "";
            txtfromage.Value = 0;
            txtlname.Text = "";
            txtwhoiwant.Text = "";
            txtwhoiwant.Text = "";
            txtlooks.Text = "";
            txtpeticut.Text = "";
            txtschool.Text = "";
            txtsexs.Text = "";
            txttillage.Value = 0;
            txtzerem.Text = "";
            txtstatus.Text = "";
            txtlearnstatus.Text = "";
            txtCity.Text = "";
            txtPhone.Text = "";
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            People p = new People();

            DetailRequiredfields d = new DetailRequiredfields();
            d.ShowDialog();
            if (d.OK)
            {
                p = d.newPeople;
                DetailForm df = new DetailForm(p);
                df.Show();
                df.FormClosed += Df_FormClosed;
            }
            else
                d = null;
        }

        private void Df_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((sender as DetailForm).SaveCard)
                olstpeople.InsertObjects(0, new List<object>() { (sender as DetailForm).MyPeople });
        }
        bool first = true;
        public void MainForm_Shown(object sender, EventArgs e)
        {
            //this.Refresh();
            LoadData();
            LoadReminder();
            LoadSettings();
            Sector.GetSectors();
            GLOBALVARS.AllLoad = true;
            LoadShadcanim();
            LoadLabelsToTxts();

            //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My_Computer_JBLU-900599.txt";
            //if (File.Exists(path)&&GLOBALVARS.IsDeveloper)
            //{
            //    MessageBox.Show("This is the Production");
            //}
            //tabPage1.MouseHover += TabPage1_MouseHover;
            ///tabPage1.MouseEnter += TabPage1_MouseEnter;
            //  - tabPage1.BackgroundImage = Properties.Resources.bg;
        }





        private void btndel_Click(object sender, EventArgs e)
        {
            try
            {
                People p = olstpeople.SelectedObject as People;
                if (p == null) return;
                int id = p.ID;

                if (GLOBALVARS.MyUser.Control != User.TypeControl.Admin)
                {
                    if (p.Show != (int)People.ShowTypes.Personal)
                    {
                        MessageBox.Show("אין לך הרשאה למחוק את הלקוח הזה", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }


                if (People.DeletePeople(id, true, false))
                    olstpeople.SelectedItem.Remove();
            }

            catch { }

        }

        private void btnaddschadcan_Click(object sender, EventArgs e)
        {
            AddUser user = new AddUser();
            user.OpenDetailsForAdd = true;
            //user.userid = int.Parse(lstchadcan.SelectedItems[0].SubItems[3].Text);
            user.Show();
        }

        private void lstchadcan_DoubleClick(object sender, EventArgs e)
        {
            AddUser user = new AddUser();
            user.OpenDetailsForAdd = false;
            user.userid = int.Parse(lstchadcan.SelectedItems[0].SubItems[3].Text);
            user.ShowDialog();
        }

        private void btnuserdel_Click(object sender, EventArgs e)
        {
            if (lstchadcan.SelectedItems.Count <= 0)
                return;
            if (MessageBox.Show("האם אתה בטוח שברצונך למחוק", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int userid = int.Parse(lstchadcan.SelectedItems[0].SubItems[3].Text);
                string sql = "delete from users where id=" + userid;
                if (DBFunction.Execute(sql))
                {
                    MessageBox.Show("נמחק בהצלחה");
                    lstchadcan.Items.Remove(lstchadcan.SelectedItems[0]);
                }
                else
                    MessageBox.Show("אירעה שגיאה");
            }
        }

        private void btnuserblock_Click(object sender, EventArgs e)
        {
            if (lstchadcan.SelectedItems.Count <= 0)
                return;
            if (MessageBox.Show("האם אתה בטוח שברצונך לחסום", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int userid = int.Parse(lstchadcan.SelectedItems[0].SubItems[3].Text);
                string sql = "update users set control=4 where id=" + userid;
                if (DBFunction.Execute(sql))
                {
                    MessageBox.Show("נחסם בהצלחה");

                }
                else
                    MessageBox.Show("אירעה שגיאה");
            }
        }

        private void btnunblockuser_Click(object sender, EventArgs e)
        {
            if (lstchadcan.SelectedItems.Count <= 0)
                return;
            if (MessageBox.Show("האם אתה בטוח שברצונך לבטל את החסימה", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int userid = int.Parse(lstchadcan.SelectedItems[0].SubItems[3].Text);
                string sql = "update users set control=0 where id=" + userid;
                if (DBFunction.Execute(sql))
                {
                    MessageBox.Show("נפתח בהצלחה");

                }
                else
                    MessageBox.Show("אירעה שגיאה");
            }
        }

        private void btndelsw_Click(object sender, EventArgs e)
        {
            if (lstchadcan.SelectedItems.Count <= 0)
                return;
            if (MessageBox.Show("האם אתה בטוח שברצונך למחוק לו את התוכנה", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int userid = int.Parse(lstchadcan.SelectedItems[0].SubItems[3].Text);
                string sql = "update users set control=" + (int)User.TypeControl.Delete + " where id=" + userid;
                if (DBFunction.Execute(sql))
                {
                    MessageBox.Show("התוכנה תמחק בפעם הבאה שהמשתמש ינסה להתחבר");

                }
                else
                    MessageBox.Show("אירעה שגיאה");
            }
        }



        private void btnnointernet_Click(object sender, EventArgs e)
        {
            DBFunction.NoInternet();
        }


        private void OpenDetails(int id, bool show = true)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm is DetailForm)
                {
                    ((DetailForm)frm).btnceratehtml.Enabled = false;
                    ((DetailForm)frm).btnconfirm.Enabled = false;
                    //((DetailForm)frm).btnshowinfo.Enabled = false;
                }
            }
            if (id == 0) return;
            SqlDataReader reader;
            People p = new People();
            p.ID = id;
            reader = People.ReadById(p.ID);
            while (reader.Read())
            {
                PeopleManipulations.ReaderToPeople(ref p, ref reader, true, true);
            }
            reader.Close();
            DBFunction.CloseConnections();
            if (GLOBALVARS.MyUser.Control == User.TypeControl.User && p.Show == (int)People.ShowTypes.Personal)
                p.OpenForPersonalEdit = true;
            DetailForm detail = new DetailForm(p);
            if (show)
                detail.Show(this);

        }




        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }



        private void btnlstchadchanrefresh_Click(object sender, EventArgs e)
        {
            lstchadcan.Items.Clear();
            LoadShadcanim();
        }
        private void btnnewmsg_Click(object sender, EventArgs e)
        {
            Message newmsg = new Message();
            newmsg.ShowDialog();
        }

        private void txtfname_TextChanged(object sender, EventArgs e)
        {

        }







        bool isAscendingLstLog = false;
        ListViewItemComparer lstlogsort;

        private void removeBG()
        {
            foreach (TabPage item in tabControl1.TabPages)
            {
                if (item.BackgroundImage != null)
                    item.BackgroundImage = null;
            }
        }
        bool isFirstLoad = true;
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {

            if (isFirstLoad)
            {
                // LoadShadcanim();
                isFirstLoad = false;
            }
            if (e.TabPage == tabPage2)
            {
                dtActivityDatefrom.Value = DateTime.Now.AddDays(-7);
                LoadTab2(dtActivityDatefrom.Value, dtActivityDateto.Value);

            }
            else if (e.TabPage == tabPage3 && Tab3 == false)
            {
                //e.TabPage.BackgroundImage = Properties.Resources.bg;
                Tab3 = true;
                People p = new People();
                SqlDataReader reader;
                reader = People.ReadAll(0, false, false, true);
                lstmyclients.BeginUpdate();
                while (reader.Read())
                {
                    PeopleManipulations.ReaderToPeople(ref p, ref reader, PeopleManipulations.RtpFor.ForSearch);
                    LoadResultToList(ref p, lstmyclients);
                }
                lstmyclients.EndUpdate();
                reader.Close();

            }

            else if (e.TabPage == tabPage9 && Tab9 == false)
            {
                if (cmb_matchclient.Items.Count == 1)
                {
                    cmb_matchclient.Items.Add(new KeyValueClass("", 1000));
                    return;
                }
                cmb_matchclient.Items.Clear();
                picmatch.Visible = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadTab9));
            }



        }
        private void btnActivityDiary_Click(object sender, EventArgs e)
        {
            lblAct.Text = "פעילויות שלך מתאריך " + dtActivityDatefrom.Value.ToShortDateString()
                + " עד " + dtActivityDateto.Value.ToShortDateString();
            lblAct.Location = new Point(this.Width - 135 - lblAct.Width, lblAct.Location.Y);
            LoadTab2(dtActivityDatefrom.Value, dtActivityDateto.Value);
        }
        private void lstshadchanfilter(object sender, EventArgs e)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(txt_filtershadcanname.Text) || !string.IsNullOrEmpty(txt_filtershadcanname.Text))
            {
                for (int i = 0; i < lstchadcan.Items.Count; i++)
                {
                    if (lstchadcan.Items[i].Text.Contains(txt_filtershadcanname.Text) && lstchadcan.Items[i].SubItems[1].Text.Contains(txt_filtershadcantel.Text))
                    {
                        lstchadcan.Items[i].BackColor = Color.Red;
                        count++;
                    }
                    else
                        lstchadcan.Items[i].BackColor = Color.White;
                }
                lbl_filterresult.Text = "נמצאו : " + count + " תוצאות";
            }
            else
            {
                lbl_filterresult.Text = "";
                for (int i = 0; i < lstchadcan.Items.Count; i++)
                    lstchadcan.Items[i].BackColor = Color.White;
            }
        }


        private void lstmyclients_DoubleClick(object sender, EventArgs e)
        {
            if (lstmyclients.SelectedItems.Count != 0)
            {
                ShiduchActivity.insertActivity(
                    new ShiduchActivity()
                    {
                        Action = (int)ShiduchActivity.ActionType.openForms,
                        Date = DateTime.Now,
                        PeopleId = int.Parse(lstmyclients.SelectedItems[0].SubItems[6].Text),
                        UserId = GLOBALVARS.MyUser.ID,

                    });
                Log.AddAction(Log.ActionType.ClientOpen, new Log(Log.ActionType.ClientOpen,
                    lstmyclients.SelectedItems[0].Text + " " +
                    lstmyclients.SelectedItems[0].SubItems[1].Text + "^" + lstmyclients.SelectedItems[0].SubItems[6].Text));
                OpenDetails(int.Parse(lstmyclients.SelectedItems[0].SubItems[6].Text));
            }
        }

        private void searchmyclients(object sender, EventArgs e)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(txt_myclientfname.Text) || !string.IsNullOrEmpty(txt_myclientlname.Text))
            {
                for (int i = 0; i < lstmyclients.Items.Count; i++)
                {
                    if (lstmyclients.Items[i].Text.Contains(txt_myclientfname.Text) && lstchadcan.Items[i].SubItems[1].Text.Contains(txt_myclientlname.Text))
                    {
                        lstmyclients.Items[i].ForeColor = Color.Red;
                        count++;
                    }
                    else
                        lstmyclients.Items[i].ForeColor = Color.Black;
                }
                lblmyclientsresult.Text = "נמצאו : " + count + " תוצאות";
            }
            else
            {
                lblmyclientsresult.Text = "";
                for (int i = 0; i < lstmyclients.Items.Count; i++)
                    lstmyclients.Items[i].ForeColor = Color.Black;
            }
        }

        private void mnu_openclient_Click(object sender, EventArgs e)
        {
        }

        private void mnu_onewindow_Click(object sender, EventArgs e)
        {

            General.CreateReport();
            Report r = new Report();
            r.ShowDialog();
        }

        private void mnu_deleteclient_Click(object sender, EventArgs e)
        {

        }



        private void btn_matchsetting_Click(object sender, EventArgs e)
        {
            Forms.MatchSettings frmmatcsetting = new Forms.MatchSettings();
            frmmatcsetting.ShowDialog();
        }








        private void btnaddotherfiles_Click(object sender, EventArgs e)
        {
            string sapp = "";

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Jpg files (*.jpg)|";
            file.ShowDialog();
            sapp = file.FileName;
            byte[] sfile = File.ReadAllBytes(sapp);


            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@app", sfile);
            prms[1] = new SqlParameter("@str", Microsoft.VisualBasic.Interaction.InputBox("הקלד את שם הקובץ"));
            bool good = DBFunction.Execute("insert into otherfiles values(@app,@str)", prms);
            if (good)
                MessageBox.Show("קובץ נוסף בהצלחה", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btntemp_Click(object sender, EventArgs e)
        {

        }

        private void btnshortreports_Click(object sender, EventArgs e)
        {
            //   cmenu_report.Show(btnshortreports, new Point(0, btnshortreports.Height));
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MakeReport rp = new MakeReport(MakeReport.ReportType.Client);
            File.WriteAllText(Application.StartupPath + "\\sr.html", rp.CreateGeneralReport(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)));
            new Report((Application.StartupPath + "\\sr.html")).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartUp.FixMyBrowser();
        }

        private void tsmenu_showrweek_Click(object sender, EventArgs e)
        {
            MakeReport rp = new MakeReport(MakeReport.ReportType.Client);
            File.WriteAllText(Application.StartupPath + "\\sr.html", rp.CreateGeneralReport(DateTime.Now.AddDays(-7), DateTime.Now.AddDays(1)));
            new Report((Application.StartupPath + "\\sr.html")).ShowDialog();
        }

        private void tsmenu_showrmonth_Click(object sender, EventArgs e)
        {
            MakeReport rp = new MakeReport(MakeReport.ReportType.Client);
            File.WriteAllText(Application.StartupPath + "\\sr.html", rp.CreateGeneralReport(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(1)));
            new Report((Application.StartupPath + "\\sr.html")).ShowDialog();
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // TabPage page = tabControl.TabPages[e.Index];
            //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, 73, 97)), tabControl1.TabPages[e.Index].Bounds.X, tabControl1.TabPages[e.Index].Bounds.Y, tabControl1.TabPages[e.Index].Bounds.Width, tabControl1.TabPages[e.Index].Bounds.Height);
            TabPage page = tabControl1.TabPages[e.Index];
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, 73, 97)), e.Bounds);

            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
            TextRenderer.DrawText(e.Graphics, page.Text, page.Font, paddedBounds, page.ForeColor);
            // page.ImageKey=""
        }



        public void DiaryListResult(ref SqlDataReader reader)
        {
            List<ShiduchActivity> s = new List<ShiduchActivity>();
            ListViewItem item;
            olstActivityDiary.Items.Clear();
            olstActivityDiary.BeginUpdate();
            while (reader.Read())
            {
                ShiduchActivity sh = new ShiduchActivity();
                ShiduchActivity.readerToShiduchActivity(ref reader, ref sh);
                string nameA = reader["FullNameA"] != System.DBNull.Value ? (string)reader["FullNameA"] : "";
                string nameB = reader["FullNameB"] != System.DBNull.Value ? (string)reader["FullNameB"] : "";

                sh.FullNameA = nameA;
                sh.FullNameB = nameB;
                sh.ActionConvert = ShiduchActivity.ConvertAction((ShiduchActivity.ActionType)sh.Action, reader);
                sh.StatusConvert = ShiduchActivity.ConvertStatus((ShiduchActivity.ActionStatus)sh.Status);
                sh.UserName = (string)reader["Name"];
                item = new ListViewItem(new string[] {
                    sh.Date.ToShortDateString(),
                    sh.ActionConvert,
                    sh.FullNameA,
                    sh.FullNameB,
                    sh.StatusConvert,
                    sh.reminder.Date.ToShortDateString(),
                    sh.NotesSummary
                });
                item.Tag = sh.Id;
                s.Add(sh);

                olstActivityDiary.Items.Add(item);
            }
            olstActivityDiary.EndUpdate();
        }


        private void cmbActivityDairy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbActivityDairy.SelectedIndex == 5)
                cmbStatusDairy.Enabled = false;
            else
                cmbStatusDairy.Enabled = true;
        }
        private void LoadTab2(DateTime d1, DateTime d2)
        {
            SqlDataReader reader = ShiduchActivity.GetActivities(false, null, false, false, false, d1, d2, 0,
                GLOBALVARS.MyUser.ID, true, cmbActivityDairy.SelectedIndex, cmbStatusDairy.SelectedIndex);
            DiaryListResult(ref reader);
            reader.Close();
        }






        private void button1_Click(object sender, EventArgs e)
        {
            ManagementForm m = new ManagementForm();
            m.Show();
        }

        private void olstActivityDiary_DoubleClick(object sender, EventArgs e)
        {
            ShiduchActivity s = new ShiduchActivity();
            s.openShiduchActivityForm(olstActivityDiary);
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uNumericUpDown1_Load(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }


        private void txtid_Load(object sender, EventArgs e)
        {

        }

        private void uNumericUpDown2_Load(object sender, EventArgs e)
        {

        }

        private void tooltipinfo_Draw(object sender, DrawToolTipEventArgs e)
        {
            //  MessageBox.Show(e.AssociatedControl.Location.X);
            //  Point p = new Point(e.AssociatedControl.Location.X, e.AssociatedControl.Location.Y);
            Brush brsh = Brushes.Red;
            Font font = new Font("Arial", 12, FontStyle.Bold);
            Point p = new Point();
            Rectangle box = new Rectangle(new Point(0, 0), new Size(100, 250));

            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawRectangle(Pens.AliceBlue, box);
            e.DrawBorder();
            //   e.Graphics.DrawIcon(new Icon(Application.StartupPath + "\\logo.ico"),new Rectangle(new Point(1,1),new Size()));
            //   e.Graphics.DrawString("מידע", font, brsh, new PointF(40,0));
            //  e.Graphics.DrawString(e.ToolTipText, new Font("Arial", 12), br, new PointF(0,10));


            /*string[] spl = e.ToolTipText.Split('\n');
            foreach (string line in spl)
            {
                if (line.Length > len)
                    len = line.Length;
            }
            Rectangle rect = new Rectangle(len * 5 + 30, 0, 24, 24);
            e.Graphics.DrawIcon(new Icon(Application.StartupPath + "\\logo.ico"), rect);
            e.Graphics.DrawString(e.ToolTipText, new Font("Arial", 12), br, new PointF(20, 20));*/


        }

        private void btn_foundmatch_Click(object sender, EventArgs e)
        {
            MessageBox.Show("תחת בנייה", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (cmb_matchclient.SelectedIndex < 0) return;
            picmatch.Visible = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(RunMatchSearch), -1);

        }
        private bool IsVIPClient(string MONEYTOSHADCHAN)
        {
            decimal result = 0;
            decimal.TryParse(Regex.Match(MONEYTOSHADCHAN, @"\d+").Value, out result);
            if ((MONEYTOSHADCHAN.Contains("דול") || MONEYTOSHADCHAN.Contains("$")) && result > 1200)
                return true;
            else if (result > 4000) return true;

            return false;
        }
        private void olstpeople_FormatRow(object sender, FormatRowEventArgs e)
        {

            People p = (People)e.Model;
            switch (p.Show)
            {
                case (int)People.ShowTypes.HideDetails:
                    e.Item.BackColor = Color.LightGreen;
                    if (GLOBALVARS.MyUser.Control == User.TypeControl.User)
                    {
                        (e.Model as People).FirstName = "חסוי";
                        (e.Model as People).Lasname = "חסוי";
                    }
                    break;
                case (int)People.ShowTypes.HideFromUsers:
                    e.Item.BackColor = Color.LightBlue;
                    break;
                case (int)People.ShowTypes.VIP:
                    e.Item.BackColor = Color.Gold;
                    break;
                case (int)People.ShowTypes.Personal:
                    e.Item.BackColor = Color.LightGreen;
                    break;
            }
            //e.Item.SubItems[0].BackColor = Color.FromKnownColor(KnownColor.Gray);
            //e.Item.SubItems[0].Text = "  ";

            //e.Item.UseItemStyleForSubItems = false;

            //e.Item.BackColor = Color.GreenYellow;
        }
        private void olstpeople_DoubleClick(object sender, EventArgs e)
        {

            if (olstpeople.SelectedObject != null)
            {
                People p = olstpeople.SelectedObject as People;
                bool show = true;

                Log.AddAction(Log.ActionType.ClientOpen, new Log(Log.ActionType.ClientOpen,
                    p.FirstName + " " +
                    p.Lasname + "^" + p.ID.ToString()));
                ShiduchActivity.insertActivity(
                   new ShiduchActivity()
                   {
                       Action = (int)ShiduchActivity.ActionType.openForms,
                       Date = DateTime.Now,
                       PeopleId = p.ID,
                       UserId = GLOBALVARS.MyUser.ID,

                   });
                if (sender is bool && (bool)sender == false)
                    show = false;

                OpenDetails(p.ID, show);
            }
        }

        private void RunMatchSearch(object obj)
        {

            KeyValueClass id = new KeyValueClass();
            if ((int)obj > 0)
                id.Value = (int)obj;
            else
                id = cmb_matchclient.SelectedItem as KeyValueClass;
            SqlDataReader read = People.ReadById((int)id.Value);
            read.Read();
            People p = new People();
            PeopleManipulations.ReaderToPeople(ref p, ref read, false, true);
            read.Close();
            ArrayList results = MatchesChecks.GetMatches(p);
            olstmatch.SetObjects(results);
            olstmatch.Sort(olvColumn9);
            picmatch.Visible = false;
            lblmatchfound.Text = "נמצאו : " + olstmatch.Items.Count.ToString() + " התאמות";
        }

        private void olstpeople_ButtonClick(object sender, CellClickEventArgs e)
        {
            People p = e.Model as People;
            if (p == null) return;
            cmb_matchclient.Items.Add(new KeyValueClass(p.FirstName + " " + p.Lasname, p.ID));
            tabControl1.SelectedIndex = 3;
            //   tabControl1_Selected(sender,new TabControlEventArgs(tabPage9,3,TabControlAction.Selected));
            cmb_matchclient.Text = p.FirstName + " " + p.Lasname;
            picmatch.Visible = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(RunMatchSearch), p.ID);
        }

        private void olstmatch_Click(object sender, EventArgs e)
        {
            Match m = olstmatch.SelectedObject as Match;
            if (m != null)
                lblwhymatch.Text = MatchesChecks.CreateInfoString(m);
        }

        private void btmsample_Click(object sender, EventArgs e)
        {

        }

        private void lstReminder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }







    }
}
