using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum
{
    public interface IRetryOnError
    {
        bool EnableRetryOnError { get; set; }
        int RetryOnErrorCount { get; set; }
    }
}
