using Schiduch.Classes.Program;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Schiduch
{
    public partial class DetailRequiredfields : Form
    {

        public People newPeople = new People();
        public List<UTextBox> TextBoxList = new List<UTextBox>();
        public bool OK { get; set; } = false;
        public DetailRequiredfields()
        {
            InitializeComponent();
            SetupControlsToValidate();
            //  Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, txtcity.Width, txtcity.Height, 20, 20));

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool err = false;
            foreach (UTextBox t in TextBoxList)
            {
                if (t.Text.Length == 0)
                {
                    ErrorProvider1.SetError(t, required);
                    err = true;
                }
            }
            if (err)
            {
                MessageBox.Show("צריך למלאות את כל השדות");
                return;
            }
            MultipileCheck();
        }
        private void SetupControlsToValidate()
        {
            //Add data entry controls to be validated
            TextBoxList.Add(txtfname);
            TextBoxList.Add(txtlname);
            TextBoxList.Add(txtcity);
            TextBoxList.Add(txtmomname);
            TextBoxList.Add(DadNametxtdadname);
            TextBoxList.Add(txtTZ);
        }
        private string required = "שדה חובה";
        private string requiredValidate = String.Empty;
        private void txtfname_Validating(object sender, CancelEventArgs e)
        {
            if (txtfname.Text.Length > 0)
                ErrorProvider1.SetError(txtfname, requiredValidate);
            else
                ErrorProvider1.SetError(txtfname, required);
        }
        private void txtlname_Validating(object sender, CancelEventArgs e)
        {
            if (txtlname.Text.Length > 0)
                ErrorProvider1.SetError(txtlname, requiredValidate);
            else
                ErrorProvider1.SetError(txtlname, required);
        }
        private void MultipileCheck()
        {
            // newPeople = new People();
            newPeople.FirstName = txtfname.Text;
            newPeople.Lasname = txtlname.Text;
            newPeople.City = txtcity.Text;
            newPeople.Tz = txtTZ.Text;
            newPeople.Details.DadName = DadNametxtdadname.Text;
            newPeople.Details.MomName = txtmomname.Text;
            newPeople.OpenDetailsForAdd = radGeneral.Checked;
            newPeople.OpenForPersonalAdd = radPersonal.Checked;
            newPeople.Sexs = radMale.Checked ? 1 : 2;
            if (newPeople.OpenDetailsForAdd)
                newPeople.Temp = GLOBALVARS.MyUser.TempGeneral;
            else
                newPeople.Temp = GLOBALVARS.MyUser.TempPersonal;
            SqlParameter[] prms = new SqlParameter[10];

            string sql = "";
            sql += "select p.ID, FirstName,lastname, ByUserName,RegDate " +
                "from peoples p inner join peopledetails pd on p.ID = pd.relatedid inner join " +
                    "registerinfo r on pd.relatedid=r.relatedid where show <> 8 AND " +
                BuildSql.GetSql(out prms[0], txtfname.Text, "FirstName", BuildSql.SqlKind.LIKE) +
                BuildSql.GetSql(out prms[1], txtlname.Text, "Lastname", BuildSql.SqlKind.LIKE) +
                BuildSql.GetSql(out prms[2], txtcity.Text, "City", BuildSql.SqlKind.LIKE) +
                BuildSql.GetSql(out prms[4], DadNametxtdadname.Text, "DadName", BuildSql.SqlKind.LIKE) +
                BuildSql.GetSql(out prms[5], txtmomname.Text, "MomName", BuildSql.SqlKind.LIKE) +
                BuildSql.GetSql(out prms[6], txtTZ.Text, "Tz", BuildSql.SqlKind.LIKE);

            sql = BuildSql.CheckForLastAnd(ref sql);
            SqlDataReader reader = DBFunction.ExecuteReader(sql, prms);
            if (reader.HasRows)
            {
                reader.Read();
                string text = "כרטיס כבר קיים במערכת" + Environment.NewLine + "כרטיס על שם:" +
                    reader["FirstName"] + " " + reader["Lastname"] + ", נקלט בתאריך: "
                    + reader["RegDate"] + ", על ידי: " + reader["ByUserName"];
                MessageBox.Show(text, "כרטיס קיים", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                reader.Close();
                return;
            }
            OK = true;
            Close();
        }
     
        private void txtcity_Validating(object sender, CancelEventArgs e)
        {
            if (txtcity.Text.Length > 0)
                ErrorProvider1.SetError(txtcity, requiredValidate);
            else
                ErrorProvider1.SetError(txtcity, required);
        }
     
        private void DadNametxtdadname_Validating(object sender, CancelEventArgs e)
        {
            if (DadNametxtdadname.Text.Length > 0)
                ErrorProvider1.SetError(DadNametxtdadname, requiredValidate);
            else
                ErrorProvider1.SetError(DadNametxtdadname, required);
        }
        private void txtmomname_Validating(object sender, CancelEventArgs e)
        {
            if (txtmomname.Text.Length > 0)
                ErrorProvider1.SetError(txtmomname, requiredValidate);
            else
                ErrorProvider1.SetError(txtmomname, required);
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            OK = false;
            Close();
        }

        private void txtTZ_Validating(object sender, CancelEventArgs e)
        {
            if (txtTZ.Text.Length > 0)
                ErrorProvider1.SetError(txtTZ, requiredValidate);
            else
                ErrorProvider1.SetError(txtTZ, required);
        }
    }
}
