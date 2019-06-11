using System;

namespace HappyStudio.Parsing.Subtitle.Interfaces
{
    public interface ISubtitleLine
    {
        TimeSpan StartTime { get; }
        TimeSpan? EndTime { get; }
        string Content { get; }

        string ToString();
    }
}
