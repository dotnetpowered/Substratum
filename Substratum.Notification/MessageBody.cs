using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public class MessageBody
    {
        public MessageBody(string Text, MessageBodyType BodyType)
        {
            this.Text = Text;
            this.BodyType = BodyType;
        }

        string _Text;
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        MessageBodyType _BodyType;

        public MessageBodyType BodyType
        {
            get { return _BodyType; }
            set { _BodyType = value; }
        }
    }
}
