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
using Windows.Foundation.Metadata;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace HappyStudio.Subtitle.Control.UWP.ItemTemplates
{
    public sealed partial class ScrollPreviewItemTemplate : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(ScrollPreviewItemTemplate), new PropertyMetadata(String.Empty));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(ScrollPreviewItemTemplate),
                new PropertyMetadata(false, IsSelected_PropertyChanged));

        public ScrollPreviewItemTemplate()
        {
            this.InitializeComponent();

            if (ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Controls.TextBlock", nameof(TextBlock.HorizontalTextAlignment)))
                Main_TextBlock.HorizontalTextAlignment = TextAlignment.Center;
            else
                Main_TextBlock.HorizontalAlignment = HorizontalAlignment.Center;
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private void ChangeStatus(bool isSelected)
        {
            Main_TextBlock.FontWeight = isSelected ? FontWeights.Bold : FontWeights.Normal;
            Main_TextBlock.FontSize = isSelected ? this.FontSize + 2 : this.FontSize;
        }

        private static void IsSelected_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var theObj = (ScrollPreviewItemTemplate) d;
            theObj.ChangeStatus((bool) e.NewValue);
        }
    }
}
