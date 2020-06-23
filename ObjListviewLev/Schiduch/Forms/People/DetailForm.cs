using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using Schiduch.Classes.Program;

namespace Schiduch
{
    public partial class DetailForm : Form
    {
        private bool Tab4Firsttime = true;
        private bool Tab5Firsttime = true;
        private bool personaluser = false;
        private bool updatePhone = false;
        public People MyPeople;
        public DetailForm(People p)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(30, 0);
            this.ActiveControl = txtfname;
            txtfname.Focus();
            MyPeople = p;

        }
        private void LoadMaleFemaleToTxt()
        {
            if (MyPeople.Sexs == 1)
            {

                lblfemaleTzevet.Visible = false;
                lblfemalePhone.Visible = false;
                lblNameFemale.Visible = false;
                lblfemaleFriends.Visible = false;
            }
            else
            {
                groupYshivaSeminary.Text = "רקע השכלתי";
                groupExpectations.Text = "ציפיות מבן הזוג";
                lblWorkMale.Visible = false;
                lblKibutz.Visible = false;
                lblYeshivaG.Visible = false;
                lblYeshivaK.Visible = false;
                lblmaleFriends.Visible = false;
                GroupBox g = groupBoxWhat;
                Panel p = panelCoverHead;
                groupYshivaSeminary.Controls.Remove(groupBoxWhat);
                groupExpectations.Controls.Remove(panelCoverHead);
                groupYshivaSeminary.Controls.Add(p);
                groupExpectations.Controls.Add(g);
                txtworkplace.Height = 60;
                txtworkplace.ScrollBars = ScrollBars.Vertical;
            }
        }
        private void LoadReqieredField()
        {
            txtdadname.Text = MyPeople.Details.DadName;
            txtmomname.Text = MyPeople.Details.MomName;
            txtfname.Text = MyPeople.FirstName;
            txtlname.Text = MyPeople.Lasname;
            txtrealcity.Text = MyPeople.City;
            txtTz.Text = MyPeople.Tz;
        }
        private void LoadPeopleToTxts()
        {
            txtNameOf.Text = MyPeople.FirstName + " " + MyPeople.Lasname;
            txtTz.Text = MyPeople.Tz;
            txtChasidut.Text = MyPeople.KindChasidut;
            txtHealthStatus.Text = MyPeople.HealthStatus;
            txtHealthDetails.Text = MyPeople.HealthDetails;
            if (txtHealthStatus.Text == "לא תקין")
                txtHealthDetails.Enabled = true;
            txtYesivaKorHighSchool.Text = MyPeople.Details.YesivaKorHighSchool;
            txtYeshivaGorSeminary.Text = MyPeople.Details.YeshivaGorSeminary;
            txtKibutzorMaslul.Text = MyPeople.Details.KibutzorMaslul;
            if (MyPeople.LearnStaus == "לומד")
                radbtnLearn.Checked = true;
            else if (MyPeople.LearnStaus == "לומד ועובד")
                radbtnLearnWork.Checked = true;
            else if (MyPeople.LearnStaus == "קובע עיתים")
                radbtnLearnSometimes.Checked = true;
            btnLicense = radbtnLicense.Checked = MyPeople.Details.Licence;
            btnSmoker = radbtnSmoker.Checked = MyPeople.Details.Smoker;
            btnBeard = radbtnBeard.Checked = MyPeople.Beard == "מזוקן";
            btnNoBeard = radbtnNoBeard.Checked = MyPeople.Beard == "מגולח";

            txtAgeExpection.Value = (decimal)MyPeople.Details.AgeExpectation;
            txtEdaEpectation.Text = MyPeople.Details.EdaExpectation;
            txtDadYeshiva.Text = MyPeople.Details.DadYeshiva;
            txtMomSeminary.Text = MyPeople.Details.MomSeminary;
            txtStatusParents.Text = MyPeople.Details.StatusParents;
            txtCommunityTo.Text = MyPeople.Details.CommunityTo;
            txtParentHealth.Text = MyPeople.Details.ParentHealth;
            txtParentHealthDetails.Text = MyPeople.Details.ParentHealthDetails;
            txtZeremMom.Text = MyPeople.ZeremMom;
            txtLocationChild.Value = MyPeople.Details.LocationChild;
            txtNumMarriedSibilings.Value = MyPeople.Details.NumMarriedSibilings;
            txtContactShiduch.Text = MyPeople.Details.ContactShiduch;
            txtContactPhone.Text = MyPeople.Details.ContactPhone;
            txtFamilyAbout.Text = MyPeople.Details.FamilyAbout;
            // txtTelephone.Text = MyPeople.Details.Telephone;
            // txtPhoneOfBachur.Text = MyPeople.Details.PhoneOfBachur;
            //  txtMail.Text = MyPeople.Details.Mail;
            radbtnPhoneKosher.Checked = (MyPeople.Details.PhoneKosherLevel == "כשר");
            radbtnPhoneProtected.Checked = (MyPeople.Details.PhoneKosherLevel == "מוגן");
            radbtnPhoneUnProtected.Checked = (MyPeople.Details.PhoneKosherLevel == "לא מוגן");

            Age.Text = MyPeople.Age.ToString();
            txtbg.Text = MyPeople.Background;
            txtcity.Text = MyPeople.Details.Street;
            txtrealcity.Text = MyPeople.City;
            txtcoverhead.Text = MyPeople.CoverHead;
            txteda.Text = MyPeople.Eda;
            string[] birthday = MyPeople.BirthDayHebrew.Split('^');
            if (birthday.Length >= 1)
                txtBearthDay.Text = birthday[0];
            if (birthday.Length >= 2)
                txtBearthMonth.Text = birthday[1];
            if (birthday.Length >= 3)
                txtBearthYear.Text = birthday[2];
            if (MyPeople.Eda == "חסידי")
            {
                labelChasidut.Enabled = true;
                txtChasidut.Enabled = true;
            }
            if (MyPeople.ShiducNum == "זיווג ראשון")
                radbtnZivug1.Checked = true;
            else if (MyPeople.ShiducNum == "זיווג שני")
                radbtnZivug2.Checked = true;
            else if (MyPeople.ShiducNum == "ביטלו שידוך")
                radbtnZivugCancel.Checked = true;

            txtfacecolor.Text = MyPeople.FaceColor;
            txtfat.Text = MyPeople.Weight;
            //txtsexs.Text = MyPeople.Sexs;
            txtfname.Text = MyPeople.FirstName;
            txtGorTorN.Text = MyPeople.GorTorN;
            txtstatus.Text = MyPeople.Status;

            txtheight.Text = MyPeople.Tall.ToString();
            txtlname.Text = MyPeople.Lasname;
            txtlooks.Text = MyPeople.Looks;
            txtnotes.Text = MyPeople.Details.Notes;
            txtwhoami.Text = MyPeople.Details.WhoAmI;
            txtwhoiwant.Text = MyPeople.Details.WhoIWant;
            txtzeremDad.Text = MyPeople.Zerem;
            txtmoneytoschadchan.Text = MyPeople.Details.MoneyToShadchan;

            txtstatus.Text = MyPeople.Status;
            txtownchildren.Text = MyPeople.Details.OwnChildrenCount.ToString();

            txtdadname.Text = MyPeople.Details.DadName;
            txtmomname.Text = MyPeople.Details.MomName;
            txtmomlname.Text = MyPeople.Details.MomLname;
            txtdadwork.Text = MyPeople.DadWork;

            txtfriends.Text = MyPeople.Details.FriendsInfo;
            txtzevet.Text = MyPeople.Details.ZevetInfo;


            txtchildern.Text = MyPeople.Details.ChildrenCount.ToString();
            txtsibilngschool.Text = MyPeople.Details.SiblingsSchools;
            txtmoneygives.Text = MyPeople.Details.MoneyGives.ToString();
            txtmoneynotes.Checked = MyPeople.Details.MoneyNotesFlex;
            txtmoneyrequired.Text = MyPeople.Details.MoneyRequired.ToString();
            // txtDadPhone.Text = MyPeople.Details.Tel1;
            //  txtMomPhone.Text = MyPeople.Details.Tel2;
            txtmecuthanim.Text = MyPeople.Details.MechutanimNames;
            txthomerav.Text = MyPeople.Details.HomeRav;
            txtworkplace.Text = MyPeople.WorkPlace;
            txtmomwork.Text = MyPeople.Details.MomWork;

            txtReceiveBy.Text = MyPeople.Register.ByUserName;
            txtDateReceive.Text = MyPeople.Register.RegDate.ToShortDateString();
            if (MyPeople.Show == (int)People.ShowTypes.HideDetails && GLOBALVARS.MyUser.Control == User.TypeControl.User)
            {
                txtfname.PasswordChar = '*';
                txtfname.Enabled = false;
                txtlname.PasswordChar = '*';
                txtlname.Enabled = false;
            }
            if (MyPeople.Show == (int)People.ShowTypes.VIP)
                rbtn_showto.Checked = true;
            txtPtichut.SelectedIndex = MyPeople.OpenHead;
        }
        List<HistoryChangeDetails> cdc = new List<HistoryChangeDetails>();

