using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Substratum.Notification
{
    public class QueuedMessageSender : SmartMessageSender
    {
        class MessageContainer
        {
            public IMailMessage msg;
            public int ErrorCount;
            public DateTime SendTime;
            public MessageType msgType;
        }

        static Queue<MessageContainer> Queue = new Queue<MessageContainer>();

        #region SmartMessageSender Override

        protected override MailMessageSenderResult Transmit(MessageType messageType, MailMessage FilteredMsg)
        {
            Enqueue(messageType, FilteredMsg);
            return new MailMessageSenderResult(MailMessageSenderStatus.Sent);
        }

        #endregion

        #region IRetryOnError Members

        bool _EnableRetryOnError;
        public bool EnableRetryOnError
        {
            get
            {
                return _EnableRetryOnError;
            }
            set
            {
                _EnableRetryOnError = value;
            }
        }

        int _RetryOnErrorCount;
        public int RetryOnErrorCount
        {
            get
            {
                return _RetryOnErrorCount;
            }
            set
            {
                _RetryOnErrorCount = value;
            }
        }

        #endregion

        static Thread SenderThread;
        static QueuedMessageSender()
        {
            SenderThread = new Thread(SenderThreadMain);
            SenderThread.Start();
        }

        private static void Enqueue(MessageType messageType, IMailMessage msg)
        {
            MessageContainer c = new MessageContainer();
            c.msg = msg;
            c.SendTime = DateTime.Now;
            c.msgType = messageType;
            Enqueue(c);
        }

        private static void Enqueue(MessageContainer c)
        {
            lock (Queue)
            {
                Queue.Enqueue(c);
            }
        }

        private static MessageContainer Dequeue()
        {
            lock (Queue)
            {
                if (Queue.Count > 0)
                    return Queue.Dequeue();
                else
                    return null;
            }
        }

        private static void SenderThreadMain()
        {
            for (; ; )
            {
                MessageContainer c = Dequeue();
                if (c != null)
                {
                    // Message is a retry the needs to be sent in the future
                    if (c.SendTime > DateTime.Now)
                    {
                        Enqueue(c);
                        Thread.Sleep(300);
                    }
                    else
                    {
                        try
                        {
                            // Create sender for this type of message
                            IMailMessageSender sender = MailMessageSenderFactory.CreateMessageSender(c.msgType, c.msg.IsBulk);

                            // If sender supports IRetryOnError, turn it off so we can retry it through the queue
                            IRetryOnError senderRetryOnError = sender as IRetryOnError;
                            bool senderRetryOnErrorEnabled = false;
                            if (senderRetryOnError != null)
                            {
                                senderRetryOnErrorEnabled = senderRetryOnError.EnableRetryOnError;
                                senderRetryOnError.EnableRetryOnError = false;
                            }

                            try
                            {
                                // Send message
                                MailMessageSenderResult senderResult = sender.Send(c.msg);

                                // TODO: add logging

                                //Logger.Info("Send " + c.msgType + " message (" + c.msg.Subject + ") to " + c.msg.Recipients.ToString()+" using "+
                                //    sender.GetType().FullName);
                            }
                            catch (Exception e)
                            {
                                // Catch errors and requeue the message to be resent later
                                if (senderRetryOnErrorEnabled)
                                {
                                    c.ErrorCount++;
                                    if (c.ErrorCount < senderRetryOnError.RetryOnErrorCount)
                                    {
                                        c.SendTime = DateTime.Now + new TimeSpan(0, 0, 10);
                                        Enqueue(c);
                                        //Logger.Error("Error sending message. Retry " + (c.ErrorCount - 1) + " of " +
                                        //    senderRetryOnError.RetryOnErrorCount + ".", e);
                                    }
                                    //else
                                    //    Logger.Error("Error sending message after " + senderRetryOnError.RetryOnErrorCount +
                                    //        " retries. Message has been discarded.", e);
                                }
                                //else
                                //    Logger.Error("Error sending message. Message has been discarded (Retry was not available).", e);
                            }
                        }
                        catch (Exception e)
                        {
                            //Logger.Error("Error creating message sender ("+c.msgType+". Message has been discarded.", e);
                        }
                    }
                }
                else
                    Thread.Sleep(500);                
            }
        }

    }
}
