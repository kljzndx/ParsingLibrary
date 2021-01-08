using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Subtitle.Control.Interface
{
    public interface ISubtitleLineUi : ISubtitleLine
    {
        bool IsSelected { get; set; }
    }
}
