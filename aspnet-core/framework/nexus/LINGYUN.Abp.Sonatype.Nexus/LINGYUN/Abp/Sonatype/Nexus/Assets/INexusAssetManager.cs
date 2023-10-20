using JetBrains.Annotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Sonatype.Nexus.Assets;

public interface INexusAssetManager
{
    Task<NexusAssetListResult> ListAsync(
        [NotNull] string repository, 
        string continuationToken = null, 
        CancellationToken cancellationToken = default);

    Task<NexusAsset> GetAsync(
        [NotNull] string id,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        [NotNull] string id,
        CancellationToken cancellationToken = default);

    Task<Stream> GetContentOrNullAsync(
        [NotNull] NexusAsset asset,
        CancellationToken cancellationToken = default);
}
