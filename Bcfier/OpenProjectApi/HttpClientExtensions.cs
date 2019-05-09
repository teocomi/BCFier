using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bcfier.OpenProjectApi
{
  // Copied from
  // https://github.com/GeorgDangl/Dangl.RestClient/blob/develop/src/Dangl.RestClient/HttpClientExtensions.cs
  // Since the Dangl.RestClient package does not yet have a target for net45

  /// <summary>
  /// Extensions that help with HttpClient operations
  /// </summary>
  public static class HttpClientExtensions
  {
    /// <summary>
    /// Sends a post request
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TPayload"></typeparam>
    /// <param name="client"></param>
    /// <param name="url"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    public static async Task<ResponseWrapper<TResponse>> PostJsonAsync<TResponse, TPayload>(this HttpClient client, string url, TPayload payload)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, url)
      {
        Content = GetJsonContent(payload)
      };
      var response = await client.SendAsync(request);
      return await ResponseWrapper<TResponse>.GetResponseWrapperAsync(response);
    }

    /// <summary>
    /// Sends a put request
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TPayload"></typeparam>
    /// <param name="client"></param>
    /// <param name="url"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    public static async Task<ResponseWrapper<TResponse>> PutJsonAsync<TResponse, TPayload>(this HttpClient client, string url, TPayload payload)
    {
      var request = new HttpRequestMessage(HttpMethod.Put, url)
      {
        Content = GetJsonContent(payload)
      };
      var response = await client.SendAsync(request);
      return await ResponseWrapper<TResponse>.GetResponseWrapperAsync(response);
    }

    /// <summary>
    /// Sends a get request
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="client"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task<ResponseWrapper<TResponse>> GetJsonAsync<TResponse>(this HttpClient client, string url)
    {
      var request = new HttpRequestMessage(HttpMethod.Get, url);
      var response = await client.SendAsync(request);
      return await ResponseWrapper<TResponse>.GetResponseWrapperAsync(response);
    }

    private static HttpContent GetJsonContent<T>(T payload)
    {
      var serializedContent = JsonConvert.SerializeObject(payload, JsonSerializationDefaults.JsonSerializerSettings);
      return new StringContent(serializedContent, Encoding.UTF8, "application/json");
    }
  }
}