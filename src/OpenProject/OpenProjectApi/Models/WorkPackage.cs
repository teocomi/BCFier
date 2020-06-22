using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenProject.OpenProjectApi.Models
{
  // This class only partially represents what is available in OpenProject
  // See here for the class definition: https://docs.openproject.org/apiv3-doc/#work-packages
  [JsonConverter(typeof(WorkPackageJsonConverter))]
  public class WorkPackage
  {
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Type { get; set; }
    public WorkPackageDescription Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonProperty("_links")]
    public Dictionary<string, WorkPackageLink> Links { get; set; }
  }
}
