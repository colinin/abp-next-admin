using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LY.MicroService.BackendAdmin.Settings;

// 后台管理项目不引用 Volo.Abp.Account.Application模块,只能自建一个配置
public class AccountSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "Identity";
    private const int GroupOrder = 10;
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                AccountSettingNames.IsSelfRegistrationEnabled,
                "true",
                L("DisplayName:Abp.Account.IsSelfRegistrationEnabled"),
                L("Description:Abp.Account.IsSelfRegistrationEnabled"),
                isVisibleToClients: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("Account", L("Settings:Identity.Account"), order: 0)
            .WithOrder(1)
            .WithValueType(ValueType.Boolean)
        );

        context.Add(
            new SettingDefinition(
                AccountSettingNames.EnableLocalLogin,
                "true",
                L("DisplayName:Abp.Account.EnableLocalLogin"),
                L("Description:Abp.Account.EnableLocalLogin"),
                isVisibleToClients: true)
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            .WithParent("Account", L("Settings:Identity.Account"), order: 0)
            .WithOrder(0)
            .WithValueType(ValueType.Boolean)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }
}