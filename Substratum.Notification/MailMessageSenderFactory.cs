using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Notification
{
    public static class MailMessageSenderFactory
    {
#if FullTrust
        static IMailMessageSenderFactory _factory = new DynamicMessageSenderFactory();
#else
        static IMailMessageSenderFactory _factory = new DefaultMessageSenderFactory();
#endif
        public static IMailMessageSenderFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }
        public static IMailMessageSender CreateImmediateEmailMessageSender()
        {
            return Factory.CreateMessageSender(MessageType.Email, false);
        }

        public static IMailMessageSender CreateBulkEmailMessageSender()
        {
            return Factory.CreateMessageSender(MessageType.Email, true);
        }

        public static IMailMessageSender CreateMessageSender()
        {
            return Factory.CreateMessageSender();
        }

        public static IMailMessageSender CreateMessageSender(MessageType MessageType, bool IsBulk)
        {
            return Factory.CreateMessageSender(MessageType, IsBulk);
        }

    }

#if FullTrust
    public class DynamicMessageSenderFactory : IMailMessageSenderFactory
    {
        private static Dictionary<string, Type> SenderTypeCache = new Dictionary<string, Type>();

        public IMailMessageSender CreateMessageSender()
        {
            Type senderType;
            string SenderKey = "Messaging.DefaultSender";
            lock (SenderTypeCache)
            {
                if (!SenderTypeCache.TryGetValue(SenderKey, out senderType))
                {
                    string MessageSender = System.Configuration.ConfigurationManager.AppSettings[SenderKey];
                    if (MessageSender != null)
                    {
                        // Attempt to load type
                        senderType = Type.GetType(MessageSender);
                        if (senderType == null)
                            throw new InvalidOperationException("Unable to create message sender: " + MessageSender);
                    }
                    else
                    {
                        senderType = typeof(SmartMessageSender);
                        SenderTypeCache.Add(SenderKey, senderType);
                    }
                }
            }
            return Activator.CreateInstance(senderType) as IMailMessageSender;
        }

        public IMailMessageSender CreateMessageSender(MessageType MessageType, bool IsBulk)
        {
            Type senderType;
            string BulkFlag;

            if (IsBulk)
                BulkFlag = ".Bulk";
            else
                BulkFlag = ".Immediate";

            string SenderKey = MessageType + BulkFlag;

            lock (SenderTypeCache)
            {

                if (!SenderTypeCache.TryGetValue(SenderKey, out senderType))
                {
                    // Look for bulk or immediate instance configured for this message type
                    string ConfigSetting = "Messaging." + SenderKey;
                    string MessageSender = System.Configuration.ConfigurationManager.AppSettings[ConfigSetting];
                    if (MessageSender == null)
                    {
                        // Look for "generic" instance for this message type (not bulk or immediate)
                        ConfigSetting = "Messaging." + MessageType;
                        MessageSender = System.Configuration.ConfigurationManager.AppSettings[ConfigSetting];

                    }
                    if (MessageSender != null)
                    {
                        System.Reflection.Assembly.Load("Alerts.Notifications.Welcorp");
                        // Attempt to load type
                        senderType = Type.GetType(MessageSender);
                        if (senderType == null)
                            throw new InvalidOperationException("Unable to create message sender: " + MessageSender);
                        SenderTypeCache.Add(SenderKey, senderType);
                    }
                    else
                    {
                        // Use SMTP as the default type for email transmission
                        if (MessageType == MessageType.Email)
                        {
                            senderType = typeof(SMTPMailSender);
                            SenderTypeCache.Add(SenderKey, senderType);
                        }
                        else
                            throw new InvalidOperationException("Unable to create message sender. Missing configuration entry: " +
                                ConfigSetting);
                    }
                }
            }
            return Activator.CreateInstance(senderType) as IMailMessageSender;
        }

    }
#else
    public class DefaultMessageSenderFactory : IMailMessageSenderFactory
    {
        #region IMailMessageSenderFactory Members

        public IMailMessageSender CreateMessageSender(MessageType MessageType, bool IsBulk)
        {
            if (MessageType == MessageType.Email)
                return new SMTPMailSender();
            else
                throw new InvalidOperationException("Unsupported message type: " + MessageType.ToString());
        }

        public IMailMessageSender CreateMessageSender()
        {
            return new SmartMessageSender();
        }

        #endregion
    }
#endif
}
