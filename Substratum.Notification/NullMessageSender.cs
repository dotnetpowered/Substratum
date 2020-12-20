using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class NullMessageSender : IMailMessageSender
    {
        #region IMailMessageSender Members

        public MailMessageSenderResult Send(IMailMessage msg)
        {
            System.Diagnostics.Debug.Write("NullMessageSender: Message to " + msg.To + " was NOT sent.");
            return new MailMessageSenderResult(MailMessageSenderStatus.Sent, "Not sent", msg.Recipients, this.GetType());
        }

        #endregion
    }
}
