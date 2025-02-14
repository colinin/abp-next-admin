using Volo.Abp.Validation;

namespace LINGYUN.Abp.OpenIddict.Applications;
public class OpenIddictApplicationTokenLifetimesDto
{
    /// <summary>
    /// 访问令牌生命周期(s)
    /// </summary>
    [DynamicRange(
        typeof(OpenIddictApplicationTokenLifetimeConsts),
        typeof(long),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MinAccessTokenValue),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MaxAccessTokenValue))]
    public long? AccessToken { get; set; }
    /// <summary>
    /// 授权码生命周期(s)
    /// </summary>
    [DynamicRange(
        typeof(OpenIddictApplicationTokenLifetimeConsts),
        typeof(long),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MinAuthorizationCodeValue),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MaxAuthorizationCodeValue))]
    public long? AuthorizationCode { get; set; }
    /// <summary>
    /// 设备代码生命周期(s)
    /// </summary>
    [DynamicRange(
        typeof(OpenIddictApplicationTokenLifetimeConsts),
        typeof(long),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MinDeviceCodeValue),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MaxDeviceCodeValue))]
    public long? DeviceCode { get; set; }
    /// <summary>
    /// 身份令牌生命周期(s)
    /// </summary>
    [DynamicRange(
        typeof(OpenIddictApplicationTokenLifetimeConsts),
        typeof(long),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MinIdentityTokenValue),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MaxIdentityTokenValue))]
    public long? IdentityToken { get; set; }
    /// <summary>
    /// 刷新令牌生命周期(s)
    /// </summary>
    [DynamicRange(
        typeof(OpenIddictApplicationTokenLifetimeConsts),
        typeof(long),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MinRefreshTokenValue),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MaxRefreshTokenValue))]
    public long? RefreshToken { get; set; }
    /// <summary>
    /// 用户代码生命周期(s)
    /// </summary>
    [DynamicRange(
        typeof(OpenIddictApplicationTokenLifetimeConsts),
        typeof(long),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MinUserCodeValue),
        nameof(OpenIddictApplicationTokenLifetimeConsts.MaxUserCodeValue))]
    public long? UserCode { get; set; }
}
