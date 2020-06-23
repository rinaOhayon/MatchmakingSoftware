using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Schiduch.Properties;
using System.Collections;
using Schiduch.Classes.Program;

namespace Schiduch
{
    public class Chat
    {
        WebBrowser web;
        NotifyIcon sicon;
        List<ChatMessage> msgs;
        public bool ChatDisable = false;
        Queue<ChatMessage> Qmsg=new Queue<ChatMessage>();
        private Timer ClockTimer = new Timer();
        public int Userid;
        bool Loading = true;
        bool FirstTime = true;
        ChatInternal cinternal = new ChatInternal();
        public void CorrectRegisterForCss3()
        {
            //need to add
        }
        string ConvertToHTML(string str)
        {
            return str.Replace("\r\n", "<br />");
            
        }
        public Chat(WebBrowser sweb,int Userid,NotifyIcon icon)
        {
            LoadSettings();
           
            cinternal.USERID = Userid;
            this.Userid = Userid;
            msgs = new List<ChatMessage>();
            if (sweb != null)
                web = sweb;
            
            else
                throw new Exception("The WEBBROWSER control is null");
            web.IsWebBrowserContextMenuEnabled = false;
            sicon = icon;
            InsertBody();
            web.DocumentCompleted += Web_DocumentCompleted;
            msgs.AddRange(cinternal.GetAllMessagesFromFile());
            msgs.AddRange(cinternal.GetAllMessagesFromDataBase());
            ClockTimer.Interval = 5000;
            ClockTimer.Tick += ClockTimer_Tick;
            if (ChatDisable)
                return;
            ClockTimer.Enabled = true;
            

        }
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            GetMessages();
        }

        private void ResetLastCheck(DateTime time,bool Load=true)
        {
            if (Load)
                GetMessages();
            cinternal.LastCheck = time;
            ResetTimer();
        }
        private void ResetTimer()
        {
            ClockTimer.Enabled = false;
            ClockTimer.Enabled = true;
        }
        private void Web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {  
            Loading = false;
            InsertQueue();
        }

        private void InsertQueue()
        {
            while(Qmsg.Count > 0) {
                if (((ChatMessage)Qmsg.Peek()).From == Userid)
                    InsertMessage(Qmsg.Dequeue());
                else
                    InsertRecviedmessage(Qmsg.Dequeue());
            }
        }
        
        private bool IsMeChatOwner(ChatMessage chat)
        {
            if (Userid == chat.From)
                return true;
            return false;
        }
        private bool IsMeChatOwner(int ChatFrom)
        {
            if (Userid == ChatFrom)
                return true;
            return false;
        }
        private void InsertBody()
        {
            web.DocumentText = Resources.ResourceManager.GetString("ChatWeb");
                
                 
        }
        private void InsertMessage(ChatMessage chat)
        {
            
            string Content = ConvertToHTML(chat.Content);
            HtmlElement elm = web.Document.CreateElement("Li");
               elm.InnerHtml = "<div class='bubble'><span class='personSay'>" +
                 Content
               + "</span><br /><p class='time'>" + chat.Date.ToString() + "</p> </div>";
          
            if (!Loading)
                web.Document.GetElementById("container").AppendChild(elm);
            else
                Qmsg.Enqueue(chat);
            
            ScrollDown();
        }
        private void InsertRecviedmessage(ChatMessage chat)
        {
            HtmlElement elm = web.Document.CreateElement("div");
            string Content=ConvertToHTML(chat.Content);
            elm.InnerHtml = "<LI class='lileft'><div dir='rtl' class='bubble2'> <span class='personName2'>" 
                +ConvertIdToUsername(chat.From) + " :</span> <br><span class='personSay2'>" +
                Content + "</span><br /><p class='time'>" + chat.Date.ToString() + "</p> </div></LI>";
            if (!Loading) { 
                web.Document.GetElementById("container").AppendChild(elm);
                MakeSound();
            }
            else
                Qmsg.Enqueue(chat);
            ScrollDown();
        }

        private void MakeSound()
        {
            
            if (File.Exists(Application.StartupPath + "\\Sound.wav")) { 
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Application.StartupPath + "\\Sound.wav");
            player.Play();
            }
        }

        public void AddMessage(string Msg,int To=0)
        {
            if (string.IsNullOrEmpty(Msg))
            {
                MessageBox.Show("ההודעה לא יכולה להיות ריקה", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            cinternal.InsertMessageToDatabase(Msg, To);
            ResetLastCheck(DateTime.Now);
        }

        public void GetMessages()
        {
            List<ChatMessage> newmsgs = new List<ChatMessage>();
            if (FirstTime) { 
                newmsgs.AddRange(cinternal.GetAllMessagesFromFile());
                List<ChatMessage> t = cinternal.GetAllMessagesFromDataBase();
                if (sicon != null && t.Count>0)
                {
                    sicon.ShowBalloonTip(5000, "לוח מודעות", sicon.Text = "יש לך " + t.Count + " הודעות חדשות", ToolTipIcon.Info);
                }
                newmsgs.AddRange(t);
                FirstTime = false;
            }
            else
            {
                List<ChatMessage> t = cinternal.GetAllMessagesFromDataBase();
                if (sicon != null && t.Count > 0)
                {
                    
                    sicon.ShowBalloonTip(5000,"לוח מודעות", sicon.Text = "יש לך " + t.Count + " הודעות חדשות",ToolTipIcon.Info);
                }
                newmsgs.AddRange(t);
            }
                
            msgs.AddRange(newmsgs);
            foreach(ChatMessage chat in newmsgs)
            {
                if (IsMeChatOwner(chat))
                    InsertMessage(chat);
                else
                    InsertRecviedmessage(chat);
            }
        }
        void ScrollDown()
        {
          //  web.Select();
           // web.Focus();
           // SendKeys.Send("{END}");
            web.Document.Window.ScrollTo(0, 10000);
        }

        public void ZoomIn()
        {
           // web.Document.Body.Style += "zoom:150%";
        }
        string ConvertIdToUsername(int UserId)
        {
            foreach(KeyValueClass k in GLOBALVARS.Shadchanim)
            {
                if ((int)k.Value == UserId)
                    return k.Text;
            } 
            return "משרד";
        }

        public bool UpdateMsg(string txt,int chatid,int userid)
        {
            return cinternal.UpdateMsg(txt, chatid, userid);
        }
        public bool DeleteMsg(int chatid, int userid)
        {
            return cinternal.DeleteMsg(chatid, userid);
        }
        private void LoadSettings()
        {
            if (!File.Exists("ChatSettings.bin"))
                WriteSettings();
            string[] data = File.ReadAllLines("ChatSettings.bin");
            ChatDisable = bool.Parse(data[0]);
        }
        private void WriteSettings()
        {
            File.WriteAllText("ChatSettings.bin", ChatDisable.ToString() + "\r\n");
        }
        public void DisableChat()
        {
            ChatDisable = true;
            WriteSettings();
            ClockTimer.Enabled = false;
        }
        public void EnableChat()
        {
            ChatDisable = false;
            WriteSettings();
            ClockTimer.Enabled = true;
        }

        public List<KeyValueClass> GetMyMessages()
        {
            List<KeyValueClass> lst;
            ClockTimer.Enabled = false;
            lst=cinternal.GetAllMyMessages();
            ClockTimer.Enabled = true;
            return lst;
        }
        public void Close()
        {
            cinternal.Close();
        }
        
    }
}
