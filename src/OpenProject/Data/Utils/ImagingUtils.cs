using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenProject.Data.Utils
{
  public static class ImagingUtils
  {
    /// <summary>
    /// Classes for working with images
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <returns></returns>
    public static ImageSource ImageSourceFromPath(string sourcePath)
    {

      try
      {
        //var image = BitmapFromPath(sourcePath);
        //return ConvertBitmapTo96Dpi(image);

        var image = BitmapFromPath(sourcePath);
        int width = image.PixelWidth;
        int height = image.PixelHeight;

        const int maxWidth = 1500;
        const int maxHeight = 1500;
        if (width > maxWidth || height > maxHeight)
        {

          double scale = (width > height) ? (double)width / (double)maxWidth : (double)height / (double)maxHeight;
          int newHeight = Convert.ToInt32(height / scale);
          int newWidth = Convert.ToInt32(width / scale);

          MessageBoxResult answer = MessageBox.Show(
            string.Format("Image size is {0}x{1}, "
              + "such a big image could increase A LOT the BCF file size. "
          + "Do you want me to resize it to {2}x{3} for you?", width, height, newWidth, newHeight), "Attention!",
              MessageBoxButton.YesNo, MessageBoxImage.Question);
          if (answer == MessageBoxResult.Yes)
          {
            width = newWidth;
          }

        }

        byte[] imageBytes = LoadImageData(sourcePath);
        return CreateImage(imageBytes, width, 0);

      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
      return null;
    }

    public static void SaveImageSource(ImageSource image, string destPath)
    {
      try
      {
        var imageBytes = GetEncodedImageData(image, ".jpg");
        SaveImageData(imageBytes, destPath);
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }

    public static BitmapImage BitmapFromPath(string path)
    {
      try
      {
        var image = new BitmapImage();
        image.BeginInit();
        image.UriSource = new Uri(path);
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
        image.EndInit();
        return image;
      }
      catch
      {
        return null;
      }

    }
    public static BitmapSource ConvertBitmapTo96Dpi(BitmapImage bitmapImage)
    {
      try
      {
        double dpi = 96;
        int width = bitmapImage.PixelWidth;
        int height = bitmapImage.PixelHeight;

        if (width > 1500 || height > 1500)
        {
          string size = width.ToString() + "x" + height.ToString();
          int newWidth = 1500;
          float scale = (float)newWidth / ((float)width / (float)height);
          int newHeight = Convert.ToInt32(scale);

          MessageBoxResult answer = MessageBox.Show("Image size is " + size + ", "
              + "such a big image could increase A LOT the BCF file size. "
          + "Do you want me to resize it to " + newWidth.ToString() + "x" + newHeight.ToString() + " for you?", "Attention!",
              MessageBoxButton.YesNo, MessageBoxImage.Question);
          if (answer == MessageBoxResult.Yes)
          {
            width = newWidth;
            height = newHeight;
          }

        }

        int stride = width * 4; // 4 bytes per pixel
        byte[] pixelData = new byte[stride * height];
        bitmapImage.CopyPixels(pixelData, stride, 0);

        return BitmapSource.Create(width, height, dpi, dpi, PixelFormats.Bgra32, null, pixelData, stride);
      }

      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
      return null;
    }

    private static byte[] LoadImageData(string filePath)
    {
      try
      {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        byte[] imageBytes = br.ReadBytes((int)fs.Length);
        br.Close();
        fs.Close();
        return imageBytes;
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
      return null;
    }

    private static ImageSource CreateImage(byte[] imageData, int decodePixelWidth, int decodePixelHeight)
    {
      try
      {
        if (imageData == null) return null;
        BitmapImage result = new BitmapImage();
        result.BeginInit();
        if (decodePixelWidth > 0)
        {
          result.DecodePixelWidth = decodePixelWidth;
        }
        if (decodePixelHeight > 0)
        {
          result.DecodePixelHeight = decodePixelHeight;
        }
        result.StreamSource = new MemoryStream(imageData);
        result.CreateOptions = BitmapCreateOptions.None;
        result.CacheOption = BitmapCacheOption.Default;
        result.EndInit();
        return result;
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
      return null;
    }

    private static void SaveImageData(byte[] imageData, string filePath)
    {
      try
      {
        FileStream fs = new FileStream(filePath, FileMode.Create,
        FileAccess.Write);
        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(imageData);
        bw.Close();
        fs.Close();
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }

    private static byte[] GetEncodedImageData(ImageSource image, string preferredFormat)
    {
      try
      {
        byte[] result = null;
        BitmapEncoder encoder = null;
        switch (preferredFormat.ToLower())
        {
          case ".jpg":
          case ".jpeg":
            encoder = new JpegBitmapEncoder();
            break;
          case ".bmp":
            encoder = new BmpBitmapEncoder();
            break;
          case ".png":
            encoder = new PngBitmapEncoder();
            break;
          case ".tif":
          case ".tiff":
            encoder = new TiffBitmapEncoder();
            break;
          case ".gif":
            encoder = new GifBitmapEncoder();
            break;
          case ".wmp":
            encoder = new WmpBitmapEncoder();
            break;
        }
        if (image is BitmapSource)
        {
          MemoryStream stream = new MemoryStream();
          encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
          encoder.Save(stream);
          stream.Seek(0, SeekOrigin.Begin);
          result = new byte[stream.Length];
          BinaryReader br = new BinaryReader(stream);
          br.Read(result, 0, (int)stream.Length);
          br.Close();
          stream.Close();
        }
        return result;
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
      return null;
    }
  }
}
