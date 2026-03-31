using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Mvc;

public class SseResult
{
    [JsonPropertyName("m")]
    public string Message { get; }
    public SseResult(string message)
    {
        Message = message;
    }
}
