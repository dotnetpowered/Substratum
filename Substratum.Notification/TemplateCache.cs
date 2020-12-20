using System;
using System.Collections.Generic;
using System.Text;
using Substratum.Notification;

namespace Substratum.Notification
{
    public class MessageTemplateCache
    {
        static Dictionary<string, MessageTemplate> cache = new Dictionary<string, MessageTemplate>();

        public static T Template<T>() where T : MessageTemplate, new()
        {
            MessageTemplate m;
            string TypeName = typeof(T).FullName;

            lock (cache)
            {
                if (!cache.TryGetValue(TypeName, out m))
                {
                    m = new T();
                    cache.Add(TypeName, m);
                }
            }
            return m as T;            
        }
    }

}
