namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Default client info provider used when no web implementation is registered.
/// </summary>
public class NullOperationRateLimitingClientInfoProvider : IOperationRateLimitingClientInfoProvider
{
    public static NullOperationRateLimitingClientInfoProvider Instance { get; } = new();

    public string? GetClientIpAddress()
    {
        return null;
    }
}
