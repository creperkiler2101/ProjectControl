using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ProjectControl
{
    public class Email
    {
        public static void Send(string to, string subject, string message)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("slimik043568@gmail.com", "masterpvp12332138"),
                EnableSsl = true,
            };

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("slimik043568@gmail.com");
            mail.IsBodyHtml = true;
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = message;

            SmtpServer.Send(mail);
        }

        public static bool Validate(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}