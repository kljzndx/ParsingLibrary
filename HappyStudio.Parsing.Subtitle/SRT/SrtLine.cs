using System;
using System.Collections.Generic;
using System.Text;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.SRT
{
    public class SrtLine : ISubtitleLine
    {
        private TimeSpan _endTime;

        public SrtLine(TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            _endTime = endTime;
        }

        public SrtLine(TimeSpan startTime, TimeSpan endTime, string content) : this(startTime, endTime)
        {
            Content = content;
        }

        public TimeSpan StartTime { get; set; }

        public TimeSpan? EndTime
        {
            get => _endTime;
            set
            {
                if (value != null)
                    _endTime = value.Value;
            }
        }

        public string Content { get; set; }

        public override string ToString()
        {
            string st = StartTime.ToString(@"hh\:mm\:ss\.fff");
            string et = EndTime.Value.ToString(@"hh\:mm\:ss\.fff");

            return $"{st} --> {et}" + Environment.NewLine + Content;
        }
    }
}
