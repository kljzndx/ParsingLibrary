using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HappyStudio.Parsing.Subtitle;
using HappyStudio.Subtitle.Control.Interface.Models.Extensions;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace HappyStudio.Subtitle.Control.UWP.Demo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OpenLyricsFile_Button_OnClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".lrc");
            picker.FileTypeFilter.Add(".srt");
            var file = await picker.PickSingleFileAsync();
            if (file is null)
                return;

            string content = await FileIO.ReadTextAsync(file);
            var subtitleBlock = SubtitleParser.Parse(content);
            Main_ScrollSubtitlePreview.Source = subtitleBlock.Lines.ToLineUiList();
        }
    }
}
