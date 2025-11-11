using LINGYUN.Abp.WeChat.Work.ExternalContact.Models;
using System.Collections.Generic;

namespace System.Text.Json.Serialization;
internal class ExternalProfileSystemTextJsonConverter : JsonConverter<ExternalProfile>
{
    public override ExternalProfile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var externalProfile = new ExternalProfile
        {
            WechatChannels = new List<WechatChannel>(),
            ExternalAttributes = new List<ExternalAttribute>()
        };

        if (root.TryGetProperty("external_corp_name", out var corpNameElement))
        {
            externalProfile.ExternalCorpName = corpNameElement.GetRawText();
        }
        if (root.TryGetProperty("wechat_channels", out var wechatChannelsElement) && wechatChannelsElement.ValueKind == JsonValueKind.Array)
        {
            externalProfile.WechatChannels = JsonSerializer.Deserialize<List<WechatChannel>>(wechatChannelsElement.GetRawText(), options)!;
        }
        if (root.TryGetProperty("external_attr", out var externalAttrsElement) && externalAttrsElement.ValueKind == JsonValueKind.Array)
        {
            foreach ( var externalAttrElement in externalAttrsElement.EnumerateArray())
            {
                if (externalAttrElement.TryGetProperty("type", out var typeElement) && typeElement.ValueKind != JsonValueKind.Null)
                {
                    var type = typeElement.Deserialize<ExternalAttributeType>();

                    externalProfile.ExternalAttributes.Add(
                        ExternalAttributeDeserializeFactory.CreateExternalAttribute(type, externalAttrElement));
                }
            }
        }

        return externalProfile;
    }

    public override void Write(Utf8JsonWriter writer, ExternalProfile value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("external_corp_name");
        JsonSerializer.Serialize(writer, value.ExternalCorpName, options);

        if (value.WechatChannels != null)
        {
            writer.WritePropertyName("wechat_channels");
            JsonSerializer.Serialize(writer, value.WechatChannels, value.WechatChannels.GetType(), options);
        }
        if (value.ExternalAttributes != null)
        {
            writer.WritePropertyName("external_attr");
            JsonSerializer.Serialize(writer, value.ExternalAttributes, value.ExternalAttributes.GetType(), options);
        }

        writer.WriteEndObject();
    }
}
