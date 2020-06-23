using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Schiduch
{
   public partial  class MyTextBox:TextBox
    {
        public MyTextBox()
        {
            
            BorderStyle = System.Windows.Forms.BorderStyle.None;
            AutoSize = false; //Allows you to change height to have bottom padding
            Controls.Add(new Label()
            { Height = 1, Dock = DockStyle.Bottom, BackColor = Color.FromArgb(200,200,200) });
            //SetStyle(ControlStyles.SupportsTransparentBackColor |
            //     ControlStyles.OptimizedDoubleBuffer |
            //     ControlStyles.AllPaintingInWmPaint |
            //     ControlStyles.ResizeRedraw |
            //     ControlStyles.UserPaint, true);
           // BackColor = Color.Transparent;
            RightToLeft = RightToLeft.Yes;
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    var backgroundBrush = new SolidBrush(Color.Transparent);
        //    Graphics g = e.Graphics;
        //    g.FillRectangle(backgroundBrush, 0, 0, this.Width, this.Height);
        //    g.DrawString(Text, Font, new SolidBrush(ForeColor), new PointF(0, 0), StringFormat.GenericDefault);
        //}
    }
    public class MyComboBox : ComboBox
    {
        public MyComboBox()
        {
            FlatStyle = FlatStyle.Flat;
            //BorderStyle = System.Windows.Forms.BorderStyle.None;
            AutoSize = false; //Allows you to change height to have bottom padding
            Controls.Add(new Label()
            { Height = 1, Dock = DockStyle.Bottom, BackColor = Color.FromArgb(200, 200, 200) });
            
        }
      
    }
    public class MyNumericUpDown: NumericUpDown
    {
        public MyNumericUpDown()
        {
           BorderStyle = BorderStyle.None;
            AutoSize = false; //Allows you to change height to have bottom padding
            Controls.Add(new Label()
            { Height = 1, Dock = DockStyle.Bottom, BackColor = Color.FromArgb(200, 200, 200) });
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //BackColor = Color.Transparent;
            
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    var backgroundBrush = new SolidBrush(Color.Transparent);
        //    Graphics g = e.Graphics;
        //    g.FillRectangle(backgroundBrush, 0, 0, this.Width, this.Height);
        //    g.DrawString(Text, Font, new SolidBrush(ForeColor), new PointF(0, 0), StringFormat.GenericDefault);
        //}

    }
    public class FixedTabControl : TabControl
    {
        [DllImportAttribute("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);

        protected override void OnHandleCreated(EventArgs e)
        {
            SetWindowTheme(this.Handle, "", "");
            base.OnHandleCreated(e);
        }
    }
}
