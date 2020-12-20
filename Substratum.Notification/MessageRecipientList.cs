using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessageRecipientList : List<IMessageRecipient>
    {
        public IMessageRecipient[] this[MessageType MessageType]
        {
            get
            {
                List<IMessageRecipient> recipients = new List<IMessageRecipient>();
                foreach (IMessageRecipient recipient in this)
                {
                    if (recipient.MessageType == MessageType)
                    {
                        recipients.Add(recipient);
                    }
                }
                return recipients.ToArray();
            }
        }

        public string GetRecipients(MessageRecipientType RecipientType)
        {
            StringBuilder builder = new StringBuilder();
            foreach (IMessageRecipient recipient in this)
            {
                if (recipient.RecipientType == RecipientType)
                {
                    if (builder.Length > 0)
                        builder.Append(";");
                    recipient.ToString(builder);
                }
            }
            return builder.ToString();
        }

        public void SetRecipients(MessageRecipientType RecipientType, string Recipients)
        {
            // Remove existing recipients of this type
            List<IMessageRecipient> RemovalList=new List<IMessageRecipient>();
            foreach (IMessageRecipient recipient in this)
            {
                if (recipient.RecipientType == RecipientType)
                {
                    RemovalList.Add(recipient);
                }
            }
            foreach (IMessageRecipient recipient in RemovalList)
            {
                this.Remove(recipient);
            }
            if (Recipients != null)
            {
                // Add new recipients
                string[] RecipientList = Recipients.Split(';');
                foreach (string recipient in RecipientList)
                {
                    this.Add(new MessageRecipient(RecipientType, recipient));
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (MessageRecipientType t in Enum.GetValues(typeof(MessageRecipientType)))
            {
                string s = GetRecipients(t);
                if (s.Length > 0)
                {
                    sb.Append(t).Append(":").Append(s).Append(";");
                }
            }
            return sb.ToString();            
        }
    }
}
