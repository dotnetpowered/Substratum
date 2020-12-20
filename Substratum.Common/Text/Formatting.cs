using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Text
{
    public class Formatting
    {
        public static string GetFileSizeAsString(long size)
        {
            double s = size;
            string[] format = new string[] { "{0} bytes", "{0} kb", "{0} mb", "{0} gb", "{0} tb", "{0} pb", "{0} eb", "{0} zb", "{0} yb" };
            int i = 0;
            while (i < format.Length - 1 && s >= 1024)
            {
                s = (100 * s / 1024) / 100.0;
                i++;
            } return string.Format(format[i], s.ToString("###,###,##0.##"));
        }
    }
}
