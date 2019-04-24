using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.LRC
{
    public class LrcProperties : ISubtitleBlockProperties
    {
        public const string TitleTag = "ti";
        public const string ArtistTag = "ar";
        public const string AlbumTag = "al";
        public const string MadeByTag = "by";
        public const string OffsetTag = "offset";
        public const string EditorNameTag = "re";
        public const string EditorVersionTag = "ve";

        private static readonly Regex PropertyRege = new Regex(@"\[(?<key>\D+?)\:(?<value>.+?)\]");
        private readonly Dictionary<string, string> _allProperties = new Dictionary<string, string>();

        public LrcProperties()
        {
        }

        public LrcProperties(string input)
        {
            var matches = PropertyRege.Matches(input);
            foreach (Match item in matches)
                _allProperties[item.Groups["key"].Value] = item.Groups["value"].Value;
        }

        public string Title
        {
            get => GetProperty(TitleTag);
            set => SetProperty(TitleTag, value);
        }

        public string Artist
        {
            get => GetProperty(ArtistTag);
            set => SetProperty(ArtistTag, value);
        }

        public string Album
        {
            get => GetProperty(AlbumTag);
            set => SetProperty(AlbumTag, value);
        }

        public string MadeBy
        {
            get => GetProperty(MadeByTag);
            set => SetProperty(MadeByTag, value);
        }

        public int Offset
        {
            get => Int32.Parse(GetProperty(OffsetTag).Replace("+", String.Empty));
            set
            {
                if (value >= 0)
                    SetProperty(OffsetTag, $"+{value}");
                else
                    SetProperty(OffsetTag, value.ToString());
            }
        }

        public string EditorName
        {
            get => GetProperty(EditorNameTag);
            set => SetProperty(EditorNameTag, value);
        }

        public string EditorVersion
        {
            get => GetProperty(EditorVersionTag);
            set => SetProperty(EditorVersionTag, value);
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
