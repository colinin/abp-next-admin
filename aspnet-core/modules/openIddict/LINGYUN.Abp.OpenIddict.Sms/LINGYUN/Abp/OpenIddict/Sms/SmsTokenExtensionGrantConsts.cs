namespace LINGYUN.Abp.OpenIddict.Sms;

public static class SmsTokenExtensionGrantConsts
{
    public const string GrantType = "phone_verify";

    public const string ParamName = "phone_number";

    public const string TokenName = "phone_verify_code";

    public const string Purpose = "phone_verify";

    public const string SecurityCodeFailed = "SecurityCodeFailed";
}
