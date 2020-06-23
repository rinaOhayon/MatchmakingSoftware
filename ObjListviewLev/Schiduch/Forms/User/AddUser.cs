using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using Schiduch.Classes.Program;
using Schiduch.Classes.Users;

namespace Schiduch
{
    public partial class AddUser : Form
    {
        public int userid;
        public bool OpenDetailsForAdd { get; set; }
        public AddUser()
        {
            InitializeComponent();
            ((ListBox)txtSectorShadchan).DataSource = GLOBALVARS.Sectors;
            ((ListBox)txtSectorShadchan).DisplayMember = "SectorName";
        }
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void AddUser_Load(object sender, EventArgs e)
        {
            this.CancelButton = btnclose;
            this.AcceptButton = btnconfirm;
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
            {
                txttype.Enabled = true;
            }
            if (!OpenDetailsForAdd)
            {
                User LookUser = User.GetUser(userid);
                txtmail.Text = LookUser.Email;
                txtusername.Text = LookUser.UserName;
                txttype.SelectedIndex = (int)LookUser.Control;
                txttel.Text = LookUser.Tel;
                txtname.Text = LookUser.Name;
                txtTempPersonal.SelectedIndex = Convert.ToInt32(LookUser.TempPersonal);
                txtTempGeneral.SelectedIndex = Convert.ToInt32(LookUser.TempGeneral);
                if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
                {
                    txtpassword.Text = LookUser.Password;
                }
                else
                {
                    txtpassword.Enabled = false;
                    txtusername.Enabled = false;
                    txtusername.PasswordChar = '*';
                    txtpassword.PasswordChar = '*';
                    txtpassword.Text = LookUser.Password;
                }
                if (LookUser.Sector.Contains(","))
                {
                    string[] sectorIndex = LookUser.Sector.Split(',');
                    for (int i = 0; i < sectorIndex.Length; i++)
                    {
                        txtSectorShadchan.SetItemChecked(int.Parse(sectorIndex[i]), true);
                    }
                }
            }

        }
        private void btnconfirm_Click(object sender, EventArgs e)
        {
            if (OpenDetailsForAdd)
            {
                Add();
            }
            else
            {
                GLOBALVARS.LastUserChangeDB = DateTime.Now;
                UpdateManager.UpdateLastTimeCheckToDb();
                UpdateUser();
            }
        }
        private void Add()
        {
            int level = 0;
            if (DBFunction.CheckExist(txtusername.Text, "USERS", "username"))
            {
                MessageBox.Show("המשתמש קיים כבר במערכת", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
            {
                level = txttype.SelectedIndex;
            }
            string sector = SetSector();
            SqlParameter[] prms = new SqlParameter[10];
            string pass = User.SetPassword(txtpassword.Text);
            string sql = "insert into users(username,password,control,email,dateadded,name,tel,TempPersonal,TempGeneral,Sector) values(" +
                BuildSql.InsertSql(out prms[0], txtusername.Text) +
                BuildSql.InsertSql(out prms[1], pass) +
                BuildSql.InsertSql(out prms[2], level) +
                BuildSql.InsertSql(out prms[3], txtmail.Text) +
                BuildSql.InsertSql(out prms[4], DateTime.Now) +
                BuildSql.InsertSql(out prms[5], txtname.Text) +
                BuildSql.InsertSql(out prms[6], txttel.Text) +
                BuildSql.InsertSql(out prms[7], txtTempPersonal.SelectedIndex) +
                BuildSql.InsertSql(out prms[8], txtTempGeneral.SelectedIndex) +
                BuildSql.InsertSql(out prms[9], sector, true) +
                ");";
            if (DBFunction.Execute(sql, prms))
                MessageBox.Show("נוסף בהצלחה", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("אירעה שגיאה", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private string SetSector()
        {
            string sector = "";
            foreach (int index in txtSectorShadchan.CheckedIndices)
            {
                if (sector.Length > 0)
                    sector += ",";
                sector += index.ToString();
            }
            return sector;
        }
        private void UpdateUser()
        {
            int level = 0;
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
            {
                level = txttype.SelectedIndex;
            }
            SqlParameter[] prms = new SqlParameter[10];
            string sql = "update users set " +
                BuildSql.UpdateSql(out prms[0], txtusername.Text, "username") +
                BuildSql.UpdateSql(out prms[1], User.SetPassword(txtpassword.Text), "password") +
                BuildSql.UpdateSql(out prms[2], level, "control") +
                BuildSql.UpdateSql(out prms[3], txtmail.Text, "email") +
                BuildSql.UpdateSql(out prms[4], DateTime.Now, "dateadded") +
                BuildSql.UpdateSql(out prms[5], txtname.Text, "name") +
                BuildSql.UpdateSql(out prms[6], txttel.Text, "tel") +
                BuildSql.UpdateSql(out prms[7], txtTempPersonal.SelectedIndex, "TempPersonal") +
                BuildSql.UpdateSql(out prms[8], txtTempGeneral.SelectedIndex, "TempGeneral") +
                BuildSql.UpdateSql(out prms[9], SetSector(), "Sector", true) +
                " where id=" + userid + ";";
            if (DBFunction.Execute(sql, prms))
                MessageBox.Show("עודכן בהצלחה", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("אירעה שגיאה", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static bool ChangeUserNameAndPassword(string uname, string psw, int userid)
        {

            SqlParameter[] prms = new SqlParameter[7];
            string sql = "update users set " +
                BuildSql.UpdateSql(out prms[0], uname, "username") +
                BuildSql.UpdateSql(out prms[1], psw, "password", true) +
                " where id=" + userid + ";";
            if (DBFunction.Execute(sql, prms))
                return true;
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtSectorShadchan.Visible)
            {
                txtSectorShadchan.Visible = false;
                return;
            }
            txtSectorShadchan.Visible = true;
        }

        private void txtSectorShadchan_Format(object sender, ListControlConvertEventArgs e)
        {
            string team1 = ((Sector)e.ListItem).IdSector.ToString();
            string team2 = ((Sector)e.ListItem).SectorName.ToString();

            e.Value =team1+". "+team2;
        }
    }
}
