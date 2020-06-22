using System;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace OpenProject.Data.ValueConverters
{
  /// <summary>
  /// Returna  string formatted to have a number and a plural
  /// using %0% and %s% as wildcards
  /// </summary>
  [ValueConversion(typeof(Int16), typeof(String))]
  public class IntPluralConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var count = (int)value;
      var plural = (count == 1) ? "" : "s";

      var text = "";
      if (parameter != null)
        text = parameter.ToString();
      text = text.Replace("%0%", count.ToString());
      text = text.Replace("%s%", plural);


      return text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {

      throw new NotImplementedException();
    }


  }
}