        private void LoadLbls()
        {
            List<Labels> ls = new List<Labels>();
            foreach (Labels lbl in GLOBALLABELS.AllLabels)
            {
                if (lbl == null)
                    continue;
                switch (lbl.Catg)
                {

                    case "zerem":
                        txteda.Items.Add(lbl.Label);
                        txtEdaEpectation.Items.Add(lbl.Label);
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

                    case "coverhead":
                        txtcoverhead.Items.Add(lbl.Label);
                        break;
                    case "sexs":
                        txtsexs.Items.Add(lbl.Label);
                        break;
                    case "status":
                        txtstatus.Items.Add(lbl.Label);
                        break;
                    case "healthStatus":

                        txtHealthStatus.Items.Add(lbl.Label);

                        txtParentHealth.Items.Add(lbl.Label);
                        break;
                    case "parentStatus":
                        txtStatusParents.Items.Add(lbl.Label);
                        break;
                    case "ptichut":
                        txtPtichut.Items.Add(lbl.Label);
                        break;
                }
            }


            txtfat.SelectedIndex = 3;
            txtfacecolor.SelectedIndex = 2;
            txtlooks.SelectedIndex = 2;
            txtGorTorN.SelectedIndex = 0;
            txtbg.SelectedIndex = 0;

            txtstatus.SelectedIndex = 0;
            txtParentHealth.SelectedIndex = 0;
            txtHealthStatus.SelectedIndex = 0;
            txtStatusParents.SelectedIndex = 0;
            txtPtichut.SelectedIndex = 0;
            radbtnZivug1.Checked = true;
            txtReceiveBy.Text = GLOBALVARS.MyUser.Name;
            txtDateReceive.Text = DateTime.Now.ToShortDateString();
            if (MyPeople.Sexs == 1)
                radbtnLearn.Checked = true;
            radbtnPhoneKosher.Checked = true;
        }
        private void DetailForm_Load(object sender, EventArgs e)
        {
            LoadMaleFemaleToTxt();
            LoadLbls();
            ShowWhatNeed();

            if (MyPeople.OpenDetailsForAdd)
            {

                LoadReqieredField();
                return;
            }
            if (MyPeople.OpenForPersonalAdd)
            {
                LoadReqieredField();

                return;
            }
            if (MyPeople.OpenForPersonalEdit || MyPeople.OpenForPersonalAdd)
            {
                personaluser = true;
            }
            if (MyPeople.OpenForTrashPeople)
                lblTrash.Visible = true;
            LoadPeopleToTxts();
            txtBearthYear_Leave(new object(), new EventArgs());
            loadNotes();
        }
        private void ShowWhatNeed()
        {
            switch (GLOBALVARS.MyUser.Control)
            {
                case User.TypeControl.User:
                    tabControl1.TabPages.Remove(tabPage4);
                    //if (MyPeople.OpenForPersonalAdd || MyPeople.OpenForPersonalEdit)
                    //{
                    //    btnceratehtml.Visible = false;
                    break;
            }
        }
        private void CreateHistory(string fieldName, object currentValue, object newValue)
        {
            //int d = String.Compare(currentValue.ToString(), newValue.ToString(), false);
            if (!MyPeople.OpenDetailsForAdd && !MyPeople.OpenForPersonalAdd && (
             String.Compare(currentValue.ToString(), newValue.ToString(), false) == -1 ||
             currentValue.ToString().Length != newValue.ToString().Length))//אם נפתח לא בשביל להוסיף
            {
                cdc.Add(new HistoryChangeDetails(
                        GLOBALVARS.MyUser.ID,
                        MyPeople.ID,
                        GLOBALVARS.MyUser.Name,
                        fieldName,
                        currentValue.ToString(),
                        newValue.ToString()));
            }
        }
        private void LoadTxtsToPeople()
        {
            CreateHistory("פתיחות", txtPtichut.Items[MyPeople.OpenHead], txtPtichut.Text);
            MyPeople.OpenHead = txtPtichut.SelectedIndex;
            CreateHistory("דמי שדכנות", MyPeople.Details.MoneyToShadchan, txtmoneytoschadchan.Text);

            MyPeople.Details.MoneyToShadchan = txtmoneytoschadchan.Text;

            CreateHistory("גיל", MyPeople.Age, (float)Age.Value);
            MyPeople.Age = (float)Age.Value;
            CreateHistory("רקע", MyPeople.Background, txtbg.Text);
            MyPeople.Background = txtbg.Text;
            CreateHistory("עיר", MyPeople.City, txtrealcity.Text);
            MyPeople.City = txtrealcity.Text;
            CreateHistory("כיסוי ראש", MyPeople.CoverHead, txtcoverhead.Text);
            MyPeople.CoverHead = txtcoverhead.Text;
            CreateHistory("חוג", MyPeople.Eda, txteda.Text);
            MyPeople.Eda = txteda.Text;
            string s = txtYesivaKorHighSchool.Text +
                ", " + txtYeshivaGorSeminary.Text + ", " + txtKibutzorMaslul.Text;
            CreateHistory("מוסדות לימוד", MyPeople.Details.Schools, s);
            MyPeople.Details.Schools = s;
            CreateHistory("צבע פנים", MyPeople.FaceColor, txtfacecolor.Text);
            MyPeople.FaceColor = txtfacecolor.Text;

            CreateHistory("משקל", MyPeople.Weight, txtfat.Text);
            MyPeople.Weight = txtfat.Text;
            CreateHistory("שם פרטי", MyPeople.FirstName, txtfname.Text);

            MyPeople.FirstName = txtfname.Text;

            MyPeople.GorTorN = txtGorTorN.Text;
            if (txtGorTorN.SelectedIndex == 0)
                MyPeople.GorTorN = "";
            // MyPeople.LearnStaus = txtlearnstatus.Text;

            CreateHistory("גובה", MyPeople.Tall, (float)txtheight.Value);

            MyPeople.Tall = (float)txtheight.Value;
            CreateHistory("שם משפחה", MyPeople.Lasname, txtlname.Text);

            MyPeople.Lasname = txtlname.Text;
            CreateHistory("מראה חיצוני", MyPeople.Looks, txtlooks.Text);

            MyPeople.Looks = txtlooks.Text;
            CreateHistory("הערות", MyPeople.Details.Notes, txtnotes.Text);

            MyPeople.Details.Notes = txtnotes.Text;
            CreateHistory("תכונות שיש בי", MyPeople.Details.WhoAmI, txtwhoami.Text);

            MyPeople.Details.WhoAmI = txtwhoami.Text;
            CreateHistory("תכונות שאני מחפש", MyPeople.Details.WhoIWant, txtwhoiwant.Text);

            MyPeople.Details.WhoIWant = txtwhoiwant.Text;
            CreateHistory("מוצא האב", MyPeople.Zerem, txtzeremDad.Text);

            MyPeople.Zerem = txtzeremDad.Text;
            CreateHistory("מספר הילדים שלו", MyPeople.Details.OwnChildrenCount, (int)txtownchildren.Value);

            MyPeople.Details.OwnChildrenCount = (int)txtownchildren.Value;

            CreateHistory("סטטוס", MyPeople.Status, txtstatus.Text);

            MyPeople.Status = txtstatus.Text;
            CreateHistory("עיסוק האם", MyPeople.Details.MomWork, txtmomwork.Text);

            MyPeople.Details.MomWork = txtmomwork.Text;



            CreateHistory("רחוב", MyPeople.Details.Street, txtcity.Text);

            MyPeople.Details.Street = txtcity.Text;
            CreateHistory("שם האב", MyPeople.Details.DadName, txtdadname.Text);

            MyPeople.Details.DadName = txtdadname.Text;
            CreateHistory("שם האם", MyPeople.Details.MomName, txtmomname.Text);

            MyPeople.Details.MomName = txtmomname.Text;
            CreateHistory("אמא - שם קודם", MyPeople.Details.MomLname, txtmomlname.Text);

            MyPeople.Details.MomLname = txtmomlname.Text;
            CreateHistory("עיסוק האב", MyPeople.DadWork, txtdadwork.Text);

            MyPeople.DadWork = txtdadwork.Text;

            CreateHistory("טלפונים " + (MyPeople.Sexs == 1 ? lblmaleFriends.Text : lblfemaleFriends.Text),
                MyPeople.Details.FriendsInfo, txtfriends.Text);

            MyPeople.Details.FriendsInfo = txtfriends.Text;

            CreateHistory("טלפונים " + (MyPeople.Sexs == 1 ? lblmaleTzevet.Text : lblfemaleTzevet.Text)
                , MyPeople.Details.ZevetInfo, txtzevet.Text);
            MyPeople.Details.ZevetInfo = txtzevet.Text;

            CreateHistory("מספר ילדים", MyPeople.Details.ChildrenCount, (int)txtchildern.Value);
            MyPeople.Details.ChildrenCount = (int)txtchildern.Value;
            CreateHistory("מוסודת לימוד אחים/ות", MyPeople.Details.SiblingsSchools, txtsibilngschool.Text);
            MyPeople.Details.SiblingsSchools = txtsibilngschool.Text;
            CreateHistory("השתתפות כספית", MyPeople.Details.MoneyGives, (float)txtmoneygives.Value);
            MyPeople.Details.MoneyGives = (float)txtmoneygives.Value;
            CreateHistory("גמישות כספית", MyPeople.Details.MoneyNotesFlex, txtmoneynotes.Checked);
            MyPeople.Details.MoneyNotesFlex = txtmoneynotes.Checked;
            CreateHistory("דרישה כספית", MyPeople.Details.MoneyRequired, (float)txtmoneyrequired.Value);
            MyPeople.Details.MoneyRequired = (float)txtmoneyrequired.Value;
            CreateHistory("פלאפון אבא", MyPeople.Details.Tel1, txtDadPhone.Text);
            MyPeople.Details.Tel1 = txtDadPhone.Text;
            CreateHistory("פלאפון אמא", MyPeople.Details.Tel2, txtMomPhone.Text);
            MyPeople.Details.Tel2 = txtMomPhone.Text;
            CreateHistory("שמות מחתונים + עיר", MyPeople.Details.MechutanimNames, txtmecuthanim.Text);
            MyPeople.Details.MechutanimNames = txtmecuthanim.Text;
            CreateHistory("רב שקשורים אליו", MyPeople.Details.HomeRav, txthomerav.Text);

            MyPeople.Details.HomeRav = txthomerav.Text;
            CreateHistory("מקום עבודה", MyPeople.WorkPlace, txtworkplace.Text);
            MyPeople.WorkPlace = txtworkplace.Text;
            CreateHistory("תעודת זהות", MyPeople.Tz, txtTz.Text);

            MyPeople.Tz = txtTz.Text;
            CreateHistory("חצר חסידות", MyPeople.KindChasidut, txtChasidut.Text);

            MyPeople.KindChasidut = txtChasidut.Text;
            CreateHistory("מצב בריאותי", MyPeople.HealthStatus, txtHealthStatus.Text);

            MyPeople.HealthStatus = txtHealthStatus.Text;
            CreateHistory("מצב בריאותי - פירוט", MyPeople.HealthDetails, txtHealthDetails.Text);

            MyPeople.HealthDetails = txtHealthDetails.Text;
            CreateHistory("מוצא האם", MyPeople.ZeremMom, txtZeremMom.Text);

            MyPeople.ZeremMom = txtZeremMom.Text;
            CreateHistory("תאריך לידה", MyPeople.BirthDayHebrew, setBirthday());

            MyPeople.BirthDayHebrew = setBirthday();

            CreateHistory(MyPeople.Sexs == 1 ? lblYeshivaK.Text : lblHighSchool.Text, MyPeople.Details.YesivaKorHighSchool, txtYesivaKorHighSchool.Text);

            MyPeople.Details.YesivaKorHighSchool = txtYesivaKorHighSchool.Text;
            CreateHistory(MyPeople.Sexs == 1 ? lblYeshivaG.Text : lblSeminary.Text,
                MyPeople.Details.YeshivaGorSeminary, txtYeshivaGorSeminary.Text);

            MyPeople.Details.YeshivaGorSeminary = txtYeshivaGorSeminary.Text;
            CreateHistory(MyPeople.Sexs == 1 ? lblKibutz.Text : lblMaslul.Text, MyPeople.Details.KibutzorMaslul, txtKibutzorMaslul.Text);

            MyPeople.Details.KibutzorMaslul = txtKibutzorMaslul.Text;
            CreateHistory("ציפיות - חוג", MyPeople.Details.EdaExpectation, txtEdaEpectation.Text);

            MyPeople.Details.EdaExpectation = txtEdaEpectation.Text;
            CreateHistory("גיל - חוג", MyPeople.Details.AgeExpectation, (float)txtAgeExpection.Value);

            MyPeople.Details.AgeExpectation = (float)txtAgeExpection.Value;
            CreateHistory("אבא - יוצא ישיבת", MyPeople.Details.DadYeshiva, txtDadYeshiva.Text);

            MyPeople.Details.DadYeshiva = txtDadYeshiva.Text;
            CreateHistory("אמא - בוגרת סמינר", MyPeople.Details.MomSeminary, txtMomSeminary.Text);
            MyPeople.Details.MomSeminary = txtMomSeminary.Text;
            CreateHistory("סטטוס הורים", MyPeople.Details.StatusParents, txtStatusParents.Text);
            MyPeople.Details.StatusParents = txtStatusParents.Text;
            CreateHistory("השתייכות קהילתית", MyPeople.Details.CommunityTo, txtCommunityTo.Text);
            MyPeople.Details.CommunityTo = txtCommunityTo.Text;
            CreateHistory("מצב ביראותי - הורים", MyPeople.Details.ParentHealth, txtParentHealth.Text);
            MyPeople.Details.ParentHealth = txtParentHealth.Text;
            CreateHistory("פירוט מצב ביראותי - הורים", MyPeople.Details.ParentHealthDetails, txtParentHealthDetails.Text);
            MyPeople.Details.ParentHealthDetails = txtParentHealthDetails.Text;
            CreateHistory("מיקום במשפחה", MyPeople.Details.LocationChild, (int)txtLocationChild.Value);
            MyPeople.Details.LocationChild = (int)txtLocationChild.Value;
            CreateHistory("מספר אחים נשואים", MyPeople.Details.NumMarriedSibilings, (int)txtNumMarriedSibilings.Value);
            MyPeople.Details.NumMarriedSibilings = (int)txtNumMarriedSibilings.Value;
            CreateHistory("איש קשר להצעת שידוך", MyPeople.Details.ContactShiduch, txtContactShiduch.Text);
            MyPeople.Details.ContactShiduch = txtContactShiduch.Text;
            CreateHistory("איש קשר - טלפון", MyPeople.Details.ContactPhone, txtContactPhone.Text);
            MyPeople.Details.ContactPhone = txtContactPhone.Text;
            CreateHistory("טלפונים אודות המשפחה", MyPeople.Details.FamilyAbout, txtFamilyAbout.Text);
            MyPeople.Details.FamilyAbout = txtFamilyAbout.Text;
            CreateHistory("טלפון", MyPeople.Details.Telephone, txtTelephone.Text);
            MyPeople.Details.Telephone = txtTelephone.Text;
            CreateHistory("טלפון של הבחו" + (MyPeople.Sexs == 1 ? "ר" : "רה"), MyPeople.Details.PhoneOfBachur, txtPhoneOfBachur.Text);
            MyPeople.Details.PhoneOfBachur = txtPhoneOfBachur.Text;
            CreateHistory("מייל", MyPeople.Details.Mail, txtMail.Text);
            MyPeople.Details.Mail = txtMail.Text;
            s = radbtnPhoneKosher.Checked ? "כשר"
                : radbtnPhoneProtected.Checked ? "מוגן" : "לא מוגן";
            CreateHistory("כשרות הפלאפון", MyPeople.Details.PhoneKosherLevel, s);
            MyPeople.Details.PhoneKosherLevel = s;

            if (rbtn_everyone.Checked)
            {
                MyPeople.Show = (int)People.ShowTypes.Show;

            }
            else if (rbtn_showto.Checked)
            {
                MyPeople.Show = (int)People.ShowTypes.Personal;
                string temp = "";
                foreach (KeyValueClass x in lstchadchanim.Items)
                {
                    temp += "{" + x.Value.ToString() + "}";
                }
                MyPeople.Chadchan = temp;
            }


        }

