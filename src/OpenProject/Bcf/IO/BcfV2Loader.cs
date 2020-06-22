using iabi.BCF.BCFv2;
using iabi.BCF.BCFv2.Schemas;
using OpenProject.Shared.ViewModels.Bcf;
using System;
using System.Collections.Generic;

namespace OpenProject.Bcf
{
  internal class BcfV2Loader
  {
    private readonly BCFv2Container _bcfContainer;

    public BcfV2Loader(BCFv2Container bcfContainer)
    {
      _bcfContainer = bcfContainer;
    }

    public IEnumerable<BcfIssueViewModel> GetIssuesForBcfV20Topics()
    {
      foreach (var bcfTopic in _bcfContainer.Topics)
      {
        var bcfIssue = new BcfIssueViewModel
        {
          DisableListeningForChanges = true
        };
        bcfIssue.Markup = GetMarkupViewModel(bcfTopic);

        foreach (var bcfViewpoint in bcfTopic.Viewpoints)
        {
          bcfIssue.Viewpoints.Add(GetViewpointViewModel(bcfTopic, bcfViewpoint));
        }

        bcfIssue.DisableListeningForChanges = false;
        yield return bcfIssue;
      }
    }

    private BcfMarkupViewModel GetMarkupViewModel(BCFTopic bcfTopic)
    {
      var markup = new BcfMarkupViewModel();

      if (bcfTopic.Markup != null)
      {
        var bcfMarkup = bcfTopic.Markup;

        if (bcfMarkup.ShouldSerializeViewpoints())
        {
          foreach (var bcfViewpointReference in bcfMarkup.Viewpoints)
          {
            var viewpointReference = new BcfMarkupViewpointReferenceViewModel();

            if (Guid.TryParse(bcfViewpointReference.Guid, out var parsedViewpointReferenceId))
            {
              viewpointReference.Id = parsedViewpointReferenceId;
            }

            if (bcfTopic.ViewpointSnapshots.ContainsKey(bcfViewpointReference.Guid))
            {
              viewpointReference.Snapshot = bcfTopic.ViewpointSnapshots[bcfViewpointReference.Guid];
            }

            markup.ViewpointReferences.Add(viewpointReference);
          }
        }

        if (bcfMarkup.ShouldSerializeTopic())
        {
          markup.BcfTopic = GetBcfTopicViewmodel(bcfMarkup.Topic);
        }
      }

      if (bcfTopic.Markup?.Header != null)
      {
        foreach (var bcfHeader in bcfTopic.Markup.Header)
        {
          var headerFile = new BcfHeaderFileViewModel
          {
            FileName = bcfHeader.Filename,
            IfcProject = bcfHeader.IfcProject,
            IfcSpatialStructureElement = bcfHeader.IfcSpatialStructureElement,
            IsExternal = bcfHeader.isExternal,
            Reference = bcfHeader.Reference,
          };

          if (bcfHeader.DateSpecified)
          {
            headerFile.FileDate = bcfHeader.Date;
          }

          markup.HeaderFiles.Add(headerFile);
        }
      }

      if (bcfTopic.Markup?.Comment != null)
      {
        foreach (var bcfComment in bcfTopic.Markup.Comment)
        {
          var comment = new BcfCommentviewModel
          {
            Author = bcfComment.Author,
            CreationDate = bcfComment.Date,
            ModifiedAuthor = bcfComment.ModifiedAuthor,
            Status = bcfComment.Status,
            Text = bcfComment.Comment1
          };

          if (bcfComment.Viewpoint?.Guid != null
            && Guid.TryParse(bcfComment.Viewpoint.Guid, out var parsedViewpointId))
          {
            comment.ViewpointId = parsedViewpointId;
          }

          if (Guid.TryParse(bcfComment.Guid, out var parsedId))
          {
            comment.Id = parsedId;
          }

          if (bcfComment.ModifiedDateSpecified)
          {
            comment.ModifiedDate = bcfComment.ModifiedDate;
          }

          markup.Comments.Add(comment);
        }
      }

      return markup;
    }

    private BcfTopicViewModel GetBcfTopicViewmodel(Topic bcfTopic)
    {
      var topic = new BcfTopicViewModel
      {
        AssignedTo = bcfTopic.AssignedTo,
        Author = bcfTopic.CreationAuthor,
        CreationDate = bcfTopic.CreationDate,
        Description = bcfTopic.Description,
        ModifiedAuthor = bcfTopic.ModifiedAuthor,
        Priority = bcfTopic.Priority,
        Status = bcfTopic.TopicStatus,
        Title = bcfTopic.Title,
        Type = bcfTopic.TopicType
      };

      if (bcfTopic.ShouldSerializeModifiedDate()
        && bcfTopic.ModifiedDate != DateTime.MinValue)
      {
        topic.ModifiedDate = bcfTopic.ModifiedDate;
      }

      if (Guid.TryParse(bcfTopic.Guid, out var parsedId))
      {
        topic.Id = parsedId;
      }

      if (bcfTopic.ShouldSerializeLabels())
      {
        foreach (var label in bcfTopic.Labels)
        {
          topic.Labels.Add(label);
        }
      }

      return topic;
    }

