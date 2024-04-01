using Dangl.BCF.BCFv3.Schemas;
using IPA.Bcfier.Models.Bcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IPA.Bcfier.Services
{
    public class BcfConversionFromModelService
    {
        public Dangl.BCF.BCFv3.BCFv3Container ConvertBcfFileToBcfContainer(BcfFile bcfFile)
        {
            var bcfContainer = new Dangl.BCF.BCFv3.BCFv3Container();
            bcfContainer.BcfProject = new Project
            {
                Name = bcfFile.Project.Name,
                ProjectId = bcfFile.Project.Id
            };

            foreach (var fileAttachment in bcfFile.FileAttachments)
            {
                if (!bcfContainer.FileAttachments.ContainsKey(fileAttachment.Name))
                {
                    bcfContainer.FileAttachments.Add(fileAttachment.Name, Convert.FromBase64String(fileAttachment.Base64Data));
                }
            }

            bcfContainer.ProjectExtensions = new Extensions
            {
                Priorities = bcfFile.ProjectExtensions.Priorities,
                SnippetTypes = bcfFile.ProjectExtensions.SnippetTypes,
                TopicLabels = bcfFile.ProjectExtensions.TopicLabels,
                TopicStatuses = bcfFile.ProjectExtensions.TopicStatuses,
                TopicTypes = bcfFile.ProjectExtensions.TopicTypes,
                Users = bcfFile.ProjectExtensions.Users
            };

            WriteTopics(bcfFile, bcfContainer);

            return bcfContainer;
        }

        private void WriteTopics(BcfFile bcfFile, Dangl.BCF.BCFv3.BCFv3Container bcfContainer)
        {
            foreach (var topic in bcfFile.Topics)
            {
                var bcfTopic = new Dangl.BCF.BCFv3.BCFTopic
                {
                    Markup = new Markup()
                };
                bcfContainer.Topics.Add(bcfTopic);

                foreach (var file in topic.Files)
                {
                    var bcfHeaderFile = new File
                    {
                        Filename = file.FileName,
                        IfcProject = file.IfcProjectId,
                        IfcSpatialStructureElement = file.IfcSpatialStructureElementId,
                        Reference = file.ReferenceLink,
                    };

                    if (file.Date != null)
                    {
                        bcfHeaderFile.Date = file.Date.Value;
                    }

                    bcfTopic.Markup.Header.Files.Add(bcfHeaderFile);
                }

                bcfTopic.Markup.Topic.AssignedTo = topic.AssignedTo;
                bcfTopic.Markup.Topic.CreationAuthor = topic.CreationAuthor;
                if (topic.CreationDate != null)
                {
                    bcfTopic.Markup.Topic.CreationDate = topic.CreationDate.Value;
                }
                bcfTopic.Markup.Topic.Description = topic.Description;
                bcfTopic.Markup.Topic.ModifiedAuthor = topic.ModifiedAuthor;
                if (topic.ModifiedDate != null)
                {
                    bcfTopic.Markup.Topic.ModifiedDate = topic.ModifiedDate.Value;
                }
                bcfTopic.Markup.Topic.ServerAssignedId = topic.ServerAssignedId;
                bcfTopic.Markup.Topic.TopicStatus = topic.TopicStatus;
                bcfTopic.Markup.Topic.Title = topic.Title;
                bcfTopic.Markup.Topic.Stage = topic.Stage;
                bcfTopic.Markup.Topic.Priority = topic.Priority;
                bcfTopic.Markup.Topic.TopicType = topic.TopicType;
                if (topic.DueDate != null)
                {
                    bcfTopic.Markup.Topic.DueDate = topic.DueDate.Value;
                }

                bcfTopic.Markup.Topic.Guid = topic.Id.ToString();
                bcfTopic.Markup.Topic.Index = topic.Index;
                bcfTopic.Markup.Topic.Labels = topic.Labels.ToList();


                bcfTopic.Markup.Topic.RelatedTopics = topic.RelatedTopicIds.Select(t => new TopicRelatedTopic
                {
                    Guid = t.ToString()
                }).ToList();

                bcfTopic.Markup.Topic.ReferenceLinks = topic.ReferenceLinks.ToList();

                foreach (var documentReference in topic.DocumentReferences)
                {
                    var bcfDocumentReference = new DocumentReference
                    {
                        Guid = documentReference.Id.ToString(),
                        ItemElementName = string.IsNullOrWhiteSpace(documentReference.Url)
                            ? ItemChoiceType.DocumentGuid
                            : ItemChoiceType.Url,
                        Item = string.IsNullOrWhiteSpace(documentReference.Url)
                        ? documentReference.DocumentId
                            : documentReference.Url,
                        Description = documentReference.Description
                    };
                    bcfTopic.Markup.Topic.DocumentReferences.Add(bcfDocumentReference);
                }

                foreach (var comment in topic.Comments)
                {
                    var bcfComment = new Comment
                    {
                        Guid = comment.Id.ToString(),
                        Comment1 = comment.Text,
                        Author = comment.Author,
                    };

                    if (comment.CreationDate != null)
                    {
                        bcfComment.Date = comment.CreationDate.Value;
                    }

                    if (comment.ModifiedDate != null)
                    {
                        bcfComment.ModifiedDate = comment.ModifiedDate.Value;
                    }

                    if (comment.ViewpointId != null)
                    {
                        bcfComment.Viewpoint = new CommentViewpoint
                        {
                            Guid = comment.ViewpointId.ToString()
                        };
                    }

                    bcfTopic.Markup.Topic.Comments.Add(bcfComment);
                }

                WriteViewpoints(bcfTopic, topic);
            }
        }

        private void WriteViewpoints(Dangl.BCF.BCFv3.BCFTopic bcfTopic, BcfTopic topic)
        {
            foreach (var viewpoint in topic.Viewpoints)
            {
                // We're not doing anything with the bitmaps
                var bcfViewpoint = new VisualizationInfo
                {
                    Guid = viewpoint.Id.ToString()
                };
                bcfTopic.Viewpoints.Add(bcfViewpoint);

                foreach (var coloring in viewpoint.ViewpointComponents.Coloring)
                {
                    bcfViewpoint.Components.Coloring.Add(new ComponentColoringColor
                    {
                        Color = coloring.Color,
                        Components = coloring.Components.Select(c => new Component
                        {
                            AuthoringToolId = c.AuthoringToolId,
                            IfcGuid = c.IfcGuid,
                            OriginatingSystem = c.OriginatingSystem
                        }).ToList()
                    });
                }

                foreach (var selection in viewpoint.ViewpointComponents.SelectedComponents)
                {
                    bcfViewpoint.Components.Selection.Add(new Component
                    {
                        AuthoringToolId = selection.AuthoringToolId,
                        IfcGuid = selection.IfcGuid,
                        OriginatingSystem = selection.OriginatingSystem
                    });
                }

                bcfViewpoint.Components.Visibility = new ComponentVisibility
                {
                    DefaultVisibility = viewpoint.ViewpointComponents.Visibility.DefaultVisibility,
                    Exceptions = viewpoint.ViewpointComponents.Visibility.Exceptions.Select(e => new Component
                    {
                        AuthoringToolId = e.AuthoringToolId,
                        IfcGuid = e.IfcGuid,
                        OriginatingSystem = e.OriginatingSystem
                    }).ToList()
                };

                if (viewpoint.OrthogonalCamera != null)
                {
                    bcfViewpoint.Item = new OrthogonalCamera
                    {
                        AspectRatio = viewpoint.OrthogonalCamera.AspectRatio,
                        ViewToWorldScale = viewpoint.OrthogonalCamera.ViewToWorldScale,
                        CameraUpVector = new Direction
                        {
                            X = viewpoint.OrthogonalCamera.UpVector.X,
                            Y = viewpoint.OrthogonalCamera.UpVector.Y,
                            Z = viewpoint.OrthogonalCamera.UpVector.Z
                        },
                        CameraViewPoint = new Point
                        {
                            X = viewpoint.OrthogonalCamera.ViewPoint.X,
                            Y = viewpoint.OrthogonalCamera.ViewPoint.Y,
                            Z = viewpoint.OrthogonalCamera.ViewPoint.Z
                        },
                        CameraDirection = new Direction
                        {
                            X = viewpoint.OrthogonalCamera.Direction.X,
                            Y = viewpoint.OrthogonalCamera.Direction.Y,
                            Z = viewpoint.OrthogonalCamera.Direction.Z
                        }
                    };
                }
                else if (viewpoint.PerspectiveCamera != null)
                {
                    bcfViewpoint.Item = new PerspectiveCamera
                    {
                        AspectRatio = viewpoint.PerspectiveCamera.AspectRatio,
                        FieldOfView = viewpoint.PerspectiveCamera.FieldOfView,
                        CameraUpVector = new Direction
                        {
                            X = viewpoint.PerspectiveCamera.UpVector.X,
                            Y = viewpoint.PerspectiveCamera.UpVector.Y,
                            Z = viewpoint.PerspectiveCamera.UpVector.Z
                        },
                        CameraViewPoint = new Point
                        {
                            X = viewpoint.PerspectiveCamera.ViewPoint.X,
                            Y = viewpoint.PerspectiveCamera.ViewPoint.Y,
                            Z = viewpoint.PerspectiveCamera.ViewPoint.Z
                        },
                        CameraDirection = new Direction
                        {
                            X = viewpoint.PerspectiveCamera.Direction.X,
                            Y = viewpoint.PerspectiveCamera.Direction.Y,
                            Z = viewpoint.PerspectiveCamera.Direction.Z
                        }
                    };
                }

                foreach (var line in viewpoint.Lines)
                {
                    var bcfViewpointLine = new Line
                    {
                        StartPoint = new Point
                        {
                            X = line.StartPoint.X,
                            Y = line.StartPoint.Y,
                            Z = line.StartPoint.Z
                        },
                        EndPoint = new Point
                        {
                            X = line.EndPoint.X,
                            Y = line.EndPoint.Y,
                            Z = line.EndPoint.Z
                        }
                    };
                    bcfViewpoint.Lines.Add(bcfViewpointLine);
                }

                foreach (var clippingPlane in viewpoint.ClippingPlanes)
                {
                    var bcfViewpointClippingPlane = new ClippingPlane
                    {
                        Location = new Point
                        {
                            X = clippingPlane.Location.X,
                            Y = clippingPlane.Location.Y,
                            Z = clippingPlane.Location.Z
                        },
                        Direction = new Direction
                        {
                            X = clippingPlane.Direction.X,
                            Y = clippingPlane.Direction.Y,
                            Z = clippingPlane.Direction.Z
                        }
                    };
                    bcfViewpoint.ClippingPlanes.Add(bcfViewpointClippingPlane);
                }

                if (!string.IsNullOrWhiteSpace(viewpoint.SnapshotBase64))
                {
                    bcfTopic.AddOrUpdateSnapshot(bcfViewpoint.Guid, Convert.FromBase64String(viewpoint.SnapshotBase64));
                }
            }
        }
    }
}
