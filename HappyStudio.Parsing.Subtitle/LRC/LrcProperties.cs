using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.LRC
{
    public class LrcProperties : ISubtitleBlockProperties
    {
        private static readonly Regex PropertyRege = new Regex(@"\[(?<key>\D+)\:(?<value>.+)\]");
        private readonly Dictionary<string, string> _allProperties = new Dictionary<string, string>();

        public LrcProperties()
        {
        }
        public string Title
        {
            get => GetProperty("ti");
            set => SetProperty("ti", value);
        }

        public string Artist
        {
            get => GetProperty("ar");
            set => SetProperty("ar", value);
        }

        public string Album
        {
            get => GetProperty("al");
            set => SetProperty("al", value);
        }

        public string MadeBy
        {
            get => GetProperty("by");
            set => SetProperty("by", value);
        }

        public string EditorName
        {
            get => GetProperty("re");
            set => SetProperty("re", value);
        }

        public string EditorVersion
        {
            get => GetProperty("ve");
            set => SetProperty("ve", value);
        }

        public LrcProperties(string input)
        {
            var matches = PropertyRege.Matches(input);
            foreach (Match item in matches)
                _allProperties[item.Groups["key"].Value] = item.Groups["value"].Value;
        }

        public string GetProperty(string key)
        {
            if (_allProperties.ContainsKey(key))
                return _allProperties[key];

            return null;
        }

        public void SetProperty(string key, string value)
        {
            _allProperties[key.Trim()] = value.Trim();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in _allProperties)
                builder.AppendLine($"[{item.Key}:{item.Value}]");

            return builder.ToString();
        }
    }
}
