using System;
using System.IO;
using System.Windows;
using Bcfier.Bcf.Bcf2;
using Autodesk.Navisworks.Api;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using System.Drawing;
using System.Drawing.Imaging;

namespace Bcfier.Navisworks.Windows
{
  /// <summary>
  /// Interaction logic for AddViewNavis.xaml
  /// </summary>
  public partial class AddViewNavis : Window
  {
    public AddViewNavis(Markup issue, string bcfTempFolder)
    {
      try
      {
        InitializeComponent();
        AddViewControl.Issue = issue;
        AddViewControl.TempFolder = bcfTempFolder;

        AddViewControl.TextBlockInfo.Text = "3D information of the current view will be included in the viewpoint";

        GetNavisSnapshot();
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1, "Error!");
      }

    }

    public void GetNavisSnapshot()
    {
      try
      {
        string tempImg = Path.Combine(Path.GetTempPath(), "BCFier", Path.GetTempFileName() + ".png");
        // get the state of COM
        ComApi.InwOpState10 oState = ComBridge.State;
        // get the IO plugin for image
        ComApi.InwOaPropertyVec options = oState.GetIOPluginOptions("lcodpimage");
        //export the viewpoint to the image
        oState.DriveIOPlugin("lcodpimage", tempImg, options);
        Bitmap bitmap = new Bitmap(tempImg);
        var imageStream = new MemoryStream();
        bitmap.Save(imageStream, ImageFormat.Jpeg);
        bitmap.Dispose();

        AddViewControl.AddViewpoint(tempImg);
        File.Delete(tempImg);

      }
      catch (Exception ex1)
      {
        MessageBox.Show("exception: " + ex1, "Error!",MessageBoxButton.OK,MessageBoxImage.Error);
      }
    }
  }
}
