using System;
using System.Text;
namespace Substratum.Notification
{
    public interface IMessageRecipient
    {
        string Address { get; set; }
        string DisplayName { get; set; }
        MessagePropertyList Properties { get; set; }
        MessageRecipientType RecipientType { get; set; }
        MessageType MessageType { get; }
        void ToString(StringBuilder builder);
        System.Net.Mail.MailAddress GetEmailAddress();
    }
}
