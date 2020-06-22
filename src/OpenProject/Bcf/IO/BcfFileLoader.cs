using OpenProject.Shared.ViewModels.Bcf;
using System;
using System.IO;
using System.Linq;

namespace OpenProject.Bcf
{
  public class BcfFileLoader
  {
    private readonly string _bcfFileName;
    private readonly Stream _bcfFileStream;

    public BcfFileLoader(string bcfFileName, Stream bcfFileStream)
    {
      _bcfFileName = bcfFileName ?? throw new ArgumentNullException(nameof(bcfFileName));
      _bcfFileStream = bcfFileStream ?? throw new ArgumentNullException(nameof(bcfFileStream));
    }

    public BcfFileViewModel LoadFromBcf()
    {
      try
      {
        if (FileExtensionIsBcfV21())
        {
          return LoadFromBcfV21();
        }
        else
        {
          return LoadFromBcfV20();
        }
      }
      catch (Exception bcfLoadException)
      {
        throw new BcfLoaderException("Failed to load the file as BCF. See the inner exception for more details.",
          bcfLoadException);
      }
    }

    private BcfFileViewModel LoadFromBcfV20()
    {
      var bcfContainer = iabi.BCF.BCFv2.BCFv2Container.ReadStream(_bcfFileStream);

      var bcfFile = new BcfFileViewModel
      {
        BcfVersion = BcfVersion.V20,
        FileName = Path.GetFileName(_bcfFileName),
        FullName = _bcfFileName,
        ProjectId = bcfContainer.BcfProject?.Project?.ProjectId,
        ProjectName = bcfContainer.BcfProject?.Project?.Name
      };

      foreach (var bcfIssueViewModel in new BcfV2Loader(bcfContainer).GetIssuesForBcfV20Topics())
      {
        bcfFile.BcfIssues.Add(bcfIssueViewModel);
      }

      bcfFile.SelectedBcfIssue = bcfFile.BcfIssuesFiltered.FirstOrDefault();
      bcfFile.IsModified = false;
      return bcfFile;
    }

    private BcfFileViewModel LoadFromBcfV21()
    {
      var bcfContainer = iabi.BCF.BCFv21.BCFv21Container.ReadStream(_bcfFileStream);

      var bcfFile = new BcfFileViewModel
      {
        BcfVersion = BcfVersion.V20,
        FileName = Path.GetFileName(_bcfFileName),
        FullName = _bcfFileName,
        ProjectId = bcfContainer.BcfProject?.Project?.ProjectId,
        ProjectName = bcfContainer.BcfProject?.Project?.Name
      };

      foreach (var bcfIssueViewModel in new BcfV21Loader(bcfContainer).GetIssuesForBcfV20Topics())
      {
        bcfFile.BcfIssues.Add(bcfIssueViewModel);
      }

      bcfFile.SelectedBcfIssue = bcfFile.BcfIssuesFiltered.FirstOrDefault();
      bcfFile.IsModified = false;
      return bcfFile;
    }

    public bool FileExtensionIsBcfV21()
    {
      return _bcfFileName.EndsWith(".bcf", StringComparison.InvariantCultureIgnoreCase);
    }
  }
}
