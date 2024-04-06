using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IPA.Bcfier.Models.Bcf;
using IPA.Bcfier.Revit.OpenProject;

namespace IPA.Bcfier.Revit.Services
{
    public class RevitViewpointDisplayService
    {
        private readonly UIDocument _uiDocument;

        public RevitViewpointDisplayService(UIDocument uiDocument)
        {
            _uiDocument = uiDocument;
        }

        public ViewContinuationInstructions? DisplayViewpoint(BcfViewpoint bcfViewpoint)
        {
            try
            {
                Document doc = _uiDocument.Document;
                // We might later want to change this so it can also optionally show the viewpoint
                // in the current view
                var uniqueView = true;

                ElementId viewId = null;
                // IS ORTHOGONAL
                if (bcfViewpoint.OrthogonalCamera != null)
                {
                    if (bcfViewpoint.OrthogonalCamera.ViewPoint == null || bcfViewpoint.OrthogonalCamera.UpVector == null || bcfViewpoint.OrthogonalCamera.Direction == null)
                    {
                        return null;
                    }

                    //type = "OrthogonalCamera";
                    var zoom = bcfViewpoint.OrthogonalCamera.ViewToWorldScale.ToFeet();
                    var cameraDirection = RevitUtilities.GetRevitXYZ(bcfViewpoint.OrthogonalCamera.Direction);
                    var cameraUpVector = RevitUtilities.GetRevitXYZ(bcfViewpoint.OrthogonalCamera.UpVector);
                    var cameraViewPoint = RevitUtilities.GetRevitXYZ(bcfViewpoint.OrthogonalCamera.ViewPoint);
                    var orient3D = RevitUtilities.ConvertBasePoint(doc, cameraViewPoint, cameraDirection, cameraUpVector, true);

                    View3D orthoView = null;
                    //if active view is 3d ortho use it
                    if (doc.ActiveView.ViewType == ViewType.ThreeD)
                    {
                        var activeView3D = doc.ActiveView as View3D;
                        if (!activeView3D.IsPerspective)
                            orthoView = activeView3D;
                    }
                    if (orthoView == null)
                    {
                        //try to use an existing 3D view
                        IEnumerable<View3D> viewcollector3D = Get3DViews(doc);
                        if (viewcollector3D.Any(o => o.Name == "{3D}" || o.Name == "BCFortho"))
                            orthoView = viewcollector3D.First(o => o.Name == "{3D}" || o.Name == "BCFortho");
                    }
                    using (var trans = new Transaction(_uiDocument.Document))
                    {
                        if (trans.Start("Open orthogonal view") == TransactionStatus.Started)
                        {
                            //create a new 3d ortho view

                            if (orthoView == null || uniqueView)
                            {
                                orthoView = View3D.CreateIsometric(doc, GetFamilyViews(doc).First().Id);
                                orthoView.Name = (uniqueView) ? "BCFortho" + DateTime.Now.ToString("yyyyMMddTHHmmss") : "BCFortho";
                            }
                            else
                            {
                                //reusing an existing view, I net to reset the visibility
                                //placed this here because if set afterwards it doesn't work
                                orthoView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
                            }
                            orthoView.SetOrientation(orient3D);
                            trans.Commit();
                        }
                    }

                    viewId = orthoView.Id;
                    _uiDocument.RequestViewChange(orthoView);
                    //adjust view rectangle


                    double x = zoom;
                    //set UI view position and zoom
                    XYZ m_xyzTl = _uiDocument.ActiveView.Origin.Add(_uiDocument.ActiveView.UpDirection.Multiply(x)).Subtract(_uiDocument.ActiveView.RightDirection.Multiply(x));
                    XYZ m_xyzBr = _uiDocument.ActiveView.Origin.Subtract(_uiDocument.ActiveView.UpDirection.Multiply(x)).Add(_uiDocument.ActiveView.RightDirection.Multiply(x));
                    _uiDocument.GetOpenUIViews().First().ZoomAndCenterRectangle(m_xyzTl, m_xyzBr);
                }
                //perspective
                else if (bcfViewpoint.PerspectiveCamera != null)
                {
                    if (bcfViewpoint.PerspectiveCamera.ViewPoint == null || bcfViewpoint.PerspectiveCamera.UpVector == null || bcfViewpoint.PerspectiveCamera.Direction == null)
                    {
                        return null;
                    }

                    //not used since the fov cannot be changed in Revit
                    var zoom = bcfViewpoint.PerspectiveCamera.FieldOfView;
                    //FOV - not used

                    var cameraDirection = RevitUtilities.GetRevitXYZ(bcfViewpoint.PerspectiveCamera.Direction);
                    var cameraUpVector = RevitUtilities.GetRevitXYZ(bcfViewpoint.PerspectiveCamera.UpVector);
                    var cameraViewPoint = RevitUtilities.GetRevitXYZ(bcfViewpoint.PerspectiveCamera.ViewPoint);
                    var orient3D = RevitUtilities.ConvertBasePoint(doc, cameraViewPoint, cameraDirection, cameraUpVector, true);

                    View3D perspView = null;
                    //try to use an existing 3D view
                    IEnumerable<View3D> viewcollector3D = Get3DViews(doc);
                    if (viewcollector3D.Any(o => o.Name == "BCFpersp"))
                        perspView = viewcollector3D.First(o => o.Name == "BCFpersp");

                    using (var trans = new Transaction(_uiDocument.Document))
                    {
                        if (trans.Start("Open perspective view") == TransactionStatus.Started)
                        {
                            if (null == perspView || uniqueView)
                            {
                                perspView = View3D.CreatePerspective(doc, GetFamilyViews(doc).First().Id);
                                perspView.Name = (uniqueView) ? "BCFpersp" + DateTime.Now.ToString("yyyyMMddTHHmmss") : "BCFpersp";
                            }
                            else
                            {
                                //reusing an existing view, I net to reset the visibility
                                //placed this here because if set afterwards it doesn't work
                                perspView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
                            }

                            perspView.SetOrientation(orient3D);

                            // turn off the far clip plane
                            if (perspView.get_Parameter(BuiltInParameter.VIEWER_BOUND_ACTIVE_FAR).HasValue)
                            {
                                Parameter m_farClip = perspView.get_Parameter(BuiltInParameter.VIEWER_BOUND_ACTIVE_FAR);
                                m_farClip.Set(0);
                            }
                            perspView.CropBoxActive = true;
                            perspView.CropBoxVisible = true;

                            trans.Commit();
                        }
                    }

                    _uiDocument.RequestViewChange(perspView);
                    viewId = perspView.Id;
                }
                //no view included
                else
                {
                    return null;
                }

                Action viewContinuation = () =>
                {
                    if (bcfViewpoint.ViewpointComponents == null)
                    {
                        return;
                    }

                    var elementsToSelect = new List<ElementId>();
                    var elementsToHide = new List<ElementId>();
                    var elementsToShow = new List<ElementId>();

                    var visibleElems = new FilteredElementCollector(doc, doc.ActiveView.Id)
                        .WhereElementIsNotElementType()
                        .WhereElementIsViewIndependent()
                        .ToElementIds()
                        .Where(e => doc.GetElement(e).CanBeHidden(doc.ActiveView)); //might affect performance, but it's necessary

                    bool canSetVisibility = (bcfViewpoint.ViewpointComponents.Visibility != null &&
                      bcfViewpoint.ViewpointComponents.Visibility.DefaultVisibility &&
                      bcfViewpoint.ViewpointComponents.Visibility.Exceptions.Any());
                    bool canSetSelection = (bcfViewpoint.ViewpointComponents.SelectedComponents != null && bcfViewpoint.ViewpointComponents.SelectedComponents.Any());

                    //loop elements
                    foreach (var e in visibleElems)
                    {
                        var guid = ExportUtils.GetExportId(doc, e).ToIfcGuid();

                        if (canSetVisibility)
                        {
                            if (bcfViewpoint.ViewpointComponents.Visibility.DefaultVisibility)
                            {
                                if (bcfViewpoint.ViewpointComponents.Visibility.Exceptions.Any(x => x.IfcGuid == guid))
                                {
                                    elementsToHide.Add(e);
                                }
                            }
                            else
                            {
                                if (bcfViewpoint.ViewpointComponents.Visibility.Exceptions.Any(x => x.IfcGuid == guid))
                                {
                                    elementsToShow.Add(e);
                                }
                            }
                        }

                        if (canSetSelection)
                        {
                            if (bcfViewpoint.ViewpointComponents.SelectedComponents.Any(x => x.IfcGuid == guid))
                            {
                                elementsToSelect.Add(e);
                            }
                        }
                    }

                    using (var trans = new Transaction(_uiDocument.Document))
                    {
                        if (trans.Start("Apply BCF visibility and selection and section box") == TransactionStatus.Started)
                        {
                            if (elementsToHide.Any())
                                doc.ActiveView.HideElementsTemporary(elementsToHide);
                            //there are no items to hide, therefore hide everything and just show the visible ones
                            else if (elementsToShow.Any())
                                doc.ActiveView.IsolateElementsTemporary(elementsToShow);

                            if (elementsToSelect.Any())
                                _uiDocument.Selection.SetElementIds(elementsToSelect);

                            if (_uiDocument.ActiveView is View3D view3d)
                            {
                                ApplyClippingPlanes(_uiDocument, view3d, bcfViewpoint);
                            }
                        }

                        trans.Commit();
                    }

                    _uiDocument.RefreshActiveView();
                };

                return new ViewContinuationInstructions
                {
                    ViewContinuation = viewContinuation,
                    ViewId = viewId
                };
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error!", "exception: " + ex);
                return null;
            }
        }

