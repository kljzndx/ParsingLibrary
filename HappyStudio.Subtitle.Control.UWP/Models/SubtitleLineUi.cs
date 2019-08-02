using System;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Subtitle.Control.UWP.Models
{
    public class SubtitleLineUi : ObservableObject, ISubtitleLine
    {
        private bool _isSelected;
        private TimeSpan _startTime;
        private TimeSpan? _endTime;
        private string _content;

        public SubtitleLineUi(ISubtitleLine line)
        {
            _startTime = line.StartTime;
            _endTime = line.EndTime;
            _content = line.Content;
        }

        public SubtitleLineUi(TimeSpan startTime)
        {
            _startTime = startTime;
        }

        public SubtitleLineUi(TimeSpan startTime, string content) : this(startTime)
        {
            _content = content;
        }

        public SubtitleLineUi(TimeSpan startTime, TimeSpan endTime) : this(startTime)
        {
            _endTime = endTime;
        }

        public SubtitleLineUi(TimeSpan startTime, TimeSpan endTime, string content) : this(startTime, endTime)
        {
            _content = content;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set => Set(ref _startTime, value);
        }

        public TimeSpan? EndTime
        {
            get => _endTime;
            set => Set(ref _endTime, value);
        }

        public string Content
        {
            get => _content;
            set => Set(ref _content, value);
        }
    }
}