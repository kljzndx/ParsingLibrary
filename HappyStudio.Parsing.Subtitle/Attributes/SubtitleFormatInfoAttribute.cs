using System;
using System.Text.RegularExpressions;

namespace HappyStudio.Parsing.Subtitle.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SubtitleFormatInfoAttribute : Attribute
    {
        public SubtitleFormatInfoAttribute(string formatRule)
        {
            Rule = new Regex(formatRule);
        }

        public Regex Rule { get; set; }

        public bool CheckFormat(string content)
        {
            return Rule.IsMatch(content);
        }
    }
}