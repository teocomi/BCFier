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
  public partial class Version
  {

    private string detailedVersionField;

    private string versionIdField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string DetailedVersion
    {
      get { return this.detailedVersionField; }
      set { this.detailedVersionField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string VersionId
    {
      get { return this.versionIdField; }
      set { this.versionIdField = value; }
    }
  }
}