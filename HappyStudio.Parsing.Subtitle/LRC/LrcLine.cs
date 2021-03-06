﻿using System;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Parsing.Subtitle.LRC
{
    public class LrcLine : ObservableObject, ISubtitleLine
    {
        protected bool SyncEndTime = true;
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private string _content;
        
        public LrcLine(TimeSpan startTime)
        {
            _startTime = startTime;
            _endTime = startTime;
        }

        public LrcLine(TimeSpan startTime, string content) : this(startTime)
        {
            _content = content;
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                Set(ref _startTime, value);

                if (SyncEndTime)
                    Set(ref _endTime, value, nameof(EndTime));
            }
        }

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                Set(ref _endTime, value);
                SyncEndTime = false;
            }
        }

        public string Content
        {
            get => _content;
            set => Set(ref _content, value);
        }

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

            string msStr = StartTime.Milliseconds.ToString("D3").Substring(0, count);

            return $"[{min:D2}:{ss:D2}.{msStr}] {Content}";
        }
    }
}
