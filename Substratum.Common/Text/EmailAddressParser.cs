using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Text
{
    public class EmailAddressParser
    {
        public static void SplitAddress(string addr, out string displayName, out string emailAddress)
        {
            if (addr == null)
            {
                displayName = null;
                emailAddress = null;
            }
            else
            {
                string[] parts = addr.Split('[', ']');
                if (parts.Length == 1)
                {
                    emailAddress = parts[0].Trim();
                    displayName = string.Empty;
                }
                else
                {
                    displayName = parts[0].Trim();
                    emailAddress = parts[1].Trim();
                }
            }
        }
    }
}
