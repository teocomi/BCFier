using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bcfier.OpenProjectApi
{
  // Copied from
  // https://github.com/GeorgDangl/Dangl.RestClient/blob/develop/src/Dangl.RestClient/ResponseWrapper.cs
  // Since the Dangl.RestClient package does not yet have a target for net45

  /// <summary>
  /// This container wraps Http responses
  /// </summary>
  public class ResponseWrapper
  {
    private ResponseWrapper(HttpResponseMessage response)
    {
      IsSuccess = response.IsSuccessStatusCode;
      Response = response;
    }

    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// The original response
    /// </summary>
    public HttpResponseMessage Response { get; }

    /// <summary>
    /// The error
    /// </summary>
    public string Error { get; private set; }

    /// <summary>
    /// This container wraps Http responses
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static async Task<ResponseWrapper> GetResponseWrapper(HttpResponseMessage response)
    {
      var wrapper = new ResponseWrapper(response);
      if (!response.IsSuccessStatusCode)
      {
        try
        {
          var responseContent = await response.Content.ReadAsStringAsync();
          wrapper.Error = responseContent;
        }
        catch
        {
          wrapper.Error = "Unknown Error";
        }
      }

      return wrapper;
    }
  }

  /// <summary>
  /// This container wraps Http responses
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class ResponseWrapper<T>
  {
    private ResponseWrapper(HttpResponseMessage response)
    {
      IsSuccess = response.IsSuccessStatusCode;
      Response = response;
    }

    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// The deserialized result
    /// </summary>
    public T Result { get; private set; }

    /// <summary>
    /// The original response
    /// </summary>
    public HttpResponseMessage Response { get; }

    /// <summary>
    /// The error
    /// </summary>
    public string Error { get; private set; }

    /// <summary>
    /// This container wraps Http responses
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static async Task<ResponseWrapper<T>> GetResponseWrapper(HttpResponseMessage response)
    {
      var wrapper = new ResponseWrapper<T>(response);
      if (response.IsSuccessStatusCode)
      {
        try
        {
          var responseContent = await response.Content.ReadAsStringAsync();
          var jObject = JObject.Parse(responseContent);
          // The OpenProject API uses a wrapper format for all responses.
          // See https://docs.openproject.org/apiv3-doc/#header-hal+json
          // The actual format used is HAL, see http://stateless.co/hal_specification.html
          if (jObject["_embedded"] != null
            && jObject["_type"] != null
            && jObject["_type"].ToString() == "Collection")
          {
            wrapper.Result = jObject["_embedded"]["elements"].ToObject<T>(JsonSerializationDefaults.JsonSerializer);
          }
          else if (jObject["_embedded"] != null)
          {
            wrapper.Result = jObject["_embedded"].ToObject<T>(JsonSerializationDefaults.JsonSerializer);
          }
          else
          {
            var deserializedResponse = JsonConvert.DeserializeObject<T>(responseContent, JsonSerializationDefaults.JsonSerializerSettings);
            wrapper.Result = deserializedResponse;
          }
        }
        catch
        {
          wrapper.Error = "Could not deserialize the server response";
          wrapper.IsSuccess = false;
        }
      }
      else
      {
        try
        {
          var responseContent = await response.Content.ReadAsStringAsync();
          wrapper.Error = string.IsNullOrWhiteSpace(responseContent)
            ? "Unknown Error"
            : responseContent;
        }
        catch
        {
          wrapper.Error = "Unknown Error";
        }
      }

      return wrapper;
    }
  }
}