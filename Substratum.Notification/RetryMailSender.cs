using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Substratum.Notification
{
    public abstract class RetryMailSender : IMailMessageSender, IRetryOnError
    {
        #region IMailMessageSender Members

        public MailMessageSenderResult Send(IMailMessage msg)
        {
            int ErrorCount=0;
            for (; ; )
            {
                try
                {
                    return Transmit(msg);
                }
                catch (Exception e)
                {
                    if (!EnableRetryOnError)
                        throw;

                    ErrorCount++;
                    if (ErrorCount < RetryOnErrorCount)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                        throw new InvalidOperationException("Send failed after " + RetryOnErrorCount + " retries. " + e.Message, e);
                }
            }
        }

        #endregion

        #region IRetryOnError Members

        bool _EnableRetryOnError = true;
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

        int _RetryOnErrorCount = 10;
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

        protected abstract MailMessageSenderResult Transmit(IMailMessage msg);

    }
}
