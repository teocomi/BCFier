using System;

namespace Bcfier.OpenProjectApi.Models
{
  public class Project
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public WorkPackageDescription Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
  }
}
