using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class RedirectSMTPMessageSender : SMTPMailSender
    {
        public RedirectSMTPMessageSender() : base()
        {
        }

        public static string RedirectTo
        {
            get; set;

            // TODO: read configuration

            //{
            //    return System.Configuration.ConfigurationManager.AppSettings["Messaging.Email.RedirectTo"];
            //}
        }

        #region IMailMessageSender Members

        protected override MailMessageSenderResult Transmit(IMailMessage msg)
        {
            msg.To = RedirectTo;
            return base.Transmit(msg);
        }

        #endregion
    }
}
