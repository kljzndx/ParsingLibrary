using System;
using System.Collections.Generic;
using System.Text;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.LRC
{
    public class LrcLine : ISubtitleLine
    {
        public LrcLine(TimeSpan startTime)
        {
            StartTime = startTime;
        }

        public LrcLine(TimeSpan startTime, string content) : this(startTime)
        {
            Content = content;
        }

        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime => null;
        public string Content { get; set; }

        public override string ToString()
        {
            return ToString(2);
        }

        public string ToString(int msBitCount)
        {
            int count = msBitCount > 3 ? 3 : msBitCount;
            if (count < 1)
                count = 2;

            int min = StartTime.Minutes + (60 * StartTime.Hours);
            int ss = StartTime.Seconds;

            string msStr = StartTime.Milliseconds.ToString().Substring(0, count);

            return $"[{min:D2}:{ss:D2}.{msStr}] {Content}";
        }
    }
}
