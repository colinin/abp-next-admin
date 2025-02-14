namespace LINGYUN.Abp.OpenIddict.Applications;
public static class OpenIddictApplicationTokenLifetimeConsts
{
    public static int MinAccessTokenValue { get; set; } = 300;
    public static int MaxAccessTokenValue { get; set; } = int.MaxValue;

    public static int MinAuthorizationCodeValue { get; set; } = 300;
    public static int MaxAuthorizationCodeValue { get; set; } = int.MaxValue;

    public static int MinDeviceCodeValue { get; set; } = 300;
    public static int MaxDeviceCodeValue { get; set; } = int.MaxValue;

    public static int MinIdentityTokenValue { get; set; } = 300;
    public static int MaxIdentityTokenValue { get; set; } = int.MaxValue;

    public static int MinRefreshTokenValue { get; set; } = 300;
    public static int MaxRefreshTokenValue { get; set; } = int.MaxValue;

    public static int MinUserCodeValue { get; set; } = 300;
    public static int MaxUserCodeValue { get; set; } = int.MaxValue;
}
