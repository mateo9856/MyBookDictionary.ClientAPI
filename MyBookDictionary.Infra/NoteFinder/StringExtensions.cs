using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.NoteFinder
{
    public static class StringExtensions
    {
        public static string FindHtmlTag(this string value)
        {
            var FindString = Regex.Match(value, @"<[a-z]{1,10}");
            var result = FindString.Value;
            return result.Substring(1);
        }
    }
}
