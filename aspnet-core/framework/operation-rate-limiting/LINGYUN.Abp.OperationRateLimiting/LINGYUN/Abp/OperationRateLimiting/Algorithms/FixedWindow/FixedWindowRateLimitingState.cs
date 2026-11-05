namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;

public class FixedWindowRateLimitingState
{
    public long Count { get; set; }

    public long WindowStartUnixMs { get; set; }
}
