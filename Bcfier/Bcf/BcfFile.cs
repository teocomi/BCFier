using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Bcfier.Bcf.Bcf2;
using Bcfier.Data.Utils;

namespace Bcfier.Bcf
{
  /// <summary>
  /// View Model of a deserialized BCF
  /// </summary>
  public class BcfFile : INotifyPropertyChanged
  {
    private Guid id;
    public string TempPath { get; set; }
    public string Fullname { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; }
    private string _filename;
    private bool _hasBeenSaved;
    private ObservableCollection<Markup> _issues;
    private Markup _selectedIssue;
    private string _textSearch;
    private ListCollectionView _view;

    public BcfFile()
    {
      _hasBeenSaved = true;
      Filename = "New BCF Report";
      Id = Guid.NewGuid();
      TempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "BCFier", Id.ToString());
      Issues = new ObservableCollection<Markup>();
    }
    public bool HasBeenSaved
    {
      get
      {
        return _hasBeenSaved;
      }

      set
      {
        _hasBeenSaved = value;
        NotifyPropertyChanged("HasBeenSaved");
      }
    }
    public Guid Id
    {
      get
      {
        return id;
      }

      set
      {
        id = value;
        NotifyPropertyChanged("Id");
      }
    }

    public string Filename
    {
      get
      {
        return _filename;
      }

      set
      {
        _filename = value;
        NotifyPropertyChanged("Filename");
      }
    }

    public ObservableCollection<Markup> Issues
    {
      get { return _issues; }

      set
      {
        _issues = value;
        this._view = new ListCollectionView(this.Issues);
        NotifyPropertyChanged("Issues");
      }
    }

    public Markup SelectedIssue
    {
      get
      {
        return _selectedIssue;
      }

      set
      {
        _selectedIssue = value;
        NotifyPropertyChanged("SelectedIssue");
      }
    }


    public ICollectionView View
    {
      get
      {
        return this._view;
      }
    }

    public string TextSearch
    {
      get { return _textSearch; }
      set
      {
        _textSearch = value;
        NotifyPropertyChanged("TextSearch");

        if (String.IsNullOrEmpty(value))
          View.Filter = null;
        else
          View.Filter = Filter;
      }
    }

    private bool Filter(object o)
    {
      var issue = (Markup)o;
      if (issue == null)
        return false;
      if (issue.Topic != null && ((issue.Topic.Title != null && issue.Topic.Title.ToLowerInvariant().Contains(TextSearch.ToLowerInvariant())) ||
          (issue.Topic.Description != null && issue.Topic.Description.ToLowerInvariant().Contains(TextSearch.ToLowerInvariant()))) ||
         issue.Comment != null && issue.Comment.Any(x => x.Comment1.ToLowerInvariant().Contains(TextSearch.ToLowerInvariant()))

          )
        return true;
      return false;
    }

    public void RemoveIssues(IEnumerable<Markup> selectetitems)
    {
      foreach (var item in selectetitems)
      {
        Utils.DeleteDirectory(Path.Combine(TempPath, item.Topic.Guid));
        Issues.Remove(item);
      }
      HasBeenSaved = false;
    }

