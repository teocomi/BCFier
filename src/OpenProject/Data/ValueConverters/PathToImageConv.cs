using OpenProject.Data.Utils;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OpenProject.Data.ValueConverters
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

      if (value is byte[] byteArray)
      {
        return ImageFromBytes(byteArray);
      }

      if (!string.IsNullOrEmpty(value.ToString()))
      {
        return ImagingUtils.BitmapFromPath(value.ToString());
      }

      return null;
    }

    private BitmapImage ImageFromBytes(byte[] array)
    {
      // Taken from https://stackoverflow.com/questions/14337071/convert-array-of-bytes-to-bitmapimage
      using (var ms = new System.IO.MemoryStream(array))
      {
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad; // here
        image.StreamSource = ms;
        image.EndInit();
        return image;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
