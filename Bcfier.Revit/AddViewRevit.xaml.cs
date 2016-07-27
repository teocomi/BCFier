using System.IO;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bcfier.Bcf.Bcf2;

namespace Bcfier.Revit
{
  /// <summary>
  /// Interaction logic for AddViewRevit.xaml
  /// </summary>
  public partial class AddViewRevit : Window
  {

    private Document doc = null;

    public AddViewRevit(Markup issue, string bcfTempFolder, Document _doc)
    {
      try
      {
        this.InitializeComponent();
        AddViewControl.Issue = issue;
        AddViewControl.TempFolder = bcfTempFolder;
        doc = _doc;

        AddViewControl.TextBlockInfo.Text = "3D/2D information of the current view will be included in the viewpoint";

        GetRevitSnapshot();
      }
      catch (System.Exception ex1)
      {
        TaskDialog.Show("Error!", "exception: " + ex1);
      }

    }

    private void GetRevitSnapshot()
    {
      try
      {
        string tempImg = Path.Combine(Path.GetTempPath(), "BCFier", Path.GetTempFileName() + ".png");
        var options = new ImageExportOptions
        {
          FilePath = tempImg,
          HLRandWFViewsFileType = ImageFileType.PNG,
          ShadowViewsFileType = ImageFileType.PNG,
          ExportRange = ExportRange.VisibleRegionOfCurrentView,
          ZoomType = ZoomFitType.FitToPage,
          ImageResolution = ImageResolution.DPI_72,
          PixelSize = 1000
        };
        doc.ExportImage(options);

        AddViewControl.AddViewpoint(tempImg);
        File.Delete(tempImg);
      }
      catch (System.Exception ex1)
      {
        TaskDialog.Show("Error!", "exception: " + ex1);
      }
    }
    
  }
}