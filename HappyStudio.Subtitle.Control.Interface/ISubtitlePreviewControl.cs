using System;
using System.Collections.Generic;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Subtitle.Control.Interface
{
    public interface ISubtitlePreviewControl
    {
        event EventHandler<ISubtitleLine> Refreshed;

        void SetSubtitle(List<ISubtitleLine> lines);
        void Refresh(TimeSpan time);
        void Reposition(TimeSpan time);
    }
}
