using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public interface IMailMessageProperty
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}
