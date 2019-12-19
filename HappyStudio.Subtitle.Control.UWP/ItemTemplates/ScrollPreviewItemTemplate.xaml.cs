using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HappyStudio.Parsing.Subtitle.Interfaces;
using HappyStudio.Subtitle.Control.UWP.Models;
using HappyStudio.Subtitle.Control.UWP.Models.Events;
using Windows.Foundation.Metadata;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace HappyStudio.Subtitle.Control.UWP.ItemTemplates
{
    public sealed partial class ScrollPreviewItemTemplate : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(ScrollPreviewItemTemplate), new PropertyMetadata(String.Empty));

        public ScrollPreviewItemTemplate()
        {
            this.InitializeComponent();

            if (ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Controls.TextBlock", nameof(TextBlock.HorizontalTextAlignment)))
                Main_TextBlock.HorizontalTextAlignment = TextAlignment.Center;
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private void ScrollPreviewItemTemplate_OnLoaded(object sender, RoutedEventArgs e)
        {
            ItemSelectionNotifier.SelectionChanged -= ItemSelectionNotifier_SelectionChanged;
            ItemSelectionNotifier.SelectionChanged += ItemSelectionNotifier_SelectionChanged;
        }

        private void ScrollPreviewItemTemplate_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ItemSelectionNotifier.SelectionChanged -= ItemSelectionNotifier_SelectionChanged;
        }

        private void ItemSelectionNotifier_SelectionChanged(object sender, ISubtitleLine e)
        {
            if (this.DataContext != null && this.DataContext == e)
            {
                Main_TextBlock.FontWeight = FontWeights.Bold;
                Main_TextBlock.FontSize = this.FontSize + 2;
            }
            else
            {
                Main_TextBlock.FontWeight = FontWeights.Normal;
                Main_TextBlock.FontSize = this.FontSize;
            }
        }
    }
}
