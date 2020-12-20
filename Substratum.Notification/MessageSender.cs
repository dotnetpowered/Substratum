using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessageSender : MessageAddress
    {
        public MessageSender()
            : base()
        { }

        public MessageSender(string CombinedNameAddress)
            : base(CombinedNameAddress)
        {
        }

        public MessageSender(string DisplayName, string Address)
            : base(DisplayName, Address)
        {
        }
    }
}
