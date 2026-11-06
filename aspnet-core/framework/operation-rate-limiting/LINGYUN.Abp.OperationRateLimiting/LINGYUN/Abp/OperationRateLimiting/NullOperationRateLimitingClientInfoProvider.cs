using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Default client info provider used when no web implementation is registered.
/// </summary>
[Dependency(TryRegister = true)]
public class NullOperationRateLimitingClientInfoProvider : IOperationRateLimitingClientInfoProvider, ISingletonDependency
{
    public string? GetClientIpAddress()
    {
        return null;
    }
}
