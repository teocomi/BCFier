using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bcfier.Bcf.Bcf2;

namespace Bcfier.Bcf.Bcf2
{
  //I store the deserialized VisualizationInfo here (inside the markup) so there is a 1 to 1 corrispondence
  //betweeen the Markup/Viewpoints tag and the actual viewpoints
  //and I don't need to add any new class
  //could be improved by creating a proper Model View / View Model class
  public partial class ViewPoint
  {
    private VisualizationInfo _visInfoField;

    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public VisualizationInfo VisInfo
    {
      get
      {
        return this._visInfoField;
      }
      set
      {
        this._visInfoField = value;
      }
    }

    private string _snapshotPath;

    //used for an easier binding in the UI
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public string SnapshotPath
    {
      get
      {
        return this._snapshotPath;
      }
      set
      {
        this._snapshotPath = value;
        NotifyPropertyChanged("SnapshotPath");
      }
    }
  }
}
