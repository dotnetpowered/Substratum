using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class SmartMessageSender : IMailMessageSender
    {
        #region IMailMessageSender Members

        public MailMessageSenderResult Send(IMailMessage msg)
        {
            List<MessageType> DistinctSenders = new List<MessageType>();

            foreach (IMessageRecipient recipient in msg.Recipients)
            {
                if (!DistinctSenders.Contains(recipient.MessageType))
                    DistinctSenders.Add(recipient.MessageType);
            }

            MailMessageSenderResult result = new MailMessageSenderResult();

            foreach (MessageType messageType in DistinctSenders)
            {
                // Create new message with only one type of recipients
                MailMessage FilteredMsg = new MailMessage();
                FilteredMsg.Recipients.AddRange(msg.Recipients[messageType]);
                FilteredMsg.Sender = msg.Sender;
                FilteredMsg.Attachments.AddRange(msg.Attachments);
                FilteredMsg.Body.AddRange(msg.Body);
                FilteredMsg.IsBulk = msg.IsBulk;
                FilteredMsg.Subject = msg.Subject;

                MailMessageSenderResult senderResult = Transmit(messageType, FilteredMsg);
                result.RecipientResults.AddRange(senderResult.RecipientResults);
            }

            return result;
        }

        protected virtual MailMessageSenderResult Transmit(MessageType messageType, MailMessage FilteredMsg)
        {
            // Create sender for the message type
            IMailMessageSender sender = MailMessageSenderFactory.CreateMessageSender(messageType, FilteredMsg.IsBulk);
            MailMessageSenderResult senderResult = sender.Send(FilteredMsg);
            return senderResult;
        }

        #endregion
    }
}
