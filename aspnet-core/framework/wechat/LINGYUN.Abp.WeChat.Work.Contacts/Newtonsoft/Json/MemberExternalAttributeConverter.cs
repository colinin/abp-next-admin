using LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json;

internal class MemberExternalAttributeConverter : JsonConverter<MemberExternalAttribute>
{
    public override MemberExternalAttribute? ReadJson(JsonReader reader, Type objectType, MemberExternalAttribute? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        var memberExternalAttribute = new MemberExternalAttribute();

        if (jObject.TryGetValue("attrs", out var externalAttrsToken) && externalAttrsToken.Type == JTokenType.Array)
        {
            var attrs = new List<MemberAttribute>();
            foreach (var externalAttrToken in externalAttrsToken)
            {
                var typeToken = externalAttrToken.SelectToken("type");
                if (typeToken != null)
                {
                    var attributeType = typeToken.Value<int>();
                    if (Enum.IsDefined(typeof(AttributeType), attributeType))
                    {
                        attrs.Add(MemberAttributeDeserializeFactory.CreateExternalAttribute((AttributeType)attributeType, externalAttrToken));
                    }
                }
            }
            memberExternalAttribute.Attributes = attrs.ToArray();
        }

        return memberExternalAttribute;
    }

    public override void WriteJson(JsonWriter writer, MemberExternalAttribute? value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        if (value != null)
        {
            writer.WritePropertyName("external_corp_name");
            serializer.Serialize(writer, value.Attributes);
        }

        writer.WriteEndObject();
    }
}
