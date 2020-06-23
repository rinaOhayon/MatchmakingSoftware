using Schiduch.Classes.Program;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schiduch
{
    public partial class ShiduchActivityForm : Form
    {
        public bool newActivity, updateActivity;
        public ArrayList Shadchanim;
        public People Shiduch;//הצד השני בשידוך
        public ShiduchActivity ShiduchActivity;// הפעילות החדשה או המעודכנת שמגיעה
        public ShiduchActivity Activity;//הפעילות שמוסיפים לטבלה, נוצרת מכל השדות בטופס
        public People MyPeople { get; set; }//הבן אדם שדרכו פתחו את הפעילות כלומר צד א'
        public bool save { get; set; } = false;//אם שמרו את הפעילות ואז צריך לטעון מחדש את הטבלה
        public bool OpenSideB { get; set; } = false;//אם רוצים לפתוח את הפעילות של הצד השני
        public bool thisSideB { get; set; } = false;//אם זה פעילות חדשה שנפתחה דרך צד א בכפתור פתח פעילות של צד ב
        public bool OpenReminder { get; set; }
        public bool isNew_Active_From_Complete_Active { get; set; }
        public ShiduchActivityForm(ShiduchActivity Activity, People p, bool newActiv = true, bool updateActiv = false, bool SideB = false, bool openReminder = false, bool newActivFrom = false)
        {
            InitializeComponent();
            newActivity = newActiv;
            updateActivity = updateActiv;
            ShiduchActivity = Activity;
            MyPeople = p;
            thisSideB = SideB;
            OpenReminder = openReminder;
        }
        private void ShiduchActivityForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label1;
            //אם זה ברור או אחר אז המספור של השידוך 
            //שווה למינוס אחד וצריך לבדוק את זה ולא להתייחס לצד ב
            if (newActivity)
            {
                //GLOBALVARS.Activity = new ShiduchActivity();
                //GLOBALVARS.Activity.Date = DateTime.Now;
                //GLOBALVARS.Activity.UserId = GLOBALVARS.MyUser.ID;
                //GLOBALVARS.Activity.PeopleId = GLOBALVARS.MyPeople.ID;
                //GLOBALVARS.Activity.IdSideB = Shiduch.ID;
                //GLOBALVARS.Activity.Action = (int)ShiduchActivity.ActionStatus.inCare;

                //GLOBALVARS.Activity.Id = ShiduchActivity.insertActivity();
            }
            else if (updateActivity)
            {

            }
            LoadTxt();
            LoadShadcanim();
        }
        private void LoadTxt()
        {
            groupBoxSideA.Text += MyPeople.FirstName + " " + MyPeople.Lasname;
            if (MyPeople.Sexs == 2)
            {
                lblSearchShiduch.Visible = false;
            }
            lblActiveFor.Text += MyPeople.FirstName + " " + MyPeople.Lasname;
            //if (lblActiveFor.Location.X + lblActiveFor.Width > btnSaveOpenB.Location.X)
            //{
            //    lblActiveFor.Location = new Point(btnSaveOpenB.Location.X - lblActiveFor.Width - 20, lblActiveFor.Location.Y);
            //}
            txtName.Text = MyPeople.FirstName + " " + MyPeople.Lasname;
            txtAge.Value = decimal.Parse(MyPeople.Age.ToString());
            txtSchool.Text = MyPeople.Details.YeshivaGorSeminary;
            txtAdress.Text = MyPeople.Details.Street + " " + MyPeople.City;
            txtWork.Text = MyPeople.WorkPlace;
            txtPhoneDad.Text = MyPeople.Details.Tel1;
            txtPhoneMom.Text = MyPeople.Details.Tel2;
            txtPhone.Text = MyPeople.Details.Telephone;

            txtDate.Text = DateTime.Now.ToString();
            if (newActivity && ShiduchActivity.IdSideB == 0)
            {
                LoadSearchShiduch();
            }
            // if (updateActivity || ShiduchActivity.IdSideB > 0)

            //אם הפעילות נוצרה עם צד' ב
            if (ShiduchActivity.IdSideB != -1 && ShiduchActivity.IdSideB != 0)
            {
                SqlDataReader reader = People.ReadById(ShiduchActivity.IdSideB);
                if (reader.Read())
                {
                    Shiduch = new People();
                    PeopleManipulations.ReaderToPeople(ref Shiduch, ref reader, true, true);
                }
                reader.Close();
                KeyValueClass item = new KeyValueClass();
                item.Text = Shiduch.FirstName + " " + Shiduch.Lasname + " " + Shiduch.Details.YeshivaGorSeminary
                    + " " + Shiduch.Details.Street + " " + Shiduch.City;
                item.Value = Shiduch.ID.ToString();
                txtSearchShiduch.Items.Add(item);
                txtSearchShiduch.SelectedIndex = 0;
                //שיהיה אותה הפעילות לשני הצדדים ולא לאפשר לשנות
                if (thisSideB || updateActivity)
                {
                    radDate.Checked = ShiduchActivity.Action == (int)ShiduchActivity.ActionType.date;
                    radProposal.Checked = ShiduchActivity.Action == (int)ShiduchActivity.ActionType.proposal;
                    radProposal.Enabled = radDate.Enabled = radOther.Enabled = radDetails.Enabled = txtOther.Enabled = false;
                    txtNotesSummary.Text = ShiduchActivity.NotesSummary;
                }
            }
            if (updateActivity)
            {
                Text = "פעילות " + ShiduchActivity.ConvertAction((ShiduchActivity.ActionType)ShiduchActivity.Action) + " ל";
                txtDate.Text = ShiduchActivity.Date.ToString();
                txtDateReminder.Value = ShiduchActivity.reminder.Date;
                radDate.Checked = ShiduchActivity.Action == (int)ShiduchActivity.ActionType.date;
                radProposal.Checked = ShiduchActivity.Action == (int)ShiduchActivity.ActionType.proposal;
                radDetails.Checked = ShiduchActivity.Action == (int)ShiduchActivity.ActionType.details;
                radOther.Checked = ShiduchActivity.Action == (int)ShiduchActivity.ActionType.other;
                if (radOther.Checked)
                {
                    string text = ShiduchActivity.NotesSummary.Substring(0, ShiduchActivity.NotesSummary.IndexOf('^'));
                    string t = ShiduchActivity.NotesSummary.Substring(ShiduchActivity.NotesSummary.IndexOf('^') + 3);
                    txtOther.Text = text;
                    txtNotesSummary.Text = t;
                }
                else
                    txtNotesSummary.Text = ShiduchActivity.NotesSummary;
                radComplete.Checked = ShiduchActivity.Status == 1;
                radNoRelevant.Checked = ShiduchActivity.Status == 2;
                radProposal.Enabled = radDate.Enabled = radOther.Enabled = radDetails.Enabled = txtOther.Enabled = false;
                //אחרי שהפעילות נוצרה רק המנהל הראשי יכול לשנות אצ בטיפול של
                if (GLOBALVARS.MyUser.Control != User.TypeControl.Admin)
                    txtReminderInCare.Enabled = false;
            }
            Text += MyPeople.FirstName + " " + MyPeople.Lasname;

        }
        private void LoadSearchShiduch()
        {

            SqlDataReader reader = People.ReadAll(0, false, false, false, false, true, MyPeople);
            KeyValueClass item;
            item = new KeyValueClass();
            item.Text = "";
            item.Value = "-1";
            txtSearchShiduch.Items.Add(item);
            while (reader.Read())
            {
                item = new KeyValueClass();
                string text = (string)reader["FirstName"] + " " + (string)reader["LastName"];
                text += " " + reader["YeshivaGorSeminary"].ToString();
                text += " " + reader["City"].ToString();
                text += " " + reader["Street"].ToString();
                item.Text = text;
                item.Value = reader["ID"].ToString();
                txtSearchShiduch.Items.Add(item);
            }
            reader.Close();

            //else
            //    lblSearchShiduch.Visible = true;
        }
        public void LoadShadcanim()
        {
            string sql, where;
            int controlhide = 1, index = -1;
            Shadchanim = new ArrayList();
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin || GLOBALVARS.MyUser.Control == User.TypeControl.Manger)
                where = "";
            else if (thisSideB)
                where = " where id=" + ShiduchActivity.UserId;
            else
                where = "where id=" + GLOBALVARS.MyUser.ID;
            sql = "select name,id from users " + where;
            SqlDataReader reader = DBFunction.ExecuteReader(sql);
            while (reader != null && reader.Read())
            {
                KeyValueClass temp = new KeyValueClass((string)reader["name"], (int)reader["ID"]);
                if (int.Parse(temp.Value.ToString()) == GLOBALVARS.MyUser.ID)
                {
                    if (index < 0)
                        index = txtReminderInCare.Items.Count;
                    temp.Text = "שלי";
                }
                //אם הטיפול הוא אצל מישהו אחר ממני
                if (updateActivity && int.Parse(temp.Value.ToString()) == ShiduchActivity.reminder.IdUser)
                    index = txtReminderInCare.Items.Count;
                Shadchanim.Add(temp);
                txtReminderInCare.Items.Add(temp);
            }
            reader.Close();
            DBFunction.CloseConnections();
            txtReminderInCare.SelectedIndex = index;
        }
        private void txtSearchShiduch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtSearchShiduch.SelectedIndex > 0 || updateActivity || thisSideB || isNew_Active_From_Complete_Active)
            {
                if (newActivity && !thisSideB && !isNew_Active_From_Complete_Active)
                {
                    int id = int.Parse((txtSearchShiduch.SelectedItem as KeyValueClass).Value.ToString());
                    SqlDataReader reader = People.ReadById(id);
                    while (reader.Read())
                    {
                        Shiduch = new People();
                        PeopleManipulations.ReaderToPeople(ref Shiduch, ref reader, true, true);
                    }
                    reader.Close();
                }
                txtNameShiduch.Text = Shiduch.FirstName + " " + Shiduch.Lasname;
                txtAgeShiduch.Text = Shiduch.Age.ToString();
                txtSchoolShiduch.Text = Shiduch.Details.YeshivaGorSeminary;
                txtAdressShiduch.Text = Shiduch.Details.Street + " " + Shiduch.City;
                txtWorkShiduch.Text = Shiduch.WorkPlace;
                btnDetailsShiduch.Enabled = true;
                groupBoxSideB.Text = "צד ב - " + Shiduch.FirstName + " " + Shiduch.Lasname;
            }
            else
            {
                btnDetailsShiduch.Enabled = false;
                txtNameShiduch.Text = "";
                txtAgeShiduch.Value = 0;
                txtSchoolShiduch.Text = "";
                txtAdressShiduch.Text = "";
                txtWorkShiduch.Text = "";
                Shiduch = null;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            //או שלא בחרו עדיין שידוך
            //או שזה פעילות חדשה ובחרו את אינדקס אפס שזה ריק
            //
            if ((sender as Button) == btnSaveOpenB && (
                txtSearchShiduch.SelectedIndex < 0 || txtSearchShiduch.SelectedIndex == 0 && newActivity && !thisSideB))
            {
                //אם רוצים לפתוח פעילות כאשר הצד השני לא נבחר אז לא לתת
                // את זה כי זה יכול לעשות הרבה בעיות
                MessageBox.Show("לא נבחר צד ב'");
            }
            else if (radOther.Checked && txtOther.Text.Length <= 0)
            {
                MessageBox.Show("נא לפרט על אופי הפעילות", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOther.Focus();
            }
            else if (!radDate.Checked && !radProposal.Checked && !radOther.Checked && !radDetails.Checked)
            {
                MessageBox.Show("נא לבחור פעולה", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                radProposal.Focus();
            }
            else
            {
                Activity = new ShiduchActivity();
                Activity.Date = DateTime.Parse(txtDate.Text);
                Activity.PeopleId = MyPeople.ID;
                Activity.Action = radProposal.Checked ? 0
                    : radDate.Checked ? 1 : radDetails.Checked ? 2 : 3;
                if (Shiduch != null && (txtSearchShiduch.SelectedIndex > 0 || updateActivity || thisSideB || isNew_Active_From_Complete_Active))
                    Activity.IdSideB = Shiduch.ID;
                else if (radDetails.Checked || radOther.Checked) //אם זה רק ברור אז אין צורך בצד ב'
                    Activity.IdSideB = -1;
                else
                {
                    MessageBox.Show("נא לבחור את ההצעה", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSearchShiduch.Focus();
                    return;
                }
                Activity.Status = radIncare.Checked ? 0
                     : radComplete.Checked ? 1 : 2;
                Activity.NotesSummary = txtNotesSummary.Text;
                if (Activity.Action == 3)
                    Activity.NotesSummary = txtOther.Text + "^^^" + txtNotesSummary.Text;
                Activity.reminder.IdUser = int.Parse(
                    (txtReminderInCare.SelectedItem as KeyValueClass).Value.ToString());
                Activity.reminder.Date = txtDateReminder.Value;
                Activity.UserId = Activity.reminder.IdUser;

                if (newActivity)
                {
                    try
                    {
                        Activity.Id = ShiduchActivity.insertActivity(Activity);
                        ReminderActivity.InsertReminder(Activity);
                    }
                    catch { }
                }
                else if (updateActivity)
                {
                    Activity.Id = ShiduchActivity.Id;
                    Activity.UserId = ShiduchActivity.UserId;
                    try
                    {
                        ShiduchActivity.updateActivity(Activity);

                        //if (OpenReminder&&txtDateReminder.Value == ShiduchActivity.reminder.Date)
                        //    Activity.reminder.Done = true;
                        if (Activity.Status == (int)ShiduchActivity.ActionStatus.inCare)
                            Activity.reminder.Done = false;
                        else
                            Activity.reminder.Done = true;
                        ReminderActivity.UpdateReminder(Activity);
                        MessageBox.Show("עודכן בהצלחה");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if ((sender as Button) == btnSaveOpenB)
                    OpenSideB = true;
                else if (Activity.Status == (int)ShiduchActivity.ActionStatus.completed
                    && MessageBox.Show("האם ברצונך לפתוח פעילות חדשה?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    OpenNewActivity = true;
                save = true;
                //foreach (Form frm in Application.OpenForms)
                //{
                //    if (frm.GetType() == typeof(MainForm))
                //    {
                //        (frm as MainForm).LoadReminder();
                //    }
                //}

                Close();
            }
        }
        public bool OpenNewActivity = false;
        private void btnDetailsShiduch_Click(object sender, EventArgs e)
        {
            General.CreateReport(Shiduch);
            Report r = new Report();
            r.ShowDialog();
        }

        private void ShiduchActivityForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!save)
            {
                if (MessageBox.Show("האם אתה רוצה לשמור?", "הפעילות החדשה",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    e.Cancel = true;
                }
                else//אם פתחו פעילות ולא שמרו - למנהל שיבדוק אם שדכן לוקח מספרי טלפון ולא מתעד
                {
                    if (newActivity)
                    {

                        ShiduchActivity.insertActivity(new ShiduchActivity()
                        {
                            Action = (int)ShiduchActivity.ActionType.ActivityNoSave,
                            PeopleId = MyPeople.ID,
                            UserId = GLOBALVARS.MyUser.ID,
                            Date = DateTime.Now,

                        });
                    }
                }
            }
        }

        public void LockControlValues(Control Container)
        {
            try
            {
                foreach (Control ctrl in Container.Controls)
                {
                    if (ctrl.GetType() == typeof(TextBox))
                        ((TextBox)ctrl).ReadOnly = true;
                    if (ctrl.GetType() == typeof(ComboBox))
                        ((ComboBox)ctrl).Enabled = false;
                    if (ctrl.GetType() == typeof(RadioButton))
                        ((RadioButton)ctrl).Enabled = false;

                    if (ctrl.GetType() == typeof(DateTimePicker))
                        ((DateTimePicker)ctrl).Enabled = false;

                    if (ctrl.Controls.Count > 0)
                        LockControlValues(ctrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
