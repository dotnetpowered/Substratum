using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Text
{ 
    public static class Parser
    {
        public static int ParseInt(string s)
        {
            if (s==null)
                return 0;
            switch (s.ToLower())
            {
                case "min": return int.MinValue;
                case "max": return int.MaxValue;
                default: return int.Parse(s, System.Globalization.NumberStyles.Any);
            }
        }

        public static decimal ParseDecimal(string s)
        {
            if (s == null)
                return 0;
            switch (s.ToLower())
            {
                case "min": return decimal.MinValue;
                case "max": return decimal.MaxValue;
                default: return decimal.Parse(s, System.Globalization.NumberStyles.Any);
            }
        }

        //TODO: Joe- parse float, parse int64, parse datetime
    }
}
