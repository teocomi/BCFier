using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenProject.WebViewIntegration
{
  /// <summary>
  /// This class is used to set the focus to the Revit desktop application,
  /// to ensure that after an action is called the Revit UI thread processes
  /// the new data.
  /// </summary>
  public static class RevitMainWindowHandler
  {
    public static void SetFocusToRevit()
    {
      var revitProcess = Process.GetProcesses()
        .Where(p => p.ProcessName == "Revit")
        .FirstOrDefault();
      if (revitProcess != null)
      {
        SetForegroundWindow(revitProcess.MainWindowHandle);
      }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);
  }
}
