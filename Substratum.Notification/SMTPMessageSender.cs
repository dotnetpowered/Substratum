using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Substratum.Notification
{
    public class SMTPMailSender : RetryMailSender
    {
        public SMTPMailSender()
        {
        }

        protected override MailMessageSenderResult Transmit(IMailMessage msg)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            MailMessageAdapter.CopyMailMessage(msg, mailMessage);

            //string smtpHost = System.Configuration.ConfigurationManager.AppSettings["Mail.smtpServer"];
            //int smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Mail.smtpPort"]);
            
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();//(smtpHost,smtpPort);
            client.Send(mailMessage);

            return new MailMessageSenderResult(MailMessageSenderStatus.Sent, null, msg.Recipients, this.GetType());
        }
    }
}
