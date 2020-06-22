using Dangl;
using System;

namespace OpenProject.Shared.ViewModels.Bcf
{
  public class BcfHeaderFileViewModel : BindableBase
  {
    private DateTime? _fileDate;
    private string _fileName;
    private string _ifcProject;
    private string _ifcSpatialStructureElement;
    private bool _isExternal;
    private string _reference;

    public DateTime? FileDate
    {
      get => _fileDate;
      set => SetProperty(ref _fileDate, value);
    }

    public string FileName
    {
      get => _fileName;
      set => SetProperty(ref _fileName, value);
    }

    public string IfcProject
    {
      get => _ifcProject;
      set => SetProperty(ref _ifcProject, value);
    }

    public string IfcSpatialStructureElement
    {
      get => _ifcSpatialStructureElement;
      set => SetProperty(ref _ifcSpatialStructureElement, value);
    }

    public bool IsExternal
    {
      get => _isExternal;
      set => SetProperty(ref _isExternal, value);
    }

    public string Reference
    {
      get => _reference;
      set => SetProperty(ref _reference, value);
    }
  }
}
