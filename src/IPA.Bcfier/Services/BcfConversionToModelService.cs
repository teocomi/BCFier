using IPA.Bcfier.Models.Bcf;
using System;
using System.Linq;

namespace IPA.Bcfier.Services
{
    public class BcfConversionToModelService
    {
        public BcfFile ConvertBcfContainerToBcfFile(Dangl.BCF.BCFv3.BCFv3Container bcfContainer, string fileName)
        {
            var bcfFile = new BcfFile
            {
                FileName = fileName
            };

            bcfFile.Project = new BcfProject
            {
                Name = bcfContainer.BcfProject?.Name ?? string.Empty,
                Id = bcfContainer.BcfProject?.ProjectId ?? string.Empty
            };

            if (bcfContainer.FileAttachments != null)
            {
                foreach (var fileAttachment in bcfContainer.FileAttachments)
                {
                    var bcfFileAttachment = new BcfFileAttachment
                    {
                        Name = fileAttachment.Key,
                        Base64Data = Convert.ToBase64String(fileAttachment.Value)
                    };
                    bcfFile.FileAttachments.Add(bcfFileAttachment);
                }
            }

            if (bcfContainer.ProjectExtensions != null)
            {
                bcfFile.ProjectExtensions.TopicLabels = bcfContainer.ProjectExtensions.TopicLabels;
                bcfFile.ProjectExtensions.TopicStatuses = bcfContainer.ProjectExtensions.TopicStatuses;
                bcfFile.ProjectExtensions.Users = bcfContainer.ProjectExtensions.Users;
                bcfFile.ProjectExtensions.SnippetTypes = bcfContainer.ProjectExtensions.SnippetTypes;
                bcfFile.ProjectExtensions.TopicTypes = bcfContainer.ProjectExtensions.TopicTypes;
                bcfFile.ProjectExtensions.Priorities = bcfContainer.ProjectExtensions.Priorities;
            }

            ReadTopics(bcfContainer, bcfFile);

            return bcfFile;
        }

