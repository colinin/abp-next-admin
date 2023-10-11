using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Sonatype.Nexus.Repositories.Raw;

public class NexusRawRepository : NexusRepository
{
    [JsonPropertyName("storage")]
    public RawStorage Storage { get; set; }
}
