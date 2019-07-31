using System;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Subtitle.Control.Interface
{
    public interface ISubtitlePreviewControl
    {
        event EventHandler<ISubtitleLine> Refreshed;

        void SetSubtitle(ISubtitleBlock subtitle);
        void Refresh(TimeSpan time);
        void Reposition(TimeSpan time);
    }
}
