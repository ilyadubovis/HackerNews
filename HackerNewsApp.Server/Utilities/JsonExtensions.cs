using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using HackerNewsApp.Server.Controllers;

namespace HackerNewsApp.Server.Utilities;
public static class JsonExtensions
{
    private static JsonSerializerSettings SerializerSettings => new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore,
        Converters =
        {
            new StringEnumConverter()
        }
    };

    public static T? FromJson<T>(this string value)
    {
        return value.FromJson<T>(null);
    }

    public static T? FromJson<T>(this string value, JsonSerializerSettings? settings)
    {
        return JsonConvert.DeserializeObject<T>(value, settings ?? SerializerSettings);
    }
}
