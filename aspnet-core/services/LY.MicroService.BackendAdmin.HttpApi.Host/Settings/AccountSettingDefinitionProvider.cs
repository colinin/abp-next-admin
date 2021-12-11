using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LY.MicroService.BackendAdmin.Settings;

// 后台管理项目不引用 Volo.Abp.Account.Application模块,只能自建一个配置
public class AccountSettingDefinitionProvider : SettingDefinitionProvider
{
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
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
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
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }
}