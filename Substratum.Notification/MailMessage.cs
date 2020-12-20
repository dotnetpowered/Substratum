using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    [Serializable]
    public class MailMessage : IMailMessage 
    {
        public string To
        {
            get { return Recipients.GetRecipients(MessageRecipientType.To); }
            set { Recipients.SetRecipients(MessageRecipientType.To, value); }
        }

        public string Cc
        {
            get { return Recipients.GetRecipients(MessageRecipientType.Cc); }
            set { Recipients.SetRecipients(MessageRecipientType.Cc, value); }
        }

        public string Bcc
        {
            get { return Recipients.GetRecipients(MessageRecipientType.Bcc); }
            set { Recipients.SetRecipients(MessageRecipientType.Bcc, value); }
        }
        private MessageSender _Sender = new MessageSender();

        public MessageSender Sender
        {
            get { return _Sender; }
            set { _Sender = value; }
        }

        public string From
        {
            get { return _Sender.ToString(); }
            set { _Sender = new MessageSender(value); }
        }
        private System.Net.Mail.MailPriority _Priority;

        public System.Net.Mail.MailPriority Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        MessageRecipientList _RecipientList = new MessageRecipientList();

        public MessageRecipientList Recipients
        {
            get { return _RecipientList; }
        }

        MessageAttachmentList _Attachments = new MessageAttachmentList();

        public MessageAttachmentList Attachments
        {
            get { return _Attachments; }
            set { _Attachments = value; }
        }

        MessagePropertyList _Properties = new MessagePropertyList();

        public MessagePropertyList Properties
        {
            get { return _Properties; }
            set { _Properties = value; }
        }

        MessageBodyList _MessageBodyList = new MessageBodyList();

        public string BodyText
        {
            get 
            {
                return _MessageBodyList.GetBodyText(MessageBodyType.Text);
            }
            set 
            {
                _MessageBodyList.SetBodyText(MessageBodyType.Text, value);
            }
        }

        public string BodyHtml
        {
            get
            {
                return _MessageBodyList.GetBodyText(MessageBodyType.Html);
            }
            set
            {
                _MessageBodyList.SetBodyText(MessageBodyType.Html, value);
            }
        }

        public MessageBodyList Body
        {
            get
            {
                return _MessageBodyList;
            }
        }

        private string _Subject;

        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        bool _IsBulk;

        public bool IsBulk
        {
            get { return _IsBulk; }
            set { _IsBulk = value; }
        }

        public MailMessage()
        {
        }

    }



}
