using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Assets;

[Serializable]
public class NexusAssetListResult
{
    [JsonPropertyName("continuationToken")]
    public string ContinuationToken { get; set; }

    [JsonPropertyName("items")]
    public List<NexusAsset> Items { get; set; }
}
