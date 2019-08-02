using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HappyStudio.Parsing.Subtitle.Interfaces;
using HappyStudio.Subtitle.Control.Interface;
using HappyStudio.Subtitle.Control.UWP.Models;

namespace HappyStudio.Subtitle.Control.UWP
{
    public class SubtitlePreviewControlBase : UserControl, ISubtitlePreviewControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(List<SubtitleLineUi>), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null));

        private SubtitleLineUi _currentLine;

        protected SubtitleLineUi PreviousLine;
        protected int NextLineIndex;

        public event EventHandler<ISubtitleLine> Refreshed;

        protected bool CanPreview => IsEnabled && Visibility == Visibility.Visible && Source != null && Source.Any();

        public List<SubtitleLineUi> Source
        {
            get => (List<SubtitleLineUi>) GetValue(SourceProperty);
            protected set => SetValue(SourceProperty, value);
        }

        public SubtitleLineUi CurrentLine
        {
            get => _currentLine;
            protected set
            {
                PreviousLine = _currentLine;
                _currentLine = value;

                Refreshed?.Invoke(this, value);
            }
        }

        public virtual void SetSubtitle(List<ISubtitleLine> source)
        {
            if (source.All(s => s is SubtitleLineUi))
                Source = source.Cast<SubtitleLineUi>().ToList();
            else
                Source = source.Select(s => new SubtitleLineUi(s)).ToList();

            Reposition(TimeSpan.Zero);
        }

        public virtual void Refresh(TimeSpan time)
        {
            if (!CanPreview)
                return;

            if (NextLineIndex >= Source.Count)
                NextLineIndex = 0;

            long currentPositionTicks = time.Ticks;
            SubtitleLineUi nextLine = Source[NextLineIndex];
            long nextLyricTimeTicks = nextLine.StartTime.Ticks;
            long nextLyricEndTimeTicks = nextLyricTimeTicks + 10000000;

            if (currentPositionTicks >= nextLyricTimeTicks && currentPositionTicks < nextLyricEndTimeTicks)
            {
                NextLineIndex++;
                CurrentLine = nextLine;
            }
        }

        public virtual void Reposition(TimeSpan time)
        {
            if (!CanPreview)
                return;

            if (time.CompareTo(Source.First().StartTime) <= 0)
            {
                NextLineIndex = 0;
                CurrentLine = null;
                return;
            }

            if (time.CompareTo(Source.Last().StartTime) >= 0)
            {
                NextLineIndex = 0;
                CurrentLine = Source.Last();
                return;
            }

            for (var i = 0; i < Source.Count; i++)
            {
                if (time.CompareTo(Source[i].StartTime) < 0)
                {
                    NextLineIndex = i;
                    CurrentLine = Source[i - 1];
                    break;
                }
            }
        }
    }
}
