namespace LINGYUN.Abp.OperationRateLimiting;

public static class AbpOperationRateLimitingErrorCodes
{
    public const string Namespace = "OperationRateLimiting";
    /// <summary>
    /// A regular operation rate limit has been exceeded.
    /// </summary>
    public const string ExceedLimit = Namespace + ":010001";

    /// <summary>
    /// A permanent ban policy (<c>maxCount: 0</c>) denied the request.
    /// </summary>
    public const string ExceedLimitPermanently = Namespace + ":010002";
}
