namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Provides the client IP address used by
/// <see cref="OperationRateLimitingPartitionType.ClientIp"/>.
/// </summary>
/// <remarks>
/// The default implementation (used outside of an ASP.NET Core application) returns null,
/// which makes the check fail with a clear message. The
/// <c>LINGYUN.Abp.OperationRateLimiting.AspNetCore</c> package replaces it with an
/// implementation based on <c>IWebClientInfoProvider</c>.
/// </remarks>
public interface IOperationRateLimitingClientInfoProvider
{
    string? GetClientIpAddress();
}
