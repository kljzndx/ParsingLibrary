using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.LRC
{
    public class LrcProperties : ISubtitleBlockProperties
    {
        public const string TitleTag = "ti";
        public const string ArtistTag = "ar";
        public const string AuthorTag = "au";
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

        public IEnumerable<KeyValuePair<string, string>> AllProperties => _allProperties;

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

        public string Author
        {
            get => GetProperty(AuthorTag);
            set => SetProperty(AuthorTag, value);
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
            get
            {
                try
                {
                    return Int32.Parse(GetProperty(OffsetTag).Replace("+", String.Empty));
                }
                catch (Exception)
                {
                    return 0;
                }
            }
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
            if (!String.IsNullOrWhiteSpace(value))
                _allProperties[key.Trim()] = value.Trim();
            else
                _allProperties.Remove(key);
        }

        public override string ToString()
        {
            return ToString(TitleTag, ArtistTag, AuthorTag, AlbumTag, MadeByTag, EditorNameTag, EditorVersionTag, OffsetTag);
        }

        /// <param name="firstOutputs">property keys that output first</param>
        public string ToString(params string[] firstOutputs)
        {
            StringBuilder builder = new StringBuilder();
            var dict = _allProperties.ToList();

            foreach (string key in firstOutputs.Where(k => dict.Any(p => p.Key == k)))
            {
                var pair = dict.First(p => p.Key == key);
                builder.AppendLine($"[{pair.Key}:{pair.Value}]");
                dict.Remove(pair);
            }

            foreach (var item in dict)
            {
                builder.AppendLine($"[{item.Key}:{item.Value}]");
            }

            return builder.ToString();
        }
    }
}
