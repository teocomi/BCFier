using Bcfier.Bcf;
using System;
using System.IO;
using Xunit;

namespace Bcfier.Tests.Bcf
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
          foreach (var bcfIssue in actual.BcfIssues)
          {
            // TODO TEST MODIFIED DATE
            Assert.False(bcfIssue.IsModified);
          }
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
          foreach (var bcfIssue in actual.BcfIssues)
          {
            // TODO TEST MODIFIED DATE
            Assert.False(bcfIssue.IsModified);
          }
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
