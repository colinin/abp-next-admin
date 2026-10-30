using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore.PostgreSql;

internal class JsonValueConverter<TPropertyType> : ValueConverter<TPropertyType, string>
{
    public JsonValueConverter()
        : base(
            d => SerializeObject(d),
            s => DeserializeObject(s))
    {

    }

    public readonly static JsonSerializerOptions SerializeOptions = new JsonSerializerOptions();

    private static string SerializeObject(TPropertyType d)
    {
        return JsonSerializer.Serialize(d, SerializeOptions);
    }

    public readonly static JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions()
    {
        Converters =
        {
            new ObjectToInferredTypesConverter()
        }
    };

    private static TPropertyType DeserializeObject(string s)
    {
        return JsonSerializer.Deserialize<TPropertyType>(s, DeserializeOptions)!;
    }
}
