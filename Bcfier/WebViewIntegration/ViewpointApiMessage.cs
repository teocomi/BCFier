using Bcfier.ViewModels.Bcf;
using iabi.BCF.APIObjects.V21;

namespace Bcfier.WebViewIntegration
{
  public class ViewpointApiMessage
  {
    public Viewpoint_GET Viewpoint { get; set; }
    public Components Components { get; set; }
  }
}