    public void RemoveComment(IEnumerable<Comment> selectetitems, Markup issue)
    {
      foreach (var item in selectetitems)
      {
        issue.Comment.Remove(item);
      }
      HasBeenSaved = false;
    }
    public void RemoveComment(Comment comment, Markup issue)
    {
      issue.Comment.Remove(comment);
      HasBeenSaved = false;
    }
    public void RemoveView(ViewPoint view, Markup issue, bool delComm)
    {

      if (File.Exists(Path.Combine(TempPath, issue.Topic.Guid, view.Viewpoint)))
        File.Delete(Path.Combine(TempPath, issue.Topic.Guid, view.Viewpoint));
      if (File.Exists(view.SnapshotPath))
        File.Delete(view.SnapshotPath);

      var guid = view.Guid;
      issue.Viewpoints.Remove(view);
      //remove comments associated with that view
      var viewcomments = issue.Comment.Where(x => x.Viewpoint != null && x.Viewpoint.Guid == guid).ToList();

      if (!viewcomments.Any())
        return;

      foreach (var viewcomm in viewcomments)
      {
        if (delComm)
          issue.Comment.Remove(viewcomm);
        else
          viewcomm.Viewpoint = null;
      }



      HasBeenSaved = false;
    }
    public void RemoveView(IEnumerable<ViewPoint> selectetitems, Markup issue, bool delComm)
    {
      foreach (var item in selectetitems)
      {
        if (File.Exists(Path.Combine(TempPath, issue.Topic.Guid, item.Viewpoint)))
          File.Delete(Path.Combine(TempPath, issue.Topic.Guid, item.Viewpoint));
        if (File.Exists(item.SnapshotPath))
          File.Delete(item.SnapshotPath);

        var guid = item.Guid;
        issue.Viewpoints.Remove(item);
        //remove comments associated with that view
        var viewcomments = issue.Comment.Where(x => x.Viewpoint.Guid == guid).ToList();
        foreach (var viewcomm in viewcomments)
        {
          if (delComm)
            issue.Comment.Remove(viewcomm);
          else
            viewcomm.Viewpoint = null;
        }


      }
      HasBeenSaved = false;
    }

    public void MergeBcfFile(IEnumerable<BcfFile> bcfFiles)
    {
      try
      {

        foreach (var bcf in bcfFiles)
        {
          foreach (var mergedIssue in bcf.Issues)
          {
            //it's a new issue
            if (!Issues.Any(x => x.Topic != null && mergedIssue.Topic != null && x.Topic.Guid == mergedIssue.Topic.Guid))
            {
              string sourceDir = Path.Combine(bcf.TempPath, mergedIssue.Topic.Guid);
              string destDir = Path.Combine(TempPath, mergedIssue.Topic.Guid);

              Directory.Move(sourceDir, destDir);
              //update path set for binding
              foreach (var view in mergedIssue.Viewpoints)
              {
                view.SnapshotPath = Path.Combine(TempPath, mergedIssue.Topic.Guid, view.Snapshot);
              }
              Issues.Add(mergedIssue);

            }
            //it exists, let's loop comments and views
            else
            {
              var issue = Issues.First(x => x.Topic.Guid == mergedIssue.Topic.Guid);
              var newComments = mergedIssue.Comment.Where(x => issue.Comment.All(y => y.Guid != x.Guid)).ToList();
              if (newComments.Any())
                foreach (var newComment in newComments)
                  issue.Comment.Add(newComment);
              //sort comments
              issue.Comment = new ObservableCollection<Comment>(issue.Comment.OrderByDescending(x => x.Date));

              var newViews = mergedIssue.Viewpoints.Where(x => issue.Viewpoints.All(y => y.Guid != x.Guid)).ToList();
              if (newViews.Any())
                foreach (var newView in newViews)
                {
                  //to avoid conflicts in case both contain a snapshot.png or viewpoint.bcfv
                  //img to be merged
                  string sourceFile = newView.SnapshotPath;
                  //assign new safe name based on guid
                  newView.Snapshot = newView.Guid + ".png";
                  //set new temp path for binding
                  newView.SnapshotPath = Path.Combine(TempPath, issue.Topic.Guid, newView.Snapshot);
                  //assign new safe name based on guid
                  newView.Viewpoint = newView.Guid + ".bcfv";
                  File.Move(sourceFile, newView.SnapshotPath);
                  issue.Viewpoints.Add(newView);


                }
            }
          }
          Utils.DeleteDirectory(bcf.TempPath);
        }
        HasBeenSaved = false;



      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }

    }

    [field: NonSerialized]
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(String info)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(info));
      }
    }
  }
}
