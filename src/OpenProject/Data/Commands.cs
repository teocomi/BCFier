using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using OpenProject.UserControls;

namespace OpenProject.Data
{
  /// <summary>
  /// Commands used by the application
  /// </summary>
  public static class Commands
  {
    //issues
    public static readonly RoutedCommand AddIssue = new RoutedCommand("AddIssue", typeof(Button));
    public static readonly RoutedCommand AddIssues = new RoutedCommand("AddIssues", typeof(Button));
    public static readonly RoutedCommand DeleteIssues = new RoutedCommand("DeleteIssues", typeof(Button));
    //comments
    public static readonly RoutedCommand AddComment = new RoutedCommand("AddComment", typeof(Button));
    public static readonly RoutedCommand DeleteComments = new RoutedCommand("DeleteComments", typeof(Button));
    //viewpoints
    public static readonly RoutedCommand AddView = new RoutedCommand("AddView", typeof(Button));
    public static readonly RoutedCommand DeleteViews = new RoutedCommand("DeleteViews", typeof(Button));
    public static readonly RoutedCommand OpenView = new RoutedCommand("OpenView", typeof(Button));
    public static readonly RoutedCommand OpenSnapshot = new RoutedCommand("OpenSnapshot", typeof(Button));
    public static readonly RoutedCommand OpenComponents = new RoutedCommand("OpenComponents", typeof(Button));

    public static readonly RoutedCommand CloseBcf = new RoutedCommand("CloseBcf", typeof(Button));
  }
}
