using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Sonatype.Nexus.Components;

public interface INexusComponentManager
{
    Task<NexusComponentListResult> ListAsync(
       [NotNull] string repository,
       string continuationToken = null,
       CancellationToken cancellationToken = default);

    Task<NexusComponent> GetAsync(
        [NotNull] string id,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        [NotNull] string id,
        CancellationToken cancellationToken = default);

    Task UploadAsync(
        [NotNull] NexusComponentUploadArgs args,
        CancellationToken cancellationToken = default);
}
