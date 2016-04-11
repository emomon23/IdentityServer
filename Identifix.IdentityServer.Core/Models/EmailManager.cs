using Identifix.IdentityServer.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Identifix.IdentityServer.Models
{
    public class EmailManager : IEmailManager
    {
        public string SendEmail(string to, string from, string subject, string body)
        {
            DeleteOtherResetPasswordEmails();

            // TODO: Setup SMTP;            
            MailMessage message = new MailMessage(from, to, subject, body);
            //message.Headers.
            using (SmtpClient client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = AppSettings.TemporaryEmailFolder;
                client.Send(message);
            }

            return GetEmailFileName();
        }

        private string GetEmailFileName()
        {
            var filename = Directory.GetFiles(AppSettings.TemporaryEmailFolder).Single(x => !x.Contains("ReadMe.txt"));
            FileInfo fi = new FileInfo(filename);
            return fi.Name;
        }

        private void DeleteOtherResetPasswordEmails()
        {
            foreach(var file in Directory.GetFiles(AppSettings.TemporaryEmailFolder))
            {         
                if(!file.Contains("ReadMe.txt"))      
                    File.Delete(file);
            }
        }
    }
}
