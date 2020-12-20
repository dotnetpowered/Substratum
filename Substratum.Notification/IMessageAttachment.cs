using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public interface IMessageAttachment
    {
        string Filename { get; set; }
        byte[] Contents { get; set; }
        string ContentType { get; set; }
    }
}
