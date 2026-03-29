using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscogsCrawler.Helpers
{
    public class DiscogsCleaner
    {
        public static string CleanRawJson(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString)) return string.Empty;

            jsonString = Regex.Replace(jsonString, @"\[url=.*?\](.*?)\[/url\]", "$1", RegexOptions.IgnoreCase);

            jsonString = Regex.Replace(jsonString, @"\[[a-zA-Z]+=(.*?)\]", "$1", RegexOptions.IgnoreCase);

            jsonString = Regex.Replace(jsonString, @"\[/?(?:b|i|u|url)\]", "", RegexOptions.IgnoreCase);

            return jsonString;
        }
    }
}
