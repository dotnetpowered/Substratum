using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessageBodyList : Dictionary<MessageBodyType, string>
    {
        public string GetBodyText(MessageBodyType BodyType)
        {
            string BodyText;
            if (!ContainsKey(BodyType))
            {
                if (BodyType != MessageBodyType.Text && BodyType != MessageBodyType.Html)
                {
                    if (ContainsKey(MessageBodyType.Text))
                        BodyText = this[MessageBodyType.Text];
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                BodyText = this[BodyType];

            if (BodyType != MessageBodyType.Html)
                return BodyText.Replace("&amp;", "&");
            else
                return BodyText;
        }

        public void SetBodyText(MessageBodyType BodyType, string Text)
        {
            this[BodyType] = Text;
        }

        public void AddRange(MessageBodyList messageBodyList)
        {
            foreach (KeyValuePair<MessageBodyType, string> b in messageBodyList)
            {
                this.Add(b.Key, b.Value);
            }
        }
    }

}
