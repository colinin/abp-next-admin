using LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
using Newtonsoft.Json.Linq;
using System;

namespace Newtonsoft.Json;
internal class ControlNewtonsoftJsonConverter : JsonConverter<Control>
{
    public override bool CanWrite => true;

    public override void WriteJson(JsonWriter writer, Control? value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        if (value?.Property != null)
        {
            writer.WritePropertyName("property");
            serializer.Serialize(writer, value.Property);
        }

        if (value?.Config != null)
        {
            writer.WritePropertyName("config");
            serializer.Serialize(writer, value.Config, value.Config.GetType());
        }

        writer.WriteEndObject();
    }

    public override Control? ReadJson(JsonReader reader, Type objectType, Control existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        var control = new Control();

        if (jObject.TryGetValue("property", out var propertyToken))
        {
            control.Property = propertyToken.ToObject<ControlInfo>(serializer)!;
        }
        // 根据 Control 类型动态反序列化 Config
        if (jObject.TryGetValue("config", out var configToken) && configToken.Type != JTokenType.Null)
        {
            control.Config = ControlConfigFactory.CreateConfig(control.Property.Control, configToken);
        }

        return control;
    }
}
