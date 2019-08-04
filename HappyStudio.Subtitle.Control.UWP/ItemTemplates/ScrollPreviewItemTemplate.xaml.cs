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
using HappyStudio.Subtitle.Control.UWP.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace HappyStudio.Subtitle.Control.UWP.ItemTemplates
{
    public sealed partial class ScrollPreviewItemTemplate : UserControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(SubtitleLineUi), typeof(ScrollPreviewItemTemplate), new PropertyMetadata(null));

        private SubtitleLineUi _currentSource;

        public ScrollPreviewItemTemplate()
        {
            this.InitializeComponent();
        }

        public SubtitleLineUi Source
        {
            get => (SubtitleLineUi) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        private void ScrollPreviewItemTemplate_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContextChanged -= ScrollPreviewItemTemplate_DataContextChanged;
            this.DataContextChanged += ScrollPreviewItemTemplate_DataContextChanged;

            ScrollPreviewItemTemplate_DataContextChanged(null, null);
        }

        private void ScrollPreviewItemTemplate_OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_currentSource != null)
                _currentSource.PropertyChanged -= Source_PropertyChanged;

            this.DataContextChanged -= ScrollPreviewItemTemplate_DataContextChanged;
        }

        private void ScrollPreviewItemTemplate_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (_currentSource != null)
                _currentSource.PropertyChanged -= Source_PropertyChanged;

            _currentSource = Source;

            if (_currentSource != null)
                _currentSource.PropertyChanged += Source_PropertyChanged;
        }

        private void Source_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Source.IsSelected):
                    if (Source.IsSelected)
                    {
                        Main_TextBlock.FontWeight = FontWeights.Bold;
                        Main_TextBlock.FontSize = this.FontSize + 2;
                    }
                    else
                    {
                        Main_TextBlock.FontWeight = FontWeights.Normal;
                        Main_TextBlock.FontSize = this.FontSize;
                    }
                    break;
            }
        }
    }
}
