using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HappyStudio.Parsing.Subtitle.Interfaces;
using HappyStudio.Subtitle.Control.Interface;
using HappyStudio.Subtitle.Control.Interface.Events;
using HappyStudio.Subtitle.Control.UWP.Models;

namespace HappyStudio.Subtitle.Control.UWP
{
    public class SubtitlePreviewControlBase : UserControl, ISubtitlePreviewControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(List<SubtitleLineUi>), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null));

        public static readonly DependencyProperty CurrentLineProperty = DependencyProperty.Register(
            nameof(CurrentLine), typeof(SubtitleLineUi), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null, CurrentLine_PropertyChangedCallback));

        protected int NextLineIndex;

        public event EventHandler<List<SubtitleLineUi>> SourceChanged;
        public event EventHandler<SubtitlePreviewRefreshedEventArgs> Refreshed;
        public event EventHandler<SubtitleLineUi> LineHided;

        protected bool CanPreview => IsEnabled && Visibility == Visibility.Visible && Source != null && Source.Any();

        public List<SubtitleLineUi> Source
        {
            get => (List<SubtitleLineUi>) GetValue(SourceProperty);
            protected set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty PreviousLineProperty = DependencyProperty.Register(
            nameof(PreviousLine), typeof(SubtitleLineUi), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null));

        public SubtitleLineUi PreviousLine
        {
            get => (SubtitleLineUi) GetValue(PreviousLineProperty);
            set => SetValue(PreviousLineProperty, value);
        }
        public SubtitleLineUi CurrentLine
        {
            get => (SubtitleLineUi) GetValue(CurrentLineProperty);
            set => SetValue(CurrentLineProperty, value);
        }

        public virtual void SetSubtitle(IEnumerable<ISubtitleLine> source)
        {
            var sl = source.ToList();

            if (sl.All(s => s is SubtitleLineUi))
                Source = sl.Cast<SubtitleLineUi>().ToList();
            else
                Source = sl.Select(s => new SubtitleLineUi(s)).ToList();

            SourceChanged?.Invoke(this, Source.ToList());
        }

        public virtual void Refresh(TimeSpan time)
        {
            if (!CanPreview)
                return;

            if (NextLineIndex >= Source.Count)
                NextLineIndex = 0;

            long currentPositionTicks = time.Ticks;
            SubtitleLineUi nextLine = Source[NextLineIndex];
            long nextLyricStartTimeTicks = nextLine.StartTime.Ticks;
            // 一个秒数插值
            long nextLyricStartAddedTimeTicks = nextLyricStartTimeTicks + TimeSpan.TicksPerSecond;


            long currentLyricEndTimeTicks = CurrentLine?.EndTime?.Ticks ?? 0;
            // 一个秒数插值
            long currentLyricEndAddedTimeTicks = currentLyricEndTimeTicks + TimeSpan.TicksPerSecond;

            if (currentPositionTicks >= nextLyricStartTimeTicks && currentPositionTicks < nextLyricStartAddedTimeTicks)
            {
                NextLineIndex++;
                PreviousLine = CurrentLine;
                CurrentLine = nextLine;
            }
            else if (currentLyricEndAddedTimeTicks > 0 &&
                     currentPositionTicks >= currentLyricEndTimeTicks && currentPositionTicks < currentLyricEndAddedTimeTicks)
            {
                LineHided?.Invoke(this, CurrentLine);
            }
        }

        public virtual void Reposition(TimeSpan time)
        {
            if (!CanPreview)
                return;

            PreviousLine = null;

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

        private static void CurrentLine_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var theObj = (SubtitlePreviewControlBase) d;
            theObj.Refreshed?.Invoke(theObj, new SubtitlePreviewRefreshedEventArgs(theObj.PreviousLine, theObj.CurrentLine));
        }
    }
}
