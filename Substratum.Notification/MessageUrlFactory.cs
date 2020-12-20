using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessageUrlFactory
    {
        public static string GetMailToUrl(IMailMessage msg)
        {
            return "mailto:?subject=" + msg.Subject + "&body=" + msg.BodyText.Replace("\r\n", "%0A");
        }
    }
}
