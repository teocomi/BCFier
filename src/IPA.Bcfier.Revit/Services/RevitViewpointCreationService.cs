using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IPA.Bcfier.Models.Bcf;
using IPA.Bcfier.Revit.OpenProject;

namespace IPA.Bcfier.Revit.Services
{
    public class RevitViewpointCreationService
    {
        private readonly UIDocument _uiDocument;

        public RevitViewpointCreationService(UIDocument uiDocument)
        {
            _uiDocument = uiDocument;
        }

        //<summary>
        //Generate a VisualizationInfo of the current view
        //</summary>
        //<returns></returns>
        public BcfViewpoint GenerateViewpoint()
        {
            try
            {
                var doc = _uiDocument.Document;

                var bcfViewpoint = new BcfViewpoint();

                //Corners of the active UI view
                var topLeft = _uiDocument.GetOpenUIViews()[0].GetZoomCorners()[0];
                var bottomRight = _uiDocument.GetOpenUIViews()[0].GetZoomCorners()[1];

                bcfViewpoint.ClippingPlanes = GetBcfClippingPlanes(_uiDocument);

                if (_uiDocument.ActiveView.ViewType == ViewType.ThreeD)
                {
                    //It's a 3d view
                    var viewCenter = new XYZ();
                    var view3D = (View3D)_uiDocument.ActiveView;
                    double zoomValue = 1;
                    // it is a orthogonal view
                    if (!view3D.IsPerspective)
                    {
                        double x = (topLeft.X + bottomRight.X) / 2;
                        double y = (topLeft.Y + bottomRight.Y) / 2;
                        double z = (topLeft.Z + bottomRight.Z) / 2;
                        //center of the UI view
                        viewCenter = new XYZ(x, y, z);

                        //vector going from BR to TL
                        XYZ diagVector = topLeft.Subtract(bottomRight);
                        //length of the vector
                        double dist = topLeft.DistanceTo(bottomRight) / 2;

                        //ViewToWorldScale value
                        zoomValue = dist * Math.Sin(diagVector.AngleTo(view3D.RightDirection)).ToMeters();

                        ViewOrientation3D t = RevitUtilities.ConvertBasePoint(doc, viewCenter, _uiDocument.ActiveView.ViewDirection,
                        _uiDocument.ActiveView.UpDirection, false);

                        XYZ c = t.EyePosition;
                        XYZ vi = t.ForwardDirection;
                        XYZ up = t.UpDirection;

                        bcfViewpoint.OrthogonalCamera = new BcfViewpointOrthogonalCamera
                        {
                            ViewPoint =
                              {
                                X = c.X.ToMeters(),
                                Y = c.Y.ToMeters(),
                                Z = c.Z.ToMeters()
                              },
                            UpVector =
                              {
                                X = up.X.ToMeters(),
                                Y = up.Y.ToMeters(),
                                Z = up.Z.ToMeters()
                              },
                            Direction =
                              {
                                X = vi.X.ToMeters() * -1,
                                Y = vi.Y.ToMeters() * -1,
                                Z = vi.Z.ToMeters() * -1
                              },
                            ViewToWorldScale = zoomValue
                        };
                    }
                    // it is a perspective view
                    else
                    {
                        viewCenter = _uiDocument.ActiveView.Origin;
                        //revit default value
                        zoomValue = 45;

                        ViewOrientation3D t = RevitUtilities.ConvertBasePoint(doc, viewCenter, _uiDocument.ActiveView.ViewDirection,
                         _uiDocument.ActiveView.UpDirection, false);

                        XYZ c = t.EyePosition;
                        XYZ vi = t.ForwardDirection;
                        XYZ up = t.UpDirection;

                        bcfViewpoint.PerspectiveCamera = new BcfViewpointPerspectiveCamera
                        {
                            ViewPoint =
                              {
                                X = c.X.ToMeters(),
                                Y = c.Y.ToMeters(),
                                Z = c.Z.ToMeters()
                              },
                            UpVector =
                              {
                                X = up.X.ToMeters(),
                                Y = up.Y.ToMeters(),
                                Z = up.Z.ToMeters()
                              },
                            Direction =
                              {
                                X = vi.X.ToMeters() * -1,
                                Y = vi.Y.ToMeters() * -1,
                                Z = vi.Z.ToMeters() * -1
                              },
                            FieldOfView = zoomValue
                        };
                    }
                }
                //COMPONENTS PART
                string versionName = doc.Application.VersionName;

                var visibleElems = new FilteredElementCollector(doc, doc.ActiveView.Id)
                  .WhereElementIsNotElementType()
                  .WhereElementIsViewIndependent()
                .ToElementIds();
                var hiddenElems = new FilteredElementCollector(doc)
                  .WhereElementIsNotElementType()
                  .WhereElementIsViewIndependent()
                  .Where(x => x.IsHidden(doc.ActiveView)
                    || !doc.ActiveView.IsElementVisibleInTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate, x.Id)).Select(x => x.Id)
                   ;//would need to check how much this is affecting performance

                var selectedElems = _uiDocument.Selection.GetElementIds();

                var viewpointComponents = new BcfViewpointComponents();
                bcfViewpoint.ViewpointComponents = viewpointComponents;
                viewpointComponents.Visibility = new BcfViewpointComponentVisibility();

                //TODO: set ViewSetupHints
                //TODO: create clipping planes
                //list of hidden components is smaller than the list of visible components
                if (visibleElems.Count() > hiddenElems.Count())
                {
                    viewpointComponents.Visibility.DefaultVisibility = true;
                    viewpointComponents.Visibility.Exceptions = hiddenElems.Select(x => new BcfViewpointComponent
                    {
                        OriginatingSystem = versionName,
                        IfcGuid = ExportUtils.GetExportId(doc, x).ToIfcGuid(),
                        AuthoringToolId = x.Value.ToString()
                    }).ToList();
                }
                //list of visible components is smaller or equals the list of hidden components
                else
                {
                    viewpointComponents.Visibility.DefaultVisibility = false;
                    viewpointComponents.Visibility.Exceptions = visibleElems.Select(x => new BcfViewpointComponent
                    {
                        OriginatingSystem = versionName,
                        IfcGuid = ExportUtils.GetExportId(doc, x).ToIfcGuid(),
                        AuthoringToolId = x.Value.ToString()
                    }).ToList();
                }

                //selected elements
                viewpointComponents.SelectedComponents = selectedElems.Select(x => new BcfViewpointComponent
                {
                    OriginatingSystem = versionName,
                    IfcGuid = IfcGuidExtensions.ToIfcGuid(ExportUtils.GetExportId(doc, x)),
                    AuthoringToolId = x.Value.ToString()
                }).ToList();

                var snapshotBase64 = RevitUtilities.GetRevitSnapshotBase64(_uiDocument.Document);
                bcfViewpoint.SnapshotBase64 = snapshotBase64;

                return bcfViewpoint;
            }
            catch (System.Exception ex1)
            {
                TaskDialog.Show("Error generating viewpoint", "exception: " + ex1);
            }

            return null;
        }

        private static List<BcfViewpointClippingPlane> GetBcfClippingPlanes(UIDocument uiDocument)
        {
            if (uiDocument.ActiveView is not View3D view3D)
            {
                return new List<BcfViewpointClippingPlane>();
            }

            BoundingBoxXYZ sectionBox = view3D.GetSectionBox();
            XYZ transformedMin = sectionBox.Transform.OfPoint(sectionBox.Min);
            XYZ transformedMax = sectionBox.Transform.OfPoint(sectionBox.Max);
            Vector3 minCorner = transformedMin.ToVector3().ToMeters();
            Vector3 maxCorner = transformedMax.ToVector3().ToMeters();

            return new AxisAlignedBoundingBox(minCorner, maxCorner).ToClippingPlanes();
        }
    }
}
