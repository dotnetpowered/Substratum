using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    [Serializable]
    public class MailMessageProperty : IMailMessageProperty
    {
        string _Name, _Value;

        public MailMessageProperty()
        {
        }

        public MailMessageProperty(string Name, string Value)
        {
            _Name = Name;
            _Value = Value;
        }

        #region IMessageProperty Members

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        #endregion
    }
}
