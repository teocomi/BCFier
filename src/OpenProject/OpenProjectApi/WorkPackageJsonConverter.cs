using OpenProject.OpenProjectApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace OpenProject.OpenProjectApi
{
  public class WorkPackageJsonConverter : JsonConverter
  {
    // The _isReading variable is thread static, because when reading Json,
    // the following sequence happens:
    // 1. This converter will be called for types it can convert
    // 2. The _isReading variable is set to true
    // 3. This converter calls the regular Json conversion for the element internally,
    //    and because it now reports it can not read the element, the default converter
    //    is used to prevent stack overflows / self referencing loops
    // 4. This converter removes some unsupported elements
    // 5. The conversion is finished and _isReading is again set to false
    // This works because Newtonsoft.Json serialization does neither use async
    // nor multiple threads, so a single [ThreadStatic] field ensures that only one
    // single instance of this converter is running in parallel
    // Multiple converters can run in separate threads, e.g. in ASP.NET Core web applications
    // that handle multiple, simultaneous requests
    [ThreadStatic]
    private static bool _isReading;

    public override bool CanRead => !_isReading;

    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(WorkPackage);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      _isReading = true;
      try
      {
        var jsonObject = JObject.Load(reader);
        var workPackage = new WorkPackage();

        foreach (var linkElement in jsonObject["_links"].ToList())
        {
          // We're only supporting objects in the '_links' node, no arrays
          if (linkElement is JProperty jProp && !(jProp.Value is JObject))
          {
            linkElement.Remove();
          }
        }

        serializer.Populate(jsonObject.CreateReader(), workPackage);
        return workPackage;
      }
      finally
      {
        _isReading = false;
      }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
