using iabi.BCF.BCFv2;
using OpenProject.Shared.ViewModels.Bcf;
using System.IO;
using System.Linq;

namespace OpenProject.Bcf
{
  public class BcfFileConverter
  {
    private readonly BcfFileViewModel _bcfFileViewModel;

    public BcfFileConverter(BcfFileViewModel bcfFileViewModel)
    {
      _bcfFileViewModel = bcfFileViewModel;
    }

    public Stream GetBcfFileStream(BcfVersion? bcfVersion = null)
    {
      var bcfContainer = GetContainer();

      var memoryStream = new MemoryStream();

      bcfVersion = bcfVersion ?? _bcfFileViewModel.BcfVersion;

      if (bcfVersion == BcfVersion.V21)
      {
        var converter = new iabi.BCF.Converter.V2ToV21(bcfContainer);
        var bcfv21Container = converter.Convert();
        bcfv21Container.WriteStream(memoryStream);
      }
      else
      {
        bcfContainer.WriteStream(memoryStream);
      }

      memoryStream.Position = 0;
      return memoryStream;
    }

    private BCFv2Container GetContainer()
    {
      var bcfContainer = new BCFv2Container();

      bcfContainer.BcfProject = new iabi.BCF.BCFv2.Schemas.ProjectExtension
      {
        Project = new iabi.BCF.BCFv2.Schemas.Project
        {
          Name = _bcfFileViewModel.ProjectName,
          ProjectId = _bcfFileViewModel.ProjectId
        }
      };

      foreach (var issue in _bcfFileViewModel.BcfIssues)
      {
        var bcfTopic = GetTopic(issue);
        bcfContainer.Topics.Add(bcfTopic);
      }

      return bcfContainer;
    }

    private BCFTopic GetTopic(BcfIssueViewModel bcfIssue)
    {
      var bcfTopic = new BCFTopic();
      AddViewpoints(bcfTopic, bcfIssue);

      if (bcfIssue.Markup != null)
      {
        bcfTopic.Markup = new iabi.BCF.BCFv2.Schemas.Markup();

        foreach (var comment in bcfIssue.Markup.Comments)
        {
          var bcfComment = new iabi.BCF.BCFv2.Schemas.Comment
          {
            Author = comment.Author,
            Comment1 = comment.Text,
            Date = comment.CreationDate,
            Guid = comment.Id.ToString(),
            Status = comment.Status
          };
          if (comment.ModifiedDate != null && comment.ModifiedDate.Value != default)
          {
            bcfComment.ModifiedDate = comment.ModifiedDate.Value;
            bcfComment.ModifiedAuthor = comment.ModifiedAuthor;
          }
          if (comment.ViewpointId != null)
          {
            bcfComment.Viewpoint = new iabi.BCF.BCFv2.Schemas.CommentViewpoint
            {
              Guid = comment.ViewpointId.ToString()
            };
          }
          bcfTopic.Markup.Comment.Add(bcfComment);
        }

        foreach (var headerFile in bcfIssue.Markup.HeaderFiles)
        {
          var bcfHeaderFile = new iabi.BCF.BCFv2.Schemas.HeaderFile
          {
            Filename = headerFile.FileName,
            IfcProject = headerFile.IfcProject,
            IfcSpatialStructureElement = headerFile.IfcSpatialStructureElement,
            isExternal = headerFile.IsExternal,
            Reference = headerFile.Reference
          };
          if (headerFile.FileDate != null && headerFile.FileDate.Value != default)
          {
            bcfHeaderFile.Date = headerFile.FileDate.Value;
          }
          bcfTopic.Markup.Header.Add(bcfHeaderFile);
        }

        bcfTopic.Markup.Topic = new iabi.BCF.BCFv2.Schemas.Topic
        {
          AssignedTo = bcfIssue.Markup.BcfTopic.AssignedTo,
          CreationAuthor = bcfIssue.Markup.BcfTopic.Author,
          CreationDate = bcfIssue.Markup.BcfTopic.CreationDate,
          Description = bcfIssue.Markup.BcfTopic.Description,
          Guid = bcfIssue.Markup.BcfTopic.Id.ToString(),
          Priority = bcfIssue.Markup.BcfTopic.Priority,
          Title = bcfIssue.Markup.BcfTopic.Title,
          TopicStatus = bcfIssue.Markup.BcfTopic.Status,
          TopicType = bcfIssue.Markup.BcfTopic.Type
        };

        if (bcfIssue.Markup.BcfTopic.ModifiedDate != null)
        {
          bcfTopic.Markup.Topic.ModifiedDate = bcfIssue.Markup.BcfTopic.ModifiedDate.Value;
          bcfTopic.Markup.Topic.ModifiedAuthor = bcfIssue.Markup.BcfTopic.ModifiedAuthor;
        }

        if (bcfIssue.Markup.BcfTopic.Labels.Any())
        {
          bcfTopic.Markup.Topic.Labels.AddRange(bcfIssue.Markup.BcfTopic.Labels);
        }
      }

      return bcfTopic;
    }

