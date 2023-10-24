using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI.Assets;

[Serializable]
public class CoreUIAssetData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("contentType")]
    public string ContentType { get; set; }

    [JsonPropertyName("blobUpdated")]
    public DateTime? BlobUpdated { get; set; }

    [JsonPropertyName("blobCreated")]
    public DateTime? BlobCreated { get; set; }

    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; }

    [JsonPropertyName("createdByIp")]
    public string CreatedByIp { get; set; }

    [JsonPropertyName("blobRef")]
    public string BlobRef { get; set; }

    [JsonPropertyName("componentId")]
    public string ComponentId { get; set; }

    [JsonPropertyName("lastDownloaded")]
    public string LastDownloaded { get; set; }

    [JsonPropertyName("containingRepositoryName")]
    public string ContainingRepositoryName { get; set; }

    [JsonPropertyName("repositoryName")]
    public string RepositoryName { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("attributes")]
    public Dictionary<string, Dictionary<string, object>> Attributes { get; set; }
}
