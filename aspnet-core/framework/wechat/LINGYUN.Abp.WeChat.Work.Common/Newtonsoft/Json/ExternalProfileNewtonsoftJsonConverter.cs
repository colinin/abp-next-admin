
using LINGYUN.Abp.WeChat.Work.Common.Members.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json;
internal class ExternalProfileNewtonsoftJsonConverter : JsonConverter<ExternalProfile>
{
    public override bool CanWrite => true;

    public override void WriteJson(JsonWriter writer, ExternalProfile value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        if (value?.ExternalCorpName != null)
        {
            writer.WritePropertyName("external_corp_name");
            serializer.Serialize(writer, value.ExternalCorpName);
        }

        if (value?.WechatChannels != null)
        {
            writer.WritePropertyName("wechat_channels");
            serializer.Serialize(writer, value.WechatChannels, value.WechatChannels.GetType());
        }

        if (value?.ExternalAttributes != null)
        {
            writer.WritePropertyName("external_attr");
            serializer.Serialize(writer, value.ExternalAttributes, value.ExternalAttributes.GetType());
        }

        writer.WriteEndObject();
    }

    public override ExternalProfile ReadJson(JsonReader reader, Type objectType, ExternalProfile existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        var externalProfile = new ExternalProfile
        {
            WechatChannels = new List<WechatChannel>(),
            ExternalAttributes = new List<ExternalAttribute>()
        };

        if (jObject.TryGetValue("external_corp_name", out var externalCorpNameToken))
        {
            externalProfile.ExternalCorpName = externalCorpNameToken.ToString();
        }
        if (jObject.TryGetValue("wechat_channels", out var wechatChannelsToken) && wechatChannelsToken.Type == JTokenType.Array)
        {
            externalProfile.WechatChannels = wechatChannelsToken.ToObject<List<WechatChannel>>(serializer)!;
        }
        if (jObject.TryGetValue("external_attr", out var externalAttrsToken) && externalAttrsToken.Type == JTokenType.Array)
        {
            foreach ( var externalAttrToken in externalAttrsToken)
            {
                var typeToken = externalAttrToken.SelectToken("type");
                if (typeToken != null)
                {
                    var attributeType = typeToken.Value<int>();
                    if (Enum.IsDefined(typeof(ExternalAttributeType), attributeType))
                    {
                        externalProfile.ExternalAttributes.Add(
                            ExternalAttributeDeserializeFactory.CreateExternalAttribute((ExternalAttributeType)attributeType, externalAttrToken));
                    }
                }
            }
        }

        return externalProfile;
    }
}
