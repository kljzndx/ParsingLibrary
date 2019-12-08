using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.LRC
{
    public class LrcProperties : ObservableObject, ISubtitleBlockProperties
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

        public LrcProperties()
        {
        }

        public LrcProperties(string input)
        {
            var matches = PropertyRege.Matches(input);
            foreach (Match item in matches)
                AllProperties[item.Groups["key"].Value] = item.Groups["value"].Value;
        }

        public Dictionary<string, string> AllProperties { get; } = new Dictionary<string, string>();

        public string Title
        {
            get => GetProperty(TitleTag);
            set => Set(TitleTag, value);
        }

        public string Artist
        {
            get => GetProperty(ArtistTag);
            set => Set(ArtistTag, value);
        }

        public string Author
        {
            get => GetProperty(AuthorTag);
            set => Set(AuthorTag, value);
        }

        public string Album
        {
            get => GetProperty(AlbumTag);
            set => Set(AlbumTag, value);
        }

        public string MadeBy
        {
            get => GetProperty(MadeByTag);
            set => Set(MadeByTag, value);
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
                if (value > 0)
                    Set(OffsetTag, $"+{value}");
                else if (value == 0)
                    Set(OffsetTag, String.Empty);
                else
                    Set(OffsetTag, value.ToString());
            }
        }

        public string EditorName
        {
            get => GetProperty(EditorNameTag);
            set => Set(EditorNameTag, value);
        }

        public string EditorVersion
        {
            get => GetProperty(EditorVersionTag);
            set => Set(EditorVersionTag, value);
        }

        public string GetProperty(string key)
        {
            if (AllProperties.ContainsKey(key))
                return AllProperties[key];

            return String.Empty;
        }

        public void SetProperty(string key, string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
                AllProperties[key.Trim()] = value.Trim();
            else
                AllProperties.Remove(key);
        }

        public void Set(string key, string value, [CallerMemberName] string propertyName = null)
        {
            SetProperty(key, value);
            RaisePropertyChanged(propertyName);
        }

        public override string ToString()
        {
            return ToString(TitleTag, ArtistTag, AuthorTag, AlbumTag, MadeByTag, EditorNameTag, EditorVersionTag, OffsetTag);
        }

        /// <param name="firstOutputs">property keys that output first</param>
        public string ToString(params string[] firstOutputs)
        {
            StringBuilder builder = new StringBuilder();
            var dict = AllProperties.ToList();

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
