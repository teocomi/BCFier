using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenProject.OpenProjectApi
{
  public class JsonSerializationDefaults
  {
    static JsonSerializationDefaults()
    {
      JsonSerializerSettings = new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      };

      JsonSerializer = new JsonSerializer
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      };
    }

    public static JsonSerializerSettings JsonSerializerSettings { get; private set; }

    public static JsonSerializer JsonSerializer { get; private set; }
  }
}
