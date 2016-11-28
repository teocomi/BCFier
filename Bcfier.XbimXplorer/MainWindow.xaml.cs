using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Bcfier.Bcf.Bcf2;
using Xbim.Common.Geometry;
using Xbim.Presentation;
using Xbim.Presentation.XplorerPluginSystem;
using PerspectiveCamera = Bcfier.Bcf.Bcf2.PerspectiveCamera;

namespace Bcfier.XbimXplorer
{
    // todo: implement IXbimXplorerPluginMessageReceiver interface
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [XplorerUiElement(PluginWindowUiContainerEnum.LayoutDoc, PluginWindowActivation.OnMenu, "BCFier BCF Editor")]
    public partial class MainWindow : IXbimXplorerPluginWindow //, IXbimXplorerPluginMessageReceiver
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowTitle = "BCFier BCF Editor";
        }
        
        // --------------------------
        // plugin system related code 
        //

        private IXbimXplorerPluginMasterWindow _xpWindow;

        public void BindUi(IXbimXplorerPluginMasterWindow mainWindow)
        {
            _xpWindow = mainWindow;
        }

        public string WindowTitle { get; }
        
        private void OnOpenView(object sender, ExecutedRoutedEventArgs e)
        {

            if (Bcfier.SelectedBcf() == null)
                return;
            var view = e?.Parameter as ViewPoint;
            
            if (view == null)
                return;
            var v = view.VisInfo;

            var position = new XbimPoint3D();
            var direction = new XbimPoint3D();
            var upDirection = new XbimPoint3D();

            if (v.PerspectiveCamera != null)
            {
                // todo: this works internally, but we must ensure it's compatible with other bcf viewers
                var pc = v.PerspectiveCamera;
                position = new XbimPoint3D(pc.CameraViewPoint.X, pc.CameraViewPoint.Y, pc.CameraViewPoint.Z);
                direction = new XbimPoint3D(pc.CameraDirection.X, pc.CameraDirection.Y, pc.CameraDirection.Z);
                upDirection = new XbimPoint3D(pc.CameraUpVector.X, pc.CameraUpVector.Y, pc.CameraUpVector.Z);

                _xpWindow.DrawingControl.Viewport.Orthographic = false;
                var pCam = _xpWindow.DrawingControl.Viewport.Camera as System.Windows.Media.Media3D.PerspectiveCamera;
                if (pCam != null)
                    pCam.FieldOfView = pc.FieldOfView;
            }
            else if (v.OrthogonalCamera != null)
            {
                // todo: this works internally, but we must ensure it's compatible with other bcf viewers
                var pc = v.OrthogonalCamera;
                _xpWindow.DrawingControl.Viewport.Orthographic = true;
                position = new XbimPoint3D(pc.CameraViewPoint.X, pc.CameraViewPoint.Y, pc.CameraViewPoint.Z);
                direction = new XbimPoint3D(pc.CameraDirection.X, pc.CameraDirection.Y, pc.CameraDirection.Z);
                upDirection = new XbimPoint3D(pc.CameraUpVector.X, pc.CameraUpVector.Y, pc.CameraUpVector.Z);

                var pCam = _xpWindow.DrawingControl.Viewport.Camera as OrthographicCamera;
                if (pCam != null)
                    pCam.Width = pc.ViewToWorldScale;
            }
            var directionV = new XbimVector3D(direction.X, direction.Y, direction.Z);
            var upDirectionV = new XbimVector3D(upDirection.X, upDirection.Y, upDirection.Z);

            var pos = new Point3D(position.X, position.Y, position.Z);
            var dir = new Vector3D(directionV.X, directionV.Y, directionV.Z);
            var upDir = new Vector3D(upDirectionV.X, upDirectionV.Y, upDirectionV.Z);
            _xpWindow.DrawingControl.Viewport.SetView(pos, dir, upDir, 500);

            if (v.ClippingPlanes != null && v.ClippingPlanes.Any()) { 
                var curP = v.ClippingPlanes[0];
                _xpWindow.DrawingControl.SetCutPlane(
                    curP.Location.X, curP.Location.Y, curP.Location.Z,
                    curP.Direction.X, curP.Direction.Y, curP.Direction.Z
                    );
            }
            else
            {
                _xpWindow.DrawingControl.ClearCutPlane();
            }

            // todo: components list to be implemented
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

                var dialog = new AddViewXbim(issue, Bcfier.SelectedBcf().TempPath, _xpWindow.DrawingControl)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                dialog.ShowDialog();
                if (!dialog.DialogResult.HasValue || !dialog.DialogResult.Value)
                    return;

                //generate and set the VisInfo
                issue.Viewpoints.Last().VisInfo = GenerateViewpoint(_xpWindow.DrawingControl);

                //get filename
                var fileName = _xpWindow.GetOpenedModelFileName();
                if (string.IsNullOrEmpty(fileName))
                    fileName = "Unknown";
                issue.Header[0].Filename = fileName;
                Bcfier.SelectedBcf().HasBeenSaved = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding a View!", "exception: " + ex);
            }
        }

        private static VisualizationInfo GenerateViewpoint(DrawingControl3D control3D)
        {
            try
            {
                var v = new VisualizationInfo();
                // it is a orthogonal view
                if (control3D.Viewport.Orthographic)
                {
                    // ReSharper disable once RedundantNameQualifier
                    var cam = control3D.Viewport.Camera as System.Windows.Media.Media3D.OrthographicCamera;

                    if (cam != null)
                        v.OrthogonalCamera = new OrthogonalCamera
                        {
                            // todo: this works internally, but we must ensure it's compatible with other bcf viewers
                            CameraViewPoint = 
                            {
                                X = cam.Position.X,
                                Y = cam.Position.Y,
                                Z = cam.Position.Z
                            },
                            CameraUpVector =
                            {
                                X = cam.UpDirection.X,
                                Y = cam.UpDirection.Y,
                                Z = cam.UpDirection.Z
                            },
                            CameraDirection =
                            {
                                X = cam.LookDirection.X,
                                Y = cam.LookDirection.Y,
                                Z = cam.LookDirection.Z
                            },
                            ViewToWorldScale = cam.Width
                        };
                }
                // it is a perspective view
                else
                {
                    // todo: this works internally, but we must ensure it's compatible with other bcf viewers
                    var cam = control3D.Viewport.Camera as System.Windows.Media.Media3D.PerspectiveCamera;
                    if (cam != null)
                        v.PerspectiveCamera = new PerspectiveCamera()
                        {
                            CameraViewPoint =
                            {
                                X = cam.Position.X,
                                Y = cam.Position.Y,
                                Z = cam.Position.Z
                            },
                            CameraUpVector =
                            {
                                X = cam.UpDirection.X,
                                Y = cam.UpDirection.Y,
                                Z = cam.UpDirection.Z
                            },
                            CameraDirection =
                            {
                                X = cam.LookDirection.X,
                                Y = cam.LookDirection.Y,
                                Z = cam.LookDirection.Z
                            },
                            FieldOfView = cam.FieldOfView
                        };
                }

                /* todo components list to be implemented

                //COMPONENTS PART
                var versionName = "XbimXplorer BCFIer Plugin";
                v.Components = new List<Component>();

                var visibleElems = new FilteredElementCollector(doc, doc.ActiveView.Id)
                    .WhereElementIsNotElementType()
                    .WhereElementIsViewIndependent()
                    .ToElementIds();
                var hiddenElems = new FilteredElementCollector(doc)
                        .WhereElementIsNotElementType()
                        .WhereElementIsViewIndependent()
                        .Where(x => x.IsHidden(doc.ActiveView)
                                    ||
                                    !doc.ActiveView.IsElementVisibleInTemporaryViewMode(
                                        TemporaryViewMode.TemporaryHideIsolate, x.Id)).ToList();
                    //would need to check how much this is affecting performance

                var selectedElems = uidoc.Selection.GetElementIds();
                
                //include only hidden elements and selected in the BCF
                if (visibleElems.Count() > hiddenElems.Count())
                {
                    foreach (var elem in hiddenElems)
                    {
                        v.Components.Add(new Component
                        {
                            OriginatingSystem = versionName,
                            IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, elem.Id)),
                            Visible = false,
                            Selected = false,
                            AuthoringToolId = elem.Id.IntegerValue.ToString()
                        });
                    }
                    foreach (var elem in selectedElems)
                    {
                        v.Components.Add(new Component
                        {
                            OriginatingSystem = versionName,
                            IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, elem)),
                            Visible = true,
                            Selected = true,
                            AuthoringToolId = elem.IntegerValue.ToString()
                        });
                    }
                }
                //include only visible elements
                //all the others are hidden
                else
                {
                    foreach (var elem in visibleElems)
                    {
                        v.Components.Add(new Component
                        {
                            OriginatingSystem = versionName,
                            IfcGuid = IfcGuid.ToIfcGuid(ExportUtils.GetExportId(doc, elem)),
                            Visible = true,
                            Selected = selectedElems.Contains(elem),
                            AuthoringToolId = elem.IntegerValue.ToString()
                        });
                    }
                }
                */
                return v;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating viewpoint", "exception: " + ex);
            }
            return null;
        }
    }
}