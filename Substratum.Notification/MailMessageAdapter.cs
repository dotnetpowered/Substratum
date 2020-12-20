using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Substratum.Notification
{
    public class MailMessageAdapter
    {
        public static void CopyMailMessage(IMailMessage msg, System.Net.Mail.MailMessage mailMessage)
        {
            if (msg.From != null)
                mailMessage.From = new System.Net.Mail.MailAddress(msg.Sender.Address, msg.Sender.DisplayName);
            mailMessage.Priority = msg.Priority;
            mailMessage.Subject = msg.Subject;
            mailMessage.IsBodyHtml = (msg.BodyHtml != null);
            if (mailMessage.IsBodyHtml)
                mailMessage.Body = msg.BodyHtml;
            else
                mailMessage.Body = msg.BodyText;
            foreach (IMessageRecipient r in msg.Recipients)
            {
                if (r.RecipientType == MessageRecipientType.Cc)
                    mailMessage.CC.Add(r.GetEmailAddress());
                if (r.RecipientType == MessageRecipientType.To)
                    mailMessage.To.Add(r.GetEmailAddress());
                if (r.RecipientType == MessageRecipientType.Bcc)
                    mailMessage.Bcc.Add(r.GetEmailAddress());
            }
            if (msg.Attachments != null)
            {
                foreach (IMessageAttachment attachment in msg.Attachments)
                {
                    MemoryStream stream = new MemoryStream(attachment.Contents);
                    mailMessage.Attachments.Add(new System.Net.Mail.Attachment(
                        stream, new System.Net.Mime.ContentType(attachment.ContentType)));
                }
            }
        }
    }
}
