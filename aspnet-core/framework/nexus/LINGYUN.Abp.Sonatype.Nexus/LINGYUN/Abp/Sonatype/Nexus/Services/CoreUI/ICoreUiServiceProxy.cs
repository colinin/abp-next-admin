using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI;

public interface ICoreUiServiceProxy : INexusServiceProxy
{
    Task<CoreUIResponse<TResult>> SearchAsync<TData, TResult>(CoreUIRequest<TData> request, CancellationToken cancellationToken = default);
}
