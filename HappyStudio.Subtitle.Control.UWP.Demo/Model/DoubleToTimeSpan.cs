using System;
using Windows.UI.Xaml.Data;

namespace HappyStudio.Subtitle.Control.UWP.Demo.Model
{
    public class DoubleToTimeSpan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double v = (double) value;
            TimeSpan time = TimeSpan.FromSeconds(v);
            if (targetType == typeof(String))
                return time.ToString();
            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}