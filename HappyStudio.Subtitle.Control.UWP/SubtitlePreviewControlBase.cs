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
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            nameof(Position), typeof(TimeSpan), typeof(SubtitlePreviewControlBase), new PropertyMetadata(TimeSpan.Zero, PositionProperty_ChangedCallback));

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(IEnumerable<ISubtitleLine>), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null, SourceProperty_ChangedCallback));

        public static readonly DependencyProperty CurrentLineProperty = DependencyProperty.Register(
            nameof(CurrentLine), typeof(ISubtitleLine), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null, CurrentLine_PropertyChangedCallback));

        public static readonly DependencyProperty PreviousLineProperty = DependencyProperty.Register(
            nameof(PreviousLine), typeof(ISubtitleLine), typeof(SubtitlePreviewControlBase), new PropertyMetadata(null));

        private static readonly TimeSpan SecondTime = TimeSpan.FromSeconds(1);

        protected TimeSpan LastPosition;
        protected int NextLineIndex;
        protected IList<ISubtitleLine> SourceList;

        public event EventHandler<IEnumerable<ISubtitleLine>> SourceChanged;
        public event EventHandler<SubtitlePreviewRefreshedEventArgs> Refreshed;
        public event EventHandler<ISubtitleLine> LineHided;

        protected bool CanPreview => IsEnabled && Visibility == Visibility.Visible && (SourceList?.Any() ?? false);

        public TimeSpan Position
        {
            get => (TimeSpan) GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

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

            TimeSpan currentLineStartTime = TimeSpan.Zero;
            TimeSpan currentLineEndTime = TimeSpan.Zero;
            if (CurrentLine != null)
            {
                currentLineStartTime = CurrentLine.StartTime;
                currentLineEndTime = CurrentLine.EndTime;
            }

            ISubtitleLine nextLine = SourceList[NextLineIndex];
            var nextLyricStartTime = nextLine.StartTime;

            if (time >= nextLyricStartTime && time < nextLyricStartTime + SecondTime)
            {
                NextLineIndex++;
                CurrentLine = nextLine;
            }

            if (currentLineEndTime > currentLineStartTime &&
                     time >= currentLineEndTime && time < currentLineEndTime + SecondTime)
            {
                LineHided?.Invoke(this, CurrentLine);

                CurrentLine = null;
            }
        }

        public virtual void Reposition(TimeSpan time)
        {
            if (!CanPreview)
                return;

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

        protected virtual void IntelligentRefresh(TimeSpan time)
        {
            var minPosition = LastPosition - SecondTime;
            var maxPosition = LastPosition + SecondTime;

            if (time > LastPosition)
                LastPosition = time;

            if (time < minPosition || time > maxPosition)
            {
                Reposition(time);
                LastPosition = time;
            }
            else
                Refresh(time);
        }

        private static void PositionProperty_ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var theObj = (SubtitlePreviewControlBase) d;
            theObj.IntelligentRefresh((TimeSpan) e.NewValue);
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
            var theLine = (ISubtitleLine) e.NewValue;
            if (theLine != null)
            {
                int lineId = theObj.SourceList.IndexOf(theLine);
                theObj.PreviousLine = lineId > 0 ? theObj.SourceList[lineId - 1] : theObj.SourceList.Last();
            }
            else
                theObj.PreviousLine = (ISubtitleLine) e.OldValue;

            theObj.Refreshed?.Invoke(theObj, new SubtitlePreviewRefreshedEventArgs(theObj.PreviousLine, theLine));
        }
    }
}
