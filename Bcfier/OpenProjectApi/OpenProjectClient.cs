using Bcfier.OpenProjectApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bcfier.OpenProjectApi
{
  public class OpenProjectClient
  {
    public const string DEFAULT_BASE_URL = "https://community.openproject.com/api/v3";

    private readonly string _openProjectAccessToken;
    private readonly string _baseUrl = DEFAULT_BASE_URL;

    public OpenProjectClient(string openProjectAccessToken,
      string customBaseUrl = null)
    {
      _openProjectAccessToken = openProjectAccessToken;
      if (customBaseUrl != null)
      {
        _baseUrl = customBaseUrl.TrimEnd('/');
      }
    }

    public Task<ResponseWrapper<List<Project>>> GetAllProjects()
    {
      var client = HttpClientFactory.GetHttpClient(_openProjectAccessToken);
      var url = $"{_baseUrl}/projects";
      return client.GetJsonAsync<List<Project>>(url);
    }
  }
}
