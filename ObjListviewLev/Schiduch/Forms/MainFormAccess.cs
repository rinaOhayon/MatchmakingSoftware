/*
-----------------------------------------------INFORMATION ABOUT THIS CLASS-------------------------------------------------------
this mainform exetend class will check the access of user and show only what it need
*/
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Schiduch.Classes.Program;

namespace Schiduch
{

    public partial class MainForm : Form
    {
       
        private void LoadLabelsToTxts()
        {
            GLOBALLABELS.LoadLabels();
            foreach (Labels lbl in GLOBALLABELS.AllLabels)
            {
                if (lbl == null)
                    continue;
                switch (lbl.Catg)
                {
                    case "ptichut":
                        txtpeticut.Items.Add(lbl.Label);
                        break;
                    case "zerem":
                        txtzerem.Items.Add(lbl.Label);
                        break;
                    case "fat":
                        txtfat.Items.Add(lbl.Label);
                        break;
                    case "facecolor":
                        txtfacecolor.Items.Add(lbl.Label);
                        break;
                    case "looks":
                        txtlooks.Items.Add(lbl.Label);
                        break;
                    case "bg":
                        txtbg.Items.Add(lbl.Label);
                        break;
                    case "beard":
                        txtbeard.Items.Add(lbl.Label);
                        break;
                    case "coverhead":
                        txtcoverhead.Items.Add(lbl.Label);
                        break;
                    case "sexs":
                        txtsexs.Items.Add(lbl.Label);
                        break;

                }
            }
        }
        public void LoadResultToList(ref People p, ListView lst, bool TempPeople = false)
        {
            
            ListViewItem item = new ListViewItem();
           // DateTime temp = DateTime.Parse("11/07/2015 23:22:22");
            item.Text = p.FirstName;
            item.SubItems.Add(p.Lasname);
            item.SubItems.Add(p.Details.Schools);
            item.SubItems.Add(p.Age.ToString());
            item.SubItems.Add(p.Tall.ToString());
            item.SubItems.Add(p.Details.Notes);
            item.SubItems.Add(p.ID.ToString());
            if (!TempPeople)
            {
              //  if (p.Register.LastUpdate < temp)
               //     item.ForeColor = Color.Gray;
            }
            if (TempPeople)
            {
                item.Tag = p.ByUser;
                item.SubItems.Add(p.TempId.ToString());
                item.SubItems.Add(p.Reason.ToString());
            }
            

            if (p.Sexs == 2)
                item.ImageIndex = 0;
            else
                item.ImageIndex = 1;
            switch (p.Reason)
            {
                case (int)People.ReasonType.Wedding:
                    item.ForeColor = Color.Red;
                    break;
                case (int)People.ReasonType.ShadChan:
                    item.ForeColor = Color.Blue;
                    break;
                case (int)People.ReasonType.AllowLimited:
                    item.ForeColor = Color.Green;
                    break;
            }
            switch (p.Show)
            {
                case (int)People.ShowTypes.HideDetails:
                    item.BackColor = Color.LightGreen;
                    if (GLOBALVARS.MyUser.Control == User.TypeControl.User)
                    {
                        item.Text = "חסוי";
                        item.SubItems[1].Text = "חסוי";
                    }
                    break;
                case (int)People.ShowTypes.HideFromUsers:
                    item.BackColor = Color.LightBlue;
                    break;
                case (int)People.ShowTypes.VIP:
                    item.BackColor = Color.Gold;
                    break;
                case (int)People.ShowTypes.Personal:
                    item.BackColor = Color.LightGreen;
                    break;
            }
            lst.Items.Add(item);
        }

        public void LoadResultToListFast(int count,ref People[] pa,ListView lst,bool TempPeople = false)
        {
            int counter = 0;
            ListViewItem[] item = new ListViewItem[count];
            // DateTime temp = DateTime.Parse("11/07/2015 23:22:22");
            foreach(People p in pa) {
                if (p == null)
                    break;
                item[counter] = new ListViewItem();
                item[counter].Text = p.FirstName;
                item[counter].SubItems.Add(p.Lasname);
                item[counter].SubItems.Add(p.Details.Schools);
                item[counter].SubItems.Add(p.Age.ToString());
                item[counter].SubItems.Add(p.Tall.ToString());
                item[counter].SubItems.Add(p.Details.Notes);
                item[counter].SubItems.Add(p.ID.ToString());
                if (!TempPeople)
                {
                    //  if (p.Register.LastUpdate < temp)
                    //     item.ForeColor = Color.Gray;
                }
                if (TempPeople)
                {
                    item[counter].Tag = p.ByUser;
                    item[counter].SubItems.Add(p.TempId.ToString());
                    item[counter].SubItems.Add(p.Reason.ToString());
                }


                if (p.Sexs == 2)
                    item[counter].ImageIndex = 0;
                else
                    item[counter].ImageIndex = 1;


                switch (p.Reason)
                {
                    case (int)People.ReasonType.Wedding:
                        item[counter].ForeColor = Color.Red;
                        break;
                    case (int)People.ReasonType.ShadChan:
                        item[counter].ForeColor = Color.Blue;
                        break;
                    case (int)People.ReasonType.AllowLimited:
                        item[counter].ForeColor = Color.Green;
                        break;
                }
                switch (p.Show)
                {
                    case (int)People.ShowTypes.HideDetails:
                        item[counter].BackColor = Color.LightGreen;
                        if (GLOBALVARS.MyUser.Control == User.TypeControl.User)
                        {
                            item[counter].Text = "חסוי";
                            item[counter].SubItems[1].Text = "חסוי";
                        }
                        break;
                    case (int)People.ShowTypes.HideFromUsers:
                        item[counter].BackColor = Color.LightBlue;
                        break;
                    case (int)People.ShowTypes.VIP:
                        item[counter].BackColor = Color.Gold;
                        break;
                    case (int)People.ShowTypes.Personal:
                        item[counter].BackColor = Color.LightGreen;
                        break;
                }
                counter++;
            }
            lst.Items.AddRange(item);
        }
        private void CheckAccess()
        {
           // TabPage managment = tabPage4;
         //   TabPage log = tabPage5;
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin || GLOBALVARS.MyUser.Control == User.TypeControl.Manger)
            {
                //todo דוחות קצרים
                //   btnshortreports.Visible = true;
                // btnadd.Visible = true;
                // btndel.Visible = true;
                //    tabControl1.TabPages.Add(managment);
                //    tabControl1.TabPages.Add(log);
                cmbActivityDairy.Items.Add("פעילות לא נשמרה");
            }
            else
            {
                btnManagmentOpen.Visible = false;
                //tabControl1.TabPages.Remove(tabPage6);
                tabControl1.TabPages.Remove(tabPage7);
                panel1.Visible = false;
            }
                //tabControl1.TabPages.Remove(tabPage12);
                tabControl1.TabPages.Remove(tabPage9);
                tabControl1.TabPages.Remove(tabPage7);
        }
        //This Function Create a thread that show the sw load data
        private void load(object state)
        {
            if (!Showloadprocess)
                return;
           
            

            
            formloadmsg.ShowDialog();
        }

      
       

       
    }
}
