using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Substratum.Text
{
    public class ReplacementDictionary : Dictionary<string, string>
    {
        public ReplacementDictionary Append(string k, string v)
        {
            this.Add(k,v);
            return this;
        }
    }

    public static class StringExtension
    {
        public static string MultiReplace(this string target, ReplacementDictionary replacementDictionary) 
        { 
            return Regex.Replace(target, "(" + String.Join("|", replacementDictionary.Keys.ToArray()) + ")", 
                delegate(Match m) { return replacementDictionary[m.Value]; }); 
        }

    }
}


