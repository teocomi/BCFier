using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bcfier.Bcf.Bcf2
{

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
  public partial class VisualizationInfo
  {

    private Components componentsField;

    private OrthogonalCamera orthogonalCameraField;

    private PerspectiveCamera perspectiveCameraField;

    //Not part of BCF schema, added to support 2D views
    private SheetCamera sheetCameraField;

    private Line[] linesField;

    private ClippingPlane[] clippingPlanesField;

    private VisualizationInfoBitmap[] bitmapField;

    private string guidField;

    /// <remarks/>
    public Components Components
    {
      get { return this.componentsField; }
      set { this.componentsField = value; }
    }

    /// <remarks/>
    public OrthogonalCamera OrthogonalCamera
    {
      get { return this.orthogonalCameraField; }
      set { this.orthogonalCameraField = value; }
    }

    /// <remarks/>
    public PerspectiveCamera PerspectiveCamera
    {
      get { return this.perspectiveCameraField; }
      set { this.perspectiveCameraField = value; }
    }


    public SheetCamera SheetCamera
    {
      get { return this.sheetCameraField; }
      set { this.sheetCameraField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
    public Line[] Lines
    {
      get { return this.linesField; }
      set { this.linesField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
    public ClippingPlane[] ClippingPlanes
    {
      get { return this.clippingPlanesField; }
      set { this.clippingPlanesField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Bitmap")]
    public VisualizationInfoBitmap[] Bitmap
    {
      get
      {
        return this.bitmapField;
      }
      set
      {
        this.bitmapField = value;
      }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Guid
    {
      get { return this.guidField; }
      set { this.guidField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class Components
  {

    private ViewSetupHints viewSetupHintsField;

    private Component[] selectionField;

    private ComponentVisibility visibilityField;

    private ComponentColoringColor[] coloringField;

    /// <remarks/>
    public ViewSetupHints ViewSetupHints
    {
      get { return this.viewSetupHintsField; }
      set { this.viewSetupHintsField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
    public Component[] Selection
    {
      get { return this.selectionField; }
      set { this.selectionField = value; }
    }

    /// <remarks/>
    public ComponentVisibility Visibility
    {
      get { return this.visibilityField; }
      set { this.visibilityField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Color", IsNullable = false)]
    public ComponentColoringColor[] Coloring
    {
      get { return this.coloringField; }
      set { this.coloringField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class ViewSetupHints
  {

    private bool spacesVisibleField;

    private bool spacesVisibleFieldSpecified;

    private bool spaceBoundariesVisibleField;

    private bool spaceBoundariesVisibleFieldSpecified;

    private bool openingsVisibleField;

    private bool openingsVisibleFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool SpacesVisible
    {
      get { return this.spacesVisibleField; }
      set { this.spacesVisibleField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool SpacesVisibleSpecified
    {
      get { return this.spacesVisibleFieldSpecified; }
      set { this.spacesVisibleFieldSpecified = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool SpaceBoundariesVisible
    {
      get { return this.spaceBoundariesVisibleField; }
      set { this.spaceBoundariesVisibleField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool SpaceBoundariesVisibleSpecified
    {
      get { return this.spaceBoundariesVisibleFieldSpecified; }
      set { this.spaceBoundariesVisibleFieldSpecified = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool OpeningsVisible
    {
      get { return this.openingsVisibleField; }
      set { this.openingsVisibleField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool OpeningsVisibleSpecified
    {
      get { return this.openingsVisibleFieldSpecified; }
      set { this.openingsVisibleFieldSpecified = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class ClippingPlane
  {

    private Point locationField;

    private Direction directionField;

    /// <remarks/>
    public Point Location
    {
      get { return this.locationField; }
      set { this.locationField = value; }
    }

    /// <remarks/>
    public Direction Direction
    {
      get { return this.directionField; }
      set { this.directionField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class Point
  {

    private double xField;

    private double yField;

    private double zField;

    /// <remarks/>
    public double X
    {
      get { return this.xField; }
      set { this.xField = value; }
    }

    /// <remarks/>
    public double Y
    {
      get { return this.yField; }
      set { this.yField = value; }
    }

    /// <remarks/>
    public double Z
    {
      get { return this.zField; }
      set { this.zField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class Direction
  {

    private double xField;

    private double yField;

    private double zField;

    /// <remarks/>
    public double X
    {
      get { return this.xField; }
      set { this.xField = value; }
    }

    /// <remarks/>
    public double Y
    {
      get { return this.yField; }
      set { this.yField = value; }
    }

    /// <remarks/>
    public double Z
    {
      get { return this.zField; }
      set { this.zField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class Line
  {

    private Point startPointField;

    private Point endPointField;

    /// <remarks/>
    public Point StartPoint
    {
      get { return this.startPointField; }
      set { this.startPointField = value; }
    }

    /// <remarks/>
    public Point EndPoint
    {
      get { return this.endPointField; }
      set { this.endPointField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class PerspectiveCamera
  {

    private Point cameraViewPointField;

    private Direction cameraDirectionField;

    private Direction cameraUpVectorField;

    private double fieldOfViewField;

    //initialize fields
    public PerspectiveCamera()
    {
      CameraViewPoint = new Point();
      CameraDirection = new Direction();
      CameraUpVector = new Direction();
    }

    /// <remarks/>
    public Point CameraViewPoint
    {
      get { return this.cameraViewPointField; }
      set { this.cameraViewPointField = value; }
    }

    /// <remarks/>
    public Direction CameraDirection
    {
      get { return this.cameraDirectionField; }
      set { this.cameraDirectionField = value; }
    }

    /// <remarks/>
    public Direction CameraUpVector
    {
      get { return this.cameraUpVectorField; }
      set { this.cameraUpVectorField = value; }
    }

    /// <remarks/>
    public double FieldOfView
    {
      get { return this.fieldOfViewField; }
      set { this.fieldOfViewField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class OrthogonalCamera
  {

    private Point cameraViewPointField;

    private Direction cameraDirectionField;

    private Direction cameraUpVectorField;

    private double viewToWorldScaleField;

    //initialize fields
    public OrthogonalCamera()
    {
      CameraViewPoint = new Point();
      CameraDirection = new Direction();
      CameraUpVector = new Direction();
    }

    /// <remarks/>
    public Point CameraViewPoint
    {
      get { return this.cameraViewPointField; }
      set { this.cameraViewPointField = value; }
    }

    /// <remarks/>
    public Direction CameraDirection
    {
      get { return this.cameraDirectionField; }
      set { this.cameraDirectionField = value; }
    }

    /// <remarks/>
    public Direction CameraUpVector
    {
      get { return this.cameraUpVectorField; }
      set { this.cameraUpVectorField = value; }
    }

    /// <remarks/>
    public double ViewToWorldScale
    {
      get { return this.viewToWorldScaleField; }
      set { this.viewToWorldScaleField = value; }
    }
  }



  /// <summary>
  /// Custom BCFier field to support 2D views in Revit
  /// EXPERIMENTAL
  /// </summary>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class SheetCamera
  {

    private Point topLeft;

    private Point bottomRight;

    private int sheetID;

    private string sheetName;

    public SheetCamera()
    {
      TopLeft = new Point();
      BottomRight = new Point();
    }

    /// <remarks/>
    public Point TopLeft
    {
      get { return this.topLeft; }
      set { this.topLeft = value; }
    }

    /// <remarks/>
    public Point BottomRight
    {
      get { return this.bottomRight; }
      set { this.bottomRight = value; }
    }

    /// <remarks/>
    public int SheetID
    {
      get { return this.sheetID; }
      set { this.sheetID = value; }
    }

    /// <remarks/>
    public string SheetName
    {
      get { return this.sheetName; }
      set { this.sheetName = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class ComponentVisibility
  {

    private Component[] exceptionsField;

    private bool defaultVisibilityField;

    private bool defaultVisibilityFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
    public Component[] Exceptions
    {
      get { return this.exceptionsField; }
      set { this.exceptionsField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool DefaultVisibility
    {
      get { return this.defaultVisibilityField; }
      set { this.defaultVisibilityField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DefaultVisibilitySpecified
    {
      get { return this.defaultVisibilityFieldSpecified; }
      set { this.defaultVisibilityFieldSpecified = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class Component
  {

    private string originatingSystemField;

    private string authoringToolIdField;

    private string ifcGuidField;

    /// <remarks/>
    public string OriginatingSystem
    {
      get { return this.originatingSystemField; }
      set { this.originatingSystemField = value; }
    }

    /// <remarks/>
    public string AuthoringToolId
    {
      get { return this.authoringToolIdField; }
      set { this.authoringToolIdField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "normalizedString")]
    public string IfcGuid
    {
      get { return this.ifcGuidField; }
      set { this.ifcGuidField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  public partial class ComponentColoringColor
  {

    private Component[] componentField;

    private string colorField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Component")]
    public Component[] Component
    {
      get { return this.componentField; }
      set { this.componentField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "normalizedString")]
    public string Color
    {
      get { return this.colorField; }
      set { this.colorField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  public partial class VisualizationInfoBitmap
  {

    private BitmapFormat bitmapField;

    private string referenceField;

    private Point locationField;

    private Direction normalField;

    private Direction upField;

    private double heightField;

    /// <remarks/>
    public BitmapFormat Bitmap
    {
      get { return this.bitmapField; }
      set { this.bitmapField = value; }
    }

    /// <remarks/>
    public string Reference
    {
      get { return this.referenceField; }
      set { this.referenceField = value; }
    }

    /// <remarks/>
    public Point Location
    {
      get { return this.locationField; }
      set { this.locationField = value; }
    }

    /// <remarks/>
    public Direction Normal
    {
      get { return this.normalField; }
      set { this.normalField = value; }
    }

    /// <remarks/>
    public Direction Up
    {
      get { return this.upField; }
      set { this.upField = value; }
    }

    /// <remarks/>
    public double Height
    {
      get { return this.heightField; }
      set { this.heightField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  public enum BitmapFormat
  {

    /// <remarks/>
    PNG,

    /// <remarks/>
    JPG,
  }

}
