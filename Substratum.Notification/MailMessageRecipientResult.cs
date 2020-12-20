using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MailMessageRecipientResult
    {
        public MailMessageRecipientResult(MailMessageSenderStatus Status, string ExtendedInfo)
        {
            _Status = Status;
            _ExtendedInfo = ExtendedInfo;
        }
        MailMessageSenderStatus _Status;

        public MailMessageSenderStatus Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        string _ExtendedInfo;
        public string ExtendedInfo
        {
            get { return _ExtendedInfo; }
            set { _ExtendedInfo = value; }
        }

        string _SentBy;
        public string SentBy
        {
            get { return _SentBy; }
            set { _SentBy = value; }
        }
        MessageRecipientList _Recipients = new MessageRecipientList();

        public MessageRecipientList Recipients
        {
            get { return _Recipients; }
        }

    }

    public class MailMessageRecipientResultList : List<MailMessageRecipientResult>
    {
    }
}
