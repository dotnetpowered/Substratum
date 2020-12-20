using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessagePropertyList : List<IMailMessageProperty>
    {
        public void Add(string PropertyName, string PropertyValue)
        {
            Add(new MailMessageProperty(PropertyName, PropertyValue));
        }
    }
}
