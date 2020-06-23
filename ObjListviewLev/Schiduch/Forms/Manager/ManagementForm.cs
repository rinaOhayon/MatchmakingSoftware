using Schiduch.Classes.Program;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace Schiduch.Forms.Manager
{
    public partial class ManagementForm : Form
    {
        ListViewItemComparer lstlogsort;

        public ManagementForm()
        {
            InitializeComponent();
            lstlogsort = new ListViewItemComparer();
            lstlog.ListViewItemSorter = lstlogsort;
        }
        private void ManagementForm_Load(object sender, EventArgs e)
        {
            //טעינת שדכנים מטפלים לזמניים
            // TemptxtShadchanBy.Items.Clear();
            ((ListBox)TempPersonalShadchan).DataSource = null;
            for (int i = 0; i < GLOBALVARS.Shadchanim.Count; i++)
            {
                TemptxtShadchanBy.Items.Add(GLOBALVARS.Shadchanim[i]);
                cmbusers.Items.Add(GLOBALVARS.Shadchanim[i]);
                TempPersonalShadchan.Items.Add(
                    new KeyValueClass(GLOBALVARS.Users[i].Name + ", " + GLOBALVARS.Users[i].SectorView, GLOBALVARS.Users[i].ID));
            }
            dtlogto.Value = DateTime.Now.AddDays(1);


        }

        private void TempbtnLoad_Click(object sender, EventArgs e)
        {
            TempLoad();

        }
        private void TempLoad()
        {
            string sql = "";
            sql = " select firstname,lastname,sexs,age,City,Eda,Background,DadWork,Status, p.id," +
                "LearnStatus,Temp, pd.schools,MomWork,WorkPlace,r.ByUserName,r.RegDate"
             + " from peoples p inner join peopledetails pd on p.ID=pd.relatedid inner join RegisterInfo r on pd.relatedid=r.relatedid where Temp= 'true' ";
            SqlDataReader reader = DBFunction.ExecuteReader(sql);
            List<People> peoples = new List<People>();
            while (reader.Read())
            {
                People p = new People();
                PeopleManipulations.ConvertTempPeople(ref reader, ref p);
                peoples.Add(p);
            }
            TempLst.BeginUpdate();
            TempLst.SetObjects(peoples);
            TempLst.EndUpdate();
            reader.Close();
        }
        private void SearchForTemp()
        {
            string sql = "", AgeSql = "";
            SqlParameter[] prms = new SqlParameter[20];
            int fromage = (int)TemptxtAgeFrom.Value;
            int tillage = (int)TemptxtAgeTo.Value;
            if (tillage > 0)
                AgeSql = BuildSql.GetSql(out prms[12], fromage, "age", BuildSql.SqlKind.BETWEEN, true, tillage);

            sql +=
            BuildSql.GetSql(out prms[0], TemptxtFirstName.Text, "FirstName", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[1], TemptxtLastName.Text, "Lastname", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[2], TemptxtCity.Text, "City", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[3], (TemptxtSexs.SelectedIndex + 1), "sexs", BuildSql.SqlKind.EQUAL) +
            BuildSql.GetSql(out prms[4], TemptxtStatus.Text, "Status", BuildSql.SqlKind.EQUAL) +
            BuildSql.GetSql(out prms[5], TemptxtLearnStatus.Text, "LearnStatus", BuildSql.SqlKind.EQUAL) +
            BuildSql.GetSql(out prms[6], TemptxtEda.Text, "eda", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[7], TemptxtBackground.Text, "background", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[8], TemptxtSchools.Text, "schools", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[9], TemptxtDadWork.Text, "dadwork", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[10], TemptxtMomWork.Text, "MomWork", BuildSql.SqlKind.LIKE) +
            BuildSql.GetSql(out prms[11], TemptxtWorkPlace.Text, "WorkPlace", BuildSql.SqlKind.LIKE) +
            AgeSql;


        }
        /// <summary>
        /// ok that those shadchanim got this temp people to their own db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempbtnPersonalOK_Click(object sender, EventArgs e)
        {
            if (TempPersonalShadchan.CheckedItems.Count <= 0)
            {
                MessageBox.Show("לא נבחרו שדכנים", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ShadchanHandler = "";
            foreach (int item in TempPersonalShadchan.CheckedIndices)
            {
                if (ShadchanHandler.Length > 0)
                    ShadchanHandler += ",";
                ShadchanHandler += "{" + GLOBALVARS.Users[item].ID + "}";
            }
            personal = true;
            DOTemp();
        }
        /// <summary>
        /// cancal the action of personal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempbtnPersonalCancel_Click(object sender, EventArgs e)
        {
            personal = general = false;

            TempPersonalShadchan.Visible = false;
            TempbtnPersonalCancel.Visible = TempbtnPersonalOK.Visible = false;
            foreach (int item in TempPersonalShadchan.CheckedIndices)
            {
                TempPersonalShadchan.SetItemCheckState(item, CheckState.Unchecked);
            }
        }

        /// <summary>
        /// add this temp people to private db of one or more shadchanim
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempbtnAddPersonal_Click(object sender, EventArgs e)
        {
            if (TempPersonalShadchan.Visible)
            {
                TempPersonalShadchan.Visible = false;
                TempbtnPersonalCancel.Visible = TempbtnPersonalOK.Visible = false;
                return;
            }
            TempPersonalShadchan.Visible = true;
            TempbtnPersonalCancel.Visible = TempbtnPersonalOK.Visible = true;
        }
        /// <summary>
        /// add this temp people to general database of lev1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempbtnAddGeneral_Click(object sender, EventArgs e)
        {
            general = true;
            DOTemp();
        }

        bool general, personal;
        string ShadchanHandler = "";
        private bool Tab1;

        /// <summary>
        /// do the update statement
        /// </summary>
        private void DOTemp()
        {
            if (TempLst.CheckedItems.Count <= 0)
            {
                MessageBox.Show("לא נבחרו שורות", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                general = personal = false;
                return;
            }
            string sql = "", AllSql = "";
            sql = "update peoples set Temp='false'";
            if (personal)
            {
                sql += ", Show=5,Chadchan='" + ShadchanHandler + "'";
            }
            sql += " where ID=";
            int i = TempLst.CheckedObjects.Count - 1;
            int nChecked = -1;
            //loop for creta string of all updates with length untill 7500 chars 
            //to avoid exception of 'index out of range'
            while (i > nChecked)
            {
                for (; i > nChecked && AllSql.Length < 7450; i--)
                {
                    People p = TempLst.CheckedObjects[i] as People;
                    AllSql += sql + p.ID.ToString() + "; ";
                    TempLst.RemoveObject(TempLst.CheckedObjects[i]);
                }
                bool succees = DBFunction.Execute(AllSql);
                AllSql = "";
            }
            TempbtnPersonalCancel_Click(new object(), new EventArgs());
        }

        private void btnloadlog_Click(object sender, EventArgs e)
        {
            SqlParameter date1 = new SqlParameter("@date1", dtlogfrom.Value);
            SqlParameter date2 = new SqlParameter("@date2", dtlogto.Value);

            SqlParameter[] prms = new SqlParameter[3];
            prms[1] = date1;
            prms[2] = date2;
            string sql = "SELECT Name,ID FROM USERS WHERE  NOT EXISTS (SELECT * FROM LOG WHERE  users.id = userid and";
            sql += " date between @date1 AND @date2)";
            SqlDataReader reader = DBFunction.ExecuteReader(sql, prms);
            lstlog.Items.Clear();
            while (reader.Read())
            {
                lstlog.Items.Add(new ListViewItem(new string[] {
                    reader["ID"].ToString(),
                    reader["Name"].ToString(),
            }, 3));
            }
            reader.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string name = "דוח ";
            if (cmbusers.SelectedIndex != -1)
                name += cmbusers.Text;
            else
                name += "כל השדכנים";
            if (cmbaction.SelectedIndex != -1)
                name += " " + cmbaction.Text;
            else
                name += " כל הפעולות";
            name += " __ " + DateTime.Now
                .ToString("dd.MM.yyyy hh-mm", System.Globalization.CultureInfo.InvariantCulture);


            ListView.ListViewItemCollection l = lstlog.Items;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel לא מותקן");
                return;
            }

            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            //Excel.Range oRng;
            //oRng = xlWorkSheet.get_Range("C2", "C6");
            //oRng.Formula = "=A2 & \" \" & B2";
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //שורה ראשונה - כותרות
            int i = 1;
            for (i = 1; i < lstlog.Columns.Count; i++)
            {
                xlWorkSheet.Cells[1, i] = lstlog.Columns[i - 1].Text;
            }

            for (i = 2; i < lstlog.Items.Count + 2; i++)
            {
                xlWorkSheet.Cells[i, 1] = lstlog.Items[i - 2].Text;

                for (int j = 2; j < lstlog.Columns.Count; j++)
                {
                    xlWorkSheet.Cells[i, j] = lstlog.Items[i - 2].SubItems[j - 1].Text;
                    //item.SubItems[j-1].Text;
                }
                // string f = l[0].SubItems[0].Text;

            }
            xlWorkSheet.Columns.AutoFit();
            try
            {
                xlWorkBook.SaveAs(path + "\\" + name + ".xlsx", Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            }
            catch (Exception)
            {
                MessageBox.Show("שמירת הקובץ לא הצליחה");
                return;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(xlWorkSheet);

            xlWorkBook.Close(true, misValue, misValue);
            Marshal.ReleaseComObject(xlWorkBook);

            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
            MessageBox.Show("הקובץ נוצר בהצלחה, תוכלו למצוא אותו ב: " + path + "\\" + name + ".xlsx");
            string fileExcel;
            fileExcel = path + "\\" + name + ".xlsx";
            //Excel.Application xlapp;
            //Excel.Workbook xlworkbook;
            //xlapp = new Excel.Application();

            //xlworkbook = xlapp.Workbooks.Open(fileExcel, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            //xlapp.Visible = true;
            Process.Start(fileExcel);
        }

        private void btn_lastday_Click(object sender, EventArgs e)
        {

            dtlogto.Value = DateTime.Now;
            switch ((string)((Button)sender).Tag)
            {
                case "D":
                    dtlogfrom.Value = DateTime.Now.AddDays(-1);
                    break;
                case "W":
                    dtlogfrom.Value = DateTime.Now.AddDays(-7);
                    break;
                case "M":
                    dtlogfrom.Value = DateTime.Now.AddDays(-30);
                    break;
                //todo לשנות לתאריך ההפצה של התוכנה
                case "E":
                    dtlogfrom.Value = DateTime.Now.AddYears(-2);
                    break;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dtlogto.Value = DateTime.Now;
            switch ((string)((Button)sender).Tag)
            {
                case "D":
                    dtlogfrom.Value = DateTime.Now.AddDays(-1);
                    break;
                case "W":
                    dtlogfrom.Value = DateTime.Now.AddDays(-7);
                    break;
                case "M":
                    dtlogfrom.Value = DateTime.Now.AddDays(-30);
                    break;
                //todo לשנות לתאריך ההפצה של התוכנה
                case "E":
                    dtlogfrom.Value = DateTime.Now.AddYears(-2);
                    break;
            }
        }

        private void btn_createreport_Click(object sender, EventArgs e)
        {
            string path = "";
            MakeReport report;
            switch (cmb_reporttype.SelectedIndex)
            {
                case 0:
                    report = new MakeReport(MakeReport.ReportType.Client);
                    path = report.CreateClientReport(((KeyValueClass)cmb_subreport.SelectedItem).Text, (int)((KeyValueClass)cmb_subreport.SelectedItem).Value, dt_reportfrom.Value, dt_reportto.Value);
                    webBrowser1.Navigate(path);
                    break;
                case 1:
                    report = new MakeReport(MakeReport.ReportType.User);
                    path = report.CreateUserReport(((KeyValueClass)cmb_subreport.SelectedItem).Text, (int)((KeyValueClass)cmb_subreport.SelectedItem).Value, dt_reportfrom.Value, dt_reportto.Value);
                    webBrowser1.Navigate(path);
                    break;
                    //case 4:
                    //    report = new MakeReport(MakeReport.ReportType.Dates);
                    //    path = report.CreateDatesReport(dt_reportfrom.Value, dt_reportto.Value);
                    //    webBrowser1.Navigate(path);
                    //    break;
            }
        }

        private void btnrecycling_Click(object sender, EventArgs e)
        {
            if (lsttrash.SelectedItems.Count <= 0)
            {
                MessageBox.Show("צריך לבחור שורה בטבלה");
                return;
            }
            DialogResult result = MessageBox.Show("האם אתה בטוח שברצונך לשחזר?", "שחזור הכרטיס", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return;
            int id = int.Parse(lsttrash.SelectedItems[0].SubItems[5].Text);
            string sql = "update peoples set show=";
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
            if (p.Chadchan.Length > 0)
            {
                sql += "5";
            }
            else
                sql += "0";
            sql += " where ID=" + id;
            if (DBFunction.Execute(sql))
                MessageBox.Show("השחזור בוצע בהצלחה");
            else
                MessageBox.Show("", "קרתה שגיאה בעת השחזור. נא נסו שוב במועד מיוחר יותר", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LoadTab10();
            ShiduchActivity.insertActivity(
                            new ShiduchActivity()
                            {
                                Action = (int)ShiduchActivity.ActionType.recycling,
                                Date = DateTime.Now,
                                PeopleId = id,
                                UserId = GLOBALVARS.MyUser.ID,
                            });
        }



        private void btnDelForever_Click(object sender, EventArgs e)
        {
            MessageBox.Show("כרגע לא ניתן למחוק. רינה");
            return;
            //if (lsttrash.SelectedItems.Count <= 0)
            //{
            //    MessageBox.Show("צריך לבחור שורה בטבלה");
            //    return;
            //}
            //DialogResult result = MessageBox.Show(" האם אתה בטוח שברצונך למחוק את הכרטיס? אם תאשר את המחיקה לא תוכל לשחזר את הכרטיס לצמיתות", "שחזור הכרטיס", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //if (result == DialogResult.Cancel) return;
            //int id = int.Parse(lsttrash.SelectedItems[0].SubItems[5].Text);

            //People.DeletePeople(id, false, true);
            //LoadTab10();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPage4)
            {
                LoadTab10();
            }

            //else if(e.TabPage==tabPage1)
            //{
            //    TemptxtShadchanBy.Items.Clear();
            //    ((ListBox)TempPersonalShadchan).DataSource = null;
            //    for (int i = 0; i < GLOBALVARS.Shadchanim.Count; i++)
            //    {
            //        TemptxtShadchanBy.Items.Add(GLOBALVARS.Shadchanim[i]);
            //        TempPersonalShadchan.Items.Add(
            //            new KeyValueClass(GLOBALVARS.Users[i].Name + ", " + GLOBALVARS.Users[i].SectorView, GLOBALVARS.Users[i].ID));
            //    }
            //}

        }
        private void LoadTab10()
        {
            People p = new People();
            SqlDataReader reader;
            reader = DBFunction.ExecuteReader("select p.id,p.firstname,p.lastname,p.city,p.DeleteReason,pd.YeshivaGorSeminary,pd.KibutzorMaslul  from peoples p inner join peopledetails pd on p.ID=pd.relatedid where show=8");
            lsttrash.Items.Clear();
            lsttrash.BeginUpdate();
            while (reader.Read())
            {
                ListViewItem item = new ListViewItem(new string[] {
                        reader["firstname"].ToString(),
                        reader["lastname"].ToString(),

                        reader["city"].ToString(),
                        reader["YeshivaGorSeminary"].ToString()+","+ reader["KibutzorMaslul"].ToString(),
                        reader["DeleteReason"].ToString(),
                        reader["id"].ToString()
                    }, 9);
                item.Tag = reader["id"].ToString();
                lsttrash.Items.Add(item);
            }
            lsttrash.EndUpdate();
            reader.Close();
        }

        private void btnOpenDetails_Click(object sender, EventArgs e)
        {
            if (lsttrash.SelectedItems.Count <= 0)
            {
                MessageBox.Show("צריך לבחור שורה בטבלה");
                return;
            }
            int id = int.Parse(lsttrash.SelectedItems[0].SubItems[5].Text);
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
            p.OpenForTrashPeople = true;
            DetailForm detail = new DetailForm(p);
            detail.Show();
            ShiduchActivity.insertActivity(
                            new ShiduchActivity()
                            {
                                Action = (int)ShiduchActivity.ActionType.openForms,
                                Date = DateTime.Now,
                                PeopleId = id,
                                UserId = GLOBALVARS.MyUser.ID,
                            });
        }

        private void cmb_reporttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_subreport.Text = "";
            switch (cmb_reporttype.SelectedIndex)
            {
                case 0:
                    LoadClients();
                    break;
                case 1:
                    cmb_subreport.Items.Clear();
                    //cmbusers.Items.Add(new KeyValueClass("הכל", 0));
                    foreach (object item in cmbusers.Items) { cmb_subreport.Items.Add(item); }
                    break;
                case 4:
                    cmb_subreport.Items.Clear();
                    //cmbusers.Items.Add(new KeyValueClass("הכל", 0));
                    GLOBALLABELS.LoadGeneralReports(cmb_subreport);
                    break;
            }
        }
        public void LoadClients()
        {
            string sql;
            GLOBALVARS.Clients = new ArrayList();
            sql = "select firstname + ' ' + lastname as allname,id from peoples where show=0 order by firstname ";
            SqlDataReader reader;
            reader = DBFunction.ExecuteReader(sql);
            cmb_subreport.Items.Clear();
            while (reader != null && reader.Read())
            {
                KeyValueClass temp = new KeyValueClass((string)reader["allname"], (int)reader["ID"]);
                GLOBALVARS.Clients.Add(temp);
                cmb_subreport.Items.Add(temp);
            }
            reader.Close();
        }

        private void txttrashfname_TextChanged(object sender, EventArgs e)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(txttrashfname.Text) || !string.IsNullOrEmpty(txttrashlname.Text))
            {
                for (int i = 0; i < lsttrash.Items.Count; i++)
                {
                    if (lsttrash.Items[i].Text.Contains(txttrashfname.Text) && lsttrash.Items[i].SubItems[1].Text.Contains(txttrashlname.Text))
                    {
                        lsttrash.Items[i].BackColor = Color.Red;
                        count++;
                    }
                    else
                        lsttrash.Items[i].BackColor = Color.White;
                }
                lbl_trashfilterresult.Text = "נמצאו : " + count + " תוצאות";
            }
            else
            {
                lbl_trashfilterresult.Text = "";
                for (int i = 0; i < lsttrash.Items.Count; i++)
                    lsttrash.Items[i].BackColor = Color.White;
            }
        }

        private void lstlog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lstlogsort.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lstlogsort.Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    lstlogsort.Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lstlogsort.Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lstlogsort.SortColumn = e.Column;
                lstlogsort.Order = System.Windows.Forms.SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lstlog.Sort();
            //lstlog.ListViewItemSorter = new ListViewItemComparer(e.Column);
            //if (isAscendingLstLog)
            //    lstlog.Sorting = System.Windows.Forms.SortOrder.Descending;
            //else
            //    lstlog.Sorting = System.Windows.Forms.SortOrder.Ascending;
            //isAscendingLstLog = !isAscendingLstLog;
            //lstlog.Sort();
        }

        private void cmbStatusDairy_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeDisable();

        }
        private void MakeEnable()
        {
            btnDiaryExportAll.Enabled = btnDiaryExportSum.Enabled = btnDiarySum.Enabled = true;
        }
        private void MakeDisable()
        {
            btnDiaryExportAll.Enabled = btnDiaryExportSum.Enabled = btnDiarySum.Enabled = false;
        }

        private void cmbReportDairy_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbCustomer.Items.Clear();
            cmbCustomer.Text = "";
            if (cmbReportDairy.SelectedIndex == 0)
            {
                cmbCustomer.Items.AddRange(GLOBALVARS.Shadchanim.ToArray());
            }
            else if (cmbReportDairy.SelectedIndex == 1)
            {
                cmbCustomer.Items.Add("טען אנשים");
            }
            MakeDisable();
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedIndex == 0 && cmbReportDairy.SelectedIndex == 1)
            {
                cmbCustomer.Items.Clear();
                cmbCustomer.Items.Add("טען אנשים");
                if (GLOBALVARS.Clients == null)
                    LoadClients();
                foreach (var item in GLOBALVARS.Clients)
                {
                    cmbCustomer.Items.Add(item);
                }
                cmbCustomer.EndUpdate();
            }
            MakeDisable();

        }

        private void btnDiaryManagerOK_Click(object sender, EventArgs e)
        {
            SqlDataReader reader = null;
            MakeEnable();
            //אם לא בחרו שדכן או לקוח התוצאה היא אותו דבר ומשתמשים בדוחות למנהל 
            if (cmbCustomer.SelectedIndex == -1 || cmbCustomer.SelectedIndex == 0)
            {
                reader = ShiduchActivity.GetActivities(false, null, false, false, true, dtActivityDatefrom.Value, dtActivityDateto.Value,
                    0, 0, false, cmbActivityDairy.SelectedIndex, cmbStatusDairy.SelectedIndex, true);
            }
            //אם זה דוח לשדכן ספציפי אז אפשר להשתמש ביומן
            else if (cmbReportDairy.SelectedIndex == 0 && cmbCustomer.SelectedIndex != -1)
            {
                int idUser = int.Parse((cmbCustomer.SelectedItem as KeyValueClass).Value.ToString());

                reader = ShiduchActivity.GetActivities(false, null, false, false, false, dtActivityDatefrom.Value
                      , dtActivityDateto.Value, 0, idUser, true, cmbActivityDairy.SelectedIndex,
                      cmbStatusDairy.SelectedIndex, true);
            }
            //אם בחרו לקוח ספציפי אפשר להשתמש בדוחות למנהל
            else if (cmbReportDairy.SelectedIndex == 1 && cmbCustomer.SelectedIndex > 0)
            {
                int idPeople = int.Parse((cmbCustomer.SelectedItem as KeyValueClass).Value.ToString());

                reader = ShiduchActivity.GetActivities(false, null, false, false, true, dtActivityDatefrom.Value,
                    dtActivityDateto.Value, idPeople);
            }

            DiaryListResult(ref reader);
            reader.Close();

        }
        public void DiaryListResult(ref SqlDataReader reader)
        {
            List<ShiduchActivity> s = new List<ShiduchActivity>();
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
                s.Add(sh);

            }
            olstActivityDiary.SetObjects(s);
            olstActivityDiary.EndUpdate();
        }

        private void cmbActivityDairy_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeDisable();
            if (cmbActivityDairy.SelectedIndex == 5)
                cmbStatusDairy.Enabled = false;
            else
                cmbStatusDairy.Enabled = true;
        }

        private void olstActivityDiary_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            ShiduchActivity s = (ShiduchActivity)e.Model;
            if (s.HideDelete)
                e.Item.BackColor = Color.Gray;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dtActivityDateto.Value = DateTime.Now;
            switch ((string)((Button)sender).Tag)
            {
                case "D":
                    dtActivityDatefrom.Value = DateTime.Now.AddDays(-1);
                    break;
                case "W":
                    dtActivityDatefrom.Value = DateTime.Now.AddDays(-7);
                    break;
                case "M":
                    dtActivityDatefrom.Value = DateTime.Now.AddDays(-30);
                    break;
                //todo לשנות לתאריך ההפצה של התוכנה
                case "E":
                    dtActivityDatefrom.Value = DateTime.Now.AddYears(-2);
                    break;
            }
        }
        private void btnDiarySum_Click(object sender, EventArgs e)
        {
            
        }
        private void createCountTable()
        {

        }
        private object GroupKeyGetter(object rowObject)
        {
            olstActivityDiary.ShowGroups = true;
            olstActivityDiary.AlwaysGroupByColumn = olvColumn5;
            //olvColumn5.GroupKeyGetter = GroupKeyGetter;
            olstActivityDiary.RebuildColumns();
            olstActivityDiary.ShowItemCountOnGroups = true;            //כדי לכתוב בשורת קיבוץ משהו אחר
            var o = rowObject as ShiduchActivity;
            if (o == null)
                return "unknown";
            return o.Action;
        }
        private void btnlogfilter_Click(object sender, EventArgs e)
        {
            string mngrtoo = "";
            int login = 0, clientsopen = 0;
            SqlParameter date1 = new SqlParameter("@date1", dtlogfrom.Value);
            SqlParameter date2 = new SqlParameter("@date2", dtlogto.Value);
            int loguserid;
            string actionsql = "";
            SqlParameter[] prms = new SqlParameter[1];
            if (cmbusers.SelectedItem is KeyValueClass || cmbusers.SelectedIndex > 0)
                loguserid = (int)(cmbusers.SelectedItem as KeyValueClass).Value;
            else
                loguserid = -1;
            if (cmbaction.SelectedIndex != -1 && cmbaction.SelectedIndex != 8)
                actionsql = " ACTION=" + cmbaction.SelectedIndex + " AND ";
            string info = "";
            string sql = "select users.name,users.id,info,action,userid,date,level,dateexit from log l inner join users on l.userid=USERS.ID  where ";
            if (loguserid != -1)
                sql += BuildSql.GetSql(out prms[0], loguserid, "UserId", BuildSql.SqlKind.EQUAL);
            sql += actionsql;
            //if (!chk_mangertoo.Checked)
            //    mngrtoo = "and userid <> 1 and userid <> 38 and userid <> 54";
            sql += " date between @date1 AND @date2 " + mngrtoo + " order by date desc";
            SqlDataReader reader = Log.ReadSql(sql, date1, date2, prms[0]);
            Log temp = new Log();
            lstlog.Items.Clear();
            lstlog.BeginUpdate();
            TimeSpan ts;
            decimal SumTime = 0;
            while (reader.Read())
            {

                Log.ReaderToLog(ref reader, ref temp);
                if (temp.Info != null)
                    info = temp.Info.Replace('^', ' ');
                //todo לחשב את הזמן שאחרי הנקודה לפי שעות כלומר חלקי 60
                ts = temp.DateExit.Subtract(temp.Date);
                string name = reader["name"].ToString();
                lstlog.Items.Add(new ListViewItem(new string[] {
                    reader["ID"].ToString(),
                    reader["name"].ToString(),
                    temp.ActionString,
                    temp.Date.ToString(),
                    temp.DateExit.ToString(),
                   Math.Floor( ts.TotalHours)+"."+ts.Minutes,
                    info
            }, 3));
                switch (temp.Command)
                {
                    case Log.ActionType.ClientOpen:
                        clientsopen++;
                        break;
                    case Log.ActionType.Login:
                        login++;
                        SumTime += decimal.Parse(ts.TotalHours.ToString());
                        break;
                }
            }
            int sum = Convert.ToInt32(Math.Floor(SumTime));
            float t6 = float.Parse((SumTime - Math.Truncate(SumTime)).ToString());
            if (t6 > 0.5)
                sum = Convert.ToInt32(Math.Ceiling(SumTime));
            lblreportstatus.Text = "סך הכל כניסות לתוכנה : " + login + "\nסך הכל פתיחת תיקי לקוחות : " + clientsopen + "\nסך עבודה בשעות על התוכנה: " + sum;
            reader.Close();
            lstlog.EndUpdate();
        }

    }
}
