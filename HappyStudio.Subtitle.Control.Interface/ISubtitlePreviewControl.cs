using System;
using System.Collections.Generic;
using HappyStudio.Parsing.Subtitle.Interfaces;
using HappyStudio.Subtitle.Control.Interface.Events;

namespace HappyStudio.Subtitle.Control.Interface
{
    public interface ISubtitlePreviewControl
    {
        IEnumerable<ISubtitleLine> Source { get; set; }

        event EventHandler<SubtitlePreviewRefreshedEventArgs> Refreshed;

        void Refresh(TimeSpan time);
        void Reposition(TimeSpan time);
    }
}
