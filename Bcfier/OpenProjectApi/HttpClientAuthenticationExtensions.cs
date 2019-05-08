using System;
using System.Net.Http;
using System.Text;

namespace Bcfier.OpenProjectApi
{
  public static class HttpClientAuthenticationExtensions
  {
    public static void EnsureHasOpenProjectApiAuthenticationHeader(this HttpClient httpClient, string openProjectAccessToken)
    {
      if (httpClient == null)
      {
        throw new ArgumentNullException(nameof(httpClient));
      }

      if (string.IsNullOrWhiteSpace(openProjectAccessToken))
      {
        throw new ArgumentNullException(nameof(openProjectAccessToken));
      }

      var basicAuthHeaderValue = GetBasicAuthenticationHeaderValue(openProjectAccessToken);
      httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuthHeaderValue);
    }

    private static string GetBasicAuthenticationHeaderValue(string openProjectAccessToken)
    {
      var plainValue = $"apikey:{openProjectAccessToken}";
      var base64Value = Convert.ToBase64String(Encoding.ASCII.GetBytes(plainValue));
      return base64Value;
    }
  }
}
