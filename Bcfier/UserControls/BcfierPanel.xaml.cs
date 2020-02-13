using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Bcfier.Api;
using Bcfier.Bcf;
using Bcfier.Data.Utils;
using Bcfier.Windows;
using Bcfier.Data;
using Version = System.Version;
using CefSharp;
using Bcfier.WebViewIntegration;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Bcfier.Shared.ViewModels.Bcf;
using Bcfier.ViewModels;
using Bcfier.Shared;

namespace Bcfier.UserControls
{
  /// <summary>
  /// Main panel UI and logic that need to be used by all modules
  /// </summary>
  public partial class BcfierPanel : UserControl
  {
    //my data context
    //private readonly BcfContainer _bcf = new BcfContainer();
    private readonly PanelViewModel _panelViewModel = new PanelViewModel();
    private readonly BrowserManager _browserManager;

    public BcfierPanel()
    {
      // This call sets up the global Chromium settings, e.g. the User Agent
      // CefBrowserInitializer.InitializeCefBrowser();

      InitializeComponent();
      DataContext = _panelViewModel;

      _browserManager = new BrowserManager(Browser);

      // TODO this is for quick testing to ensure a 'loaded' event is sent to OpenProject at the start
      var hasSentTestMessage = false;
      Browser.LoadingStateChanged += (s, e) =>
      {
        if (!e.IsLoading)
        {
          if (hasSentTestMessage)
          {
            return;
          }

          hasSentTestMessage = true;
          var myTimer = new System.Timers.Timer(1000);
          myTimer.Elapsed += (s2, e2) =>
          {
            JavaScriptBridge.Instance
            .SendMessageToOpenProject(MessageTypes.REVIT_LOADED, string.Empty, JsonConvert.SerializeObject(new
            {
              Title = "Demo.ifc"
            }));
            myTimer.Stop();
          };
          myTimer.Start();
        }
      };

      if (UserSettings.GetBool("checkupdates"))
        CheckUpdates();

    }