        public bool SaveCard { get; set; } = false;
        private void btnconfirm_Click(object sender, EventArgs e)
        {
            LoadTxtsToPeople();
            // try {
            GLOBALVARS.LastPeopleCheckDB = DateTime.Now;
            UpdateManager.UpdateLastTimeCheckToDb();
            if (MyPeople.OpenDetailsForAdd || MyPeople.OpenForPersonalAdd)
            {
                People p = MyPeople;
                p.Details.Pic = "sample49";
                int id;
                if (People.InsretNew(p, out id))
                {
                    SaveCard = true;
                    //foreach (var item in p.Note)
                    //{
                    //    item.PeopleId = id;
                    // HistoryChangeDetails h = new HistoryChangeDetails(
                    // GLOBALVARS.MyUser.ID,
                    // id,
                    // GLOBALVARS.MyUser.Name,
                    // "הערה חדשה",
                    //"",
                    //item.NoteText);
                    // h.InserHistory();
                    //   People.InsertNewNotes(item);
                    // }
                    MyPeople.ID = id;
                    MessageBox.Show("נוסף בהצלחה", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                    MessageBox.Show("אירעה שגיאה. אנא בדקו שכל הנתונים שהזנתם תקינים", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                People.UpdatePeople(MyPeople, false);
                SaveCard = true;
                HistoryChangeDetails.InserListHistory(cdc);
            }
            if ((sender as Button) == btnConfirmClose)
                Close();
        }

        private void btnshadcanupdate_Click(object sender, EventArgs e)
        {
            LoadTxtsToPeople();
        }



        private void txtctrlplusa(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
            else if (e.Control & e.KeyCode == Keys.Back)
            {
                SendKeys.SendWait("^+{LEFT}{BACKSPACE}");
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                ((TextBox)sender).Text += (string)Clipboard.GetData("Text");
                e.Handled = true;
            }
        }



        private void btnceratehtml_Click(object sender, EventArgs e)
        {
            General.CreateReport(MyPeople);
            Report r = new Report();
            r.ShowDialog();
        }




        private void txteda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txteda.SelectedIndex == 2 || txteda.Text == "חסידי")
            {
                txtChasidut.Enabled = true;
                labelChasidut.Enabled = true;
            }
            else
            {
                txtChasidut.Text = "";
                txtChasidut.Enabled = false;
                labelChasidut.Enabled = false;

            }
        }

        private void txtlearnstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnshowinfo_Click(object sender, EventArgs e)
        {
            if (MyPeople.OpenDetailsForAdd || MyPeople.OpenForPersonalAdd)
            {
                MessageBox.Show("לא זמין במצב הוספה. " + Environment.NewLine + "שמרו את הכרטיס החדש ולאחר מכן פתחו שוב", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ShiduchActivityForm activityForm = new ShiduchActivityForm(new ShiduchActivity(), MyPeople);
                activityForm.Show();
                activityForm.FormClosed += s.SForm_FormClosed;
                //  string info = "";
                // info = txtDadPhone.Text + "\n" + txtMomPhone.Text + "\n" + txtTelephone.Text + "\n" + txtMail.Text;
                // Forms.FrmPhone frmphone = new Forms.FrmPhone(info, txtfname.Text + " " + txtlname.Text + "^" + MyPeople.ID.ToString(), MyPeople.ID.ToString());
                //   Log.AddAction(Log.ActionType.PhoneFormOpen, new Log(Log.ActionType.PhoneFormOpen, txtfname.Text + " " + txtlname.Text + "^" + MyPeople.ID.ToString()));
                // frmphone.ShowDialog();
            }
        }

        private void rbtn_everyone_CheckedChanged(object sender, EventArgs e)
        {
            MyPeople.Show = 0;
            cmb_shadchanim.Enabled = false;
            btn_addchadchan.Enabled = false;
            btn_removechadchan.Enabled = false;
            lstchadchanim.Enabled = false;
        }

        private void rbtn_showto_CheckedChanged(object sender, EventArgs e)
        {
            MyPeople.Show = 5;
            cmb_shadchanim.Enabled = true;
            btn_addchadchan.Enabled = true;
            btn_removechadchan.Enabled = true;
            lstchadchanim.Enabled = true;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex == 4 && Tab4Firsttime)
            {
                foreach (KeyValueClass x in GLOBALVARS.Shadchanim)
                {
                    cmb_shadchanim.Items.Add(x);
                    if (!MyPeople.OpenDetailsForAdd && MyPeople.Chadchan.Contains("{" + x.Value + "}"))
                        lstchadchanim.Items.Add(x);
                }

                Tab4Firsttime = false;

            }

            if (tabControl1.SelectedIndex == 3)
                loadNotes();
            if (tabControl1.SelectedIndex == 1)
            {
                this.ActiveControl = txtdadname;
                txtdadname.Focus();
            }
            if (tabControl1.SelectedIndex == 2)
            {
                this.ActiveControl = txtContactShiduch;
                txtContactShiduch.Focus();
            }
            if (tabControl1.SelectedTab == tabPage8)
            {
                LoadMyActivities();
                LoadAllActivities();
            }
            if (tabControl1.SelectedTab == tabPage9)
            {
                LoadHistory();
            }
        }

        private void LoadHistory()
        {
            if (MyPeople.OpenDetailsForAdd || MyPeople.OpenForPersonalAdd)
                return;
            SqlDataReader reader = HistoryChangeDetails.GetHistory(true, MyPeople);
            List<HistoryChangeDetails> hl = new List<HistoryChangeDetails>();
            olstHistoryChangeDetails.BeginUpdate();
            while (reader.Read())
            {
                HistoryChangeDetails h = new HistoryChangeDetails();
                h = HistoryChangeDetails.ReaderToHistoryChangeDetails(ref reader);
                hl.Add(h);
            }
            olstHistoryChangeDetails.SetObjects(hl);
            olstHistoryChangeDetails.EndUpdate();
            reader.Close();
        }

        private void LoadMyActivities()
        {
            SqlDataReader reader = ShiduchActivity.GetActivities(true, MyPeople);
            lstMyActivity.Items.Clear();
            ListView lst = lstMyActivity;
            ListViewItem item;
            lstMyActivity.BeginUpdate();
            while (reader.Read())
            {
                string notes = reader["NotesSummary"].ToString();
                string name = reader["FullNameB"] != System.DBNull.Value ? (string)reader["FullNameB"] : "";
                if ((ShiduchActivity.ActionType)(int)reader["Action"] == ShiduchActivity.ActionType.other)
                    notes = notes.Substring(notes.IndexOf('^') + 3);
                item = new ListViewItem(new string[] {
                 DateTime.Parse(reader["Date"].ToString()).ToShortDateString(),
                   ShiduchActivity.ConvertAction((ShiduchActivity.ActionType) (int)reader["Action"], reader),
                    name,
                    ShiduchActivity.ConvertStatus((ShiduchActivity.ActionStatus)(int)reader["Status"]),
                  notes,
                  reader["IdSideB"].ToString()

                });
                item.Tag = reader["Id"];
                lst.Items.Add(item);
            }
            lstMyActivity.EndUpdate();
            reader.Close();
        }
        private void LoadAllActivities()
        {
            SqlDataReader reader = ShiduchActivity.GetActivities(false, MyPeople, true);
            lstAllActivity.Items.Clear();
            ListView lst = lstAllActivity;
            ListViewItem item;
            lstAllActivity.BeginUpdate();
            while (reader.Read())
            {
                string name = reader["FullNameB"] != System.DBNull.Value ? (string)reader["FullNameB"] : "";

                item = new ListViewItem(new string[] {
                 DateTime.Parse(reader["Date"].ToString()).ToShortDateString(),
                 (string)reader["Name"],
                  ShiduchActivity.ConvertAction((ShiduchActivity.ActionType) (int)reader["Action"], reader),
                  name
                });
                lst.Items.Add(item);
            }
            lstAllActivity.EndUpdate();
            reader.Close();
        }
        private void btn_addchadchan_Click(object sender, EventArgs e)
        {
            if (cmb_shadchanim.SelectedIndex != -1)
            {
                //KeyValueClass temp =(KeyValueClass) ;
                lstchadchanim.Items.Add(cmb_shadchanim.SelectedItem);
            }
        }
        private void btn_removechadchan_Click(object sender, EventArgs e)
        {
            if (lstchadchanim.SelectedIndex != -1)
                lstchadchanim.Items.RemoveAt(lstchadchanim.SelectedIndex);
        }

        private void DetailForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }




