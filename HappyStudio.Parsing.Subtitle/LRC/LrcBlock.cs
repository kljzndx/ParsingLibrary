using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HappyStudio.Parsing.Subtitle.Attributes;
using HappyStudio.Parsing.Subtitle.Extensions;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.LRC
{
    [SubtitleFormatInfo(@"\[(?<min>\d{1,})\:(?<ss>\d{1,2})\.(?<ms>\d{1,3})\](?<content>.*)")]
    public class LrcBlock : ISubtitleBlock
    {
        private static readonly Regex LineRegex = new Regex(@"\[(?<min>\d{1,})\:(?<ss>\d{1,2})\.(?<ms>\d{1,3})\](?<content>.*)");

        public LrcBlock()
        {
            Properties = new LrcProperties();
            Lines = new ObservableCollection<ISubtitleLine>();
        }

        public LrcBlock(LrcProperties properties, IList<ISubtitleLine> lines)
        {
            Properties = properties;
            Lines = lines;
        }

        public LrcBlock(string lrcFileContent)
        {
            var lrcLines = new ObservableCollection<ISubtitleLine>();
            Properties = new LrcProperties(lrcFileContent);
            Lines = lrcLines;
            string[] strLines = lrcFileContent.ToLines();

            LrcLine lrcLine = null;
            StringBuilder contentBuilder = new StringBuilder();

            foreach (var item in strLines)
            {
                if (String.IsNullOrWhiteSpace(item))
                    continue;

                string line = item.Trim();
                Match match = LineRegex.Match(line);
                if (!match.Success && lrcLine is null)
                    continue;

                if (match.Success)
                {
                    if (lrcLine != null)
                    {
                        lrcLine.Content = contentBuilder.ToString().Trim();
                        lrcLines.Add(lrcLine);
                        contentBuilder.Clear();
                    }

                    int min = Int32.Parse(match.Groups["min"].Value);
                    int ss = Int32.Parse(match.Groups["ss"].Value);

                    string msStr = match.Groups["ms"].Value;
                    int ms = Int32.Parse(msStr);

                    for (int i = msStr.Length; i < 3; i++)
                        ms *= 10;


                    lrcLine = new LrcLine(new TimeSpan(0, 0, min, ss, ms));

                    contentBuilder.AppendLine(match.Groups["content"].Value);
                }
                else
                    contentBuilder.AppendLine(line);
            }

            if (lrcLine != null)
            {
                lrcLine.Content = contentBuilder.ToString().Trim();
                lrcLines.Add(lrcLine);
            }
        }

        public ISubtitleBlockProperties Properties { get; }
        public IList<ISubtitleLine> Lines { get; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(Properties.ToString());

            foreach (var item in Lines)
                builder.AppendLine(item.ToString());

            return builder.ToString().Trim();
        }
    }
}
