using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace OpenProject.Data.ValueConverters
{

  /// <summary>
  /// If the value is 0 or -1 returns collapsed
  /// </summary>
    [ValueConversion(typeof(String), typeof(Visibility))]
    public class IntVisibConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var c = (int)value;
            return (c == 0 || c == -1) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotImplementedException();
        }


    }
}
