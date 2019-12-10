using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using HappyStudio.Parsing.Subtitle.Attributes;
using HappyStudio.Parsing.Subtitle.Extensions;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.SRT
{
    [SubtitleFormatInfo(@"\d{2,3}\:\d{2}\:\d{2}\,\d{3} --> \d{2,3}\:\d{2}\:\d{2}\,\d{3}")]
    public class SrtBlock : ISubtitleBlock
    {
        private IList<SrtLine> _lines;

        public SrtBlock()
        {
            _lines = new ObservableCollection<SrtLine>();
        }

        public SrtBlock(IList<SrtLine> lines)
        {
            _lines = lines;
        }

        public SrtBlock(string srtFileContent)
        {
            ObservableCollection<SrtLine> srtLines = new ObservableCollection<SrtLine>();
            _lines = srtLines;
            string[] fileLines = srtFileContent.ToLines();
            StringBuilder builder = new StringBuilder();

            SrtLine srtLine = null;
            foreach (string item in fileLines)
            {
                if (String.IsNullOrWhiteSpace(item))
                    continue;

                string line = item.Trim();

                if (Int32.TryParse(line, out int id))
                {
                    if (srtLine != null)
                    {
                        srtLine.Content = builder.ToString().Trim();
                        srtLines.Add(srtLine);
                    }

                    builder.Clear();
                    continue;
                }

                if (line.Contains("-->"))
                {
                    string[] times = line.Split(' ');
                    TimeSpan startTime = TimeSpan.Parse(times[0].Replace(',', '.'));
                    TimeSpan endTime = TimeSpan.Parse(times[2].Replace(',', '.'));

                    srtLine = new SrtLine(startTime, endTime);
                }
                else
                    builder.AppendLine(line);
            }


            if (srtLine != null)
            {
                srtLine.Content = builder.ToString().Trim();
                srtLines.Add(srtLine);
            }
        }

        public IList<ISubtitleLine> Lines => (IList<ISubtitleLine>) _lines;
        public ISubtitleBlockProperties Properties => null;

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
