using System;
using System.Collections.Generic;
using System.Text;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.SRT
{
    public class SrtLine : ObservableObject, ISubtitleLine
    {
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private string _content;

        public SrtLine(TimeSpan startTime, TimeSpan endTime)
        {
            _startTime = startTime;
            _endTime = endTime;
        }

        public SrtLine(TimeSpan startTime, TimeSpan endTime, string content) : this(startTime, endTime)
        {
            _content = content;
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set => Set(ref _startTime, value);
        }

        public TimeSpan EndTime
        {
            get => _endTime;
            set => Set(ref _endTime, value);
        }

        public string Content
        {
            get => _content;
            set => Set(ref _content, value);
        }

        public override string ToString()
        {
            string st = StartTime.ToString(@"hh\:mm\:ss\,fff");
            string et = EndTime.ToString(@"hh\:mm\:ss\,fff");

            return $"{st} --> {et}" + Environment.NewLine + Content;
        }
    }
}
