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
  public partial class ProjectExtension
  {

    private Project projectField;

    private string extensionSchemaField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Project Project
    {
      get { return this.projectField; }
      set { this.projectField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified,
      DataType = "anyURI")]
    public string ExtensionSchema
    {
      get { return this.extensionSchemaField; }
      set { this.extensionSchemaField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class Project
  {

    private string nameField;

    private string projectIdField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Name
    {
      get { return this.nameField; }
      set { this.nameField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ProjectId
    {
      get { return this.projectIdField; }
      set { this.projectIdField = value; }
    }
  }
}
