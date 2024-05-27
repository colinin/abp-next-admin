using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OpenApi;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class AlwaysAllowClientChecker : IClientChecker
{
    public Task<bool> IsGrantAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}
