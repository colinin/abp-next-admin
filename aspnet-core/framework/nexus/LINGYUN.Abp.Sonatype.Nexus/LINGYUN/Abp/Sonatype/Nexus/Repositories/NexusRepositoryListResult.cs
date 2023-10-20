using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Repositories;

[Serializable]
public class NexusRepositoryListResult
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("attributes")]
    public Dictionary<string, object> Attributes { get; set; }

    public NexusRepositoryListResult()
    {
        Attributes = new Dictionary<string, object>();
    }
}
