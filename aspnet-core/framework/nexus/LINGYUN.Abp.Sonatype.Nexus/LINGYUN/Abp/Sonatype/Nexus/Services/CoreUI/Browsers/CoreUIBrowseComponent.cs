using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI.Browsers;

[Serializable]
public class CoreUIBrowseComponent
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("assetId")]
    public string AssetId { get; set; }

    [JsonPropertyName("componentId")]
    public string ComponentId { get; set; }

    [JsonPropertyName("packageUrl")]
    public string PackageUrl { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("leaf")]
    public bool Leaf { get; set; }
}
