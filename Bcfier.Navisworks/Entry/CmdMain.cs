using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;

namespace Bcfier.Navisworks.Entry
{

  [Plugin("Bcfier.CmdMain", "Bcfier", DisplayName = "BCFier")]
  [RibbonLayout("RibbonDefinition.xaml")]
  [RibbonTab("bcfierTab")]
  [Command("bcfierBtn", DisplayName = "BCFier", Icon = "BCFierIcon16x16.png", LargeIcon = "BCFierIcon32x32.png", ToolTip = "BCFier", ExtendedToolTip = "BCFier")]
  public class CmdMain : CommandHandlerPlugin
  {
#if Version2017

    public const string NavisVersion = "2017";

#elif Version2015

      public const string NavisVersion = "2015";

#elif Version2016

          public const string NavisVersion = "2016";

#endif
    /// <summary>
    /// Constructor, just initialises variables.
    /// </summary>
    public CmdMain()
    {

    }

    public override int ExecuteCommand(string commandId, params string[] parameters)
    {
      switch (commandId)
      {
        case "bcfierBtn":
          {

            LaunchPlugin();
            break;
          }

        default:
          {
            MessageBox.Show("You have clicked on the command with ID = '" + commandId + "'");
            break;
          }
      }

      return 0;
    }

    public override bool TryShowCommandHelp(String commandId)
    {
      MessageBox.Show("Showing Help for command with the Id " + commandId);
      return true;
    }

    /// <summary>
    /// Launch
    /// </summary>
    public void LaunchPlugin()
    {

      // Running Navis
      if (Autodesk.Navisworks.Api.Application.IsAutomated)
      {
        throw new InvalidOperationException("Invalid when running using Automation");
      }

      // Version
      if (!Autodesk.Navisworks.Api.Application.Version.RuntimeProductName.Contains(NavisVersion))
      {
        MessageBox.Show("This Add - In was built and tested only for Navisworks Manage  " + NavisVersion + ", proceed at your own risk",
                     "Untested Navisworks Version",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
      }

      //Find the plugin
      PluginRecord pr = Autodesk.Navisworks.Api.Application.Plugins.FindPlugin("Bcfier.AppMain.Bcfier");

      if (pr != null && pr is DockPanePluginRecord && pr.IsEnabled)
      {
        //string bcfier = "Bcfier.Navisworks.dll";

        //check if it needs loading
        if (pr.LoadedPlugin == null)
        {
          pr.LoadPlugin();
        }

        DockPanePlugin dpp = pr.LoadedPlugin as DockPanePlugin;
        if (dpp != null)
        {
          //switch the Visible flag
          dpp.Visible = !dpp.Visible;
        }
      }
    }
  }
}



// addin tab
//  [Plugin("Bcfier.CmdMain", "Bcfier", DisplayName = "BCFier")]
//  //[RibbonLayout("RibbonDefinition.xaml")]
// // [RibbonTab("bcfierTab")]
//  [Command("bcfierBtn", DisplayName = "BCFier", Icon = "BCFierIcon16x16.png", LargeIcon = "BCFierIcon32x32.png", ToolTip = "BCFier", ExtendedToolTip = "BCFier")]
//  public class BasicDockPaneAddin : AddInPlugin
//  {
//    public override int Execute(params string[] parameters)
//    {
//       // Find the plugin 
//       PluginRecord pr = Autodesk.Navisworks.Api.Application.Plugins.FindPlugin("Bcfier.AppMain.Bcfier");

//      if (pr != null && pr is DockPanePluginRecord && pr.IsEnabled)
//      { //check if it needs loading if (pr.LoadedPlugin == null) { pr.LoadPlugin(); }

//        DockPanePlugin dpp = pr.LoadedPlugin as DockPanePlugin; if (dpp != null)
//        { // Switch the Visible flag 
//          dpp.Visible = !dpp.Visible;
//        }
//      }
//      return 0;
//    }
//  }
//}