using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bcfier.WebViewIntegration
{
  public static class MessageTypes
  {
    // TODO UPDATE CONST NAMES TO MATCH THEIR DATA -> OTHERWISE CONFUSING
    public const string REVIT_LOADED = "RevitLoaded";
    public const string VIEWPOINT_DATA = "ShowViewpoint";
    public const string VIEWPOINT_GENERATION_REQUESTED = "ViewpointGenerationRequest";
    public const string VIEWPOINT_GENERATED = "ViewpointData";
  }
}
