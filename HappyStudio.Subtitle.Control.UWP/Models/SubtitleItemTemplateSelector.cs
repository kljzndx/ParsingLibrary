using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HappyStudio.Subtitle.Control.UWP.Models
{
    public class SubtitleItemTemplateSelector : DataTemplateSelector
    {
        public SubtitleItemTemplateSelector(DataTemplate template)
        {
            Template = template;
        }

        public DataTemplate Template { get; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return Template;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}