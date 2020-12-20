using System;

namespace Substratum.Notification
{
    public interface IMailMessageSender
    {
        MailMessageSenderResult Send(IMailMessage msg);
    }
}
