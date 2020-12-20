using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public interface IMailMessageSenderFactory
    {
        IMailMessageSender CreateMessageSender(MessageType MessageType, bool IsBulk);
        IMailMessageSender CreateMessageSender();
    }
}
