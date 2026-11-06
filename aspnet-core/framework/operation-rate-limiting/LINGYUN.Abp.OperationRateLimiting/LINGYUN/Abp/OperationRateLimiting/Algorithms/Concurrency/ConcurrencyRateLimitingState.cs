using System.Collections.Generic;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;

public class ConcurrencyRateLimitingState
{
    public List<long> LeaseStartUnixMs { get; set; } = new();
}
