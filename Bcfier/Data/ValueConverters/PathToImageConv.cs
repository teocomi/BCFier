using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Globalization;
using Bcfier.Data.Utils;

namespace Bcfier.Data.ValueConverters
{
  /// <summary>
  /// This avoids issues when deleting an image that is loaded by the UI
  /// </summary>
    [ValueConversion(typeof(String), typeof(BitmapImage))]
    public class PathToImageConv : IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (!string.IsNullOrEmpty(value.ToString()))
            {
              return ImagingUtils.BitmapFromPath(value.ToString());
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotImplementedException();
        }



    }
}
