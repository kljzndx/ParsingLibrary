using System;
using System.Collections.Generic;
using System.Text;

namespace HappyStudio.Parsing.Subtitle.Extensions
{
    public static class StringExtension
    {
        public static string[] ToLines(this string input)
        {
            bool hasNewLineSymbol = input.Contains("\n");
            string[] lines = input.Trim().Split(hasNewLineSymbol ? '\n' : '\r');
            return lines;
        }
    }
}
