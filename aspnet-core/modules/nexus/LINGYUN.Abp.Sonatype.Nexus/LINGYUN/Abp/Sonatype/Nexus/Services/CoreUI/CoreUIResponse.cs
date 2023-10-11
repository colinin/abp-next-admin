using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI;

[Serializable]
public class CoreUIResponse<TResult>
{
    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("tid")]
    public long Tid { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("result")]
    public TResult Result { get; set; }
}
