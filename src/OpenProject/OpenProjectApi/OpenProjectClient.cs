using OpenProject.OpenProjectApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenProject.OpenProjectApi
{
  public class OpenProjectClient
  {
    public const string DEFAULT_BASE_URL = "https://community.openproject.com";

    private readonly string _openProjectAccessToken;
    private readonly string _baseUrl = DEFAULT_BASE_URL;
    private readonly string _apiV3BaseUrl = DEFAULT_BASE_URL + "/api/v3";
    private readonly string _bcfXmlApiV1BaseUrl = DEFAULT_BASE_URL + "/bcf_xml_api/v1";

    public OpenProjectClient(string openProjectAccessToken,
      string customBaseUrl = null)
    {
      _openProjectAccessToken = openProjectAccessToken;
      if (customBaseUrl != null)
      {
        _baseUrl = customBaseUrl.TrimEnd('/');
        _apiV3BaseUrl = _baseUrl + "/api/v3";
        _bcfXmlApiV1BaseUrl = _baseUrl + "/bcf_xml_api/v1";
      }
    }

    public Task<ResponseWrapper<List<Project>>> GetAllProjectsAsync()
    {
      var client = HttpClientFactory.GetHttpClient(_openProjectAccessToken);
      var url = $"{_apiV3BaseUrl}/projects?pageSize=50&offset=1";
      return client.GetJsonAsync<List<Project>>(url);
    }

    public Task<ResponseWrapper<List<WorkPackage>>> GetBcfWorkPackagesForProjectAsync(int projectId,
      int pageSize,
      int pageNumber,
      string userTextQuery = null)
    {
      var client = HttpClientFactory.GetHttpClient(_openProjectAccessToken);
      var url = $"{_apiV3BaseUrl}/projects/{projectId}/work_packages";
      var query = $"?pageSize={pageSize}&offset={pageNumber}";
      url += query;

      var textFilter = string.Empty;
      if (!string.IsNullOrWhiteSpace(userTextQuery))
      {
        var jsonFilter = Newtonsoft.Json.JsonConvert.SerializeObject(new
        {
          values = new[] { userTextQuery },
          @operator = "**"
        });

        textFilter = ",{\"search\":" + jsonFilter + "}";
      }

      var bcfTopicFilter = "&filters=[" +
        "{\"status\":{\"operator\":\"*\",\"values\":[]}}," +
        "{\"bcfIssueAssociated\":{\"operator\":\"=\",\"values\":[\"t\"]}}" +
         textFilter +
        "]&sortBy=[[\"createdAt\",\"desc\"]]";
      url += bcfTopicFilter;
      return client.GetJsonAsync<List<WorkPackage>>(url);
    }

    public async Task<ResponseWrapper> DownloadAllBcfWorkPackagesInProject(int projectId)
    {
      var client = HttpClientFactory.GetHttpClient(_openProjectAccessToken);
      var url = $"{_bcfXmlApiV1BaseUrl}/projects/{projectId}/bcf_xml";
      var query = "?filters=[{\"status\":{\"operator\":\"*\",\"values\":[]}},{\"bcfIssueAssociated\":{\"operator\":\"=\",\"values\":[\"t\"]}}]";
      url += query;
      var response = await client.GetAsync(url);
      var responseWrapper = await ResponseWrapper.GetResponseWrapperAsync(response);
      return responseWrapper;
    }

    public async Task<ResponseWrapper> GetBcfWorkPackageAsBcfXmlAsync(int projectId, int workPackageId)
    {
      var client = HttpClientFactory.GetHttpClient(_openProjectAccessToken);
      var url = $"{_bcfXmlApiV1BaseUrl}/projects/{projectId}/bcf_xml";
      var query = "?filters=[{\"status\":{\"operator\":\"*\",\"values\":[]}},{\"id\":{\"operator\":\"=\",\"values\":[" + workPackageId + "]}}]";
      url += query;
      var response = await client.GetAsync(url);
      var responseWrapper = await ResponseWrapper.GetResponseWrapperAsync(response);
      return responseWrapper;
    }

    public async Task<ResponseWrapper> UploadBcfXmlToOpenProjectAsync(int projectId, Stream bcfXmlV21)
    {
      var client = HttpClientFactory.GetHttpClient(_openProjectAccessToken);
      var url = $"{_bcfXmlApiV1BaseUrl}/projects/{projectId}/bcf_xml";

      var multiPartFormContent = new MultipartFormDataContent();
      var streamContent = new StreamContent(bcfXmlV21);
      streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
      var fileName = "bcf_xml_file";
      multiPartFormContent.Add(streamContent, fileName, fileName);

      var request = new HttpRequestMessage(HttpMethod.Post, url)
      {
        Content = multiPartFormContent
      };

      var response = await client.SendAsync(request);

      return await ResponseWrapper.GetResponseWrapperAsync(response);
    }
  }
}