    private BcfViewpointViewModel GetViewpointViewModel(BCFTopic bcfTopic, VisualizationInfo bcfViewpoint)
    {
      var viewpoint = new BcfViewpointViewModel();

      if (Guid.TryParse(bcfViewpoint.GUID, out var parsedId))
      {
        viewpoint.Id = parsedId;
      }

      if (bcfViewpoint.Lines != null)
      {
        foreach (var bcfLine in bcfViewpoint.Lines)
        {
          viewpoint.Lines.Add(new BcfViewpointLineViewModel
          {
            Start = new BcfPointOrVectorViewModel
            {
              X = bcfLine.StartPoint.X,
              Y = bcfLine.StartPoint.Y,
              Z = bcfLine.StartPoint.Z
            },
            End = new BcfPointOrVectorViewModel
            {
              X = bcfLine.EndPoint.X,
              Y = bcfLine.EndPoint.Y,
              Z = bcfLine.EndPoint.Z
            }
          });
        }
      }

      if (bcfViewpoint.ClippingPlanes != null)
      {
        foreach (var bcfClippingPlane in bcfViewpoint.ClippingPlanes)
        {
          viewpoint.ClippingPlanes.Add(new BcfViewpointClippingPlaneViewModel
          {
            Direction = new BcfPointOrVectorViewModel
            {
              X = bcfClippingPlane.Direction.X,
              Y = bcfClippingPlane.Direction.Y,
              Z = bcfClippingPlane.Direction.Z
            },
            Location = new BcfPointOrVectorViewModel
            {
              X = bcfClippingPlane.Location.X,
              Y = bcfClippingPlane.Location.Y,
              Z = bcfClippingPlane.Location.Z
            }
          });
        }
      }

      if (bcfViewpoint.PerspectiveCamera != null)
      {
        viewpoint.PerspectiveCamera = new BcfViewpointPerspectiveCameraViewModel
        {
          DirectionX = bcfViewpoint.PerspectiveCamera.CameraDirection.X,
          DirectionY = bcfViewpoint.PerspectiveCamera.CameraDirection.Y,
          DirectionZ = bcfViewpoint.PerspectiveCamera.CameraDirection.Z,
          UpX = bcfViewpoint.PerspectiveCamera.CameraUpVector.X,
          UpY = bcfViewpoint.PerspectiveCamera.CameraUpVector.Y,
          UpZ = bcfViewpoint.PerspectiveCamera.CameraUpVector.Z,
          ViewPointX = bcfViewpoint.PerspectiveCamera.CameraViewPoint.X,
          ViewPointY = bcfViewpoint.PerspectiveCamera.CameraViewPoint.Y,
          ViewPointZ = bcfViewpoint.PerspectiveCamera.CameraViewPoint.Z,
          FieldOfView = bcfViewpoint.PerspectiveCamera.FieldOfView
        };
      }
      else if (bcfViewpoint.OrthogonalCamera != null)
      {
        viewpoint.OrthogonalCamera = new BcfViewpointOrthogonalCameraViewModel
        {
          DirectionX = bcfViewpoint.OrthogonalCamera.CameraDirection.X,
          DirectionY = bcfViewpoint.OrthogonalCamera.CameraDirection.Y,
          DirectionZ = bcfViewpoint.OrthogonalCamera.CameraDirection.Z,
          UpX = bcfViewpoint.OrthogonalCamera.CameraUpVector.X,
          UpY = bcfViewpoint.OrthogonalCamera.CameraUpVector.Y,
          UpZ = bcfViewpoint.OrthogonalCamera.CameraUpVector.Z,
          ViewPointX = bcfViewpoint.OrthogonalCamera.CameraViewPoint.X,
          ViewPointY = bcfViewpoint.OrthogonalCamera.CameraViewPoint.Y,
          ViewPointZ = bcfViewpoint.OrthogonalCamera.CameraViewPoint.Z,
          ViewToWorldScale = bcfViewpoint.OrthogonalCamera.ViewToWorldScale
        };
      }

      if (bcfViewpoint.Components != null)
      {
        foreach (var bcfComponent in bcfViewpoint.Components)
        {
          viewpoint.Components.Add(new BcfViewpointComponentViewModel
          {
            AuthoringToolId = bcfComponent.AuthoringToolId,
            Color = bcfComponent.Color,
            IfcGuid = bcfComponent.IfcGuid,
            IsSelected = bcfComponent.Selected,
            IsVisible = bcfComponent.Visible,
            OriginatingSystem = bcfComponent.OriginatingSystem
          });
        }
      }

      if (bcfTopic.ViewpointSnapshots.ContainsKey(bcfViewpoint.GUID))
      {
        viewpoint.Snapshot = bcfTopic.ViewpointSnapshots[bcfViewpoint.GUID];
      }

      return viewpoint;
    }
  }
}
