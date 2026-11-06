using System;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;

public class SlidingWindowRateLimitingState
{
    public long[] SegmentStartUnixMs { get; set; } = Array.Empty<long>();

    public long[] SegmentCounts { get; set; } = Array.Empty<long>();
}
