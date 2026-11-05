using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Lease returned by <see cref="IOperationRateLimitingChecker.AcquireAsync"/>.
/// </summary>
public interface IOperationRateLimitingLease : IAsyncDisposable
{
    Task ReleaseAsync(CancellationToken cancellationToken = default);
}
