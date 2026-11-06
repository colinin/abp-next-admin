using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

internal sealed class OperationRateLimitingLease : IOperationRateLimitingLease
{
    private readonly IOperationRateLimitingChecker _checker;
    private readonly string _policyName;
    private readonly OperationRateLimitingContext _context;
    private int _released;

    public OperationRateLimitingLease(
        IOperationRateLimitingChecker checker,
        string policyName,
        OperationRateLimitingContext context)
    {
        _checker = checker;
        _policyName = policyName;
        _context = context;
    }

    public async Task ReleaseAsync(CancellationToken cancellationToken = default)
    {
        if (Interlocked.Exchange(ref _released, 1) == 1)
        {
            return;
        }

        await _checker.ReleaseAsync(_policyName, _context, cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(ReleaseAsync());
    }
}
