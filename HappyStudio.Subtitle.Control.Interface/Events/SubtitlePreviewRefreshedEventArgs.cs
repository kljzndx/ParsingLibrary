using System;

namespace HappyStudio.Subtitle.Control.Interface.Events
{
    public class SubtitlePreviewRefreshedEventArgs : EventArgs
    {
        public SubtitlePreviewRefreshedEventArgs(ISubtitleLineUi oldLine, ISubtitleLineUi newLine)
        {
            OldLine = oldLine;
            NewLine = newLine;
        }

        public ISubtitleLineUi OldLine { get; }
        public ISubtitleLineUi NewLine { get; }
    }
}