        private void ReadTopics(Dangl.BCF.BCFv3.BCFv3Container bcfContainer, BcfFile bcfFile)
        {
            if (bcfContainer.Topics != null)
            {
                foreach (var topic in bcfContainer.Topics)
                {
                    var bcfTopic = new BcfTopic();
                    bcfFile.Topics.Add(bcfTopic);

                    if (topic.Markup.Header.Files != null)
                    {
                        foreach (var file in topic.Markup.Header.Files)
                        {
                            var bcfTopicFile = new BcfTopicFile
                            {
                                Date = file.ShouldSerializeDate() && file.DateSpecified ? file.Date : null,
                                FileName = file.Filename,
                                IfcProjectId = file.IfcProject,
                                IfcSpatialStructureElementId = file.IfcSpatialStructureElement,
                                ReferenceLink = file.Reference
                            };
                            bcfTopic.Files.Add(bcfTopicFile);
                        }
                    }

                    if (topic.Viewpoints != null)
                    {
                        ReadViewpoints(bcfTopic, topic);
                    }

                    if (topic.Markup.Topic != null)
                    {
                        bcfTopic.AssignedTo = topic.Markup.Topic.AssignedTo;
                        bcfTopic.CreationAuthor = topic.Markup.Topic.CreationAuthor;
                        if (topic.Markup.Topic.ShouldSerializeCreationDate())
                        {
                            bcfTopic.CreationDate = topic.Markup.Topic.CreationDate;
                        }
                        bcfTopic.Description = topic.Markup.Topic.Description;
                        bcfTopic.ModifiedAuthor = topic.Markup.Topic.ModifiedAuthor;
                        if (topic.Markup.Topic.ModifiedDateSpecified)
                        {
                            bcfTopic.ModifiedDate = topic.Markup.Topic.ModifiedDate;
                        }
                        bcfTopic.ServerAssignedId = topic.Markup.Topic.ServerAssignedId;
                        bcfTopic.TopicStatus = topic.Markup.Topic.TopicStatus;
                        bcfTopic.Title = topic.Markup.Topic.Title;
                        bcfTopic.Stage = topic.Markup.Topic.Stage;
                        bcfTopic.Priority = topic.Markup.Topic.Priority;
                        bcfTopic.TopicType = topic.Markup.Topic.TopicType;

                        if (topic.Markup.Topic.ShouldSerializeDueDate() && topic.Markup.Topic.DueDateSpecified)
                        {
                            bcfTopic.DueDate = topic.Markup.Topic.DueDate;
                        }

                        if (Guid.TryParse(topic.Markup.Topic.Guid, out var topicId))
                        {
                            bcfTopic.Id = topicId;
                        }

                        bcfTopic.Index = topic.Markup.Topic.Index;

                        if (topic.Markup.Topic.Labels != null)
                        {
                            bcfTopic.Labels = topic.Markup.Topic.Labels.ToList();
                        }

                        if (topic.Markup.Topic.RelatedTopics != null)
                        {
                            bcfTopic.RelatedTopicIds = topic.Markup.Topic.RelatedTopics
                                .Where(r => Guid.TryParse(r.Guid, out var _))
                                .Select(r => Guid.Parse(r.Guid))
                                .ToList();
                        }

                        if (topic.Markup.Topic.ReferenceLinks != null)
                        {
                            bcfTopic.ReferenceLinks = topic.Markup.Topic.ReferenceLinks.ToList();
                        }

                        if (topic.Markup.Topic.DocumentReferences != null)
                        {
                            foreach (var documentReference in topic.Markup.Topic.DocumentReferences)
                            {
                                var bcfDocumentReference = new BcfDocumentReference
                                {
                                    Id = Guid.Parse(documentReference.Guid),
                                    Url = documentReference.ItemElementName == Dangl.BCF.BCFv3.Schemas.ItemChoiceType.Url
                                        ? documentReference.Item
                                        : string.Empty,
                                    DocumentId = documentReference.ItemElementName == Dangl.BCF.BCFv3.Schemas.ItemChoiceType.DocumentGuid
                                        ? documentReference.Item
                                        : string.Empty,
                                    Description = documentReference.Description
                                };
                                bcfTopic.DocumentReferences.Add(bcfDocumentReference);
                            }
                        }

                        if (topic.Markup.Topic.Comments != null)
                        {
                            foreach (var comment in topic.Markup.Topic.Comments)
                            {
                                var bcfComment = new BcfComment
                                {
                                    Id = Guid.Parse(comment.Guid),
                                    Text = comment.Comment1,
                                    Author = comment.Author,
                                    CreationDate = comment.Date,
                                    ModifiedBy = comment.ModifiedAuthor,
                                    ModifiedDate = comment.ModifiedDate,
                                    ViewpointId = Guid.TryParse(comment.Viewpoint?.Guid, out var viewpointId)
                                        ? viewpointId
                                        : null
                                };
                                bcfTopic.Comments.Add(bcfComment);

                            }
                        }
                    }

                    // We're not doing anything with the BIM Snippet
                }
            }

        }

