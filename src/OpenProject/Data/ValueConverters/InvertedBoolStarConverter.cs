using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenProject.Data.ValueConverters
{
  /// <summary>
  /// If the value is true return a *. 
  /// Used to append * to unsaved file names.
  /// </summary>
  [ValueConversion(typeof(Boolean), typeof(string))]
  public class InvertedBoolStarConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return ((bool)value) ? "*" : "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

}
