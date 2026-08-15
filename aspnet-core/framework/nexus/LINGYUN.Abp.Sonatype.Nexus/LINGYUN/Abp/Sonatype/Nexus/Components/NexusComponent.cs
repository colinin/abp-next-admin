using LINGYUN.Abp.Sonatype.Nexus.Assets;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Components;

[Serializable]
public class NexusComponent
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("repository")]
    public string Repository { get; set; } = default!;

    [JsonPropertyName("format")]
    public string? Format { get; set; }

    [JsonPropertyName("group")]
    public string? Group { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("version")]
    public string Version { get; set; } = default!;

    [JsonPropertyName("assets")]
    public List<NexusAsset> Assets { get; set; }

    public NexusComponent()
    {
        Assets = new List<NexusAsset>();
    }
}
