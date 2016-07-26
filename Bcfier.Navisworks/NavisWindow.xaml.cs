using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Bcfier.Bcf.Bcf2;

namespace Bcfier.Navisworks
{
  /// <summary>
  /// Interaction logic for NavisWindow.xaml
  /// </summary>
  public partial class NavisWindow : UserControl
  {
    public NavisWindow()
    {
      InitializeComponent();
    }
    #region commands
    /// <summary>
    /// Raised when opening a view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnOpenView(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        if (Bcfier.SelectedBcf() == null)
          return;
        var view = e.Parameter as ViewPoint;
        if (view == null)
          return;
       
        //todo

      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1, "Error opening a View!");
      }
    }
    /// <summary>
    /// Same as in the windows app, but here we generate a VisInfo that is attached to the view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnAddView(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {

        if (Bcfier.SelectedBcf() == null)
          return;
        var issue = e.Parameter as Markup;
        if (issue == null)
        {
          MessageBox.Show("No Issue selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }

        //todo

        //var dialog = new AddViewRevit(issue, Bcfier.SelectedBcf().TempPath, uiapp.ActiveUIDocument.Document);
        //dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //dialog.ShowDialog();
        //if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
        //{
        //  //generate and set set the VisInfo
        //  issue.Viewpoints.Last().VisInfo = GenerateViewpoint();

        //  //get filename
        //  UIDocument uidoc = uiapp.ActiveUIDocument;

        //  if (uidoc.Document.Title != null)
        //    issue.Header[0].Filename = uidoc.Document.Title;
        //  else
        //    issue.Header[0].Filename = "Unknown";

        //  Bcfier.SelectedBcf().HasBeenSaved = false;
        //}

      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1, "Error adding a View!");
      }
    }
    #endregion
    //stats
    private void NavisWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      Task.Run(() =>
      {
        StatHat.Post.EzCounter(@"hello@teocomi.com", "BCFierNavisStart", 1);
      });
    }
  }
}
