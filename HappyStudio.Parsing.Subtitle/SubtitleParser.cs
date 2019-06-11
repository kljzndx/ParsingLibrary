using System;
using System.Reflection;
using HappyStudio.Parsing.Subtitle.Attributes;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle
{
    public class SubtitleParser
    {
        public static ISubtitleBlock Parse(string content)
        {
            return Parse(content, typeof(SubtitleParser).GetTypeInfo().Assembly);
        }

        public static ISubtitleBlock Parse(string content, Assembly assembly)
        {
            var types = assembly.DefinedTypes;
            foreach (var typeInfo in types)
            {
                var attribute = typeInfo.GetCustomAttribute<SubtitleFormatInfoAttribute>();
                if (attribute is null || !attribute.CheckFormat(content))
                    continue;

                var subtitle = Activator.CreateInstance(typeInfo.AsType(), content);
                return (ISubtitleBlock) subtitle;
            }

            throw new Exception("Cannot parse the subtitle content");
        }
    }
}