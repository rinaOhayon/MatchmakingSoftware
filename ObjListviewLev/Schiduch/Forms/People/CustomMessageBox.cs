using Schiduch.Classes.Program;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schiduch.Forms
{
    public partial class CustomMessageBox : Form
    {
        Label message = new Label();
        Button b1 = new Button();
        Button b2 = new Button();
        public bool cancel = false;
        public People p = new People();
        public int Sexs { get; set; }
        /// <summary>
        /// להוספת בחור/ה חדשים משמש לבחירת המין
        /// </summary>
        public CustomMessageBox()
        {
            InitializeComponent();
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Sexs = 1;
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Sexs = 2;
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Sexs = 0;
            Close();
        }
        /// <summary>
        /// להוספת למאגר אישי או למאגר כללי
        /// </summary>
        /// <param name="label1">הטקסט שיופיע</param>
        /// <param name="button1">טקסט כפתור 1</param>
        /// <param name="button2">טקסט כפתור 2</param>
        public CustomMessageBox(string label1, string button1, string button2)
        {
            InitializeComponent();
            this.label1.Text = label1;
            this.button1.Text = button1;
            this.button2.Text = button2;
            this.button3.Click += button3_Click_Cancel;
            this.button1.Click += button1_Click_Add_Data;
            this.button2.Click += button2_Click_Add_Data;
            this.button1.Height = 55;
            this.button2.Height = 55;
            this.button3.Height = 55;
            //if (!GLOBALVARS.MyUser.Control)
            //    this.button1.Visible = false;


        }
        /// <summary>
        /// הוספה למאגר הכללי
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_Add_Data(object sender, EventArgs e)
        {
            p.OpenDetailsForAdd = true;
            p.Temp = GLOBALVARS.MyUser.TempGeneral;
            Close();
            cancel = true;
        }
        /// <summary>
        /// הוספה למאגר האישי
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_Add_Data(object sender, EventArgs e)
        {
            p.OpenForPersonalAdd = true;
            p.Temp = GLOBALVARS.MyUser.TempPersonal;
            Close();
            cancel = true;
        }
        private void button3_Click_Cancel(object sender, EventArgs e)
        {
            button3_Click(new object(), new EventArgs());
        }
    }
}
