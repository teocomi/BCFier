using OpenProject.Bcf;
using System.Linq;
using Xunit;

namespace OpenProject.Tests.ViewModels.Bcf
{
  public class BcfMarkupViewModelTests
  {
    [Fact]
    public void SetsIsModifiedWhenDataIsChanged()
    {
      using (var bcfStream = TestFilesFactory.GetTestFileStream(TestFile.V20_MaximumInformation))
      {
        var loader = new BcfFileLoader("file.bcfzip", bcfStream);
        var actual = loader.LoadFromBcf();

        Assert.False(actual.IsModified);
        actual.BcfIssuesFiltered.First().Markup.BcfTopic.Title = "Changed Title";
        Assert.True(actual.IsModified);
      }
    }
  }
}