        private void btnconfirm_MouseHover(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.LightGray;

        }

        private void btnconfirm_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.White;

        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            MyPeople.LearnStaus = (sender as RadioButton).Text;
        }

        private void txtstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtstatus.SelectedIndex != 0)
                lblownChildren.Enabled = txtownchildren.Enabled = true;
            else
                lblownChildren.Enabled = txtownchildren.Enabled = false;

        }

        private void radbtnZivug1_CheckedChanged(object sender, EventArgs e)
        {
            MyPeople.ShiducNum = (sender as RadioButton).Text;
        }

        private void txtHealthStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtHealthStatus.SelectedIndex != 0)
            {
                txtHealthDetails.Enabled = true;
                labelHealthDetails.Enabled = true;
            }
            else
            {
                txtHealthDetails.Text = "";
                txtHealthDetails.Enabled = false;
                labelHealthDetails.Enabled = false;

            }
            //MessageBox.Show(txtHealthStatus.SelectedValue.ToString());
            //MessageBox.Show((txtHealthStatus.SelectedItem as ComboboxItem).Value.ToString());
        }

        private void radbtnNoBeard_CheckedChanged(object sender, EventArgs e)
        {
            MyPeople.Beard = (sender as RadioButton).Text;
        }

        private void txtParentHealth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtParentHealth.SelectedIndex != 0)
            {
                txtParentHealthDetails.Enabled = true;
                lblParentHealthDetails.Enabled = true;
            }
            else
            {
                txtParentHealthDetails.Text = "";
                txtParentHealthDetails.Enabled = false;
                lblParentHealthDetails.Enabled = false;
            }
        }
        bool isnew = false;
        private void btnNotesNew_Click(object sender, EventArgs e)
        {
            txtNoteText.Text = "";
            isnew = true;
            btnNoteSave.Enabled = false;
        }
        private void loadNotes(ListView lstvw = null)
        {
            lstNotesOfPeople.Items.Clear();
            txtNoteText.Text = "";
            btnNoteSave.Enabled = false;
            SqlDataReader reader = null;
            if (lstvw == null) lstvw = lstNotesOfPeople;

            ListViewItem item;
            //  ||GLOBALVARS.MyUser.CanEdit
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Manger || GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
            {
                reader = DBFunction.ExecuteReader("select * from NotesOfPeople n where n.PeopleId=" + MyPeople.ID);
            }
            else
                reader = DBFunction.ExecuteReader("select * from NotesOfPeople n where n.UserId=" + GLOBALVARS.MyUser.ID + "and n.PeopleId=" + MyPeople.ID);
            lstNotesOfPeople.BeginUpdate();
            MyPeople.Note = new List<NotesOfPeople>();
            NotesOfPeople n = new NotesOfPeople();
            while (reader.Read())
            {
                MyPeople.Note.Add(n.ReaderToNotes(ref reader));
                item = new ListViewItem(new string[] {
                 DateTime.Parse(reader["NoteDate"].ToString()).ToShortDateString(),
                    (string)reader["UserName"],
                    (string)reader["NoteText"],
                  reader["NoteId"].ToString()

                });
                item.Tag = reader["UserId"].ToString();
                lstvw.Items.Add(item);
            }
            lstNotesOfPeople.EndUpdate();
            reader.Close();

        }

        //private void btnNoteEdit_Click(object sender, EventArgs e)
        //{
        //    if (lstNotesOfPeople.SelectedItems.Count == 0)
        //        return;

        //    txtNoteText.Enabled = btnNoteSave.Enabled = true;
        //    txtNoteText.Text = lstNotesOfPeople.SelectedItems[0].SubItems[2].Text;

        //}
        public static int tempId = 0;
        private void btnNoteSave_Click(object sender, EventArgs e)
        {
            if (MyPeople.OpenDetailsForAdd || MyPeople.OpenForPersonalAdd)
            {
                if (isnew)//הערה חדשה
                {
                    if (txtNoteText.Text.Length < 1000)
                    {

                        MyPeople.Note.Add(new NotesOfPeople() { NoteText = txtNoteText.Text, NoteId = tempId });
                        tempId++;
                        ListViewItem item;
                        lstNotesOfPeople.BeginUpdate();

                        item = new ListViewItem(new string[] {
                        DateTime.Now.ToShortDateString(),
                        GLOBALVARS.MyUser.Name,
                        txtNoteText.Text,
                        tempId.ToString()
                    });
                        item.Tag = tempId.ToString();
                        lstNotesOfPeople.Items.Add(item);
                        lstNotesOfPeople.EndUpdate();
                    }
                    else
                    {
                        MessageBox.Show("אורך הערה מקסימלי הוא עד 1000 תווים." + Environment.NewLine + "נא פצלו ל2 הערות");
                    }
                }
                else//הערה ששינו אותה
                {
                    lstNotesOfPeople.SelectedItems[0].SubItems[2].Text = txtNoteText.Text;
                    MyPeople.Note.ToArray()[lstNotesOfPeople.SelectedItems[0].Index].NoteText = txtNoteText.Text;
                }
            }

            else//הערה שמשנים אותה נשמרת מייד
            {
                if (isnew)
                {
                    if (txtNoteText.Text.Length < 1000)
                    {

                        HistoryChangeDetails h = new HistoryChangeDetails(
                      GLOBALVARS.MyUser.ID,
                      MyPeople.ID,
                      GLOBALVARS.MyUser.Name,
                      "הערה חדשה",
                     "",
                     txtNoteText.Text);
                        h.InserHistory();
                        People.InsertNewNotes(new NotesOfPeople() { NoteText = txtNoteText.Text, PeopleId = MyPeople.ID });
                        loadNotes();
                    }
                    else
                    {
                        MessageBox.Show("אורך הערה מקסימלי הוא עד 1000 תווים." + Environment.NewLine + "נא פצלו ל2 הערות");
                    }
                }
                else
                {
                    NotesOfPeople n = new NotesOfPeople()
                    {
                        NoteId = int.Parse(lstNotesOfPeople.SelectedItems[0].SubItems[3].Text),
                        UserId = int.Parse(lstNotesOfPeople.SelectedItems[0].Tag.ToString()),
                        UserName = lstNotesOfPeople.SelectedItems[0].SubItems[1].Text,
                        NoteText = txtNoteText.Text,
                        PeopleId = MyPeople.ID,
                        NoteDate = DateTime.Parse(lstNotesOfPeople.SelectedItems[0].SubItems[0].Text)
                    };
                    if (txtNoteText.Text.Length < 1000)
                    {
                        HistoryChangeDetails h = new HistoryChangeDetails(
                                GLOBALVARS.MyUser.ID,
                                MyPeople.ID,
                                GLOBALVARS.MyUser.Name,
                                "הערה - עדכון",
                                lstNotesOfPeople.SelectedItems[0].SubItems[2].Text,
                                txtNoteText.Text);
                        h.InserHistory();
                        People.UpdateNotes(n);
                        loadNotes();
                    }
                    else
                    {
                        MessageBox.Show("אורך הערה מקסימלי הוא עד 1000 תווים." + Environment.NewLine + "נא פצלו ל2 הערות");
                    }
                }
            }

        }

        private void btnNoteDelete_Click(object sender, EventArgs e)
        {

            if (lstNotesOfPeople.SelectedItems.Count <= 0)
                return;
            txtNoteText.Text = "";
            btnNoteSave.Enabled = false;

            if (MyPeople.OpenDetailsForAdd || MyPeople.OpenForPersonalAdd)
            {
                NotesOfPeople n = MyPeople.Note.Find(p => p.NoteId == int.Parse(lstNotesOfPeople.SelectedItems[0].SubItems[3].Text));
                MyPeople.Note.Remove(n);
                lstNotesOfPeople.Items.Remove(lstNotesOfPeople.SelectedItems[0]);
            }
            else
            {
                int id = int.Parse(lstNotesOfPeople.SelectedItems[0].SubItems[3].Text);
                DBFunction.Execute("delete from NotesOfPeople where NoteId=" + id);
                HistoryChangeDetails h = new HistoryChangeDetails(
                           GLOBALVARS.MyUser.ID,
                           MyPeople.ID,
                           GLOBALVARS.MyUser.Name,
                          "הערה - מחיקה",
                           lstNotesOfPeople.SelectedItems[0].SubItems[2].Text,
                           "");
                h.InserHistory();
                loadNotes();
            }
        }

        private void lstNotesOfPeople_Click(object sender, EventArgs e)
        {
            if (lstNotesOfPeople.SelectedItems.Count <= 0)
            {
                return;
            }
            txtNoteText.Text = lstNotesOfPeople.SelectedItems[0].SubItems[2].Text;
            btnNoteSave.Enabled = false;
            isnew = false;
            //txtNoteText.Text = "נכתבה על ידי: " + lstNotesOfPeople.SelectedItems[0].SubItems[1].Text + Environment.NewLine + "בתאריך: "
            //    + lstNotesOfPeople.SelectedItems[0].SubItems[0].Text + Environment.NewLine +"תוכן:" + Environment.NewLine + lstNotesOfPeople.SelectedItems[0].SubItems[2].Text;

        }

        private void txtNoteText_TextChanged(object sender, EventArgs e)
        {
            btnNoteSave.Enabled = true;
            if (lstNotesOfPeople.SelectedItems.Count <= 0)
                isnew = true;
        }
        public string setBirthday()
        {
            string year = txtBearthYear.Text;
            if (year.Contains("\""))
                year = year.Replace("\"", "");
            year = year.Insert(year.Length - 1, "\"");
            txtBearthYear.Text = year;
            return txtBearthDay.Text + "^" + txtBearthMonth.Text + "^" + year;

        }

        DateTime birthdate;

        public bool openReminder { get; set; } = false;
        public bool saveReminder { get; set; } = false;

        private void txtBearthYear_Leave(object sender, EventArgs e)
        {
            if (txtBearthDay.SelectedIndex >= 0 && txtBearthMonth.SelectedIndex >= 0 && txtBearthYear.Text.Length > 2)
            {
                setBirthday();
                string birth = txtBearthDay.Text + " " + txtBearthMonth.Text + " " + txtBearthYear.Text;
                HebrewCalendar hc = new HebrewCalendar();
                CultureInfo jewishCulture = CultureInfo.CreateSpecificCulture("he-IL");
                //  if(txtBearthDay.Text.Length>1)

                jewishCulture.DateTimeFormat.Calendar = hc;
                //   dt=hc.ToDateTime(birth,1,1,1,0,0,0)
                try
                {

                    birthdate = DateTime.Parse(birth, jewishCulture.DateTimeFormat);
                    txtBirthdateGregorian.Text = birthdate.ToShortDateString();
                    calcAge();
                }
                catch
                {
                    MessageBox.Show("התאריך שהזנת לא חוקי");
                }

            }
        }

        private void calcAge()
        {
            var today = DateTime.Today;
            int age = today.Year - birthdate.Year;
            int month;

            if (today.Month < birthdate.Month ||
                (today.Month == birthdate.Month && today.Day < birthdate.Day))
                age--;

            if (today.Month == birthdate.Month && today.Day < birthdate.Day)
                month = 12;
            else if (today.Month == birthdate.Month && today.Day > birthdate.Day)
                month = 0;
            else if (today.Month < birthdate.Month)
            {
                month = 12 - (birthdate.Month - today.Month);
            }
            else
                month = today.Month - birthdate.Month;

            if (month > 9)
                Age.DecimalPlaces = 2;
            else
                Age.DecimalPlaces = 1;
            Age.Value = decimal.Parse(age + "." + month);

        }

        private void lstMyActivity_DoubleClick(object sender, EventArgs e)
        {
            s.openShiduchActivityForm(lstMyActivity, MyPeople);
        }
        public ShiduchActivity s = new ShiduchActivity();
        public static string removeFromString(string s)
        {
            if (s.IndexOf("=====צד ב'=====") >= 0)
                return s.Substring(0, s.IndexOf("=====צד ב'====="));
            else return s;
        }

        private void radbtnLearn_Click(object sender, EventArgs e)
        {
            (sender as RadioButton).Checked = !(sender as RadioButton).Checked;
        }

        private void radbtnLearn_MouseClick(object sender, MouseEventArgs e)
        {

        }
        bool btnLicense;
        private void radbtnLicense_Click(object sender, EventArgs e)
        {
            btnLicense = !btnLicense;
            (sender as RadioButton).Checked = btnLicense;

        }
        bool btnSmoker;
        private void radbtnSmoker_Click(object sender, EventArgs e)
        {
            btnSmoker = !btnSmoker;
            (sender as RadioButton).Checked = btnSmoker;
        }
        bool btnBeard;
        private void radbtnBeard_Click(object sender, EventArgs e)
        {
            btnBeard = !btnBeard; if (btnNoBeard) btnNoBeard = false;
            (sender as RadioButton).Checked = btnBeard;
        }
        bool btnNoBeard;
        private void radbtnNoBeard_Click(object sender, EventArgs e)
        {
            btnNoBeard = !btnNoBeard;
            if (btnBeard) btnBeard = false;
            (sender as RadioButton).Checked = btnNoBeard;
        }
        private void DetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveCard)
            {
                if (MessageBox.Show("האם אתה רוצה לשמור?", "הכרטיס החדש",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    e.Cancel = true;
                }
                else//אם פתחו פעילות ולא שמרו - למנהל שיבדוק אם שדכן לוקח מספרי טלפון ולא מתעד
                {

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updatePhone = true;

        }
    }
}
