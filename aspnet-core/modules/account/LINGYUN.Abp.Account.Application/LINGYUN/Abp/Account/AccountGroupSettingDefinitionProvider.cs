using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using ValueType = Volo.Abp.Settings.ValueType;

namespace LINGYUN.Abp.Account;

public class AccountGroupSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "Identity";
    private const int GroupOrder = 10;
    public override void Define(ISettingDefinitionContext context)
    {
        context.GetOrNull(AccountSettingNames.EnableLocalLogin)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Account", L("Settings:Identity.Account"), order: 0)
            ?.WithOrder(0)
            ?.WithValueType(ValueType.Boolean);
        context.GetOrNull(AccountSettingNames.IsSelfRegistrationEnabled)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Account", L("Settings:Identity.Account"), order: 0)
            ?.WithOrder(1)
            ?.WithValueType(ValueType.Boolean);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }
}
