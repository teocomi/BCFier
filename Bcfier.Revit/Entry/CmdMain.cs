using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Reflection;

namespace Bcfier.Revit.Entry
{

  /// <summary>
  /// Obfuscation Ignore for External Interface
  /// </summary>
  [Obfuscation(Exclude = true, ApplyToMembers = false)]
  [Transaction(TransactionMode.Manual)]
  [Regeneration(RegenerationOption.Manual)]
  public class CmdMain : IExternalCommand
  {

#if Version2019

    public const string RevitVersion = "2019";

#elif Version2018

    public const string RevitVersion = "2018";

#elif Version2017

    public const string RevitVersion = "2017";

#elif Version2016

    public const string RevitVersion = "2016";

#elif Version2015

    public const string RevitVersion = "2015";


#endif

   

    internal static CmdMain ThisCmd = null;
    private static bool _isRunning;
    private static ExtAppBcfier _extAppBcfier;

    /// <summary>
    /// Main Command Entry Point
    /// </summary>
    /// <param name="commandData"></param>
    /// <param name="message"></param>
    /// <param name="elements"></param>
    /// <returns></returns>
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      try
      {

        //Version check
        
        if (!commandData.Application.Application.VersionName.Contains(RevitVersion))
        {
          using (var td = new TaskDialog("Untested version"))
          {
            td.TitleAutoPrefix = false;
            td.MainInstruction = "Untested Revit Version";
            td.MainContent = "This Add-In was built and tested only for Revit "+ RevitVersion + ", proceed at your own risk";
            td.Show();
          }
        }
        
        // Form Running?
        if (_isRunning && _extAppBcfier != null && _extAppBcfier.RvtWindow.IsLoaded)
        {
          _extAppBcfier.Focus();
          return Result.Succeeded;
        }

        _isRunning = true;

        ThisCmd = this;
        _extAppBcfier = new ExtAppBcfier();
        _extAppBcfier.ShowForm(commandData.Application);
        return Result.Succeeded;

      }
      catch (Exception e)
      {
        message = e.Message;
        return Result.Failed;
      }

    }

  }
}