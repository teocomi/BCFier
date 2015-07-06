using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Windows;

namespace Bcfier.Revit.Entry
{

  /// <summary>
  /// Obfuscation Ignore for External Interface
  /// </summary>
  [Obfuscation(Exclude = true, ApplyToMembers = false)]
  [Transaction(TransactionMode.Manual)]
  public class ExtAppBcfier : IExternalApplication
  {

    // class instance  
    public static ExtAppBcfier This = null;
    // ModelessForm instance  
    public RevitWindow RvtWindow;

    #region Revit IExternalApplication Implementation

    /// <summary>
    /// Startup
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    public Result OnStartup(UIControlledApplication application)
    {
      RvtWindow = null;   // no dialog needed yet; the command will bring it  
      This = this;  // static access to this application instance  

      return Result.Succeeded;
    }

    /// <summary>
    /// Shut Down
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    public Result OnShutdown(UIControlledApplication application)
    {
      if (RvtWindow != null && RvtWindow.IsVisible)
      {
        RvtWindow.Close();
      }

      return Result.Succeeded;
    }

    #endregion

    #region public methods
    /// <summary>
    /// The external command invokes this on the end-user's request 
    /// </summary>
    /// <param name="uiapp"></param>
    public void ShowForm(UIApplication uiapp)
    {
      try
      {
        // If we do not have a dialog yet, create and show it  
        if (RvtWindow != null) return;

        // A new handler to handle request posting by the dialog  
        var handler = new ExtEvntOpenView();

        // External Event for the dialog to use (to post requests)  
        var extEvent = ExternalEvent.Create(handler);

        // We give the objects to the new dialog;  
        // The dialog becomes the owner responsible for disposing them, eventually.
        RvtWindow = new RevitWindow(uiapp, extEvent, handler);
        RvtWindow.Show();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    /// <summary>
    /// Set Focus
    /// </summary>
    public void Focus()
    {
      try
      {
        if (RvtWindow == null) return;
        RvtWindow.Activate();
        RvtWindow.WindowState = WindowState.Normal;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }

    }
    #endregion
  }

}