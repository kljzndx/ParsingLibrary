using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HappyStudio.Parsing.Subtitle.Extensions;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.SRT
{
    public class SrtBlock : ISubtitleBlockBasic
    {
        private List<SrtLine> _lines;

        public SrtBlock()
        {
            _lines = new List<SrtLine>();
        }

        public SrtBlock(string srtFileContent) : this()
        {
            string[] lines = srtFileContent.ToLines();
            StringBuilder builder = new StringBuilder();

            SrtLine srtLine = null;
            foreach (string item in lines)
            {
                if (String.IsNullOrWhiteSpace(item))
                    continue;

                string line = item.Trim();

                if (Int32.TryParse(line, out int id))
                {
                    if (srtLine != null)
                    {
                        srtLine.Content = builder.ToString().Trim();
                        _lines.Add(srtLine);
                    }

                    builder.Clear();
                    continue;
                }

                if (line.Contains("-->"))
                {
                    string[] times = line.Split(' ');
                    TimeSpan startTime = TimeSpan.Parse(times[0]);
                    TimeSpan endTime = TimeSpan.Parse(times[2]);

                    srtLine = new SrtLine(startTime, endTime);
                }
                else
                    builder.AppendLine(line);
            }


            if (srtLine != null)
            {
                srtLine.Content = builder.ToString().Trim();
                _lines.Add(srtLine);
            }
        }

        public IEnumerable<ISubtitleLine> Lines => _lines;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            var lastItem = Lines.LastOrDefault();

            foreach (var item in Lines)
            {
                i++;
                builder.AppendLine(i.ToString());
                builder.AppendLine(item.ToString());

                if (lastItem != item)
                    builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
