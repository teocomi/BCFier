namespace OpenProject.Shared
{
  public static class MessageTypes
  {
    // TODO UPDATE CONST NAMES TO MATCH THEIR DATA -> OTHERWISE CONFUSING
    public const string REVIT_LOADED = "RevitLoaded";

    // This indicates that the Revit desktop application should be focussed
    public const string FOCUS_REVIT_APPLICATION = "FocusRevitApplication";

    // This message may be sent by Revit to force the closing of the sesktop application
    public const string CLOSE_DESKTOP_APPLICATION = "CloseDesktopApplication";

    // This is used from the landing page when the user selects an OpenProject instance
    public const string INSTANCE_SELECTED = "InstanceSelected";

    // This is sent from the landing page to request the last used OpenProject instance
    public const string LAST_INSTANCE_REQUESTED = "LastInstanceRequested";

    // This is the response for the LAST_INSTANCE_REQUESTED message
    public const string LAST_INSTANCE = "LastInstance";

    public const string VIEWPOINT_DATA = "ShowViewpoint";
    public const string VIEWPOINT_GENERATION_REQUESTED = "ViewpointGenerationRequest";
    public const string VIEWPOINT_GENERATED = "ViewpointData";
  }
}
