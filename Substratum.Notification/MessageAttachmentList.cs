using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessageAttachmentList : List<IMessageAttachment>
    {
        public void Add(string Filename, byte[] Contents)
        {
            this.Add(new MailAttachment(Filename, Contents));
        }

        public void Add(string Filename)
        {
            this.Add(new MailAttachment(Filename, System.IO.File.ReadAllBytes(Filename)));
        }
    }
}
