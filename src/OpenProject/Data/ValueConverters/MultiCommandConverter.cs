using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenProject.Data.ValueConverters
{
  /// <summary>
  /// Needed to assign bultiple bindings to a command
  /// </summary>
  public class MultiCommandConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      return values.Clone();
    }

    public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}


