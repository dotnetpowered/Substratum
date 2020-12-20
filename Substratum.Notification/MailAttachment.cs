using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    [Serializable]
    public class MailAttachment : IMessageAttachment 
    {
        string _Filename;
        byte[] _Contents;
        string _ContentType;

        public MailAttachment()
        {
        }

        public MailAttachment(string Filename, byte[] Contents)
        {
            this.Filename = Filename;
            this.Contents = Contents;
        }


        #region IMailAttachment Members

        public string ContentType 
        {
            get
            {
                return _ContentType;
            }
            set
            {
                _ContentType = value;
            }
        }

        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                _Filename = value;
            }
        }

        public byte[] Contents
        {
            get
            {
                return _Contents;
            }
            set
            {
                _Contents = value;
            }
        }

        #endregion
    }
}
