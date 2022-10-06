using LINGYUN.Abp.OpenIddict.Sms;

namespace Microsoft.Extensions.DependencyInjection;

public static class SmsOpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AllowSmsFlow(this OpenIddictServerBuilder builder)
    {
        return builder.AllowCustomFlow(SmsTokenExtensionGrantConsts.GrantType);
    }
}
