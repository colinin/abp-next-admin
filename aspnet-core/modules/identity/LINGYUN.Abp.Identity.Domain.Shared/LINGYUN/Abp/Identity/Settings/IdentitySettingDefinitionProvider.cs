using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Identity.Settings;

public class IdentitySettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "Identity";
    private const int GroupOrder = 10;

    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    public IdentitySettingDefinitionProvider(IStringLocalizerFactory stringLocalizerFactory)
    {
        StringLocalizerFactory = stringLocalizerFactory;
    }

    public override void Define(ISettingDefinitionContext context)
    {
        var stringLocalizer = StringLocalizerFactory.Create<IdentityResource>();

        context.Add(
            new SettingDefinition(
                name: IdentitySettingNames.User.SmsNewUserRegister,
                defaultValue: "",
                displayName: L("DisplayName:Abp.Identity.User.SmsNewUserRegister"),
                description: L("Description:Abp.Identity.User.SmsNewUserRegister"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("User", L("Settings:Identity.User"), order: 1)
            .WithOrder(0)
            .WithValueType(ValueType.String),
            new SettingDefinition(
                name: IdentitySettingNames.User.SmsUserSignin,
                defaultValue: "",
                displayName: L("DisplayName:Abp.Identity.User.SmsUserSignin"),
                description: L("Description:Abp.Identity.User.SmsUserSignin"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("User", L("Settings:Identity.User"), order: 1)
            .WithOrder(1)
            .WithValueType(ValueType.String),
            new SettingDefinition(
                name: IdentitySettingNames.User.SmsResetPassword,
                defaultValue: "",
                displayName: L("DisplayName:Abp.Identity.User.SmsResetPassword"),
                description: L("Description:Abp.Identity.User.SmsResetPassword"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("User", L("Settings:Identity.User"), order: 1)
            .WithOrder(2)
            .WithValueType(ValueType.String),
            new SettingDefinition(
                name: IdentitySettingNames.User.SmsPhoneNumberConfirmed,
                defaultValue: "",
                displayName: L("DisplayName:Abp.Identity.User.SmsPhoneNumberConfirmed"),
                description: L("Description:Abp.Identity.User.SmsPhoneNumberConfirmed"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("User", L("Settings:Identity.User"), order: 1)
            .WithOrder(3)
            .WithValueType(ValueType.String),
            new SettingDefinition(
                name: IdentitySettingNames.User.SmsRepetInterval,
                defaultValue: "5",
                displayName: L("DisplayName:Abp.Identity.User.SmsRepetInterval"),
                description: L("Description:Abp.Identity.User.SmsRepetInterval"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("User", L("Settings:Identity.User"), order: 1)
            .WithOrder(4)
            .WithValueType(ValueType.Number),
            new SettingDefinition(
                name: IdentitySettingNames.Session.ConcurrentLoginStrategy,
                defaultValue: ConcurrentLoginStrategy.None.ToString(),
                displayName: L("DisplayName:Abp.Identity.Session.ConcurrentLoginStrategy"),
                description: L("Description:Abp.Identity.Session.ConcurrentLoginStrategy"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("Session", L("Settings:Identity.Session"), order: 5)
            .WithOrder(0)
            .WithOptions([
                new NameValue<string>(stringLocalizer["ConcurrentLoginStrategy:None"].Value, ConcurrentLoginStrategy.None.ToString()),
                new NameValue<string>(stringLocalizer["ConcurrentLoginStrategy:LogoutFromSameTypeDevicesLimit"].Value, ConcurrentLoginStrategy.LogoutFromSameTypeDevicesLimit.ToString()),
                new NameValue<string>(stringLocalizer["ConcurrentLoginStrategy:LogoutFromSameTypeDevices"].Value, ConcurrentLoginStrategy.LogoutFromSameTypeDevices.ToString()),
                new NameValue<string>(stringLocalizer["ConcurrentLoginStrategy:LogoutFromAllDevices"].Value, ConcurrentLoginStrategy.LogoutFromAllDevices.ToString()),
            ]),
            new SettingDefinition(
                name: IdentitySettingNames.Session.LogoutFromSameTypeDevicesLimit,
                defaultValue: "1",
                displayName: L("DisplayName:Abp.Identity.Session.LogoutFromSameTypeDevicesLimit"),
                description: L("Description:Abp.Identity.Session.LogoutFromSameTypeDevicesLimit"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("Session", L("Settings:Identity.Session"), order: 5)
            .WithOrder(1)
            .WithValueType(ValueType.Number),

            new SettingDefinition(
                name: IdentitySettingNames.Link.UserLoginUri,
                defaultValue: "http://localhost:8080/Account/Login",
                displayName: L("DisplayName:Abp.Identity.Link.UserLoginUri"),
                description: L("Description:Abp.Identity.Link.UserLoginUri"),
                isVisibleToClients: false)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("Link", L("Settings:Identity.Link"), order: 7)
            .WithOrder(0)
            .WithValueType(ValueType.String)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
