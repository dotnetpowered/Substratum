using System;

namespace Substratum.Notification
{
    public interface IMailMessage
    {
        string Bcc { get; set; }
        string BodyHtml { get; set; }
        string BodyText { get; set; }
        string Cc { get; set; }
        System.Net.Mail.MailPriority Priority { get; set; }
        string Subject { get; set; }
        string To { get; set; }
        bool IsBulk { get; set; }
        string From { get; set; }
        MessageSender Sender { get; set; }

        MessageRecipientList Recipients { get; }
        MessageBodyList Body { get; }
        MessageAttachmentList Attachments { get; }
        MessagePropertyList Properties { get; }
    }
}
