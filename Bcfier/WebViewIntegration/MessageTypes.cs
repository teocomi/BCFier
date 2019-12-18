using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bcfier.WebViewIntegration
{
  public static class MessageTypes
  {
    public const string REVIT_LOADED = "RevitLoaded";
    public const string VIEWPOINT_DATA = "ViewpointData";
    public const string VIEWPOINT_GENERATION_REQUESTED = "ViewpointGenerationRequested";
    public const string VIEWPOINT_GENERATED = "ViewpointGenerated";
  }
}