        private void ReadViewpoints(BcfTopic bcfTopic, Dangl.BCF.BCFv3.BCFTopic topic)
        {
            foreach (var viewpoint in topic.Viewpoints)
            {
                // We're not doing anything with the bitmaps
                var bcfViewpoint = new BcfViewpoint();
                bcfTopic.Viewpoints.Add(bcfViewpoint);

                if (Guid.TryParse(viewpoint.Guid, out var viewpointId))
                {
                    bcfViewpoint.Id = viewpointId;
                }

                if (viewpoint.Components != null)
                {
                    if (viewpoint.Components.Coloring != null)
                    {
                        foreach (var coloring in viewpoint.Components.Coloring)
                        {
                            bcfViewpoint.ViewpointComponents.Coloring.Add(new BcfViewpointComponentColoring
                            {
                                Color = coloring.Color,
                                Components = coloring.Components.Select(c => new BcfViewpointComponent
                                {
                                    AuthoringToolId = c.AuthoringToolId,
                                    IfcGuid = c.IfcGuid,
                                    OriginatingSystem = c.OriginatingSystem
                                }).ToList()
                            });
                        }
                    }

                    if (viewpoint.Components.Selection != null)
                    {
                        foreach (var selection in viewpoint.Components.Selection)
                        {
                            bcfViewpoint.ViewpointComponents.SelectedComponents.Add(new BcfViewpointComponent
                            {
                                AuthoringToolId = selection.AuthoringToolId,
                                IfcGuid = selection.IfcGuid,
                                OriginatingSystem = selection.OriginatingSystem
                            });
                        }
                    }

                    if (viewpoint.Components.Visibility != null)
                    {
                        bcfViewpoint.ViewpointComponents.Visibility = new BcfViewpointComponentVisibility
                        {
                            DefaultVisibility = viewpoint.Components.Visibility.DefaultVisibility,
                            Exceptions = viewpoint.Components.Visibility.Exceptions.Select(e => new BcfViewpointComponent
                            {
                                AuthoringToolId = e.AuthoringToolId,
                                IfcGuid = e.IfcGuid,
                                OriginatingSystem = e.OriginatingSystem
                            }).ToList()
                        };
                    }
                }

                if (viewpoint.Item != null)
                {
                    switch (viewpoint.Item)
                    {
                        case Dangl.BCF.BCFv3.Schemas.OrthogonalCamera orthogonalCamera:
                            bcfViewpoint.OrthogonalCamera = new BcfViewpointOrthogonalCamera
                            {
                                AspectRatio = orthogonalCamera.AspectRatio,
                                ViewToWorldScale = orthogonalCamera.ViewToWorldScale,
                                UpVector = new BcfViewpointVector
                                {
                                    X = orthogonalCamera.CameraUpVector.X,
                                    Y = orthogonalCamera.CameraUpVector.Y,
                                    Z = orthogonalCamera.CameraUpVector.Z
                                },
                                ViewPoint = new BcfViewpointPoint
                                {
                                    X = orthogonalCamera.CameraViewPoint.X,
                                    Y = orthogonalCamera.CameraViewPoint.Y,
                                    Z = orthogonalCamera.CameraViewPoint.Z
                                },
                                Direction = new BcfViewpointVector
                                {
                                    X = orthogonalCamera.CameraDirection.X,
                                    Y = orthogonalCamera.CameraDirection.Y,
                                    Z = orthogonalCamera.CameraDirection.Z
                                }
                            };
                            break;
                        case Dangl.BCF.BCFv3.Schemas.PerspectiveCamera perspectiveCamera:
                            bcfViewpoint.PerspectiveCamera = new BcfViewpointPerspectiveCamera
                            {
                                AspectRatio = perspectiveCamera.AspectRatio,
                                FieldOfView = perspectiveCamera.FieldOfView,
                                UpVector = new BcfViewpointVector
                                {
                                    X = perspectiveCamera.CameraUpVector.X,
                                    Y = perspectiveCamera.CameraUpVector.Y,
                                    Z = perspectiveCamera.CameraUpVector.Z
                                },
                                ViewPoint = new BcfViewpointPoint
                                {
                                    X = perspectiveCamera.CameraViewPoint.X,
                                    Y = perspectiveCamera.CameraViewPoint.Y,
                                    Z = perspectiveCamera.CameraViewPoint.Z
                                },
                                Direction = new BcfViewpointVector
                                {
                                    X = perspectiveCamera.CameraDirection.X,
                                    Y = perspectiveCamera.CameraDirection.Y,
                                    Z = perspectiveCamera.CameraDirection.Z
                                }
                            };
                            break;
                        default:
                            throw new NotSupportedException($"Invalid camery type encountered: {viewpoint.Item.GetType()}");
                    }
                }

                if (viewpoint.Lines != null)
                {
                    foreach (var line in viewpoint.Lines)
                    {
                        var bcfViewpointLine = new BcfViewpointLine
                        {
                            StartPoint = new BcfViewpointPoint
                            {
                                X = line.StartPoint.X,
                                Y = line.StartPoint.Y,
                                Z = line.StartPoint.Z
                            },
                            EndPoint = new BcfViewpointPoint
                            {
                                X = line.EndPoint.X,
                                Y = line.EndPoint.Y,
                                Z = line.EndPoint.Z
                            }
                        };
                        bcfViewpoint.Lines.Add(bcfViewpointLine);

                    }
                }

                if (viewpoint.ClippingPlanes != null)
                {
                    foreach (var clippingPlane in viewpoint.ClippingPlanes)
                    {
                        var bcfViewpointClippingPlane = new BcfViewpointClippingPlane
                        {
                            Location = new BcfViewpointPoint
                            {
                                X = clippingPlane.Location.X,
                                Y = clippingPlane.Location.Y,
                                Z = clippingPlane.Location.Z
                            },
                            Direction = new BcfViewpointVector
                            {
                                X = clippingPlane.Direction.X,
                                Y = clippingPlane.Direction.Y,
                                Z = clippingPlane.Direction.Z
                            }
                        };
                        bcfViewpoint.ClippingPlanes.Add(bcfViewpointClippingPlane);

                    }
                }

                if (topic.ViewpointSnapshots?.TryGetValue(viewpoint.Guid, out var snapshotBytes) ?? false)
                {
                    bcfViewpoint.SnapshotBase64 = Convert.ToBase64String(snapshotBytes);
                }
            }

        }

    }
}
