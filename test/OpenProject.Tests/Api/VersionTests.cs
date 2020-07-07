using Xunit;

namespace OpenProject.Tests.Api
{
  public class VersionTests
  {
    [Theory]
    [InlineData("1.0.0", "0.1.0", true)]
    [InlineData("1.0.0", "1.1.0", false)]
    [InlineData("2.2.19", "2.2.2", true)]
    [InlineData("v1.0.0", "v0.1.0", true)]
    [InlineData("v1.0.0", "v1.1.0", false)]
    [InlineData("v2.2.19", "v2.2.2", true)]
    [InlineData("1.0.0", "v0.1.0", true)]
    [InlineData("1.0.0", "v1.1.0", false)]
    [InlineData("2.2.19", "v2.2.2", true)]
    [InlineData("v1.0.0", "0.1.0", true)]
    [InlineData("v1.0.0", "1.1.0", false)]
    [InlineData("v2.2.19", "2.2.2", true)]
    [InlineData("v.2.19", ".2.2", true)]
    public void CompareVersions(string source, string dest, bool isHigher)
    {
      var sourceVersion = new OpenProject.Api.Version(source);
      var destVersion = new OpenProject.Api.Version(dest);

      var actual = sourceVersion.CompareTo(destVersion) > 0;
      Assert.Equal(isHigher, actual);
    }
  }
}
