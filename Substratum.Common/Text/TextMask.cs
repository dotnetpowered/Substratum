using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Text
{
    public enum MaskDirection
    {
        FromLeft,
        FromRight
    }

    public class TextMask
    {
        public static string Mask(string s, char c, MaskDirection direction, int charsToLeave)
        {
            char[] charArray = new char[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                if (i < s.Length - charsToLeave && direction == MaskDirection.FromRight)
                    charArray[i] = c;
                else
                if (i >= charsToLeave && direction == MaskDirection.FromLeft)
                    charArray[i] = c;
                else
                    charArray[i] = s[i];
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(charArray);
            return builder.ToString();
        }
    }
}
