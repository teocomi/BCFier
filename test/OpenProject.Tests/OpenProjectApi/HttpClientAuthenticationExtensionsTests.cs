using OpenProject.OpenProjectApi;
using System;
using System.Net.Http;
using Xunit;

namespace OpenProject.Tests.OpenProjectApi
{
  public class HttpClientAuthenticationExtensionsTests
  {
    [Fact]
    public void ArgumentNullExceptionForNullClient()
    {
      Assert.Throws<ArgumentNullException>("httpClient", () => HttpClientAuthenticationExtensions.EnsureHasOpenProjectApiAuthenticationHeader(null, "Hello World!"));
    }

    [Fact]
    public void ArgumentNullExceptionForNullToken()
    {
      Assert.Throws<ArgumentNullException>("openProjectAccessToken", () => new HttpClient().EnsureHasOpenProjectApiAuthenticationHeader(null));
    }

    [Fact]
    public void ArgumentNullExceptionForEmptyToken()
    {
      Assert.Throws<ArgumentNullException>("openProjectAccessToken", () => new HttpClient().EnsureHasOpenProjectApiAuthenticationHeader(string.Empty));
    }

    [Fact]
    public void AppendsCorrectHeader()
    {
      var httpClient = new HttpClient();
      var accessToken = "Hello World!";
      Assert.Null(httpClient.DefaultRequestHeaders.Authorization);
      httpClient.EnsureHasOpenProjectApiAuthenticationHeader(accessToken);
      var expectedHeaderValue = "YXBpa2V5OkhlbGxvIFdvcmxkIQ=="; // The format is 'apikey:<access_token>'
      Assert.Equal(expectedHeaderValue, httpClient.DefaultRequestHeaders.Authorization.Parameter);
      Assert.Equal("Basic", httpClient.DefaultRequestHeaders.Authorization.Scheme);
    }
  }
}
