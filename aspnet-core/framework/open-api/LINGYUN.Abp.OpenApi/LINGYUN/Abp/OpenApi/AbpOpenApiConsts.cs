namespace LINGYUN.Abp.OpenApi;

public static class AbpOpenApiConsts
{
    public const string SecurityChecking = "_AbpOpenApiSecurityChecking";

    public const string HEADER_APP_KEY = "X-API-APPKEY";
    public const string HEADER_SIGNATURE = "X-API-SIGN";
    public const string HEADER_NONCE = "X-API-NONCE";
    public const string HEADER_TIMESTAMP = "X-API-TIMESTAMP";

    public const string KeyPrefix = "AbpOpenApi";
    /// <summary>
    /// 无效的应用标识 {AppKey}.
    /// </summary>
    public const string InvalidAccessWithAppKey = KeyPrefix + ":9100";
    /// <summary>
    /// 未携带应用标识(appKey).
    /// </summary>
    public const string InvalidAccessWithAppKeyNotFound = KeyPrefix + ":9101";

    /// <summary>
    /// 无效的签名 sign.
    /// </summary>
    public const string InvalidAccessWithSign = KeyPrefix + ":9110";
    /// <summary>
    /// 未携带签名(sign).
    /// </summary>
    public const string InvalidAccessWithSignNotFound = KeyPrefix + ":9111";

    /// <summary>
    /// 请求超时或会话已过期.
    /// </summary>
    public const string InvalidAccessWithTimestamp = KeyPrefix + ":9210";
    /// <summary>
    /// 未携带时间戳标识.
    /// </summary>
    public const string InvalidAccessWithTimestampNotFound = KeyPrefix + ":9211";

    /// <summary>
    /// 重复发起的请求.
    /// </summary>
    public const string InvalidAccessWithNonceRepeated = KeyPrefix + ":9220";
    /// <summary>
    /// 未携带随机数.
    /// </summary>
    public const string InvalidAccessWithNonceNotFound = KeyPrefix + ":9221";

    /// <summary>
    /// 客户端不在允许的范围内.
    /// </summary>
    public const string InvalidAccessWithClientId = KeyPrefix + ":9300";
    /// <summary>
    /// 客户端IP不在允许的范围内.
    /// </summary>
    public const string InvalidAccessWithIpAddress = KeyPrefix + ":9400";
}
