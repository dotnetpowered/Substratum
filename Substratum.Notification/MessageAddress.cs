using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessageAddress
    {
        public MessageAddress()
            : base()
        { }

        public MessageAddress(string CombinedNameAddress)
        {
            Substratum.Text.EmailAddressParser.SplitAddress(CombinedNameAddress, out _DisplayName, out _Address);
        }

        public MessageAddress(string DisplayName, string Address)
        {
            this.DisplayName = DisplayName;
            this.Address = Address;
        }

        private string _DisplayName;

        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        private MessagePropertyList _PropertyList = new MessagePropertyList();

        public MessagePropertyList Properties
        {
            get { return _PropertyList; }
            set { _PropertyList = value; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            ToString(builder);
            if (builder.Length == 0)
                return null;
            else
                return builder.ToString();
        }

        public void ToString(StringBuilder builder)
        {
            if (Address != null)
            {
                if (DisplayName == null)
                    builder.Append(Address);
                else
                {
                    builder.Append(DisplayName).Append(" [").Append(Address).Append("]");
                }
            }
        }

        public virtual System.Net.Mail.MailAddress GetEmailAddress()
        {
            return new System.Net.Mail.MailAddress(Address, DisplayName);
        }

    }

}
