using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HappyStudio.Parsing.Subtitle.Interfaces;
using HappyStudio.Subtitle.Control.Interface.Events;
using HappyStudio.Subtitle.Control.UWP.Models;
using HappyStudio.Subtitle.Control.UWP.Models.Events;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace HappyStudio.Subtitle.Control.UWP
{
    public sealed partial class ScrollSubtitlePreview : SubtitlePreviewControlBase
    {
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            nameof(ItemTemplate), typeof(DataTemplate), typeof(ScrollSubtitlePreview), new PropertyMetadata(null));

        public ScrollSubtitlePreview()
        {
            this.InitializeComponent();
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath(nameof(ItemTemplate));
            binding.Mode = BindingMode.OneWay;
            binding.TargetNullValue = Normal_DataTemplate;
            Main_ListView.SetBinding(ListView.ItemTemplateProperty, binding);

            base.Refreshed += ScrollLyricsPreview_Refreshed;
            this.LineHided += ScrollSubtitlePreview_LineHided;
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public event ItemClickEventHandler ItemClick;

        private double GetItemPosition(ISubtitleLine line)
        {
            ListViewItem container = Main_ListView.ContainerFromItem(line) as ListViewItem;
            if (container == null)
                return 0;

            var transform = container.TransformToVisual(Main_ListView);

            Point position = transform.TransformPoint(new Point());
            double result = (position.Y) + (container.ActualHeight / 2D) - (Root_ScrollViewer.ActualHeight / 2D);

            return result > 0 ? result : 0;
        }

        private async void ScrollLyricsPreview_Refreshed(object sender, SubtitlePreviewRefreshedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var theLine = args.NewLine;

                if (theLine == null)
                {
                    Main_ListView.SelectedIndex = -1;
                    ItemSelectionNotifier.ChangeSelection(null);
                    return;
                }

                Main_ListView.SelectedItem = theLine;
                ItemSelectionNotifier.ChangeSelection(theLine);
                Root_ScrollViewer.ChangeView(null, GetItemPosition(theLine), null);
            });
        }

        private void ScrollSubtitlePreview_LineHided(object sender, ISubtitleLine e)
        {
            ItemSelectionNotifier.ChangeSelection(null);
        }

        private void Main_ListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ItemClick?.Invoke(sender, e);
        }
    }
}
