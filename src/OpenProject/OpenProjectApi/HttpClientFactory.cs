using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OpenProject.OpenProjectApi
{
  public static class HttpClientFactory
  {
    private static readonly Dictionary<string, HttpClient> _cachedClientsByApiKey = new Dictionary<string, HttpClient>();
    private static readonly Dictionary<string, DateTime> _clientCreationDateByApiAccessToken = new Dictionary<string, DateTime>();

    private static readonly object _cacheUpdateLock = new object();
    private static readonly object _clientUpdateLock = new object();

    /// <summary>
    /// This method will internally cache the underlying http client and only return a new instance if the client is
    /// older than two minutes. Callers should ensure that the returned instance is not disposed. See here for more information:
    /// https://stackoverflow.com/questions/15705092/do-httpclient-and-httpclienthandler-have-to-be-disposed
    /// Clients for different access tokens are unique
    /// </summary>
    /// <param name="openProjectAccessToken"></param>
    /// <returns></returns>
    public static HttpClient GetHttpClient(string openProjectAccessToken)
    {
      if (string.IsNullOrWhiteSpace(openProjectAccessToken))
      {
        throw new ArgumentNullException(nameof(openProjectAccessToken));
      }

      if (ShouldReturnCachedClient(openProjectAccessToken))
      {
        return GetCachedClient(openProjectAccessToken);
      }

      UpdateClient(openProjectAccessToken);
      return GetCachedClient(openProjectAccessToken);
    }

    private static bool ShouldReturnCachedClient(string openProjectAccessToken)
    {
      lock (_cacheUpdateLock)
      {
        // We're locking here since there might be multithreaded access to the clients,
        // and we need two access to the dictionaries to decide if the client cache
        // is still valid
        return _cachedClientsByApiKey.ContainsKey(openProjectAccessToken)
          && _clientCreationDateByApiAccessToken[openProjectAccessToken] <= DateTime.UtcNow.AddMinutes(2);
      }
    }

    private static void UpdateClient(string openProjectAccessToken)
    {
      lock (_clientUpdateLock)
      {
        if (!ShouldReturnCachedClient(openProjectAccessToken))
        {
          var httpClient = new HttpClient();
          httpClient.EnsureHasOpenProjectApiAuthenticationHeader(openProjectAccessToken);
          lock (_cacheUpdateLock)
          {
            if (_cachedClientsByApiKey.ContainsKey(openProjectAccessToken))
            {
              _cachedClientsByApiKey[openProjectAccessToken] = httpClient;
              _clientCreationDateByApiAccessToken[openProjectAccessToken] = DateTime.UtcNow;
            }
            else
            {
              _cachedClientsByApiKey.Add(openProjectAccessToken, httpClient);
              _clientCreationDateByApiAccessToken.Add(openProjectAccessToken, DateTime.UtcNow);
            }
          }
        }
      }
    }

    private static HttpClient GetCachedClient(string openProjectAccessToken)
    {
      return _cachedClientsByApiKey[openProjectAccessToken];
    }
  }
}
