using OpenProject.Bcf;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace OpenProject.Tests.Bcf
{
  public class BcfFileLoaderTests
  {
    [Fact]
    public void ArgumentNullExceptionForNullBcfFileName()
    {
      Assert.Throws<ArgumentNullException>("bcfFileName", () => new BcfFileLoader(null, new MemoryStream()));
    }

    [Fact]
    public void ArgumentNullExceptionForNullBcfFileStream()
    {
      Assert.Throws<ArgumentNullException>("bcfFileStream", () => new BcfFileLoader("file.bcf", null));
    }

    public class LoadFromBcf
    {
      [Fact]
      public void BcfFileLoadExceptionForNonBcfFile()
      {
        var loader = new BcfFileLoader("file.bcf", new MemoryStream());
        Assert.Throws<BcfLoaderException>(() => loader.LoadFromBcf());
      }

      [Fact]
      public void CanLoadBcfV20File()
      {
        using (var bcfStream = TestFilesFactory.GetTestFileStream(TestFile.V20_MaximumInformation))
        {
          var loader = new BcfFileLoader("file.bcfzip", bcfStream);
          var actual = loader.LoadFromBcf();
          Assert.NotNull(actual);
          Assert.False(actual.IsModified);
          Assert.NotEmpty(actual.BcfIssues);

          // Checking modified dates on the topics -> They should not be updated in import
          Assert.Equal(2, actual.BcfIssues.Count);
          Assert.False(actual.BcfIssues[0].IsModified);
          Assert.False(actual.BcfIssues[1].IsModified);
          Assert.Null(actual.BcfIssues[0].Markup.BcfTopic.ModifiedDate);
          Assert.Equal(new DateTime(2015,6,21,14,22,47, DateTimeKind.Utc), actual.BcfIssues[1].Markup.BcfTopic.ModifiedDate);

          // Checking modified dates on the comments -> They should not be updated in import
          var comments = actual.BcfIssues.SelectMany(bcfIssue => bcfIssue.Markup.Comments).ToList();
          Assert.Equal(4, comments.Count);
          Assert.Null(comments[0].ModifiedDate);
          Assert.Null(comments[1].ModifiedDate);
          Assert.Null(comments[2].ModifiedDate);
          Assert.Equal(new DateTime(2015,8,31,16,7,11, DateTimeKind.Utc), comments[3].ModifiedDate);
        }
      }

      [Fact]
      public void CanLoadBcfV21File()
      {
        using (var bcfStream = TestFilesFactory.GetTestFileStream(TestFile.V21_MaximumInformation))
        {
          var loader = new BcfFileLoader("file.bcf", bcfStream);
          var actual = loader.LoadFromBcf();
          Assert.NotNull(actual);
          Assert.False(actual.IsModified);
          Assert.NotEmpty(actual.BcfIssues);

          // Checking modified dates on the topics -> They should not be updated in import
          Assert.Equal(2, actual.BcfIssues.Count);
          Assert.False(actual.BcfIssues[0].IsModified);
          Assert.False(actual.BcfIssues[1].IsModified);
          Assert.Null(actual.BcfIssues[0].Markup.BcfTopic.ModifiedDate);
          Assert.Equal(new DateTime(2015, 6, 21, 14, 22, 47, DateTimeKind.Utc), actual.BcfIssues[1].Markup.BcfTopic.ModifiedDate);

          // Checking modified dates on the comments -> They should not be updated in import
          var comments = actual.BcfIssues.SelectMany(bcfIssue => bcfIssue.Markup.Comments).ToList();
          Assert.Equal(4, comments.Count);
          Assert.Null(comments[0].ModifiedDate);
          Assert.Null(comments[1].ModifiedDate);
          Assert.Null(comments[2].ModifiedDate);
          Assert.Equal(new DateTime(2015, 8, 31, 16, 7, 11, DateTimeKind.Utc), comments[3].ModifiedDate);
        }
      }

      [Fact]
      public void ThrowsWhenTryingToLoadBcfV21WithBcfV20FileExtension()
      {
        using (var bcfStream = TestFilesFactory.GetTestFileStream(TestFile.V21_MaximumInformation))
        {
          var loader = new BcfFileLoader("file.bcfzip", bcfStream);
          Assert.Throws<BcfLoaderException>(() => loader.LoadFromBcf());
        }
      }
    }
  }
}
