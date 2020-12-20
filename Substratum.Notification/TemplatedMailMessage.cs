using System;
using System.Xml;
using System.Collections;

namespace Substratum.Notification
{
    [Serializable]
    public class TemplatedMailMessage : MailMessage
    {
        private string FirstLetterCaps(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            else
                if (s.Length == 1)
                    return s.ToUpper();
                else
                    return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }

        static string GetInnerText(XmlNode node, string defaultValue = null)
        {
            if (node == null)
                return defaultValue;
            else
                return node.InnerText;
        }

        public TemplatedMailMessage(MessageTemplate template, Hashtable Data) : base()
        {
            string msgText = template.Transform(Data);

            XmlDocument Msg = new XmlDocument();
            Msg.LoadXml(msgText);

            this.To = GetInnerText(Msg.SelectSingleNode("/Root/MailHeader/To"));
            this.From = GetInnerText(Msg.SelectSingleNode("/Root/MailHeader/From"));
            this.Cc = GetInnerText(Msg.SelectSingleNode("/Root/MailHeader/Cc"));
            this.Bcc = GetInnerText(Msg.SelectSingleNode("/Root/MailHeader/Bcc"));
            this.Subject = GetInnerText(Msg.SelectSingleNode("/Root/MailHeader/Subject"));
            this.Priority = (System.Net.Mail.MailPriority) Enum.Parse(typeof(System.Net.Mail.MailPriority),
                GetInnerText(Msg.SelectSingleNode("/Root/MailHeader/Priority"), "Normal"));
            XmlNode BodyNode = Msg.SelectSingleNode("/Root/Body");
            foreach (XmlNode node in BodyNode.ChildNodes)
            {
                if (node is XmlElement)
                {
                    MessageBodyType t = (MessageBodyType)Enum.Parse(typeof(MessageBodyType), FirstLetterCaps(node.Name));
                    Body.Add(t, node.InnerXml);
                }
            }
            //this.BodyHtml = XmlTools.GetOuter(Msg.SelectSingleNode("/Root/Body/html"));
            //this.BodyText = XmlTools.GetInnerText(Msg.SelectSingleNode("/Root/Body/text"));

            if (Data != null)
            {
                foreach (DictionaryEntry entry in Data)
                {
                    if (entry.Value != null)
                        this.Properties.Add(entry.Key.ToString(), entry.Value.ToString());
                }
            }
        }
    }

}
