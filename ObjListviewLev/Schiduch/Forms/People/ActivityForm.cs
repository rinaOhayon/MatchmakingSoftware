using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schiduch
{
    public partial class ActivityForm : Form
    {
        public ActivityForm()
        {
            InitializeComponent();
        }

        //private void ActivityForm_Load(object sender, EventArgs e)
        //{

        //    SqlDataReader reader = People.ReadAll(0, false, false, false, false, true);
        //    while (reader.Read())
        //    {
        //        KeyValueClass item = new KeyValueClass();
        //        string text = (string)reader["FirstName"] + " " + (string)reader["LastName"];
        //        text += " " + reader["YeshivaGorSeminary"].ToString();
        //        text += " " + reader["City"].ToString();
        //        text += " " + reader["Street"].ToString();
        //        item.Text = text;
        //        item.Value = reader["ID"].ToString();
        //        txtSearchShiduch.Items.Add(item);
        //    }
        //    reader.Close();
        //    groupBox2.Text += GLOBALVARS.MyPeople.FirstName + " " + GLOBALVARS.MyPeople.Lasname;
        //    if (GLOBALVARS.MyPeople.Sexs == "בחורה")
        //    {
        //        groupBox1.Text = "המוצע";
        //        lblSearchShiduch.Visible = false;
        //    }
        //    //else
        //    //    lblSearchShiduch.Visible = true;
        //}

        private void txtSearchShiduch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (txtSearchShiduch.SelectedIndex != -1)
            //{
            //    int id = int.Parse((txtSearchShiduch.SelectedItem as KeyValueClass).Value.ToString());
            //    SqlDataReader reader = People.ReadById(id);
            //    while (reader.Read())
            //    {
            //        Shiduch = new People();
            //        PeopleManipulations.ReaderToPeople(ref Shiduch, ref reader, true, true);
            //    }
            //    reader.Close();
            //    txtNameShiduch.Text = Shiduch.FirstName + " " + Shiduch.Lasname;
            //    txtAgeShiduch.Text = Shiduch.Age.ToString();
            //    txtSchoolShiduch.Text = Shiduch.Details.YeshivaGorSeminary;
            //    txtAdressShiduch.Text = Shiduch.Details.Street + " " + Shiduch.City;
            //    txtWorkShiduch.Text = Shiduch.WorkPlace;
            //    btnconfirm.Enabled = true;
            //}
            //else
            //    btnconfirm.Enabled = false;
        }

        //private void btnDetailsShiduch_Click(object sender, EventArgs e)
        //{
        //    General.CreateReport(hiduch);
        //    Report r = new Report();
        //    r.ShowDialog();
        //}

        private void btnconfirm_Click(object sender, EventArgs e)
        {
            //ShiduchActivityForm s = new ShiduchActivityForm(new ShiduchActivity());
            //s.ShowDialog();
            Close();
        }
    }
}
