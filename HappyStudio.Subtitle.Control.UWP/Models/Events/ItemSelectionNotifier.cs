using System;
using HappyStudio.Parsing.Subtitle.Interfaces;

namespace HappyStudio.Subtitle.Control.UWP.Models.Events
{
    public static class ItemSelectionNotifier
    {
        public static event EventHandler<ISubtitleLine> SelectionChanged;

        public static void ChangeSelection(ISubtitleLine line)
        {
            SelectionChanged?.Invoke(null, line);
        }
    }
}