    private void AddViewpoints(BCFTopic bcfTopic, BcfIssueViewModel bcfIssue)
    {
      foreach (var viewpoint in bcfIssue.Viewpoints)
      {
        var bcfViewpoint = new iabi.BCF.BCFv2.Schemas.VisualizationInfo();
        bcfViewpoint.GUID = viewpoint.Id.ToString();
        bcfTopic.Viewpoints.Add(bcfViewpoint);

        var bcfLines = viewpoint.Lines
          .Select(l => new iabi.BCF.BCFv2.Schemas.Line
          {
            StartPoint = new iabi.BCF.BCFv2.Schemas.Point
            {
              X = l.Start.X,
              Y = l.Start.Y,
              Z = l.Start.Z
            },
            EndPoint = new iabi.BCF.BCFv2.Schemas.Point
            {
              X = l.End.X,
              Y = l.End.Y,
              Z = l.End.Z
            }
          });
        foreach (var bcfLine in bcfLines)
        {
          bcfViewpoint.Lines.Add(bcfLine);
        }

        var bcfClippingPlanes = viewpoint.ClippingPlanes
          .Select(cp => new iabi.BCF.BCFv2.Schemas.ClippingPlane
          {
            Direction = new iabi.BCF.BCFv2.Schemas.Direction
            {
              X = cp.Direction.X,
              Y = cp.Direction.Y,
              Z = cp.Direction.Z
            },
            Location = new iabi.BCF.BCFv2.Schemas.Point
            {
              X = cp.Location.X,
              Y = cp.Location.Y,
              Z = cp.Location.Z
            }
          });
        foreach (var bcfClippingPlane in bcfClippingPlanes)
        {
          bcfViewpoint.ClippingPlanes.Add(bcfClippingPlane);
        }

        var bcfComponents = viewpoint.Components
          .Select(c => new iabi.BCF.BCFv2.Schemas.Component
          {
            AuthoringToolId = c.AuthoringToolId,
            Color = c.Color,
            IfcGuid = c.IfcGuid,
            OriginatingSystem = c.OriginatingSystem,
            Selected = c.IsSelected,
            Visible = c.IsVisible
          });
        foreach (var bcfComponent in bcfComponents)
        {
          bcfViewpoint.Components.Add(bcfComponent);
        }

        if (viewpoint.OrthogonalCamera != null)
        {
          bcfViewpoint.OrthogonalCamera = new iabi.BCF.BCFv2.Schemas.OrthogonalCamera
          {
            CameraDirection = new iabi.BCF.BCFv2.Schemas.Direction
            {
              X = viewpoint.OrthogonalCamera.DirectionX,
              Y = viewpoint.OrthogonalCamera.DirectionY,
              Z = viewpoint.OrthogonalCamera.DirectionZ
            },
            CameraUpVector = new iabi.BCF.BCFv2.Schemas.Direction
            {
              X = viewpoint.OrthogonalCamera.UpX,
              Y = viewpoint.OrthogonalCamera.UpY,
              Z = viewpoint.OrthogonalCamera.UpZ
            },
            CameraViewPoint = new iabi.BCF.BCFv2.Schemas.Point
            {
              X = viewpoint.OrthogonalCamera.ViewPointX,
              Y = viewpoint.OrthogonalCamera.ViewPointY,
              Z = viewpoint.OrthogonalCamera.ViewPointZ
            },
            ViewToWorldScale = viewpoint.OrthogonalCamera.ViewToWorldScale
          };
        }
        else if (viewpoint.PerspectiveCamera != null)
        {
          bcfViewpoint.PerspectiveCamera = new iabi.BCF.BCFv2.Schemas.PerspectiveCamera
          {
            CameraDirection = new iabi.BCF.BCFv2.Schemas.Direction
            {
              X = viewpoint.PerspectiveCamera.DirectionX,
              Y = viewpoint.PerspectiveCamera.DirectionY,
              Z = viewpoint.PerspectiveCamera.DirectionZ
            },
            CameraUpVector = new iabi.BCF.BCFv2.Schemas.Direction
            {
              X = viewpoint.PerspectiveCamera.UpX,
              Y = viewpoint.PerspectiveCamera.UpY,
              Z = viewpoint.PerspectiveCamera.UpZ
            },
            CameraViewPoint = new iabi.BCF.BCFv2.Schemas.Point
            {
              X = viewpoint.PerspectiveCamera.ViewPointX,
              Y = viewpoint.PerspectiveCamera.ViewPointY,
              Z = viewpoint.PerspectiveCamera.ViewPointZ
            },
            FieldOfView = viewpoint.PerspectiveCamera.FieldOfView
          };
        }

        if (viewpoint.Snapshot != null)
        {
          bcfTopic.AddOrUpdateSnapshot(bcfViewpoint.GUID, viewpoint.Snapshot);
        }
      }
    }
  }
}
