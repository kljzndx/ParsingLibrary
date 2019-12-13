using System;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    public interface ISubtitleLine
    {
        TimeSpan StartTime { get; set; }
        TimeSpan EndTime { get; set; }
        string Content { get; set; }

        string ToString();
    }
}
