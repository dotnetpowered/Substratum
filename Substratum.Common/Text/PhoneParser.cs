using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Text
{
    public class PhoneParser
    {
        public static string StripFormatting(string phoneNumber)
        {
            return phoneNumber.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ","");
        }
    }
}
