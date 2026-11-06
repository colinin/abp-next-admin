namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;

public class TokenBucketRateLimitingState
{
    public double Tokens { get; set; }

    public long LastRefillUnixMs { get; set; }
}
