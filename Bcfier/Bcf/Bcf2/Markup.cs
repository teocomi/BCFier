using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Bcfier.Data;
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
  public partial class Markup : INotifyPropertyChanged
  {

    private List<HeaderFile> headerField;

    private Topic topicField;

    private ObservableCollection<Comment> commentField;

    private ObservableCollection<ViewPoint> viewpointsField;


    /// <remarks/>
    [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    [System.Xml.Serialization.XmlArrayItemAttribute("File", Form = System.Xml.Schema.XmlSchemaForm.Unqualified,
      IsNullable = false)]
    public List<HeaderFile> Header
    {
      get { return this.headerField; }
      set { this.headerField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Topic Topic
    {
      get { return this.topicField; }
      set { this.topicField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Comment", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ObservableCollection<Comment> Comment
    {
      get { return this.commentField; }
      set
      {
        this.commentField = value;
        NotifyPropertyChanged("Comment");
      }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Viewpoints", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ObservableCollection<ViewPoint> Viewpoints
    {
      get { return this.viewpointsField; }
      set
      {
        this.viewpointsField = value;
        NotifyPropertyChanged("Viewpoints");

      }
    }
    /// <summary>
    /// Generates ViewCommentobjects from View and Comments Dynamically
    /// Could be removed by implmenting a proper MVVM model
    /// But this approach simplifies things a lot 
    /// </summary>

    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public ObservableCollection<ViewComment> ViewComments
    {
      get
      {
        var viewCommentsField = new ObservableCollection<ViewComment>();
        foreach (var viewpoint in Viewpoints)
        {
          var vc = new ViewComment
          {
            Viewpoint = viewpoint,
            Comments = new ObservableCollection<Comment>(Comment.Where(x => x.Viewpoint != null && x.Viewpoint.Guid == viewpoint.Guid))
          };
          viewCommentsField.Add(vc);
        }
        var vcEmpty = new ViewComment
        {
          Comments =
            new ObservableCollection<Comment>(Comment.Where(x => !Viewpoints.Any(v => x.Viewpoint != null && v.Guid == x.Viewpoint.Guid)))
        };
        viewCommentsField.Add(vcEmpty);
        return viewCommentsField;
      }
    }

    ////this might need to change
    //[System.Xml.Serialization.XmlIgnoreAttribute()]
    //public string LastCommentStatus
    //{
    //  get
    //  {
    //    if (Comment == null || !Comment.Any())
    //      return "";

    //    return Comment.LastOrDefault().Status;
    //  }   
    //}
    ////this might need to change
    //[System.Xml.Serialization.XmlIgnoreAttribute()]
    //public string LastCommentVerbalStatus
    //{
    //  get
    //  {
    //    if (Comment == null || !Comment.Any())
    //      return "";

    //    return Comment.LastOrDefault().VerbalStatus;
    //  }
    //}

    //parameterless constructor needed
    public Markup()
    {
    }
    public Markup(DateTime created)
    {
      Topic = new Topic();

      Comment = new ObservableCollection<Comment>();
      Viewpoints = new ObservableCollection<ViewPoint>();
      RegisterEvents();
      Header = new List<HeaderFile> { new HeaderFile { Date = created, DateSpecified = true } };
    }
    //when Views or comments change refresh the ViewComments
    public void RegisterEvents()
    {
      if (Viewpoints != null)
        Viewpoints.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args) { NotifyPropertyChanged("ViewComments"); };
      if (Comment != null)
        Comment.CollectionChanged += CommentOnCollectionChanged;
    }

    private void CommentOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
      NotifyPropertyChanged("ViewComments");
      //NotifyPropertyChanged("LastCommentStatus");
      //NotifyPropertyChanged("LastCommentVerbalStatus");
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

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  public partial class HeaderFile
  {

    private string filenameField;

    private System.DateTime dateField;

    private bool dateFieldSpecified;

    private string referenceField;

    private string ifcProjectField;

    private string ifcSpatialStructureElementField;

    private bool isExternalField;

    public HeaderFile()
    {
      this.isExternalField = true;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Filename
    {
      get { return this.filenameField; }
      set { this.filenameField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public System.DateTime Date
    {
      get { return this.dateField; }
      set { this.dateField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DateSpecified
    {
      get { return this.dateFieldSpecified; }
      set { this.dateFieldSpecified = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Reference
    {
      get { return this.referenceField; }
      set { this.referenceField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string IfcProject
    {
      get { return this.ifcProjectField; }
      set { this.ifcProjectField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string IfcSpatialStructureElement
    {
      get { return this.ifcSpatialStructureElementField; }
      set { this.ifcSpatialStructureElementField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(true)]
    public bool isExternal
    {
      get { return this.isExternalField; }
      set { this.isExternalField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class ViewPoint : INotifyPropertyChanged
  {

    private string viewpointField;

    private string snapshotField;

    private int indexField;

    private bool indexFieldSpecified;

    private string guidField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Viewpoint
    {
      get { return this.viewpointField; }
      set { this.viewpointField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Snapshot
    {
      get { return this.snapshotField; }
      set { this.snapshotField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public int Index
    {
      get { return this.indexField; }
      set { this.indexField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool IndexSpecified
    {
      get { return this.indexFieldSpecified; }
      set { this.indexFieldSpecified = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Guid
    {
      get { return this.guidField; }
      set { this.guidField = value; }
    }
    public ViewPoint()
    {
    }
    public ViewPoint(bool isFirst)
    {
      Guid = System.Guid.NewGuid().ToString();
      if (isFirst)
      {
        Viewpoint = "viewpoint.bcfv";
        Snapshot = "snapshot.png";
      }
      else
      {
        Viewpoint = Guid + ".bcfv";
        Snapshot = Guid + ".png";
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

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class Comment
  {

    //private string verbalStatusField;

    //private string statusField;

    private System.DateTime dateField;

    private string authorField;

    private string comment1Field;

    //private CommentTopic topicField;

    private CommentViewpoint viewpointField;

    //private CommentReplyToComment replyToCommentField;

    private System.DateTime modifiedDateField;

    private bool modifiedDateFieldSpecified;

    private string modifiedAuthorField;

    private string guidField;

    public Comment()
    {
      //this.statusField = "Unknown";
    }

    // /// <remarks/>
    // [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    // public string VerbalStatus
    // {
    //   get { return this.verbalStatusField; }
    //   set { this.verbalStatusField = value; }
    // }

    // /// <remarks/>
    // [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    // public string Status
    // {
    //   get { return this.statusField; }
    //   set { this.statusField = value; }
    // }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public System.DateTime Date
    {
      get { return this.dateField; }
      set { this.dateField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Author
    {
      get { return this.authorField; }
      set { this.authorField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Comment", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Comment1
    {
      get { return this.comment1Field; }
      set { this.comment1Field = value; }
    }

    // /// <remarks/>
    // [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    // public CommentTopic Topic
    // {
    //   get { return this.topicField; }
    //   set { this.topicField = value; }
    // }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public CommentViewpoint Viewpoint
    {
      get { return this.viewpointField; }
      set { this.viewpointField = value; }
    }

    // /// <remarks/>
    // [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    // public CommentReplyToComment ReplyToComment
    // {
    //   get { return this.replyToCommentField; }
    //   set { this.replyToCommentField = value; }
    // }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public System.DateTime ModifiedDate
    {
      get { return this.modifiedDateField; }
      set { this.modifiedDateField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ModifiedDateSpecified
    {
      get { return this.modifiedDateFieldSpecified; }
      set { this.modifiedDateFieldSpecified = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string ModifiedAuthor
    {
      get { return this.modifiedAuthorField; }
      set { this.modifiedAuthorField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Guid
    {
      get { return this.guidField; }
      set { this.guidField = value; }
    }
  }

  // /// <remarks/>
  // [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  // [System.SerializableAttribute()]
  // [System.Diagnostics.DebuggerStepThroughAttribute()]
  // [System.ComponentModel.DesignerCategoryAttribute("code")]
  // [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  // public partial class CommentTopic
  // {

  //   private string guidField;

  //   /// <remarks/>
  //   [System.Xml.Serialization.XmlAttributeAttribute()]
  //   public string Guid
  //   {
  //     get { return this.guidField; }
  //     set { this.guidField = value; }
  //   }
  // }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  public partial class CommentViewpoint
  {

    private string guidField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Guid
    {
      get { return this.guidField; }
      set { this.guidField = value; }
    }
  }

  // /// <remarks/>
  // [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  // [System.SerializableAttribute()]
  // [System.Diagnostics.DebuggerStepThroughAttribute()]
  // [System.ComponentModel.DesignerCategoryAttribute("code")]
  // [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  // public partial class CommentReplyToComment
  // {

  //   private string guidField;

  //   /// <remarks/>
  //   [System.Xml.Serialization.XmlAttributeAttribute()]
  //   public string Guid
  //   {
  //     get { return this.guidField; }
  //     set { this.guidField = value; }
  //   }
  // }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class BimSnippet
  {

    private string referenceField;

    private string referenceSchemaField;

    private string snippetTypeField;

    private bool isExternalField;

    public BimSnippet()
    {
      this.isExternalField = false;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Reference
    {
      get { return this.referenceField; }
      set { this.referenceField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string ReferenceSchema
    {
      get { return this.referenceSchemaField; }
      set { this.referenceSchemaField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string SnippetType
    {
      get { return this.snippetTypeField; }
      set { this.snippetTypeField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(false)]
    public bool isExternal
    {
      get { return this.isExternalField; }
      set { this.isExternalField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  public partial class Topic
  {

    private string[] referenceLinkField;

    private string titleField;

    private string priorityField;

    private int indexField;

    private bool indexFieldSpecified;

    private string[] labelsField;

    private System.DateTime creationDateField;

    //private bool creationDateFieldSpecified;

    private string creationAuthorField;

    private System.DateTime modifiedDateField;

    private bool modifiedDateFieldSpecified;

    private string modifiedAuthorField;

    private System.DateTime dueDateField;

    private bool dueDateFieldSpecified;

    private string assignedToField;

    private string stageField;

    private string descriptionField;

    private BimSnippet bimSnippetField;

    private TopicDocumentReferences[] documentReferencesField;

    private TopicRelatedTopics[] relatedTopicsField;

    private string guidField;

    private string topicTypeField;

    private ObservableCollection<string> topicTypesCollection;

    private string topicStatusField;

    private ObservableCollection<string> topicStatusesCollection;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string[] ReferenceLink
    {
      get { return this.referenceLinkField; }
      set { this.referenceLinkField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Title
    {
      get { return this.titleField; }
      set { this.titleField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Priority
    {
      get { return this.priorityField; }
      set { this.priorityField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public int Index
    {
      get
      {
        return this.indexField;
      }
      set
      {
        this.indexField = value;
      }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool IndexSpecified
    {
      get
      {
        return this.indexFieldSpecified;
      }
      set
      {
        this.indexFieldSpecified = value;
      }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Labels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string[] Labels
    {
      get { return this.labelsField; }
      set { this.labelsField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public System.DateTime CreationDate
    {
      get { return this.creationDateField; }
      set { this.creationDateField = value; }
    }

    // /// <remarks/>
    // [System.Xml.Serialization.XmlIgnoreAttribute()]
    // public bool CreationDateSpecified
    // {
    //   get { return this.creationDateFieldSpecified; }
    //   set { this.creationDateFieldSpecified = value; }
    // }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string CreationAuthor
    {
      get { return this.creationAuthorField; }
      set { this.creationAuthorField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public System.DateTime ModifiedDate
    {
      get { return this.modifiedDateField; }
      set { this.modifiedDateField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ModifiedDateSpecified
    {
      get { return this.modifiedDateFieldSpecified; }
      set { this.modifiedDateFieldSpecified = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string ModifiedAuthor
    {
      get { return this.modifiedAuthorField; }
      set { this.modifiedAuthorField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public System.DateTime DueDate
    {
      get
      {
        return this.dueDateField;
      }
      set
      {
        this.dueDateField = value;
      }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DueDateSpecified
    {
      get
      {
        return this.dueDateFieldSpecified;
      }
      set
      {
        this.dueDateFieldSpecified = value;
      }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string AssignedTo
    {
      get { return this.assignedToField; }
      set { this.assignedToField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Stage
    {
      get
      {
        return this.stageField;
      }
      set
      {
        this.stageField = value;
      }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Description
    {
      get { return this.descriptionField; }
      set { this.descriptionField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public BimSnippet BimSnippet
    {
      get { return this.bimSnippetField; }
      set { this.bimSnippetField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("DocumentReferences",
      Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public TopicDocumentReferences[] DocumentReferences
    {
      get { return this.documentReferencesField; }
      set { this.documentReferencesField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("RelatedTopics", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public TopicRelatedTopics[] RelatedTopics
    {
      get { return this.relatedTopicsField; }
      set { this.relatedTopicsField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Guid
    {
      get { return this.guidField; }
      set { this.guidField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TopicType
    {
      get { return this.topicTypeField; }
      set { this.topicTypeField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TopicStatus
    {
      get { return this.topicStatusField; }
      set { this.topicStatusField = value; }
    }

    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public ObservableCollection<string> TopicTypesCollection
    {
      get { return topicTypesCollection; }
      set { this.topicTypesCollection = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public ObservableCollection<string> TopicStatusesCollection
    {
      get { return topicStatusesCollection; }
      set { this.topicStatusesCollection = value; }
    }

    public Topic()
    {
      Guid = System.Guid.NewGuid().ToString();
      CreationDate = DateTime.Now;
      ModifiedDate = CreationDate;

      TopicStatusesCollection = new ObservableCollection<string>();
      TopicTypesCollection = new ObservableCollection<string>();
      foreach (var status in Globals.AvailStatuses)
      {
        TopicStatusesCollection.Add(status);
      }
      foreach (var type in Globals.AvailTypes)
      {
        TopicTypesCollection.Add(type);
      }


    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  public partial class TopicDocumentReferences
  {

    private string referencedDocumentField;

    private string descriptionField;

    private string guidField;

    private bool isExternalField;

    public TopicDocumentReferences()
    {
      this.isExternalField = false;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string ReferencedDocument
    {
      get { return this.referencedDocumentField; }
      set { this.referencedDocumentField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Description
    {
      get { return this.descriptionField; }
      set { this.descriptionField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Guid
    {
      get { return this.guidField; }
      set { this.guidField = value; }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    [System.ComponentModel.DefaultValueAttribute(false)]
    public bool isExternal
    {
      get { return this.isExternalField; }
      set { this.isExternalField = value; }
    }
  }

  /// <remarks/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1586.0")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  public partial class TopicRelatedTopics
  {

    private string guidField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Guid
    {
      get { return this.guidField; }
      set { this.guidField = value; }
    }
  }
}
