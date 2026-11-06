namespace LINGYUN.Abp.OperationRateLimiting;

public static class AbpOperationRateLimitingErrorCodes
{
    /// <summary>
    /// A regular operation rate limit has been exceeded.
    /// </summary>
    public const string ExceedLimit = "Volo.Abp.OperationRateLimiting:010001";

    /// <summary>
    /// A permanent ban policy (<c>maxCount: 0</c>) denied the request.
    /// </summary>
    public const string ExceedLimitPermanently = "Volo.Abp.OperationRateLimiting:010002";
}
