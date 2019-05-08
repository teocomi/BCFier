using Bcfier.OpenProjectApi;
using System;
using Xunit;

namespace Bcfier.Tests.OpenProjectApi
{
  public class HttpClientFactoryTests
  {
    [Fact]
    public void ArgumentNullExceptionForNullAccessToken()
    {
      Assert.Throws<ArgumentNullException>("openProjectAccessToken", () => HttpClientFactory.GetHttpClient(null));
    }

    [Fact]
    public void ArgumentNullExceptionForEmptyAccessToken()
    {
      Assert.Throws<ArgumentNullException>("openProjectAccessToken", () => HttpClientFactory.GetHttpClient(string.Empty));
    }

    [Fact]
    public void ReturnsCachedClient()
    {
      var firstClient = HttpClientFactory.GetHttpClient("Hello World!");
      var secondClient = HttpClientFactory.GetHttpClient("Hello World!");
      Assert.True(ReferenceEquals(firstClient, secondClient));
    }

    [Fact]
    public void ReturnsDifferentClientForDifferentToken()
    {
      var firstClient = HttpClientFactory.GetHttpClient("Hello World!");
      var secondClient = HttpClientFactory.GetHttpClient("Hello beautiful World!");

      Assert.False(ReferenceEquals(firstClient, secondClient));

      Assert.Equal("YXBpa2V5OkhlbGxvIFdvcmxkIQ==", firstClient.DefaultRequestHeaders.Authorization.Parameter);
      Assert.Equal("YXBpa2V5OkhlbGxvIGJlYXV0aWZ1bCBXb3JsZCE=", secondClient.DefaultRequestHeaders.Authorization.Parameter);
    }
  }
}
