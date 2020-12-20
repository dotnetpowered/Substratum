using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.Text
{
    public class NameParser
    {
        string _FullName;
        public NameParser(string FullName)
        {
            _FullName = FullName;
        }

        public string FullName
        {
            get
            {
                return _FullName;
            }
        }

        public string FirstName
        {
            get
            {
                if (_FullName == null)
                    return string.Empty;
                else
                {
                    string[] s = _FullName.ToString().Split(' ');
                    return s[0];
                }
            }
        }

        public string MiddleInitial
        {
            get
            {
                if (_FullName == null)
                    return string.Empty;
                else
                {
                    string[] s = _FullName.ToString().Split(' ');
                    if (s.Length > 2)
                        return s[0];
                    else
                        return string.Empty;
                }
            }
        }

        public string LastName
        {
            get
            {
                if (_FullName == null)
                    return string.Empty;
                else
                {
                    string[] s = _FullName.ToString().Split(' ');
                    int StartAt = 2;
                    if (s.Length == 2)
                        StartAt = 1;
                    StringBuilder sb = new StringBuilder();
                    for (int i = StartAt; i < s.Length; i++)
                        sb.Append(s[i]).Append(" ");
                    return sb.ToString().Trim();
                }
            }
        }
    }
}
