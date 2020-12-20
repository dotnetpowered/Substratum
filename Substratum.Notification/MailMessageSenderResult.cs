using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MailMessageSenderResult
    {
        MailMessageSenderStatus _Status;
        MailMessageRecipientResultList _RecipientResults = new MailMessageRecipientResultList();

        public MailMessageSenderResult()
        {
        }

        public MailMessageSenderResult(
                MailMessageSenderStatus Status)
        {
            _Status = Status;
        }

        public MailMessageSenderResult(
                MailMessageSenderStatus Status, 
                string ExtendedInfo,
                MessageRecipientList Recipients,
                Type SentBy
                )
        {
            _Status = Status;
            MailMessageRecipientResult result = new MailMessageRecipientResult(Status, ExtendedInfo);
            result.Recipients.AddRange(Recipients);
            result.SentBy = SentBy.FullName;
            _RecipientResults.Add(result);
        }

        #region IMailMessageSenderResult Members

        public MailMessageSenderStatus Status
        {
            get 
            {
                int successCount = 0;
                foreach (MailMessageRecipientResult subResult in RecipientResults)
                {
                    if (subResult.Status == MailMessageSenderStatus.Sent)
                        successCount++;
                }
                if (successCount == RecipientResults.Count)
                    return MailMessageSenderStatus.Sent;
                else
                {
                    if (successCount == 0)
                        return MailMessageSenderStatus.Failed;
                    else
                        return MailMessageSenderStatus.Partial;
                }
            }
        }

        public MailMessageRecipientResultList RecipientResults
        {
            get { return _RecipientResults; }
        }


        #endregion
    }
}
