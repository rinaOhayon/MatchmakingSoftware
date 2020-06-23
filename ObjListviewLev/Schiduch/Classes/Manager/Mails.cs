using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Schiduch
{
    class Mails
    {
        public static void SendMail()
        {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("nevermind@gmail.com","שנתבשר");
                mail.To.Add("eli.vb.92@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = "This is for testing SMTP mail from GMAIL";

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Credentials = new System.Net.NetworkCredential("shenitbaser@gmail.com", "036779708");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                

        }
    }
}