    private void PropagateAvailableStatiAndTypes()
    {
      var availableStati = UserSettings.Get("Stauses").Split(',');
      var availableTypes = UserSettings.Get("Types").Split(',');

      foreach (var bcfFile in _panelViewModel.BcfFiles)
      {
        foreach (var bcfIssue in bcfFile.BcfIssues)
        {
          bcfIssue.Markup.BcfTopic.AvailableStati.Clear();
          foreach (var availableStatus in availableStati.Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)))
          {
            bcfIssue.Markup.BcfTopic.AvailableStati.Add(availableStatus);
          }

          bcfIssue.Markup.BcfTopic.AvailableTypes.Clear();
          foreach (var availableType in availableTypes.Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)))
          {
            bcfIssue.Markup.BcfTopic.AvailableTypes.Add(availableType);
          }
        }
      }
    }

    #region commands
    private void OnDeleteIssues(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        if (SelectedBcf() == null)
          return;

        var selItems = e.Parameter as IList;
        var issues = selItems.Cast<BcfIssueViewModel>().ToList();
        if (!issues.Any())
        {
          MessageBox.Show("No Issue selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        MessageBoxResult answer = MessageBox.Show(
            String.Format("Are you sure you want to delete {0} Issue{1}?\n{2}", 
            issues.Count, 
            (issues.Count > 1) ? "s" : "",
            "\n - " + string.Join("\n - ", issues.Select(x => x.Markup.BcfTopic.Title))),
            String.Format("Delete Issue{0}?", (issues.Count > 1) ? "s" : ""), 
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer == MessageBoxResult.No)
          return;

        foreach (var issueToRemove in issues)
        {
          _panelViewModel.SelectedBcfFile
            .BcfIssues
            .Remove(issueToRemove);
        }
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }

    private void OnAddComment(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {

        if (SelectedBcf() == null)
          return;
        var values = (object[])e.Parameter;
        var view = values[0] as BcfViewpointViewModel;
        var issue = values[1] as BcfIssueViewModel;
        var content = values[2].ToString();
        //var status = (values[3] == null) ? "" : values[3].ToString();
        //var verbalStatus = values[4].ToString();
        if (issue == null)
        {
          MessageBox.Show("No Issue selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }


        var c = new BcfCommentviewModel();
        c.Text = content;
        //c.Topic = new CommentTopic();
        //c.Topic.Guid = issue.Topic.Guid;
        c.CreationDate = DateTime.UtcNow;
        //c.VerbalStatus = verbalStatus;
        //c.Status = status;
        c.Author = Utils.GetUsername();

        c.ViewpointId = view?.Id;

        issue.Markup.Comments.Add(c);

      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }
    private void OnDeleteComment(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        if (SelectedBcf() == null)
          return;
        var values = (object[])e.Parameter;
        var comment = values[0] as BcfCommentviewModel;
        //  var comments = selItems.Cast<Comment>().ToList();
        var issue = (BcfIssueViewModel)values[1];
        if (issue == null)
        {
          MessageBox.Show("No Issue selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        if (comment == null)
        {
          MessageBox.Show("No Comment selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        MessageBoxResult answer = MessageBox.Show(
          "Are you sure you want to\nDelete this comment?",
           "Delete Comment?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //MessageBoxResult answer = MessageBox.Show(
        //  String.Format("Are you sure you want to\nDelete {0} Comment{1}?", comments.Count, (comments.Count > 1) ? "s" : ""),
        //   "Delete Issue?", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (answer == MessageBoxResult.No)
          return;

        _panelViewModel.SelectedBcfFile.SelectedBcfIssue
          .Markup
          .Comments
          .Remove(comment);
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }

    private void OnDeleteView(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {

        if (SelectedBcf() == null)
          return;
        var values = (object[])e.Parameter;
        var view = values[0] as BcfViewpointViewModel;
        var issue = (BcfIssueViewModel)values[1];
        if (issue == null)
        {
          MessageBox.Show("No Issue selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        if (view == null)
        {
          MessageBox.Show("No View selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        var delComm = true;

        MessageBoxResult answer = MessageBox.Show("Do you also want to delete the comments linked to the selected viewpoint?",
           "Delete Viewpoint's Comments?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

        if (answer == MessageBoxResult.Cancel)
          return;
        if (answer == MessageBoxResult.No)
          delComm = false;

        _panelViewModel.SelectedBcfFile.SelectedBcfIssue.Viewpoints.Remove(view);
        if (delComm)
        {
          var commentsToRemove = _panelViewModel.SelectedBcfFile.SelectedBcfIssue
            .Markup
            .Comments
            .Where(c => c.ViewpointId == view.Id)
            .ToList();
          foreach (var commentToRemove in commentsToRemove)
          {
            _panelViewModel.SelectedBcfFile.SelectedBcfIssue
              .Markup
              .Comments
              .Remove(commentToRemove);
          }
        }
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }
    private void OnAddIssue(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {

        if (SelectedBcf() == null)
          return;
        var issue = new BcfIssueViewModel();
        issue.DisableListeningForChanges = true;
        issue.Markup = new BcfMarkupViewModel
        {
          BcfTopic = new BcfTopicViewModel()
        };
        issue.DisableListeningForChanges = false;

        _panelViewModel.SelectedBcfFile.BcfIssues.Add(issue);
        _panelViewModel.SelectedBcfFile.SelectedBcfIssue = issue;
        this.PropagateAvailableStatiAndTypes();

      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }
    private void HasIssueSelected(object sender, CanExecuteRoutedEventArgs e)
    {
      if (_panelViewModel.SelectedBcfFile != null)
        e.CanExecute = true;
      else
        e.CanExecute = false;

    }
    private void OnOpenSnapshot(object sender, ExecutedRoutedEventArgs e)
    {
      // TODO -> This is currently not supported
      throw new NotImplementedException();

      /*
      try
      {

        var view = e.Parameter as BcfViewpointViewModel;
        if (view == null || !File.Exists(view.SnapshotPath))
        {
          MessageBox.Show("The selected Snapshot does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        if (!UserSettings.GetBool("useDefPhoto", true))
        {
          var dialog = new SnapWin(view.SnapshotPath);
          dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
          dialog.Show();
        }
        else
          Process.Start(view.SnapshotPath);
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
      */
    }
    private void OnOpenComponents(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        var view = e.Parameter as BcfViewpointViewModel;
        if (view == null)
        {
          MessageBox.Show("The selected ViewPoint is null", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        var dialog = new ComponentsList(view);
        dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        dialog.Show();
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }

    private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      var target = e.Source as Control;

      if (target != null)
      {
        e.CanExecute = true;
      }
      else
      {
        e.CanExecute = false;
      }
    }
    #endregion
    #region events 

    private void OnOpenWebProject(object sender, RoutedEventArgs routedEventArgs)
    {

    }

    public void BcfFileClicked(string path)
    {
      // This is called when the program is started and a path to a BCF file is passed as an argument
      var bcfFile = BcfLocalFileLoader.OpenLocalBcfFile(path);
      _panelViewModel.BcfFiles.Add(bcfFile);
      _panelViewModel.SelectedBcfFile = bcfFile;
      PropagateAvailableStatiAndTypes();
    }

    private void HelpBtnOnClick(object sender, RoutedEventArgs routedEventArgs)
    {
      const string url = "http://bcfier.com/";
      try
      {
        Process.Start(url);
      }
      catch (Win32Exception)
      {
        Process.Start("IExplore.exe", url);
      }
    }

    #endregion

    #region web
    //check github API for new release
    private void CheckUpdates()
    {
      Task.Run(() =>
      {
        try
        {
          var release = GitHubRest.GetLatestRelease();
          if (release == null)
            return;

          string version = release.tag_name.Replace("v", "");
          var mine = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
          var online = Version.Parse(version);

          if (mine.CompareTo(online) < 0 && release.assets.Any())
          {
            Application.Current.Dispatcher.Invoke((Action)delegate {

              var dialog = new NewVersion();
              dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
              dialog.Description.Text = release.name + " has been released on " + release.published_at.ToLongDateString() + "\ndo you want to check it out now?";
              //dialog.NewFeatures.Text = document.Element("Bcfier").Element("Changelog").Element("NewFeatures").Value;
              //dialog.BugFixes.Text = document.Element("Bcfier").Element("Changelog").Element("BugFixes").Value;
              //dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
              dialog.ShowDialog();
              if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
                Process.Start(release.assets.First().browser_download_url);

            });
          
          }
        }
        catch (System.Exception ex1)
        {
          //warning suppressed
          Console.WriteLine("exception: " + ex1);
        }
      });
    }

    #endregion
    #region drag&drop
    private void Window_DragEnter(object sender, DragEventArgs e)
    {
      // whitespace.Visibility = Visibility.Visible;
    }
    private void Window_DragLeave(object sender, DragEventArgs e)
    {
      // whitespace.Visibility = Visibility.Hidden;
    }
    private void Window_Drop(object sender, DragEventArgs e)
    {
      try
      {
        // whitespace.Visibility = Visibility.Hidden;
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
          var files = (string[])e.Data.GetData(DataFormats.FileDrop);
          foreach (var f in files)
          {
            if (File.Exists(f))
            {
              var newBcfFile = BcfLocalFileLoader.OpenLocalBcfFile(f);
              _panelViewModel.BcfFiles.Add(newBcfFile);
              _panelViewModel.SelectedBcfFile = newBcfFile;
              PropagateAvailableStatiAndTypes();
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Exception: " + Environment.NewLine + ex);
      }
    }
    private void Window_DragOver(object sender, DragEventArgs e)
    {
      try
      {
        var dropEnabled = true;

        if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
        {
          var filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
          if (filenames.Any(x => Path.GetExtension(x).ToUpperInvariant() != ".BCFZIP"))
            dropEnabled = false;
        }
        else
          dropEnabled = false;

        if (!dropEnabled)
        {
          e.Effects = DragDropEffects.None;
          e.Handled = true;
        }
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }
    #endregion
    #region shortcuts
    public BcfFileViewModel SelectedBcf()
    {
      return _panelViewModel.SelectedBcfFile;
    }
    #endregion

  }
}
