using LINGYUN.Abp.Sonatype.Nexus.Assets;
using LINGYUN.Abp.Sonatype.Nexus.Components;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Sonatype.Nexus.Search;

public interface INexusLookupService
{
    Task<NexusComponentListResult> ListComponentAsync(NexusSearchArgs args, CancellationToken cancellationToken = default);

    Task<NexusAssetListResult> ListAssetAsync(NexusSearchArgs args, CancellationToken cancellationToken = default);
}
