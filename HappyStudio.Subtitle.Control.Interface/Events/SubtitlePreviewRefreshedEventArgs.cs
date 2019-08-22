using System;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Subtitle.Control.Interface.Events
{
    public class SubtitlePreviewRefreshedEventArgs : EventArgs
    {
        public SubtitlePreviewRefreshedEventArgs(ISubtitleLine oldLine, ISubtitleLine newLine)
        {
            OldLine = oldLine;
            NewLine = newLine;
        }

        public ISubtitleLine OldLine { get; }
        public ISubtitleLine NewLine { get; }
    }
}