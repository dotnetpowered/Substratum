using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessageRecipient : MessageAddress, IMessageRecipient
    {
        public MessageRecipient(string DisplayName, string Address, MessageRecipientType RecipientType) :
            base(DisplayName, Address)
        {
            this.RecipientType = RecipientType;
        }

        public MessageRecipient(string Address, MessageRecipientType RecipientType) : base(null, Address)
        {
            this.RecipientType = RecipientType;
        }

        public MessageRecipient(MessageRecipientType RecipientType, string CombinedNameAddress) : 
            base(CombinedNameAddress)
        {
            this.RecipientType = RecipientType;
        }

        MessageRecipientType _RecipientType;

        public MessageRecipientType RecipientType
        {
            get { return _RecipientType; }
            set {
 
                _RecipientType = value; 
            }
        }

        public MessageType MessageType
        {
            get
            {
                switch (_RecipientType)
                {
                    case MessageRecipientType.To:
                    case MessageRecipientType.Cc:
                    case MessageRecipientType.Bcc:
                        return MessageType.Email;
                    case MessageRecipientType.Fax:
                        return MessageType.Fax;
                    case MessageRecipientType.SMS:
                        return MessageType.SMS;
                    case MessageRecipientType.Tts:
                        return MessageType.Tts;
                    default:
                        throw new InvalidOperationException("Unknown recipient type: " + _RecipientType);
                }
            }
        }

        public override System.Net.Mail.MailAddress GetEmailAddress()
        {
            if (MessageType == MessageType.Email)
            {
                if (DisplayName == null)
                    return new System.Net.Mail.MailAddress(Address);
                else
                    return new System.Net.Mail.MailAddress(Address, DisplayName);
            }
            else
                throw new InvalidOperationException("Message address is not an email address.");
        }

    }
}
