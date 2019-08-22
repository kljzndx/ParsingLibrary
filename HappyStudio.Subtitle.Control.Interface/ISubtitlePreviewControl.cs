using System;
using System.Collections.Generic;
using HappyStudio.Parsing.Subtitle.Interfaces;
using HappyStudio.Subtitle.Control.Interface.Events;

namespace HappyStudio.Subtitle.Control.Interface
{
    public interface ISubtitlePreviewControl
    {
        event EventHandler<SubtitlePreviewRefreshedEventArgs> Refreshed;

        void SetSubtitle(IEnumerable<ISubtitleLine> lines);
        void Refresh(TimeSpan time);
        void Reposition(TimeSpan time);
    }
}
