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
            nameof(Source), typeof(IEnumerable<ISubtitleLine>), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null, SourceProperty_ChangedCallback));

        public static readonly DependencyProperty CurrentLineProperty = DependencyProperty.Register(
            nameof(CurrentLine), typeof(ISubtitleLine), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null, CurrentLine_PropertyChangedCallback));

        public static readonly DependencyProperty PreviousLineProperty = DependencyProperty.Register(
            nameof(PreviousLine), typeof(ISubtitleLine), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null));

        protected int NextLineIndex;
        protected IList<ISubtitleLine> SourceList;

        public event EventHandler<IEnumerable<ISubtitleLine>> SourceChanged;
        public event EventHandler<SubtitlePreviewRefreshedEventArgs> Refreshed;
        public event EventHandler<ISubtitleLine> LineHided;

        protected bool CanPreview => IsEnabled && Visibility == Visibility.Visible && (Source?.Any() ?? false);

        public IEnumerable<ISubtitleLine> Source
        {
            get => (IEnumerable<ISubtitleLine>) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public ISubtitleLine PreviousLine
        {
            get => (ISubtitleLine) GetValue(PreviousLineProperty);
            set => SetValue(PreviousLineProperty, value);
        }

        public ISubtitleLine CurrentLine
        {
            get => (ISubtitleLine) GetValue(CurrentLineProperty);
            set => SetValue(CurrentLineProperty, value);
        }

        public virtual void Refresh(TimeSpan time)
        {
            if (!CanPreview)
                return;

            if (NextLineIndex >= SourceList.Count)
                NextLineIndex = 0;

            long currentPositionTicks = time.Ticks;
            ISubtitleLine nextLine = SourceList[NextLineIndex];
            long nextLyricStartTimeTicks = nextLine.StartTime.Ticks;
            // 一个秒数插值
            long nextLyricStartAddedTimeTicks = nextLyricStartTimeTicks + TimeSpan.TicksPerSecond;


            long currentLyricEndTimeTicks = 0;
            if (CurrentLine != null && CurrentLine.EndTime > CurrentLine.StartTime)
                currentLyricEndTimeTicks = CurrentLine.EndTime.Ticks;
            // 一个秒数插值
            long currentLyricEndAddedTimeTicks = currentLyricEndTimeTicks + TimeSpan.TicksPerSecond;

            if (currentPositionTicks >= nextLyricStartTimeTicks && currentPositionTicks < nextLyricStartAddedTimeTicks)
            {
                NextLineIndex++;
                PreviousLine = CurrentLine;
                CurrentLine = nextLine;
            }

            if (currentLyricEndTimeTicks > 0 &&
                     currentPositionTicks >= currentLyricEndTimeTicks && currentPositionTicks < currentLyricEndAddedTimeTicks)
            {
                LineHided?.Invoke(this, CurrentLine);

                PreviousLine = CurrentLine;
                CurrentLine = null;
            }
        }

        public virtual void Reposition(TimeSpan time)
        {
            if (!CanPreview)
                return;

            PreviousLine = null;

            if (time < SourceList.First().StartTime)
            {
                NextLineIndex = 0;
                CurrentLine = null;
                return;
            }

            var lastLine = SourceList.Last();
            if (lastLine.EndTime <= lastLine.StartTime || time >= lastLine.StartTime && time < lastLine.EndTime)
            {
                NextLineIndex = 0;
                CurrentLine = lastLine;
                return;
            }

            if (lastLine.EndTime > lastLine.StartTime && time >= lastLine.EndTime)
            {
                NextLineIndex = 0;
                CurrentLine = null;
                return;
            }

            for (var i = 0; i < SourceList.Count; i++)
            {
                if (SourceList[i].StartTime > time)
                {
                    NextLineIndex = i;

                    var currentLine = SourceList[i - 1];
                    if (currentLine.EndTime <= currentLine.StartTime || time >= currentLine.StartTime && time < currentLine.EndTime)
                        CurrentLine = currentLine;
                    else
                        CurrentLine = null;

                    break;
                }
            }
        }

        private static void SourceProperty_ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var theObj = (SubtitlePreviewControlBase) d;

            if (e.NewValue is IList<ISubtitleLine> list)
                theObj.SourceList = list;
            else
                theObj.SourceList = ((IEnumerable<ISubtitleLine>) e.NewValue).ToList();

            theObj.SourceChanged?.Invoke(theObj, (IEnumerable<ISubtitleLine>) e.NewValue);
        }

        private static void CurrentLine_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var theObj = (SubtitlePreviewControlBase) d;
            theObj.Refreshed?.Invoke(theObj, new SubtitlePreviewRefreshedEventArgs(theObj.PreviousLine, theObj.CurrentLine));
        }
    }
}
