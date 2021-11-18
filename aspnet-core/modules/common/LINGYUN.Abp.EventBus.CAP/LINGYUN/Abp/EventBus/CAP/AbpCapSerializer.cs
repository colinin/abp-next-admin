using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;

namespace LINGYUN.Abp.EventBus.CAP
{
    public class AbpCapSerializer : ISerializer
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly AbpSystemTextJsonSerializerOptions _jsonSerializerOptions;

        public AbpCapSerializer(
            IJsonSerializer jsonSerializer, 
            IOptions<AbpSystemTextJsonSerializerOptions> options)
        {
            _jsonSerializer = jsonSerializer;
            _jsonSerializerOptions = options.Value;
        }

        public Task<TransportMessage> SerializeAsync(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.Value == null)
            {
                return Task.FromResult(new TransportMessage(message.Headers, null));
            }
            var messageStr = _jsonSerializer.Serialize(message.Value);
            var jsonBytes = Encoding.UTF8.GetBytes(messageStr);
            // var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(message.Value, _jsonSerializerOptions.JsonSerializerOptions);
            return Task.FromResult(new TransportMessage(message.Headers, jsonBytes));
        }

        public Task<Message> DeserializeAsync(TransportMessage transportMessage, Type valueType)
        {
            if (valueType == null || transportMessage.Body == null)
            {
                return Task.FromResult(new Message(transportMessage.Headers, null));
            }

            var messageBytes = Encoding.UTF8.GetString(transportMessage.Body);
            var obj = _jsonSerializer.Deserialize(valueType, messageBytes);
            // var obj = JsonSerializer.Deserialize(transportMessage.Body, valueType, _jsonSerializerOptions.JsonSerializerOptions);

            return Task.FromResult(new Message(transportMessage.Headers, obj));
        }

        public string Serialize(Message message)
        {
            return _jsonSerializer.Serialize(message);
            // return JsonSerializer.Serialize(message, _jsonSerializerOptions.JsonSerializerOptions);
        }

        public Message Deserialize(string json)
        {
            return _jsonSerializer.Deserialize<Message>(json);
            // return JsonSerializer.Deserialize<Message>(json, _jsonSerializerOptions.JsonSerializerOptions);
        }

        public object Deserialize(object value, Type valueType)
        {
            if (value is JsonElement jToken)
            {
                var bufferWriter = new ArrayBufferWriter<byte>();
                using (var writer = new Utf8JsonWriter(bufferWriter))
                {
                    jToken.WriteTo(writer);
                }
                return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, valueType, _jsonSerializerOptions.JsonSerializerOptions);
            }
            throw new NotSupportedException("Type is not of type JToken");
        }

        public bool IsJsonType(object jsonObject)
        {
            return jsonObject is JsonElement;
        }
    }
}
