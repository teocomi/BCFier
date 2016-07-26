using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;

using Bcfier.Data.Utils;
using Bcfier.Navisworks.Data;
using Bcfier.Bcf.Bcf2;
using Autodesk.Navisworks.Api;
using ComBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;


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

        var v = view.VisInfo;
        
        //current document
        var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
        NavisUtils.GetGunits(doc);

        Viewpoint viewpoint = new Viewpoint();
       
        //orthogonal
        if (v.OrthogonalCamera != null)
        {
          if (v.OrthogonalCamera.CameraViewPoint == null || v.OrthogonalCamera.CameraUpVector == null || v.OrthogonalCamera.CameraDirection == null)
            return;

          var zoom = v.OrthogonalCamera.ViewToWorldScale.ToInternal();
          var cameraDirection = NavisUtils.GetNavisVector(v.OrthogonalCamera.CameraDirection);
          var cameraUpVector = NavisUtils.GetNavisVector(v.OrthogonalCamera.CameraUpVector);
          var cameraViewPoint = NavisUtils.GetNavisXYZ(v.OrthogonalCamera.CameraViewPoint);

          viewpoint.Position = cameraViewPoint;
          viewpoint.AlignUp(cameraUpVector);
          viewpoint.AlignDirection(cameraDirection);
          viewpoint.Projection = ViewpointProjection.Orthographic;
          viewpoint.FocalDistance = 1;

          //TODO
          //for better zooming from revit should use > zoom * 1.25
          //for better zooming from tekla should use > zoom / 1.25
          //still not sure why
          Point3D xyzTL = cameraViewPoint.Add(cameraUpVector.Multiply(zoom));
          var dist = xyzTL.DistanceTo(cameraViewPoint);
          viewpoint.SetExtentsAtFocalDistance(1, dist);
        }
        //perspective
        else if (v.PerspectiveCamera != null)
        {
          if (v.PerspectiveCamera.CameraViewPoint == null || v.PerspectiveCamera.CameraUpVector == null || v.PerspectiveCamera.CameraDirection == null)
            return;

          var zoom = v.PerspectiveCamera.FieldOfView;
          var cameraDirection = NavisUtils.GetNavisVector(v.PerspectiveCamera.CameraDirection);
          var cameraUpVector = NavisUtils.GetNavisVector(v.PerspectiveCamera.CameraUpVector);
          var cameraViewPoint = NavisUtils.GetNavisXYZ(v.PerspectiveCamera.CameraViewPoint);

          viewpoint.Position = cameraViewPoint;
          viewpoint.AlignUp(cameraUpVector);
          viewpoint.AlignDirection(cameraDirection);
          viewpoint.Projection = ViewpointProjection.Perspective;
          viewpoint.FocalDistance = zoom;
        }
      
        doc.CurrentViewpoint.CopyFrom(viewpoint);


        //show/hide elements
        //todo: needs improvement
        //todo: add settings
        if (v.Components != null && v.Components.Any())
        {
          List<ModelItem> attachedElems = new List<ModelItem>();
          List<ModelItem> elems = doc.Models.First.RootItem.DescendantsAndSelf.ToList<ModelItem>();


          foreach (var item in elems.Where(o => o.InstanceGuid != Guid.Empty))
          {
            string ifcguid = IfcGuid.ToIfcGuid(item.InstanceGuid).ToString();
            if (v.Components.Any(o => o.IfcGuid == ifcguid))
              attachedElems.Add(item);

          }
          if (attachedElems.Any())//avoid to hide everything if no elements matches
          {
            if (UserSettings.Get("selattachedelems") == "0")
            {
              List<ModelItem> elemsVisible = new List<ModelItem>();
              foreach (var item in attachedElems)
              {
                elemsVisible.AddRange(item.AncestorsAndSelf);
              }
              foreach (var item in elemsVisible)
                elems.Remove(item);

              doc.Models.ResetAllHidden();
              doc.Models.SetHidden(elems, true);
            }
            else
            {
              doc.CurrentSelection.Clear();
              doc.CurrentSelection.AddRange(attachedElems);
            }
          }
        }
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