        private IEnumerable<ViewFamilyType> GetFamilyViews(Document doc)
        {
            return from elem in new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
                   let type = elem as ViewFamilyType
                   where type.ViewFamily == ViewFamily.ThreeDimensional
                   select type;
        }

        private IEnumerable<View3D> Get3DViews(Document doc)
        {
            return from elem in new FilteredElementCollector(doc).OfClass(typeof(View3D))
                   let view = elem as View3D
                   select view;
        }

        public string GetName()
        {
            return "3D View";
        }

        // Take from:
        // https://github.com/opf/openproject-revit-add-in/blob/93e117ad10176f4fffa741116733a3ee113e9335/src/OpenProject.Revit/Entry/OpenViewpointEventHandler.cs#L212
        private const decimal _viewpointAngleThresholdRad = 0.087266462599716m;

        private void ApplyClippingPlanes(UIDocument uiDocument, View3D view, BcfViewpoint bcfViewpoint)
        {
            AxisAlignedBoundingBox boundingBox = GetViewpointClippingBox(bcfViewpoint);

            if (!boundingBox.Equals(AxisAlignedBoundingBox.Infinite))
            {
                view.SetSectionBox(ToRevitSectionBox(boundingBox));
                view.IsSectionBoxActive = true;
            }
        }

        private AxisAlignedBoundingBox GetViewpointClippingBox(BcfViewpoint bcfViewpoint)
        {
            return bcfViewpoint.ClippingPlanes
                .Select(p => p.ToAxisAlignedBoundingBox(_viewpointAngleThresholdRad))
                .Aggregate(AxisAlignedBoundingBox.Infinite, (current, nextBox) => current.MergeReduce(nextBox));
        }

        private static BoundingBoxXYZ ToRevitSectionBox(AxisAlignedBoundingBox box)
        {
            var min = new XYZ(
              box.Min.X == decimal.MinValue ? double.MinValue : ((double)box.Min.X).ToInternalRevitUnit(),
              box.Min.Y == decimal.MinValue ? double.MinValue : ((double)box.Min.Y).ToInternalRevitUnit(),
              box.Min.Z == decimal.MinValue ? double.MinValue : ((double)box.Min.Z).ToInternalRevitUnit());
            var max = new XYZ(
              box.Max.X == decimal.MaxValue ? double.MaxValue : ((double)box.Max.X).ToInternalRevitUnit(),
              box.Max.Y == decimal.MaxValue ? double.MaxValue : ((double)box.Max.Y).ToInternalRevitUnit(),
              box.Max.Z == decimal.MaxValue ? double.MaxValue : ((double)box.Max.Z).ToInternalRevitUnit());

            return new BoundingBoxXYZ { Min = min, Max = max };
        }
    }
}
