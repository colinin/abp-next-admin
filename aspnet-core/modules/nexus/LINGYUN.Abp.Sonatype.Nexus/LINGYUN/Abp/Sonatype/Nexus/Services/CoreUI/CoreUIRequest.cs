using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI;

[Serializable]
public class CoreUIRequest<TData>
{
    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("tid")]
    public long Tid { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("data")]
    public TData Data { get; set; }

    public CoreUIRequest(
        string action,
        string method,
        TData data,
        string type = "rpc")
    {
        Action = action;
        Method = method;
        Type = type;
        Data = data;
    }
}
